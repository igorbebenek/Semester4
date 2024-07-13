using System;
using System.Linq;
using ScottPlot;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
public class Program
{
    static int B = 10;
    static int W = 2;
    static double Tc = 5;
    static int fs = 168;
    static double Tb = Tc / B;
    static double fn = W / Tb;
    static double fn1 = (W + 1) / Tb;
    static double fn2 = (W + 2) / Tb;
    static int N = (int)(fs * Tc);
    static double[] X = Enumerable.Range(0, N).Select(i => i * (Tc / (double)N)).ToArray();
    
    static double[] ASK(bool[] bity)
    {
        double[] sygnal = new double[N];
        double A1 = 1;
        double A2 = 2;
        int probkiBit = (int)(fs * Tb);
        for (int i = 0; i < bity.Length; i++)
        {
            double amplituda = bity[i] ? A2 : A1;
            for (int j = 0; j < probkiBit && (i * probkiBit + j) < N; j++)
            {
                int index = i * probkiBit + j;
                double time = index / (double)fs;
                sygnal[index] = amplituda * Math.Sin(2 * Math.PI * fn * time);
            }
        }
        return sygnal;
    }

    static double[] PSK(bool[] bity)
    {
        double[] sygnal = new double[N];
        int probkiBit = (int)(fs * Tb);
        for (int i = 0; i < bity.Length; i++)
        {
            double faza = bity[i] ? Math.PI : 0;
            for (int j = 0; j < probkiBit && (i * probkiBit + j) < N; j++)
            {
                int index = i * probkiBit + j;
                double czas = index / (double)fs;
                sygnal[index] = Math.Sin(2 * Math.PI * fn * czas + faza);
            }
        }
        return sygnal;
    }

    static double[] FSK(bool[] bity)
    {
        double[] sygnal = new double[N];
        int probkiBit = (int)(fs * Tb);
        for (int i = 0; i < bity.Length; i++)
        {
            double czestotliwosc = bity[i] ? fn2 : fn1;
            for (int j = 0; j < probkiBit && (i * probkiBit + j) < N; j++)
            {
                int index = i * probkiBit + j;
                double czas = index / (double)fs;
                sygnal[index] = Math.Sin(2 * Math.PI * czestotliwosc * czas);
            }
        }
        return sygnal;
    }
    
    static (double[] Frequency, double[] Magnitude) DostanWidmo(double[] signal)
    {
        var complexSignal = new System.Numerics.Complex[signal.Length];
        for (int i = 0; i < signal.Length; i++)
            complexSignal[i] = new System.Numerics.Complex(signal[i], 0);

        Fourier.Forward(complexSignal, FourierOptions.Matlab);

        var czestotliwosci = Enumerable.Range(0, signal.Length / 2).Select(i => i * fs / (double)signal.Length).ToArray();
        var magnitudy = complexSignal.Take(signal.Length / 2).Select(x => 20 * Math.Log10(x.Magnitude)).ToArray();

        return (czestotliwosci, magnitudy);
    }

    static void RysujSygnal(string tytuł, double[] X, double[] dane, string nazwaPliku)
    {
        var plt = new ScottPlot.Plot(800, 400);
        plt.AddSignalXY(X, dane);
        plt.Title(tytuł);
        plt.YLabel("Amplituda");
        plt.XLabel("Czas (s)");
        plt.SaveFig(nazwaPliku);
    }
    
    static void RysujWidmo(double[] signal, string tytul)
    {
        var complexSygnal = new System.Numerics.Complex[signal.Length];
        for (int i = 0; i < signal.Length; i++)
            complexSygnal[i] = new System.Numerics.Complex(signal[i], 0);

        Fourier.Forward(complexSygnal, FourierOptions.Matlab);
        

        var plt = new ScottPlot.Plot(800, 400);
        var czestotliwosci = Enumerable.Range(0, signal.Length / 2)
            .Select(i => i * fs / (double)signal.Length)
            .ToArray();
        var magnitudy = complexSygnal.Take(signal.Length / 2)
            .Select(x => 20 * Math.Log10(x.Magnitude))
            .ToArray();

        plt.AddScatter(czestotliwosci, magnitudy);
        plt.Title(tytul);
        plt.YLabel("Magnituda (dB)");
        plt.XLabel("Czestotliwosc (Hz)");
        plt.SaveFig(tytul.Replace(" ", "") + ".png");
    }
    
