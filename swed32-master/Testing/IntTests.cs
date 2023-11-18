namespace Testing;

[TestFixture]
public unsafe class IntTests
{
    private byte[] _bytes = new byte[sizeof(int)];
    private Swed _swed = new(Process.GetCurrentProcess());

    [Test]
    public void ReadIntTest()
    {
        var n = Random.Shared.Next();
        var nBytes = BitConverter.GetBytes(n);
        Assert.That(_bytes.Length, Is.EqualTo(nBytes.Length));
        _bytes = nBytes;
        
        fixed (byte* b0 = &_bytes[0])
        {
            var safePtr = new IntPtr(b0);
            var read = _swed.ReadInt(safePtr);
            Assert.That(read, Is.EqualTo(n));
        }
    }

    [Test]
    public void WriteIntTest()
    {
        var n = Random.Shared.Next();
        var safePtr = new IntPtr(&n);
        var read = _swed.ReadInt(safePtr);

        Assert.That(read, Is.EqualTo(n));
    }
    
    [Test]
    public void ReadUIntTest()
    {
        var n = (uint)Random.Shared.Next();
        var nBytes = BitConverter.GetBytes(n);
        Assert.That(_bytes.Length, Is.EqualTo(nBytes.Length));
        _bytes = nBytes;
        
        fixed (byte* b0 = &_bytes[0])
        {
            var safePtr = new IntPtr(b0);
            var read = _swed.ReadInt(safePtr);
            Assert.That(read, Is.EqualTo(n));
        }
    }

    [Test]
    public void WriteUIntTest()
    {
        var n = (uint)Random.Shared.Next();
        var safePtr = new IntPtr(&n);
        var read = _swed.ReadInt(safePtr);

        Assert.That(read, Is.EqualTo(n));
    }
}