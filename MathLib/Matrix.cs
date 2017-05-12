using System;
using System.Collections.Generic;
using System.Linq;

namespace MathLib
{
    /// <summary>
    /// Provides methods for mathematical operations with matrices
    /// </summary>
    public class Matrix
    {
        double[,] matrixBody;
        double[] matrixAns;

        /// <summary>
        /// Gets the matrix koef.
        /// </summary>
        /// <value>
        /// The matrix body.
        /// </value>
        public double[,] MatrixBody
        {
            get
            {
                return matrixBody;
            }
        }

        /// <summary>
        /// Gets the ans vector.
        /// </summary>
        /// <value>
        /// The matrix ans.
        /// </value>
        public double[] MatrixAns      
        {
            get
            {
                return matrixAns;
            }
        }

        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        /// <value>
        /// The identity matrix.
        /// </value>
        public Matrix IdentityMatrix
        {
            get
            {
                return GetIdentityMatrix(CountRows);
            }
        }

        /// <summary>
        /// Gets the reverse matrix.
        /// </summary>
        /// <value>
        /// The reverse matrix.
        /// </value>
        public Matrix ReverseMatrix
        {
            get
            {
                return GetReverseMatrixByGauss();
            }
        }

        /// <summary>
        /// Gets the rows count.
        /// </summary>
        /// <value>
        /// The rows count.
        /// </value>
        public int CountRows
        {
            get
            {
                return matrixBody.GetLength(0);
            }
        }

        /// <summary>
        /// Gets the columns count.
        /// </summary>
        /// <value>
        /// The columns count.
        /// </value>
        public int CountColumns         
        {
            get
            {
                return matrixBody.GetLength(1);
            }
        }

