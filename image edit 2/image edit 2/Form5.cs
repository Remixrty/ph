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
	public partial class Form5 : Form
	{

		private static Bitmap img = null;
		private Bitmap image = null;
		byte[,] arr = new byte[10, 10];
		public Form5()
		{
			InitializeComponent();
		}
		private void button1_Click(object sender, EventArgs e) //видимость текстбоксов
		{
			int X = trackBar1.Value;
			int Y = trackBar2.Value;

			textBox1.Visible = true;
			button3.Visible = true;
			
			if (X >= 2) textBox2.Visible = true; else textBox2.Visible = false;
			if (X >= 3)	textBox3.Visible = true; else textBox3.Visible = false;
			if (X >= 4)	textBox4.Visible = true; else textBox4.Visible = false;
			if (X >= 5) textBox5.Visible = true; else textBox5.Visible = false;
			if (X >= 6)	textBox6.Visible = true; else textBox6.Visible = false;
			if (X >= 7) textBox7.Visible = true; else textBox7.Visible = false;
			if (X >= 8)	textBox8.Visible = true; else textBox8.Visible = false;
			if (X >= 9)	textBox9.Visible = true; else textBox9.Visible = false;
			if (X >= 10) textBox10.Visible = true; else textBox10.Visible = false;

			if (Y >= 2) textBox11.Visible = true; else textBox11.Visible = false;
			if (Y >= 3) textBox21.Visible = true; else textBox21.Visible = false;
			if (Y >= 4)	textBox31.Visible = true; else textBox31.Visible = false;
			if (Y >= 5) textBox41.Visible = true; else textBox41.Visible = false;
			if (Y >= 6) textBox51.Visible = true; else textBox51.Visible = false;
			if (Y >= 7) textBox61.Visible = true; else textBox61.Visible = false;
			if (Y >= 8) textBox71.Visible = true; else textBox71.Visible = false;
			if (Y >= 9)	textBox81.Visible = true; else textBox81.Visible = false;
			if (Y >= 10) textBox91.Visible = true; else textBox91.Visible = false;

			if ((X >= 2) && (Y >= 2)) textBox12.Visible = true;
			else textBox12.Visible = false;
			if ((X >= 2) && (Y >= 3)) textBox22.Visible = true;
			else textBox22.Visible = false;
			if ((X >= 2) && (Y >= 4)) textBox32.Visible = true;
			else textBox32.Visible = false;
			if ((X >= 2) && (Y >= 5)) textBox42.Visible = true;
			else textBox42.Visible = false;
			if ((X >= 2) && (Y >= 6)) textBox52.Visible = true;
			else textBox52.Visible = false;
			if ((X >= 2) && (Y >= 7)) textBox62.Visible = true;
			else textBox62.Visible = false;
			if ((X >= 2) && (Y >= 8)) textBox72.Visible = true;
			else textBox72.Visible = false;
			if ((X >= 2) && (Y >= 9)) textBox82.Visible = true;
			else textBox82.Visible = false;
			if ((X >= 2) && (Y >= 10)) textBox92.Visible = true;
			else textBox92.Visible = false;

			if ((X >= 3) && (Y >= 2)) textBox13.Visible = true;
			else textBox13.Visible = false;
			if ((X >= 3) && (Y >= 3)) textBox23.Visible = true;
			else textBox23.Visible = false;
			if ((X >= 3) && (Y >= 4)) textBox33.Visible = true;
			else textBox33.Visible = false;
			if ((X >= 3) && (Y >= 5)) textBox43.Visible = true;
			else textBox43.Visible = false;
			if ((X >= 3) && (Y >= 6)) textBox53.Visible = true;
			else textBox53.Visible = false;
			if ((X >= 3) && (Y >= 7)) textBox63.Visible = true;
			else textBox63.Visible = false;
			if ((X >= 3) && (Y >= 8)) textBox73.Visible = true;
			else textBox73.Visible = false;
			if ((X >= 3) && (Y >= 9)) textBox83.Visible = true;
			else textBox83.Visible = false;
			if ((X >= 3) && (Y >= 10)) textBox93.Visible = true;
			else textBox93.Visible = false;

			if ((X >= 4) && (Y >= 2)) textBox14.Visible = true;
			else textBox14.Visible = false;
			if ((X >= 4) && (Y >= 3)) textBox24.Visible = true;
			else textBox24.Visible = false;
			if ((X >= 4) && (Y >= 4)) textBox34.Visible = true;
			else textBox34.Visible = false;
			if ((X >= 4) && (Y >= 5)) textBox44.Visible = true;
			else textBox44.Visible = false;
			if ((X >= 4) && (Y >= 6)) textBox54.Visible = true;
			else textBox54.Visible = false;
			if ((X >= 4) && (Y >= 7)) textBox64.Visible = true;
			else textBox64.Visible = false;
			if ((X >= 4) && (Y >= 8)) textBox74.Visible = true;
			else textBox74.Visible = false;
			if ((X >= 4) && (Y >= 9)) textBox84.Visible = true;
			else textBox84.Visible = false;
			if ((X >= 4) && (Y >= 10)) textBox94.Visible = true;
			else textBox94.Visible = false;

			if ((X >= 5) && (Y >= 2)) textBox15.Visible = true;
			else textBox15.Visible = false;
			if ((X >= 5) && (Y >= 3)) textBox25.Visible = true;
			else textBox25.Visible = false;
			if ((X >= 5) && (Y >= 4)) textBox35.Visible = true;
			else textBox35.Visible = false;
			if ((X >= 5) && (Y >= 5)) textBox45.Visible = true;
			else textBox45.Visible = false;
			if ((X >= 5) && (Y >= 6)) textBox55.Visible = true;
			else textBox55.Visible = false;
			if ((X >= 5) && (Y >= 7)) textBox65.Visible = true;
			else textBox65.Visible = false;
			if ((X >= 5) && (Y >= 8)) textBox75.Visible = true;
			else textBox75.Visible = false;
			if ((X >= 5) && (Y >= 9)) textBox85.Visible = true;
			else textBox85.Visible = false;
			if ((X >= 5) && (Y >= 10)) textBox95.Visible = true;
			else textBox95.Visible = false;

			if ((X >= 6) && (Y >= 2)) textBox16.Visible = true;
			else textBox16.Visible = false;
			if ((X >= 6) && (Y >= 3)) textBox26.Visible = true;
			else textBox26.Visible = false;
			if ((X >= 6) && (Y >= 4)) textBox36.Visible = true;
			else textBox36.Visible = false;
			if ((X >= 6) && (Y >= 5)) textBox46.Visible = true;
			else textBox46.Visible = false;
			if ((X >= 6) && (Y >= 6)) textBox56.Visible = true;
			else textBox56.Visible = false;
			if ((X >= 6) && (Y >= 7)) textBox66.Visible = true;
			else textBox66.Visible = false;
			if ((X >= 6) && (Y >= 8)) textBox76.Visible = true;
			else textBox76.Visible = false;
			if ((X >= 6) && (Y >= 9)) textBox86.Visible = true;
			else textBox86.Visible = false;
			if ((X >= 6) && (Y >= 10)) textBox96.Visible = true;
			else textBox96.Visible = false;

			if ((X >= 7) && (Y >= 2)) textBox17.Visible = true;
			else textBox17.Visible = false;
			if ((X >= 7) && (Y >= 3)) textBox27.Visible = true;
			else textBox27.Visible = false;
			if ((X >= 7) && (Y >= 4)) textBox37.Visible = true;
			else textBox37.Visible = false;
			if ((X >= 7) && (Y >= 5)) textBox47.Visible = true;
			else textBox47.Visible = false;
			if ((X >= 7) && (Y >= 6)) textBox57.Visible = true;
			else textBox57.Visible = false;
			if ((X >= 7) && (Y >= 7)) textBox67.Visible = true;
			else textBox67.Visible = false;
			if ((X >= 7) && (Y >= 8)) textBox77.Visible = true;
			else textBox77.Visible = false;
			if ((X >= 7) && (Y >= 9)) textBox87.Visible = true;
			else textBox87.Visible = false;
			if ((X >= 7) && (Y >= 10)) textBox97.Visible = true;
			else textBox97.Visible = false;

			if ((X >= 8) && (Y >= 2)) textBox18.Visible = true;
			else textBox18.Visible = false;
			if ((X >= 8) && (Y >= 3)) textBox28.Visible = true;
			else textBox28.Visible = false;
			if ((X >= 8) && (Y >= 4)) textBox38.Visible = true;
			else textBox38.Visible = false;
			if ((X >= 8) && (Y >= 5)) textBox48.Visible = true;
			else textBox48.Visible = false;
			if ((X >= 8) && (Y >= 6)) textBox58.Visible = true;
			else textBox58.Visible = false;
			if ((X >= 8) && (Y >= 7)) textBox68.Visible = true;
			else textBox68.Visible = false;
			if ((X >= 8) && (Y >= 8)) textBox78.Visible = true;
			else textBox78.Visible = false;
			if ((X >= 8) && (Y >= 9)) textBox88.Visible = true;
			else textBox88.Visible = false;
			if ((X >= 8) && (Y >= 10)) textBox98.Visible = true;
			else textBox98.Visible = false;

			if ((X >= 9) && (Y >= 2)) textBox19.Visible = true;
			else textBox19.Visible = false;
			if ((X >= 9) && (Y >= 3)) textBox29.Visible = true;
			else textBox29.Visible = false;
			if ((X >= 9) && (Y >= 4)) textBox39.Visible = true;
			else textBox39.Visible = false;
			if ((X >= 9) && (Y >= 5)) textBox49.Visible = true;
			else textBox49.Visible = false;
			if ((X >= 9) && (Y >= 6)) textBox59.Visible = true;
			else textBox59.Visible = false;
			if ((X >= 9) && (Y >= 7)) textBox69.Visible = true;
			else textBox69.Visible = false;
			if ((X >= 9) && (Y >= 8)) textBox79.Visible = true;
			else textBox79.Visible = false;
			if ((X >= 9) && (Y >= 9)) textBox89.Visible = true;
			else textBox89.Visible = false;
			if ((X >= 9) && (Y >= 10)) textBox99.Visible = true;
			else textBox99.Visible = false;

			if ((X >= 10) && (Y >= 2)) textBox20.Visible = true;
			else textBox20.Visible = false;
			if ((X >= 10) && (Y >= 3)) textBox30.Visible = true;
			else textBox30.Visible = false;
			if ((X >= 10) && (Y >= 4)) textBox40.Visible = true;
			else textBox40.Visible = false;
			if ((X >= 10) && (Y >= 5)) textBox50.Visible = true;
			else textBox50.Visible = false;
			if ((X >= 10) && (Y >= 6)) textBox60.Visible = true;
			else textBox60.Visible = false;
			if ((X >= 10) && (Y >= 7)) textBox70.Visible = true;
			else textBox70.Visible = false;
			if ((X >= 10) && (Y >= 8)) textBox80.Visible = true;
			else textBox80.Visible = false;
			if ((X >= 10) && (Y >= 9)) textBox90.Visible = true;
			else textBox90.Visible = false;
			if ((X >= 10) && (Y >= 10)) textBox100.Visible = true;
			else textBox100.Visible = false;


		}
		private void button3_Click(object sender, EventArgs e) //запись в массив
		{
			int X = trackBar1.Value;
			int Y = trackBar2.Value;
			Array.Clear(arr, 0, arr.Length);

			arr[0, 0] = Convert.ToByte(textBox1.Text); arr[1, 0] = Convert.ToByte(textBox2.Text); arr[2, 0] = Convert.ToByte(textBox3.Text); arr[3, 0] = Convert.ToByte(textBox4.Text); arr[4, 0] = Convert.ToByte(textBox5.Text); arr[5, 0] = Convert.ToByte(textBox6.Text); arr[6, 0] = Convert.ToByte(textBox7.Text); arr[7, 0] = Convert.ToByte(textBox8.Text); arr[8, 0] = Convert.ToByte(textBox9.Text); arr[9, 0] = Convert.ToByte(textBox10.Text);
			arr[0, 1] = Convert.ToByte(textBox11.Text); arr[1, 1] = Convert.ToByte(textBox12.Text); arr[2, 1] = Convert.ToByte(textBox13.Text); arr[3, 1] = Convert.ToByte(textBox14.Text); arr[4, 1] = Convert.ToByte(textBox15.Text); arr[5, 1] = Convert.ToByte(textBox16.Text); arr[6, 1] = Convert.ToByte(textBox17.Text); arr[7, 1] = Convert.ToByte(textBox18.Text); arr[8, 1] = Convert.ToByte(textBox19.Text); arr[9, 1] = Convert.ToByte(textBox20.Text);
			arr[0, 2] = Convert.ToByte(textBox21.Text); arr[1, 2] = Convert.ToByte(textBox22.Text); arr[2, 2] = Convert.ToByte(textBox23.Text); arr[3, 2] = Convert.ToByte(textBox24.Text); arr[4, 2] = Convert.ToByte(textBox25.Text); arr[5, 2] = Convert.ToByte(textBox26.Text); arr[6, 2] = Convert.ToByte(textBox27.Text); arr[7, 2] = Convert.ToByte(textBox28.Text); arr[8, 2] = Convert.ToByte(textBox29.Text); arr[9, 2] = Convert.ToByte(textBox30.Text);
			arr[0, 3] = Convert.ToByte(textBox31.Text); arr[1, 3] = Convert.ToByte(textBox32.Text); arr[2, 3] = Convert.ToByte(textBox33.Text); arr[3, 3] = Convert.ToByte(textBox34.Text); arr[4, 3] = Convert.ToByte(textBox35.Text); arr[5, 3] = Convert.ToByte(textBox36.Text); arr[6, 3] = Convert.ToByte(textBox37.Text); arr[7, 3] = Convert.ToByte(textBox38.Text); arr[8, 3] = Convert.ToByte(textBox39.Text); arr[9, 3] = Convert.ToByte(textBox40.Text);
			arr[0, 4] = Convert.ToByte(textBox41.Text); arr[1, 4] = Convert.ToByte(textBox42.Text); arr[2, 4] = Convert.ToByte(textBox43.Text); arr[3, 4] = Convert.ToByte(textBox44.Text); arr[4, 4] = Convert.ToByte(textBox45.Text); arr[5, 4] = Convert.ToByte(textBox46.Text); arr[6, 4] = Convert.ToByte(textBox47.Text); arr[7, 4] = Convert.ToByte(textBox48.Text); arr[8, 4] = Convert.ToByte(textBox49.Text); arr[9, 4] = Convert.ToByte(textBox50.Text);
			arr[0, 5] = Convert.ToByte(textBox51.Text); arr[1, 5] = Convert.ToByte(textBox52.Text); arr[2, 5] = Convert.ToByte(textBox53.Text); arr[3, 5] = Convert.ToByte(textBox54.Text); arr[4, 5] = Convert.ToByte(textBox55.Text); arr[5, 5] = Convert.ToByte(textBox56.Text); arr[6, 5] = Convert.ToByte(textBox57.Text); arr[7, 5] = Convert.ToByte(textBox58.Text); arr[8, 5] = Convert.ToByte(textBox59.Text); arr[9, 5] = Convert.ToByte(textBox60.Text);
			arr[0, 6] = Convert.ToByte(textBox61.Text); arr[1, 6] = Convert.ToByte(textBox62.Text); arr[2, 6] = Convert.ToByte(textBox63.Text); arr[3, 6] = Convert.ToByte(textBox64.Text); arr[4, 6] = Convert.ToByte(textBox65.Text); arr[5, 6] = Convert.ToByte(textBox66.Text); arr[6, 6] = Convert.ToByte(textBox67.Text); arr[7, 6] = Convert.ToByte(textBox68.Text); arr[8, 6] = Convert.ToByte(textBox69.Text); arr[9, 6] = Convert.ToByte(textBox70.Text);
			arr[0, 7] = Convert.ToByte(textBox71.Text); arr[1, 7] = Convert.ToByte(textBox72.Text); arr[2, 7] = Convert.ToByte(textBox73.Text); arr[3, 7] = Convert.ToByte(textBox74.Text); arr[4, 7] = Convert.ToByte(textBox75.Text); arr[5, 7] = Convert.ToByte(textBox76.Text); arr[6, 7] = Convert.ToByte(textBox77.Text); arr[7, 7] = Convert.ToByte(textBox78.Text); arr[8, 7] = Convert.ToByte(textBox79.Text); arr[9, 7] = Convert.ToByte(textBox80.Text);
			arr[0, 8] = Convert.ToByte(textBox81.Text); arr[1, 8] = Convert.ToByte(textBox82.Text); arr[2, 8] = Convert.ToByte(textBox83.Text); arr[3, 8] = Convert.ToByte(textBox84.Text); arr[4, 8] = Convert.ToByte(textBox85.Text); arr[5, 8] = Convert.ToByte(textBox86.Text); arr[6, 8] = Convert.ToByte(textBox87.Text); arr[7, 8] = Convert.ToByte(textBox88.Text); arr[8, 8] = Convert.ToByte(textBox89.Text); arr[9, 8] = Convert.ToByte(textBox90.Text);
			arr[0, 9] = Convert.ToByte(textBox91.Text); arr[1, 9] = Convert.ToByte(textBox92.Text); arr[2, 9] = Convert.ToByte(textBox93.Text); arr[3, 9] = Convert.ToByte(textBox94.Text); arr[4, 9] = Convert.ToByte(textBox95.Text); arr[5, 9] = Convert.ToByte(textBox96.Text); arr[6, 9] = Convert.ToByte(textBox97.Text); arr[7, 9] = Convert.ToByte(textBox98.Text); arr[8, 9] = Convert.ToByte(textBox99.Text); arr[9, 9] = Convert.ToByte(textBox100.Text);


			pictureBox2.Image = MatrixRash(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}

		private Bitmap MatrixRash (Bitmap image)
		{
			int w = image.Width;
			int h = image.Height;
			int X = trackBar1.Value;
			int Y = trackBar2.Value;
			using Bitmap _tmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using var g = Graphics.FromImage(_tmp);
			g.DrawImageUnscaled(image, 0, 0);

			byte[] inb = getImgBytes(_tmp);
			byte[] outb = new byte[inb.Length];
			byte[,,] outt = new byte[w*h/X*Y, X*Y, 3];


			for (int i = 0; i < inb.Length-X*Y*3; i += 3 * X * Y)
			{
				int fl = 0;
				for (int k = 0; k < X; k++)
				for (int j = 0; j < Y; j++)
				{
					outt[k, j, 0] = clmp(inb[3*(fl)*i]*arr[k,j]);
					outt[k, j, 1] = clmp(inb[3*(fl)*i + 1]*arr[k,j]);
					outt[k, j, 2] = clmp(inb[3*(fl)*i + 2]*arr[k,j]);
					fl++;
				}
			}
				for (int i = 0; i<inb.Length-Width; i+=3) 
				{
						outb[i] = clmp(inb[i] * arr[0, 0]);
						outb[i+1] = clmp(inb[i] * arr[0, 0]);
						outb[i+2] = clmp(inb[i] * arr[0, 0]);
						outb[i + Width] = clmp(inb[i] * arr[0, 1]);
						outb[i + 1 + Width] = clmp(inb[i] * arr[0, 1]);
						outb[i + 2 + Width] = clmp(inb[i] * arr[0, 1]);
				}




			Bitmap image_out = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			writeImageBytes(image_out, outb);
			return image_out;
		}

		#region get/write
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
	}
}
