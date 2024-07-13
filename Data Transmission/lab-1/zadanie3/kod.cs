using System;
using ScottPlot;

class Program
{
    static void Main()
    {
        double fs = 8000.0; 
        double Tc = 1.0; 
        int N = (int)Math.Round(Tc * fs);

        double[] t = new double[N];
        double[] u = new double[N];

        for (int i = 0; i < N; i++)
        {
            t[i] = i / fs;
            if (t[i] >= 0 && t[i] < 0.1)
            {
                u[i] = Math.Sin(6 * Math.PI * t[i]) * Math.Cos(5 * Math.PI * t[i]);
            }
            else if (t[i] >= 0.1 && t[i] < 0.4)
            {
                u[i] = -1.1 * t[i] * Math.Cos(41 * Math.PI * Math.Pow(t[i], 2));
            }
            else if (t[i] >= 0.4 && t[i] < 0.72)
            {
                u[i] = t[i] * Math.Sin(20 * Math.Pow(t[i], 4));
            }
            else if (t[i] >= 0.72 && t[i] <= 1)
            {
                u[i] = 3.3 * (t[i] - 0.72) * Math.Cos(27 * t[i] + 1.3);
            }
        }

        var plot = new ScottPlot.Plot(600, 400);
        plot.AddSignal(u, fs);
        plot.YLabel("u(t)");
        plot.SaveFig("C:\\Users\\igorb\\Desktop\\u.png");
        Console.WriteLine("Zapisane");
    }
}