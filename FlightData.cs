namespace KneeboardApp.Models;

/// <summary>
/// Holds everything on one kneeboard card: identifiers you need in flight
/// (callsign, squawk, SELCAL) plus the procedures for this leg (SID/STAR)
/// and free-form scratchpad notes (ATIS, clearance, etc).
/// </summary>
public class FlightData
{
    public string Callsign { get; set; } = "";
    public string DepartureAirport { get; set; } = "";
    public string ArrivalAirport { get; set; } = "";

    /// <summary>Standard Instrument Departure procedure, e.g. "DEEZZ5".</summary>
    public string Sid { get; set; } = "";

    /// <summary>Standard Terminal Arrival Route, e.g. "ANJLL2".</summary>
    public string Star { get; set; } = "";

    /// <summary>4-digit transponder code, e.g. "4721".</summary>
    public string SquawkCode { get; set; } = "";

    /// <summary>Selective calling code, e.g. "AB-CD".</summary>
    public string SelcalCode { get; set; } = "";

    public string Notes { get; set; } = "";
}
