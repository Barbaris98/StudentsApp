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
        //элемент безопасности, строка подключения будет в этом листе храниться
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentsDB"].ConnectionString;



        static void Main(string[] args)
        {

            

            Console.ReadKey();
        }
    }
}
