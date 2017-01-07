﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    public class Vector
    {
        double[] body;
        public double[] Body
        {
            get
            {
                return body;
            }
        }

        public int Size
        {
            get
            {
                return body.Length;
            }
        }

        public static Vector operator +(Vector v1, Vector v2)       //сложение векторов
        {
            return v1.Plus(v2);
        }

        public static string operator +(string s, Vector v)       //сложение вектора со строкой
        {
            return s + v.ToString();
        }

        public static string operator +(Vector v, string s)       //сложение вектора со строкой
        {
            return v.ToString() + s;
        }

        public static Vector operator -(Vector v1, Vector v2)       //вычитание векторов
        {
            return v1.Minus(v2);
        }

        public static Vector operator *(Vector v, double number)       //умножение вектора на число
        {
            return v.Multiply(number);
        }

        public static Vector operator *(double number, Vector v)       //умножение вектора на число
        {
            return v.Multiply(number);
        }

        //public static Vector operator *(Vector v1, Vector v2)       //умножение двух векторов
        //{
        //    return m1.Multiply(m2);
        //}

        public Vector(int size)
        {
            body = new double[size];
        }

        public Vector(double[] v)
        {
            body = (double[])v.Clone();
        }

        public Vector(Vector v)
        {
            body = (double[])v.Body.Clone();
        }

        public Vector Plus(Vector v2)
        {
            Vector v = new Vector(this);
            for (int i = 0; i < Size; i++)
            {
                v.Body[i] += v2.Body[i];
            }
            return v;
        }

        public Vector Minus(Vector v2)
        {
            Vector v = new Vector(this);
            for (int i = 0; i < Size; i++)
            {
                v.Body[i] -= v2.Body[i];
            }
            return v;
        }

        public Vector Multiply(double number)
        {
            Vector v = new Vector(this);
            for(int i = 0; i < Size; i++)
            {
                v.Body[i] *= number;
            }
            return v;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for(int i = 0; i < Size; i++)
            {
                s.Append(Body[i]);
                if (i != Size - 1)
                    s.Append("; ");
            }
            return string.Format("({0})", s.ToString());
        }

    }
}
