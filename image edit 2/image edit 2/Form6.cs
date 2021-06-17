using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Numerics;
using System.Globalization;

namespace image_edit_2

{
    public partial class Form1 : Form
    {
        private static Bitmap img = null;
        private Bitmap image = null;
        public Form1()
        {
            InitializeComponent();
        }

        #region open/save
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string filename_2 = ofd.FileName;


            if (ofd.ShowDialog() == DialogResult.OK)
            {

                image = new Bitmap(ofd.FileName);
                pictureBox1.Image = image;
                img = new Bitmap(image.Width, image.Height);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Файлы JPG и PNG (*.png;*.jpg;*.bmp;*.gif) | *.png;*.jpg;*.bmp;*.gif";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (img != null)
                    {
                        img = (Bitmap)pictureBox2.Image;
                        img.Save(sfd.FileName);
                    }
                }
            }
        }
        #endregion

        #region getwrite

        static byte clmp(double d)
        {
            if (d > 255)
                return 255;
            if (d < 0)
                return 0;
            return (byte)d;
        }
        static byte[] getImgBytes(Bitmap img)
        {
            byte[] bytes = new byte[img.Width * img.Height * 3];  //выделяем память под массив байтов

            var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),  //блокируем участок памати, занимаемый изображением
                ImageLockMode.ReadOnly,
                img.PixelFormat);
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);  //копируем байты изображения в массив
            img.UnlockBits(data);   //разблокируем изображение
            return bytes; //возвращаем байты
        }
        static void writeImageBytes(Bitmap img, byte[] bytes)
        {
            var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),  //блокируем участок памати, занимаемый изображением
                ImageLockMode.WriteOnly,
                img.PixelFormat);
            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length); //копируем байты массива в изображение

            img.UnlockBits(data);  //разблокируем изображение
        }



        #endregion

        #region funcs
        public static double F(double x)
        {
            return Math.Log(x + 1);
        }

        public static double Butter(double x, double y, double wx, double n, double dx = 0, double dy = 0, double G = 1.0, double h = 0)
        {
            double D = Math.Sqrt((x - dx) * (x - dx) + (y - dy) * (y - dy)) - h;
            return G / (1 + Math.Pow(D / wx, 2 * n));
        }

        public static double Gauss(double x, double y, double wx, double dx = 0, double dy = 0, double G = 1.0, double h = 0)
        {
            double D = Math.Sqrt((x - dx) * (x - dx) + (y - dy) * (y - dy)) - h;
            return G * Math.Exp(-(D * D / (2.0 * wx * wx)));
        }
        public void Furier(Bitmap input)
        {
            string filter = "0;0;0;10;";
            double in_filter_zone = 1.0;
            double out_filter_zone = 0.0;
            int width = input.Width;
            int height = input.Height;
            int filter_type = 0;
            int new_width = width;
            int new_height = height;
            double furier_multiplyer = 1.0;

            var p = Math.Log2(width);
            if (p != Math.Floor(p))
                new_width = (int)Math.Pow(2, Math.Ceiling(p));
            p = Math.Log2(height);
            if (p != Math.Floor(p))
                new_height = (int)Math.Pow(2, Math.Ceiling(p));

            using Bitmap _tmp = new Bitmap(new_width, new_height, PixelFormat.Format24bppRgb);
            _tmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);

            byte[] new_bytes = new byte[new_width * new_height * 3];
            byte[] furier_ma_bytes = new byte[new_width * new_height * 3];
            byte[] filter_bytes = new byte[new_width * new_height * 3];

            using Graphics g = Graphics.FromImage(_tmp);
            g.DrawImageUnscaled(input, 0, 0);

            byte[] old_bytes = getImgBytes(_tmp);


            var ss = StringSplitOptions.RemoveEmptyEntries;
            var filter_params_strings = filter.Split("\n", ss);
            filter_params_strings = (from s in filter_params_strings where (s.Trim() != string.Empty) select s).ToArray();
            var cult = new CultureInfo("en-US");
            var filter_params_double = filter_params_strings.Select(a => a.Split(";", ss)
                .Select(b => Convert.ToDouble(b.Trim(), cult)).ToArray()).ToArray();




            Complex[] complex_bytes = new Complex[new_width * new_height];

            for (int color = 0; color <= 2; color++)
            {

                for (int i = 0; i < new_width * new_height; ++i)
                {
                    int y = i / new_width;
                    int x = i - y * new_width;
                    complex_bytes[i] = Math.Pow(-1, x + y) * old_bytes[i * 3 + color];
                }


                complex_bytes = Fur.ditfft2d(complex_bytes, new_width, new_height);



                var max_ma = complex_bytes.Max(x => F(x.Imaginary));

                Complex[] complex_bytes_filtered = null;


                if (filter_type == 0) //идеальный
                {
                    complex_bytes_filtered = complex_bytes.Select((a, i) =>
                    {
                        int y = i / new_width;
                        int x = i - y * new_width;
                        y -= new_height / 2;
                        x -= new_width / 2;


                        foreach (var v in filter_params_double)
                        {
                            if ((x - v[0]) * (x - v[0]) + (y - v[1]) * (y - v[1]) >= v[2] * v[2] &&
                                (x - v[0]) * (x - v[0]) + (y - v[1]) * (y - v[1]) <= v[3] * v[3])
                            {
                                filter_bytes[i * 3 + color] = clmp(255 * in_filter_zone);
                                return a * in_filter_zone;
                            }

                        }
                        filter_bytes[i * 3 + color] = clmp(255 * out_filter_zone);
                        return a * out_filter_zone;

                    }).ToArray();
                }
                else
                if (filter_type == 1) //Баттерворта ФНЧ
                {
                    complex_bytes_filtered = complex_bytes.Select((a, i) =>
                    {
                        var val = filter_params_double.Select(v =>
                        {
                            int y = i / new_width;
                            int x = i - y * new_width;
                            y -= new_height / 2;
                            x -= new_width / 2;

                            double wc = 0.5 * v[3] - 0.5 * v[2];
                            double h = v[3] - wc;
                            double b = Butter(x, y, wc, (int)out_filter_zone, v[0], v[1], in_filter_zone, h);
                            return b;
                        }).Max();
                        filter_bytes[i * 3 + color] = clmp(255 * val);
                        return a * val;
                    }).ToArray();
                }
                else if (filter_type == 2)          //Баттерворта ФВЧ
                {
                    complex_bytes_filtered = complex_bytes.Select((a, i) =>
                    {
                        var val = filter_params_double.Select(v =>
                        {
                            int y = i / new_width;
                            int x = i - y * new_width;
                            y -= new_height / 2;
                            x -= new_width / 2;

                            double wc = 0.5 * v[3] - 0.5 * v[2];
                            double h = v[3] - wc;
                            double b = in_filter_zone - Butter(x, y, wc, (int)out_filter_zone, v[0], v[1], in_filter_zone, h);
                            return b;
                        }).Min();
                        filter_bytes[i * 3 + color] = clmp(255 * val);
                        return a * val;
                    }).ToArray();
                }
                else if (filter_type == 3)  //Гаусса ФНЧ
                {
                    complex_bytes_filtered = complex_bytes.Select((a, i) =>
                    {
                        var val = filter_params_double.Select(v =>
                        {
                            int y = i / new_width;
                            int x = i - y * new_width;
                            y -= new_height / 2;
                            x -= new_width / 2;
                            double wc = 0.5 * v[3] - 0.5 * v[2];
                            double h = v[3] - wc;
                            double b = Gauss(x, y, wc, v[0], v[1], in_filter_zone, h);
                            return b;
                        }).Max();
                        filter_bytes[i * 3 + color] = clmp(255 * val);
                        return a * val;
                    }).ToArray();
                }
                else if (filter_type == 4) //Гаусса ФВЧ
                {
                    complex_bytes_filtered = complex_bytes.Select((a, i) =>
                    {

                        var val = filter_params_double.Select(v =>
                        {
                            int y = i / new_width;
                            int x = i - y * new_width;
                            y -= new_height / 2;
                            x -= new_width / 2;
                            double wc = 0.5 * v[3] - 0.5 * v[2];
                            double h = v[3] - wc;
                            double b = in_filter_zone - Gauss(x, y, wc, v[0], v[1], in_filter_zone, h);
                            return b;
                        }).Min();
                        filter_bytes[i * 3 + color] = clmp(255 * val);
                        return a * val;
                    }).ToArray();
                }


                var complex_bytes_result = Fur.ditifft2d(complex_bytes_filtered, new_width, new_height);

                for (int i = 0; i < new_width * new_height; ++i)
                {
                    int y = i / new_width;
                    int x = i - y * new_width;
                    y -= new_height / 2;
                    x -= new_width / 2;
                    new_bytes[i * 3 + color] = clmp(Math.Round((Math.Pow(-1, x + y) * complex_bytes_result[i]).Real));
                    furier_ma_bytes[i * 3 + color] = clmp(furier_multiplyer * F(complex_bytes[i].Magnitude) * 255 / max_ma);
                }

            }

            //Вывод коэф. преобразования в *.csv
            /*using StreamWriter sw = new StreamWriter("fur.csv");
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < new_width; ++j)
                {
                    sw.Write("\""+ spec_re[i*new_width+j].ToString() + "\",");
                }

                sw.Write("\n");
            }
            sw.Close();
           */

            //формируем восстановленное изображение
            using Bitmap new_bitmap = new Bitmap(new_width, new_height, PixelFormat.Format24bppRgb);
            new_bitmap.SetResolution(input.HorizontalResolution, input.VerticalResolution);
            writeImageBytes(new_bitmap, new_bytes);

            //рисуем восстановленное изображение на новом, размер которого совпадает с исходным
            //так как размер восстановленного может отличатся (степени двойки)
            Bitmap new_bitamp_ret = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            new_bitamp_ret.SetResolution(input.HorizontalResolution, input.VerticalResolution);
            using (Graphics g1 = Graphics.FromImage(new_bitamp_ret))
            {
                g1.DrawImageUnscaled(new_bitmap, 0, 0);
            }

            //рисуем Фурье-образ и рисуем на нем оверлеи.
            Bitmap new_bitmap_re = new Bitmap(new_width, new_height, PixelFormat.Format24bppRgb);
            new_bitmap_re.SetResolution(input.HorizontalResolution, input.VerticalResolution);
            writeImageBytes(new_bitmap_re, furier_ma_bytes);
            Furier_re(new_bitmap_re);
            using var g_fur = Graphics.FromImage(new_bitmap_re);
            foreach (var v in filter_params_double)
            {
                g_fur.DrawEllipse(Pens.GreenYellow, (int)v[0] - (int)v[2] + new_width / 2, (int)v[1] - (int)v[2] + new_height / 2, (int)v[2] * 2, (int)v[2] * 2);
                g_fur.DrawEllipse(Pens.GreenYellow, (int)v[0] - (int)v[3] + new_width / 2, (int)v[1] - (int)v[3] + new_height / 2, (int)v[3] * 2, (int)v[3] * 2);
            }

            //рисуем маску фильтра
            Bitmap new_bitmap_mask = new Bitmap(new_width, new_height, PixelFormat.Format24bppRgb);
            new_bitmap_mask.SetResolution(input.HorizontalResolution, input.VerticalResolution);
            writeImageBytes(new_bitmap_mask, filter_bytes);
            Furier_mask(new_bitmap_mask);
            //Furier_image(new_bitmap); //пофиксите это и будет збс   

        }

        public void Furier_image(Bitmap bitmap_ret)
        {
            pictureBox2.Image = bitmap_ret;
            Refresh();
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void Furier_mask(Bitmap bitmap_mask)
        {
            pictureBox3.Image = bitmap_mask;
            Refresh();
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void Furier_re(Bitmap bitmap_re)
        {
            pictureBox4.Image = bitmap_re;
            Refresh();
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            Furier(image);

        }
    }
}
