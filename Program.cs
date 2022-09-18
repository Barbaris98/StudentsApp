using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;


//#region для красоты кода,чтоб модно было сварачивать/
#region

/* User Manual
     1.Выход exit


*/
#endregion



namespace StudentsApp
{
    class Program
    {
        //1 элемент безопасности, строка подключения будет в этом листе храниться
        //сделаем их static чтоб работать со static методом Main
        private static string connectionString = ConfigurationManager.ConnectionStrings["StudentsDB"].ConnectionString;

        // 2 Посредник SqlConnection, черз него будем работать с sql сервером
        //сделаем их static чтоб работать со static методом Main
        private static SqlConnection sqlConnection = null;


        static void Main(string[] args)
        {
            //3
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();


            Console.WriteLine("<START StudentsApp> " + "| Для справки введите  help | " + "| Для выхода введите  exit |");

            //4
            SqlDataReader sqlDataReader = null;

            //5 строка в которой будут храиться вводимые значения
            // от пользователя ???? что такое string.Empty???
            //Empty - типо пустая
            string command = string.Empty;

            string result = string.Empty; 

            while (true)
            {
                try
                {
                    Console.Write("> ");
                    command = Console.ReadLine();

                    
                    //Exit
                    //без учёта регистра|| сравнииваем значение ввода Equals
                    if (command.ToLower().Equals("exit"))
                    {
                        //закрываем соединение, если оно открыто
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }
                        //закрываемм чтеца
                        if (sqlDataReader != null)
                        {
                            sqlDataReader.Close();
                        }
                        break;// выкинет из while
                    }

                    //6 созд SqlCommand и передаём      команду(то что вводим) и sqlConnection
                    SqlCommand sqlCommand = null;

                    //разбиваем команду по символам на подсткроки по пробелу,,,,,
                    //Split возвращает массив, поэтому для того что бы получить первое слово
                    //т.е нулевой элемент в массиве мы должы обратиться к нулевому элементу [0]
                    //НЕТ Split- разбивает строку на подстроки , начиная с нулевого эл-та массива
                    //

                    string[] commandArray = command.ToLower().Split(' ');

                    switch (commandArray[0])
                    {
                        case "select":

                            sqlCommand = new SqlCommand(command, sqlConnection);

                            //ExecuteReader() - ??
                            // sqlDataReader здесь будет возвращать нам двумерный массив
                            // m на n ячкек
                            sqlDataReader = sqlCommand.ExecuteReader();

                            //Read() перемещает sqlDataReader к следующей записи/строке
                            // и возвращает булевое значение когда уже вся таблицв прочитана 
                            while (sqlDataReader.Read())
                            {

                                // к Ридеру обращаемся по идексатору , те [ID] и тд
                                Console.WriteLine($"{sqlDataReader["Id"]} {sqlDataReader["FIO"]}" +
                                    $"{sqlDataReader["Birthday"]} {sqlDataReader["Universuty"]}" +
                                    $"{sqlDataReader["Group_number"]} {sqlDataReader["Course"]}" +
                                    $"{sqlDataReader["Averange_score"]}");

                                Console.WriteLine(new string('-', 30));

                            }
                            //закроем чтеца и потом его по коду заного создадим
                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            break;

                        case "fselectall":// case "fselect": указывает на case "select", который буде выполняться...
                        case "selectall":

                            sqlCommand = new SqlCommand("SELECT * FROM [Students]", sqlConnection);


                            //ExecuteReader() - ??
                            // sqlDataReader здесь будет возвращать нам двумерный массив
                            // m на n ячкек
                            sqlDataReader = sqlCommand.ExecuteReader();

                            //Read() перемещает sqlDataReader к следующей записи/строке
                            // и возвращает булевое значение когда уже вся таблицв прочитана 
                            while (sqlDataReader.Read())
                            {
                                // к Ридеру обращаемся по идексатору , те [ID] и тд
                                /*Console.WriteLine($"{sqlDataReader["Id"]} {sqlDataReader["FIO"]}" +
                                    $"{sqlDataReader["Birthday"]} {sqlDataReader["Universuty"]}" +
                                    $"{sqlDataReader["Group_number"]} {sqlDataReader["Course"]}" +
                                    $"{sqlDataReader["Averange_score"]}");



                                Console.WriteLine(new string('-', 30));
                                */

                                // += -это значит вешаем результат на переменну, или присваиваем???
                                result += $"{sqlDataReader["Id"]} {sqlDataReader["FIO"]}" +
                                    $"{sqlDataReader["Birthday"]} {sqlDataReader["Universuty"]}" +
                                    $"{sqlDataReader["Group_number"]} {sqlDataReader["Course"]}" +
                                    $"{sqlDataReader["Averange_score"]}\n";

                                result += new string('-', 30) + "\n";

                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            Console.WriteLine(result);

                            //здесь мы обращаемся к нулевому индексу массива
                            //, вводмых нами на клавиатуры, строк commandArray[0] . 
                            // далее к нулевому индексу/символу нулевой строки т.е commandArray[0][0]
                            if (commandArray[0][0] == 'f')
                            {
                                // первый параметр у new StreamWriter() - передаём путь куда будем сохранять файл
                                //AppDomain.CurrentDomain.BaseDirectory - обращаемся
                                // к баззовой директории, т.е там где наш exe., там будем сохранять
                                // через слэш "/" указывем имя файла в виде: команжа + время сохраниения
                                // DateTime.Now.ToString().Replace(':', '-')} приводим к строке и заменяем двоеточие на тире Replace(':', '-')
                                // true - булевое значение,это типо говорит о том что мы доб данные в файл
                                // Encoding.UTF8 - кодровка стандартная 
                                using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
                                {
                                    sW.WriteLine(DateTime.Now.ToString());

                                    sW.WriteLine(command);

                                    sW.WriteLine(result);

                                }
                            }
                            

                            break;
                        case "insert":

                            sqlCommand = new SqlCommand(command, sqlConnection);

                            // ExecuteNonQuery вернёт нам кол-во изменённый строк
                            // т.е "1"   если не получилось то "0"
                            Console.WriteLine($"Добавлено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "update":

                            sqlCommand = new SqlCommand(command, sqlConnection);

                            Console.WriteLine($"Изменено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "delete":

                            sqlCommand = new SqlCommand(command, sqlConnection);

                            Console.WriteLine($"Удалено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;

                        case "fsortby":
                        case "sortby":

                            //sortby fio asc | desc
                            // сократим команду,            [1] - имя колонки , [2]- параметр asc | desc
                            sqlCommand = new SqlCommand($"SELECT * FROM [Students] ORDER BY {commandArray[1]} {commandArray[2]}", sqlConnection);

                            sqlDataReader = sqlCommand.ExecuteReader();

                            //Read() перемещает sqlDataReader к следующей записи/строке
                            // и возвращает булевое значение когда уже вся таблицв прочитана 
                            while (sqlDataReader.Read())
                            {
                                result += $"{sqlDataReader["Id"]} {sqlDataReader["FIO"]}" +
                                    $"{sqlDataReader["Birthday"]} {sqlDataReader["Universuty"]}" +
                                    $"{sqlDataReader["Group_number"]} {sqlDataReader["Course"]}" +
                                    $"{sqlDataReader["Averange_score"]}\n";

                                result += new string('-', 30) + "\n";
                            }

                            //закроем чтеца и потом его по коду заного создадим
                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            Console.WriteLine(result);
                            //запись в файл 
                            if (commandArray[0][0] == 'f')
                            {
                                using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
                                {
                                    sW.WriteLine(DateTime.Now.ToString());

                                    sW.WriteLine(command);

                                    sW.WriteLine(result);
                                }
                            }

                            break;
                        case "clear":
                            sqlCommand = new SqlCommand(command, sqlConnection);

                            Console.Clear();
                            Console.WriteLine("Пространство ОЧИЩЕННО!");

                            break;

                        case "fsearch":
                        case "search":

                            if (commandArray[1].Equals("fio"))
                            {
                                //   в %%- это у нас...что такое
                                sqlCommand = new SqlCommand($"SELECT * FROM [Students] WHERE FIO LIKE N'%{commandArray[2]}%'", sqlConnection);

                            }
                            else if (commandArray[1].Equals("birthday"))
                            {
                                sqlCommand = new SqlCommand($"SELECT * FROM [Students] WHERE Birthday='{commandArray[2]}'", sqlConnection);

                            }
                            else
                            {
                                Console.WriteLine("Команда НЕ корректна!");
                            }

                            try
                            {
                                sqlDataReader = sqlCommand.ExecuteReader();

                                while (sqlDataReader.Read())
                                {
                                    result += $"{sqlDataReader["Id"]} {sqlDataReader["FIO"]}" +
                                    $"{sqlDataReader["Birthday"]} {sqlDataReader["Universuty"]}" +
                                    $"{sqlDataReader["Group_number"]} {sqlDataReader["Course"]}" +
                                    $"{sqlDataReader["Averange_score"]}\n";

                                    result += new string('-', 30) + "\n";
                                }

                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine($"Ошибка: {ex.Message}");
                            }
                            finally
                            {
                                //закроем чтеца
                                if (sqlDataReader != null)
                                {
                                    sqlDataReader.Close();
                                }

                                Console.WriteLine(result);


                                if (commandArray[0][0] == 'f')
                                {
                                    using (StreamWriter sW = new StreamWriter(
                                        $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                        true, Encoding.UTF8))
                                    {
                                        sW.WriteLine(DateTime.Now.ToString());

                                        sW.WriteLine(command);

                                        sW.WriteLine(result);
                                    }
                                }

                            }


                            break;
                        // ... для того чтобы выводить результат этих 4-ёх команд
                        // нам Ридеры не понадобятся т.к у нас будет лишь
                        // одно значение возвращенно.
                        case "fmin":
                        case "min":

                            sqlCommand = new SqlCommand("SELECT MIN(Averange_score) FROM [Students]", sqlConnection);

                            //  $ - форматированнный, он же "красивый" вывод
                            //                      ExecuteScalar() - возвращает одно какое-то значение
                            //Console.WriteLine($"Минимальный средний балл: {sqlCommand.ExecuteScalar()}");

                            result = $"Минимальный средний балл: {sqlCommand.ExecuteScalar()}";
                            Console.WriteLine(result);
                            if (commandArray[0][0] == 'f')
                            {
                                using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
                                {
                                    sW.WriteLine(DateTime.Now.ToString());

                                    sW.WriteLine(command);

                                    sW.WriteLine(result);
                                }
                            }

                            break;
                        case "fmax":
                        case "max":

                            sqlCommand = new SqlCommand("SELECT MAX(Averange_score) FROM [Students]", sqlConnection);
                            
                            //Console.WriteLine($"Максимальный средний балл: {sqlCommand.ExecuteScalar()}");

                            result = $"Максимальный средний балл: {sqlCommand.ExecuteScalar()}";
                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
                                {
                                    sW.WriteLine(DateTime.Now.ToString());

                                    sW.WriteLine(command);

                                    sW.WriteLine(result);
                                }
                            }

                            break;
                        case "favg":
                        case "avg":

                            sqlCommand = new SqlCommand("SELECT AVG(Averange_score) FROM [Students]", sqlConnection);

                            result = $"Среднее значение по колонке 'Средний балл' : {sqlCommand.ExecuteScalar()}";
                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
                                {
                                    sW.WriteLine(DateTime.Now.ToString());

                                    sW.WriteLine(command);

                                    sW.WriteLine(result);
                                }
                            }

                            break;
                        case "fsum":
                        case "sum":

                            sqlCommand = new SqlCommand("SELECT SUM(Averange_score) FROM [Students]", sqlConnection);

                            //Console.WriteLine($"Сумма средних баллов : {sqlCommand.ExecuteScalar()}");

                            result = $"Сумма средних баллов : {sqlCommand.ExecuteScalar()}";
                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
                                {
                                    sW.WriteLine(DateTime.Now.ToString());

                                    sW.WriteLine(command);

                                    sW.WriteLine(result);
                                }
                            }

                            break;
                        case "help":
                            
                            Console.WriteLine("Доступные команды: "
                                + "\n" + "     select"
                                + "\n" + "     insert"
                                + "\n" + "     update"
                                + "\n" + "     delete"
                                + "\n" + "     sortby (команда |SELECT * FROM [Students] ORDER BY| ставится авт-ки, используй сразу |sortby fio asc|)");
                            break;
                        default:

                            Console.WriteLine($"Команда  {command}  некорректна!" + "Для справки введите  help ");

                            break;
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                //очищаеам result перед каждой новой итерацией
                result = string.Empty;

            }

            Console.WriteLine("Для продолжения нажмите любую клавишу ... ");
            Console.ReadKey();
        }
    }
}
