namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISupportsCustomCharset
    {
        /// <summary>
        /// Gets or sets the charset used by the algorithm.
        /// </summary>
        /// <returns></returns>
        string Charset { get; set; }
    }
}