    static double ObliczSzerokoscPasma(double[] magnitudy, double[] czestotliwosci, double db)
    {
        double maxAmp = magnitudy.Max();
        double poziomOdciecia = maxAmp - db;
        double fMin = 0;
        double fMax = 0;
        bool czyWykrytoPasmo = false;

        for (int i = 0; i < magnitudy.Length; i++)
        {
            if (magnitudy[i] >= poziomOdciecia)
            {
                if (!czyWykrytoPasmo)
                {
                    fMin = czestotliwosci[i];
                    czyWykrytoPasmo = true;
                }
                fMax = czestotliwosci[i];
            }
        }

        return fMax - fMin; 
    }
    
    
    
     public static void Main(string[] args)
    {
        int[] bitArray = {0,1,0,0,1,1,0,1,0,1,1,0,0,0,0,1,0,1,1,1,0,0,1,0,0,1,1,0,0,0,1,1,0,1,1,0,1,0,0,1,0,1,1,0,1,1,1,0};  
        bool[] bitStream = bitArray.Select(bit => bit == 1).ToArray();  // zmieniamy na boola

        double[] askSyg = ASK(bitStream);
        double[] pskSYg = PSK(bitStream);
        double[] fskSyg = FSK(bitStream);

        RysujSygnal("ASK", X, askSyg, "za.png");
        RysujSygnal("PSK", X, pskSYg, "zp.png");
        RysujSygnal("FSK", X, fskSyg, "zf.png");
        
        
        RysujWidmo(askSyg, "za_widmo");
        RysujWidmo(pskSYg, "zp_widmo");
        RysujWidmo(fskSyg, "zf_widmo");
        
        var askSpectrum = DostanWidmo(askSyg);
        var pskSpectrum = DostanWidmo(pskSYg);
        var fskSpectrum = DostanWidmo(fskSyg);
        
        Console.WriteLine($"ASK pasmo 3 dB: {ObliczSzerokoscPasma(askSpectrum.Magnitude, askSpectrum.Frequency, 3)} Hz");
        Console.WriteLine($"ASK pasmo 6 dB: {ObliczSzerokoscPasma(askSpectrum.Magnitude, askSpectrum.Frequency, 6)} Hz");
        Console.WriteLine($"ASK pasmo 12 dB: {ObliczSzerokoscPasma(askSpectrum.Magnitude, askSpectrum.Frequency, 12)} Hz");

        Console.WriteLine($"PSK pasmo 3 dB: {ObliczSzerokoscPasma(pskSpectrum.Magnitude, pskSpectrum.Frequency, 3)} Hz");
        Console.WriteLine($"PSK pasmo 6 dB: {ObliczSzerokoscPasma(pskSpectrum.Magnitude, pskSpectrum.Frequency, 6)} Hz");
        Console.WriteLine($"PSK pasmo 12 dB: {ObliczSzerokoscPasma(pskSpectrum.Magnitude, pskSpectrum.Frequency, 12)} Hz");

        Console.WriteLine($"FSK pasmo 3 dB: {ObliczSzerokoscPasma(fskSpectrum.Magnitude, fskSpectrum.Frequency, 3)} Hz");
        Console.WriteLine($"FSK pasmo 6 dB: {ObliczSzerokoscPasma(fskSpectrum.Magnitude, fskSpectrum.Frequency, 6)} Hz");
        Console.WriteLine($"FSK pasmo 12 dB: {ObliczSzerokoscPasma(fskSpectrum.Magnitude, fskSpectrum.Frequency, 12)} Hz");
    }
    
}
