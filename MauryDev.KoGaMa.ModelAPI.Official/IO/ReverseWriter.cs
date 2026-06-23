using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Official.IO
{
    public class ReverseWriter
    {
        private readonly Stream _stream;
        private readonly byte[] _cache = new byte[32];

        public ReverseWriter(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public void WriteUsingCache(int len, bool reverse = false)
        {
            if (reverse)
            {
                Array.Reverse(_cache, 0, len);
            }
            _stream.Write(_cache, 0, len);
        }

        public void Write(int value)
        {
            _cache[0] = (byte)value;
            _cache[1] = (byte)(value >> 8);
            _cache[2] = (byte)(value >> 16);
            _cache[3] = (byte)(value >> 24);

            WriteUsingCache(4, true);
        }

        public void Write(short value)
        {
            _cache[0] = (byte)value;
            _cache[1] = (byte)(value >> 8);

            WriteUsingCache(2, true);
        }

        public void Write(byte value)
        {
            _stream.WriteByte(value);
        }

        public void Write(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            _stream.Write(buffer, 0, buffer.Length);
        }
    }

}
