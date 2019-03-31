using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFMS.API.Common.Utils
{
    public static class Utils
    {
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string RemoveSign(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            try
            {
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }
            catch
            {
                return  null;
            }

            return str;
        }

        public static List<string> SplitString2ListString(string strInput, char character)
        {
            if (!string.IsNullOrEmpty(strInput))
                return strInput.Split(character).ToList();
            return new List<string>();
        }

        public static List<string> convert2ListString(string strInput, char character)
        {
            if (!string.IsNullOrEmpty(strInput))
            {
                return strInput.Split(character).ToList();
            }
            return new List<string>();
        }
    }
}