        /////////////////////////////////////////////////////////////
        ///////////////////***Operator implementing***///////////////
        /////////////////////////////////////////////////////////////


        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="m1">The first matrix.</param>
        /// <param name="m2">The second matrix.</param>
        /// <returns>
        /// The result of addition of matrices.
        /// </returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return m1.Add(m2);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="m1">The first matrix.</param>
        /// <param name="m2">The second matrix.</param>
        /// <returns>
        /// The result of substraction of matrices.
        /// </returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return m1.Sub(m2);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="number">The number.</param>
        /// <returns>
        /// The result of the matrix multiplication by number.
        /// </returns>
        public static Matrix operator *(Matrix m, double number)       //умножение матрицы на число
        {
            return m.Multiply(number);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="number">The number.</param>
        /// <returns>
        /// The result of the matrix multiplication by number.
        /// </returns>
        public static Matrix operator *(double number, Matrix m)       //умножение матрицы на число
        {
            return m.Multiply(number);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="m1">The first matrix.</param>
        /// <param name="m2">The second matrix.</param>
        /// <returns>
        /// The result of the matrix multiplication by other matrix.
        /// </returns>
        public static Matrix operator *(Matrix m1, Matrix m2)       //умножение двух матриц
        {
            return m1.Multiply(m2);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="m">The matrix.</param>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The result of the matrix multiplication by vector.
        /// </returns>
        public static Vector operator *(Matrix m, Vector v)
        {
            return m.Multiply(v);
        }

        /////////////////////////////////////////////////////////////
        /////////////////////***Constructors***//////////////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="size">The size of the square matrix.</param>
        public Matrix(int size)
        {
            this.matrixBody = new double[size, size];
            this.matrixAns = new double[size];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="rows">The rows count.</param>
        /// <param name="columns">The columns count.</param>
        public Matrix(int rows, int columns)
        {
            this.matrixBody = new double[rows, columns];
            this.matrixAns = new double[rows];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="mBody">The matrix body (koef).</param>
        public Matrix(double[,] mBody)
        {
            this.matrixBody = (double[,])mBody.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="mBody">The matrix body (koef).</param>
        /// <param name="mAns">The result vector.</param>
        public Matrix(double[,] mBody, IEnumerable<double> mAns)
        {
            this.matrixBody = (double[,])mBody.Clone();
            this.matrixAns = mAns.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="vBody">The array of koef vectors.</param>
        /// <param name="vAns">The result vector.</param>
        public Matrix(IEnumerable<Vector> vBody, Vector vAns)
        {
            int size = vBody.Count();
            this.matrixBody = new double[size, size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    this.matrixBody[j, i] = vBody.ElementAt(i).Body[j];

            this.matrixAns = vAns.Body;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="vBody">The array of koef vectors.</param>
        public Matrix(IEnumerable<Vector> vBody)
        {
            for (int i = 0; i < vBody.Count(); i++)
                for (int j = 0; j < vBody.Count(); j++)
                    this.matrixBody[j, i] = vBody.ElementAt(i).Body[j];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="m">The object of the <see cref="Matrix"/> class.</param>
        public Matrix(Matrix m)
        {
            this.matrixBody = (double[,])m.matrixBody.Clone();
            if(m.MatrixAns != null)
                this.matrixAns = (double[])m.matrixAns.Clone();          
        }

        /////////////////////////////////////////////////////////////
        /////////////////////////***Methods***///////////////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Adds the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>
        /// The result of addition of matrices.
        /// </returns>
        public Matrix Add(Matrix matrix)
        {
            if (this.CountRows != matrix.CountRows || this.CountColumns != matrix.CountColumns)
                return null;
            
            Matrix m = new Matrix(this.CountRows, this.CountColumns);
            for (int i = 0; i < m.CountRows; i++)
                for (int j = 0; j < m.CountColumns; j++)
                {
                    m.matrixBody[i, j] = this.matrixBody[i, j] + matrix.matrixBody[i, j];
                }
            return m;         
        }

        /// <summary>
        /// Substract the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>
        /// The result of substraction of matrices.
        /// </returns>
        public Matrix Sub(Matrix matrix)
        {
            if (this.CountRows != matrix.CountRows || this.CountColumns != matrix.CountColumns)
                return null;
            
            Matrix m = new Matrix(this.CountRows, this.CountColumns);
            for (int i = 0; i < m.CountRows; i++)
                for (int j = 0; j < m.CountColumns; j++)
                    m.matrixBody[i, j] = this.matrixBody[i, j] - matrix.matrixBody[i, j];
                    
            return m;
        }

        /// <summary>
        /// Multiplication by vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The result of multiplication by vector.
        /// </returns>
        public Vector Multiply(Vector v)
        {
            if (this.CountColumns != v.Size)
                return null;
            
            Vector ans = new Vector(v.Size);
            for (int i = 0; i < this.CountRows; i++)
                for (int j = 0; j < this.CountColumns; j++)
                    ans.Body[i] += this.MatrixBody[i, j] * v.Body[j];

            return ans;     
        }

        /// <summary>
        /// Multiplication by matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>
        /// The result of multiplication by matrix.
        /// </returns>
        public Matrix Multiply(Matrix matrix)
        {
            if (this.CountColumns != matrix.CountRows)
                return null;
  
            Matrix m = new Matrix(this.CountRows, matrix.CountColumns);
            for (int c = 0; c < m.CountColumns; c++)
                for (int i = 0; i < m.CountRows; i++)
                    for (int j = 0; j < m.CountColumns; j++)                  
                        m.matrixBody[i, c] += this.matrixBody[i, j] * matrix.matrixBody[j, c];

            return m;
        }

        /// <summary>
        /// Multiplication by number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        /// The result of multiplication by number.
        /// </returns>
        public Matrix Multiply(double number)
        {
            Matrix m = new Matrix(this.CountRows, this.CountColumns);
            for (int i = 0; i < m.CountRows; i++)
                for (int j = 0; j < m.CountColumns; j++)
                    m.matrixBody[i, j] = this.matrixBody[i, j] * number;
  
            return m;
        }

        /// <summary>
        /// Rounds the elements of matrix.
        /// </summary>
        /// <param name="decimals">The decimals.</param>
        /// <returns>
        /// Matrix with rounded elements
        /// </returns>
        public Matrix RoundElements(int decimals)
        {
            Matrix m = new Matrix(this);
            for (int i = 0; i < m.CountRows; i++)
            {
                for (int j = 0; j < m.CountColumns; j++)
                {
                    m.MatrixBody[i, j] = Math.Round(m.MatrixBody[i, j], decimals);
                }
                if(m.MatrixAns != null)
                {
                    m.MatrixAns[i] = Math.Round(m.MatrixAns[i], decimals);
                }
            }
            return m;
        }

        /// <summary>
        /// Gets the trace of matrix.
        /// </summary>
        /// <returns>
        /// The trace value
        /// </returns>
        public double GetTrace()
        {
            double val = 0;
            for(int i = 0; i < CountRows; i++)
            {
                val += this.MatrixBody[i, i];
            }
            return val;
        }

        /// <summary>
        /// Sets the row of matrix.
        /// </summary>
        /// <param name="index">The index of row.</param>
        /// <param name="arr">The new row.</param>
        public void SetRow(int index, IEnumerable<double> arr)
        {
            for (int i = 0; i < CountColumns; i++)
                MatrixBody[index, i] = arr.ElementAt(i);
        }

        /// <summary>
        /// Gets the row of matrix.
        /// </summary>
        /// <param name="index">The index of row.</param>
        /// <returns>
        /// The vector of row
        /// </returns>
        public Vector GetRow(int index)
        {
            double[] ans = new double[CountColumns];
            for (int i = 0; i < CountColumns; i++)
                ans[i] = MatrixBody[index, i];

            return new Vector(ans);
        }

        /// <summary>
        /// To the vectors collection.
        /// </summary>
        /// <returns>
        /// Vectors collection
        /// </returns>
        public IEnumerable<Vector> ToVectors()
        {
            Vector[] vectors = new Vector[CountColumns];
            double[] ans = new double[CountColumns];
            for (int i = 0; i < CountRows; i++)
            {
                for (int j = 0; j < CountColumns; j++)
                {
                    ans[j] = MatrixBody[j, i];
                }
                vectors[i] = new Vector(ans);
            }
            return vectors;
        }

        /// <summary>
        /// Gets the triangle matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix GetTriangleMatrix()
        {
            Matrix m = new Matrix(this.matrixBody, this.matrixAns);
            double[,] a = m.matrixBody;
            double[] x = m.matrixAns;
            double h;

            for (int k = 0; k < m.CountColumns; k++)
            {
                for (int i = k + 1; i < m.CountRows; i++)
                {
                    h = a[i, k] / a[k, k];
                    for (int j = 0; j < m.CountColumns; j++)
                    {
                        a[i, j] -= h * a[k, j];
                    }
                    
                    x[i] -= h * x[k];
                }
            }
            
            return m;
        }

        //-------------------Methods for calculating matrix arguments------------------//

        /// <summary>
        /// Gets the arguments of matrix.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetArgs()
        {
            return GetArgsByGauss();
        }

        /// <summary>
        /// Gets the arguments of matrix by Gauss method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetArgsByGauss()
        {
            Matrix m = GetTriangleMatrix();
            double[,] a = m.matrixBody;
            double[] x = m.matrixAns;
            int n = CountRows;

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

        //-------------------Methods for computing the inverse matrix of a matrix------------------//

        /// <summary>
        /// Gets the reverse matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix GetReverseMatrix()
        {
            return GetReverseMatrixByGauss();
        }

        /// <summary>
        /// Gets the reverse matrix by Gauss method.
        /// </summary>
        /// <returns></returns>
        public Matrix GetReverseMatrixByGauss()
        {
            int n = CountRows;
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

                double[] x = m.GetArgsByGauss().ToArray();
                for (int j = 0; j < n; j++)
                {
                    a[j, i] = x[j];
                }
                
            }
            return new Matrix(a);
        }

        //-------------------Methods for calculating the determinant of a matrix------------------//

        /// <summary>
        /// Gets the determinant of matrix.
        /// </summary>
        /// <returns></returns>
        public double GetDeterminant()
        {
            return GetDeterminantByGauss();
        }

        /// <summary>
        /// Gets the determinant of matrix by decomposition method.
        /// </summary>
        /// <returns></returns>
        public double GetDeterminantByDecomposition()
        {
            if (CountRows == 2 && CountColumns == 2)
            {
                return matrixBody[0, 0] * matrixBody[1, 1] - matrixBody[0, 1] * matrixBody[1, 0];
            }
            else
            {
                double det = 0;
                for (int i = 0; i < CountColumns; i++)
                    det += matrixBody[0, i] * Math.Pow(-1, i + 2) * new Matrix(GetMinor(0, i)).GetDeterminantByDecomposition();
                return det;
            }
        }

        /// <summary>
        /// Gets the determinant by Gauss method.
        /// </summary>
        /// <returns></returns>
        public double GetDeterminantByGauss()
        {
            Matrix m = GetTriangleMatrix();
            double[,] a = m.matrixBody;
            int n = CountRows;
            double val = 1;
            for (int i = 0; i < n; i++)
            {
                val *= a[i, i];
            }
            return val;
        }

        //---------------------------------------

        /// <summary>
        /// Gets the minor matrix by element.
        /// </summary>
        /// <param name="mRow">The element row.</param>
        /// <param name="mColumn">The element column.</param>
        /// <returns></returns>
        public Matrix GetMinorMatrix(int mRow, int mColumn)
        {
            return new Matrix(GetMinor(mRow, mColumn));
        }

        /// <summary>
        /// Gets the minor matrix by element.
        /// </summary>
        /// <param name="mRow">The element row.</param>
        /// <param name="mColumn">The element column.</param>
        /// <returns></returns>
        public double[,] GetMinor(int mRow, int mColumn)
        {
            double[,] minor = new double[CountRows - 1, CountColumns - 1];
            for (int i = 0, I = 0; i < CountRows; i++)
            {
                if (i != mRow)
                {
                    for (int j = 0, J = 0; j < CountColumns; j++)
                    {
                        if (j != mColumn)
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

        //-------------------Methods for calculating eigenvalues ​​and matrix vectors------------------//

        /// <summary>
        /// Gets the eigen values of matrix.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetEigenValues()
        {
            double[] p = GetCharactPolynom().Select(e => e*(-1)).ToArray();
            Function f = new Function(p);
            return f.FindRoots_CHORD(eps:0.00001, steps:1000).ToArray();
        }

        /// <summary>
        /// Gets the eigen vectors of matrix.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vector> GetEigenVectors()
        {
            int n = CountRows;
            double[] la = GetEigenValues().ToArray();
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
                y = Matrix.GetIdentityMatrix(n).ToVectors().ToArray()[0];
                for (int j = 0; j < n - 1; j++)
                {
                    b = arrB[j].ToVectors().ToArray()[0];
                    y = y * la[i] + b;
                }
                res[i] = new Vector(y);
            }

            return res;
        }

        //-------------------Methods for computing the characteristic polynomial of a matrix------------------//

        /// <summary>
        /// Gets the charact polynom of matrix.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetCharactPolynom()
        {
            return GetCharactPolynomByLeverrierFaddeev();
        }

        /// <summary>
        /// Gets the charact polynom of matrix by Leverrier method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetCharactPolynomByLeverrier()
        {
            Matrix a = new Matrix(this);
            double[] p = new double[CountRows + 1],
                     s = new double[CountRows];
            p[0] = -1;
            for (int i = 0; i < CountRows; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                    sum += p[j + 1] * s[i - j - 1];

                s[i] = a.GetTrace();
                p[i + 1] = 1.0 / (i + 1) * (s[i] - sum);
                a *= this;
            }
            return p;
        }

        /// <summary>
        /// Gets the charact polynom of matrix by Leverrier-Faddeev method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetCharactPolynomByLeverrierFaddeev()
        {
            Matrix a = new Matrix(this), b;
            double val;
            double[] p = new double[CountRows + 1];
            p[0] = -1;

            for(int i = 0; i < CountRows; i++)
            {
                val = a.GetTrace()/(i+1);
                p[i + 1] = val;
                b = a - val * a.IdentityMatrix;
                a = this * b;
            }

            return p;
        }

        /// <summary>
        /// Gets the charact polynom of matrix by Danilevskiy method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetCharactPolynomByDanilevskiy()
        {
            Vector coefs = new Vector(CountRows + 1);
            Matrix s = Matrix.GetIdentityMatrix(CountRows);
            Matrix m;
            Matrix mInv;
            Matrix a = new Matrix(this);
            for (int i = CountRows - 2; i >= 0; i--)
            {
                m = Matrix.GetIdentityMatrix(CountRows);
                m.MatrixBody[i, i] = 1 / a.MatrixBody[i + 1, i];
                for (int j = 0; j < CountRows; j++)
                {
                    if (i != j)
                        m.MatrixBody[i, j] = -a.MatrixBody[i + 1, j] / a.MatrixBody[i + 1, i];
                }
                mInv = Matrix.GetIdentityMatrix(CountRows);
                mInv.SetRow(i, a.GetRow(i + 1).Body);
                a = mInv * a * m;
                s *= m;
            }
            coefs.Body[0] = -1;

            for (int i = 1; i < coefs.Body.Length; i++)
                coefs.Body[i] = a.MatrixBody[0, i - 1];

            return coefs.Body;
        }

        /// <summary>
        /// Gets the charact polynom of matrix by Krylov method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetCharactPolynomByKrylov()
        {
            Matrix a = new Matrix(this);
            Vector c = new Vector(new double[CountRows]);
            c.Body[0] = 1;

            var vectors = new Vector[CountRows + 1];
            vectors[0] = c;
            for (int i = 1; i < CountRows + 1; i++)
            {
                vectors[i] = a * c;
                a *= this;
            }
            Matrix mAns = new Matrix(vectors.Take(CountRows), vectors[CountRows]);
            var ans = mAns.GetArgs().ToList();
            ans.Add(-1);
            ans.Reverse();

            return ans;
        }

        /////////////////////////////////////////////////////////////
        ///////////////////***Static methods***//////////////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        /// <param name="size">The size of the identity matrix.</param>
        /// <returns></returns>
        public static Matrix GetIdentityMatrix(int size)
        {
            double[,] m = new double[size, size];
            for (int i = 0; i < size; i++)
                m[i, i] = 1;

            return new Matrix(m);
        }

        /////////////////////////////////////////////////////////////
        ///////////***Methods for HTML-reports generating***/////////
        /////////////////////////////////////////////////////////////
        /*public Matrix GetTriangleMatrix_HtmlReport(string reportName = "report.html", bool IsGenerate = true)   //метод приведения матрицы к треугольному виду
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
        }*/

    }
}
