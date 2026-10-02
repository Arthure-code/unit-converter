namespace UnitConverter.Core.Conversions
{
    /// <summary>
    /// The two temperature conversions. A degree is not a quantity of
    /// something, so its scale has an offset and one multiplication is not
    /// enough.
    /// </summary>
    public sealed class TemperatureConversion : IUnitConversion
    {
        private const double FreezingPointInFahrenheit = 32;
        private const double DegreesPerCelsiusDegree = 9.0 / 5.0;

        private readonly bool _towardsFahrenheit;

        private TemperatureConversion(string key, string from, string to, bool towardsFahrenheit)
        {
            Key = key;
            From = from;
            To = to;
            _towardsFahrenheit = towardsFahrenheit;
        }

        public static TemperatureConversion CelsiusToFahrenheit(string key)
            => new TemperatureConversion(key, "degree Celsius", "degree Fahrenheit", true);

        public static TemperatureConversion FahrenheitToCelsius(string key)
            => new TemperatureConversion(key, "degree Fahrenheit", "degree Celsius", false);

        public string Key { get; }

        public string From { get; }

        public string To { get; }

        public double Convert(double value)
        {
            return _towardsFahrenheit
                ? (value * DegreesPerCelsiusDegree) + FreezingPointInFahrenheit
                : (value - FreezingPointInFahrenheit) / DegreesPerCelsiusDegree;
        }

        public string Describe()
        {
            return _towardsFahrenheit
                ? "°F = (°C × 9 / 5) + 32"
                : "°C = (°F − 32) × 5 / 9";
        }
    }
}
