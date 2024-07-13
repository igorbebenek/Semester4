using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

 public class Demodulacja
    {
        private double A1 = 1;
        private double A2 = 0.5;
        private int B = 77;
        private double W = 2;
        private double Tc = 5;
        private double fs = 154;
        private double Tb;
        private double fn;
        private double fn1;
        private double fn2;
        private int N;
        private double[] t;
        private double[] X;
        private int P = 10;

        public Demodulacja()
        {
            Tb = Tc / B;
            fn = W / Tb;
            fn1 = (W + 1) / Tb;
            fn2 = (W + 2) / Tb;
            N = (int)(fs * Tc);
            t = Enumerable.Range(0, N).Select(i => i / (4.0 * fs)).ToArray();
            X = Enumerable.Range(0, N).Select(i => i * Tc / N).ToArray();
        }

        private double Calka(double[] x)
        {
            double calka = 0;
            double h = x[1] - x[0];
            for (int i = 0; i < P - 1; i++)
            {
                calka += ((x[i] + x[i + 1]) * h) / 2;
            }
            return calka;
        }

        public (List<int> ASK, List<int> PSK, List<int> FSK) Demodul(double[] a, double[] ask, double[] psk, double[] fsk)
        {
            List<double> A = a.Take(B).ToList();

            // ASK
            List<double> Y = new List<double>(ask);
            int k = 0;
            for (int i = 0; i < N; i++)
            {
                Y[i] *= A[k] * Math.Sin(2 * Math.PI * fn * t[i]);
                if (i != 0 && i % P == 0)
                    k++;
            }
            List<double> calkaASK = new List<double>();
            k = 0;
            for (int i = 0; i < B; i++)
            {
                calkaASK.Add(Calka(Y.Skip(k).Take(P).ToArray()));
                k += P;
            }

            // PSK
            Y = new List<double>(psk);
            k = 0;
            for (int i = 0; i < N; i++)
            {
                Y[i] *= A[k] * Math.Sin(2 * Math.PI * fn * t[i]);
                if (i != 0 && i % P == 0)
                    k++;
            }
            List<double> calkaPSK = new List<double>();
            k = 0;
            for (int i = 0; i < B; i++)
            {
                calkaPSK.Add(Calka(Y.Skip(k).Take(P).ToArray()));
                k += P;
            }

            // FSK
            Y = new List<double>(fsk);
            List<double> Y2 = new List<double>(fsk);
            k = 0;
            for (int i = 0; i < N; i++)
            {
                Y[i] *= A[k] * Math.Sin(2 * Math.PI * fn1 * t[i]);
                Y2[i] *= A[k] * Math.Sin(2 * Math.PI * fn2 * t[i]);
                if (i != 0 && i % P == 0)
                    k++;
            }
            List<double> calka3 = new List<double>();
            List<double> calka4 = new List<double>();
            k = 0;
            for (int i = 0; i < B; i++)
            {
                calka3.Add(Y.Skip(k).Take(P).Select(Math.Abs).Sum());
                calka4.Add(Y2.Skip(k).Take(P).Select(Math.Abs).Sum());
                k += P;
            }
            List<double> calkaFSK = calka3.Zip(calka4, (a, b) => a + b).ToList();

            Console.WriteLine(calkaFSK.Count);

            List<int> ASK = new List<int>();
            List<int> PSK = new List<int>();
            List<int> FSK = new List<int>();
            double h = 0.1;

            for (int i = 0; i < B; i++)
            {
                if (calkaASK[i] > h)
                    ASK.Add(1);
                else
                    ASK.Add(0);
                if (calkaPSK[i] <= 0)
                    PSK.Add(0);
                else
                    PSK.Add(1);
                if (calkaFSK[i] > 0.005)
                    FSK.Add(1);
                else
                    FSK.Add(0);
            }

            Console.WriteLine("ASK " + string.Join(", ", ASK));
            Console.WriteLine("PSK " + string.Join(", ", PSK));
            Console.WriteLine("FSK " + string.Join(", ", FSK));

            return (ASK, PSK, FSK);
        }
    }

