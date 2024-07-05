using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HQ.Framework.Utility
{
    /// <summary>
    /// 路径转换
    /// </summary>
    public class PathSwitch
    {

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern bool PathRelativePathTo(
                     [Out] StringBuilder pszPath,
                      string pszFrom,
                      FileAttributes dwAttrFrom,
                      string pszTo,
                      FileAttributes dwAttrTo
        );

        /// <summary>
        /// 相对路径修正，例如"~/BackupFiles/"需要转换为"/ShipRecord/BackupFiles/"被拼接到下载链接之后才能被正确识别
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string RelativePathCorrection(string virtualPath)
        {
            if (virtualPath.IndexOf("~") == 0)
            {
                string vRootPath = HttpRuntime.AppDomainAppVirtualPath;   //网站的虚拟根路径
                virtualPath = virtualPath.Replace("~", vRootPath);
            }
            return virtualPath;
        }

        /// <summary>
        /// 把相对路径转换为绝对路径
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string RelativePath2AbsolutePath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }

        /// <summary>
        /// 把虚拟路径转换为物理路径（兼容线程调用）
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string RelativePath2AbsolutePath4Thread(string virtualPath)
        {
            string root = HttpRuntime.AppDomainAppPath;
            virtualPath = virtualPath.Replace('/', '\\');
            //~开头
            if (virtualPath.IndexOf("~") == 0)
            {
                return root.TrimEnd('\\') + virtualPath.Remove(0, 1).TrimEnd('\\') + (virtualPath.IndexOf(".") > -1 ? "" : "\\");
            }
            root += "Common";
            //.开头
            if (virtualPath == "." || virtualPath.IndexOf(".\\") == 0)
            {
                return root + virtualPath.TrimStart('.');
            }
            else
            {
                //循环处理../
                while (virtualPath.IndexOf("..") == 0)
                {
                    virtualPath = virtualPath.TrimStart('.').TrimStart('\\');
                    root = root.Substring(0, root.LastIndexOf('\\'));
                }
                return root + "\\" + virtualPath;
            }
        }

        /// <summary>
        /// 绝对路径转相对路径
        /// </summary>
        /// <param name="absolutePath">待转换的绝对路径</param>
        /// <param name="absolutePath">当前目录或文件不在网站物理目录中时</param>
        /// <returns></returns>
        public static string AbsolutePath2RelativePath(string absolutePath, string prefixPath = "/")
        {
            string relativePath = "";
            StringBuilder path = new StringBuilder(260);
            PathRelativePathTo(path, Application.StartupPath, FileAttributes.Directory, absolutePath, FileAttributes.Normal);

            relativePath = $"{prefixPath}{path.ToString()}";
            relativePath = relativePath.Replace("//", "/").Replace("\\", "/");
            return relativePath;
        }

        /// <summary>
        /// 绝对路径转换为相对路径
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string AbsolutePath2RelativePath4Thread(string absolutePath)
        {
            string root = HttpRuntime.AppDomainAppPath;
            string virtualPath = absolutePath.Replace(root, ""); //转换成相对路径
            virtualPath = virtualPath.Replace(@"\", @"/");
            virtualPath = virtualPath.IndexOf("./") == 0 ? virtualPath : "./" + virtualPath;
            return virtualPath;
        }
    }
}