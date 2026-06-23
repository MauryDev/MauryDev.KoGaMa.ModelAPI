using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MauryDev.KoGaMa.ModelAPI.Official.IO
{
    public class ReverseReader
    {
        Stream _stream;
        public ReverseReader(Stream stream) { _stream = stream; }
        byte[] _cache = new byte[32];

        public void ReadUsingCache(int len, bool reverse = false)
        {
            _stream.Read(_cache, 0, len);
            if (reverse) Array.Reverse(_cache, 0, len);
        }
        public int ReadInt32()
        {
            ReadUsingCache(4, true);
            return BitConverter.ToInt32(_cache, 0);
        }
        public short ReadInt16()
        {
            ReadUsingCache(2, true);

            return BitConverter.ToInt16(_cache, 0);
        }
        public byte ReadByte()
        {

            return (byte)_stream.ReadByte();
        }
        public byte[] ReadBytes(int len)
        {
            byte[] buffer = new byte[len];
            _stream.Read(buffer, 0, len);
            return buffer;
        }
    }
}