public static class HammingCode
{
    public static int Xor(int[] s, int[] index)
    {
        return (s[index[0]] ^ s[index[1]]) ^ s[index[2]];
    }

    public static int[] Hamming74(int[] bits)
    {
        int x1 = Xor(bits, new int[] { 0, 1, 3 });
        int x2 = Xor(bits, new int[] { 0, 2, 3 });
        int x3 = Xor(bits, new int[] { 1, 2, 3 });
        return new int[] { x1, x2, bits[0], x3, bits[1], bits[2], bits[3] };
    }

    public static int[] Dehamming74(int[] bits)
    {
        int x1 = Xor(bits, new int[] { 2, 4, 6 });
        int x2 = Xor(bits, new int[] { 2, 5, 6 });
        int x4 = Xor(bits, new int[] { 4, 5, 6 });

        x1 = bits[0] ^ x1;
        x2 = bits[1] ^ x2;
        x4 = bits[3] ^ x4;
        int suma = x1 + x2 * 2 + x4 * 4;
        if (suma == 0)
        {
            Console.WriteLine("Bit zgodny z oryginalnym");
            return new int[] { bits[2], bits[4], bits[5], bits[6] }; 
        }
        else
        {
            Console.WriteLine($"Niezgodność w bicie x{suma}");
            suma--; 
            bits[suma] = bits[suma] == 0 ? 1 : 0;
            Console.WriteLine($"Bit po zmianie to: {string.Join(", ", bits)}");
            return new int[] { bits[2], bits[4], bits[5], bits[6] }; 
        }
    }
}

 public static class Modulacja
    {
        public static double A1 = 1;
        public static double A2 = 0.5;
        public static int B = 77;
        public static int W = 2;
        public static int Tc = 5;
        public static int fs = 154;
        public static double Tb = (double)Tc / B;
        public static double fn = W * Math.Pow(Tb, -1);
        public static double fn1 = (W + 1) / Tb;
        public static double fn2 = (W + 2) / Tb;
        public static int N = fs * Tc;
        public static double[] t = Enumerable.Range(0, N).Select(i => i / (4.0 * fs)).ToArray();
        public static double[] X = Enumerable.Range(0, N).Select(i => i * Tc / (N - 1.0)).ToArray();

        public static double[] Tlumienie(double x, double y)
        {
            double[] a = t.Select(ti => Math.Exp(-y * ti)).ToArray();
            return a.Select(ai => x * ai).ToArray();
        }

        public static double[] Ask(int[] b)
        {
            double[] za = new double[N];
            int temp = 0;
            for (int i = 0; i < N; i++)
            {
                if (b[temp] == 0)
                {
                    za[i] = A1 * Math.Sin(2 * Math.PI * fn * t[i]);
                }
                else
                {
                    za[i] = A2 * Math.Sin(2 * Math.PI * fn * t[i]);
                }
                if (i != 0 && i % (N / b.Length) == 0)
                {
                    temp++;
                }
            }
            return za;
        }

        public static double[] Psk(int[] b)
        {
            double[] za = new double[N];
            int temp = 0;
            for (int i = 0; i < N; i++)
            {
                if (b[temp] == 0)
                {
                    za[i] = Math.Sin(2 * Math.PI * fn * t[i]);
                }
                else
                {
                    za[i] = Math.Sin(2 * Math.PI * fn * t[i] + Math.PI);
                }
                if (i != 0 && i % (N / b.Length) == 0)
                {
                    temp++;
                }
            }
            return za;
        }

        public static double[] Fsk(int[] b)
        {
            double[] za = new double[N];
            int temp = 0;
            for (int i = 0; i < N; i++)
            {
                if (b[temp] == 0)
                {
                    za[i] = Math.Sin(2 * Math.PI * fn1 * t[i]);
                }
                else
                {
                    za[i] = Math.Sin(2 * Math.PI * fn2 * t[i]);
                }
                if (i != 0 && i % (N / b.Length) == 0)
                {
                    temp++;
                }
            }
            return za;
        }
    }
