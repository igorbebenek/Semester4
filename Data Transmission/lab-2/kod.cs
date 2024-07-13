using System;
using System.Linq;
using System.Numerics;
using ScottPlot;
using System.Diagnostics;
using MathNet.Numerics.IntegralTransforms;

namespace Lab2
{
    class Program
    {
        static Complex[] DFT(double[] x)
        {
            int N = x.Length;
            Complex[] result = new Complex[N];
            for (int k = 0; k < N; k++)
            {
                Complex sum = 0;
                for (int n = 0; n < N; n++)
                {
                    double angle = -2 * Math.PI * k * n / N;
                    sum += x[n] * Complex.Exp(new Complex(0, angle));
                }
                result[k] = sum;
            }
            return result;
        }

        static Complex[] FFT(double[] x)
        {
            Complex[] result = x.Select(v => new Complex(v, 0)).ToArray();
            Fourier.Forward(result, FourierOptions.Matlab);
            return result;
        }

        static double[] CalculateAmplitude(Complex[] dftResult)
        {
            return dftResult.Select(c => c.Magnitude).ToArray();
        }

        static double[] dbScale(double[] input)
        {
            double[] output = new double[input.Length];
            for (int k = 0; k < input.Length; k++)
            {
                output[k] = 10 * Math.Log10(Math.Max(input[k], 1e-10)); // Avoid log(0) error
            }
            return output;
        }

        static double[] FreqScale(double fs, int N)
        {
            return Enumerable.Range(0, N / 2).Select(k => k * fs / N).ToArray();
        }

        static void PlotSignal(double[] x, double[] y, string title, string fileName)
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddScatter(x, y);
            plt.Title(title);
            plt.SaveFig(fileName);
        }

        static void MeasureAndPlotDFT(double[] signal, double fs, string title, string spectrumFileName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Complex[] dftResult = DFT(signal);
            stopwatch.Stop();
            Console.WriteLine($"Time taken for DFT of {title}: {stopwatch.ElapsedTicks} ticks ({stopwatch.Elapsed.TotalMilliseconds} ms)");

            double[] amplitude = CalculateAmplitude(dftResult);
            double[] decibels = dbScale(amplitude);
            double[] freqScale = FreqScale(fs, signal.Length);

            PlotSignal(freqScale, decibels.Take(signal.Length / 2).ToArray(), title, spectrumFileName);
        }

        static void MeasureFFT(double[] signal, string title)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Complex[] fftResult = FFT(signal);
            stopwatch.Stop();
            Console.WriteLine($"Time taken for FFT of {title}: {stopwatch.ElapsedTicks} ticks ({stopwatch.Elapsed.TotalMilliseconds} ms)");
        }

        static void Main(string[] args)
        {
            double fs = 100;
            double Tc = 2;
            int N = (int)(fs * Tc);

            double[] t = Enumerable.Range(0, N).Select(i => (double)i / fs).ToArray();

            double f1 = 5;
            double[] x = new double[N];
            double[] y = new double[N];
            double[] z = new double[N];
            double[] v = new double[N];
            double[] u = new double[N];
            double[][] bk = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                bk[i] = new double[N];
            }

            for (int i = 0; i < N; i++)
            {
                x[i] = Math.Abs(Math.Pow(Math.Sin(2 * Math.PI * f1 * t[i] * t[i]), 13)) + Math.Cos(2 * Math.PI * t[i]);
                y[i] = Math.Pow(t[i], 3) - 1 + Math.Cos(4 * Math.Pow(t[i], 2) * Math.PI) * t[i];
                z[i] = x[i] / (Math.Abs(y[i] * Math.Cos(5 * t[i]) - x[i] * y[i]) + 3);
                v[i] = x[i] * 662 / (Math.Abs(x[i] - y[i] + 0.5));

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

                bk[0][i] = Math.Sin(2 * Math.PI * 1 * t[i]);
                bk[1][i] = Math.Sin(2 * Math.PI * 2 * t[i]);
                bk[2][i] = Math.Sin(2 * Math.PI * 3 * t[i]);
            }

            MeasureAndPlotDFT(x, fs, "X", "x.png");
            MeasureFFT(x, "Zadanie 3 - Sygnal X");

            MeasureAndPlotDFT(y, fs, "Y", "y.png");
            MeasureFFT(y, "Zadanie 3 - Sygnal Y");

            MeasureAndPlotDFT(z, fs, "Z", "z.png");
            MeasureFFT(z, "Zadanie 3 - Sygnal Z");

            MeasureAndPlotDFT(v, fs, "V", "v.png");
            MeasureFFT(v, "Zadanie 3 - Sygnal V");

            MeasureAndPlotDFT(u, fs, "U", "u.png");
            MeasureFFT(u, "Zadanie 3 - Sygnal U");

            MeasureAndPlotDFT(bk[0], fs, "BK1", "b1.png");
            MeasureFFT(bk[0], "Zadanie 3 - Sygnal BK1");

            MeasureAndPlotDFT(bk[1], fs, "BK2", "b2.png");
            MeasureFFT(bk[1], "Zadanie 3 - Sygnal BK2");

            MeasureAndPlotDFT(bk[2], fs, "BK3", "b3.png");
            MeasureFFT(bk[2], "BK3");
        }
    }
}
