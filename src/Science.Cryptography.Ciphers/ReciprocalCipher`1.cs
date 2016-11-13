namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class ReciprocalKeyedCipher<TKey> : IKeyedCipher<TKey>
    {
        public string Encrypt(string plaintext, TKey key)
        {
            return this.Crypt(plaintext, key);
        }

        public string Decrypt(string ciphertext, TKey key)
        {
            return this.Crypt(ciphertext, key);
        }

        protected abstract string Crypt(string text, TKey key);
    }
}
