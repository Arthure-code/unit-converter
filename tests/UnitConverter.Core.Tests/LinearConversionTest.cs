using UnitConverter.Core.Conversions;

namespace UnitConverter.Core.Tests
{
    public class LinearConversionTest
    {
        private static LinearConversion InchToCentimetre()
            => new LinearConversion("in-to-cm", "inch", "centimetre", 2.54);

        [Fact]
        public void AConversionMultipliesByItsFactor()
        {
            //Given the inch, which is 2.54 centimetres exactly
            LinearConversion conversion = InchToCentimetre();

            //Then
            Assert.Equal(25.4, conversion.Convert(10), 10);
        }

        [Fact]
        public void AConversionCarriesItsTwoUnits()
        {
            //Given the conversion
            LinearConversion conversion = InchToCentimetre();

            //Then
            Assert.Equal("in-to-cm", conversion.Key);
            Assert.Equal("inch", conversion.From);
            Assert.Equal("centimetre", conversion.To);
        }

        [Fact]
        public void TheInverseReadsTheSamePairTheOtherWayRound()
        {
            //Given the conversion read backwards
            LinearConversion inverse = InchToCentimetre().Inverse("cm-to-in");

            //Then the units swap places
            Assert.Equal("cm-to-in", inverse.Key);
            Assert.Equal("centimetre", inverse.From);
            Assert.Equal("inch", inverse.To);
            Assert.Equal(1, inverse.Convert(2.54), 10);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(37.5)]
        [InlineData(-12.25)]
        public void AValueConvertedAndConvertedBackComesHome(double value)
        {
            //Given a conversion and its inverse, which is computed rather
            //than typed a second time
            LinearConversion conversion = InchToCentimetre();
            LinearConversion inverse = conversion.Inverse("cm-to-in");

            //Then the round trip loses nothing
            Assert.Equal(value, inverse.Convert(conversion.Convert(value)), 10);
        }

        [Fact]
        public void AConversionDescribesItselfWithItsRate()
        {
            //Given the conversion
            LinearConversion conversion = InchToCentimetre();

            //Then the sentence says what one unit is worth
            Assert.Equal("1 inch = 2.54 centimetre", conversion.Describe());
        }
    }
}
