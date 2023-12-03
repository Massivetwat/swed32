using System.Numerics;
using System.Runtime.InteropServices;

namespace Testing;

public sealed unsafe class MAlloc<T> : IDisposable
    where T : unmanaged
{
    public int Length { get; }
    private readonly T* _buf;
    public Span<T> Span => new(_buf, Length);

    public MAlloc(int length, bool zero = true)
    {
        Length = length;
        _buf = (T*)Marshal.AllocHGlobal(sizeof(T) * length);
        
        if (zero)
            Span.Clear();
    }

    public void RandomizeBytes(Random? rand = null)
    {
        rand ??= Random.Shared;
        var bytes = MemoryMarshal.Cast<T, byte>(Span);
        rand.NextBytes(bytes);
    }

    public void Randomize(Func<int, T> indexedRandomizer)
    {
        var span = Span;
        for (var i = 0; i < span.Length; i++)
            span[i] = indexedRandomizer(i);
    }

    public MAlloc<T> Copy()
    {
        var malloc = new MAlloc<T>(Length);
        Span.CopyTo(malloc.Span);
        return malloc;
    }

    private void ReleaseUnmanagedResources()
    {
        Marshal.FreeHGlobal(new IntPtr(_buf));
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~MAlloc()
    {
        ReleaseUnmanagedResources();
    }
}