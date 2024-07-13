using System;
using System.Linq;
using ScottPlot;

class Program
{
    static void Main(string[] args)
    {
        double totalTime = 1;
        double sampleRate = 8000;
        int totalSamples = Convert.ToInt32(totalTime * sampleRate);
        int frequencyMultiplier = 2;
        int[] inputBits = { 1, 0, 1, 1, 0, 1, 0, 0, 1, 0 };
        int bitCount = inputBits.Length;
        double lowAmplitude = 500;
        double highAmplitude = 1000;
        double bitDuration = totalTime / bitCount;
        double samplesPerBit = bitDuration * sampleRate;
        double carrierFrequency = frequencyMultiplier / bitDuration;

        var askSignal = GenerateASK(inputBits, samplesPerBit, sampleRate, carrierFrequency, totalSamples, lowAmplitude, highAmplitude);
        PlotSignal(askSignal, "ASK Signal", 2000);
        var demodAsk = DemodulateASK(askSignal, samplesPerBit, carrierFrequency, sampleRate, inputBits.Length);
        PlotSignal(demodAsk.multiplied, "ASK Multiplied", 2000);
        PlotSignal(demodAsk.integrated, "ASK Integrated", 2000);
        PlotSignal(demodAsk.detected, "ASK Detected", 2000);

        var pskSignal = GeneratePSK(inputBits, samplesPerBit, sampleRate, carrierFrequency, totalSamples);
        PlotSignal(pskSignal, "PSK Signal", 2000);
        var demodPsk = DemodulatePSK(pskSignal, samplesPerBit, carrierFrequency, sampleRate, inputBits.Length);
        PlotSignal(demodPsk.multiplied, "PSK Multiplied", 2000);
        PlotSignal(demodPsk.integrated, "PSK Integrated", 2000);
        PlotSignal(demodPsk.detected, "PSK Detected", 2000);

        var fskSignal = GenerateFSK(inputBits, samplesPerBit, bitDuration, sampleRate, totalSamples, frequencyMultiplier);
        PlotSignal(fskSignal, "FSK Signal", 2000);
        var demodFsk = DemodulateFSK(fskSignal, samplesPerBit, bitDuration, carrierFrequency, sampleRate, inputBits.Length, frequencyMultiplier);
        PlotSignal(demodFsk.multiplied1, "FSK Multiplied 1", 2000);
        PlotSignal(demodFsk.multiplied2, "FSK Multiplied 2", 2000);
        PlotSignal(demodFsk.integrated1, "FSK Integrated 1", 2000);
        PlotSignal(demodFsk.integrated2, "FSK Integrated 2", 2000);
        PlotSignal(demodFsk.detected, "FSK Detected", 2000);
        PlotSignal(demodFsk.detectedBits.Select(x => (double)x).ToArray(), "FSK Detected Bits", 2000);
    }

    static double[] GenerateASK(int[] bits, double samplesPerBit, double sampleRate, double carrierFrequency, int totalSamples, double lowAmplitude, double highAmplitude)
    {
        double[] signal = new double[totalSamples];
        int bitIndex = 0;

        for (int i = 0; i < totalSamples; i++)
        {
            double t = i / sampleRate;
            signal[i] = bits[bitIndex] == 1 ? highAmplitude * Math.Sin(2 * Math.PI * carrierFrequency * t) : lowAmplitude * Math.Sin(2 * Math.PI * carrierFrequency * t);

            if (i > 0 && i % samplesPerBit == 0)
                bitIndex++;

            if (bitIndex == bits.Length)
                break;
        }

        return signal;
    }

    static double[] GeneratePSK(int[] bits, double samplesPerBit, double sampleRate, double carrierFrequency, int totalSamples)
    {
        double[] signal = new double[totalSamples];
        int bitIndex = 0;

        for (int i = 0; i < totalSamples; i++)
        {
            double t = i / sampleRate;
            signal[i] = bits[bitIndex] == 1 ? Math.Sin(2 * Math.PI * carrierFrequency * t + Math.PI) : Math.Sin(2 * Math.PI * carrierFrequency * t);

            if (i > 0 && i % samplesPerBit == 0)
                bitIndex++;

            if (bitIndex == bits.Length)
                break;
        }

        return signal;
    }

