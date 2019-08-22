using System;
using System.Text;
using System.Threading;
using MassiveDynamicProxyGenerator.Utils;

namespace MassiveDynamicProxyGenerator
{
    /// <summary>
    /// Type name creator using korean alphabet.
    /// </summary>
    /// <seealso cref="MassiveDynamicProxyGenerator.ITypeNameCreator" />
    public class KoreanTypeNameCreator : ITypeNameCreator
    {
        private readonly int defaultTypeLength;
        private readonly Random rand;
        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="KoreanTypeNameCreator"/> class.
        /// </summary>
        public KoreanTypeNameCreator()
        {
            this.defaultTypeLength = 7;
            this.counter = 10000;
            this.rand = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KoreanTypeNameCreator"/> class.
        /// </summary>
        /// <param name="defaultTypeLength">The default type length.</param>
        /// <exception cref="ArgumentOutOfRangeException">Parameter defaultTypeLength must by greater than zero.</exception>
        public KoreanTypeNameCreator(int defaultTypeLength)
        {
            if (defaultTypeLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(defaultTypeLength), "Parameter defaultTypeLength must by greater than zero.");
            }

            this.defaultTypeLength = defaultTypeLength;
            this.counter = 10000;
            this.rand = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <returns>
        /// A new name of the method.
        /// </returns>
        public string CreateMethodName()
        {
            int count = Interlocked.Increment(ref this.counter);
            StringBuilder sb = new StringBuilder(this.defaultTypeLength);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (this.defaultTypeLength > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, this.defaultTypeLength - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="length">The length of name.</param>
        /// <returns>
        /// A new name of the method.
        /// </returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        /// <exception cref="ArgumentOutOfRangeException">length - length</exception>
        public string CreateMethodName(string prefix, int length)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"Parameter {nameof(length)} must by more than zero.");
            }

            int count = Interlocked.Increment(ref this.counter);
            StringBuilder sb = new StringBuilder(prefix.Length + length);
            sb.Append(prefix);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (length + prefix.Length > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, (length + prefix.Length) - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <returns>
        /// A new name of the type.
        /// </returns>
        public string CreateTypeName()
        {
            int count = Interlocked.Increment(ref this.counter);
            StringBuilder sb = new StringBuilder(this.defaultTypeLength);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (this.defaultTypeLength > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, this.defaultTypeLength - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="length">The length of name.</param>
        /// <returns>
        /// A new name of the type.
        /// </returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        /// <exception cref="ArgumentOutOfRangeException">length - length</exception>
        public string CreateTypeName(string prefix, int length)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"Parameter {nameof(length)} must by more than zero.");
            }

            int count = Interlocked.Increment(ref this.counter);
            StringBuilder sb = new StringBuilder(prefix.Length + length);
            sb.Append(prefix);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (length + prefix.Length > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, (length + prefix.Length) - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }
    }
}
