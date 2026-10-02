using UnitConverter.Core;

namespace UnitConverter.Core.Tests
{
    public class NumberEntryTest
    {
        private static NumberEntry Typed(string keys)
        {
            var entry = new NumberEntry();

            foreach (char key in keys)
            {
                if (key == NumberEntry.DecimalSeparator)
                {
                    entry.AppendDecimalSeparator();
                }
                else
                {
                    entry.Append(key);
                }
            }

            return entry;
        }

        [Fact]
        public void ANewEntryShowsNothing()
        {
            //Given a keypad nobody has touched
            var entry = new NumberEntry();

            //Then
            Assert.True(entry.IsEmpty);
            Assert.Equal(string.Empty, entry.Text);
            Assert.False(entry.TryGetValue(out _));
        }

        [Fact]
        public void TheDigitsPressedMakeTheNumber()
        {
            //Given three digits
            NumberEntry entry = Typed("407");

            //Then
            Assert.Equal("407", entry.Text);
            Assert.True(entry.TryGetValue(out double value));
            Assert.Equal(407, value);
        }

        [Fact]
        public void ALoneZeroCanBeConverted()
        {
            //Given the zero key alone, which the first version refused to read
            NumberEntry entry = Typed("0");

            //Then zero is a number like any other
            Assert.Equal("0", entry.Text);
            Assert.True(entry.TryGetValue(out double value));
            Assert.Equal(0, value);
        }

        [Fact]
        public void ADigitTakesThePlaceOfALeadingZero()
        {
            //Given a zero then a digit
            NumberEntry entry = Typed("05");

            //Then the number never reads 05
            Assert.Equal("5", entry.Text);
        }

        [Fact]
        public void AZeroAfterTheDecimalPointIsKept()
        {
            //Given a number that starts below one
            NumberEntry entry = Typed("0.05");

            //Then
            Assert.Equal("0.05", entry.Text);
            Assert.True(entry.TryGetValue(out double value));
            Assert.Equal(0.05, value);
        }

        [Fact]
        public void TheDecimalPointOpensTheNumberWithAZero()
        {
            //Given the decimal point pressed first
            var entry = new NumberEntry();

            //When
            entry.AppendDecimalSeparator();

            //Then
            Assert.Equal("0.", entry.Text);
        }

        [Fact]
        public void TheDecimalPointIsAcceptedOnlyOnce()
        {
            //Given a number that already carries a decimal point
            NumberEntry entry = Typed("1.5");

            //When the key is pressed again
            entry.AppendDecimalSeparator();

            //Then
            Assert.Equal("1.5", entry.Text);
        }

        [Fact]
        public void ANumberLeftOnItsDecimalPointReadsAsTheWholePart()
        {
            //Given a number whose decimals were never typed
            NumberEntry entry = Typed("12.");

            //Then the conversion still has something to work on
            Assert.True(entry.TryGetValue(out double value));
            Assert.Equal(12, value);
        }

        [Fact]
        public void TheSignKeyTurnsTheNumberNegativeAndBack()
        {
            //Given a number on the display
            NumberEntry entry = Typed("40");

            //When
            entry.ToggleSign();

            //Then
            Assert.Equal("-40", entry.Text);
            Assert.True(entry.TryGetValue(out double negative));
            Assert.Equal(-40, negative);

            entry.ToggleSign();
            Assert.Equal("40", entry.Text);
        }

        [Fact]
        public void ALoneMinusSignIsNotANumber()
        {
            //Given the sign key pressed on an empty display
            var entry = new NumberEntry();

            //When
            entry.ToggleSign();

            //Then
            Assert.Equal("-", entry.Text);
            Assert.False(entry.TryGetValue(out _));
        }

        [Fact]
        public void ADigitAfterTheSignBuildsANegativeNumber()
        {
            //Given the sign pressed before the digits
            var entry = new NumberEntry();
            entry.ToggleSign();
            entry.Append('4');
            entry.Append('0');

            //Then
            Assert.Equal("-40", entry.Text);
        }

        [Fact]
        public void BackspaceErasesTheLastKey()
        {
            //Given three digits
            NumberEntry entry = Typed("123");

            //When
            entry.RemoveLast();

            //Then
            Assert.Equal("12", entry.Text);
        }

        [Fact]
        public void BackspaceOnAnEmptyDisplayChangesNothing()
        {
            //Given an empty display
            var entry = new NumberEntry();

            //When
            entry.RemoveLast();

            //Then
            Assert.True(entry.IsEmpty);
        }

        [Fact]
        public void ClearStartsOver()
        {
            //Given a number typed
            NumberEntry entry = Typed("98.6");

            //When
            entry.Clear();

            //Then
            Assert.True(entry.IsEmpty);
        }

        [Fact]
        public void TheDisplayStopsAtItsLength()
        {
            //Given more digits than the display can hold
            NumberEntry entry = Typed(new string('9', NumberEntry.MaximumLength + 5));

            //Then
            Assert.Equal(NumberEntry.MaximumLength, entry.Text.Length);
        }

        [Theory]
        [InlineData('a')]
        [InlineData(' ')]
        [InlineData('-')]
        public void AKeyThatIsNotADigitIsIgnored(char key)
        {
            //Given a key the keypad does not carry
            var entry = new NumberEntry();

            //When
            entry.Append(key);

            //Then
            Assert.True(entry.IsEmpty);
        }
    }
}
