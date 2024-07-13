using System;
using System.Linq;
using System.Numerics;
using ScottPlot;

namespace SygnałyModulowane
{
    class Program
    {
        static Complex[] DFT(double[] sygnal)
        {
            int N = sygnal.Length;
            Complex[] wyniki = new Complex[N];
            for (int k = 0; k < N; k++)
            {
                Complex sum = Complex.Zero;
                for (int n = 0; n < N; n++)
                {
                    double angle = -2 * Math.PI * k * n / N;
                    sum += sygnal[n] * Complex.Exp(new Complex(0, angle));
                }
                wyniki[k] = sum;
            }
            return wyniki;
        }

        static void StworzIZapiszWykres(string tytuł, double[] x, double[] y, string nazwaPliku)
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddSignal(y, sampleRate: (int)(1.0 / (x[1] - x[0])));
            plt.Title(tytuł);
            plt.YLabel("Amplituda");
            plt.XLabel("Czas");
            plt.SaveFig(nazwaPliku);
        }
        
        static void StworzIZapiszWykresZad3(string tytul, double[] czestotliwosci, double[] magnitudy, string nazwaPliku)
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddScatter(czestotliwosci, magnitudy);
            plt.Title(tytul);
            plt.XLabel("Częstotliwość (Hz)");
            plt.YLabel("Magnituda (dB)");
            plt.SaveFig(nazwaPliku);
        }

        static double[] MagnitudaWDecybelach(Complex[] wynikiDFT)
        {
            return wynikiDFT.Select(c => 20 * Math.Log10(Complex.Abs(c) + 1e-10)).ToArray();
        }
        
        static double ObliczSzerokoscPasma(double[] magnitudy, double[] czestotliwosci, double db)
        {
            double maxAmp = magnitudy.Max();
            double poziomOdciecia = maxAmp - 10 * Math.Log10(db);
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

        static void Main(string[] args)
        {
            double Tc = 2;
            double fm = 2;
            double fn = 20;
            double fs = 2 * Math.Max(fm, fn);
            int N = (int)(fs * Tc);
            double[] t = Enumerable.Range(0, N).Select(i => i / fs).ToArray();

            double[] m = t.Select(x => Math.Sin(2 * Math.PI * fm * x)).ToArray();

            Func<double, double[]> za = (ka) =>
                t.Select((x, i) => (ka * m[i] + 1) * Math.Cos(2 * Math.PI * fn * x)).ToArray();
            Func<double, double[]> zp = (kp) =>
                t.Select((x, i) => Math.Cos(2 * Math.PI * fn * x + kp * m[i])).ToArray();
            Func<double, double[]> zf = (kf) =>
                t.Select((x, i) => Math.Cos(2 * Math.PI * fn * x + (kf / fm) * m[i])).ToArray();

            /*StworzIZapiszWykres("Modulacja amplitudy ka=0.5", t, za(0.5), "zad1_amplitudaA.png");
            StworzIZapiszWykres("Modulacja amplitudy ka=7", t, za(7), "zad1_amplitudaB.png");
            StworzIZapiszWykres("Modulacja amplitudy ka=21", t, za(21), "zad1_amplitudaC.png");
            StworzIZapiszWykres("Modulacja fazy kp=0.5", t, zp(0.5), "zad1_fazaA.png");
            StworzIZapiszWykres("Modulacja fazy kp=pi/2", t, zp(Math.PI / 2), "zad1_fazaB.png");
            StworzIZapiszWykres("Modulacja fazy kp=2pi", t, zp(2 * Math.PI), "zad1_fazaC.png");
            StworzIZapiszWykres("Modulacja kf=0.5", t, zf(0.5), "zad1_czestotliwoscA.png");
            StworzIZapiszWykres("Modulacja kf=pi/2", t, zf(Math.PI / 2), "zad1_czestotliwoscB.png");
            StworzIZapiszWykres("Modulacja kf=2pi", t, zf(2 * Math.PI), "zad1_czestotliwoscC.png");*/
            double[] freqs = Enumerable.Range(0, N).Select(i => i * fs / N).ToArray();


            Complex[] dftZa1 = DFT(za(0.5));
            Complex[] dftZa2 = DFT(za(7));
            Complex[] dftZa3 = DFT(za(21));
            Complex[] dftZp1 = DFT(zp(0.5));
            Complex[] dftZp2 = DFT(zp(0.5 * Math.PI));
            Complex[] dftZp3 = DFT(zp(2 * Math.PI));
            Complex[] dftZf1 = DFT(zf(0.5));
            Complex[] dftZf2 = DFT(zf(0.5 * Math.PI));
            Complex[] dftZf3 = DFT(zf(2 * Math.PI));
            /*StworzIZapiszWykresZad3("Zad 3 za 0.5", freqs, MagnitudaWDecybelach(dftZa1), "DFT_za_0_5.png");
            StworzIZapiszWykresZad3("Zad 3 za 2", freqs, MagnitudaWDecybelach(dftZa2), "DFT_za_7.png");
            StworzIZapiszWykresZad3("Zad 3 za 21", freqs, MagnitudaWDecybelach(dftZa3), "DFT_za_21.png");
            StworzIZapiszWykresZad3("Zad 3 zp 0.5", freqs, MagnitudaWDecybelach(dftZp1), "zp_0_5.png");
            StworzIZapiszWykresZad3("Zad 3 zp 0.5pi", freqs, MagnitudaWDecybelach(dftZp2), "zp_0_5pi.png");
            StworzIZapiszWykresZad3("Zad 3 zp 2pi", freqs, MagnitudaWDecybelach(dftZp3), "zp_2pi.png");
            StworzIZapiszWykresZad3("Zad 3 zf 0.5", freqs, MagnitudaWDecybelach(dftZf1), "zf_0_5.png");
            StworzIZapiszWykresZad3("Zad 3 zf 0.5pi", freqs, MagnitudaWDecybelach(dftZf2), "zf_0_5pi.png");
            StworzIZapiszWykresZad3("Zad 3 zf 2pi", freqs, MagnitudaWDecybelach(dftZf3), "zf_2pi.png");*/
            
            double[] dbZa1 = MagnitudaWDecybelach(dftZa1);
            double[] dbZa2 = MagnitudaWDecybelach(dftZa2);
            double[] dbZa3 = MagnitudaWDecybelach(dftZa3);
            double[] dbZp1 = MagnitudaWDecybelach(dftZp1);
            double[] dbZp2 = MagnitudaWDecybelach(dftZp2);
            double[] dbZp3 = MagnitudaWDecybelach(dftZp3);
            double[] dbZf1 = MagnitudaWDecybelach(dftZf1);
            double[] dbZf2 = MagnitudaWDecybelach(dftZf2);
            double[] dbZf3 = MagnitudaWDecybelach(dftZf3);

            double dB3_za05 = ObliczSzerokoscPasma(dbZa1, freqs, 3);
            double dB6_za05 = ObliczSzerokoscPasma(dbZa1, freqs, 6);
            double dB12_za05 = ObliczSzerokoscPasma(dbZa1, freqs, 12);
            double dB3_za7 =  ObliczSzerokoscPasma(dbZa2, freqs, 3);
            double dB6_za7 =  ObliczSzerokoscPasma(dbZa2, freqs, 6);
            double dB12_za7 = ObliczSzerokoscPasma(dbZa2, freqs, 12);
            double dB3_za21 = ObliczSzerokoscPasma(dbZa3, freqs, 3);
            double dB6_za21 = ObliczSzerokoscPasma(dbZa3, freqs, 6);
            double dB12_za21 =ObliczSzerokoscPasma(dbZa3, freqs, 12);
            double dB3_zp05 =  ObliczSzerokoscPasma(dbZp1, freqs, 3);
            double dB6_zp05 =  ObliczSzerokoscPasma(dbZp1, freqs, 6);
            double dB12_zp05 =  ObliczSzerokoscPasma(dbZp1, freqs, 12);
            double dB3_zp05pi =  ObliczSzerokoscPasma(dbZp2, freqs, 3);
            double dB6_zp05pi =  ObliczSzerokoscPasma(dbZp2, freqs, 6);
            double dB12_zp05pi =  ObliczSzerokoscPasma(dbZp2, freqs, 12);
            double dB3_zp2pi =  ObliczSzerokoscPasma(dbZp3, freqs, 3);
            double dB6_zp2pi =  ObliczSzerokoscPasma(dbZp3, freqs, 6);
            double dB12_zp2pi =  ObliczSzerokoscPasma(dbZp3, freqs, 12);
            double dB3_zf05 =  ObliczSzerokoscPasma(dbZf1, freqs, 3);
            double dB6_zf05 =  ObliczSzerokoscPasma(dbZf1, freqs, 6);
            double dB12_zf05 = ObliczSzerokoscPasma(dbZf1, freqs, 12);
            double dB3_zf05pi = ObliczSzerokoscPasma(dbZf2, freqs, 3);
            double dB6_zf05pi = ObliczSzerokoscPasma(dbZf2, freqs, 6);
            double dB12_zf05pi = ObliczSzerokoscPasma(dbZf2, freqs, 12);
            double dB3_zf2pi = ObliczSzerokoscPasma(dbZf3, freqs, 3);
            double dB6_zf2pi = ObliczSzerokoscPasma(dbZf3, freqs, 6);
            double dB12_zf2pi =ObliczSzerokoscPasma(dbZf3, freqs, 12);

            Console.WriteLine("za 0.5 dB 3: " + dB3_za05);
            Console.WriteLine("za 0.5 dB 6: " + dB6_za05);
            Console.WriteLine("za 0.5 dB 12: " + dB12_za05);
            Console.WriteLine("za 7 dB 3: " + dB3_za7);
            Console.WriteLine("za 7 dB 6: " + dB6_za7);
            Console.WriteLine("za 7 dB 12: " + dB12_za7);
            Console.WriteLine("za 21 dB 3: " + dB3_za21);
            Console.WriteLine("za 21 dB 6: " + dB6_za21);
            Console.WriteLine("za 21 dB 12: " + dB12_za21);
            Console.WriteLine("zp 0.5 dB 3: " + dB3_zp05);
            Console.WriteLine("zp 0.5 dB 6: " + dB6_zp05);
            Console.WriteLine("zp 0.5 dB 12: " + dB12_zp05);
            Console.WriteLine("zp 0.5pi dB 3: " + dB3_zp05pi);
            Console.WriteLine("zp 0.5pi dB 6: " + dB6_zp05pi);
            Console.WriteLine("zp 0.5pi dB 12: " + dB12_zp05pi);
            Console.WriteLine("zp 2pi dB 3: " + dB3_zp2pi);
            Console.WriteLine("zp 2pi dB 6: " + dB6_zp2pi);
            Console.WriteLine("zp 2pi dB 12: " + dB12_zp2pi);
            Console.WriteLine("zf 0.5 dB 3: " + dB3_zf05);
            Console.WriteLine("zf 0.5 dB 6: " + dB6_zf05);
            Console.WriteLine("zf 0.5 dB 12: " + dB12_zf05);
            Console.WriteLine("zf 0.5pi dB 3: " + dB3_zf05pi);
            Console.WriteLine("zf 0.5pi dB 6: " + dB6_zf05pi);
            Console.WriteLine("zf 0.5pi dB 12: " + dB12_zf05pi);
            Console.WriteLine("zf 2pi dB 3: " + dB3_zf2pi);
            Console.WriteLine("zf 2pi dB 6: " + dB6_zf2pi);
            Console.WriteLine("zf 2pi dB 12: " + dB12_zf2pi);

            
            
            



        }


    }
}
