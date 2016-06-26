using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib
{
    static class FuncStrParsing
    {
        public static string ToSystemSyntax(string funcStr)
        {
            funcStr = funcStr.Replace(" ", string.Empty);
            funcStr = funcStr.Replace("+", " + ");
            funcStr = funcStr.Replace("-", " - ");
            funcStr = funcStr.Replace("*", " * ");
            funcStr = funcStr.Replace("/", " / ");
            funcStr = funcStr.Replace("(", "( ");
            funcStr = funcStr.Replace(")", " )");
            funcStr = Replace_pow(funcStr);
            //fStr = Replace_factorial(fStr);
            funcStr = funcStr.Replace("sin", "Math.Sin");
            funcStr = funcStr.Replace("cos", "Math.Cos");
            funcStr = funcStr.Replace("tg", "Math.Tan");
            funcStr = funcStr.Replace("ctg", "1/Math.Tan");
            funcStr = funcStr.Replace("ln", "Math.Log");
            funcStr = funcStr.Replace("lg", "Math.Log10");
            funcStr = funcStr.Replace("e", "Math.E");
            return funcStr;
        }

        static string Replace_pow(string str)
        {
            string leftVal, rightVal;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '^')
                {
                    leftVal = rightVal = null;
                    Str_detect(ref str, ref leftVal, i, 'l');
                    Str_detect(ref str, ref rightVal, i, 'r');
                    str = str.Replace(leftVal + "^" + rightVal, "Math.Pow(" + leftVal + "," + rightVal + ")");
                }

            }
            return str;
        }

        //static string Replace_factorial(string str)
        //{
        //    string value;
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (str[i] == '!')
        //        {
        //            value = null;
        //            Str_detect(ref str, ref value, i, 'l');
        //            str = str.Replace(value + "!", Fact(Convert.ToInt32(value)));
        //        }

        //    }
        //    return str;
        //}

        static void Str_detect(ref string s, ref string value, int cur_pos, char way)
        {
            int step, index, B_br_cnt = 0, E_br_cnt = 0;
            char Begin_brack, End_brack;
            bool Close_brack = false;

            if (way == 'l')
            {
                step = -1;
                Begin_brack = ')';
                End_brack = '(';
            }
            else
            {
                step = 1;
                Begin_brack = '(';
                End_brack = ')';
            }

            index = cur_pos + step;

            do
            {
                if (s[cur_pos + step] == Begin_brack && !Close_brack)
                {
                    if (s[index] == Begin_brack)
                        B_br_cnt++;
                    if (s[index] == End_brack)
                        E_br_cnt++;

                    if (B_br_cnt != E_br_cnt)
                        index += step;
                    else
                    {
                        Close_brack = true;
                    }
                }
                else
                {
                    if (index == 0 || index == s.Length - 1 || s[index + step] == ' ')
                    {
                        if (way == 'l')
                            value = s.Substring(index, cur_pos - index);
                        else
                            value = s.Substring(cur_pos + 1, index - cur_pos);
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
