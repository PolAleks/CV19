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
            yield return line.Replace("Korea,", "Korea -").Replace("Bonaire,", "\"Bonaire ");
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

    /// <summary>
    /// Метод возвращающий кортеж содержащий Страну, провинцию и массив кол-во зараженных по дням
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<(string Contry, string Province, int[] Counts)> GetData()
    {
        var lines = GetDataLines()
           .Skip(1)
           .Select(line => line.Split(','));

        foreach (var row in lines)
        {
            var province = row[0].Trim();
            var country_name = row[1].Trim(' ', '"');
            var counts = row.Skip(4).Select(int.Parse).ToArray();

            yield return (country_name, province, counts);
        }
    }



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

        //var date = GetDates();
        //Console.WriteLine(string.Join("\r\n", date));

        var russia_data = GetData()
               .First(v => v.Contry.Equals("Russia", StringComparison.OrdinalIgnoreCase));

        Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (date, count) => $"{date:dd:MM} - {count}")));
    }
}