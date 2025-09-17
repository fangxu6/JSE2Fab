using System;

namespace WindowsFormTool
{
    public class JSECommonUtil
    {

        /// <summary>
        /// 判断一个字符是是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            return int.TryParse(str, out _);
        }

        public static bool OverIndexOfXLimit(string str)
        {

            if (Convert.ToInt32(str) < -1000)
            {
                return true;
            }
            if (Convert.ToInt32(str) > 1000)
            {
                return true;
            }
            return false;
        }

    }
}