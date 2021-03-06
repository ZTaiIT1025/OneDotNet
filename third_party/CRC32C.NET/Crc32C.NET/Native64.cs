﻿using System.Runtime.InteropServices;

namespace Crc32C
{
    class Native64 : NativeProxy
    {
        public Native64() : base("crc32c64.dll") { }

        public override uint Append(uint crc, byte[] input, int offset, int length)
        {
            return AppendInternal(crc, input, offset, length);
        }

        private unsafe uint AppendInternal(uint initial, byte[] input, int offset, int length)
        {
            fixed (byte* ptr = &input[offset])
                return crc32c_append(initial, ptr, checked((uint)length));
        }

        [DllImport("crc32c64.dll", CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern uint crc32c_append(uint crc, byte* input, ulong length);
    }
}
