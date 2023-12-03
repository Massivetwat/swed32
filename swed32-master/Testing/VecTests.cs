using System.Numerics;
using System.Runtime.InteropServices;

namespace Testing;

public unsafe class VecTests
{
    private readonly Swed _swed = new(Process.GetCurrentProcess());
    private readonly MAlloc<Vector3> _malloc = new(1);

    [Test]
    public void ReadVec()
    {
        _malloc.RandomizeBytes();
        var v3 = _swed.ReadVec(_malloc.Buffer);
        Assert.That(v3, Is.EqualTo(_malloc.Span[0]));

        _malloc.RandomizeBytes();
        _swed.Unsafe.ReadStruct(_malloc.Buffer.ToPointer(), out v3);
        Assert.That(v3, Is.EqualTo(_malloc.Span[0]));

        _malloc.RandomizeBytes();
        Span<float> buffer = stackalloc float[3];
        _swed.Unsafe.ReadStructs(_malloc.Buffer.ToPointer(), buffer);
        Assert.That(MemoryMarshal.Cast<float, Vector3>(buffer)[0], Is.EqualTo(_malloc.Span[0]));
    }

    [Test]
    public void WriteVec()
    {
        var v3 = new Vector3(6, 9, 420);
        
        _malloc.RandomizeBytes();
        _swed.WriteVec(_malloc.Buffer, v3);
        Assert.That(_malloc.Span[0], Is.EqualTo(v3));
        
        _malloc.RandomizeBytes();
        _swed.Unsafe.WriteStruct(_malloc.Buffer.ToPointer(), ref v3);
        Assert.That(_malloc.Span[0], Is.EqualTo(v3));
    }
}