using Wisedev.Magic.Titan.Logic;
using Wisedev.Magic.Titan.Utils;
using System.Text;
using Wisedev.Magic.Titan.Debug;

namespace Wisedev.Magic.Titan.DataStream
{
    public class ByteStream : ChecksumEncoder
    {
        private int _bitIdx;

        private byte[] _buffer;
        private int _length;
        private int _offset;

        public ByteStream(int capacity)
        {
            _buffer = new byte[capacity];
        }

        public ByteStream(byte[] buffer, int length)
        {
            _length = length;
            _buffer = buffer;
        }

        public int GetLength()
        {
            if (_offset < _length)
            {
                return _length;
            }

            return _offset;
        }

        public int GetOffset()
        {
            return _offset;
        }

        public bool IsAtEnd()
        {
            return _offset >= _length;
        }

        public void Clear(int capacity)
        {
            _buffer = new byte[capacity];
            _offset = 0;
        }

        public byte[] GetByteArray()
        {
            return _buffer;
        }

        public bool ReadBoolean()
        {
            if (_bitIdx == 0)
            {
                ++_offset;
            }

            bool value = (_buffer[_offset - 1] & (1 << _bitIdx)) != 0;
            _bitIdx = (_bitIdx + 1) & 7;
            return value;
        }

        public byte ReadByte()
        {
            _bitIdx = 0;
            return _buffer[_offset++];
        }

        public short ReadShort()
        {
            _bitIdx = 0;

            return (short)((_buffer[_offset++] << 8) |
                            _buffer[_offset++]);
        }

        public int ReadInt()
        {
            _bitIdx = 0;

            return (_buffer[_offset++] << 24) |
                   (_buffer[_offset++] << 16) |
                   (_buffer[_offset++] << 8) |
                   _buffer[_offset++];
        }

        public void WriteVInt(int value)
        {
            EnsureCapacity(5);

            if (value >= 64)
            {
                if (value >= 0x2000)
                {
                    if (value >= 0x100000)
                    {
                        if (value >= 0x8000000)
                        {
                            _buffer[_offset++] = (byte)((value & 0x3F) | 0x80);
                            _buffer[_offset++] = (byte)((value >> 6) & 0x7F);
                        }
                    }
                }
            }
        }

        public LogicLong ReadLong()
        {
            LogicLong logicLong = new LogicLong();
            logicLong.Decode(this);
            return logicLong;
        }

        public LogicLong ReadLong(LogicLong longValue)
        {
            longValue.Decode(this);
            return longValue;
        }

        public long ReadLongLong()
        {
            return LogicLong.ToLong(ReadInt(), ReadInt());
        }

        public int ReadBytesLength()
        {
            _bitIdx = 0;
            return (_buffer[_offset++] << 24) |
                   (_buffer[_offset++] << 16) |
                   (_buffer[_offset++] << 8) |
                   _buffer[_offset++];
        }

        public byte[] ReadBytes(int length, int maxCapacity)
        {
            _bitIdx = 0;

            if (length <= -1)
            {
                if (length != -1)
                {
                    Debugger.Warning("Negative readBytes length encountered.");
                }

                return null;
            }

            if (length <= maxCapacity)
            {
                byte[] array = new byte[length];
                Buffer.BlockCopy(_buffer, _offset, array, 0, length);
                _offset += length;
                return array;
            }

            Debugger.Warning("readBytes too long array, max " + maxCapacity);

            return null;
        }

        public string? ReadString(int maxCapacity=900000)
        {
            int length = ReadBytesLength();

            if (length <= -1)
            {
                if (length != -1)
                {
                    Debugger.Warning("Too long String encountered.");
                }
            }
            else
            {
                if (length <= maxCapacity)
                {
                    string value = Encoding.UTF8.GetString(_buffer, _offset, length);
                    _offset += length;
                    return value;
                }

                Debugger.Warning("Too long String encountered, max " + maxCapacity);
            }

            return null;
        }

        public string ReadStringReference(int maxCapacity)
        {
            int length = ReadBytesLength();

            if (length <= -1)
            {
                Debugger.Warning("Negative String length encountered.");
            }
            else
            {
                if (length <= maxCapacity)
                {
                    string value = Encoding.UTF8.GetString(_buffer, _offset, length);
                    _offset += length;
                    return value;
                }

                Debugger.Warning("Too long String encountered, max " + maxCapacity);
            }

            return string.Empty;
        }

