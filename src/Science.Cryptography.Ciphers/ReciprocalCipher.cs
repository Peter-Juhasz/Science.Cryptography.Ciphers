namespace Science.Cryptography.Ciphers
{
    public abstract class ReciprocalCipher : ICipher
    {
        public string Encrypt(string plaintext)
        {
            return this.Crypt(plaintext);
        }

        public string Decrypt(string ciphertext)
        {
            return this.Crypt(ciphertext);
        }

        protected abstract string Crypt(string text);
    }
}
