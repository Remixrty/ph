using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace image_edit_2
{
	public partial class Form4 : Form
	{
		private static Bitmap img = null;
		private Bitmap image = null;
		List<double> rgbValues = new List<double>();
		List<byte> rgbValuesb = new List<byte>();
		public Form4()
		{
			InitializeComponent();

		}

		#region buttons
		private void button3_Click(object sender, EventArgs e) //критерий Гаврилова
		{
			pictureBox2.Image = gavr(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}
		private void button1_Click(object sender, EventArgs e) //критерий Отсу
		{
			pictureBox2.Image = otsu(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}
		private void button5_Click(object sender, EventArgs e) //критерий Ниблека
		{
			pictureBox2.Image = niblack(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}
		private void button6_Click(object sender, EventArgs e) //критерий Сауволы
		{
			pictureBox2.Image = sauval(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}
		private void button8_Click(object sender, EventArgs e)
		{
			pictureBox2.Image = bredly(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}

		#endregion

		#region open/save
		private void button2_Click(object sender, EventArgs e)
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
		private void button4_Click(object sender, EventArgs e)
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

		#region bytes
		public void inbytes()
		{
			rgbValues.Clear();
			rgbValuesb.Clear();
			for (int x = 0; x < image.Height; x++)
				for (int y = 0; y < image.Width; y++)
				{
					var pix = image.GetPixel(y, x);
					rgbValues.Add(0.2125 * pix.R);
					rgbValues.Add(0.7154 * pix.G);
					rgbValues.Add(0.0721 * pix.B);
				}
		}

		void outbytes()
		{
			int l;
			int i = -1;


			for (int x = 0; x < image.Height; x++)
				for (int y = 0; y < image.Width; y++)
				{
					++i;
					if (rgbValuesb[i] == 0)
						l = 0;
					else
						l = 255;
					var pix = image.GetPixel(y, x);
					pix = Color.FromArgb(l, l, l);
					img.SetPixel(y, x, pix);
				}
			pictureBox2.Image = img;
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}
		#endregion

		#region funcs
		public static Bitmap gavr(Bitmap image)
		{
			int width = image.Width;
			int height = image.Height;
			using (Bitmap _tmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
			{
				_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
				using var g = Graphics.FromImage(_tmp);
				g.DrawImageUnscaled(image, 0, 0);


				byte[] inb = getImgBytes(_tmp);
				byte[] outb = new byte[inb.Length];
				for (int i = 0; i < inb.Length; i += 3) //нормализация
				{
					inb[i] = clmp(0.2125 * inb[i + 2] + 0.7154 * inb[i + 1] + 0.0721 * inb[0]);
					inb[i + 2] = inb[i + 1] = inb[i];
				}

				var t = inb.Average(x => x);

				var bytes = inb.Select(x => (x < t) ? (byte)0 : (byte)255).ToArray();
				int a = 10;
				var ty = a << 2;

				Bitmap image_out = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				image_out.SetResolution(image.HorizontalResolution, image.VerticalResolution);
				writeImageBytes(image_out, bytes);

				return image_out;
			}

		}
		public static Bitmap otsu(Bitmap image)
		{
			int w = image.Width;
			int h = image.Height;
			using Bitmap _tmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using var g = Graphics.FromImage(_tmp);
			g.DrawImageUnscaled(image, 0, 0);

			byte[] inb = getImgBytes(_tmp);
			byte[] outb = new byte[inb.Length];



			for (int i = 0; i < inb.Length; i += 3) //нормализация
			{
				inb[i] = clmp(0.2125 * inb[i + 2] + 0.7154 * inb[i + 1] + 0.0721 * inb[i]);
			}



			double[] N = new double[256];
			double[] sum_N = new double[256];
			double[] sum_u = new double[256];
			var Nt = 0.0;
			var max = 0;
			for (int i = 0; i < w * h; ++i)
			{
				N[inb[i * 3]] += 1.0 / w / h;
				Nt += 1.0 / w / h;
				if (inb[i * 3] > max)
					max = inb[i * 3];
			}

			var sum = 0.0;
			var _sum_u = 0.0;

			for (int i = 0; i <= max; ++i)
			{
				sum += N[i];
				_sum_u += i * N[i];
				sum_N[i] = sum;
				sum_u[i] = _sum_u;
			}

			double w1 = 0.0, w2 = 0.0, u1 = 0.0, u2 = 0.0;

			int final_t = 0;
			double sig_max = 0.0;
			for (int t = 1; t <= max; ++t)
			{
				w1 = sum_N[t - 1];
				w2 = 1.0 - w1;
				u1 = sum_u[t - 1] / w1;
				u2 = (sum_u[max] - u1 * w1) / w2;
				var sig = w1 * w2 * (u1 - u2) * (u1 - u2);
				if (sig > sig_max)
				{
					sig_max = sig;
					final_t = t;
				}
			}
			for (int i = 0; i < w * h; ++i)
			{

				if (inb[i * 3] > final_t)
				{
					outb[i * 3] = 255;
					outb[i * 3 + 1] = 255;
					outb[i * 3 + 2] = 255;
				}

				else
				{
					outb[i * 3] = 0;
					outb[i * 3 + 1] = 0;
					outb[i * 3 + 2] = 0;
				}

			}



			Bitmap image_out = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			writeImageBytes(image_out, outb);
			return image_out;

		}
		public static Bitmap niblack(Bitmap image)
		{
			int a = 21;
			double sens = -0.2;
			int w = image.Width;
			int h = image.Height;
			using Bitmap _tmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using var g = Graphics.FromImage(_tmp);
			g.DrawImageUnscaled(image, 0, 0);

			byte[] inb = getImgBytes(_tmp);

			for (int i = 0; i < inb.Length; i += 3)  //нормализация
			{
				inb[i] = (byte)(0.2125 * inb[i + 2] + 0.7154 * inb[i + 1] +
										0.0721 * inb[i + 0]);
			}

			int[,] int_mat = new int[h, w];
			int[,] int_sqr_mat = new int[h, w];

			for (int i = 0; i < h; ++i)
			{
				for (int j = 0; j < w; ++j)
				{
					int_mat[i, j] = inb[i * w * 3 + j * 3] +
									(j >= 1 ? int_mat[i, j - 1] : 0) +
									(i >= 1 ? int_mat[i - 1, j] : 0) -
									(i >= 1 && j >= 1 ? int_mat[i - 1, j - 1] : 0);

					int_sqr_mat[i, j] = inb[i * w * 3 + j * 3] * inb[i * w * 3 + j * 3] +
									(j >= 1 ? int_sqr_mat[i, j - 1] : 0) +
									(i >= 1 ? int_sqr_mat[i - 1, j] : 0) -
									(i >= 1 && j >= 1 ? int_sqr_mat[i - 1, j - 1] : 0);
				}
			}

			for (int _i = 0; _i < h; ++_i)
			{
				int y_min = _i - (int)Math.Ceiling(1.0 * a / 2) + 1;
				y_min = (y_min < 0) ? 0 : y_min;
				int y_max = _i + (int)Math.Floor(1.0 * a / 2);
				y_max = (y_max >= h) ? h - 1 : y_max;

				for (int _j = 0; _j < w; ++_j)
				{

					int index = _i * w * 3 + _j * 3;
					long sum = 0;
					long sqr_sum = 0;

					int x_min = _j - (int)Math.Ceiling(1.0 * a / 2) + 1;
					x_min = (x_min < 0) ? 0 : x_min;
					int x_max = _j + (int)Math.Floor(1.0 * a / 2);
					x_max = (x_max >= w) ? w - 1 : x_max;



					sum = ((x_min >= 1 && y_min >= 1) ? int_mat[y_min - 1, x_min - 1] : 0) + //A
						int_mat[y_max, x_max] -    //D
						((y_min >= 1) ? int_mat[y_min - 1, x_max] : 0) -   //B
						((x_min >= 1) ? int_mat[y_max, x_min - 1] : 0);  //C

					sqr_sum = ((x_min >= 1 && y_min >= 1) ? int_sqr_mat[y_min - 1, x_min - 1] : 0) + //A
							  int_sqr_mat[y_max, x_max] -    //D
						  ((y_min >= 1) ? int_sqr_mat[y_min - 1, x_max] : 0) -   //B
						  ((x_min >= 1) ? int_sqr_mat[y_max, x_min - 1] : 0);  //C

					sqr_sum /= (x_max - x_min + 1) * (y_max - y_min + 1);
					sum /= (x_max - x_min + 1) * (y_max - y_min + 1);

					double D = Math.Sqrt(sqr_sum - sum * sum);
					double t = sum + sens * D;




					//результат обработки кладем в синий канал
					inb[index + 1] = (inb[index + 1] <= t) ? (byte)0 : (byte)255;

				}
			}


			for (int i = 0; i < inb.Length; i += 3)
			{
				inb[i + 0] = inb[i + 1];
				inb[i + 2] = inb[i + 1];
			}


			Bitmap image_out = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			writeImageBytes(image_out, inb);
			return image_out;
		}
		public static Bitmap sauval(Bitmap image)
		{
			int a = 21;
			double k = 0.5;
			int w = image.Width;
			int h = image.Height;
			using Bitmap _tmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using var g = Graphics.FromImage(_tmp);
			g.DrawImageUnscaled(image, 0, 0);

			byte[] inb = getImgBytes(_tmp);

			for (int i = 0; i < inb.Length; i += 3)
			{
				inb[i] = (byte)(0.2125 * inb[i + 2] + 0.7154 * inb[i + 1] +
										0.0721 * inb[i + 0]);
			}

			var int_mat = new long[h, w];
			var int_sqr_mat = new long[h, w];

			for (int i = 0; i < h; ++i)
			{
				for (int j = 0; j < w; ++j)
				{
					int_mat[i, j] = inb[i * w * 3 + j * 3] + (j >= 1 ? int_mat[i, j - 1] : 0) + (i >= 1 ? int_mat[i - 1, j] : 0) - (i >= 1 && j >= 1 ? int_mat[i - 1, j - 1] : 0);

					int_sqr_mat[i, j] = inb[i * w * 3 + j * 3] * inb[i * w * 3 + j * 3] + (j >= 1 ? int_sqr_mat[i, j - 1] : 0) + (i >= 1 ? int_sqr_mat[i - 1, j] : 0) - (i >= 1 && j >= 1 ? int_sqr_mat[i - 1, j - 1] : 0);
				}
			}

			for (int _i = 0; _i < h; ++_i)
			{
				int y_min = _i - (int)Math.Ceiling(1.0 * a / 2) + 1;
				y_min = (y_min < 0) ? 0 : y_min;
				int y_max = _i + (int)Math.Floor(1.0 * a / 2);
				y_max = (y_max >= h) ? h - 1 : y_max;

				for (int _j = 0; _j < w; ++_j)
				{

					int index = _i * w * 3 + _j * 3;
					long sum = 0;
					long sqr_sum = 0;

					int x_min = _j - (int)Math.Ceiling(1.0 * a / 2) + 1;
					x_min = (x_min < 0) ? 0 : x_min;
					int x_max = _j + (int)Math.Floor(1.0 * a / 2);
					x_max = (x_max >= w) ? w - 1 : x_max;



					sum = ((x_min >= 1 && y_min >= 1) ? int_mat[y_min - 1, x_min - 1] : 0) + //A
						int_mat[y_max, x_max] -    //D
						((y_min >= 1) ? int_mat[y_min - 1, x_max] : 0) -   //B
						((x_min >= 1) ? int_mat[y_max, x_min - 1] : 0);  //C

					sqr_sum = ((x_min >= 1 && y_min >= 1) ? int_sqr_mat[y_min - 1, x_min - 1] : 0) + //A
							  int_sqr_mat[y_max, x_max] -    //D
						  ((y_min >= 1) ? int_sqr_mat[y_min - 1, x_max] : 0) -   //B
						  ((x_min >= 1) ? int_sqr_mat[y_max, x_min - 1] : 0);  //C

					sqr_sum /= (x_max - x_min + 1) * (y_max - y_min + 1);
					sum /= (x_max - x_min + 1) * (y_max - y_min + 1);

					double D = Math.Sqrt(sqr_sum - sum * sum);
					double t = sum * (1 + k * (D / 128 - 1));

					inb[index + 1] = (inb[index + 1] <= t) ? (byte)0 : (byte)255;

				}
			}

			for (int i = 0; i < inb.Length; i += 3)
			{
				inb[i + 0] = inb[i + 1];
				inb[i + 2] = inb[i + 1];
			}


			Bitmap image_out = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			writeImageBytes(image_out, inb);
			return image_out;
		}
		public static Bitmap bredly(Bitmap image)
		{
			int a = 21;
			double t = 0.15;
			int w = image.Width;
			int h = image.Height;
			using Bitmap _tmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using var g = Graphics.FromImage(_tmp);
			g.DrawImageUnscaled(image, 0, 0);

			byte[] inb = getImgBytes(_tmp);

			for (int i = 0; i < inb.Length; i += 3)
			{
				inb[i] = (byte)(0.2125 * inb[i + 2] + 0.7154 * inb[i + 1] +
										0.0721 * inb[i + 0]);
			}
			var int_mat = new long[h, w];

			for (int i = 0; i < h; ++i)
			{
				for (int j = 0; j < w; ++j)
				{
					int_mat[i, j] = inb[i * w * 3 + j * 3] + (j >= 1 ? int_mat[i, j - 1] : 0) + (i >= 1 ? int_mat[i - 1, j] : 0) - (i >= 1 && j >= 1 ? int_mat[i - 1, j - 1] : 0);
				}
			}

			for (int _i = 0; _i < h; ++_i)
			{
				int y_min = _i - (int)Math.Ceiling(1.0 * a / 2) + 1;
				y_min = (y_min < 0) ? 0 : y_min;
				int y_max = _i + (int)Math.Floor(1.0 * a / 2);
				y_max = (y_max >= h) ? h - 1 : y_max;

				for (int _j = 0; _j < w; ++_j)
				{

					int index = _i * w * 3 + _j * 3;
					long sum = 0;

					int x_min = _j - (int)Math.Ceiling(1.0 * a / 2) + 1;
					x_min = (x_min < 0) ? 0 : x_min;
					int x_max = _j + (int)Math.Floor(1.0 * a / 2);
					x_max = (x_max >= w) ? w - 1 : x_max;



					sum = ((x_min >= 1 && y_min >= 1) ? int_mat[y_min - 1, x_min - 1] : 0) + //A
						int_mat[y_max, x_max] -    //D
						((y_min >= 1) ? int_mat[y_min - 1, x_max] : 0) -   //B
						((x_min >= 1) ? int_mat[y_max, x_min - 1] : 0);  //C

					int count = (x_max - x_min + 1) * (y_max - y_min + 1);

					inb[index + 1] = ((Int64)(inb[index + 1] * count) < (Int64)(sum * (1.0 - t))) ? (byte)0 : (byte)255;

				}
			}

			for (int i = 0; i < inb.Length; i += 3)
			{
				inb[i + 0] = inb[i + 1];
				inb[i + 2] = inb[i + 1];
			}


			Bitmap image_out = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			writeImageBytes(image_out, inb);
			return image_out;
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

		#region back/exit
		private void button7_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void button9_Click(object sender, EventArgs e)
		{
			Form1 lob = new Form1();
			lob.Show(this);
			this.Hide();
		}
		#endregion

	}
}
