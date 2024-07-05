using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;

namespace dbe.FoundationLibrary.Core.Drawing
{
    /// <summary>
    /// 表示定义二维平面中的点的一对值为double类型有序x和y坐标。
    /// </summary>
    [Serializable]
    public struct PointD : IEquatable<PointD>
    {
        /// <summary>
        /// 创建<see cref=System.Drawing.PointD/>类的新实例，其中成员数据未初始化。
        /// </summary>
        public static readonly PointD Empty;
        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='PointD'/> class with the specified coordinates.
        /// </summary>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='PointD'/> struct from the specified
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public PointD(Vector2 vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector2"/> from this <see cref="System.Drawing.PointD"/>.
        /// </summary>
        public Vector2 ToVector2() => new Vector2((float)x, (float)y);

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointD'/> is empty.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => x == 0d && y == 0d;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='PointD'/>.
        /// </summary>
        public double X
        {
            get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='PointD'/>.
        /// </summary>
        public double Y
        {
            get => y;
            set => y = value;
        }

        /// <summary>
        /// 给X重新赋值
        /// </summary>
        /// <param name="newX"></param>
        /// <returns></returns>
        public void SetX(double newX)
        {
            this = new PointD(newX, Y);
        }

        /// <summary>
        /// 给X重新赋值并返回该副本
        /// </summary>
        /// <param name="newX"></param>
        /// <returns></returns>
        public PointD SetXToCopy(double newX)
        {
            return new PointD(newX, Y);
        }

        /// <summary>
        /// 给Y重新赋值
        /// </summary>
        /// <param name="newY"></param>
        /// <returns></returns>
        public void SetY(double newY)
        {
            this = new PointD(X, newY);
        }

        /// <summary>
        /// 给Y重新赋值并返回该副本
        /// </summary>
        /// <param name="newY"></param>
        /// <returns></returns>
        public PointD SetYToCopy(double newY)
        {
            return new PointD(X, newY);
        }

        /// <summary>
        /// Converts the specified <see cref="System.Drawing.PointD"/> to a <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public static explicit operator Vector2(PointD point) => point.ToVector2();

        /// <summary>
        /// Converts the specified <see cref="System.Numerics.Vector2"/> to a <see cref="System.Drawing.PointD"/>.
        /// </summary>
        public static explicit operator PointD(Vector2 vector) => new PointD(vector);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD operator +(PointD pt, Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD operator -(PointD pt, Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator +(PointD pt, SizeF sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator -(PointD pt, SizeF sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='PointD'/> objects. The result specifies whether the values of the
        /// <see cref='System.Drawing.PointD.X'/> and <see cref='System.Drawing.PointD.Y'/> properties of the two
        /// <see cref='PointD'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='PointD'/> objects. The result specifies whether the values of the
        /// <see cref='System.Drawing.PointD.X'/> or <see cref='System.Drawing.PointD.Y'/> properties of the two
        /// <see cref='PointD'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointD left, PointD right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD Add(PointD pt, Size sz) => new PointD(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, Size sz) => new PointD(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD Add(PointD pt, SizeF sz) => new PointD(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, SizeF sz) => new PointD(pt.X - sz.Width, pt.Y - sz.Height);

        public override bool Equals(object obj) => obj is PointD && Equals((PointD)obj);

        public bool Equals(PointD other) => this == other;

        //public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        public override string ToString() => $"{{X={x}, Y={y}}}";
    }
}