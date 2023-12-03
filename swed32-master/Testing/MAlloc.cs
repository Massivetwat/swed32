using System.Runtime.InteropServices;

namespace Testing;

public sealed unsafe class MAlloc<T> : IDisposable
    where T : unmanaged
{
    public int Length { get; }
    private readonly T* _buf;
    public Span<T> Span => new(_buf, Length);

    public MAlloc(int length)
    {
        Length = length;
        _buf = (T*)Marshal.AllocHGlobal(sizeof(T) * length);
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