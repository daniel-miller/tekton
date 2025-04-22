namespace Tek.Terminal;

public class WeatherResponse
{
    public int QueryCost { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ResolvedAddress { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Timezone { get; set; } = null!;
    public double Tzoffset { get; set; }
    public string Description { get; set; } = null!;
    public List<DayData> Days { get; set; } = null!;
    public CurrentConditions CurrentConditions { get; set; } = null!;
}

public class DayData
{
    public string Datetime { get; set; } = null!;
    public long DatetimeEpoch { get; set; }
    public double TempMax { get; set; }
    public double TempMin { get; set; }
    public double Temp { get; set; }
    public double FeelslikeMax { get; set; }
    public double FeelslikeMin { get; set; }
    public double Feelslike { get; set; }
    public double Dew { get; set; }
    public double Humidity { get; set; }
    public double? Precip { get; set; }
    public double? PrecipProb { get; set; }
    public double? PrecipCover { get; set; }
    public List<string>? PrecipType { get; set; }
    public double? Snow { get; set; }
    public double? SnowDepth { get; set; }
    public double WindGust { get; set; }
    public double WindSpeed { get; set; }
    public double WindDir { get; set; }
    public double Pressure { get; set; }
    public double CloudCover { get; set; }
    public double Visibility { get; set; }
    public double SolarRadiation { get; set; }
    public double SolarEnergy { get; set; }
    public double UVIndex { get; set; }
    public double SevereRisk { get; set; }
    public string Sunrise { get; set; } = null!;
    public long SunriseEpoch { get; set; }
    public string Sunset { get; set; } = null!;
    public long SunsetEpoch { get; set; }
    public double MoonPhase { get; set; }
    public string Conditions { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public List<string> Stations { get; set; } = null!;
    public string Source { get; set; } = null!;
    public List<HourData> Hours { get; set; } = null!;
}

public class HourData
{
    public string Datetime { get; set; } = null!;
    public long DatetimeEpoch { get; set; }
    public double Temp { get; set; }
    public double Feelslike { get; set; }
    public double Humidity { get; set; }
    public double Dew { get; set; }
    public double? Precip { get; set; }
    public double? PrecipProb { get; set; }
    public double? Snow { get; set; }
    public double? SnowDepth { get; set; }
    public List<string>? PrecipType { get; set; }
    public double WindGust { get; set; }
    public double WindSpeed { get; set; }
    public double WindDir { get; set; }
    public double Pressure { get; set; }
    public double Visibility { get; set; }
    public double CloudCover { get; set; }
    public double SolarRadiation { get; set; }
    public double SolarEnergy { get; set; }
    public double UVIndex { get; set; }
    public double SevereRisk { get; set; }
    public string Conditions { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public List<string> Stations { get; set; } = null!;
    public string Source { get; set; } = null!;
}

public class CurrentConditions
{
    public string Datetime { get; set; } = null!;
    public long DatetimeEpoch { get; set; }
    public double Temp { get; set; }
    public double Feelslike { get; set; }
    public double Humidity { get; set; }
    public double Dew { get; set; }
    public double? Precip { get; set; }
    public double? PrecipProb { get; set; }
    public double? Snow { get; set; }
    public double? SnowDepth { get; set; }
    public List<string>? PrecipType { get; set; }
    public double WindGust { get; set; }
    public double WindSpeed { get; set; }
    public double WindDir { get; set; }
    public double Pressure { get; set; }
    public double Visibility { get; set; }
    public double CloudCover { get; set; }
    public double SolarRadiation { get; set; }
    public double SolarEnergy { get; set; }
    public double UVIndex { get; set; }
    public string Conditions { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public List<string> Stations { get; set; } = null!;
    public string Source { get; set; } = null!;
    public string Sunrise { get; set; } = null!;
    public long SunriseEpoch { get; set; }
    public string Sunset { get; set; } = null!;
    public long SunsetEpoch { get; set; }
    public double MoonPhase { get; set; }

    public string DescribeMoonPhase()
    {
        return MoonPhase switch
        {
            0 => "New Moon",
            > 0 and < 0.25 => "Waxing Crescent",
            0.25 => "First Quarter",
            > 0.25 and < 0.5 => "Waxing Gibbous",
            0.5 => "Full Moon",
            > 0.5 and < 0.75 => "Waning Gibbous",
            0.75 => "Last Quarter",
            > 0.75 and <= 1 => "Waning Crescent",
            _ => "Unknown"
        };
    }
}