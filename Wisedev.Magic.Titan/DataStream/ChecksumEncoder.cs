namespace Wisedev.Magic.Titam.DataStream
{
    using Wisedev.Magic.Titam.Logic;
    using System.Runtime.CompilerServices;

    public class ChecksumEncoder
    {
        private int m_checksum;
        private int m_snapshotChecksum;

        private bool m_enabled;

        public ChecksumEncoder()
        {
            m_enabled = true;
        }

        public void EnableCheckSum(bool enable)
        {
            if (!m_enabled || enable)
            {
                if (!m_enabled && enable)
                {
                    m_checksum = m_snapshotChecksum;
                }

                m_enabled = enable;
            }
            else
            {
                m_snapshotChecksum = m_checksum;
                m_enabled = false;
            }
        }

        public void ResetCheckSum()
        {
            m_checksum = 0;
        }

        public virtual void WriteBoolean(bool value)
        {
            m_checksum = (value ? 13 : 7) + __ROR4__(m_checksum, 31);
        }

        public virtual void WriteByte(byte value)
        {
            m_checksum = value + __ROR4__(m_checksum, 31) + 11;
        }

        public virtual void WriteShort(short value)
        {
            m_checksum = value + __ROR4__(m_checksum, 31) + 19;
        }

        public virtual void WriteInt(int value)
        {
            m_checksum = value + __ROR4__(m_checksum, 31) + 9;
        }

        public virtual void WriteLong(LogicLong value)
        {
            value.Encode(this);
        }

        public virtual void WriteLongLong(long value)
        {
            int high = (int)(value >> 32);
            int low = (int)value;

            m_checksum = high + __ROR4__(low + __ROR4__(m_checksum, 31) + 67, 31) + 91;
        }

        public virtual void WriteBytes(byte[] value, int length)
        {
            m_checksum = ((value != null ? length + 28 : 27) + (m_checksum >> 31)) | (m_checksum << (32 - 31));
        }

        public virtual void WriteString(string? value)
        {
            m_checksum = (value != null ? value.Length + 28 : 27) + __ROR4__(m_checksum, 31);
        }

        public virtual void WriteStringReference(string value)
        {
            m_checksum = value.Length + __ROR4__(m_checksum, 31) + 38;
        }

        public bool IsCheckSumEnabled()
        {
            return m_enabled;
        }

        public virtual bool IsCheckSumOnlyMode()
        {
            return true;
        }

        public bool Equals(ChecksumEncoder encoder)
        {
            if (encoder != null)
            {
                int checksum = encoder.m_checksum;
                int checksum2 = m_checksum;

                if (!encoder.m_enabled)
                {
                    checksum = encoder.m_snapshotChecksum;
                }

                if (!m_enabled)
                {
                    checksum2 = m_snapshotChecksum;
                }

                return checksum == checksum2;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int __ROR4__(int value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
    }

}
