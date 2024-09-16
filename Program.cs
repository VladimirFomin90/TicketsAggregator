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
    private readonly string _ticketsFolder;

    public TicketsAggregator(string ticketsFolder)
    {
        _ticketsFolder = ticketsFolder;
    }

    public void Run()
    {
        foreach (var filepath in Directory.GetFiles(_ticketsFolder, "*.pdf"))
        {
            using var document = PdfDocument.Open(filepath);

            // Page number starts from 1, not 0.
            var page = document.GetPage(1);
            var text = page.Text;
            var splitString = text.Split(new[] { "Title:", "PMTitle:", "Date:", "Time:", "Visit us:" },
                StringSplitOptions.None);

            for (var i = 1; i < splitString.Length - 3; i += 3)
            {
                var title = splitString[i];
                var date = splitString[i + 1];
                var time = splitString[i + 2];
            }
        }
    }
}