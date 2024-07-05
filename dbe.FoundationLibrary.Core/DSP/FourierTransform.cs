using dbe.FoundationLibrary.Core.Algorithms;

using System;

namespace dbe.FoundationLibrary.Core.DSP
{
    /// <summary>
    /// 傅里叶变换
    /// 参考链接：
    ///    二维离散傅里叶变换(C#版) http://www.devacg.com/?post=1358
    /// </summary>
    public sealed class FourierTransform
    {
        /// <summary>
        /// 快速傅里叶变换
        /// </summary>
        /// <param name="TD2FD"></param>
        public static void FFT(Complex[] TD2FD)
        {
            FFT_Core(TD2FD, WT_LUT(TD2FD.Length, 1));
        }

        /// <summary>
        /// 快速傅里叶变换(二维)
        /// </summary>
        /// <param name="TD2FD"></param>
        public static void FFT2(Complex2D TD2FD)
        {
            //对每一行做FFT
            for (int i = 0; i < TD2FD.Height; i++)
            {
                Complex[] row = TD2FD.GetRow(i);
                FFT(row);
                TD2FD.SetRow(i, row);
            }

            //对每一列做FFT
            for (int i = 0; i < TD2FD.Width; i++)
            {
                Complex[] column = TD2FD.GetColumn(i);
                FFT(column);
                TD2FD.SetColumn(i, column);
            }
        }

        /// <summary>
        /// 快速傅里叶逆变换
        /// </summary>
        /// <param name="FD2TD"></param>
        public static void IFFT(Complex[] FD2TD)
        {
            //做FFT变换
            Complex[] WT = WT_LUT(FD2TD.Length, -1);
            FFT_Core(FD2TD, WT);
            //除以N
            for (int i = 0; i < FD2TD.Length; i++)
            {
                FD2TD[i].Real /= FD2TD.Length;
                FD2TD[i].Imag /= FD2TD.Length;
            }
        }

        /// <summary>
        /// 快速傅里叶逆变换(二维)
        /// </summary>
        /// <param name="FD2TD"></param>
        public static void IFFT2(Complex2D FD2TD)
        {
            //对每一行做IFFT
            for (int i = 0; i < FD2TD.Height; i++)
            {
                Complex[] row = FD2TD.GetRow(i);
                IFFT(row);
                FD2TD.SetRow(i, row);
            }

            //对每一列做IFFT
            for (int i = 0; i < FD2TD.Width; i++)
            {
                Complex[] column = FD2TD.GetColumn(i);
                IFFT(column);
                FD2TD.SetColumn(i, column);
            }
        }

        /// <summary>
        /// 将直流分量移到频谱图的中心
        /// </summary>
        /// <param name="complex2D"></param>
        public static void FFT2Shift(Complex2D complex2D)
        {
            int halfH = complex2D.Height / 2;
            int halfW = complex2D.Width / 2;
            //将图像四个象限区域按对角线交换
            for (int i = 0; i < halfH; i++)
            {
                for (int j = 0; j < complex2D.Width; j++)
                {
                    if (j < halfW)
                        complex2D.SwapComplex(i, j, i + halfH, j + halfW);
                    else
                        complex2D.SwapComplex(i, j, i + halfH, j - halfW);
                }
            }
        }

        /// <summary>
        /// 高通滤波
        /// </summary>
        /// <param name="complex2D"></param>
        public static void HighPassFilting(Complex2D complex2D)
        {
            int halfH = complex2D.Height / 2;
            int halfW = complex2D.Width / 2;
            //这里的数值可根据效果调整
            int centerH = 4;
            int centerW = 4;
            //在中间挖个洞(挖圆形效果更好,这里挖矩形方便写代码)
            for (int i = halfH - centerH; i < halfH + centerH; i++)
            {
                for (int j = halfW - centerW; j < halfW + centerW; j++)
                {
                    Complex cpx = complex2D.GetComplex(i, j);
                    cpx.Real = 0;
                    cpx.Imag = 0;
                    complex2D.SetComplex(i, j, cpx);
                }
            }
        }

        /// <summary>
        /// 低通滤波
        /// </summary>
        /// <param name="complex2D"></param>
        public static void LowPassFilting(Complex2D complex2D)
        {
            int halfH = complex2D.Height / 2;
            int halfW = complex2D.Width / 2;
            //这里的数值可根据效果调整
            int centerH = 32;
            int centerW = 32;
            for (int i = 0; i < complex2D.Height; i++)
            {
                for (int j = 0; j < complex2D.Width; j++)
                {
                    if (i < halfH - centerH || i > halfH + centerH ||
                        j < halfW - centerW || j > halfW + centerW)
                    {
                        Complex cpx = complex2D.GetComplex(i, j);
                        cpx.Real = 0;
                        cpx.Imag = 0;
                        complex2D.SetComplex(i, j, cpx);
                    }
                }
            }
        }

