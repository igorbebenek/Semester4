using System;
using System.Linq;

class Hamming
{
    private static int XOR(int[] s, int[] index)
    {
        return index.Aggregate(0, (current, i) => current ^ s[i]);
    }

    public static int[] Encode74(int[] bits)
    {
        int x1 = XOR(bits, new[] { 0, 1, 3 });
        int x2 = XOR(bits, new[] { 0, 2, 3 });
        int x3 = XOR(bits, new[] { 1, 2, 3 });
        return new[] { x1, x2, bits[0], x3, bits[1], bits[2], bits[3] };
    }

    public static int[] Decode74(int[] bits)
    {
        int[] receivedBits = (int[])bits.Clone();

        int x1 = XOR(bits, new[] { 2, 4, 6 });
        int x2 = XOR(bits, new[] { 2, 5, 6 });
        int x4 = XOR(bits, new[] { 4, 5, 6 });

        x1 = bits[0] ^ x1;
        x2 = bits[1] ^ x2;
        x4 = bits[3] ^ x4;

        int errorPosition = x1 + x2 * 2 + x4 * 4;

        if (errorPosition != 0)
        {
            errorPosition -= 1;
            Console.WriteLine("Niezgodność w bicie " + (errorPosition + 1));
            receivedBits[errorPosition] ^= 1;
            Console.WriteLine("Bit po zmianie to: " + string.Join(", ", receivedBits));
        }

        return new[] { receivedBits[2], receivedBits[4], receivedBits[5], receivedBits[6] };
    }

    public static int[] Encode1511(int[] bits)
    {
        int[,] G = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1 },
            { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 }
        };

        int[] encoded = new int[15];

        for (int i = 0; i < 15; i++)
        {
            int sum = 0;
            for (int j = 0; j < 11; j++)
            {
                sum += bits[j] * G[j, i];
            }
            encoded[i] = sum % 2;
        }

        return encoded;
    }

    public static int[] Decode1511(int[] bits)
    {
        int[,] H = new int[,]
        {
            { 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1 },
            { 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 0, 1 },
            { 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1 }
        };

        int[] syndrome = new int[4];

        for (int i = 0; i < 4; i++)
        {
            int sum = 0;
            for (int j = 0; j < 15; j++)
            {
                sum += bits[j] * H[i, j];
            }
            syndrome[i] = sum % 2;
        }

        int errorPosition = syndrome[0] + syndrome[1] * 2 + syndrome[2] * 4 + syndrome[3] * 8;

        if (errorPosition != 0)
        {
            errorPosition -= 1;
            Console.WriteLine("Niezgodność w bicie " + (errorPosition + 1));
            bits[errorPosition] ^= 1;
            Console.WriteLine("Bit po zmianie to: " + string.Join(", ", bits));
        }

        int[] decoded = new int[11];
        Array.Copy(bits, 0, decoded, 0, 11);

        return decoded;
    }

    public static void Main(string[] args)
    {
        int[] original74 = { 1, 1, 0, 1 };
        Console.WriteLine("(7,4)  Wartość orginalna : " + string.Join(", ", original74));

        int[] encoded74 = HammingCode.Hamming74(original74);
        Console.WriteLine("(7,4)  sygnał dobrze odebrany = " + string.Join(", ", encoded74));

        encoded74[2] ^= 1;
        Console.WriteLine("(7,4)  sygnał źle odebrany = " + string.Join(", ", encoded74));

        int[] decoded74 = HammingCode.Dehamming74(encoded74);
        Console.WriteLine("(7,4)  Odkodowane bity = " + string.Join(", ", decoded74));

        int[] original1511 = { 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0 };
        Console.WriteLine("(15,11) Wartość orginalna : " + string.Join(", ", original1511));

        int[] encoded1511 = Encode1511(original1511);
        Console.WriteLine("(15,11) Zakodowany: " + string.Join(", ", encoded1511));

        encoded1511[4] ^= 1;
        Console.WriteLine("(15,11) Zły sygnał: " + string.Join(", ", encoded1511));

        int[] decoded1511 = Decode1511(encoded1511);
        Console.WriteLine("(15,11) Poprawiony sygnał " + string.Join(", ", decoded1511));
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
