using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;

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
                FileReport.WriteLine("КН род; КН доч; Вид доч.объекта; Путь к файлу; Наличие в хранилище; Ошибка при конвертации;");
                System.Data.DataTable objParentDb = new System.Data.DataTable();

                TextFieldParser tfp = new TextFieldParser("path_to_file.csv", Encoding.GetEncoding(1251));
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(";");

                OraAdap.SelectCommand = new OracleCommand();
                OraAdap.SelectCommand.Connection = OraCon;

                while (!tfp.EndOfData)
                {
                    string[] tfpMassive = tfp.ReadFields();
                    OraAdap.SelectCommand.CommandText = "";
                    OraAdap.SelectCommand.Parameters.Add(":PATH", tfpMassive[0].ToString());
                    OraAdap.Fill(objParentDb);

                    foreach (System.Data.DataRow row in objParentDb.Rows)
                    {
                        if (row[1].ToString() != "")
                        {
                            {
                                FileReport.Write(row[0].ToString() + ";" + row[1].ToString() + ";" + row[3].ToString() + ";" + row[5].ToString() + ";" + tfpMassive[1].ToString() + ";");
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
                                FileReport.Write(row[0].ToString() + ";" + row[1].ToString() + ";" + "Этаж" + ";" + row[5].ToString() + ";" + tfpMassive[1].ToString() + ";");
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
                    objParentDb.Clear();


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
