namespace Testing;

public unsafe class ShortTests
{
    private readonly Swed _swed = new(Process.GetCurrentProcess());
    private readonly MAlloc<short> _malloc = new(1);

    [Test]
    public void ReadShort()
    {
        _malloc.RandomizeBytes();
        var n = _swed.ReadShort(_malloc.Buffer);
        Assert.That(n, Is.EqualTo(_malloc.Span[0]));

        _malloc.RandomizeBytes();
        _swed.Unsafe.ReadStruct(_malloc.Buffer.ToPointer(), out n);
        Assert.That(n, Is.EqualTo(_malloc.Span[0]));
    }

    [Test]
    public void WriteShort()
    {
        const short n = short.MaxValue / 4 * 3;
        
        _malloc.RandomizeBytes();
        _swed.WriteShort(_malloc.Buffer, n);
        Assert.That(_malloc.Span[0], Is.EqualTo(n));
        
        _malloc.RandomizeBytes();
        var nCpy = n; // allows ref usage
        _swed.Unsafe.WriteStruct(_malloc.Buffer.ToPointer(), ref nCpy);
        Assert.That(_malloc.Span[0], Is.EqualTo(n));
    }
}