// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using Swed32;

namespace ConsoleApp1;

public static class Program
{
    public static unsafe void Main()
    {
        var swed = new Swed(Process.GetCurrentProcess());
        nint n = 1000;
        // Get address of n and "cast" it to IntPtr
        var nP = new IntPtr(&n);
        Console.WriteLine($"&{nameof(n)} = 0x{nP:x16}");

        nP = swed.ReadPointer(nP);

        Console.WriteLine($"{nameof(n)}: {n}");
        Console.WriteLine($"{nameof(nP)}: {nP}");
        Console.WriteLine($"{nameof(n)} == {nameof(nP)}?: {n == nP}");



        var vectors = Enumerable.Range(0, 5)
            .Select(i => new Vector3(i))
            .ToArray();
        
        fixed (Vector3* pVectors = vectors)
        {
            // v0 = 0,0,0
            Console.WriteLine($"{nameof(vectors)}[0] = {vectors[0]}");

            swed.Unsafe.ReadStruct(&pVectors[4], out vectors[0]);

            // v0 = v4 = 4,4,4
            Console.WriteLine($"{nameof(vectors)}[0] = {vectors[0]}");
            
            var vBuf = new Vector3[vectors.Length];
            swed.Unsafe.ReadStructs<Vector3>(pVectors, vBuf);

            // vb0-vb4 = 4,4,4; 1,1,1; 2,2,2; 3,3,3; 4,4,4
            Console.WriteLine($"vBuf:\n{string.Join('\n', vBuf)}");
            
            swed.Unsafe.WriteStruct(&pVectors[1], ref vectors[0]);
            swed.Unsafe.WriteStruct(&pVectors[2], ref vectors[0]);
            swed.Unsafe.WriteStruct(&pVectors[3], ref vectors[0]);

            // v0-v3 = v4 = 4,4,4 for all
            Console.WriteLine($"Vectors:\n{string.Join('\n', vectors)}");
            
            MemoryMarshal.Cast<Vector3, float>(vBuf).Fill(420.69f);
            swed.Unsafe.WriteStructs<Vector3>(pVectors, vBuf);

            // 420.69f for all
            Console.WriteLine($"Vectors:\n{string.Join('\n', vectors)}");
        }
    }
}