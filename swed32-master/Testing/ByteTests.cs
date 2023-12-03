namespace Testing;

public unsafe class ByteTests
{
    private Swed _swed = new(Process.GetCurrentProcess());
    private MAlloc<byte> _bytes = new(451_723);

    [Test]
    public void ReadBytesTest()
    {
        var readBytes = _swed.ReadBytes(_bytes.Buffer, _bytes.Length);
        Assert.That(readBytes, Is.EquivalentTo(_bytes.Span.ToArray()));

        _swed.Unsafe.ReadStructs<byte>(_bytes.Buffer.ToPointer(), readBytes);
        Assert.That(readBytes, Is.EquivalentTo(_bytes.Span.ToArray()));
    }

    [Test]
    public void WriteBytesTest()
    {
        var written = _swed.WriteBytes(_bytes.Buffer, new byte[_bytes.Length]);
            
        Assert.That(written, Is.True);
        Assert.That(Array.TrueForAll(_bytes.Span.ToArray(), b => b == 0), Is.True);
    }
}