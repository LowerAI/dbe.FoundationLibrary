namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// 坐标系类型
    /// </summary>
    public enum CoordinateType : int
    {
        /// <summary>
        /// 绘图坐标系：以画布中心的原点为零点坐标，左负右正下负上正
        /// </summary>
        DrawingCoordinate = 0,
        /// <summary>
        /// 位图坐标系：以位图的左上角为零点坐标，左负右正上负下正
        /// </summary>
        BitmapCoordinate,
        /// <summary>
        /// 页面坐标系：以容器控件的左上角为零点坐标，左负右正上负下正
        /// </summary>
        PageCoordinate
    }
}
