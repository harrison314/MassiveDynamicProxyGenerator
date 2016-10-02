using System;
using System.Text;
using System.Threading;
using MassiveDynamicProxyGenerator.Utils;

namespace MassiveDynamicProxyGenerator
{
    public class KoreanTypeNameCreator : ITypeNameCreator
    {
        private readonly int defaultTypeLenght;
        private readonly Random rand;
        private int counter;

        public KoreanTypeNameCreator()
        {
            this.defaultTypeLenght = 7;
            this.counter = 10000;
            this.rand = new Random(DateTime.Now.Millisecond);
        }

        public KoreanTypeNameCreator(int defaultTypeLenght)
        {
            // TODO: vynimka
            this.defaultTypeLenght = defaultTypeLenght;
            this.counter = 10000;
            this.rand = new Random(DateTime.Now.Millisecond);
        }

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
