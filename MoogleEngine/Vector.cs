using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public struct Vector
    {
        public double[] elements;
        int count;
        public Vector(double[] elements)
        {
            this.elements = elements;
            this.count = elements.Length;
        }
        public double this[int i]
        {
            get
            {
                return elements[i];
            }
            set
            {
                elements[i] = value;
            }
        }
        public static double operator *(Vector a, Vector b)
        {
            if (a.count != b.count)
                throw new ArgumentException("Vectors have not the same size");
            double result = 0;
            for (int i = 0; i < a.count; i++)
            {
                result += a.elements[i] * b.elements[i];
            }
            return result;
        }
    }
}
