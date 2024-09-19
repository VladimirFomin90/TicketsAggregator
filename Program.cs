using System.Globalization;
using System.Text;
using UglyToad.PdfPig;

const string TicketsFolder = @"/home/vladimir/RiderProjects/TicketsAggregator/src/Tickets";

try
{
    var tickertsAggregator = new TicketsAggregator(TicketsFolder);

    tickertsAggregator.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Exception message" + ex.Message);
}

Console.WriteLine("Press any key to close.");
Console.ReadKey();

public class TicketsAggregator
{

    private readonly Dictionary<string, string> _domainCultureInfo = new()
    {
        [".com"] = "en-US",
        [".fr"] = "fr-FR",
        [".jp"] = "ja-JP"
    };

    private readonly string _ticketsFolder;

    public TicketsAggregator(string ticketsFolder)
    {
        _ticketsFolder = ticketsFolder;
    }

    public void Run()
    {
        var stringBuilder = new StringBuilder();

        foreach (var filepath in Directory.GetFiles(_ticketsFolder, "*.pdf"))
        {
            using var document = PdfDocument.Open(filepath);

            // Page number starts from 1, not 0.
            var page = document.GetPage(1);
            var text = page.Text;
            var splitString = text.Split(new[] { "Title:", "PMTitle:", "Date:", "Time:", "Visit us:" },
                StringSplitOptions.None);

            var domain = ExtractDomain(splitString.Last());
            var tickerCulture = _domainCultureInfo[domain];

            for (var i = 1; i < splitString.Length - 3; i += 3)
            {
                var title = splitString[i];
                var dateAsString = splitString[i + 1];
                var timeAsString = splitString[i + 2];

                var date = DateOnly.Parse(dateAsString, new CultureInfo(tickerCulture));
                var time = TimeOnly.Parse(timeAsString, new CultureInfo(tickerCulture));

                var dateAsStringInvariant = date.ToString(CultureInfo.InvariantCulture);
                var timeAsStringInvariant = time.ToString(CultureInfo.InvariantCulture);

                var ticketData =
                    $"{title,-30}|{dateAsStringInvariant}|{timeAsStringInvariant}";

                stringBuilder.AppendLine(ticketData);
            }
        }

        var resultPathToFile = Path.Combine(_ticketsFolder, "aggregatedTickets.txt");
        File.WriteAllText(resultPathToFile, stringBuilder.ToString());
        Console.WriteLine("Tickets saved to " + resultPathToFile);
    }

    private static string ExtractDomain(string webAddress)
    {
        var lastDotIndex = webAddress.LastIndexOf('.');
        return webAddress.Substring(lastDotIndex);
    }
}