        /// <summary>
        /// 返回旋转因子查询表<see cref="twiddle factor lookup table">
        /// </summary>
        /// <param name="N"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private static Complex[] WT_LUT(int N, int flag = 1)
        {
            Complex[] WT = new Complex[N];
            for (int i = 0; i < N; i++)
            {
                Complex cpx_wt = new Complex();
                float angle = (float)(-i * Math.PI * 2 / N);
                cpx_wt.Real = (float)Math.Cos(angle);
                //IFFT flag=-1, FFT flag=1
                cpx_wt.Imag = flag * (float)Math.Sin(angle);
                WT[i] = cpx_wt;
            }
            return WT;
        }

        /// <summary>
        /// 快速傅里叶变换的核心算法
        /// </summary>
        /// <param name="TD2FD"></param>
        /// <param name="WT"></param>
        private static void FFT_Core(Complex[] TD2FD, Complex[] WT)
        {
            int power = (int)Math.Log(TD2FD.Length, 2);
            int butterfly;
            int p, s;
            Complex x1, x2, wt;

            //倒位排序
            BitReverse(TD2FD);

            //蝶形运算
            for (int k = 0; k < power; k++) //级数
            {
                for (int j = 0; j < 1 << power - k - 1; j++) //组数
                {
                    //每组有几个元素
                    butterfly = 1 << k + 1;
                    //计算参与蝶形运算的两个元素的索引
                    p = j * butterfly;
                    s = p + butterfly / 2;
                    for (int i = 0; i < butterfly / 2; i++) //蝶形运算次数
                    {
                        x1 = TD2FD[i + p];
                        x2 = TD2FD[i + s];
                        wt = WT[i * TD2FD.Length / butterfly];
                        TD2FD[i + p] = x1 + x2 * wt;
                        TD2FD[i + s] = x1 - x2 * wt;
                    }
                }
            }
        }

        /// <summary>
        /// 倒位排序——雷德算法
        /// </summary>
        /// <param name="array"></param>
        private static void BitReverse(Complex[] array)
        {
            //倒位排序原理
            //0   1   2   3   4   5   6   7   十进制
            //000 001 010 011 100 101 110 111 十进制对应的二进制
            //000 100 010 110 001 101 011 111 码位反转
            //0   4   2   6   1   5   3   7   码位反转后对应的十进制

            int i, j, k;
            int N = array.Length;
            Complex temp;
            j = 0;

            for (i = 0; i < N - 1; i++)
            {
                if (i < j)
                {
                    temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
                // 求j的下一个倒序位
                // N/2的二进制最高位为1，其他位都为0
                // 判断最高位是否为1，可与N/2进行比较
                // 判断次高位是否为1，可与N/4进行比较
                k = N >> 1;
                //如果k<=j,表示j的最高位为1
                while (k <= j)
                {
                    //当k<=j时，需要将最高位变为0
                    j = j - k;
                    //判断次高位是否为1,依次类推，逐个比较，直到j某个位为0
                    k >>= 1;
                }
                j = j + k;//将0变为1
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="TD2FD"></param>
        public static void Print(Complex[] TD2FD)
        {
            for (int i = 0; i < TD2FD.Length; i++)
            {
                Console.WriteLine(TD2FD[i].ToString());
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 离散时间傅里叶变换
        /// </summary>
        /// <param name="dft_in">振动数据</param>
        /// <returns>振幅数据</returns>
        public static double[] DFT(int N, double[] dft_in)
        {
            //int N = dft_in.Length;// 根据采样大小/间隔时间得到的采样点数
            //Complex[] part = new Complex[N];
            Complex[] dft_out = new Complex[N];
            double[] amp = new double[N];

            for (int i = 0; i < N; i++)
            {
                for (int n = 0; n < N; n++)
                {
                    var arg = 2 * Math.PI * i * n / N;
                    //欧拉公式 cos(x)+jsin(x)
                    var real = Math.Cos(arg) * dft_in[n];
                    var imag = -Math.Sin(arg) * dft_in[n];

                    dft_out[i].Real += real;
                    dft_out[i].Imag += imag;
                }
                amp[i] = Math.Sqrt(dft_out[i].Real * dft_out[i].Real + dft_out[i].Imag * dft_out[i].Imag); // 振幅
            }
            return amp;
        }
    }
}