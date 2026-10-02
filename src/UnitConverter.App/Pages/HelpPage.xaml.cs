using UnitConverter.Core.Conversions;

namespace UnitConverter.App.Pages
{
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();

            ConversionList.ItemsSource = ConversionCatalog.All
                .Select(c => new HelpLine($"{c.From} vers {c.To}", c.Describe()))
                .ToList();
        }

        private sealed record HelpLine(string Title, string Description);
    }
}
