using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;


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


            while(true)
            {
                Console.Write("> ");
                command = Console.ReadLine();

                //без учёта регистра|| сравнииваем значение ввода Equals
                if (command.ToLower().Equals("exit"))
                {

                }


            }


            Console.ReadKey();
        }
    }
}
