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
        private readonly int defaultTypeLenght;
        private readonly Random rand;
        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="KoreanTypeNameCreator"/> class.
        /// </summary>
        public KoreanTypeNameCreator()
        {
            this.defaultTypeLenght = 7;
            this.counter = 10000;
            this.rand = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KoreanTypeNameCreator"/> class.
        /// </summary>
        /// <param name="defaultTypeLenght">The default type lenght.</param>
        public KoreanTypeNameCreator(int defaultTypeLenght)
        {
            // TODO: vynimka
            this.defaultTypeLenght = defaultTypeLenght;
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
            StringBuilder sb = new StringBuilder(this.defaultTypeLenght);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (this.defaultTypeLenght > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, this.defaultTypeLenght - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the name of the method.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="lenght">The lenght of name.</param>
        /// <returns>
        /// A new name of the method.
        /// </returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        /// <exception cref="ArgumentOutOfRangeException">lenght - lenght</exception>
        public string CreateMethodName(string prefix, int lenght)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (lenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lenght), $"Parameter {nameof(lenght)} must by more than zero.");
            }

            int count = Interlocked.Increment(ref this.counter);
            StringBuilder sb = new StringBuilder(prefix.Length + lenght);
            sb.Append(prefix);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (lenght + prefix.Length > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, (lenght + prefix.Length) - sb.Length);
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
            StringBuilder sb = new StringBuilder(this.defaultTypeLenght);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (this.defaultTypeLenght > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, this.defaultTypeLenght - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the name of the type.
        /// </summary>
        /// <param name="prefix">The name prefix.</param>
        /// <param name="lenght">The lenght of name.</param>
        /// <returns>
        /// A new name of the type.
        /// </returns>
        /// <exception cref="ArgumentNullException">prefix</exception>
        /// <exception cref="ArgumentOutOfRangeException">lenght - lenght</exception>
        public string CreateTypeName(string prefix, int lenght)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (lenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lenght), $"Parameter {nameof(lenght)} must by more than zero.");
            }

            int count = Interlocked.Increment(ref this.counter);
            StringBuilder sb = new StringBuilder(prefix.Length + lenght);
            sb.Append(prefix);

            sb.Append(KoreanAlphabet.GenerateString(count));
            if (lenght + prefix.Length > sb.Length)
            {
                string apendix = KoreanAlphabet.GenerateRandom(this.rand, (lenght + prefix.Length) - sb.Length);
                sb.Append(apendix);
            }

            return sb.ToString();
        }
    }
}
