using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib
{
    public delegate double Dfunction(double x);  // делегат, ссылающийся на определенную функцию

    public class Dot  // тип, характеризующий координаты точки на плоскости
    {
        public double X { set; get; }
        public double Y { set; get; }
        public Dot(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    public class Function
    {
        public Dfunction F { get; set; }   

        public Function(params double[] koef)
        {
            this.F = delegate(double x){
                double val = 0;
                foreach(var k in koef)
                {
                    val += k * x;
                }
                return val;
            };
        }

        public Function(Dfunction FDelegate)
        {
            this.F = FDelegate;
        }

        public Function(string funcStr)
        {
            string fStr = "public static double FuncStruct(double x)" +
                            "{" +
                            "       return " + FuncStrParsing.ToSystemSyntax(funcStr) + ";" +
                            "}";
            F = DelegateGenerator.CreateDelegate<Dfunction>("FuncStruct", fStr);
        }

        public List<Dot> GetFunctionDots(double x1, double x2)   //функция для нахождения точек построения графика
        {
            List<Dot> dots = new List<Dot>();
            int Nsteps = 1000;
            double x = x1, y, step = (x2 - x1) / Nsteps;

            for (int i = 0; i < Nsteps; i++)
            {
                if (x != 0)
                {
                    y = F(x);
                }
                else
                    y = 0;

                dots.Add(new Dot(x, y));
                x += step;
            }
            return dots;
        }

        // a, b - пределы хорды, eps - необходимая погрешность
        public double FindRoot(double x1, double x2, double eps)
        {
            while (Math.Abs(x2 - x1) > eps)
            {
                x1 = x2 - (x2 - x1) * F(x2) / (F(x2) - F(x1));
                x2 = x1 + (x1 - x2) * F(x1) / (F(x1) - F(x2));
            }
            return x2;
        }

        public double Golden_section_method(double x1, double x2, double eps)
        {
            //extr_iter_dots = new List<Dot>();
            double a, b;
            int n = 0;
            a = x2 - 2 * (x2 - x1) / (1 + Math.Sqrt(5));
            b = x1 + 2 * (x2 - x1) / (1 + Math.Sqrt(5));


            while ((x2 - x1) > eps)
            {
                if (F(a) <= F(b))
                {
                    x1 = a;
                    a = b;
                    b = x1 + 2 * (x2 - x1) / (1 + Math.Sqrt(5));
                    n++;
                }
                else
                {
                    x2 = b;
                    b = a;
                    a = x2 - 2 * (x2 - x1) / (1 + Math.Sqrt(5));
                    n++;
                }

            }

            return (x1 + x2) / 2;
        }

        /*public double Quadr_approximation_method(double eps)
        {
            double a = X1, b = X2, c = (a + b) / 2, Xmin = 0, Xextr;
            double C2;
            do
            {
                if ((c - a) / (b - a) < 0.1)
                {
                    Xmin = c + (b - c) / 4;
                }
                else if ((b - c) / (b - a) < 0.1)
                {
                    Xmin = c - (c - a) / 4;
                }
                else
                {
                    C2 = (1 / (b - c)) * ((F(b) - F(a)) / (b - a) - (F(c) - F(a)) / (c - a));
                    if (C2 == 0)
                    {
                    }
                    else
                    {
                        Xextr = (a + b) / 2 - (F(b) - F(a)) / (2 * C2 * (b - a));
                        if (C2 > 0)
                        {
                            if (Xextr < a)
                            {
                                Xmin = (a + c) / 2;
                                b = c;

                            }
                            else if (Xextr > b)
                            {
                                Xmin = (b + c) / 2;
                                a = c;
                            }
                            else
                            {
                                Xmin = Xextr;
                            }
                        }
                        else
                        {
                            if (Xextr < (a + b) / 2)
                            {
                                Xmin = (b + c) / 2;
                                a = c;
                            }
                            else
                            {
                                Xmin = (a + c) / 2;
                                b = c;
                            }
                            //continue;
                        }
                    }

                }

                if (Xmin < c)
                {
                    if (F(Xmin) < F(c))
                    {
                        b = c;
                        c = Xmin;
                    }
                    else
                        a = Xmin;
                }
                else
                {
                    if (F(Xmin) < F(c))
                    {
                        a = c;
                        c = Xmin;
                    }
                    else
                        b = Xmin;
                }
            } while (b - a > eps);

            return Xmin;
        }*/

        public double Phibonacci_method(double x1, double x2, double eps)
        {
            int n = Convert.ToInt32((x2 - x1) / eps);
            double a, b, y1, y2;
            int iteration = 1;
            double[] f_nums = new double[n];
            f_nums[0] = f_nums[1] = 1;
            for (int i = 2; i < n; i++)
            {
                f_nums[i] = f_nums[i - 1] + f_nums[i - 2];
            }

            a = x1 + (x2 - x1) * f_nums[n - 3] / f_nums[n - 1];
            b = x1 + (x2 - x1) * f_nums[n - 2] / f_nums[n - 1];
            y1 = F(a);
            y2 = F(b);

            while (n != 1)
            {
                n--;
                if (y1 <= y2)
                {
                    x1 = a;
                    a = b;
                    b = x2 - (a - x1);
                    y1 = y2;
                    y2 = F(b);
                    iteration++;
                }
                else
                {
                    x2 = b;
                    b = a;
                    a = x1 + (x2 - b);
                    y2 = y1;
                    y1 = F(a);
                    iteration++;
                }

            }

            return Math.Min(a, b);
        }

        public double Polinom(Dot [] lagrange_dots, double arg)
        {
            double result = 0;

            for (short i = 0; i < lagrange_dots.Length; i++)
            {
                double P = 1.0;

                for (short j = 0; j < lagrange_dots.Length; j++)
                    if (j != i)
                        P *= (arg - lagrange_dots[j].X) / (lagrange_dots[i].X - lagrange_dots[j].Y);

                result += P * lagrange_dots[i].Y;
            }

            return result;
        }

        public double GetIntegral_RECTANGLE(double a, double b, int n)
        {
            double h, S, x, x0, x1;
            int i;
            h = (b - a) / n;
            S = 0;
            x0 = a;
            x1 = x0 + h;
            for (i = 0; i < n; i++)
            {
                x = (x0 + x1) / 2;
                S = S + Math.Abs(F(x));
                x0 += h;
                x1 += h;
            }
            S = h * S;
            return S;
        }

        public double GetIntegral_RECTANGLE(double a, double b, int n, double eps)
        {
            int len = eps.ToString().Replace("0,",string.Empty).Length;
            double y1 = GetIntegral_RECTANGLE(a, b, n), y2 = GetIntegral_RECTANGLE(a, b, n*2);
            while(Math.Abs(y2 - y1) > eps)
            {
                n *= 2;
                y1 = GetIntegral_RECTANGLE(a, b, n);
                y2 = GetIntegral_RECTANGLE(a, b, n * 2);  
            }
            return Math.Round(y2, len);
        }

        public double GetIntegral_TRAPEZE(double a, double b, int n)
        {
            double h, S, x;
            int i;
            h = (b - a) / n;
            S = (F(a)+F(b))/2;
            for (i = 1; i < n; i++)
            {
                x = a + i * h;
                S = S + Math.Abs(F(x));
            }
            S = h * S;
            return S;
        }

        public double GetIntegral_TRAPEZE(double a, double b, int n, double eps)
        {
            int len = eps.ToString().Replace("0,", string.Empty).Length;
            double y1 = GetIntegral_TRAPEZE(a, b, n), y2 = GetIntegral_TRAPEZE(a, b, n * 2);
            while (Math.Abs(y2 - y1) > eps)
            {
                n *= 2;
                y1 = GetIntegral_TRAPEZE(a, b, n);
                y2 = GetIntegral_TRAPEZE(a, b, n * 2);
            }
            return Math.Round(y2, len);
        }

        public double GetIntegral_SIMPSON(double a, double b, int n)
        {
            double h, S, x;
            int i;
            h = (b - a) / n;
            S = (F(a) + F(b));
            for (i = 1; i < n; i++)
            {
                x = a + i * h;
                S = S + ((i % 2 == 0) ? Math.Abs(2 * F(x)) : Math.Abs(4 * F(x)));
            }
            S = h / 3 * S;
            return S;
        }

        public double GetIntegral_SIMPSON(double a, double b, int n, double eps)
        {
            int len = eps.ToString().Replace("0,", string.Empty).Length;
            double y1 = GetIntegral_SIMPSON(a, b, n), y2 = GetIntegral_SIMPSON(a, b, n * 2);
            while (Math.Abs(y2 - y1) > eps)
            {
                n *= 2;
                y1 = GetIntegral_SIMPSON(a, b, n);
                y2 = GetIntegral_SIMPSON(a, b, n * 2);
            }
            return Math.Round(y2, len);
        }
    }
}
