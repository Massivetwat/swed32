using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Swed32;

public partial class Swed
{
    public SwedUnsafe Unsafe => new(Proc.Handle);

    /// <summary>
    /// Ref struct allows temporary unsafe usage without potential boxing semantics.
    /// Guarantees this object disappears in the same scope as declaration, preventing
    /// anything stupid.
    ///
    /// Also only as large as a handle so there's basically no perf hit
    /// </summary>
    public readonly unsafe ref struct SwedUnsafe
    {
        private readonly void* _h;

        public SwedUnsafe(void* handle)
        {
            _h = handle;
        }

        public SwedUnsafe(IntPtr handle) : this(handle.ToPointer())
        {
        }

        public nuint ReadBytes(void* address, Span<byte> bytes)
        {
            nuint bytesRead = 0;
            fixed (void* b0 = &bytes.GetPinnableReference())
            {
                Kernel32.ReadProcessMemory(
                    _h,
                    address,
                    b0,
                    (nuint)bytes.Length,
                    &bytesRead);
            }

            return bytesRead;
        }
        
        public nuint WriteBytes(void* address, ReadOnlySpan<byte> bytes)
        {
            nuint bytesWritten = 0;
            fixed (void* b0 = &bytes.GetPinnableReference())
            {
                Kernel32.WriteProcessMemory(
                    _h,
                    address,
                    b0,
                    (nuint)bytes.Length,
                    &bytesWritten);
            }

            return bytesWritten;
        }
        
        public nuint ReadStructs<T>(void* address, Span<T> outBuffer)
            where T : unmanaged
        {
            var bSpan = MemoryMarshal.Cast<T, byte>(outBuffer);
            return ReadBytes(address, bSpan);
        }

        public nuint ReadStruct<T>(void* address, out T value)
            where T : unmanaged
        {
            value = default;
            var tSpan = MemoryMarshal.CreateSpan(ref value, 1);
            return ReadStructs(address, tSpan);
        }
        
        public nuint WriteStructs<T>(void* address, ReadOnlySpan<T> inBuffer)
            where T : unmanaged
        {
#if DEBUG
            var defensiveCopies = new T[inBuffer.Length];
            inBuffer.CopyTo(defensiveCopies);
#endif
            
            var bSpan = MemoryMarshal.Cast<T, byte>(inBuffer);
            var writtenBytes = WriteBytes(address, bSpan);
            
#if DEBUG
            if (!defensiveCopies.AsSpan().SequenceEqual(inBuffer))
                throw new ArithmeticException(
                    "One or more values read from read-only buffer are not the same as the defensive copies");
#endif

            return writtenBytes;
        }

        public nuint WriteStruct<T>(void* address, ref T value)
            where T : unmanaged
        {
            var tSpan = MemoryMarshal.CreateReadOnlySpan(ref value, 1);
            return WriteStructs(address, tSpan);
        }
    }
}