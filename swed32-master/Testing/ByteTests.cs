namespace Testing;

public unsafe class ByteTests
{
    private readonly Swed _swed = new(Process.GetCurrentProcess());
    private readonly MAlloc<byte> _bytes = new(451_723);

    [Test]
    public void ReadBytes()
    {
        var readBytes = _swed.ReadBytes(_bytes.Buffer, _bytes.Length);
        Assert.That(readBytes, Is.EquivalentTo(_bytes.Span.ToArray()));

        _swed.Unsafe.ReadStructs<byte>(_bytes.Buffer.ToPointer(), readBytes);
        Assert.That(readBytes, Is.EquivalentTo(_bytes.Span.ToArray()));
    }

    [Test]
    public void WriteBytes()
    {
        var written = _swed.WriteBytes(_bytes.Buffer, new byte[_bytes.Length]);
        Assert.That(written, Is.True);
        Assert.That(Array.TrueForAll(_bytes.Span.ToArray(), b => b == 0), Is.True);

        var toWrite = new byte[_bytes.Length];
        const byte fillValue = 235;
        Array.Fill(toWrite, fillValue);
        _swed.Unsafe.WriteStructs<byte>(_bytes.Buffer.ToPointer(), toWrite);
        Assert.That(Array.TrueForAll(_bytes.Span.ToArray(), b => b == fillValue), Is.True);
    }
}