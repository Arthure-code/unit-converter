using System.Globalization;

namespace UnitConverter.Core.Conversions
{
    /// <summary>
    /// A conversion that is one multiplication, which is every conversion
    /// here except the two temperatures.
    /// </summary>
    public sealed class LinearConversion : IUnitConversion
    {
        private readonly double _factor;

        public LinearConversion(string key, string from, string to, double factor)
        {
            Key = key;
            From = from;
            To = to;
            _factor = factor;
        }

        public string Key { get; }

        public string From { get; }

        public string To { get; }

        public double Convert(double value) => value * _factor;

        public string Describe()
        {
            string rate = Convert(1).ToString("0.######", CultureInfo.InvariantCulture);

            return $"1 {From} = {rate} {To}";
        }

        /// <summary>
        /// The same conversion read the other way round. The inverse is
        /// computed, never typed a second time, so a value that makes the
        /// round trip comes back to itself.
        /// </summary>
        public LinearConversion Inverse(string key) => new LinearConversion(key, To, From, 1 / _factor);
    }
}
