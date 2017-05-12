using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace MathLib
{
    /// <summary>
    /// Represent mathematical vector and methods to work with it
    /// </summary>
    public class Vector
    {
        List<double> _body;

        /// <summary>
        /// Gets the body of vector.
        /// </summary>
        /// <value>
        /// The body of vector.
        /// </value>
        public List<double> Body
        {
            get
            {
                return _body;
            }
        }

        /// <summary>
        /// Gets the size of vector.
        /// </summary>
        /// <value>
        /// The size of vector.
        /// </value>
        public int Size
        {
            get
            {
                return _body.Count();
            }
        }

        /////////////////////////////////////////////////////////////
        ///////////////////***Operators implementing***///////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return v1.Add(v2);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static string operator +(string s, Vector v)
        {
            return s + v.ToString();
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="v">The vector</param>
        /// <param name="s">The string</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static string operator +(Vector v, string s)
        {
            return v.ToString() + s;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return v1.Sub(v2);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="number">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vector operator *(Vector v, double number)
        {
            return v.Multiply(number);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="v">The vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vector operator *(double number, Vector v)
        {
            return v.Multiply(number);
        }

        /////////////////////////////////////////////////////////////
        ///////////////////***Indexers implementing***///////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or sets the <see cref="System.Double"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Double"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public double this[int index]
        {
            get
            {
                return _body[index];
            }
            set
            {
                _body[index] = value;
            }
        }

        /////////////////////////////////////////////////////////////
        ////////////////***Constructors implementing***//////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="size">The size of vector.</param>
        public Vector(int size)
        {
            _body = new List<double>(size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="v">Elements collection.</param>
        public Vector(IEnumerable<double> v)
        {
            _body = v.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="v">The vector object.</param>
        public Vector(Vector v)
        {
            _body = v.Body.ToList();
        }

        /////////////////////////////////////////////////////////////
        ///////////////////////***Methods***/////////////////////////
        /////////////////////////////////////////////////////////////

        /// <summary>
        /// Add the vector.
        /// </summary>
        /// <param name="v2">The vector.</param>
        /// <returns></returns>
        public Vector Add(Vector v2)
        {
            Vector v = new Vector(this);
            for (int i = 0; i < Size; i++)
                v[i] += v2[i];
            
            return v;
        }

        /// <summary>
        /// Substract the vector.
        /// </summary>
        /// <param name="v2">The vector.</param>
        /// <returns></returns>
        public Vector Sub(Vector v2)
        {
            Vector v = new Vector(this);
            for (int i = 0; i < Size; i++)
                v[i] -= v2[i];

            return v;
        }

        /// <summary>
        /// Multiplication by number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public Vector Multiply(double number)
        {
            Vector v = new Vector(this);
            for(int i = 0; i < Size; i++)
                v[i] *= number;
     
            return v;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for(int i = 0; i < Size; i++)
            {
                s.Append(_body[i]);
                if (i != Size - 1)
                    s.Append("; ");
            }
            return string.Format("({0})", s.ToString());
        }

    }
}
