using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using IdeaEngineAuth.Models;

namespace IdeaEngineAuth.Data
{
    public interface RevisionOf<T, R> where R : RevisionOf<T, R>
    {
        R build(Revision revision, T t);
    }

    public static class DigitUtils
    {
        public static long? parseOrNull(String str)
        {
            String digits = DigitUtils.parseDigits(str);
            if (digits.Length.Equals(10))
            {
                return long.Parse(digits);
            }
            return null;
        }

        public static String parseDigits(String str)
        {
            String result = "";
            if (str != null)
            {
                foreach (char character in str)
                {
                    if ("1234567890".Contains(character))
                    {
                        result += character;
                    }
                }
            }
            return result;
        }
    }
}