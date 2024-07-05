using System;
using System.Diagnostics;

using sn = System.Numerics;

namespace dbe.FoundationLibrary.Core.DSP
{
    /// <summary>
    /// 参考链接：
    /// C#中实现FFT的两种方法_zhoudapeng01的博客-CSDN博客_c# fft https://blog.csdn.net/zhoudapeng01/article/details/103685352
    /// 方法一：
    ///     不依赖C#中的Complex，需要实现计算过程的每一步详细步骤。
    ///     输入序列长度为2的N次幂，使用前需先定义序列长度：
    ///     FFT filter = new FFT(256);
    ///     filter.fft(x, y) 其中x为实部y为虚部，计算后x为FFT后的实部，y为FFT后的虚部。
    /// </summary>
    public class FFT1
    {
        int n, m;
        // Lookup tables. Only need to recompute when size of FFT changes.
        double[] cos;
        double[] sin;

        public FFT1(int n)
        {
            this.n = n;
            m = (int)(Math.Log(n) / Math.Log(2));

            // Make sure n is a power of 2
            if (n != 1 << m)
                Console.Out.WriteLine("FFT length must be power of 2");

            // precompute tables
            cos = new double[n / 2];
            sin = new double[n / 2];

            for (int i = 0; i < n / 2; i++)
            {
                cos[i] = Math.Cos(-2 * Math.PI * i / n);
                sin[i] = Math.Sin(-2 * Math.PI * i / n);
            }
        }

