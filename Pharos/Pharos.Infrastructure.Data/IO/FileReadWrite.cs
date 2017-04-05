using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pharos.Infrastructure.Data.IO
{
    /// <summary>
    /// 处理多线程读写
    /// </summary>
    public static class FileReadWrite
    {
        private static Dictionary<string, object> lockObjects = new Dictionary<string, object>();
        private static object lockDict = new object();
        public static void Write(string path, FileMode mode, Action<FileStream> writeAction)
        {
            try
            {
                var lockobj = new object();
                lock (lockDict)
                {
                    if (!lockObjects.ContainsKey(path))
                    {
                        lockObjects.Add(path, lockobj);
                    }
                    else
                    {
                        lockobj = lockObjects[path];
                    }
                }
                lock (lockobj)
                {
                    var dir = Path.GetDirectoryName(path);
                    var identities = new string[] { "Everyone", "Users" };
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        SecurityControl.AddSecurityControllToFolder(identities, dir);
                    }
                    else
                    {
                        SecurityControl.AddSecurityControllToFolder(identities, dir);
                    }
                    using (FileStream fsWrite = new FileStream(path, mode, FileAccess.ReadWrite, FileShare.None))
                    {
                        writeAction(fsWrite);
                    }
                }
            }
            catch (System.Security.SecurityException)
            {
                throw new System.Security.SecurityException("权限未配置或异常,请检查相关文件及文件夹权限!");
            }
            catch (System.UnauthorizedAccessException)
            {
                throw new System.UnauthorizedAccessException("只读文件未能写入,请检查相关文件及文件夹读写权限!");
            }
        }
        public static T Read<T>(string path, FileMode mode, Func<FileStream, T> readAction)
        {
            try
            {
                var lockobj = new object();
                var identities = new string[] { "Everyone", "Users" };

                lock (lockDict)
                {
                    if (!lockObjects.ContainsKey(path))
                    {
                        lockObjects.Add(path, lockobj);
                    }
                    else
                    {
                        lockobj = lockObjects[path];
                    }
                }
                lock (lockobj)
                {
                    SecurityControl.AddSecurityControllToFile(identities, path);
                    using (FileStream fsRead = new FileStream(path, mode))
                    {
                        return readAction(fsRead);
                    }
                }
            }
            catch (System.Security.SecurityException)
            {
                throw new System.Security.SecurityException("权限未配置或异常,请检查相关文件及文件夹权限!");
            }
            catch (System.UnauthorizedAccessException)
            {
                throw new System.UnauthorizedAccessException("只读文件未能写入,请检查相关文件及文件夹读写权限!");
            }
        }

    }
}
