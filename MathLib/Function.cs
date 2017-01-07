using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib
{
    public delegate double Dfunction(double x);  // делегат, ссылающийся на определенную функцию
    public delegate double Dfunction2(double x, double y);

    [Serializable]
    public class Dot  // тип, характеризующий координаты точки на плоскости
    {
        public double X { set; get; }
        public double Y { set; get; }
		
		public Dot()
        {
           
        }
		
        public Dot(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    public class Function
    {
        public Dfunction F { get; set; }
        public Dfunction2 F2 { get; set; }
        Dot[] lagrangeDots;
        public string IterationsHistory { get; set; }

        public Function(Dot[] lagrangeDots)     //Конструктор, инициализирующий функцию по точкам с помощью полинома Лагранжа
        {
            this.lagrangeDots = lagrangeDots;
            this.F = Polinom;
        }

        public Function(params double[] koef)   //Конструктор, инициализирующий функцию по коэффициентам уравнения степени N(зависит от количества коэффициентов);
        {
            this.F = delegate(double x){
                double val = Math.Pow(x, koef.Length);
                for(int i = 0; i < koef.Length; i++)
                {
                    val += koef[i] * Math.Pow(x, koef.Length - (i+1));
                }
                return val;
            };
        }

        public Function(Dfunction FDelegate)    //Конструктор, инициализирующий функцию по делегату
        {
            this.F = FDelegate;
        }

        public Function(Dfunction2 FDelegate)
        {
            this.F2 = FDelegate;
        }

        public Function(string funcStr)         //Конструктор, инициализирующий функцию по строке (например x^2+x+5)
        {
            string fStr = "public static double FuncStruct(double x)" +
                            "{" +
                            "       return " + FuncStrParsing.ToSystemSyntax(funcStr) + ";" +
                            "}";
            F = DelegateGenerator.CreateDelegate<Dfunction>("FuncStruct", fStr);
        }

        public Function(string funcStr, int argsCnt)
        {
            string fStr = "public static double FuncStruct(double x, double y)" +
                            "{" +
                            "       return " + FuncStrParsing.ToSystemSyntax(funcStr) + ";" +
                            "}";
            F2 = DelegateGenerator.CreateDelegate<Dfunction2>("FuncStruct", fStr);
        }

        public List<Dot> GetFunctionDots(double x1, double x2)   //Метод для получения точек построения графика
        {
            List<Dot> dots = new List<Dot>();
            int Nsteps = 1000;
            double x = x1, y, step = (x2 - x1) / Nsteps;

            for (int i = 0; i < Nsteps; i++)
            {
                if (x != 0 && !double.IsInfinity(F(x)))
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
        public double FindRoot_CHORD(double x1, double x2, double eps)      //Метод для получения корня функции на интервале с помощью метода хорд с заданной точностью
        {
            while (Math.Abs(x2 - x1) > eps)
            {
                x1 = x2 - (x2 - x1) * F(x2) / (F(x2) - F(x1));
                x2 = x1 + (x1 - x2) * F(x1) / (F(x1) - F(x2));
            }
            return x2;
        }

        public double FindRoot_DICHOTOMY(double x1, double x2, double eps)      //Метод для получения корня функции на интервале с помощью метода дихотомии с заданной точностью
        {
            double x;
            do {
                x = (x1 + x2) / 2;
                if (F(x) * F(x1) < 0)
                    x2 = x;
                else
                    x1 = x;
            } while (Math.Abs(x2 - x1) > eps && F(x) != 0);
            return x;
        }

        public List<double> FindRoots_CHORD(double x1 = -100, double x2 = 100, double eps = 0.0001, int steps = 200)    //Метод для получения всех корней функции на интервале с помощью метода хорд с заданной точностью
        {
            List<double> roots = new List<double>();
            double step = (x2 - x1) / steps;
            double val = 0;
            x2 = x1 + step;
            for (int i = 1; i <= steps; i++, x1+=step, x2+=step)
            {
                val = FindRoot_CHORD(x1, x2, eps);
                if (F(x1)*F(x2) < 0)
                {
                    roots.Add(val);
                }
            }
            return roots;
        }

        public List<double> FindRoots_DICHOTOMY(double x1 = -100, double x2 = 100, double eps = 0.0001, int steps = 200)    //Метод для получения всех корней функции на интервале с помощью метода дихотомии с заданной точностью
        {
            List<double> roots = new List<double>();
            double step = (x2 - x1) / steps;
            double val = 0;
            x2 = x1 + step;
            for (int i = 1; i <= steps; i++, x1 += step, x2 += step)
            {
                val = FindRoot_DICHOTOMY(x1, x2, eps);
                if (F(x1) * F(x2) < 0)
                {
                    roots.Add(val);
                }
            }
            return roots;
        }

        public double Golden_section_method(double x1, double x2, double eps)       //Метод золотого сечения для нахождения экстремумов функции на промежутке с заданой точностью
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

        public double Phibonacci_method(double x1, double x2, double eps)   //Метод Фибоначчи для нахождения экстремумов функции на промежутке с заданой точностью
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

        double Polinom(double arg)      //Метод для нахождения значения функции в точке с помощью полинома Лагранжа
        {
            double result = 0;

            for (short i = 0; i < lagrangeDots.Length; i++)
            {
                double P = 1.0;

                for (short j = 0; j < lagrangeDots.Length; j++)
                    if (j != i)
                        P *= (arg - lagrangeDots[j].X) / (lagrangeDots[i].X - lagrangeDots[j].X);

                result += P * lagrangeDots[i].Y;
            }

            return result;
        }

        public double GetIntegral_RECTANGLE(double a, double b, int n)      //Нахождение определенного интеграла функции с помощью метода прямоугольников
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

        public double GetIntegral_RECTANGLE(double a, double b, int n, double eps)      //Нахождение определенного интеграла функции с помощью метода прямоугольников с указанной точностью
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

        public double GetIntegral_TRAPEZE(double a, double b, int n)        //Нахождение определенного интеграла функции с помощью метода трапеций
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

        public double GetIntegral_TRAPEZE(double a, double b, int n, double eps)        //Нахождение определенного интеграла функции с помощью метода трапеций с указанной точностью
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

        public double GetIntegral_SIMPSON(double a, double b, int n)        //Нахождение определенного интеграла функции с помощью метода Симпсона
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

        public double GetIntegral_SIMPSON(double a, double b, int n, double eps)        //Нахождение определенного интеграла функции с помощью метода Симпсона с указанной точностью
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

        public double GetDifferential_RUNGE_KUTTA(double x, double y, double h, double xl)      //Метод для нахождения производной с помощью метода Рунге-Кутты 4-го порядка
        {
            double k1, k2, k3, k4, d;
            int n = 1;
            for (; x <= xl; x += h)
            {
                IterationsHistory += n+") x=" + x + "  , y=" + y + "\r\n";
                k1 = h * F2(x, y);
                k2 = h * F2(x + h / 2, y + k1 / 2);
                k3 = h * F2(x + h / 2, y + k2 / 2);
                k4 = h * F2(x + h, y + k3);
                d = (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                y += d;
                n++;
            }
            return y;
        }

    }
}
