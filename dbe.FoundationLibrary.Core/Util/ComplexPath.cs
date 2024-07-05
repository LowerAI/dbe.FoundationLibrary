using System;
using System.IO;

namespace HQ.Framework.Utility
{
    /// <summary>
    /// 复合路径
    /// </summary>
    public class ComplexPath
    {
        /// <summary>
        /// 物理路径
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualPath { get; set; }

        public ComplexPath(string pPath, string vPath)
        {
            this.PhysicalPath = pPath;
            this.VirtualPath = vPath;
            string folder = Path.GetDirectoryName(pPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public ComplexPath(string path)
        {
            try
            {
                if (Path.IsPathRooted(path))  //path为绝对路径
                {
                    this.PhysicalPath = path;
                    this.VirtualPath = PathSwitch.AbsolutePath2RelativePath(this.PhysicalPath);
                }
                else                          //path为相对路径
                {
                    this.VirtualPath = PathSwitch.RelativePathCorrection(path);
                    this.PhysicalPath = PathSwitch.RelativePath2AbsolutePath(this.VirtualPath);
                }

                string folder = Path.GetDirectoryName(this.PhysicalPath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}