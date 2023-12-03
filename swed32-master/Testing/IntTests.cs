namespace Testing;

[TestFixture]
public unsafe class IntTests
{
    private readonly Swed _swed = new(Process.GetCurrentProcess());
    private readonly MAlloc<int> _malloc = new(1);

    [Test]
    public void ReadInt()
    {
        _malloc.RandomizeBytes();
        var read = _swed.ReadInt(_malloc.Buffer);
        Assert.That(read, Is.EqualTo(_malloc.Span[0]));

        _malloc.RandomizeBytes();
        _swed.Unsafe.ReadStruct(_malloc.Buffer.ToPointer(), out read);
        Assert.That(read, Is.EqualTo(_malloc.Span[0]));
    }

    [Test]
    public void WriteInt()
    {
        _malloc.RandomizeBytes();
        var read = _swed.ReadInt(_malloc.Buffer);
        Assert.That(read, Is.EqualTo(_malloc.Span[0]));

        _malloc.RandomizeBytes();
        _swed.Unsafe.WriteStruct(_malloc.Buffer.ToPointer(), ref read);
        Assert.That(read, Is.EqualTo(_malloc.Span[0]));
    }
}