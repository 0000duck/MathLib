using System;
using System.Collections.Generic;

namespace MathLib
{
    /// <summary>
    /// f(x) delegate
    /// </summary>
    /// <param name="x">The x.</param>
    /// <returns></returns>
    public delegate double Dfunction(double x);
    /// <summary>
    /// f(x1, x2) delegate
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns></returns>
    public delegate double Dfunction2(double x, double y);

    /// <summary>
    /// Represent a geometric point
    /// </summary>
    [Serializable]
    public class Dot
    {
        /// <summary>
        /// Gets or sets the X-Axis value.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { set; get; }
        /// <summary>
        /// Gets or sets the Y-Axis value.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { set; get; }
		
		public Dot()
        {
           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dot"/> class.
        /// </summary>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        public Dot(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    /// <summary>
    /// Represent mathematical function and methods to work with it
    /// </summary>
    public class Function
    {
        Dot[] _lagrangeDots;

        public Dfunction F { get; set; }
        public Dfunction2 F2 { get; set; }
        public string IterationsHistory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="lagrangeDots">The Lagrange polynom dots.</param>
        public Function(Dot[] lagrangeDots)
        {
            this._lagrangeDots = lagrangeDots;
            this.F = Polinom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="koef">The koef of N-order polynom.</param>
        public Function(params double[] koef)
        {
            this.F = delegate(double x){
                double val = Math.Pow(x, koef.Length - 1);
                for(int i = 1; i < koef.Length; i++)
                {
                    val += koef[i] * Math.Pow(x, koef.Length - (i+1));
                }
                return val;
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="FDelegate">The f(x) delegate.</param>
        public Function(Dfunction FDelegate)
        {
            this.F = FDelegate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="FDelegate">The f(x1, x2) delegate.</param>
        public Function(Dfunction2 FDelegate)
        {
            this.F2 = FDelegate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Function"/> class.
        /// </summary>
        /// <param name="funcStr">The function string. For example x^2+x+5.</param>
        public Function(string funcStr)
        {
            string fStr = "public static double FuncStruct(double x)" +
                            "{" +
                            "       return " + FunctionStringParser.ToSystemSyntax(funcStr) + ";" +
                            "}";
            F = DelegateGenerator.CreateDelegate<Dfunction>("FuncStruct", fStr);
        }

        public Function(string funcStr, int argsCnt)
        {
            string fStr = "public static double FuncStruct(double x, double y)" +
                            "{" +
                            "       return " + FunctionStringParser.ToSystemSyntax(funcStr) + ";" +
                            "}";
            F2 = DelegateGenerator.CreateDelegate<Dfunction2>("FuncStruct", fStr);
        }

        /// <summary>
        /// Gets the function dots for graphical building.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <returns></returns>
        public IEnumerable<Dot> GetFunctionDots(double x1, double x2)
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


        /// <summary>
        /// Get the argument of function by chord method.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetArgByChord(double x1, double x2, double eps)
        {
            while (Math.Abs(x2 - x1) > eps)
            {
                x1 = x2 - (x2 - x1) * F(x2) / (F(x2) - F(x1));
                x2 = x1 + (x1 - x2) * F(x1) / (F(x1) - F(x2));
            }
            return x2;
        }

        /// <summary>
        /// Gets the argument of function by dichotomy method.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetArgByDichotomy(double x1, double x2, double eps)
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

        /// <summary>
        /// Gets the arguments of function by chord method.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <param name="eps">The eps.</param>
        /// <param name="steps">The steps count.</param>
        /// <returns></returns>
        public List<double> GetArgsByChord(double x1 = -100, double x2 = 100, double eps = 0.0001, int steps = 200)
        {
            List<double> roots = new List<double>();
            double step = (x2 - x1) / steps;
            double val = 0;
            x2 = x1 + step;
            for (int i = 1; i <= steps; i++, x1+=step, x2+=step)
            {
                val = GetArgByChord(x1, x2, eps);
                if (F(x1)*F(x2) < 0)
                {
                    roots.Add(val);
                }
            }
            return roots;
        }

        /// <summary>
        /// Gets the arguments of function by dichotomy method.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <param name="eps">The eps.</param>
        /// <param name="steps">The steps count.</param>
        /// <returns></returns>
        public List<double> GetArgsByDichotomy(double x1 = -100, double x2 = 100, double eps = 0.0001, int steps = 200)
        {
            List<double> roots = new List<double>();
            double step = (x2 - x1) / steps;
            double val = 0;
            x2 = x1 + step;
            for (int i = 1; i <= steps; i++, x1 += step, x2 += step)
            {
                val = GetArgByDichotomy(x1, x2, eps);
                if (F(x1) * F(x2) < 0)
                {
                    roots.Add(val);
                }
            }
            return roots;
        }

        /// <summary>
        /// Gets the argument of function by golden section method.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetArgByGoldenSection(double x1, double x2, double eps)
        {
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

        /// <summary>
        /// Gets the argument of function by Phibonacci method.
        /// </summary>
        /// <param name="x1">The left limit.</param>
        /// <param name="x2">The right limit.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetArgByPhibonacci(double x1, double x2, double eps)
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

        private double Polinom(double arg)
        {
            double result = 0;

            for (short i = 0; i < _lagrangeDots.Length; i++)
            {
                double P = 1.0;

                for (short j = 0; j < _lagrangeDots.Length; j++)
                    if (j != i)
                        P *= (arg - _lagrangeDots[j].X) / (_lagrangeDots[i].X - _lagrangeDots[j].X);

                result += P * _lagrangeDots[i].Y;
            }

            return result;
        }

        /// <summary>
        /// Gets the integral of function by rectangle method.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public double GetIntegralByRectangle(double a, double b, int n)
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

        /// <summary>
        /// Gets the integral of function by rectangle method.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="n">The n.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetIntegralByRectangle(double a, double b, int n, double eps)
        {
            int len = eps.ToString().Replace("0,",string.Empty).Length;
            double y1 = GetIntegralByRectangle(a, b, n), y2 = GetIntegralByRectangle(a, b, n*2);
            while(Math.Abs(y2 - y1) > eps)
            {
                n *= 2;
                y1 = GetIntegralByRectangle(a, b, n);
                y2 = GetIntegralByRectangle(a, b, n * 2);  
            }
            return Math.Round(y2, len);
        }

        /// <summary>
        /// Gets the integral of function by trapeze method.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public double GetIntegralByTrapeze(double a, double b, int n)
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

        /// <summary>
        /// Gets the integral of function by trapeze method.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="n">The n.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetIntegralByTrapeze(double a, double b, int n, double eps)
        {
            int len = eps.ToString().Replace("0,", string.Empty).Length;
            double y1 = GetIntegralByTrapeze(a, b, n), y2 = GetIntegralByTrapeze(a, b, n * 2);
            while (Math.Abs(y2 - y1) > eps)
            {
                n *= 2;
                y1 = GetIntegralByTrapeze(a, b, n);
                y2 = GetIntegralByTrapeze(a, b, n * 2);
            }
            return Math.Round(y2, len);
        }

        /// <summary>
        /// Gets the integral of function by Simpsons method.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public double GetIntegralBySimpson(double a, double b, int n)
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

        /// <summary>
        /// Gets the integral of function by Simpsons method.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="n">The n.</param>
        /// <param name="eps">The eps.</param>
        /// <returns></returns>
        public double GetIntegralBySimpson(double a, double b, int n, double eps)        //Нахождение определенного интеграла функции с помощью метода Симпсона с указанной точностью
        {
            int len = eps.ToString().Replace("0,", string.Empty).Length;
            double y1 = GetIntegralBySimpson(a, b, n), y2 = GetIntegralBySimpson(a, b, n * 2);
            while (Math.Abs(y2 - y1) > eps)
            {
                n *= 2;
                y1 = GetIntegralBySimpson(a, b, n);
                y2 = GetIntegralBySimpson(a, b, n * 2);
            }
            return Math.Round(y2, len);
        }

        /// <summary>
        /// Gets the differential of function by Runge Kutta method.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="h">The h.</param>
        /// <param name="xl">The xl.</param>
        /// <returns></returns>
        public double GetDifferentialByRungeKutta(double x, double y, double h, double xl)      //Метод для нахождения производной с помощью метода Рунге-Кутты 4-го порядка
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
