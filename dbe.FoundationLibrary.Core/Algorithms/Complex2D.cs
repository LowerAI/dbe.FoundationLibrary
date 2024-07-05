using System;

namespace dbe.FoundationLibrary.Core.Algorithms
{
    public class Complex2D
    {
        private Complex[,] matrix;
        private int m_width;
        private int m_height;

        // width:图像宽度 height:图像高度
        public Complex2D(int width, int height)
        {
            m_width = width;
            m_height = height;
            matrix = new Complex[height, width];
        }

        public int Width { get { return m_width; } }
        public int Height { get { return m_height; } }

        /// <summary>
        /// 获取指定行的复数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Complex[] GetRow(int i)
        {
            Complex[] row = new Complex[m_width];
            for (int j = 0; j < m_width; j++)
                row[j] = matrix[i, j];
            return row;
        }

        /// <summary>
        /// 设置指定行的复数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="array"></param>
        public void SetRow(int i, Complex[] array)
        {
            for (int j = 0; j < m_width; j++)
                matrix[i, j] = array[j];
        }

        /// <summary>
        /// 获取指定列的复数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Complex[] GetColumn(int i)
        {
            Complex[] column = new Complex[m_height];
            for (int j = 0; j < m_height; j++)
                column[j] = matrix[j, i];
            return column;
        }

        /// <summary>
        /// 设置指定列的复数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="array"></param>
        public void SetColumn(int i, Complex[] array)
        {
            for (int j = 0; j < m_height; j++)
                matrix[j, i] = array[j];
        }

        /// <summary>
        /// 获取指定行和列的复数
        /// i: 第几行  j: 第几列
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public Complex GetComplex(int i, int j)
        {
            return matrix[i, j];
        }

        /// <summary>
        /// 设置指定行和列的复数
        /// i: 第几行  j: 第几列
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="src"></param>
        public void SetComplex(int i, int j, Complex src)
        {
            matrix[i, j] = src;
        }

        /// <summary>
        /// 交换两个元素
        /// i: 第几行  j: 第几列
        /// </summary>
        /// <param name="i0"></param>
        /// <param name="j0"></param>
        /// <param name="i1"></param>
        /// <param name="j1"></param>
        public void SwapComplex(int i0, int j0, int i1, int j1)
        {
            Complex tmp = matrix[i0, j0];
            matrix[i0, j0] = matrix[i1, j1];
            matrix[i1, j1] = tmp;
        }

        /// <summary>
        /// 交换行
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void SwapRow(int i, int j)
        {
            for (int k = 0; k < m_width; k++)
            {
                Complex cpx0 = matrix[i, k];
                Complex cpx1 = matrix[j, k];
                matrix[i, k] = cpx1;
                matrix[j, k] = cpx0;
            }
        }

        /// <summary>
        /// 交换列
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void SwapColumn(int i, int j)
        {
            for (int k = 0; k < m_height; k++)
            {
                Complex cpx0 = matrix[k, i];
                Complex cpx1 = matrix[k, j];
                matrix[k, i] = cpx1;
                matrix[k, j] = cpx0;
            }
        }

        public void Print(string fileName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < m_height; i++)
            {
                for (int j = 0; j < m_width; j++)
                    sb.AppendFormat("{0:G} ", Math.Floor(matrix[i, j].Real).ToString().PadRight(5));
                sb.AppendLine();
            }
            System.IO.File.WriteAllText(string.Format("D://{0}.txt", fileName), sb.ToString());
        }
    }
}