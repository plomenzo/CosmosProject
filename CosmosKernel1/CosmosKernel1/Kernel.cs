//James Bardunias
//Preston Lomenzo

using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        const int DIR_SIZE = 100;

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. \nType 'help' to see available commands.");
        }

        protected override void Run()
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();
            String[] filenames = new String[DIR_SIZE];
            String[] filedata = new String[DIR_SIZE];
            String[] fileext = new String[DIR_SIZE];
            String[] filedate = new String[DIR_SIZE];
            int[] filesize = new int[DIR_SIZE];
            int numFiles = 0;

            if (input.ToLower() == "help") //help
            {
                //should be in alphabetical order
                Console.WriteLine("\t-create <Filename>.<Extension>\tCreates a file and requests input. See -save");
                Console.WriteLine("\t-date\tDisplays current date");
                Console.WriteLine("\t-dir\tDisplays directory");
                Console.WriteLine("\t-echo\tEchos input");
                Console.WriteLine("\t-help\tDisplays a list of available commands");
                Console.WriteLine("\t-save\tSaves open file");
                Console.WriteLine("\t-time\tDisplays the current time");

            } else if (input.ToLower() == "time") //time
            {
                Console.WriteLine("The current time is: " + Cosmos.Hardware.RTC.Hour + ":" + Cosmos.Hardware.RTC.Minute + ":" + Cosmos.Hardware.RTC.Second);
            } else if (input.ToLower() == "date") //date
            {
                var day = "";

                switch (Cosmos.Hardware.RTC.DayOfTheWeek)
                {
                    case 0: day = "Sunday";
                        break;
                    case 1: day = "Monday";
                        break;
                    case 2: day = "Tuesday";
                        break;
                    case 3: day = "Wednesday";
                        break;
                    case 4: day = "Thursday";
                        break;
                    case 5: day = "Friday";
                        break;
                    case 6: day = "Saturday";
                        break;
                    default:
                        day = "ERROR: Date function malfunction";
                        break;
                }

                Console.WriteLine("The current date is: " + day + " " + Cosmos.Hardware.RTC.Month + "/" + Cosmos.Hardware.RTC.DayOfTheMonth + "/" + Cosmos.Hardware.RTC.Century + Cosmos.Hardware.RTC.Year );
            } else if (input.ToLower().Substring(0,5) == "echo ") //echo
            {
                 Console.WriteLine(input.Substring(7, 100));
            } else if (input.ToLower().Substring(0, 7) == "create ") //create
            {
                //TODO*************UNDER CONSTRUCTION****************

                var fileName = input.Substring(7, 100);

                //if (fileName.Contains(" "))
                //    Console.WriteLine("ERROR: Filename may not contain a space");
                //else
                //{
                    int runningsize = 0;
                    String inputdata = "";

                    while (input.ToLower() != "save") //gathering each lines input data
                    {
                        input = Console.ReadLine();
                        runningsize += input.Length;
                        inputdata += input;
                    }

                    int index = -1;
                    
                    for (int i = 0; i < DIR_SIZE; i++) //finds first avaible slot for a new file
                    {
                        if (filenames[i] == null)
                        {

                            index = i;
                            break;
                        }
                    }

                    filenames[index] = "name";//TODO get file name somehow
                    fileext[index] = ".txt"; //TODO file file ext somehow
                    filedate[index] = Cosmos.Hardware.RTC.Month + "/" + Cosmos.Hardware.RTC.DayOfTheMonth + "/" + Cosmos.Hardware.RTC.Century + Cosmos.Hardware.RTC.Year;
                    filesize[index] = 20 + (runningsize / 2) * 4; //"In the current implementation at least, strings take up 20+(n/2)*4 bytes"
                    filedata[index] = inputdata;
                    numFiles++;
                //}




            } else if (input.ToLower().Substring(0, 5) == "save ") //save
            {
                 Console.WriteLine("ERROR: No open file to save");
            } else if (input.ToLower() == "dir") //directory
            {
                Console.WriteLine("Filename           \tExtension\tDate     \tSize");
                Console.WriteLine("---------------------------------------------------------------------");
                for (int i = 0; i < DIR_SIZE; i++)
                {
                    Console.Write(i);
                    if (filenames[i] == null)
                        continue;
                    
                    Console.WriteLine(filenames[i] + "\t" + fileext[i] + "\t" + filedate[i] + "\t" + filesize[i]);
                }
            }
            else //unknown command
            {
                Console.Write("'" + input + "'" + " is not a recognized command");
            }
        }
    }
}

