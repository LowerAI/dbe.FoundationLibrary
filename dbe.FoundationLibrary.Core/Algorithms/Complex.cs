using System;
using System.Runtime.InteropServices;

namespace dbe.FoundationLibrary.Core.Algorithms
{
    /// <summary>
    /// 复数
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Complex
    {
        private double real;
        /// <summary>
        /// 实部
        /// </summary>
        public double Real
        {
            get { return real; }
            set { real = value; }
        }

        private double imag;
        /// <summary>
        /// 虚部
        /// </summary>
        public double Imag
        {
            set { imag = value; }
            get { return imag; }
        }

        /// <summary>
        /// 量值/绝对值
        /// </summary>
        public double Magn
        {
            get
            {
                return Math.Sqrt(imag * imag + real * real);
            }
        }

        /// <summary>
        /// 相位角(保留6位小数)
        /// </summary>
        public double Phase
        {
            get
            {
                return Math.Round(Math.Atan2(real, imag), 6);
            }
        }

        //构造函数
        public Complex(double x, double y)
        {
            real = x;
            imag = y;
        }

        //重载加法
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.real + c2.real, c1.imag + c2.imag);
        }

        public static Complex operator +(double c1, Complex c2)
        {
            return new Complex(c1 + c2.real, c2.imag);
        }

        public static Complex operator +(Complex c1, double c2)
        {
            return new Complex(c1.Real + c2, c1.imag);
        }

        //重载减法
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.real - c2.real, c1.imag - c2.imag);
        }

        public static Complex operator -(double c1, Complex c2)
        {
            return new Complex(c1 - c2.real, -c2.imag);
        }

        public static Complex operator -(Complex c1, double c2)
        {
            return new Complex(c1.real - c2, c1.imag);
        }

        //重载乘法
        public static Complex operator *(Complex c1, Complex c2)
        {
            double cr = c1.real * c2.real - c1.imag * c2.imag;
            double ci = c1.imag * c2.real + c2.imag * c1.real;
            return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));
        }

        public static Complex operator *(double c1, Complex c2)
        {
            double cr = c1 * c2.real;
            double ci = c1 * c2.imag;
            return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));
        }

        public static Complex operator *(Complex c1, double c2)
        {
            double cr = c1.Real * c2;
            double ci = c1.Imag * c2;
            return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));
        }

        //重载除法
        public static Complex operator /(Complex c1, Complex c2)
        {
            if (c2.real == 0 && c2.imag == 0)
            {
                return new Complex(double.NaN, double.NaN);
            }
            else
            {
                double cr = (c1.imag * c2.imag + c2.real * c1.real) / (c2.imag * c2.imag + c2.real * c2.real);
                double ci = (c1.imag * c2.real - c2.imag * c1.real) / (c2.imag * c2.imag + c2.real * c2.real);
                return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));           //保留四位小数后输出
            }
        }

        public static Complex operator /(double c1, Complex c2)
        {
            if (c2.real == 0 && c2.imag == 0)
            {
                return new Complex(double.NaN, double.NaN);
            }
            else
            {
                double cr = c1 * c2.Real / (c2.imag * c2.imag + c2.real * c2.real);
                double ci = -c1 * c2.imag / (c2.imag * c2.imag + c2.real * c2.real);
                return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));           //保留四位小数后输出
            }
        }

        public static Complex operator /(Complex c1, double c2)
        {
            if (c2 == 0)
            {
                return new Complex(double.NaN, double.NaN);
            }
            else
            {
                double cr = c1.Real / c2;
                double ci = c1.imag / c2;
                return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));           //保留四位小数后输出
            }
        }

        //重载字符串转换方法,便于显示复数
        public override string ToString()
        {
            if (imag >= 0)
                return string.Format("{0}+i{1}", real, imag);
            else
                return string.Format("{0}-i{1}", real, -imag);
        }

        //欧拉公式
        public static Complex Exp(Complex c)
        {
            double amplitude = Math.Exp(c.real);
            double cr = amplitude * Math.Cos(c.imag);
            double ci = amplitude * Math.Sin(c.imag);
            return new Complex(Math.Round(cr, 4), Math.Round(ci, 4));//保留四位小数输出
        }
    }
}