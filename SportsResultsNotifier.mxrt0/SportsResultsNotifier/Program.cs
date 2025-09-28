namespace SportsResultsNotifier;

public class Program
{
    static async Task Main()
    {
        try
        {
            using var cts = new CancellationTokenSource();
            Console.WriteLine("Service is running.");

            var scraper = new WebScraper(cts);
            scraper.ScrapeToEmail();

            var cancellationListener = Task.Run(() =>
            {
                Console.WriteLine("Press 's' at any time to stop service.");
                while (!cts.IsCancellationRequested)
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == 's')
                    {
                        Console.WriteLine("Terminating...");
                        cts.Cancel();
                        break;
                    }
                }
            });

            await cancellationListener;

            Console.WriteLine("Service terminated!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
