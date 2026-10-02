using UnitConverter.Core.Conversions;

namespace UnitConverter.Core.Tests
{
    public class TemperatureConversionTest
    {
        [Theory]
        [InlineData(0, 32)]
        [InlineData(100, 212)]
        [InlineData(37, 98.6)]
        [InlineData(-40, -40)]
        public void CelsiusBecomesFahrenheit(double celsius, double fahrenheit)
        {
            //Given the conversion towards Fahrenheit
            TemperatureConversion conversion = TemperatureConversion.CelsiusToFahrenheit("c-to-f");

            //Then
            Assert.Equal(fahrenheit, conversion.Convert(celsius), 10);
        }

        [Theory]
        [InlineData(32, 0)]
        [InlineData(212, 100)]
        [InlineData(-40, -40)]
        public void FahrenheitBecomesCelsius(double fahrenheit, double celsius)
        {
            //Given the conversion towards Celsius
            TemperatureConversion conversion = TemperatureConversion.FahrenheitToCelsius("f-to-c");

            //Then
            Assert.Equal(celsius, conversion.Convert(fahrenheit), 10);
        }

        [Fact]
        public void ATemperatureConvertedAndConvertedBackComesHome()
        {
            //Given the two conversions
            TemperatureConversion towardsFahrenheit = TemperatureConversion.CelsiusToFahrenheit("c-to-f");
            TemperatureConversion towardsCelsius = TemperatureConversion.FahrenheitToCelsius("f-to-c");

            //Then
            Assert.Equal(21.5, towardsCelsius.Convert(towardsFahrenheit.Convert(21.5)), 10);
        }

        [Fact]
        public void EachConversionCarriesItsTwoUnits()
        {
            //Given the conversion towards Fahrenheit
            TemperatureConversion conversion = TemperatureConversion.CelsiusToFahrenheit("c-to-f");

            //Then
            Assert.Equal("c-to-f", conversion.Key);
            Assert.Equal("degré Celsius", conversion.From);
            Assert.Equal("degré Fahrenheit", conversion.To);
        }

        [Fact]
        public void ATemperatureDescribesItselfWithItsFormula()
        {
            //Given the two conversions, whose rule is not one factor
            TemperatureConversion towardsFahrenheit = TemperatureConversion.CelsiusToFahrenheit("c-to-f");
            TemperatureConversion towardsCelsius = TemperatureConversion.FahrenheitToCelsius("f-to-c");

            //Then the sentence is the formula itself
            Assert.Contains("9 / 5", towardsFahrenheit.Describe(), StringComparison.Ordinal);
            Assert.Contains("5 / 9", towardsCelsius.Describe(), StringComparison.Ordinal);
        }
    }
}
