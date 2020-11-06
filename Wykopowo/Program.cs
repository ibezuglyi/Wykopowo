using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
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
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "subscriptions.db");
            var token = Environment.GetEnvironmentVariable("tele_token", EnvironmentVariableTarget.User);
            bootstrapper = new Bootstrapper();
            bootstrapper.Initialize(file, token);
            //var timer = new Timer(1000 * 60 * 60 * 1.5d);
            //timer.Elapsed += TimerOnElapsed;
            //timer.Start();
            TimerOnElapsed(null, null);
            Console.ReadKey();
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