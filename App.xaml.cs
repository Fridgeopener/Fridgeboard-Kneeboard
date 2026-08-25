using Microsoft.UI.Xaml;

namespace KneeboardApp;

/// <summary>
/// Application entry point. Creates and activates the single MainWindow
/// that hosts the kneeboard UI.
/// </summary>
public partial class App : Application
{
    private Window? _window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}
