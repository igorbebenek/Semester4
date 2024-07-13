using System;
using System.Numerics;


class Program
{
    static void Main()
    {
        double[] array = { -2, 3, 0, 1 }; 
        int N = array.Length;
        Complex[] rezultat = new Complex[N];

        for (int k = 0; k < N; k++)
        {
            for (int n = 0; n < N; n++)
            {
                double angle = -2 * Math.PI * k * n / N;
                rezultat[k] += array[n] * Complex.Exp(new Complex(0, angle));
            }
        }

        foreach (Complex c in rezultat)
        {
            Console.WriteLine("Wyniki : {0}", c);
        }
    }
}


