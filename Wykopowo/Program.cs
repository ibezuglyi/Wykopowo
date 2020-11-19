using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Wykopowo.Implementations;


namespace Wykopowo
{
    static class Program
    {
        private static Bootstrapper bootstrapper;

        static void Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("TELE_TOKEN", EnvironmentVariableTarget.Process);
            var connectionString = Environment.GetEnvironmentVariable("WYKOPOWO_CS", EnvironmentVariableTarget.Process);
            AssertVariables(token, connectionString);
            bootstrapper = new Bootstrapper();
            bootstrapper.Initialize(connectionString, token);
            var timer = new Timer(1000 * 60 * 15);
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
            TimerOnElapsed(null, null);
            Console.ReadKey();
        }

        private static void AssertVariables(string token, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("token is not set");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString is not set.");
            }
        }

        private static void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                var supplier = new ScratchSupplier("https://wykop.pl");
                var htmlPage = await supplier.GetPage();
                var extractor = new WykopTitlesExtractor();
                var headers = extractor.ExtractNews(htmlPage);
                bootstrapper.PublishNews(headers);
            }).GetAwaiter().GetResult();
        }
    }

    public class ScratchSupplier
    {
        private readonly string _url;
        private HttpClient _client;

        public ScratchSupplier(string url)
        {
            _url = url;
            _client = new HttpClient();
        }

        public async Task<string> GetPage()
        {
            return await _client.GetStringAsync(_url);
        }
    }
}