using System.Reflection;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    public static class PropertyGridExtensions
    {
        /// <summary>
        /// 软刷新 - 刷新后不滚动
        /// </summary>
        /// <param name="propertyGrid"></param>
        public static void SoftRefresh(this PropertyGrid propertyGrid)
        {
            var peMain = propertyGrid.GetType().GetField("peMain", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(propertyGrid) as GridItem;
            if (peMain != null)
            {
                var refreshMethod = peMain.GetType().GetMethod("Refresh");
                if (refreshMethod != null)
                {
                    refreshMethod.Invoke(peMain, null);
                    propertyGrid.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// 收缩指定名称的分组
        /// </summary>
        /// <param name="propertyGrid"></param>
        /// <param name="groupName">指定分组的名称</param>
        public static void CollapseGridItem(this PropertyGrid propertyGrid, string groupName)
        {
            var peMain = propertyGrid.GetType().GetField("peMain", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(propertyGrid) as GridItem;
            if (peMain != null)
            {
                foreach (GridItem g in peMain.GridItems)
                {
                    if (g.GridItemType == GridItemType.Category && g.Label == groupName)
                    {
                        g.Expanded = false;
                        break;
                    }
                }
            }
        }
    }
}