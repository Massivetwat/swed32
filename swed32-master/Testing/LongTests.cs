namespace Testing;

public unsafe class LongTests
{
    private readonly Swed _swed = new(Process.GetCurrentProcess());
    private readonly MAlloc<long> _malloc = new(1);

    [Test]
    public void ReadLong()
    {
        // TODO: swed32 doesn't have ReadLong???

        _malloc.RandomizeBytes();
        _swed.Unsafe.ReadStruct<long>(_malloc.Buffer.ToPointer(), out var n);
        Assert.That(n, Is.EqualTo(_malloc.Span[0]));
    }

    [Test]
    public void WriteLong()
    {
        const long n = 123456789L;
        
        _malloc.RandomizeBytes();
        var nCpy = n;
        _swed.Unsafe.WriteStruct(_malloc.Buffer.ToPointer(), ref nCpy);
        Assert.That(_malloc.Span[0], Is.EqualTo(n));
    }
}