        /// <summary>
        /// x为实部y为虚部
        /// </summary>
        /// <param name="x">实部</param>
        /// <param name="y">虚部</param>
        public void fft(double[] x, double[] y)
        {
            int i, j, k, n1, n2, a;
            double c, s, t1, t2;

            // Bit-reverse
            j = 0;
            n2 = n / 2;
            for (i = 1; i < n - 1; i++)
            {
                n1 = n2;
                while (j >= n1)
                {
                    j = j - n1;
                    n1 = n1 / 2;
                }
                j = j + n1;

                if (i < j)
                {
                    t1 = x[i];
                    x[i] = x[j];
                    x[j] = t1;
                    t1 = y[i];
                    y[i] = y[j];
                    y[j] = t1;
                }
            }

            // FFT
            n1 = 0;
            n2 = 1;

            for (i = 0; i < m; i++)
            {
                n1 = n2;
                n2 = n2 + n2;
                a = 0;

                for (j = 0; j < n1; j++)
                {
                    c = cos[a];
                    s = sin[a];
                    a += 1 << m - i - 1;

                    for (k = j; k < n; k = k + n2)
                    {
                        t1 = c * x[k + n1] - s * y[k + n1];
                        t2 = s * x[k + n1] + c * y[k + n1];
                        x[k + n1] = x[k] - t1;
                        y[k + n1] = y[k] - t2;
                        x[k] = x[k] + t1;
                        y[k] = y[k] + t2;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 参考链接：
    /// C#中实现FFT的两种方法_zhoudapeng01的博客-CSDN博客_c# fft https://blog.csdn.net/zhoudapeng01/article/details/103685352
    /// 方法二：
    ///     傅里叶变换的计算中需要用到复数，考虑使用C#中的复数直接计算的方式实现
    ///     FFT filter = new FFT(256);
    ///     filter.DoFFT(x) ; x为输入参数(Complex[] 类型)，计算后x为FFT后的结果。
    /// </summary>
    public class FFT2
    {
        #region    字段 start
        private sn.Complex[] W;//定义旋转因子
        private int N;
        #endregion 字段 end

        #region    构造函数 start
        /// <summary>
        /// 构造函数用于初始化旋转因子。
        /// </summary>
        public FFT2(int N)
        {
            W = initW(N);
            this.N = N;
        }
        #endregion 构造函数 end

        #region    公开函数 start
        /// <summary>
        /// 快速傅立叶正变换函数
        /// W：输入旋转因子序列数组名
        /// x: 输入信号序列数组名
        /// </summary>
        public void DoFFT(sn.Complex[] x)
        {
            int i = 0, j = 0, k = 0, l = 0;
            sn.Complex up, down, product;
            ReverseOder(x);         // 对输入序列进行倒序
            for (i = 0; i < Math.Log(N, 2); i++)            // 外循环，变换级数循环
            {
                l = 1 << i;
                for (j = 0; j < N; j += 2 * l)                       // 中间循环，旋转因子循环
                {
                    for (k = 0; k < l; k++)                          // 内循环，序列循环
                    {
                        product = x[j + k + l] * W[N * k / 2 / l];
                        up = x[j + k] + product;
                        down = x[j + k] - product;
                        x[j + k] = up;
                        x[j + k + l] = down;
                    }
                }
            }
        }
        #endregion 公开函数 end

        #region    私有函数 start
        /// <summary>
        /// 倒序函数
        /// W：输入旋转因子序列长度
        /// </summary>
        private sn.Complex[] initW(int N)
        {
            sn.Complex[] W = new sn.Complex[N];
            for (int i = 0; i < N; i++)
            {
                double real = Math.Cos(2 * Math.PI * i / N);        //旋转因子实部展开
                double imag = -1 * Math.Sin(2 * Math.PI * i / N);   //旋转因子虚部展开
                W[i] = new sn.Complex(real, imag);
            }
            return W;
        }

        /// <summary>
        /// 倒序变换
        /// </summary>
        /// <param name="x"></param>
        private void ReverseOder(sn.Complex[] x)
        {
            sn.Complex temp;
            int i = 0, j = 0, k = 0;
            double t;
            for (i = 0; i < N; i++)
            {
                k = i; j = 0;
                t = Math.Log(N, 2);
                while (t-- > 0)
                {
                    j = j << 1;
                    j |= k & 1;
                    k = k >> 1;
                }
                if (j > i)
                {
                    temp = x[i];
                    x[i] = x[j];
                    x[j] = temp;
                }
            }
        }
        #endregion 私有函数 end
    }

    /// <summary>
    /// 参考链接：
    /// 2021-11-01-FFT心得（皮毛）--运用C#实现FFT_Joy_ou悠的博客-CSDN博客_c# fft https://blog.csdn.net/weixin_49079904/article/details/121077640
    /// </summary>
    public class FFT
    {
        /// <summary>
        /// 快速傅里叶变换（当信号源长度等于2^N时，结果与dft相同，当长度不等于2^N时，先在尾部补零，所以计算结果与dft不同）
        /// 返回幅值数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[] fft(double[] array)
        {
            array = FillArray(array);
            int N = array.Length;
            Debug.WriteLine($"补充后数据长度：{N}");
            //生成WN表，以免运行时进行重复计算
            sn.Complex[] WN = new sn.Complex[N];
            for (int i = 0; i < N / 2; i++)
            {
                //cos(-x) = cos(x) sin(-x) = -sin(x)
                //WN[i] = new Complex(Math.Cos(-2 * Math.PI / N * i), 1 * Math.Sin(-2 * Math.PI / N * i));
                WN[i] = new sn.Complex(Math.Cos(2 * Math.PI / N * i), -1 * Math.Sin(2 * Math.PI / N * i));
            }

            int stageNum = ReLog2N(N);
            int[] stage = new int[stageNum];
            stage[0] = 0;
            for (int i = 1; i < stageNum; i++)
            {
                stage[i] = Convert.ToInt32(Math.Round(Math.Pow(2, stageNum - 1 - i)));
            }
            //重排数据
            sn.Complex[] Register = new sn.Complex[N];
            for (int i = 0; i < N; i++)
            {
                int index = ReArrange(i, stageNum);
                Register[i] = new sn.Complex(array[index], 0);
            }
            //蝶形运算
            sn.Complex[] p = new sn.Complex[N];
            sn.Complex[] q = new sn.Complex[N];
            sn.Complex[] w = new sn.Complex[N];
            int group = N;
            for (int i = 0; i < stageNum; i++)
            {
                group = group >> 1;
                int subnum = N / group;
                for (int k = 0; k < group; k++)
                {
                    for (int n = 0; n < subnum / 2; n++)
                    {
                        p[k * subnum + n] = p[k * subnum + n + subnum / 2] = Register[k * subnum + n];
                        w[k * subnum + n] = WN[stage[i] * n];
                    }
                    for (int n = subnum / 2; n < subnum; n++)
                    {
                        q[k * subnum + n] = q[k * subnum + n - subnum / 2] = Register[k * subnum + n];
                        w[k * subnum + n] = -1 * w[n - subnum / 2];
                    }
                }
                for (int k = 0; k < N; k++)
                {
                    Register[k] = p[k] + w[k] * q[k];
                }
            }
            double[] dest = new double[N];
            for (int k = 0; k < N; k++)
            {
                //dest[k] = Register[k].Modulus();
                dest[k] = Register[k].Magnitude;
            }
            return dest;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[][] fft_D(double[] array)
        {
            array = FillArray(array);
            int N = array.Length;
            Debug.WriteLine($"补零后数据长度：{N}");
            //生成WN表，以免运行时进行重复计算
            sn.Complex[] WN = new sn.Complex[N];
            for (int i = 0; i < N / 2; i++)
            {
                WN[i] = new sn.Complex(Math.Cos(2 * Math.PI / N * i), -1 * Math.Sin(2 * Math.PI / N * i));
            }
            int stageNum = ReLog2N(N);
            int[] stage = new int[stageNum];
            stage[0] = 0;
            for (int i = 1; i < stageNum; i++)
            {
                stage[i] = Convert.ToInt32(Math.Round(Math.Pow(2, stageNum - 1 - i)));
            }
            //重排数据
            sn.Complex[] Register = new sn.Complex[N];
            for (int i = 0; i < N; i++)
            {
                int index = ReArrange(i, stageNum);
                Register[i] = new sn.Complex(array[index], 0);
            }
            //蝶形运算
            sn.Complex[] p = new sn.Complex[N];
            sn.Complex[] q = new sn.Complex[N];
            sn.Complex[] w = new sn.Complex[N];
            int group = N;
            double[][] fftdata = new double[N][];
            for (int i = 0; i < stageNum; i++)
            {
                group = group >> 1;
                int subnum = N / group;
                for (int k = 0; k < group; k++)
                {
                    for (int n = 0; n < subnum / 2; n++)
                    {
                        p[k * subnum + n] = p[k * subnum + n + subnum / 2] = Register[k * subnum + n];
                        w[k * subnum + n] = WN[stage[i] * n];
                    }
                    for (int n = subnum / 2; n < subnum; n++)
                    {
                        q[k * subnum + n] = q[k * subnum + n - subnum / 2] = Register[k * subnum + n];
                        w[k * subnum + n] = -1 * w[n - subnum / 2];
                    }
                }

                for (int k = 0; k < N; k++)
                {
                    fftdata[k] = new double[2];
                    Register[k] = p[k] + w[k] * q[k];
                    fftdata[k][0] = Register[k].Real;
                    fftdata[k][1] = Register[k].Imaginary;
                }
            }
            return fftdata;
        }

        /// <summary>
        /// 返回扩展长度(补零)后的幂次(2^n的n)
        /// 由于fft要求长度为2^n，所以用此函数来获取所需长度
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int ReLog2N(int count)
        {
            int log2N = 0;
            uint mask = 0x80000000;
            for (int i = 0; i < 32; i++)
            {
                if (0 != (mask >> i & count))
                {
                    if (mask >> i == count) log2N = 31 - i;
                    else log2N = 31 - i + 1;
                    break;
                }
            }
            return log2N;
        }

        /// <summary>
        /// 数据补零
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static double[] FillArray(double[] array)
        {
            //补零后长度的幂次
            int relog2N = ReLog2N(array.Length);

            int bitlenghth = relog2N;
            int N = 0x01 << bitlenghth;
            double[] ret = new double[N];
            for (int i = 0; i < N; i++)
            {
                if (i < array.Length)
                    ret[i] = array[i];
                else ret[i] = 0;
            }
            return ret;
        }

        /// <summary>
        /// 码位倒置-获取按位逆序，bitlenght为数据长度
        /// fft函数内使用，处理结果时使用
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="bitlenght"></param>
        /// <returns></returns>
        private static int ReArrange(int dat, int bitlenght)
        {
            int ret = 0;
            for (int i = 0; i < bitlenght; i++)
            {
                //若bitlenght = 3 dat 和 0000 0001 << i按位做与运算 ！= 0
                //ret = ret | ((0x01 << (bitlenght - 1)) >> i)
                //搞不清楚时可以将dat = 011 与 bitlenght = 3代入验算
                if (0 != (dat & 0x01 << i))
                {
                    ret |= 0x01 << bitlenght - 1 >> i;
                }
            }
            return ret;
        }
    }
}