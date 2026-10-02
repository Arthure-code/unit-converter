namespace UnitConverter.Core.Conversions
{
    /// <summary>
    /// One conversion, from one unit to another.
    /// </summary>
    public interface IUnitConversion
    {
        /// <summary>The name the screen uses to ask for this conversion.</summary>
        string Key { get; }

        /// <summary>The unit the value is read in.</summary>
        string From { get; }

        /// <summary>The unit the value is given in.</summary>
        string To { get; }

        /// <summary>Converts one value from <see cref="From"/> to <see cref="To"/>.</summary>
        double Convert(double value);

        /// <summary>
        /// The conversion in one sentence, for the help page, so that what
        /// is written there is what the code does.
        /// </summary>
        string Describe();
    }
}
