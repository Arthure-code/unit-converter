namespace UnitConverter.Core.Conversions
{
    /// <summary>
    /// Every conversion the application offers, and the only place where a
    /// factor is written.
    /// </summary>
    public static class ConversionCatalog
    {
        // The international definitions, exact to the digit. Each pair is
        // declared once and read backwards by Inverse, so a value converted
        // and converted back comes home.
        private const double CentimetresPerInch = 2.54;
        private const double MetresPerFoot = 0.3048;
        private const double GramsPerOunce = 28.349523125;
        private const double KilogramsPerPound = 0.45359237;
        private const double KilometresPerMile = 1.609344;

        private static readonly LinearConversion InchToCentimetre =
            new LinearConversion("in-to-cm", "pouce", "centimètre", CentimetresPerInch);

        private static readonly LinearConversion FootToMetre =
            new LinearConversion("ft-to-m", "pied", "mètre", MetresPerFoot);

        private static readonly LinearConversion OunceToGram =
            new LinearConversion("oz-to-g", "once", "gramme", GramsPerOunce);

        private static readonly LinearConversion PoundToKilogram =
            new LinearConversion("lb-to-kg", "livre", "kilogramme", KilogramsPerPound);

        private static readonly LinearConversion MileToKilometre =
            new LinearConversion("mi-to-km", "mile", "kilomètre", KilometresPerMile);

        private static readonly IUnitConversion[] Conversions =
        {
            InchToCentimetre.Inverse("cm-to-in"),
            InchToCentimetre,
            FootToMetre.Inverse("m-to-ft"),
            FootToMetre,
            OunceToGram.Inverse("g-to-oz"),
            OunceToGram,
            PoundToKilogram.Inverse("kg-to-lb"),
            PoundToKilogram,
            MileToKilometre.Inverse("km-to-mi"),
            MileToKilometre,
            TemperatureConversion.CelsiusToFahrenheit("c-to-f"),
            TemperatureConversion.FahrenheitToCelsius("f-to-c"),
        };

        /// <summary>The conversions, in the order the screen lays them out.</summary>
        public static IReadOnlyList<IUnitConversion> All => Conversions;

        /// <summary>
        /// The conversion a button asks for, or null when no conversion
        /// carries that name.
        /// </summary>
        public static IUnitConversion? Find(string? key)
        {
            return Conversions.FirstOrDefault(c => string.Equals(c.Key, key, StringComparison.Ordinal));
        }
    }
}
