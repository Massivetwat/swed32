// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Swed32;

namespace ConsoleApp1;

public static class Program
{
    public static unsafe void Main()
    {
        nint n = 1000;
        // Get address of n and "cast" it to IntPtr
        var nP = new IntPtr(&n);
        Console.WriteLine($"&{nameof(n)} = 0x{nP:x16}");

        nP = new Swed(Process.GetCurrentProcess())
            .ReadPointer(nP);

        Console.WriteLine($"{nameof(n)}: {n}");
        Console.WriteLine($"{nameof(nP)}: {nP}");
        Console.WriteLine($"{nameof(n)} == {nameof(nP)}?: {n == nP}");
    }
}