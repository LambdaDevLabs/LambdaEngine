using System.Runtime.InteropServices;

namespace LambdaEngine;

/// <summary>
/// Represents a 32-byte hash value.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public readonly record struct Hash32 {
    [FieldOffset(0)] private readonly long l0;
    [FieldOffset(8)] private readonly long l1;
    [FieldOffset(16)] private readonly long l2;
    [FieldOffset(24)] private readonly long l3;

    /// <summary>
    /// Creates a new TextureHash based on the specified <see cref="byte"/>[].
    /// </summary>
    /// <param name="hash"></param>
    public unsafe Hash32(byte[] hash) {
        fixed (byte* ptr = hash) {
            l0 = *(long*)ptr;
            l1 = *(long*)(ptr + 8);   
            l2 = *(long*)(ptr + 16);   
            l3 = *(long*)(ptr + 24);   
        }
    }

    public Hash32(long l0, long l1, long l2, long l3) {
        this.l0 = l0;
        this.l1 = l1;
        this.l2 = l2;
        this.l3 = l3;
    }
}