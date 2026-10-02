using UnitConverter.Core.Conversions;

namespace UnitConverter.App.Pages
{
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();

            ConversionList.ItemsSource = ConversionCatalog.All
                .Select(c => new HelpLine(Title(c.Key), c.Describe()))
                .ToList();
        }

        // The key a button carries, read out loud: cm-to-in becomes CM TO IN,
        // which is exactly what the button shows.
        private static string Title(string key)
        {
            return key.Replace('-', ' ').ToUpperInvariant();
        }

        private sealed record HelpLine(string Title, string Description);
    }
}
