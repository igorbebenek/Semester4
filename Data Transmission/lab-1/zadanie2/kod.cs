using System;
using ScottPlot;

class Program
{

//Zadanie 2 funkcja 3 
   static void Main()
    {
        double f = 1.0;
        double Tc = 1.0;
        double fs = 8000.0;
        int N = (int)Math.Round(Tc * fs);

        double[] czas = new double[N];
        double[] x = new double[N];
        double[] y = new double[N];
        double[] z = new double[N];
        double[] v = new double[N];

        for (int i = 0; i < N; i++)
        {
            czas[i] = i / fs;
            x[i] = Math.Abs(Math.Pow(Math.Sin(2 * Math.PI * f * czas[i] * czas[i]), 13)) +
                   Math.Cos(2 * Math.PI * czas[i]);
            y[i] = Math.Pow(czas[i], 3) - 1 + Math.Cos(4 * Math.Pow(czas[i], 2) * Math.PI) * czas[i];
            z[i] = x[i] / (Math.Abs(y[i] * Math.Cos(5 * czas[i]) - x[i] * y[i]) + 3);
            v[i] = x[i] * 662 / (Math.Abs(x[i] - y[i] + 0.5));
        }

        var wykresY = new ScottPlot.Plot(600, 400);
        wykresY.AddSignal(y, fs);
        wykresY.SaveFig("C:\\Users\\igorb\\Desktop\\y.png");

        var wykresZ = new ScottPlot.Plot(600, 400);
        wykresZ.AddSignal(z, fs);
        wykresZ.SaveFig("C:\\Users\\igorb\\Desktop\\z.png");

        var wykresV = new ScottPlot.Plot(600, 400);
        wykresV.AddSignal(v, fs);
        wykresV.SaveFig("C:\\Users\\igorb\\Desktop\\v.png");

        Console.WriteLine("Zapisane");
        
    }
}