    static double[] GenerateFSK(int[] bits, double samplesPerBit, double bitDuration, double sampleRate, int totalSamples, int modulationIndex)
    {
        double carrierFrequency1 = (modulationIndex + 1) / bitDuration;
        double carrierFrequency2 = (modulationIndex + 2) / bitDuration;
        double[] signal = new double[totalSamples];
        int bitIndex = 0;

        for (int i = 0; i < totalSamples; i++)
        {
            double t = i / sampleRate;
            signal[i] = bits[bitIndex] == 1 ? Math.Sin(2 * Math.PI * carrierFrequency2 * t) : Math.Sin(2 * Math.PI * carrierFrequency1 * t);

            if (i > 0 && i % samplesPerBit == 0)
                bitIndex++;

            if (bitIndex == bits.Length)
                break;
        }

        return signal;
    }

    static (double[] multiplied, double[] integrated, double[] detected) DemodulateASK(double[] input, double samplesPerBit, double carrierFrequency, double sampleRate, int bitCount)
    {
        double[] multipliedSignal = MultiplyByCarrier(input, carrierFrequency, sampleRate);
        double[] integratedSignal = IntegrateSignal(multipliedSignal, samplesPerBit);
        double[] detectedSignal = DetectSignal(integratedSignal, 200000);
        int[] decodedBits = DecodeBits(detectedSignal, bitCount, samplesPerBit);

        return (multipliedSignal, integratedSignal, detectedSignal);
    }

    static (double[] multiplied, double[] integrated, double[] detected) DemodulatePSK(double[] input, double samplesPerBit, double carrierFrequency, double sampleRate, int bitCount)
    {
        double[] multipliedSignal = MultiplyByCarrier(input, carrierFrequency, sampleRate);
        double[] integratedSignal = IntegrateSignal(multipliedSignal, samplesPerBit);
        double[] detectedSignal = DetectSignal(integratedSignal, 0);
        int[] decodedBits = DecodeBits(detectedSignal, bitCount, samplesPerBit);

        return (multipliedSignal, integratedSignal, detectedSignal);
    }

    static (double[] multiplied1, double[] multiplied2, double[] integrated1, double[] integrated2, double[] detected, int[] detectedBits) DemodulateFSK(double[] input, double samplesPerBit, double bitDuration, double carrierFrequency, double sampleRate, int bitCount, int modulationIndex)
    {
        double carrierFrequency1 = (modulationIndex + 1) / bitDuration;
        double carrierFrequency2 = (modulationIndex + 2) / bitDuration;

        double[] multipliedSignal1 = MultiplyByCarrier(input, carrierFrequency1, sampleRate);
        double[] multipliedSignal2 = MultiplyByCarrier(input, carrierFrequency2, sampleRate);
        double[] integratedSignal1 = IntegrateSignal(multipliedSignal1, samplesPerBit);
        double[] integratedSignal2 = IntegrateSignal(multipliedSignal2, samplesPerBit);
        double[] detectedSignal = SubtractSignals(integratedSignal2, integratedSignal1);
        int[] decodedBits = DecodeBits(detectedSignal, bitCount, samplesPerBit);

        return (multipliedSignal1, multipliedSignal2, integratedSignal1, integratedSignal2, detectedSignal, decodedBits);
    }

    static double[] MultiplyByCarrier(double[] input, double carrierFrequency, double sampleRate)
    {
        double[] output = new double[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            double t = i / sampleRate;
            output[i] = input[i] * Math.Sin(2 * Math.PI * carrierFrequency * t);
        }

        return output;
    }

    static double[] IntegrateSignal(double[] input, double samplesPerBit)
    {
        double[] output = new double[input.Length];
        double sum = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (i > 0 && i % samplesPerBit == 0)
                sum = 0;

            sum += input[i];
            output[i] = sum;
        }

        return output;
    }

    static double[] DetectSignal(double[] input, double threshold)
    {
        double[] output = new double[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            output[i] = input[i] > threshold ? 1 : 0;
        }

        return output;
    }

    static double[] SubtractSignals(double[] signal1, double[] signal2)
    {
        double[] output = new double[signal1.Length];

        for (int i = 0; i < signal1.Length; i++)
        {
            output[i] = signal1[i] - signal2[i];
        }

        return output;
    }

    static int[] DecodeBits(double[] input, int bitCount, double samplesPerBit)
    {
        int[] bits = new int[bitCount];
        int bitIndex = 0;

        for (int i = 0; i < input.Length && bitIndex < bitCount; i++)
        {
            if (i % samplesPerBit == 0)
                bits[bitIndex++] = input[i] > 0 ? 1 : 0;
        }

        return bits;
    }

    static void PlotSignal(double[] signal, string title, int length)
    {
        var plt = new ScottPlot.Plot(600, 400);
        plt.AddSignal(signal.Take(length).ToArray());
        plt.Title(title);
        plt.XLabel("Czas (Ms)");
        plt.YLabel("Sygnał");
        plt.SaveFig($"{title}.png");
    }
}
