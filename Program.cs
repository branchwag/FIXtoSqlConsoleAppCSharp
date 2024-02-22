using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixtoSqlConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string logFile;
            Console.WriteLine("Welcome to the FIX to SQL processor. This processor accepts FIX logs with a pipe separator.");
            Console.WriteLine("Enter your FIX log filepath:");
            logFile = Console.ReadLine();

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = Path.Combine(docPath, "FIXtoSqlProcessorOutput.txt");

            //Deletes any output file that may already be in Documents
            File.Delete(fileName);

            try
            {
                foreach (string line in File.ReadAllLines(logFile))
                {
                    if (line.Contains("35=8"))
                    {
                        //establishing fields we want to pluck from the FIX message. Feel free to set your own fields and adjust the tags further below to fit your requirements. 
                        string tradeDate = null;
                        string clOrdID = null;
                        string symbol = null;
                        string price = null;
                        string account = null;
                        string side = null;
                        string status = null;
                        string quantity = null;
                        
                        //feel free to edit the below line to use another delimiter if you are not using pipes
                        foreach (var a in line.Split('|'))
                        {
                            //tradedate
                            if (a.StartsWith("75="))
                            {
                                tradeDate = a.Substring(3);
                            }
                            //clorderid
                            if (a.StartsWith("11="))
                            {
                                clOrdID = a.Substring(3);
                            }
                            //symbol
                            if (a.StartsWith("55="))
                            {
                                symbol = a.Substring(3);
                            }
                            //price
                            if (a.StartsWith("44="))
                            {
                                price = a.Substring(3);
                            }
                            //account
                            if (a.StartsWith("1="))
                            {
                                account = a.Substring(2);
                            }
                            //side
                            if (a.StartsWith("54="))
                            {
                                side = a.Substring(3);
                            }
                            //status
                            if (a.StartsWith("39="))
                            {
                                status = a.Substring(3);
                            }
                            //quantity
                            if (a.StartsWith("38="))
                            {
                                quantity = a.Substring(3);
                            }
                        }

                        //string together values to make an Exec statement. Fill this in with your stored proc or whatever SQL you are running
                        using (StreamWriter sw = new StreamWriter(fileName, true))
                        {
                            sw.WriteLine("EXEC CoolStoredProc @pTradeDate = '" + tradeDate + "',@pOrderNumber = '" + clOrdID + "',@pSymbol = '" + symbol + "',@pPrice = '" + price + "',@pAccount = '" + account + "',@pSide = '" + side + "',@pStatus = '" + status + "',@pQuantity = '" + quantity + "';");
                        }
                    }

                    else
                    {
                        Console.WriteLine("No executions found.");
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read. " + e.Message);
            }

            Console.WriteLine("SQL generation complete");

            //keep console open
            Console.ReadLine();
        }
    }
}