public static class Hamming74
{
    public static int[] Encode(int[] data)
    {
        int[] encoded = new int[7];
        encoded[0] = (data[0] + data[1] + data[3]) % 2;
        encoded[1] = (data[0] + data[2] + data[3]) % 2;
        encoded[2] = (data[1] + data[2] + data[3]) % 2;
        encoded[3] = data[0];
        encoded[4] = data[1];
        encoded[5] = data[2];
        encoded[6] = data[3];
        return encoded;
    }

    public static int[] Decode(int[] encoded)
    {
        int[] decoded = new int[4];
        int x1 = (encoded[0] + encoded[2] + encoded[4] + encoded[6]) % 2;
        int x2 = (encoded[1] + encoded[2] + encoded[5] + encoded[6]) % 2;
        int x4 = (encoded[3] + encoded[4] + encoded[5] + encoded[6]) % 2;
        int syndrome = x1 + 2 * x2 + 4 * x4;
        if (syndrome != 0)
        {
            encoded[syndrome - 1] ^= 1;
        }
        decoded[0] = encoded[3];
        decoded[1] = encoded[4];
        decoded[2] = encoded[5];
        decoded[3] = encoded[6];
        return decoded;
    }
}

public static class Modulation
{
    public static double[] ASK(int[] data)
    {
        double[] modulated = new double[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            modulated[i] = data[i] == 0 ? -1 : 1;
        }
        return modulated;
    }

    public static double[] PSK(int[] data)
    {
        double[] modulated = new double[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            modulated[i] = data[i] == 0 ? 1 : -1;
        }
        return modulated;
    }

    public static double[] FSK(int[] data)
    {
        double[] modulated = new double[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            modulated[i] = data[i] == 0 ? 1 : -1;
        }
        return modulated;
    }
}

public static class Demodulation
{
    public static int[] Demodulate(int[] data, double[] modulated)
    {
        int[] demodulated = new int[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            demodulated[i] = modulated[i] >= 0 ? 1 : 0;
        }
        return demodulated;
    }
}

public static class Attenuation
{
    public static double[] Apply(double[] signal, double beta)
    {
        double[] attenuated = new double[signal.Length];
        for (int i = 0; i < signal.Length; i++)
        {
            attenuated[i] = signal[i] * Math.Pow(beta, i);
        }
        return attenuated;
    }
}

public static class Noise
{
    public static double[] AddWhiteNoise(double[] signal, double alpha)
    {
        double[] noisy = new double[signal.Length];
        Random rand = new Random();
        for (int i = 0; i < signal.Length; i++)
        {
            noisy[i] = signal[i] + alpha * (rand.NextDouble() * 2 - 1); 
        }
        return noisy;
    }
}

public static class BER
{
    public static double Calculate(int[] original, int[] decoded)
    {
        int errors = 0;
        int minLength = Math.Min(original.Length, decoded.Length);
        for (int i = 0; i < minLength; i++)
        {
            if (original[i] != decoded[i])
            {
                errors++;
            }
        }
        return (double)errors / minLength;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        int[] a = new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1 };
        int[,] b = new int[11, 4];
        int c = 0;

        Console.WriteLine("Oryginalny sygnał:");
        Console.WriteLine(string.Join(", ", a));

