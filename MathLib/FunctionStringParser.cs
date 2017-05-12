namespace MathLib
{
    /// <summary>
    /// Provides convertation from usual function string to system function string
    /// </summary>
    static class FunctionStringParser
    {
        /// <summary>
        /// Convert function string from simple to system syntax.
        /// </summary>
        /// <param name="funcStr">The function string.</param>
        /// <returns></returns>
        public static string ToSystemSyntax(string funcStr)
        {
            funcStr = funcStr.Replace(" ", string.Empty);
            funcStr = funcStr.Replace("+", " + ");
            funcStr = funcStr.Replace("-", " - ");
            funcStr = funcStr.Replace("*", " * ");
            funcStr = funcStr.Replace("/", " / ");
            funcStr = funcStr.Replace("(", "( ");
            funcStr = funcStr.Replace(")", " )");
            funcStr = ReplacePow(funcStr);
            funcStr = funcStr.Replace("sin", "Math.Sin");
            funcStr = funcStr.Replace("cos", "Math.Cos");
            funcStr = funcStr.Replace("tg", "Math.Tan");
            funcStr = funcStr.Replace("ctg", "1/Math.Tan");
            funcStr = funcStr.Replace("ln", "Math.Log");
            funcStr = funcStr.Replace("lg", "Math.Log10");
            funcStr = funcStr.Replace("e", "Math.E");
            funcStr = funcStr.Replace("sqrt", "Math.Sqrt");

            return funcStr;
        }

        private static string ReplacePow(string str)
        {
            string leftVal, rightVal;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '^')
                {
                    leftVal = rightVal = null;
                    StrDetect(ref str, ref leftVal, i, 'l');
                    StrDetect(ref str, ref rightVal, i, 'r');
                    str = str.Replace(leftVal + "^" + rightVal, "Math.Pow(" + leftVal + "," + rightVal + ")");
                }

            }
            return str;
        }

       
        private static void StrDetect(ref string s, ref string value, int currPos, char way)
        {
            int step, index, beginBrackCnt = 0, endBrackCnt = 0;
            char beginBrack, endBrack;
            bool closeBrack = false;

            if (way == 'l')
            {
                step = -1;
                beginBrack = ')';
                endBrack = '(';
            }
            else
            {
                step = 1;
                beginBrack = '(';
                endBrack = ')';
            }

            index = currPos + step;

            do
            {
                if (s[currPos + step] == beginBrack && !closeBrack)
                {
                    if (s[index] == beginBrack)
                        beginBrackCnt++;
                    if (s[index] == endBrack)
                        endBrackCnt++;

                    if (beginBrackCnt != endBrackCnt)
                        index += step;
                    else
                    {
                        closeBrack = true;
                    }
                }
                else
                {
                    if (index == 0 || index == s.Length - 1 || s[index + step] == ' ')
                    {
                        if (way == 'l')
                            value = s.Substring(index, currPos - index);
                        else
                            value = s.Substring(currPos + 1, index - currPos);
                    }
                    else
                    {
                        index += step;
                    }
                }
            }
            while (value == null);

        }
    }
}
