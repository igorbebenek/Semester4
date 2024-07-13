using System;
using ScottPlot;
//f = czestotliwosc Hz = 1/s
//fi - faza poczatkowa [rad]
//fs - czestotliwosc probkowania [hz]
//Tc - czas trwania sygnalu [s]
//N  - liczba probek
// f max - gorna granica czestotliwosci [hz]
// t - czas [s] <- t = n / fs
// Ts - okres probkowania [s]
// Tc = 2s, fs =  2000 Hz
// N = Tc * fs
// fmax = fs/2

// ZADANIE 1 FUNKCJA NUMER 2
double f = 1.0; //czest sygnalu
double Tc = 1.0; //czas trwania sygnalu
double fs = 8000.0; //czest probkowania
int N = (int)Math.Round(Tc * fs); 

double[] czas = new double[N]; 
double[] wartoscSygnalu = new double[N];

for (int n = 0; n < N; n++)
{
    czas[n] = n / fs;
    wartoscSygnalu[n] = Math.Abs(Math.Pow(Math.Sin(2 * Math.PI * f * czas[n] * czas[n]), 13)) + Math.Cos(2 * Math.PI * czas[n]);
}

var wykres = new ScottPlot.Plot(600, 400);
wykres.SetAxisLimits(yMin: -2, yMax: 2); 
wykres.SetAxisLimits(xMin: 0, xMax: Tc);
wykres.AddSignal(wartoscSygnalu,fs);
wykres.Title("Sygnał");
wykres.XLabel("Czas w sek");
wykres.YLabel("Maksymalna wartość x(t)");
wykres.SaveFig("C:\\Users\\igorb\\Desktop\\wykres_lab1_td.png");
Console.WriteLine("Zrobione");













