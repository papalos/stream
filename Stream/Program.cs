using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream
{
    class Program
    {
        static void Main(string[] args)
        {  
            /*1.1 Используя диалоги в окне консоли, сформировать текстовый файл 
             * с информацией об индексе учебной группы, количестве студентов в ней 
             * и количестве студентов, успешно сдавших сессию (не менее 10-ти групп).*/

            FileGenerator myFile = new FileGenerator(@"C:\filetest\droup.txt");
            myFile.generate();


            /*1.2Открыть созданный файл и переписать во второй файл следующую информацию: 
             * индекс группы и процент успеваемости в ней для групп, 
             * в которых успеваемость выше заданной величины (задается с клавиатуры).*/
            myFile.percentSuccess(@"C:\filetest\success.txt");

            /*2.1 Используя перенаправление потоков для программы, написанной в части 1 данного задания, 
             * организовать печать в файл «system.log» содержимого окна консоли. */
            //          myFile.generate(@"C:\filetest\system.log");

            /* 2.2 Используя перенаправление потоков организовать печать из одного файла в другой, 
             * при этом посредником должно выступать окно консоли. */
            myFile.clone(@"C:\filetest\droup.txt");

            return;
        }
    }
}
