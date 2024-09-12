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
    }
}