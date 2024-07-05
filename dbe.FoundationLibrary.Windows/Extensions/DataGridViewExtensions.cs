using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    public static class DataGridViewExtensions
    {
        /// <summary>
        /// 返回鼠标Y轴所在行的索引
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="mouseLocation_Y"></param>
        /// <returns></returns>
        public static int GetRowIndexAt(this DataGridView dgv, int mouseLocation_Y)
        {
            if (dgv.FirstDisplayedScrollingRowIndex < 0)
            {
                return -1;
            }

            if (dgv.ColumnHeadersVisible == true && mouseLocation_Y <= dgv.ColumnHeadersHeight)
            {
                return -1;
            }

            int index = dgv.FirstDisplayedScrollingRowIndex;
            int displayedCount = dgv.DisplayedRowCount(true);

            for (int k = 1; k <= displayedCount; k++)
            {
                if (dgv.Rows[index].Visible)
                {
                    var rect = dgv.GetRowDisplayRectangle(index, true);  // 取该区域的显示部分区域
                    if (rect.Top <= mouseLocation_Y && mouseLocation_Y < rect.Bottom)
                    {
                        return index;
                    }
                    //k++;
                }
                index++;
            }
            return -1;
        }
    }
}