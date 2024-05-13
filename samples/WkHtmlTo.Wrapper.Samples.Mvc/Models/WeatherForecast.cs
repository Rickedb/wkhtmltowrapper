namespace WkHtmlTo.Wrapper.Samples.Mvc.Models
{
    public class WeatherForecast
    {
        private static Lazy<IEnumerable<WeatherForecast>> _forecasts = new Lazy<IEnumerable<WeatherForecast>>(GetRandomForecast);
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public static IEnumerable<WeatherForecast> Forecasts => _forecasts.Value;

        private static IEnumerable<WeatherForecast> GetRandomForecast()
        {
            var startDate = DateTime.Now;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}
