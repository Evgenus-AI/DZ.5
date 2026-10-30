using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Failik
{
    internal class fail
    {
        static void Shuffle(List<string> list) //#1  (без изменений, он верный)
        {
            Random range = new Random();
            int n = list.Count;
            while (n > 1)
            {
                int k = range.Next(n--);
                var value = list[n];
                list[n] = list[k];
                list[k] = value;
            }
        }

        // НОВОЕ: маленький класс студента вместо массива строк string[,]
        // Год и баллы - int, чтобы сортировать по баллам без TryParse
        class Student
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public int BirthYear { get; set; }
            public string Exam { get; set; }
            public int Score { get; set; }

            public override string ToString() =>
                $"{LastName} {FirstName}, Год рождения: {BirthYear}, Экзамен: {Exam}, Баллы: {Score}";
        }

        // НОВОЕ: ключ словаря "фамилия имя" в нижнем регистре, чтобы "Иванов Иван" и "иванов иван" были одним студентом
        static string MakeKey(string lastName, string firstName) =>
            (lastName.Trim() + " " + firstName.Trim()).ToLower();

        // НОВОЕ: чтение студентов из файла. Формат строки: Фамилия;Имя;Год;Экзамен;Баллы
        static Dictionary<string, Student> LoadStudents(string filePath)
        {
            var result = new Dictionary<string, Student>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден, словарь пустой.");
                return result;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split(';');
                // строки в неправильном формате просто пропускаем
                if (parts.Length != 5 || !int.TryParse(parts[2], out int year) ||
                    !int.TryParse(parts[4], out int score))
                {
                    continue;
                }

                var student = new Student
                {
                    LastName = parts[0].Trim(),
                    FirstName = parts[1].Trim(),
                    BirthYear = year,
                    Exam = parts[3].Trim(),
                    Score = score
                };
                result[MakeKey(student.LastName, student.FirstName)] = student;
            }

            return result;
        }

        // ИЗМЕНЕНО: теперь работает со словарём, а не с массивом
        static void AddNewStudent(Dictionary<string, Student> students) //СОЗДАНИЕ СТУДЕНТА
        {
            var student = new Student();
            Console.Write("Введите фамилию: ");
            student.LastName = Console.ReadLine();
            Console.Write("Введите имя: ");
            student.FirstName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            student.BirthYear = int.Parse(Console.ReadLine());

            Console.Write("Введите экзамен: ");
            student.Exam = Console.ReadLine();

            Console.Write("Введите баллы: ");
            student.Score = int.Parse(Console.ReadLine());

            // TryAdd вернёт false, если такой ключ уже есть
            if (students.TryAdd(MakeKey(student.LastName, student.FirstName), student))
                Console.WriteLine("Студент добавлен.");
            else
                Console.WriteLine("Такой студент уже есть.");
        }

        // ИЗМЕНЕНО: удаление из словаря по ключу, сдвигать ничего не надо
        static void RemoveStudent(Dictionary<string, Student> students) //УДАЛЕНИЕ СТУДЕНТА
        {
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            if (students.Remove(MakeKey(lastName, firstName)))
                Console.WriteLine("Студент удален.");
            else
                Console.WriteLine("Студент не найден.");
        }

        // ИЗМЕНЕНО: у словаря нет порядка, поэтому сортируем в List и выводим его
        static void SortStudents(Dictionary<string, Student> students) //СОРТИРОВКА СТУДЕНТОВ
        {
            List<Student> sorted = students.Values.OrderBy(s => s.Score).ToList(); // по возрастанию
            Console.WriteLine("Студенты отсортированы по баллам:");
            foreach (var student in sorted)
            {
                Console.WriteLine(student);
            }
        }

        static void DisplayStudents(Dictionary<string, Student> students) //СПИСОК СТУДЕНТОВ
        {
            Console.WriteLine("Список студентов:");
            foreach (var student in students.Values)
            {
                Console.WriteLine(student);
            }
        }

        // ИЗМЕНЕНО: был DFS (находит какой-то путь), теперь BFS (находит кратчайший)
        static List<int> ShortestPath(Dictionary<int, List<int>> graph, int start, int finish)
        {
            var queue = new Queue<int>();
            // parent[v] = откуда пришли в вершину v. Заодно работает как "посещено"
            var parent = new Dictionary<int, int>();
            parent[start] = -1; // у старта родителя нет
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == finish)
                {
                    // идём от финиша назад по родителям, потом разворачиваем
                    var path = new List<int>();
                    for (int v = finish; v != -1; v = parent[v])
                    {
                        path.Add(v);
                    }

                    path.Reverse();
                    return path;
                }

                foreach (var neighbor in graph[current])
                {
                    if (!parent.ContainsKey(neighbor)) // ещё не были в этой вершине
                    {
                        parent[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return null; // пути нет
        }

        class Granny
        {
            public string Name { get; set; }
            public int Age { get; set; }

            // ИЗМЕНЕНО: словарь "болезнь -> лекарство" вместо списка болезней.
            // OrdinalIgnoreCase: "Грипп" и "грипп" - одно и то же (чинит баг с регистром)
            public Dictionary<string, string> Medicines { get; set; } =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            public string Location { get; set; } = "на улице"; // НОВОЕ: куда попала
        }

        class Hospital
        {
            public string Name { get; set; }
            public int Capacity { get; set; }

            public HashSet<string> TreatableDiseases { get; set; } =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase); // ИЗМЕНЕНО: без учёта регистра

            // ИЗМЕНЕНО: список бабуль вместо счётчика, чтобы можно было их показать
            public List<Granny> Patients { get; set; } = new List<Granny>();

            public bool IsFull => Patients.Count >= Capacity;

            // НОВОЕ: процент заполненности (100.0, а не 100, чтобы деление было дробным)
            public double FillPercent => Patients.Count * 100.0 / Capacity;

            public bool CanTreat(Granny granny)
            {
                if (IsFull) return false;
                if (granny.Medicines.Count == 0) return true; // Если здорова - пускаем

                // Считаем пересечение болезней
                int matchCount = granny.Medicines.Keys.Count(d => TreatableDiseases.Contains(d));

                // Более 50%
                return ((double)matchCount / granny.Medicines.Count) > 0.5;
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // ИЗМЕНЕНО: перенесено в начало, до любого вывода

            // ---------------- Задание 1 ----------------
            Console.WriteLine("     Задание 1");
            // НОВОЕ: описание и что делать пользователю (Прим. 1)
            Console.WriteLine("Список из 64 картинок (32 пары) выводится до и после перемешивания.");
            Console.WriteLine("От вас ничего не требуется.");
            Console.WriteLine(string.Empty);

            string[] img = new string[]
            {
                "..//..//..//Pictures//1.jpg",
                "..//..//..//Pictures//2.jpg",
                "..//..//..//Pictures//3.jpg",
                "..//..//..//Pictures//4.jpg",
                "..//..//..//Pictures//5.jpg",
                "..//..//..//Pictures//6.jpg",
                "..//..//..//Pictures//7.jpg",
                "..//..//..//Pictures//8.jpg",
                "..//..//..//Pictures//9.jpg",
                "..//..//..//Pictures//10.jpg",
                "..//..//..//Pictures//11.jpg",
                "..//..//..//Pictures//12.jpg",
                "..//..//..//Pictures//13.jpg",
                "..//..//..//Pictures//14.jpg",
                "..//..//..//Pictures//15.jpg",
                "..//..//..//Pictures//16.jpg",
                "..//..//..//Pictures//17.jpg",
                "..//..//..//Pictures//18.jpg",
                "..//..//..//Pictures//19.jpg",
                "..//..//..//Pictures//20.jpg",
                "..//..//..//Pictures//21.jpg",
                "..//..//..//Pictures//22.jpg",
                "..//..//..//Pictures//23.jpg",
                "..//..//..//Pictures//24.jpg",
                "..//..//..//Pictures//25.jpg",
                "..//..//..//Pictures//26.jpg",
                "..//..//..//Pictures//27.jpg",
                "..//..//..//Pictures//28.jpg",
                "..//..//..//Pictures//29.jpg",
                "..//..//..//Pictures//30.jpg",
                "..//..//..//Pictures//31.jpg",
                "..//..//..//Pictures//32.jpg"
            };
            List<string> imageList = new List<string>(); // для пар 
            foreach (var url in img)
            {
                imageList.Add(url);
                imageList.Add(url);
            }

            // НОВОЕ: копия ДО перемешивания (Shuffle меняет список на месте)
            List<string> original = new List<string>(imageList);

            Console.WriteLine("Исходный список:");
            for (int i = 0; i < original.Count; i++)
            {
                Console.WriteLine($"{i}: {original[i]}");
            }

            Shuffle(imageList); // перемешиваем список
            Console.WriteLine(string.Empty);
            Console.WriteLine("Перемешанный список:");
            for (int i = 0; i < imageList.Count; i++)
            {
                Console.WriteLine($"{i}: {imageList[i]}");
            }


            // ---------------- Задание 2 (ИЗМЕНЕНО: теперь стоит вторым) ----------------
            Console.WriteLine(string.Empty);
            Console.WriteLine("     Задание 2");
            Console.WriteLine("Студенты хранятся в словаре и читаются из файла Students.txt.");
            Console.WriteLine("Вводите команды: Новый студент, Удалить, Сортировать, Показать, Выход.");
            Console.WriteLine(string.Empty);

            // Файл должен лежать рядом с .exe (bin/Debug/net8.0)
            Dictionary<string, Student> students = LoadStudents("Students.txt");
            Console.WriteLine($"Загружено студентов: {students.Count}");

            bool isContinue = true;
            while (isContinue)
            {
                Console.WriteLine("Меню: Новый студент | Удалить | Сортировать | Показать | Выход");
                Console.Write("Введите команду: ");

                // ИЗМЕНЕНО: текстовые команды вместо цифр; ToLower - чтобы регистр не мешал
                string choice = (Console.ReadLine() ?? "").Trim().ToLower();
                switch (choice)
                {
                    case "новый студент":
                    {
                        AddNewStudent(students);
                        break;
                    }
                    case "удалить":
                    {
                        RemoveStudent(students);
                        break;
                    }
                    case "сортировать":
                    {
                        SortStudents(students);
                        break;
                    }
                    case "показать":
                    {
                        DisplayStudents(students);
                        break;
                    }
                    case "выход":
                    {
                        // ИЗМЕНЕНО: был return (он завершал всю программу и задания 3 и 4 не запускались)
                        isContinue = false;
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Неизвестная команда");
                        break;
                    }
                }
            }


            // ---------------- Задание 3 ----------------
            Console.WriteLine(string.Empty);
            Console.WriteLine("     Задание 3");
            Console.WriteLine("Бабули (очередь) распределяются по больницам (стек): больница берёт бабулю,");
            Console.WriteLine("если лечит больше 50% её болезней и есть место.");
            Console.WriteLine("Введите больницы, затем бабуль. Пустая строка в болезнях = болезней больше нет.");
            Console.WriteLine(string.Empty);

            // 1. Настройка больниц (Стек)
            var hospitalsStack = new Stack<Hospital>();
            Console.Write("Сколько больниц создать? ");
            int hospitalsCount = int.Parse(Console.ReadLine());

            for (int i = 1; i <= hospitalsCount; i++)
            {
                var hospital = new Hospital();
                Console.WriteLine($"Больница #{i}");
                Console.Write("Название: ");
                hospital.Name = Console.ReadLine();
                Console.Write("Вместимость: ");
                hospital.Capacity = int.Parse(Console.ReadLine());

                Console.Write("Лечимые болезни (через запятую): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    foreach (var s in input.Split(','))
                        hospital.TreatableDiseases.Add(s.Trim()); // ToLower не нужен: регистр уже игнорируется
                }

                hospitalsStack.Push(hospital);
            }

            // 2. Настройка бабуль (Очередь)
            var grannyQueue = new Queue<Granny>();
            var allGrannies = new List<Granny>(); // НОВОЕ: чтобы в конце вывести всех бабуль
            Console.Write("Сколько бабуль добавить? ");
            int gCount = int.Parse(Console.ReadLine());

            for (int i = 1; i <= gCount; i++)
            {
                var g = new Granny();
                Console.WriteLine($" Бабуля #{i}");
                Console.Write("Имя: ");
                g.Name = Console.ReadLine();
                Console.Write("Возраст: ");
                g.Age = int.Parse(Console.ReadLine());

                // ИЗМЕНЕНО: вводим болезни по одной вместе с лекарством
                while (true)
                {
                    Console.Write("Болезнь (Enter - больше нет): ");
                    string disease = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(disease)) break;
                    Console.Write($"Лекарство от {disease.Trim()}: ");
                    g.Medicines[disease.Trim()] = Console.ReadLine();
                }

                grannyQueue.Enqueue(g);
                allGrannies.Add(g);
            }

            Console.WriteLine("     Кто, куда: ");
            var hospitalsArray = hospitalsStack.ToArray(); // Чтобы перебирать стек, не ломая его

            while (grannyQueue.TryDequeue(out Granny current))
            {
                bool placed = false;
                foreach (var hospital in hospitalsArray)
                {
                    if (hospital.CanTreat(current))
                    {
                        hospital.Patients.Add(current); // ИЗМЕНЕНО: добавляем бабулю, а не счётчик++
                        current.Location = hospital.Name;
                        Console.WriteLine($"{current.Name} -> {hospital.Name}");
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                    Console.WriteLine($"{current.Name} остаётся на улице плакать :(");
            } // ИЗМЕНЕНО: цикл закрывается ЗДЕСЬ, раньше задания 4 и 2 были внутри него

            // НОВОЕ: итоговый вывод
            Console.WriteLine(string.Empty);
            Console.WriteLine("Все бабули:");
            foreach (var g in allGrannies)
            {
                string diseases = g.Medicines.Count == 0
                    ? "здорова"
                    : string.Join(", ", g.Medicines.Select(p => $"{p.Key} (лекарство: {p.Value})"));
                Console.WriteLine($"{g.Name}, {g.Age} лет, болезни: {diseases} -> {g.Location}");
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("Все больницы:");
            foreach (var h in hospitalsArray)
            {
                Console.WriteLine($"{h.Name}: лечит [{string.Join(", ", h.TreatableDiseases)}], " +
                                  $"бабуль: {h.Patients.Count}/{h.Capacity}, заполнено: {h.FillPercent:F0}%");
            }


            // ---------------- Задание 4 ----------------
            Console.WriteLine(string.Empty);
            Console.WriteLine("     Задание 4");
            Console.WriteLine("Поиск кратчайшего пути в графе обходом в ширину (BFS).");
            Console.WriteLine("От вас ничего не требуется.");
            Console.WriteLine(string.Empty);

            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>()
            {
                { 0, new List<int> { 1, 2 } },
                { 1, new List<int> { 0, 3, 4 } },
                { 2, new List<int> { 0, 5 } },
                { 3, new List<int> { 1 } },
                { 4, new List<int> { 1, 5 } },
                { 5, new List<int> { 2, 4 } }
            };

            int start = 0;
            int finish = 5;
            List<int> path = ShortestPath(graph, start, finish);

            if (path != null)
            {
                Console.WriteLine("Кратчайший путь: " + string.Join(" -> ", path));
            }
            else
            {
                Console.WriteLine("Путь не найден.");
            }
        }
    }
}