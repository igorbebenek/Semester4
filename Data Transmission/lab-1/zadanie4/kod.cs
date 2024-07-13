using System;
using ScottPlot;

class Program
{
    static void Main(string [] args)
    {
        double fs = 22050.0; 
        double Tc = 1.0; 
        int N = (int)Math.Round(Tc * fs); 

        double[] t = new double[N]; 
        double[][] bk = new double[3][]; 

        
        for (int k = 0; k < 3; k++)
        {
            bk[k] = new double[N];
        }

        for (int i = 0; i < N; i++)
        {
            t[i] = i / fs;
            bk[0][i] = Math.Sin(2 * Math.PI * 1 * t[i]); 
            bk[1][i] = Math.Sin(2 * Math.PI * 2 * t[i]); 
            bk[2][i] = Math.Sin(2 * Math.PI * 3 * t[i]); 
        }

        var plot1 = new ScottPlot.Plot(600, 400);
        plot1.AddSignal(bk[0], fs);
        plot1.Title("Sygnał 1");
        plot1.SaveFig("C:\\Users\\igorb\\Desktop\\sygnał1.png");
        var plot2 = new ScottPlot.Plot(600, 400);
        plot2.AddSignal(bk[1], fs);
        plot2.Title("Sygnał 2");
        plot2.SaveFig("C:\\Users\\igorb\\Desktop\\sygnał2.png");
        var plot3 = new ScottPlot.Plot(600, 400);
        plot3.AddSignal(bk[2], fs);
        plot3.Title("Sygnał 3");
        plot3.SaveFig("C:\\Users\\igorb\\Desktop\\sygnał3.png");

        Console.WriteLine("Zapisane");
    }
}
