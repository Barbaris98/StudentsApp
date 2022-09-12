using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


//#region для красоты кода,чтоб модно было сварачивать
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


            Console.WriteLine("StudentsApp");

            //4
            SqlDataReader sqlDataReader = null;

            //5 строка в которой будут храиться вводимые значения
            // от пользователя ???? что такое string.Empty???
            string command = string.Empty;


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
                    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

                    //разбиваем команду по символам на подсткроки по пробелу,,,,,
                    //Split возвращает массив, поэтому для того что бы получить первое слово
                    //т.е нулевой элемент в массиве мы должы обратиться к нулевому элементу [0]
                    //
                    switch (command.Split(' ')[0].ToLower())
                    {
                        case "select":

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
                        case "insert":

                            // ExecuteNonQuery вернёт нам кол-во изменённый строк
                            // т.е "1"   если не получилось то "0"
                            Console.WriteLine($"Добавлено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "update":

                            Console.WriteLine($"Изменено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "delete":

                            Console.WriteLine($"Удалено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        default:

                            Console.WriteLine($"Команда  {command}  некорректна!");

                            break;
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

            }



            Console.WriteLine("Для продолжения нажмите любую клавишу ... ");
            Console.ReadKey();
        }
    }
}
