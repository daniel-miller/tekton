namespace Atom.Terminal;

public class MoonPhaseImageRequest
{
    public string format { get; set; }
    public MoonPhaseImageStyle style { get; set; }
    public MoonPhaseObserver observer { get; set; }
    public MoonPhaseView view { get; set; }

    public MoonPhaseImageRequest()
    {
        format = "png";

        style = new MoonPhaseImageStyle();
        style.moonStyle = "default";     // default | shaded | sketch
        style.backgroundStyle = "stars"; // stars | solid
        style.backgroundColor = "black"; // https://www.w3schools.com/tags/ref_colornames.asp
        style.headingColor = "white";
        style.textColor = "white";

        observer = new MoonPhaseObserver();
        observer.latitude = 51.1917;
        observer.longitude = 114.4668;
        observer.date = $"{DateTime.Today:yyyy-MM-dd}";

        view = new MoonPhaseView();
        view.type = "portrait-simple"; // portrait-simple | landscape-simple
        view.orientation = "north-up"; // north-up | south-up
    }
}

public class MoonPhaseImageStyle
{
    public string moonStyle { get; set; } = null!;
    public string backgroundStyle { get; set; } = null!;
    public string backgroundColor { get; set; } = null!;
    public string headingColor { get; set; } = null!;
    public string textColor { get; set; } = null!;
}

public class MoonPhaseObserver
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string date { get; set; } = null!;
}

public class MoonPhaseView
{
    public string type { get; set; } = null!;
    public string orientation { get; set; } = null!;
}

public class MoonPhaseImageResponse
{
    public MoonPhaseImageData data { get; set; } = null!;
}

public class MoonPhaseImageData
{
    public string imageUrl { get; set; } = null!;
}
