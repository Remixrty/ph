using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using static System.Environment;


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
			pictureBox2.Image = MatrixRash(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}
		private void button3_Click(object sender, EventArgs e) //запись в массив
		{
			
			pictureBox2.Image = Median(image);
			Refresh();
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
		}

		private Bitmap MatrixRash (Bitmap image)
		{
			int width = image.Width;
			int height = image.Height;
			string matrix = richTextBox1.Text;

			Bitmap _tmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using (Graphics g = Graphics.FromImage(_tmp))
			{
				g.DrawImageUnscaled(image, 0, 0);
				//g.DrawImage(input,0,0,new RectangleF(0,0,input.Width,input.Height),GraphicsUnit.Pixel);
			}


			byte[] old_bytes = getImgBytes(_tmp);
			byte[] new_bytes = new byte[width * height * 3];


			var core = getCoreFromStr(matrix);
			int M = core.GetLength(0);
			int N = core.GetLength(1);

			ParallelOptions opt = new ParallelOptions();
			if (ProcessorCount > 2)
				opt.MaxDegreeOfParallelism = ProcessorCount - 2;
			else opt.MaxDegreeOfParallelism = 1;
			Parallel.For(0, width * height, opt, arr_i =>
			{
				int _i = arr_i / width;
				int _j = arr_i - _i * width;

				double sum1 = 0;
				double sum2 = 0;
				double sum3 = 0;

				for (int ii = 0; ii < M; ++ii)   // h - (i - h)     h - i + h = 2h-i
				{
					int i = _i + ii - M / 2;
					if (i < 0)
						i *= -1;
					if (i >= height)
						i = 2 * height - i - 1;

					for (int jj = 0; jj < N; ++jj)
					{
						int j = _j + jj - N / 2;

						if (j < 0)
							j *= -1;

						if (j >= width)
							j = 2 * width - j - 1;

						sum1 += old_bytes[width * i * 3 + j * 3 + 0] * core[ii, jj];
						sum2 += old_bytes[width * i * 3 + j * 3 + 1] * core[ii, jj];
						sum3 += old_bytes[width * i * 3 + j * 3 + 2] * core[ii, jj];
					}
				}
				new_bytes[arr_i * 3 + 0] = clmp(sum1);
				new_bytes[arr_i * 3 + 1] = clmp(sum2);
				new_bytes[arr_i * 3 + 2] = clmp(sum3);

			});

			Bitmap new_bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			new_bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			writeImageBytes(new_bitmap, new_bytes);

			return new_bitmap;
		}


		public Bitmap Median (Bitmap image)
		{
			int width = image.Width;
			int height = image.Height;
			int wnd_size = Convert.ToInt32(textBox1.Text);

			Bitmap _tmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			_tmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using (Graphics g = Graphics.FromImage(_tmp))
			{
				g.DrawImageUnscaled(image, 0, 0);
				//g.DrawImage(input,0,0,new RectangleF(0,0,input.Width,input.Height),GraphicsUnit.Pixel);
			}

			byte[] old_bytes = getImgBytes(_tmp);
			byte[] new_bytes = new byte[width * height * 3];

			//массивчик для медианы

			Mutex mutex = new Mutex();
			int iter_count = old_bytes.Length / 3;

			//for (int _i = 0; _i < height; ++_i)
			ParallelOptions opt = new ParallelOptions();
			if (ProcessorCount > 2)
				opt.MaxDegreeOfParallelism = ProcessorCount - 2;
			else opt.MaxDegreeOfParallelism = 1;

			//opt.MaxDegreeOfParallelism = 1;

			Parallel.For(0, height, opt, _i =>
			//for (int _i = 0; _i < height; ++_i)
			{

				var curPriority = Thread.CurrentThread.Priority;
				Thread.CurrentThread.Priority = ThreadPriority.Lowest;

				int[] wndR = new int[wnd_size * wnd_size];
				int[] wndG = new int[wnd_size * wnd_size];
				int[] wndB = new int[wnd_size * wnd_size];

				for (int _j = 0; _j < width; ++_j)
				{

					for (int ii = 0; ii < wnd_size; ++ii) // h - (i - h)     h - i + h = 2h-i
					{
						for (int jj = 0; jj < wnd_size; ++jj)
						{
							int i = _i + ii - wnd_size / 2;

							if (i < 0)
								i *= -1;
							if (i >= height)
								i = 2 * height - i - 1;

							int j = _j + jj - wnd_size / 2;
							if (j < 0)
								j *= -1;
							if (j >= width)
								j = 2 * width - j - 1;

							wndR[ii * wnd_size + jj] = old_bytes[i * width * 3 + j * 3 + 0];
							wndG[ii * wnd_size + jj] = old_bytes[i * width * 3 + j * 3 + 1];
							wndB[ii * wnd_size + jj] = old_bytes[i * width * 3 + j * 3 + 2];
						}
					}

					//new_bytes[_i * width * 3 + _j * 3 + 0] = (byte)QuickSelect.kthSmallest(wndR, 0, wndR.Length - 1, wnd_size * wnd_size / 2 - 1); 
					//new_bytes[_i * width * 3 + _j * 3 + 1] = (byte)QuickSelect.kthSmallest(wndG, 0, wndG.Length - 1, wnd_size * wnd_size / 2 - 1); ;
					//new_bytes[_i * width * 3 + _j * 3 + 2] = (byte)QuickSelect.kthSmallest(wndB, 0, wndB.Length - 1, wnd_size * wnd_size / 2 - 1); ;

					//new_bytes[_i * width * 3 + _j * 3 + 0] = (byte)quickselect(wndR, wnd_size * wnd_size / 2);
					//new_bytes[_i * width * 3 + _j * 3 + 1] = (byte)quickselect(wndG, wnd_size * wnd_size / 2);
					//new_bytes[_i * width * 3 + _j * 3 + 2] = (byte)quickselect(wndB, wnd_size * wnd_size / 2);

					new_bytes[_i * width * 3 + _j * 3 + 0] = (byte)quickselect2(wndR, 0, wnd_size * wnd_size, wnd_size * wnd_size / 2);
					new_bytes[_i * width * 3 + _j * 3 + 1] = (byte)quickselect2(wndG, 0, wnd_size * wnd_size, wnd_size * wnd_size / 2);
					new_bytes[_i * width * 3 + _j * 3 + 2] = (byte)quickselect2(wndB, 0, wnd_size * wnd_size, wnd_size * wnd_size / 2);


				}

				
				Thread.CurrentThread.Priority = curPriority;

			});


			Bitmap new_bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			new_bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			writeImageBytes(new_bitmap, new_bytes);

			return new_bitmap;
		}

		static double[,] getCoreFromStr(string matrix)
		{
			
			char[] splitter = { '\n' };
			matrix = matrix.Replace('\r', ' ');
			var str_list = matrix.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
			double[,] mat = new double[0, 0];


			for (int i = 0; i < str_list.Count(); ++i)
			{
				
				str_list[i] = str_list[i].Replace('\r', ' ');
				var chars = str_list[i].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

				if (i == 0)
				{
					mat = new double[str_list.Length, chars.Length];
				}

				for (int j = 0; j < chars.Length; ++j)
				{
					var frac = Fraction.fromString(chars[j]);
					mat[i, j] = frac.toDouble();
				}
			}

			return mat;

		}

		private static (byte, int) quickselect((byte, int)[] arr, int k)
		{
			if (arr.Length == 1)
				return arr[0];

			Random r = new Random();
			int pivot = r.Next(arr.Length);
			//int pivot = 0;

			var lows = arr.Where(x => x.Item1 < arr[pivot].Item1).ToArray();
			var high = arr.Where(x => x.Item1 > arr[pivot].Item1).ToArray();
			var eqv = arr.Where(x => x.Item1 == arr[pivot].Item1).ToArray();

			if (k < lows.Length)
				return quickselect(lows, k);
			else if (k < lows.Length + eqv.Length)
				return eqv[k - lows.Length];
			else
				return quickselect(high, k - lows.Length - eqv.Length);

		}

		private static int quickselect2(int[] arr, int left, int right, int k)
		{
			if (right - left == 1)
				return arr[left];

			int left_count = 0;
			int eqv_count = 0;
			int tmp = 0;

			for (int i = left; i < right - 1; ++i)
			{
				if (arr[i] < arr[right - 1])
				{
					tmp = arr[i];
					arr[i] = arr[left + left_count];
					arr[left + left_count] = tmp;
					left_count++;
				}
			}
			for (int i = left + left_count; i < right - 1; ++i)
			{
				if (arr[i] == arr[right - 1])
				{
					tmp = arr[i];
					arr[i] = arr[left + left_count + eqv_count];
					arr[left + left_count + eqv_count] = tmp;
					eqv_count++;
				}
			}
			tmp = arr[right - 1];
			arr[right - 1] = arr[left + left_count + eqv_count];
			arr[left + left_count + eqv_count] = tmp;


			if (k < left_count)
				return quickselect2(arr, left, left + left_count, k);
			else if (k < left_count + eqv_count)
				return arr[left + left_count];
			else
				return quickselect2(arr, left + left_count + eqv_count, right, k - left_count - eqv_count);

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

		private void Form5_Load(object sender, EventArgs e)
		{

		}

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
	}
}
