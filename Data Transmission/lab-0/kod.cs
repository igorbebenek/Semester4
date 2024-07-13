using ScottPlot; 
using System;

class Program
{
    static void Main()
    {
        double Minimum = -10;
        double Maksimum = 10;
        int liczbaPunktow = 100; 

        double[] x = new double[liczbaPunktow]; 
        double[] y = new double[liczbaPunktow]; 

        double krok = (Maksimum - Minimum) / (liczbaPunktow - 1); 

        for (int i = 0; i < liczbaPunktow; i++)
        {
            x[i] = Minimum + i * krok; 
            y[i] = x[i];
        }

        var plt = new ScottPlot.Plot(600, 400); 
        plt.AddScatter(x, y);
        plt.Title("Wykres f(x) = x");
        plt.XLabel("X");
        plt.YLabel("f(X)");
        plt.Grid(enable: true);

        plt.SaveFig("C:\\Users\\igorb\\Desktop\\liniowa.png");

        Console.WriteLine("Zrobione");
    }
}