using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MathLib
{
    public class Matrix
    {
        double[,] matrixBody;
        double[] matrixAns;

        public double[,] MatrixBody    //Матрица коэффициентов
        {
            get
            {
                return matrixBody;
            }
        }
        public double[] MatrixAns      //Вектор значений выражений матрицы
        {
            get
            {
                return matrixAns;
            }
        }

        public Matrix IdentityMatrix      //Единичная матрица
        {
            get
            {
                return GetIdentityMatrix(Rows);
            }
        }

        public Matrix ReverseMatrix      //Обратная матрица для исходной
        {
            get
            {
                return GetReverseMatrix_GAUSS_M();
            }
        }

        public int Rows                 //Количество строк матрицы
        {
            get
            {
                return matrixBody.GetLength(0);
            }
        }
        public int Columns              //Количество столбцов матрицы
        {
            get
            {
                return matrixBody.GetLength(1);
            }
        }

        /////////////////////////////////////////////////////////////
        ///////////////////***Перегрузка операторов***///////////////
        /////////////////////////////////////////////////////////////

        public static Matrix operator +(Matrix m1, Matrix m2)       //сложение матриц
        {
            return m1.Plus(m2);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)       //вычитание матриц
        {
            return m1.Minus(m2);
        }

        public static Matrix operator *(Matrix m, double number)       //умножение матрицы на число
        {
            return m.Multiply(number);
        }

        public static Matrix operator *(double number, Matrix m)       //умножение матрицы на число
        {
            return m.Multiply(number);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)       //умножение двух матриц
        {
            return m1.Multiply(m2);
        }

        /////////////////////////////////////////////////////////////
        /////////////////////***Конструкторы***//////////////////////
        /////////////////////////////////////////////////////////////

        public Matrix(int size)    //Инициализирует пустую квадратную матрицу с заданным размером
        {
            this.matrixBody = new double[size, size];
            this.matrixAns = new double[size];
        }

        public Matrix(int rows, int columns)    //Инициализирует пустую матрицу с заданным количеством строк и столбцов
        {
            this.matrixBody = new double[rows, columns];
            this.matrixAns = new double[rows];
        }

        public Matrix(double[,] mBody, double[] mAns)      //Инициализирует матрицу по заданному двумерному массиву и массиву со значениями выражений
        {
            this.matrixBody = (double[,])mBody.Clone();
            this.matrixAns = (double[])mAns.Clone();
        }

        public Matrix(Matrix m)      //Инициализирует матрицу по заданному двумерному массиву и массиву со значениями выражений
        {
            this.matrixBody = (double[,])m.matrixBody.Clone();
            if(m.MatrixAns != null)
                this.matrixAns = (double[])m.matrixAns.Clone();          
        }

        public Matrix(double[,] mBody)
        {
            this.matrixBody = (double[,])mBody.Clone();
        }

        /////////////////////////////////////////////////////////////
        /////////////////////////***Методы***////////////////////////
        /////////////////////////////////////////////////////////////

        public Matrix Plus(Matrix Matrix2)
        {
            if (this.Rows != Matrix2.Rows || this.Columns != Matrix2.Columns)
                return null;
            else
            {
                Matrix m = new Matrix(this.Rows, this.Columns);
                for (int i = 0; i < m.Rows; i++)
                    for (int j = 0; j < m.Columns; j++)
                    {
                        m.matrixBody[i, j] = this.matrixBody[i, j] + Matrix2.matrixBody[i, j];
                    }
                return m;
            }
        }

        public Matrix Minus(Matrix Matrix2)
        {
            if (this.Rows != Matrix2.Rows || this.Columns != Matrix2.Columns)
                return null;
            else
            {
                Matrix m = new Matrix(this.Rows, this.Columns);
                for (int i = 0; i < m.Rows; i++)
                    for (int j = 0; j < m.Columns; j++)
                    {
                        m.matrixBody[i, j] = this.matrixBody[i, j] - Matrix2.matrixBody[i, j];
                    }
                return m;
            }
        }

        public Matrix Multiply(Matrix Matrix2)
        {
            if (this.Columns != Matrix2.Rows)
                return null;
            else
            {
                Matrix m = new Matrix(this.Rows, Matrix2.Columns);
                for (int c = 0; c < m.Columns; c++)
                    for (int i = 0; i < m.Rows; i++)
                        for (int j = 0; j < m.Columns; j++)
                        {
                            m.matrixBody[i, c] += this.matrixBody[i, j] * Matrix2.matrixBody[j, c];
                        }
                return m;
            }
        }

        public Matrix Multiply(double number)
        {
            Matrix m = new Matrix(this.Rows, this.Columns);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Columns; j++)
                {
                    m.matrixBody[i, j] = this.matrixBody[i, j] * number;
                }
            return m;
        }

        public Matrix RoundElements(int n)  //Округляет элементы матрицы до n-го знака после запятой
        {
            Matrix m = new Matrix(this);
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    m.MatrixBody[i, j] = Math.Round(m.MatrixBody[i, j], n);
                }
                if(m.MatrixAns != null)
                {
                    m.MatrixAns[i] = Math.Round(m.MatrixAns[i], n);
                }
            }
            return m;
        }

        public double GetTrace()
        {
            double val = 0;
            for(int i = 0; i < Rows; i++)
            {
                val += this.MatrixBody[i, i];
            }
            return val;
        }

        public Vector[] ToVectors()
        {
            Vector[] vectors = new Vector[Columns];
            double[] ans = new double[Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    ans[j] = MatrixBody[j, i];
                }
                vectors[i] = new Vector(ans);
            }
            return vectors;
        }

        public Matrix GetTriangleMatrix()   //метод приведения матрицы к треугольному виду
        {
            Matrix m = new Matrix(this.matrixBody, this.matrixAns);
            double[,] a = m.matrixBody;
            double[] x = m.matrixAns;
            double h;

            for (int k = 0; k < m.Columns; k++)
            {
                for (int i = k + 1; i < m.Rows; i++)
                {
                    h = a[i, k] / a[k, k];
                    for (int j = 0; j < m.Columns; j++)
                    {
                        a[i, j] -= h * a[k, j];
                    }
                    
                    x[i] -= h * x[k];
                }
            }
            
            return m;
        }

        //-------------------Методы вычисления аргументов матрицы------------------//

        public double[] GetArgs()
        {
            return GetArgs_GAUSS_M();
        }

        public double[] GetArgs_GAUSS_M()
        {
            Matrix m = GetTriangleMatrix();
            double[,] a = m.matrixBody;
            double[] x = m.matrixAns;
            int n = Rows;

            for (int i = n - 1; i >= 0; i--)
            {
                if (i != n - 1)
                    for (int j = i + 1; j < n; j++)
                    {
                        x[i] -= a[i, j] * x[j];
                    }
                x[i] = x[i] / a[i, i];
            }
            return x;
        }

        //-------------------Методы вычисления обратной матрицы матрицы------------------//

        public Matrix GetReverseMatrix()
        {
            return GetReverseMatrix_GAUSS_M();
        }

        public Matrix GetReverseMatrix_GAUSS_M()
        {
            int n = Rows;
            double[] x1 = new double[n];
            double[,] a = new double[n, n];
            
            for (int i = 0; i < n; i++)
            {

                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        x1[j] = 1;
                    else
                        x1[j] = 0;
                }
                Matrix m = new Matrix(this.matrixBody, x1);

                double[] x = m.GetArgs_GAUSS_M();
                for (int j = 0; j < n; j++)
                {
                    a[j, i] = x[j];
                }
                
            }
            return new Matrix(a);
        }

        //-------------------Методы вычисления определителя матрицы------------------//

        public double GetDeterminant()
        {
            return GetDeterminant_GAUSS_M();
        }

        public double GetDeterminant_DECOMPOSITION()         //возвращает определитель (детерминант) матрицы
        {
            if (Rows == 2 && Columns == 2)
            {
                return matrixBody[0, 0] * matrixBody[1, 1] - matrixBody[0, 1] * matrixBody[1, 0];
            }
            else
            {
                double det = 0;
                for (int i = 0; i < Columns; i++)
                    det += matrixBody[0, i] * Math.Pow(-1, i + 2) * new Matrix(GetMinor(0, i)).GetDeterminant_DECOMPOSITION();
                return det;
            }
        }

        public double GetDeterminant_GAUSS_M()
        {
            Matrix m = GetTriangleMatrix();
            double[,] a = m.matrixBody;
            int n = Rows;
            double val = 1;
            for (int i = 0; i < n; i++)
            {
                val *= a[i, i];
            }
            return val;
        }

        //---------------------------------------

        public double[,] GetMinor (int Mrow, int Mcolumn)       //возвращает минор элемента
        {
            double[,] minor = new double[Rows - 1, Columns - 1];
            for (int i = 0, I = 0; i < Rows; i++)
            {
                if (i != Mrow)
                {
                    for (int j = 0, J = 0; j < Columns; j++)
                    {
                        if (j != Mcolumn)
                        {
                            minor[I, J] = matrixBody[i, j];
                            J++;
                        }
                    }
                    I++;
                }
            }
            return minor;
        }

        //-------------------Методы вычисления собственных значений и векторов матрицы------------------//

        public double[] GetEigenValues()
        {
            double[] p = GetCharactPolynomial().Select(e => e*(-1)).ToArray();
            Function f = new Function(p);
            return f.FindRoots_CHORD(eps:0.00001).ToArray();
        }

        public Vector[] GetEigenVectors()
        {
            int n = Rows;
            double[] la = GetEigenValues();
            Matrix A = new Matrix(this), B;
            Matrix[] arrB = new Matrix[n - 1];
            Vector y, b;
            Vector[] res = new Vector[n];
            double val;

            for(int i = 0; i < n-1; i++)
            {
                val = A.GetTrace() / (i + 1);
                B = A - val * A.IdentityMatrix;
                A = this * B;
                arrB[i] = new Matrix(B);
            }

            for (int i = 0; i < la.Length; i++)
            {
                y = Matrix.GetIdentityMatrix(n).ToVectors()[0];
                for (int j = 0; j < n - 1; j++)
                {
                    b = arrB[j].ToVectors()[0];
                    y = y * la[i] + b;
                }
                res[i] = new Vector(y);
            }

            return res;
        }

        //-------------------Методы вычисления характеристического многочлена матрицы------------------//

        public double[] GetCharactPolynomial()
        {
            return GetCharactPolynomial_LEVERRIER_FADDEEV();
        }

        public double[] GetCharactPolynomial_LEVERRIER_FADDEEV()    //Метод Леверье-Фадеева
        {
            Matrix a = new Matrix(this), b;
            double val;
            double[] p = new double[Rows];

            for(int i = 0; i < Rows; i++)
            {
                val = a.GetTrace()/(i+1);
                p[i] = val;
                b = a - val * a.IdentityMatrix;
                a = this * b;
            }

            return p;
        }

        /////////////////////////////////////////////////////////////
        ///////////////////***Статические методы***//////////////////
        /////////////////////////////////////////////////////////////

        public static Matrix GetIdentityMatrix(int size)
        {
            double[,] m = new double[size, size];
            for (int i = 0; i < size; i++)
                m[i, i] = 1;

            return new Matrix(m);
        }

        /////////////////////////////////////////////////////////////
        /////////////***Методы получения HTML-отчетов***/////////////
        /////////////////////////////////////////////////////////////
        public Matrix GetTriangleMatrix_VSReport(string reportName = "report.html", bool IsGenerate = true)   //метод приведения матрицы к треугольному виду
        {
            HtmlReportCreator rep = new HtmlReportCreator(reportName);
            Matrix m = new Matrix(this.matrixBody, this.matrixAns);
            double[,] a = m.matrixBody;
            double[] x = m.matrixAns;
            double h;

            rep.WriteLine("Исходная матрица:");
            rep.WriteMatrix(new Matrix(m));

            for (int k = 0; k < m.Columns; k++)
            {
                for (int i = k + 1; i < m.Rows; i++)
                {
                    h = a[i, k] / a[k, k];
                    for (int j = 0; j < m.Columns; j++)
                    {
                        a[i, j] -= h * a[k, j];
                    }

                    x[i] -= h * x[k];
                    
                    rep.WriteLine("Шаг " + i + ": ( a" + (i + 1) + " = a" + (i + 1) + " - a" + i + ")");
                    rep.WriteMatrix(new Matrix(m));
                }
            }

            if(IsGenerate)
                rep.GenerateReport();

            return m;
        }

    }
}
