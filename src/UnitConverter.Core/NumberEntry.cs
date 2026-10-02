using System.Globalization;

namespace UnitConverter.Core
{
    /// <summary>
    /// The number being typed on the keypad. The keypad has no text field:
    /// the digits are collected here, and the rules about what a number may
    /// look like live here rather than in the page.
    /// </summary>
    public sealed class NumberEntry
    {
        /// <summary>The character the keypad shows for the decimal point.</summary>
        public const char DecimalSeparator = '.';

        /// <summary>How many characters the display can hold.</summary>
        public const int MaximumLength = 12;

        private string _text = string.Empty;

        /// <summary>What the display shows, empty when nothing is typed.</summary>
        public string Text => _text;

        /// <summary>True while nothing has been typed.</summary>
        public bool IsEmpty => _text.Length == 0;

        /// <summary>
        /// Adds one digit. A leading zero is kept on its own, so zero can be
        /// converted, but the next digit takes its place: 0 then 5 reads 5,
        /// never 05.
        /// </summary>
        public void Append(char digit)
        {
            if (!char.IsAsciiDigit(digit) || _text.Length >= MaximumLength)
            {
                return;
            }

            if (IsALoneZero())
            {
                _text = _text[..^1];
            }

            _text += digit;
        }

        /// <summary>
        /// Adds the decimal point, once, and opens the number with a zero
        /// when it is the first key pressed.
        /// </summary>
        public void AppendDecimalSeparator()
        {
            if (_text.Contains(DecimalSeparator, StringComparison.Ordinal) || _text.Length >= MaximumLength)
            {
                return;
            }

            if (_text.Length == 0 || _text == "-")
            {
                _text += '0';
            }

            _text += DecimalSeparator;
        }

        /// <summary>Turns the number negative, or positive again.</summary>
        public void ToggleSign()
        {
            if (_text.StartsWith('-'))
            {
                _text = _text[1..];
            }
            else if (_text.Length < MaximumLength)
            {
                _text = '-' + _text;
            }
        }

        /// <summary>Erases the last key pressed.</summary>
        public void RemoveLast()
        {
            if (_text.Length > 0)
            {
                _text = _text[..^1];
            }
        }

        /// <summary>Starts over.</summary>
        public void Clear() => _text = string.Empty;

        /// <summary>
        /// Reads the number. Answers false while the display holds nothing
        /// that stands on its own, an empty display or a lone minus sign. A
        /// number left on its decimal point reads as its whole part.
        /// </summary>
        public bool TryGetValue(out double value)
        {
            return double.TryParse(_text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out value);
        }

        private bool IsALoneZero()
        {
            return _text == "0" || _text == "-0";
        }
    }
}
