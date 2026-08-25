using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using KneeboardApp.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace KneeboardApp;

/// <summary>
/// Single-window kneeboard: a data panel (callsign, squawk, SELCAL, SID/STAR)
/// plus a free-form notes pad. Cards can be saved to / loaded from a JSON
/// file so you can prep them before a flight and reload mid-session.
/// </summary>
public sealed partial class MainWindow : Window
{
    // Squawk: 4 octal digits (0-7).
    private static readonly Regex SquawkRegex = new(@"^[0-7]{4}$", RegexOptions.Compiled);

    // SELCAL: 4 letters from the A-S alphabet used for SELCAL, optionally
    // written as two pairs separated by a hyphen (e.g. "ABCD" or "AB-CD").
    private static readonly Regex SelcalRegex = new(@"^[A-S]{2}-?[A-S]{2}$", RegexOptions.Compiled);

    private static readonly SolidColorBrush ErrorBrush = new(Microsoft.UI.ColorHelper.FromArgb(255, 229, 83, 61));

    private readonly Brush? _defaultSquawkBrush;
    private readonly Brush? _defaultSelcalBrush;

    public MainWindow()
    {
        this.InitializeComponent();
        Title = "Fridgeboard";

        // Sets the icon shown in the window's title bar and Alt+Tab preview.
        // (The taskbar/File Explorer icon comes from <ApplicationIcon> in the
        // .csproj, baked into the .exe — this is the separate runtime icon.)
        AppWindow.SetIcon("Assets\\AppIcon.ico");

        // Remember the default TextBox border so validation can restore it.
        _defaultSquawkBrush = SquawkBox.BorderBrush;
        _defaultSelcalBrush = SelcalBox.BorderBrush;
    }

    private FlightData CollectData() => new()
    {
        Callsign = CallsignBox.Text.Trim(),
        DepartureAirport = DepartureAirportBox.Text.Trim(),
        ArrivalAirport = ArrivalAirportBox.Text.Trim(),
        Sid = SidBox.Text.Trim(),
        Star = StarBox.Text.Trim(),
        SquawkCode = SquawkBox.Text.Trim(),
        SelcalCode = SelcalBox.Text.Trim(),
        Notes = NotesBox.Text
    };

    private void ApplyData(FlightData data)
    {
        CallsignBox.Text = data.Callsign;
        DepartureAirportBox.Text = data.DepartureAirport;
        ArrivalAirportBox.Text = data.ArrivalAirport;
        SidBox.Text = data.Sid;
        StarBox.Text = data.Star;
        SquawkBox.Text = data.SquawkCode;
        SelcalBox.Text = data.SelcalCode;
        NotesBox.Text = data.Notes;

        SquawkError.Visibility = Visibility.Collapsed;
        SelcalError.Visibility = Visibility.Collapsed;
        SquawkBox.BorderBrush = _defaultSquawkBrush;
        SelcalBox.BorderBrush = _defaultSelcalBrush;
    }

    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyData(new FlightData());
        StatusText.Text = "New card started.";
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileSavePicker();
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        picker.FileTypeChoices.Add("Kneeboard Card (JSON)", new List<string> { ".json" });
        picker.SuggestedFileName = string.IsNullOrWhiteSpace(CallsignBox.Text)
            ? "flight_card"
            : CallsignBox.Text.Replace(" ", "_");

        StorageFile? file = await picker.PickSaveFileAsync();
        if (file is null) return;

        var json = JsonSerializer.Serialize(CollectData(), new JsonSerializerOptions { WriteIndented = true });
        await FileIO.WriteTextAsync(file, json);

        StatusText.Text = $"Saved to {file.Name}";
    }

    private async void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        picker.FileTypeFilter.Add(".json");

        StorageFile? file = await picker.PickSingleFileAsync();
        if (file is null) return;

        try
        {
            var json = await FileIO.ReadTextAsync(file);
            var data = JsonSerializer.Deserialize<FlightData>(json);
            if (data is not null)
            {
                ApplyData(data);
                StatusText.Text = $"Loaded {file.Name}";
            }
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Failed to load: {ex.Message}";
        }
    }

    private void SquawkBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var text = SquawkBox.Text.Trim();
        bool valid = text.Length == 0 || SquawkRegex.IsMatch(text);
        SquawkError.Visibility = valid ? Visibility.Collapsed : Visibility.Visible;
        SquawkBox.BorderBrush = valid ? _defaultSquawkBrush : ErrorBrush;
    }

    private void SelcalBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var text = SelcalBox.Text.Trim();
        bool valid = text.Length == 0 || SelcalRegex.IsMatch(text);
        SelcalError.Visibility = valid ? Visibility.Collapsed : Visibility.Visible;
        SelcalBox.BorderBrush = valid ? _defaultSelcalBrush : ErrorBrush;
    }
}
