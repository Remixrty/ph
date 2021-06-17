﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace image_edit_2
{
    public static class Fur
    {
        //Быстрое преобразование Фурье (FFT).
        public static Complex[] ditfft2(Complex[] arr, int x0, int N, int s)
        {
            Complex[] X = new Complex[N];
            if (N == 1)
            {
                X[0] = arr[x0];
            }
            else
            {
                ditfft2(arr, x0, N / 2, 2 * s).CopyTo(X, 0);
                ditfft2(arr, x0 + s, N / 2, 2 * s).CopyTo(X, N / 2);

                for (int k = 0; k < N / 2; k++)
                {
                    var t = X[k];
                    double u = -2.0 * Math.PI * k / N;
                    X[k] = t + new Complex(Math.Cos(u), Math.Sin(u)) * X[k + N / 2];
                    X[k + N / 2] = t - new Complex(Math.Cos(u), Math.Sin(u)) * X[k + N / 2];
                }
            }

            return X;
        }

        //Преобразование Фурье
        public static Complex[] ditft(Complex[] arr)
        {
            Complex[] X = new Complex[arr.Length];
            for (int i = 0; i < arr.Length; ++i)
            {
                for (int j = 0; j < arr.Length; ++j)
                {
                    double u = -2.0 * Math.PI * i * j / arr.Length;
                    X[i] += (new Complex(Math.Cos(u), Math.Sin(u)) * arr[j]);
                }

            }

            return X;
        }

        public static Complex[] ditfft2d(Complex[] arr, int width, int height)
        {
            Complex[] X = new Complex[arr.Length];

            //for (int i = 0; i < height; ++i)
            Parallel.For(0, height, (i) =>
            {
                Complex[] tmp = new Complex[width];
                Array.Copy(arr, i * width, tmp, 0, width);
                tmp = ditfft2(tmp, 0, width, 1);
                //tmp = ditft(tmp);
                for (int k = 0; k < width; ++k)
                    X[i * width + k] = tmp[k] / width;
            }
            );
            //for (int j = 0; j < width; ++j)
            Parallel.For(0, width, (j) =>
            {
                Complex[] tmp = new Complex[height];
                for (int k = 0; k < height; ++k)
                    tmp[k] = X[j + k * width];
                tmp = ditfft2(tmp, 0, tmp.Length, 1);
                //tmp = ditft(tmp);
                for (int k = 0; k < height; ++k)
                    X[j + k * width] = tmp[k] / height;
            }
            );
            return X;
        }

        public static Complex[] ditifft2d(Complex[] arr, int width, int height)
        {
            Complex[] X = new Complex[arr.Length];

            //for (int i = 0; i < height; ++i)
            Parallel.For(0, height, (i) =>
            {
                Complex[] tmp = new Complex[width];
                Array.Copy(arr, i * width, tmp, 0, width);
                for (int k = 0; k < width; ++k)
                    tmp[k] = new Complex(arr[i * width + k].Real, -arr[i * width + k].Imaginary);
                tmp = ditfft2(tmp, 0, width, 1);
                //tmp = ditft(tmp);
                for (int k = 0; k < width; ++k)
                    X[i * width + k] = (new Complex(tmp[k].Real, -tmp[k].Imaginary));

            }
            );

            //for (int j = 0; j < width; ++j)
            Parallel.For(0, width, (j) =>
            {
                Complex[] tmp = new Complex[height];
                for (int k = 0; k < height; ++k)
                    tmp[k] = new Complex(X[j + k * width].Real, -X[j + k * width].Imaginary);
                tmp = ditfft2(tmp, 0, tmp.Length, 1);
                //tmp = ditft(tmp);
                for (int k = 0; k < height; ++k)
                    X[j + k * width] = (new Complex(tmp[k].Real, -tmp[k].Imaginary));
            }
            );
            return X;
        }
    }

    public class Fraction
	{
        long numerator;
        long denominator;
        long sign;

        public long Numeretor
        {
            get => numerator;
            set => numerator = value;
        }

        public long Denumerator
        {
            get => denominator;
            set => denominator = value;
        }

        public static Fraction fromString(string str)
        {
            return new Fraction(str);
        }

        public string toString()
        {
            string s = string.Empty;

            if (sign < 0)
                s += "-";

            var nok = Mathematics.NOK(10, denominator);

            for (int i = 1; i <= 2; i++)
            {
                if (Math.Pow(10, i) % denominator == 0)
                {
                    s += Convert.ToString(1.0 * numerator / denominator, new CultureInfo("en-US"));
                    return s;
                }
            }

            s += Convert.ToString(numerator, new CultureInfo("en-US"));
            if (numerator == 0 || denominator == 1)
                return s;
            s += "/" + Convert.ToString(denominator, new CultureInfo("en-US"));
            return s;

        }

        public Fraction(long i)
        {
            denominator = 1;
            numerator = Math.Abs(i);
            sign = getSign(i);

        }

        public Fraction()
        {
            sign = 1;
            numerator = 0;
            denominator = 1;
        }

        private static int getSign(double i)
        {
            if (i >= 0) return 1;
            return -1;
        }

        private static int getSign(int i)
        {
            if (i >= 0) return 1;
            return -1;
        }

        public Fraction(string s)
        {
            string[] arr = s.Split(new char[] { '/' });
            if (arr.Count<string>() == 2)
            {
                var a = Convert.ToInt64(arr[0]);
                var b = Convert.ToInt64(arr[1]);

                numerator = Math.Abs(a);
                denominator = Math.Abs(b);
                if (numerator != 0)
                    sign = getSign(a);
                return;
            }

            if (arr.Count<string>() == 1)
            {
                double a = Convert.ToDouble(s, new CultureInfo("en-US"));
                double drobn = a - (long)a;

                numerator = Math.Abs((long)a);
                denominator = 1;
                sign = getSign(a);

                if (drobn != 0)
                {

                    //double d = 0;

                    string tmp = string.Empty;
                    tmp = s.Split(new char[] { '.' })[1];

                    var razm = tmp.Length;
                    var chislo = Convert.ToInt64(tmp);

                    Fraction f = Fraction.Null();
                    f.numerator = chislo;
                    f.denominator = (long)Math.Pow(10, razm);

                    Fraction f1 = this + sign * f;

                    numerator = f1.numerator;
                    denominator = f1.denominator;
                    //Rediction();

                }

                return;
            }
            throw (new Exception());

        }

        public static Fraction Null()
        {
            Fraction f = new Fraction();
            f.numerator = 0;
            f.denominator = 1;
            f.sign = 1;

            return f;
        }

        public void Rediction()
        {
            var nod = Mathematics.NOD(numerator, denominator);
            numerator /= nod;
            denominator /= nod;
        }

        public static Fraction operator +(Fraction f1, Fraction f2)
        {
            Fraction f = Fraction.Null();

            var new_d = Mathematics.NOK(f1.denominator, f2.denominator);
            var m1 = new_d / f1.denominator;
            var m2 = new_d / f2.denominator;
            var new_n = f1.sign * m1 * f1.numerator + f2.sign * m2 * f2.numerator;


            f.sign = getSign(new_n);
            f.numerator = Math.Abs(new_n);
            f.denominator = new_d;

            //f.Rediction();
            return f;

        }

        public static Fraction operator -(Fraction f1)
        {
            Fraction f = Fraction.Null();
            f.numerator = f1.numerator;
            f.denominator = f1.denominator;
            f.sign = -f1.sign;
            return f;
        }

        public static Fraction operator -(Fraction f1, Fraction f2)
        {
            return -f2 + f1;
        }

        public static Fraction operator *(Fraction f1, Fraction f2)
        {
            Fraction f = Fraction.Null();

            f.numerator = f1.numerator * f2.numerator;
            f.denominator = f1.denominator * f2.denominator;
            f.sign = f1.sign * f2.sign;

            //f.Rediction();
            return f;

        }

        public static Fraction operator *(long i, Fraction f2)
        {
            Fraction f = new Fraction(i);

            return f * f2;
        }
        public static Fraction operator *(Fraction f, long i)
        {
            return i * f;
        }


        public static Fraction operator /(Fraction f1, Fraction f2)
        {
            Fraction f = Fraction.Null();
            Fraction f2_obr = Fraction.Null();

            if (f2.Numeretor == 0)
                throw (new Exception());
            f2_obr.numerator = f2.denominator;
            f2_obr.denominator = f2.numerator;
            f2_obr.sign = f2.sign;

            return f1 * f2_obr;
        }

        public static Fraction operator /(Fraction f1, int i)
        {
            Fraction f = new Fraction(i);
            return f1 / f;
        }

        public static Fraction operator /(int i, Fraction f1)
        {
            Fraction f = new Fraction(i);
            return f / f1;
        }

        public double toDouble()
        {
            return 1.0 * sign * numerator / denominator;
        }

    }

    public static class Mathematics
    {
        public static long NOD(long a, long b)
        {
            if (a == 0 || b == 0)
                return a + b;

            if (a < b)
            {
                a = a + b;
                b = a - b;
                a = a - b;
            }

            while (true)
            {
                a = a - (a / b) * b;
                if (a == 0) return b;
                b = b - (b / a) * a;
                if (b == 0) return a;
            }

        }

        public static long NOK(long a, long b)
        {
            return Math.Abs(a * b) / NOD(a, b);
        }

        public static int NumSize(int i)
        {

            int razm = 0;

            while (true)
            {
                if (i / Math.Pow(10, razm) >= 1 && i / Math.Pow(10, razm + 1) < 1)
                    return razm + 1;
                razm++;
            }
        }
    }
}
