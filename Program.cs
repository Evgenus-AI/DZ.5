class Program
{
    static void Main()
    {
        var a = FromArray(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        var b = FromArray(new int[,] { { 7, 8 }, { 9, 10 }, { 11, 12 } });

        Print(Multiply(a, b));
    }

    // Вспомогательный метод, чтобы не заполнять списки вручную
    static LinkedList<LinkedList<int>> FromArray(int[,] arr)
    {
        var matrix = new LinkedList<LinkedList<int>>();
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            var row = new LinkedList<int>();
            for (int j = 0; j < arr.GetLength(1); j++)
                row.AddLast(arr[i, j]);
            matrix.AddLast(row);
        }
        return matrix;
    }

    static void Print(LinkedList<LinkedList<int>> m)
    {
        foreach (var row in m)
        {
            foreach (int value in row)
                Console.Write($"{value,5}");
            Console.WriteLine();
        }
    }

    static LinkedList<LinkedList<int>> Transpose(LinkedList<LinkedList<int>> m)
    {
        var result = new LinkedList<LinkedList<int>>();

        // m.First — это LinkedListNode<...>? (может быть null, если список пуст).
        // Восклицательный знак ! говорит компилятору: "я знаю, что тут не null"
        int cols = m.First!.Value.Count;
        for (int j = 0; j < cols; j++)
            result.AddLast(new LinkedList<int>()); // пустые будущие строки

        foreach (var row in m)
        {
            var target = result.First; // идём по новым строкам параллельно с элементами текущей
            foreach (int value in row)
            {
                target!.Value.AddLast(value);
                target = target.Next;
            }
        }
        return result;
    }

    static LinkedList<LinkedList<int>> Multiply(LinkedList<LinkedList<int>> a, LinkedList<LinkedList<int>> b)
    {
        if (a.First!.Value.Count != b.Count)
            throw new ArgumentException("Число столбцов A должно равняться числу строк B");

        var bColumns = Transpose(b);
        var result = new LinkedList<LinkedList<int>>();

        foreach (var rowA in a)
        {
            var newRow = new LinkedList<int>();
            foreach (var colB in bColumns)
            {
                int sum = 0;
                var nodeA = rowA.First;
                var nodeB = colB.First;
                // Идём по двум спискам одновременно, пока оба не закончатся
                while (nodeA != null && nodeB != null)
                {
                    sum += nodeA.Value * nodeB.Value;
                    nodeA = nodeA.Next;
                    nodeB = nodeB.Next;
                }
                newRow.AddLast(sum);
            }
            result.AddLast(newRow);
        }
        return result;
    }
}