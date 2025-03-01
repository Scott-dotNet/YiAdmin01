using System.Text;
using Microsoft.AspNetCore.Http;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Global;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Enum;
using YiAdmin01.Model;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace YiAdmin01.BLL
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileHelper
    {
        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void CreateFile(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            using StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);
            {
                streamWriter.Write(content);
            }
        }
        #endregion

        #region 上传单个文件
        /// <summary>
        /// 上传单个文件
        /// </summary>
        /// <param name="fileModule">类型</param>
        /// <param name="files">文件</param>
        /// <returns></returns>
        public async static Task<TData<string>> UploadFile(int fileModule, IFormFileCollection files)
        {
            string dirModule = string.Empty;
            TData<string> obj = new();

            if (files == null || files.Count == 0)
            {
                obj.Message = "请先选择文件！";
                return obj;
            }
            if (files.Count > 1)
            {
                obj.Message = "一次只能上传一个文件！";
                return obj;
            }

            TData objCheck = null;
            IFormFile file = files[0];
            // 上传文件类型
            switch (fileModule)
            {
                case (int)UploadFileType.Portrait: // 头像，1
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }
                    dirModule = UploadFileType.Portrait.ToString();
                    break;

                case (int)UploadFileType.Import: // 导入的表格文件，10
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".xls|.xlsx");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }
                    dirModule = UploadFileType.Import.ToString();
                    break;

                default:
                    obj.Message = "请指定上传到的模块";
                    return obj;
            }
            // 文件扩展名
            string fileExtension = TextHelper.GetCustomValue(Path.GetExtension(file.FileName), ".png");
            // 新文件名
            string newFileName = SecurityHelper.GetGuid(true) + fileExtension;
            // 文件夹路径 Resource/10/yyyy/MM/dd/
            string dir = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd").Replace('-', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            // 绝对路径
            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);
            // 绝对文件名
            string absoluteFileName = string.Empty;

            if (!Directory.Exists(absoluteDir))
            {
                Directory.CreateDirectory(absoluteDir);
            }
            absoluteFileName = absoluteDir + newFileName;
            try
            {
                using (FileStream fs = File.Create(absoluteFileName))
                {
                    await file.CopyToAsync(fs);//将文件复制到流
                    fs.Flush();
                }
                // 返回文件路径
                obj.Data = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(dir) + newFileName;
                // 返回文件名
                obj.Message = Path.GetFileNameWithoutExtension(TextHelper.GetCustomValue(file.FileName, newFileName));
                // 返回文件大小
                obj.Description = (file.Length / 1024).ToString(); // KB
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }
            return obj;
        }
        #endregion

        #region 删除单个文件
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="fileModule"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = new TData<string>();
            string dirModule = fileModule.GetDescriptionByEnum<UploadFileType>();

            if (string.IsNullOrEmpty(filePath))
            {
                obj.Message = "请先选择文件！";
                return obj;
            }
            // 文件路径
            filePath = FilterFilePath(filePath);
            //  文件夹路径 Resource/10/filePath
            filePath = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + filePath;
            // 绝对路径
            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, filePath);
            try
            {
                if (File.Exists(absoluteDir))
                {
                    File.Delete(absoluteDir);// 删除文件
                }
                else
                {
                    obj.Message = "文件不存在";
                }
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }
            return obj;
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        public static TData<FileContentResult> DownloadFile(string filePath, int delete)
        {
            filePath = FilterFilePath(filePath);
            if (!filePath.StartsWith("wwwroot") && !filePath.StartsWith("Resource"))
            {
                throw new Exception("非法访问");
            }
            TData<FileContentResult> obj = new TData<FileContentResult>();
            // 文件绝对路径
            string absoluteFilePath = GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + filePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            byte[] fileBytes = File.ReadAllBytes(absoluteFilePath);
            if (delete == 1)
            {
                File.Delete(absoluteFilePath);// 删除文件
            }
            // md5 值
            string fileNamePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string title = fileNameWithoutExtension;
            if (fileNameWithoutExtension.Contains("_"))
            {
                title = fileNameWithoutExtension.Split('_')[1].Trim();
            }
            // 文件扩展名
            string fileExtensionName = Path.GetExtension(filePath);
            obj.Data = new FileContentResult(fileBytes, "application/octet-stream")
            {// 设置文件下载名
                FileDownloadName = string.Format("{0}_{1}{2}", fileNamePrefix, title, fileExtensionName)
            };
            obj.Tag = 1;
            return obj;
        }
        #endregion 

        #region 辅助方法
        /// <summary>
        /// 检查文件扩展名
        /// </summary>
        /// <param name="fileExtension">扩展名</param>
        /// <param name="allowExtension">允许的扩展名</param>
        /// <returns></returns>
        public static TData CheckFileExtension(string fileExtension, string allowExtension)
        {
            TData obj = new TData();
            string[] allowArr = TextHelper.SplitToArray<string>(allowExtension.ToLower(), '|');
            if (allowArr.Where(p => p.Trim() == fileExtension.ParseToString().ToLower()).Any())
            { //匹配扩展名
                obj.Tag = 1;
            }
            else
            {
                obj.Message = "只有文件扩展名是 " + allowExtension + " 的文件才能上传";
            }
            return obj;
        }

        /// <summary>
        /// 目录转换为Http格式
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string ConvertDirectoryToHttp(string directory)
        {
            directory = directory.ParseToString();
            directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return directory;
        }

        /// <summary>
        /// 过滤文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FilterFilePath(string filePath)
        {
            filePath = filePath.Replace("../", string.Empty);
            filePath = filePath.Replace("..", string.Empty);
            filePath = filePath.TrimStart('/');
            return filePath;
        }

        #endregion

    }
}