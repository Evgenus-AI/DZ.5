class Program
{
    static void Main()
    {
        string[] monthNames =
        {
            "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
            "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        };

        // Ключ — название месяца, значение — массив из 30 температур
        var temperature = new Dictionary<string, int[]>();
        foreach (string month in monthNames)
        {
            var days = new int[30];
            for (int d = 0; d < days.Length; d++)
                days[d] = Random.Shared.Next(-20, 36);
            temperature[month] = days; 
        }

        Dictionary<string, double> averages = AverageByMonth(temperature);
        
        var sorted = averages.OrderBy(p => p.Value);

        foreach (var pair in sorted) // каждая pair — KeyValuePair со свойствами Key и Value
            Console.WriteLine($"{pair.Key,-10} {pair.Value,6:F1}");
    }

    static Dictionary<string, double> AverageByMonth(Dictionary<string, int[]> temps)
    {
        var result = new Dictionary<string, double>();
        foreach (var pair in temps)
            result[pair.Key] = pair.Value.Average(); // Average() — тоже LINQ, для int[] вернёт double
        return result;
    }
}