        // Hamming74 
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                b[i, j] = a[c];
                if (c < 44) c++;
            }
        }

        List<int> d = new List<int>();
        for (int i = 0; i < 11; i++)
        {
            d.AddRange(Hamming74.Encode(new int[] { b[i, 0], b[i, 1], b[i, 2], b[i, 3] }));
        }

        Console.WriteLine("Sygnał po kodowaniu Hamming74 z podziałem na ramki:");
        Console.WriteLine(string.Join(", ", d));

        int[] original = d.ToArray();

        // Modulacja
        double[] askModulated = Modulation.ASK(d.ToArray());
        double[] pskModulated = Modulation.PSK(d.ToArray());
        double[] fskModulated = Modulation.FSK(d.ToArray());

        // Szum
        double alpha = 0.75;
        double beta = 0.8;
        double[] askNoisy = Noise.AddWhiteNoise(askModulated, alpha);
        double[] pskNoisy = Noise.AddWhiteNoise(pskModulated, alpha);
        double[] fskNoisy = Noise.AddWhiteNoise(fskModulated, alpha);
        double[] askAttenuated = Attenuation.Apply(askModulated, beta);
        double[] pskAttenuated = Attenuation.Apply(pskModulated, beta);
        double[] fskAttenuated = Attenuation.Apply(fskModulated, beta);
        double[] askNoisyAttenuated = Attenuation.Apply(askNoisy, beta);
        double[] pskNoisyAttenuated = Attenuation.Apply(pskNoisy, beta);
        double[] fskNoisyAttenuated = Attenuation.Apply(fskNoisy, beta);

        // Demodulacja
        Console.WriteLine("Sygnał z szumem białym po demodulacji:");
        int[] askDemodulated = Demodulation.Demodulate(d.ToArray(), askNoisy);
        int[] pskDemodulated = Demodulation.Demodulate(d.ToArray(), pskNoisy);
        int[] fskDemodulated = Demodulation.Demodulate(d.ToArray(), fskNoisy);
        Console.WriteLine("Wytłumiony sygnał po demodulacji:");
        int[] askAttenuatedDemodulated = Demodulation.Demodulate(d.ToArray(), askAttenuated);
        int[] pskAttenuatedDemodulated = Demodulation.Demodulate(d.ToArray(), pskAttenuated);
        int[] fskAttenuatedDemodulated = Demodulation.Demodulate(d.ToArray(), fskAttenuated);

        // Deframing i Hamming74 decoding
        int[] askDeframed = Dehamming74(askDemodulated);
        int[] pskDeframed = Dehamming74(pskDemodulated);
        int[] fskDeframed = Dehamming74(fskDemodulated);
        int[] askAttenuatedDeframed = Dehamming74(askAttenuatedDemodulated);
        int[] pskAttenuatedDeframed = Dehamming74(pskAttenuatedDemodulated);
        int[] fskAttenuatedDeframed = Dehamming74(fskAttenuatedDemodulated);

        Console.WriteLine("Wynik końcowy po dekodowaniu Hamming74:");
        Console.WriteLine("ASK szum:");
        Console.WriteLine(string.Join(", ", askDeframed));
        Console.WriteLine("PSK szum:");
        Console.WriteLine(string.Join(", ", pskDeframed));
        Console.WriteLine("FSK szum:");
        Console.WriteLine(string.Join(", ", fskDeframed));
        Console.WriteLine("ASK tłumienie:");
        Console.WriteLine(string.Join(", ", askAttenuatedDeframed));
        Console.WriteLine("PSK tłumienie:");
        Console.WriteLine(string.Join(", ", pskAttenuatedDeframed));
        Console.WriteLine("FSK tłumienie:");
        Console.WriteLine(string.Join(", ", fskAttenuatedDeframed));

        // BER kalkulacja
        try
        {
            Console.WriteLine("BER ASK szum: " + BER.Calculate(original, askDeframed));
            Console.WriteLine("BER PSK szum: " + BER.Calculate(original, pskDeframed));
            Console.WriteLine("BER FSK szum: " + BER.Calculate(original, fskDeframed));
            Console.WriteLine("BER ASK tłumienie: " + BER.Calculate(original, askAttenuatedDeframed));
            Console.WriteLine("BER PSK tłumienie: " + BER.Calculate(original, pskAttenuatedDeframed));
            Console.WriteLine("BER FSK tłumienie: " + BER.Calculate(original, fskAttenuatedDeframed));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }

    private static int[] Dehamming74(int[] data)
    {
        int[] deframed = new int[data.Length / 7 * 4];
        int j = 0;
        int idx = 0;
        while (j < data.Length)
        {
            int[] frame = new int[7];
            for (int i = 0; i < 7; i++)
            {
                frame[i] = data[i + j];
            }
            j += 7;
            int[] decoded = Hamming74.Decode(frame);
            for (int i = 0; i < 4; i++)
            {
                deframed[idx++] = decoded[i];
            }
        }
        return deframed;
    }
}
