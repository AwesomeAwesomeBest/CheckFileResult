using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace CheckFiles_forConvertDWG
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                OracleConnection OraCon = new OracleConnection();
                OraCon.ConnectionString = File.ReadAllText("Setting.txt");
                OraCon.Open();
                Console.WriteLine("Подключение к базе установлено");
                OracleDataAdapter OraAdap = new OracleDataAdapter();
                StreamWriter FileReport = new StreamWriter("Result.txt", false, Encoding.UTF8);
                FileReport.WriteLine("КН род; КН доч; Вид доч.объекта; Путь к файлу; Наличие в хранилище;");
                StreamReader pathToFile = new StreamReader("path_to_file.txt", Encoding.GetEncoding(1251));
                System.Data.DataTable objParent = new System.Data.DataTable();
                string sLine = "";
                ArrayList arrText = new ArrayList();
                while (sLine != null)
                {
                    sLine = pathToFile.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                foreach (string sOutput in arrText)
                {
                    OraAdap.SelectCommand = new OracleCommand();
                    OraAdap.SelectCommand.Connection = OraCon;
                    OraAdap.SelectCommand.CommandText = "";
                    OraAdap.SelectCommand.Parameters.Add(":PATH", sOutput.ToString());
                    OraAdap.Fill(objParent);

                    foreach (System.Data.DataRow row in objParent.Rows)
                    {
                        if (row[1].ToString() != "")
                        {
                            {
                                FileReport.Write(row[0].ToString() + ";" + row[1].ToString() + ";" + row[3].ToString() + ";" + row[5].ToString() + ";");
                                if (File.Exists(row[5].ToString()) == false)
                                {
                                    FileReport.Write("    Файл отсутствует в хранилище;");
                                    FileReport.WriteLine();
                                }
                                else
                                {
                                    FileReport.Write("    Файл присутствует в хранилище;");
                                    FileReport.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            {
                                FileReport.Write(row[0].ToString() + ";" + row[1].ToString() + ";" + "Этаж" + ";" + row[5].ToString() + ";");
                                if (File.Exists(row[5].ToString()) == false)
                                {
                                    FileReport.Write("    Файл отсутствует в хранилище;");
                                    FileReport.WriteLine();
                                }
                                else
                                {
                                    FileReport.Write("    Файл присутствует в хранилище;");
                                    FileReport.WriteLine();
                                }
                            }
                        }
                    }
                    objParent.Clear();

                }



                FileReport.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Работа программы завершена. Нажмите любую клавишу.");
            Console.ReadKey();
        }
    }
}
