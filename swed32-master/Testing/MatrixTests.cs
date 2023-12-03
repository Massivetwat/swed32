using System.Numerics;

namespace Testing;

public unsafe class MatrixTests
{
    private readonly Swed _swed = new(Process.GetCurrentProcess());
    private readonly MAlloc<float> _malloc = new(16);

    [Test]
    public void ReadMatrix()
    {
        _malloc.RandomizeBytes();
        var matrix = _swed.ReadMatrix(_malloc.Buffer);
        Assert.That(matrix, Is.EquivalentTo(_malloc.Span.ToArray()));

        _malloc.RandomizeBytes();
        _swed.Unsafe.ReadStructs<float>(_malloc.Buffer.ToPointer(), matrix);
        Assert.That(matrix, Is.EquivalentTo(_malloc.Span.ToArray()));
    }

    [Test]
    public void WriteMatrix()
    {
        float[] matrix = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        // swed32 cannot write matrices

        _malloc.RandomizeBytes();
        _swed.Unsafe.WriteStructs<float>(_malloc.Buffer.ToPointer(), matrix);
        Assert.That(_malloc.Span.ToArray(), Is.EquivalentTo(matrix));
    }
}