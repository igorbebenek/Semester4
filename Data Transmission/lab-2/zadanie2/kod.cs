using System;
using System.Numerics;
using ScottPlot;

class Program
{
    static double[] GenerujSygnal(int iloscProbek, double czestotliwoscProbkowania, double amplituda, double czestotliwosc1, double czestotliwosc2)
    {
        double[] sygnal = new double[iloscProbek];
        for (int i = 0; i < iloscProbek; i++)
        {
            double czas = i / czestotliwoscProbkowania;
            sygnal[i] = amplituda * Math.Sin(2 * Math.PI * czestotliwosc1 * czas) + amplituda * Math.Sin(2 * Math.PI * czestotliwosc2 * czas);
        }
        return sygnal;
    }

    static Complex[] ObliczDFT(double[] sygnal, int iloscProbek)
    {
        Complex[] wyniki = new Complex[iloscProbek];
        for (int k = 0; k < iloscProbek; k++)
        {
            Complex suma = new Complex(0, 0);
            for (int n = 0; n < iloscProbek; n++)
            {
                double kat = -2 * Math.PI * k * n / iloscProbek;
                suma += sygnal[n] * Complex.Exp(new Complex(0, kat));
            }
            wyniki[k] = suma;
        }
        return wyniki;
    }

    static (double[], double[], double[]) ObliczWidmo(Complex[] dft, int iloscProbek, double czestotliwoscProbkowania)
    {
        double[] amplitudy = new double[iloscProbek / 2];
        double[] decybele = new double[iloscProbek / 2];
        double[] czestotliwosci = new double[iloscProbek / 2];
        for (int k = 0; k < iloscProbek / 2; k++)
        {
            amplitudy[k] = Math.Sqrt(dft[k].Real * dft[k].Real + dft[k].Imaginary * dft[k].Imaginary);
            decybele[k] = 10 * Math.Log10(amplitudy[k]);
            czestotliwosci[k] = k * czestotliwoscProbkowania / iloscProbek;
        }
        return (czestotliwosci, decybele, amplitudy);
    }

    static void Main(string[] args)
    {
        
        double czasTrwaniaSygnalu = 0.1;
        double czestotliwoscProbkowania = 40000.0;
        int iloscProbek = (int)(czasTrwaniaSygnalu * czestotliwoscProbkowania);

        double amplituda = 200;
        double czestotliwosc1 = 100;
        double czestotliwosc2 = 1000;

        double[] sygnal = GenerujSygnal(iloscProbek, czestotliwoscProbkowania, amplituda, czestotliwosc1, czestotliwosc2);
        Complex[] wynikiDFT = ObliczDFT(sygnal, iloscProbek);
        var (czestotliwosci, decybele, _) = ObliczWidmo(wynikiDFT, iloscProbek, czestotliwoscProbkowania);

        var wykres = new ScottPlot.Plot(600, 400);
        wykres.AddScatterLines(czestotliwosci, decybele, lineWidth: 2);
        wykres.Title("Widmo Amplitudowe");
        wykres.XLabel("Częstotliwość");
        wykres.YLabel("Amplituda");
        wykres.SetAxisLimits(yMin: -100);
        wykres.SaveFig("widmoZadanie2.png");
    }
}
