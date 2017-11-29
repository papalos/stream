using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stream
{
    class FileGenerator
    {
        private int num1, num2;        // переменные для проверки введенных символов на принадлежность к цифрам
        private string stringForFile;  // переменная для формирования строки которая будет записана в файл
        private string _pathFile;      // путь к файлу
        private FileStream text;       // поток
        private FileStream success;       // поток
        private StreamWriter write;    // для управление текстом запись в потоке
        private StreamReader read;     // для управление текстом чтение в потоке

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pathFile">Полный путь к создаваемому файлу</param>
        public FileGenerator(string pathFile)
        {
            _pathFile = pathFile;
        }

        /// <summary>
        /// Создает файл с данными о группах
        /// </summary>
        public void generate() {
            text = new FileStream(_pathFile, FileMode.OpenOrCreate, FileAccess.Write);
            write = new StreamWriter(text);
            //создаем 10 групп
            for (int i = 1; i <= 10; i++) {
                stringForFile = ""; // очищаем переменную от предыдущих записей
                Console.WriteLine("Индекс группы " + i + "\nУкажите количество студентов в этой группе: ");
                //считываем количество студентов и проверяем чтобы введенные данные были целым числом
                if (int.TryParse(Console.ReadLine(), out num1))
                {   // если условие выполнено то записываем индекс группы и через | количество студентов
                    stringForFile += i.ToString() + "|" + num1.ToString();
                    Console.WriteLine("\nУкажите количество студентов успешно сдавших сессию в этой группе: ");
                    if (int.TryParse(Console.ReadLine(), out num2)&&num1>=num2)
                    {
                        stringForFile += "|" + num2.ToString(); //если условие выполнено записываем количество здавших студентов
                    }
                    else
                    {   // иначе выводим сообщение об ошибке
                        Console.WriteLine("Введен неверный формат данных");
                        i--;      // уменьшаем текущее положение счетчика к исходному
                        continue; // и запускаем цикл заново
                    }
                }
                else
                {
                    Console.WriteLine("Введен неверный формат данных");
                    i--;
                    continue;
                }

                // Записываем в файл сформированную строку
                write.WriteLine(stringForFile);                
            }
            write.Close();
        }
        /// <summary>
        /// Создает файл с данными о группах, при этом ведет логирование консольного вывода
        /// </summary>
        /// <param name="logFile">файл в который происходит логирование</param>
        public void generate(string logFile)
        {   // перегрузка метода расположенного выше, данный метод ведет логирование
            text = new FileStream(_pathFile, FileMode.OpenOrCreate, FileAccess.Write);            
            write = new StreamWriter(text);

            FileStream log = new FileStream(logFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter outLog = new StreamWriter(log);
            TextWriter outCons = Console.Out;
            string stringOut;
            
            for (int i = 1; i <= 10; i++)
            {
                stringForFile = "";
                stringOut = "Индекс группы " + i + "\nУкажите количество студентов в этой группе: ";

                Console.SetOut(outLog);
                Console.WriteLine(stringOut);
                Console.SetOut(outCons);
                Console.WriteLine(stringOut);


                if (int.TryParse(Console.ReadLine(), out num1))
                {
                    Console.SetOut(outLog);
                    Console.WriteLine(num1.ToString());
                    Console.SetOut(outCons);

                    stringForFile += i.ToString() + "|" + num1.ToString();

                    stringOut = "\nУкажите количество студентов успешно сдавших сессию в этой группе: ";
                    Console.SetOut(outLog);
                    Console.WriteLine(stringOut);
                    Console.SetOut(outCons);
                    Console.WriteLine(stringOut);

                    if (int.TryParse(Console.ReadLine(), out num2) && num1 >= num2)
                    {
                        Console.SetOut(outLog);
                        Console.WriteLine(num2.ToString());
                        Console.SetOut(outCons);

                        stringForFile += "|" + num2.ToString(); 
                    }
                    else
                    {
                        stringOut = "Введен неверный формат данных";
                        Console.SetOut(outLog);
                        Console.WriteLine(stringOut);
                        Console.SetOut(outCons);
                        Console.WriteLine(stringOut);
                        i--;
                        continue; 
                    }
                }
                else
                {
                    stringOut = "Введен неверный формат данных";
                    Console.SetOut(outLog);
                    Console.WriteLine(stringOut);
                    Console.SetOut(outCons);
                    Console.WriteLine(stringOut);
                    i--;
                    continue;
                }
                
                write.WriteLine(stringForFile);
            }
            write.Close();
            outLog.Close();
        }
        /// <summary>
        /// Создает файл с процентом сдавших сессию
        /// </summary>
        /// <param name="successFile">файл в который происходит запись успешных групп</param>
        public void percentSuccess(string successFile)
        {   // открываем два потока и передаем управление классам reader и writer
            text = new FileStream(_pathFile, FileMode.Open, FileAccess.Read);
            read = new StreamReader(text);
            success = new FileStream(successFile, FileMode.OpenOrCreate, FileAccess.Write);
            write = new StreamWriter(success);
            string readText;                  // прочитанная строка
            string[] splitString;             // распарсенная строка
            int percent;                      // процент успеваемости
            Console.WriteLine("Задайте значение успеваемости в процентах для формирования отчета: ");
            if (int.TryParse(Console.ReadLine(), out num1))
            {
                for (;;)
                {
                    readText = read.ReadLine();          // читаем строку из файла
                    Debug.WriteLine(readText + "ppp\n");
                    if (readText == "1"|| readText == "") { break; }       // если строка пустая завершаем чтение
                    splitString = readText.Split('|');   // иначе парсим
                    // ищем процент успеваемости в группе
                    Debug.WriteLine(splitString[2] + "\n" + splitString[1] + "\n");
                    percent = (int)((double.Parse(splitString[2]) / double.Parse(splitString[1])) * 100);
                    Debug.WriteLine(percent + "\n");
                    // сравниваем с заданным
                    if (percent > num1)
                    {   // при выполнении условия записываем новую строку в новый файл
                        Debug.WriteLine(splitString[0] + "|" + percent.ToString() + "\n");
                        write.WriteLine(splitString[0] + "|" + percent.ToString());
                    }
                }
            }
            write.Close();
            read.Close();
            

        }
        /// <summary>
        /// Клонирует файл и выводит его содержимое на консоль
        /// </summary>
        /// <param name="cloneFile">копируемый файл</param>
        /// <returns>true-при успешном копировании</returns>
        public bool clone(string cloneFile)
        {
            //проверяем наличие копируемого файла
            if (!File.Exists(cloneFile)) { return false; }

            //Переменные для буфера и для хранения консольных настроек
            string buff;
            TextWriter outConsole = Console.Out;
            TextReader InConsole = Console.In;

            //Создаем потоки 
            FileStream inFile = new FileStream(cloneFile, FileMode.Open, FileAccess.Read);
            FileStream outFile = new FileStream(@"C:\filetest\clone.cln" , FileMode.OpenOrCreate, FileAccess.ReadWrite); 
            
            //обработчики потоков
            StreamReader newIn = new StreamReader(inFile);
            StreamWriter newOut = new StreamWriter(outFile);
            
            //Меняем консольные настройки
            Console.SetIn(newIn);
            Console.SetOut(newOut);

            while (true)
            { 
                //закончить когда достигнут конец файла
                if (newIn.EndOfStream) break;
                
                buff = Console.ReadLine();
                Console.WriteLine(buff);

            }

            //Записываем все в выводной поток
            newOut.Flush();
            //Перходим в начало этого файла
            newOut.BaseStream.Position = 0;
            //создаем обработчик на чтение скопированного файла
            StreamReader clon = new StreamReader(outFile); 
            //Закрываем не нужный нам больше поток копируемого файла
            newIn.Close();

            
            //Меняем консольные настройки, на вход приходит вновь созданный файл
            Console.SetIn(clon);
            Console.SetOut(outConsole);
            
            //Читаем этот файл в консоль до конца
            while (true)
            {
                if (clon.EndOfStream) break;

                buff = Console.ReadLine();
                Console.WriteLine(buff);

            }

            //Возвращаем консоли возможность читать с клавиатуры
            Console.SetIn(InConsole);
            //закрываем базовый поток вывода
            clon.Close();       
            
            return true;
        }
    }
}