        public override void WriteBoolean(bool value)
        {
            base.WriteBoolean(value);

            if (_bitIdx == 0)
            {
                EnsureCapacity(1);
                _buffer[_offset++] = 0;
            }

            if (value)
            {
                _buffer[_offset - 1] |= (byte)(1 << _bitIdx);
            }

            _bitIdx = (_bitIdx + 1) & 7;
        }

        public override void WriteByte(byte value)
        {
            base.WriteByte(value);
            EnsureCapacity(1);

            _bitIdx = 0;

            _buffer[_offset++] = value;
        }

        public override void WriteShort(short value)
        {
            base.WriteShort(value);
            EnsureCapacity(2);

            _bitIdx = 0;

            _buffer[_offset++] = (byte)(value >> 8);
            _buffer[_offset++] = (byte)value;
        }

        public override void WriteInt(int value)
        {
            base.WriteInt(value);
            EnsureCapacity(4);

            _bitIdx = 0;
                
            _buffer[_offset++] = (byte)(value >> 24);
            _buffer[_offset++] = (byte)(value >> 16);
            _buffer[_offset++] = (byte)(value >> 8);
            _buffer[_offset++] = (byte)value;
        }

        public void WriteIntToByteArray(int value)
        {
            EnsureCapacity(4);
            _bitIdx = 0;

            _buffer[_offset++] = (byte)(value >> 24);
            _buffer[_offset++] = (byte)(value >> 16);
            _buffer[_offset++] = (byte)(value >> 8);
            _buffer[_offset++] = (byte)value;
        }

        public override void WriteLongLong(long value)
        {
            base.WriteLongLong(value);

            WriteIntToByteArray((int)(value >> 32));
            WriteIntToByteArray((int)value);
        }

        public override void WriteBytes(byte[] value, int length)
        {
            base.WriteBytes(value, length);

            if (value == null)
            {
                WriteIntToByteArray(-1);
            }
            else
            {
                EnsureCapacity(length + 4);
                WriteIntToByteArray(length);

                Buffer.BlockCopy(value, 0, _buffer, _offset, length);

                _offset += length;
            }
        }

        public void WriteBytesWithoutLength(byte[] value, int length)
        {
            base.WriteBytes(value, length);

            if (value != null)
            {
                EnsureCapacity(length);
                Buffer.BlockCopy(value, 0, _buffer, _offset, length);
                _offset += length;
            }
        }

        public override void WriteString(string? value)
        {
            base.WriteString(value);

            if (value == null)
            {
                WriteIntToByteArray(-1);
            }
            else
            {
                byte[] bytes = LogicStringUtil.GetBytes(value);
                int length = bytes.Length;

                if (length <= 900000)
                {
                    EnsureCapacity(length + 4);
                    WriteIntToByteArray(length);

                    Buffer.BlockCopy(bytes, 0, _buffer, _offset, length);

                    _offset += length;
                }
                else
                {
                    Debugger.Warning("ByteStream.writeString invalid string length " + length);
                    WriteIntToByteArray(-1);
                }
            }
        }

        public override void WriteStringReference(string value)
        {
            base.WriteStringReference(value);

            byte[] bytes = LogicStringUtil.GetBytes(value);
            int length = bytes.Length;

            if (length <= 900000)
            {
                EnsureCapacity(length + 4);
                WriteIntToByteArray(length);

                Buffer.BlockCopy(bytes, 0, _buffer, _offset, length);

                _offset += length;
            }
            else
            {
                Debugger.Warning("ByteStream.writeString invalid string length " + length);
                WriteIntToByteArray(-1);
            }
        }

        public void SetByteArray(byte[] buffer, int length)
        {
            _offset = 0;
            _bitIdx = 0;
            _buffer = buffer;
            _length = length;
        }

        public void ResetOffset()
        {
            _offset = 0;
            _bitIdx = 0;
        }

        public void SetOffset(int offset)
        {
            _offset = offset;
            _bitIdx = 0;
        }

        public byte[] RemoveByteArray()
        {
            byte[] byteArray = _buffer;
            _buffer = null;
            return byteArray;
        }

        public void EnsureCapacity(int capacity)
        {
            int bufferLength = _buffer.Length;

            if (_offset + capacity > bufferLength)
            {
                byte[] tmpBuffer = new byte[_buffer.Length + capacity + 100];
                Buffer.BlockCopy(_buffer, 0, tmpBuffer, 0, bufferLength);
                _buffer = tmpBuffer;
            }
        }

        public void Destruct()
        {
            _buffer = null;
            _bitIdx = 0;
            _length = 0;
            _offset = 0;
        }
    }
}
