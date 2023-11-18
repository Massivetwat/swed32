namespace Testing;

public unsafe class ByteTests
{
    private Swed _swed = new(Process.GetCurrentProcess());
    private const int BytesLength = 451_723;
    private byte[] _bytes = new byte[BytesLength];

    [Test]
    public void ReadBytesTest()
    {
        fixed (byte* b0 = &_bytes[0])
        {
            var safePtr = new IntPtr(b0);
            var buffer = _swed.ReadBytes(safePtr, _bytes.Length);

            Assert.That(buffer, Is.EquivalentTo(_bytes));
        }
    }

    [Test]
    public void WriteBytesTest()
    {
        fixed (byte* b0 = &_bytes[0])
        {
            var safePtr = new IntPtr(b0);
            var written = _swed.WriteBytes(safePtr, new byte[_bytes.Length]);
            
            Assert.That(written, Is.True);
            Assert.That(Array.TrueForAll(_bytes, b => b == 0), Is.True);
        }
    }
}