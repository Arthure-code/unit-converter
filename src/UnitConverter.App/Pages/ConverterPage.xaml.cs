using System.Globalization;
using UnitConverter.Core;
using UnitConverter.Core.Conversions;

namespace UnitConverter.App.Pages
{
    public partial class ConverterPage : ContentPage
    {
        // Four decimals read well and say the truth: 1 inch is 2.54 cm, and
        // 1 cm is 0.3937 inch rather than 0.39.
        private const string ResultFormat = "0.####";

        private readonly NumberEntry _entry = new NumberEntry();

        public ConverterPage()
        {
            InitializeComponent();
        }

        // Every digit key calls this one method and says which digit it is
        // through its CommandParameter, so the key never has to be told
        // apart by the text it displays.
        private void Digit_Clicked(object sender, EventArgs e)
        {
            string? digit = Parameter(sender);

            if (digit?.Length == 1)
            {
                _entry.Append(digit[0]);
                ShowEntry();
            }
        }

        private void DecimalPoint_Clicked(object sender, EventArgs e)
        {
            _entry.AppendDecimalSeparator();
            ShowEntry();
        }

        private void Sign_Clicked(object sender, EventArgs e)
        {
            _entry.ToggleSign();
            ShowEntry();
        }

        private void Backspace_Clicked(object sender, EventArgs e)
        {
            _entry.RemoveLast();
            ShowEntry();
        }

        private void Clear_Clicked(object sender, EventArgs e)
        {
            _entry.Clear();

            SourceUnit.Text = string.Empty;
            SourceValue.Text = string.Empty;
            TargetUnit.Text = string.Empty;
            TargetValue.Text = string.Empty;

            SetKeysEnabled(true);
        }

        // The twelve conversion keys also share one method. Which conversion
        // to apply is a name the key carries, and the catalogue owns the
        // arithmetic.
        private async void Convert_Clicked(object sender, EventArgs e)
        {
            IUnitConversion? conversion = ConversionCatalog.Find(Parameter(sender));

            if (conversion == null)
            {
                return;
            }

            if (!_entry.TryGetValue(out double value))
            {
                await DisplayAlert("Nothing to convert", "Type a value before converting.", "OK");
                return;
            }

            SourceUnit.Text = conversion.From;
            SourceValue.Text = value.ToString(ResultFormat, CultureInfo.InvariantCulture);
            TargetUnit.Text = conversion.To;
            TargetValue.Text = conversion.Convert(value).ToString(ResultFormat, CultureInfo.InvariantCulture);

            // The result stays on screen until CLEAR: the keys go quiet so
            // that what is read belongs to the number that was typed.
            SetKeysEnabled(false);
        }

        private static string? Parameter(object sender)
        {
            return (sender as Button)?.CommandParameter as string;
        }

        private void ShowEntry()
        {
            SourceValue.Text = _entry.Text;
        }

        private void SetKeysEnabled(bool enabled)
        {
            foreach (IView key in Keypad.Children.Concat(Conversions.Children))
            {
                if (key is Button button)
                {
                    button.IsEnabled = enabled;
                }
            }

            BackspaceKey.IsEnabled = enabled;
        }
    }
}
