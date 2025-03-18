namespace Wisedev.Magic.Titam.Crypto
{
    public class RC4Encrypter : StreamEncrypter
    {
        private byte[] m_key;
        private byte m_x;
        private byte m_y;

        public RC4Encrypter(string baseKey, string nonce)
        {
            InitState(baseKey, nonce);
        }

        public override void Destruct()
        {
            base.Destruct();

            m_key = null;
            m_x = 0;
            m_y = 0;
        }

        public void InitState(string baseKey, string nonce)
        {
            string key = baseKey + nonce;

            m_key = new byte[256];
            m_x = 0;
            m_y = 0;

            for (int i = 0; i < 256; i++)
            {
                m_key[i] = (byte)i;
            }

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (byte)(j + m_key[i] + key[i % key.Length]);

                byte tmpSwap = m_key[i];

                m_key[i] = m_key[j];
                m_key[j] = tmpSwap;
            }

            for (int i = 0; i < key.Length; i++)
            {
                m_x += 1;
                m_y += m_key[m_x];

                byte tmpSwap = m_key[m_y];

                m_key[m_y] = m_key[m_x];
                m_key[m_x] = tmpSwap;
            }
        }

        public override int Decrypt(byte[] input, byte[] output, int length)
        {
            return Encrypt(input, output, length);
        }

        public override int Encrypt(byte[] input, byte[] output, int length)
        {
            for (int i = 0; i < length; i++)
            {
                m_x += 1;
                m_y += m_key[m_x];

                byte tmpSwap = m_key[m_y];

                m_key[m_y] = m_key[m_x];
                m_key[m_x] = tmpSwap;

                output[i] = (byte)(input[i] ^ m_key[(byte)(m_key[m_x] + m_key[m_y])]);
            }

            return 0;
        }
    }
}