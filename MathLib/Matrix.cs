using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib
{
    public class Matrix
    {
        double[,] matrixBody;
        double[] matrixAns;
        double[,] identityMatrix;

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
                return new Matrix(identityMatrix);
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
            return m1.Add(m2);
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
            this.identityMatrix = this.InitIdentityMatrix(size);
        }

        public Matrix(int rows, int columns)    //Инициализирует пустую матрицу с заданным количеством строк и столбцов
        {
            this.matrixBody = new double[rows, columns];
            this.matrixAns = new double[rows];
            this.identityMatrix = this.InitIdentityMatrix(rows);
        }

        public Matrix(double[,] mBody, double[] mAns)      //Инициализирует матрицу по заданному двумерному массиву и массиву со значениями выражений
        {
            this.matrixBody = (double[,])mBody.Clone();
            this.matrixAns = (double[])mAns.Clone();
            this.identityMatrix = this.InitIdentityMatrix(mBody.GetLength(0));
        }

        public Matrix(Matrix m)      //Инициализирует матрицу по заданному двумерному массиву и массиву со значениями выражений
        {
            this.matrixBody = (double[,])m.matrixBody.Clone();
            this.matrixAns = (double[])m.matrixAns.Clone();          
            this.identityMatrix = this.InitIdentityMatrix(this.matrixBody.GetLength(0));
        }

        Matrix(double[,] mBody)
        {
            this.matrixBody = (double[,])mBody.Clone();
            this.identityMatrix = this.InitIdentityMatrix(mBody.GetLength(0));
        }

        /////////////////////////////////////////////////////////////
        /////////////////////////***Методы***////////////////////////
        /////////////////////////////////////////////////////////////

        double[,] InitIdentityMatrix(int n)
        {
            double[,] m = new double[n, n];
            for (int i = 0; i < n; i++)
                m[i, i] = 1;
            return m;
        }

        public Matrix Add(Matrix Matrix2)
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

        /*        Matrix Gauss_Jordan_matrix()
                {
                    Matrix m = new Matrix(this.matrixBody, this.matrixAns);
                    double[,] a = m.matrixBody;
                    double[,] rev_a = m.reverseMatrix;
                    double[] x = m.matrixAns;
                    double h;


                    for (int k = 0; k < m.Columns; k++)
                    {
                        for (int j = k + 1; j < m.Rows; j++)
                        {
                            h = a[j, k] / a[k, k];
                            for (int i = 0; i < m.Rows; i++)
                            {
                                a[j, i] -= h * a[k, i];
                                rev_a[j, i] -= h * rev_a[k, i];
                            }

                            x[j] -= h * x[k];
                        }
                    }

                    for (int k = m.Columns - 1; k >= 0; k--)
                    {
                        for (int j = k - 1; j >= 0; j--)
                        {
                            h = a[j, k] / a[k, k];
                            for (int i = 0; i < m.Rows; i++)
                            {
                                a[j, i] -= h * a[k, i];
                                rev_a[j, i] -= h * rev_a[k, i];
                            }

                            x[j] -= h * x[k];
                        }
                    }

                    for (int i = Rows - 1; i >= 0; i--)
                    {
                        x[i] = x[i] / a[i, i];
                    }

                    for (int i = 0; i < m.Rows; i++)
                        for (int j = 0; j < m.Columns; j++)
                        {
                            rev_a[i, j] /= a[i, i];
                        }

                    return m;
                }

                public double[] Get_args_GAUSS_M()            //Решение СЛАУ (матрицы) методом Гаусса
                {
                    return Gauss_Jordan_matrix().MatrixAns;
                }

                public double[] Get_args_EASY_ITERATION_M(double eps)
                {
                    Matrix m = new Matrix(Rows, Columns);
                    double[,] a = m.MatrixBody;
                    double[] b = m.MatrixAns;
                    double[] x0 = new double[Rows];
                    double[] x1 = new double[Rows];
                    double norm = 0;

                    for (int i = 0; i < Rows; i++)
                    {
                        b[i] = matrixAns[i] / matrixBody[i, i];
                        for (int j = 0; j < Columns; j++)
                        {
                            if (i != j)
                            {
                                a[i, j] = -matrixBody[i, j] / matrixBody[i, i];
                            }
                        }
                    }

                    for (int i = 0; i < Columns; i++)
                    {
                        double max = 0;
                        for (int j = 0; j < Rows; j++)
                        {
                            max += a[j, i];
                        }
                        if (max > norm)
                            norm = max;
                    }

                    b.CopyTo(x1, 0);
                    double sum = 0;

                    do
                    {
                        double[] diff = new double[Rows];
                        x1.CopyTo(x0, 0);
                        x1 = new double[Rows];

                        for (int i = 0; i < Rows; i++)
                        {
                            for (int j = 0; j < Columns; j++)
                            {
                                x1[i] += a[i, j] * x0[j];
                            }
                            x1[i] += b[i];
                            diff[i] = x1[i] - x0[i];
                        }

                        sum = diff.Sum(i => Math.Abs(i));
                    }
                    while (sum > eps * (1 - norm) / norm);

                    return x1;
                }

                public Matrix GetReverseMatrix()            //Решение СЛАУ (матрицы) методом Гаусса
                {
                    return new Matrix(Gauss_Jordan_matrix().reverseMatrix);
                }*/

        public double GetTrace()
        {
            double val = 0;
            for(int i = 0; i < Rows; i++)
            {
                val += this.MatrixBody[i, i];
            }
            return val;
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
            return GetEigenValues_LEVERRIER_FADDEEV();
        }

        public double[] GetEigenValues_LEVERRIER_FADDEEV()
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

    }
}
