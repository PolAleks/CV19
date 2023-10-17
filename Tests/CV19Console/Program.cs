using System.Globalization;

internal class Program
{
    private const string dataUrl = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

    /// <summary>
    /// Метод получение потока из Http запроса
    /// </summary>
    private static async Task<Stream> GetDataStream()
    {
        var client = new HttpClient();
        var response = await client.GetAsync(dataUrl, HttpCompletionOption.ResponseHeadersRead);
        return await response.Content.ReadAsStreamAsync();
    }

    /// <summary>
    /// Метод получение итератора для построчного чтения потока 
    /// </summary>
    private static IEnumerable<string> GetDataLines()
    {
        // Захватываем поток из метода GetDataStream
        using var dataStream = GetDataStream().Result;
        using var dataReader = new StreamReader(dataStream);

        while (!dataReader.EndOfStream)
        {
            var line = dataReader.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;
            yield return line;
        }
    }

    /// <summary>
    /// Считываем все даты из первой строки
    /// </summary>
    private static DateTime[] GetDates() => GetDataLines()
        .First()
        .Split(',')
        .Skip(4)
        .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
        .ToArray();




    static void Main(string[] args)
    {
        //var client = new HttpClient();

        //var response = client.GetAsync(dataUrl).Result;

        //var csvData = response.Content.ReadAsStringAsync().Result;

        //Console.WriteLine("Hello, World!");

        //foreach (var line in GetDataLines())
        //{
        //    Console.WriteLine(line);
        //}

        var date = GetDates();
        Console.WriteLine(string.Join("\r\n", date));
    }
}