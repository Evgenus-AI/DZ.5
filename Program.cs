class Fail
{
    static int[,] MultiplyMatrices(int[,] a, int[,] b) // упр 6.2
    {
        int rowsA = a.GetLength(0); // Количество строк 
        int colsA = a.GetLength(1); // Количество столбцов 
        int rowsB = b.GetLength(0);
        int colsB = b.GetLength(1);
        int[,] resultData = new int[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                for (int k = 0; k < colsA; k++) // k пробегает по столбцам a и строкам b).
                {
                    resultData[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return resultData;
    }

    static void PrintMatrix(int[,] matrix) // упр 6.2 // принимает матрицу и выводит её
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }

            Console.WriteLine();
        }
    }

    static Dictionary<string, int[]> GenerateTemperatures()
    {
        Dictionary<string, int[]> temperatures = new Dictionary<string, int[]>
        {
            { "Январь", new int[30] },
            { "Февраль", new int[30] },
            { "Март", new int[30] },
            { "Апрель", new int[30] },
            { "Май", new int[30] },
            { "Июнь", new int[30] },
            { "Июль", new int[30] },
            { "Август", new int[30] },
            { "Сентябрь", new int[30] },
            { "Октябрь", new int[30] },
            { "Ноябрь", new int[30] },
            { "Декабрь", new int[30] }
        };

        Random random = new Random();
        foreach (string month in temperatures.Keys) //.Keys свойство класса , возвращает коллекцию
        {
            for (int day = 0; day < 30; day++)
            {
                temperatures[month][day] = random.Next(-10, 36);
            }
        }

        return temperatures;
    }
    static double[] AverageTemperatures(int[,] temperatures) // упр 6.3
    {
        double[] averages = new double[12]; //для хранения средних температур каждого месяца
        for (int month = 0; month < 12; month++)
        {
            double sum = 0;
            for (int day = 0; day < 30; day++)
            {
                sum += temperatures[month, day];
            }

            averages[month] = sum / 30; // сред.температура
        }

        return averages;
    }


    static LinkedList<LinkedList<int>>
        MultiplyMatrices(LinkedList<LinkedList<int>> a, LinkedList<LinkedList<int>> b) //дз 6.2
    {
        int rowsA = a.Count;
        int colsA = a.First.Value.Count;
        int rowsB = b.Count;
        int colsB = b.First.Value.Count;

        LinkedList<LinkedList<int>> resulttData = new LinkedList<LinkedList<int>>();
        for (int i = 0; i < rowsA; i++)
        {
            LinkedList<int> resultRow = new LinkedList<int>();
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                IEnumerator<int> rowAE = a.ElementAt(i).GetEnumerator();
                IEnumerator<int> colBE = GetColumn(b, j).GetEnumerator(); // Получаем столбец из матрицы B
                for (int k = 0; k < colsA; k++)
                {
                    rowAE.MoveNext();
                    colBE.MoveNext();
                    sum += rowAE.Current * colBE.Current;
                }

                resultRow.AddLast(sum);
            }

            resulttData.AddLast(resultRow);
        }

        return resulttData;
    }

    static LinkedList<int> GetColumn(LinkedList<LinkedList<int>> matrix, int columnIndex) //дз 6.2
    {
        //На вход получает матрицу и индекс столбца
        LinkedList<int> column = new LinkedList<int>();
        foreach (var row in matrix)
        {
            column.AddLast(row.ElementAt(columnIndex)); //связный список column, куда помещает элементы 
        }

        return column; //хранит элементы одной строки результат матрицы
    }

    static void PrintMatrix(LinkedList<LinkedList<int>> matrix) //дз 6.2
    {
        foreach (var row in matrix)
        {
            foreach (var element in row)
            {
                Console.Write(element + "\t");
            }

            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        string filePath = "..//..//..//Files//Letters.txt";
        var readedText = File.ReadAllText(filePath);
        char[] chars = readedText.ToCharArray();
    
        
        Console.WriteLine("     Упражнение 6_1");
        int countVowels = 0;
        int countConsonants = 0;

        string vowelsList = "аеёиоуыэюяАЕЁИОУЫЭЮЯaeiouAEIOU";

        for (int i = 0; i < chars.Length; i++)
        {
            char currentChar = chars[i];

            if (char.IsLetter(currentChar))
            {
                if (vowelsList.IndexOf(currentChar) >= 0)
                {
                    countVowels++;
                }
                else
                {
                    countConsonants++;
                }
            }
        }

        Console.WriteLine($"Гласных букв: {countVowels}");
        Console.WriteLine($"Согласных букв: {countConsonants}");

        Console.WriteLine(String.Empty);
        Console.WriteLine("     Упражнение 6_2");
            int[,] matrixA =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            int[,] matrixB =
            {
                { 7, 8 },
                { 9, 10 },
                { 11, 12 }
            };

            int[,]
                resultMatrix =
                    MultiplyMatrices(matrixA,
                        matrixB); //матрица resultData размером rowsA x colsB для хранения резл умножения
            PrintMatrix(resultMatrix);
            
            Console.WriteLine(String.Empty);
            Console.WriteLine("     Упражнение 6_3");
            int[,] temperature = new int[12, 30];
            Random random = new Random();
            for (int month = 0; month < 12; month++)
            {
                for (int day = 0; day < 30; day++)
                {
                    temperature[month, day] = random.Next(-10, 36);
                }
            }

            double[] averageTemp = AverageTemperatures(temperature); //средняя температура месяца
            Array.Sort(averageTemp); // сортирует полученный массив средних температур по возрастанию 
            Console.WriteLine("Средние температуры по месяцам:");
            foreach (var avgTemp in averageTemp)
            {
                Console.WriteLine(avgTemp);
            }
        }
}











