namespace UnitConverter.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        // The window carries the shell. Setting MainPage did the same thing
        // and is deprecated since .NET 9.
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
