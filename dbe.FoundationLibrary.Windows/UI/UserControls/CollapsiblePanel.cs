using System;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.UserControls
{
    public partial class CollapsiblePanel : UserControl
    {
        /// <summary>
        /// 表格布局面板的原始高度
        /// </summary>
        private int TableLayoutPanelHeight { get; set; }

        /// <summary>
        /// 内容面板的原始高度
        /// </summary>
        private float ContenPanelHeight { get; set; }

        public CollapsiblePanel()
        {
            InitializeComponent();
        }

        private void CollapsiblePanel_Load(object sender, EventArgs e)
        {
            TableLayoutPanelHeight = tableLayoutPanel.Height;
            ContenPanelHeight = tableLayoutPanel.RowStyles[1].Height;
        }

        private void btn_Toggle_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.RowStyles[1].Height == 0)
            {
                tableLayoutPanel.RowStyles[1].Height = ContenPanelHeight;
                tableLayoutPanel.Height = TableLayoutPanelHeight;
                btn_Toggle.Image = Properties.Resources.arrowRight;
            }
            else
            {
                tableLayoutPanel.RowStyles[1].Height = 0;
                tableLayoutPanel.Height = (int)tableLayoutPanel.RowStyles[0].Height;
                btn_Toggle.Image = Properties.Resources.arrowDown;
            }
            this.Parent.Invalidate();
            this.Parent.Refresh();
        }
    }
}
