using UnitConverter.Core.Conversions;

namespace UnitConverter.Core.Tests
{
    public class ConversionCatalogTest
    {
        [Fact]
        public void TheCatalogueHoldsTheTwelveConversionsTheScreenShows()
        {
            //Given the catalogue
            IReadOnlyList<IUnitConversion> all = ConversionCatalog.All;

            //Then
            Assert.Equal(12, all.Count);
        }

        [Fact]
        public void NoTwoConversionsAnswerToTheSameName()
        {
            //Given the catalogue, whose names are what the buttons carry
            IEnumerable<string> keys = ConversionCatalog.All.Select(c => c.Key);

            //Then
            Assert.Equal(12, keys.Distinct(StringComparer.Ordinal).Count());
        }

        [Theory]
        [InlineData("cm-to-in", "centimetre", "inch")]
        [InlineData("in-to-cm", "inch", "centimetre")]
        [InlineData("m-to-ft", "metre", "foot")]
        [InlineData("ft-to-m", "foot", "metre")]
        [InlineData("g-to-oz", "gram", "ounce")]
        [InlineData("oz-to-g", "ounce", "gram")]
        [InlineData("kg-to-lb", "kilogram", "pound")]
        [InlineData("lb-to-kg", "pound", "kilogram")]
        [InlineData("km-to-mi", "kilometre", "mile")]
        [InlineData("mi-to-km", "mile", "kilometre")]
        [InlineData("c-to-f", "degree Celsius", "degree Fahrenheit")]
        [InlineData("f-to-c", "degree Fahrenheit", "degree Celsius")]
        public void EachButtonFindsItsConversion(string key, string from, string to)
        {
            //Given the name a button carries
            IUnitConversion? conversion = ConversionCatalog.Find(key);

            //Then
            Assert.NotNull(conversion);
            Assert.Equal(from, conversion!.From);
            Assert.Equal(to, conversion.To);
        }

        [Theory]
        [InlineData("km-to-mile")]
        [InlineData("")]
        [InlineData(null)]
        public void AnUnknownNameFindsNothing(string? key)
        {
            //Given a name no conversion carries
            //Then nothing is found, and nothing throws
            Assert.Null(ConversionCatalog.Find(key));
        }

        [Theory]
        [InlineData("in-to-cm", 1, 2.54)]
        [InlineData("ft-to-m", 1, 0.3048)]
        [InlineData("lb-to-kg", 1, 0.45359237)]
        [InlineData("oz-to-g", 1, 28.349523125)]
        [InlineData("mi-to-km", 1, 1.609344)]
        [InlineData("c-to-f", 100, 212)]
        [InlineData("f-to-c", 32, 0)]
        public void TheFactorsAreTheInternationalDefinitions(string key, double value, double expected)
        {
            //Given a conversion of the catalogue
            IUnitConversion conversion = ConversionCatalog.Find(key)!;

            //Then the result is the defined one, to the digit
            Assert.Equal(expected, conversion.Convert(value), 10);
        }

        [Theory]
        [InlineData("cm-to-in", "in-to-cm")]
        [InlineData("m-to-ft", "ft-to-m")]
        [InlineData("g-to-oz", "oz-to-g")]
        [InlineData("kg-to-lb", "lb-to-kg")]
        [InlineData("km-to-mi", "mi-to-km")]
        [InlineData("c-to-f", "f-to-c")]
        public void EveryPairComesHome(string key, string inverseKey)
        {
            //Given a conversion and the one that reads it backwards
            IUnitConversion conversion = ConversionCatalog.Find(key)!;
            IUnitConversion inverse = ConversionCatalog.Find(inverseKey)!;

            //Then a value that makes the round trip is itself again, which
            //two separately rounded factors would not give
            Assert.Equal(12.5, inverse.Convert(conversion.Convert(12.5)), 10);
        }

        [Fact]
        public void EveryConversionSaysWhatItDoes()
        {
            //Given the catalogue, which the help page reads
            //Then not one conversion is left without its sentence
            Assert.All(ConversionCatalog.All, c => Assert.False(string.IsNullOrWhiteSpace(c.Describe())));
        }
    }
}
