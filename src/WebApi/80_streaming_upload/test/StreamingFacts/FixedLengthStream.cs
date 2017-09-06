using System;
using System.IO;

namespace StreamingFacts
{
    public class FixedLengthStream : Stream
    {
        long position;

        public FixedLengthStream(long length)
        {
            Length = length <= 0
                ? throw new ArgumentOutOfRangeException(nameof(length))
                : length;
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset and length.");
            int actualCount = checked((int)(Length - Position));
            if (actualCount > count)
                actualCount = count;
            if (actualCount <= 0)
                return 0;
            for (int i = 0; i < actualCount; ++i)
            {
                buffer[offset + i] = 6;
                ++position;
            }

            return actualCount;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = false;
        public override bool CanWrite { get; } = false;
        public override long Length { get; }

        public override long Position
        {
            get => position;
            set => throw new NotSupportedException();
        }
    }
}