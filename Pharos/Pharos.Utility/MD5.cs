using AX.CSF.DBHelper;

namespace Pharos.Utility
{
    /// <summary>
    /// 安全类
    /// </summary>
    public class Security
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <returns>返回已加密的MD5</returns>
        public static string MD5_Encrypt(string text)
        {
            //return text;    //明文临时，用于初期快速调试
            return MD5.Encrypt(text, MD5.ShiftMode.None, MD5.ReturnType.FullLower); //正式加密
        }
    }
}
