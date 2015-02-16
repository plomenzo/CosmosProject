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
        String[] filenames;
        String[] filedata;
        String[] fileext;
        String[] filedate;
        int[] filesize;
        String[] variable;
        int[] variableValue;

        protected override void BeforeRun()
        {
            filenames = new String[DIR_SIZE];
            filedata = new String[DIR_SIZE];
            fileext = new String[DIR_SIZE];
            filedate = new String[DIR_SIZE];
            filesize = new int[DIR_SIZE];
            variable = new String[DIR_SIZE];
            variableValue = new int[DIR_SIZE];
            Console.WriteLine("Cosmos booted successfully. \nType 'help' to see available commands.");
        }

        protected override void Run()
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();

            int numFiles = 0;

            if (input.ToLower() == "help") //help
            {
                //should be in alphabetical order
                Console.WriteLine("\t-create <Filename>.<Extension>\tCreates a file and requests input. See -save");
                Console.WriteLine("\t-date\tDisplays current date");
                Console.WriteLine("\t-dir\t Displays directory");
                Console.WriteLine("\t-echo\tEchos input");
                Console.WriteLine("\t-help\tDisplays a list of available commands");
                Console.WriteLine("\t-save\tSaves open file");
                Console.WriteLine("\t-time\tDisplays the current time");
                Console.WriteLine("\t-set\t Set variable to an expression");
                Console.WriteLine("\t-out\t Display variable value");
            }
            else if (input.ToLower() == "time") //time
            {
                Console.WriteLine("The current time is: " + Cosmos.Hardware.RTC.Hour + ":" + Cosmos.Hardware.RTC.Minute + ":" + Cosmos.Hardware.RTC.Second);
            }
            else if (input.ToLower() == "date") //date
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

                Console.WriteLine("The current date is: " + day + " " + Cosmos.Hardware.RTC.Month + "/" + Cosmos.Hardware.RTC.DayOfTheMonth + "/" + Cosmos.Hardware.RTC.Century + Cosmos.Hardware.RTC.Year);
            }
            else if (input.ToLower().Substring(0, 5) == "echo ") //echo
            {
                Console.WriteLine(input.Substring(7, 100));
            }
            else if (input.ToLower().Substring(0, 7) == "create ") //create
            {
                var fileName = input.Substring(7, 100);

                //if (fileName.Contains(" "))
                //    Console.WriteLine("ERROR: Filename may not contain a space");
                //else
                //{
                int runningsize = 0;
                String inputdata = "";
                String fullFileName = input.Substring(7, 100);

                while (input.ToLower() != "save") //gathering each lines input data
                {
                    input = Console.ReadLine();
                    runningsize += input.Length;
                    inputdata += input;
                }

                int index = -1;

                for (int i = 0; i < DIR_SIZE; i++) //finds first avaible slot for a new file
                    if (filenames[i] == null)
                    {
                        index = i;
                        break;
                    }

                int indexOfDot = fullFileName.LastIndexOf('.', fullFileName.Length - 1, fullFileName.Length - 2);
                filenames[index] = fullFileName.Substring(0, indexOfDot);
                fileext[index] = fullFileName.Substring(indexOfDot, indexOfDot + 10);
                filedate[index] = Cosmos.Hardware.RTC.Month + "/" + Cosmos.Hardware.RTC.DayOfTheMonth + "/" + Cosmos.Hardware.RTC.Century + Cosmos.Hardware.RTC.Year;
                filesize[index] = 20 + (runningsize / 2) * 4; //"In the current implementation at least, strings take up 20+(n/2)*4 bytes"
                filedata[index] = inputdata;
                numFiles++;

                //}

            }
            else if (input.ToLower().Substring(0, 5) == "save ") //save
            {
                Console.WriteLine("ERROR: No open file to save");
            }
            else if (input.ToLower() == "dir") //directory
            {
                Console.WriteLine("Filename           \tExtension\tDate     \tSize");
                Console.WriteLine("---------------------------------------------------------------------");
                for (int i = 0; i < DIR_SIZE; i++)
                {
                    if (filenames[i] == null)
                        continue;

                    Console.WriteLine(filenames[i] + "               \t" + fileext[i] + "     \t" + filedate[i] + "\t" + filesize[i] + " b");
                }
            }
            // IF INPUT IS SET 
            else if (input.ToLower().Substring(0, 4) == "set ") 
            {
                string varName = input.Substring(4, input.IndexOf('=') - 5);    
                string inputOperand = "";
                char operators = ' ';
                int sum = 0;
                // Iterates through each char in string starting from end of command
                for (int x = (input.IndexOf('=') + 2); x < input.Length; x++)
                {
                    // Sets the first operand if there is an arithmetic expression
                    if ((input[x] == '+' || input[x] == '-' || input[x] == '*' || input[x] == '/' || input[x] == '&' || input[x] == '|' || input[x] == '^') 
                        && operators == ' ')
                    {
                        operators = input[x];
                        sum += Int32.Parse(inputOperand);
                        inputOperand = "";
                    }
                    // If more the two operands 
                    else if (input[x] == '+')
                    {
                        sum += Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else if (input[x] == '-')
                    {
                        sum -= Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else if (input[x] == '/')
                    {
                        sum /= Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else if (input[x] == '*')
                    {
                        sum *= Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else if (input[x] == '&')
                    {
                        sum &= Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else if (input[x] == '|')
                    {
                        sum |= Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else if (input[x] == '^')
                    {
                        sum ^= Int32.Parse(inputOperand);
                        inputOperand = "";
                        operators = input[x];
                    }
                    else // Iterates through char's until it reaches an operator
                        inputOperand += input[x];
                    // If there is no arithmetic expression set variable to user input
                    if (operators == ' ' && x == input.Length - 1)
                        sum += Int32.Parse(inputOperand);
                     // Reaches end of expression, perform last calculation
                    else if (x == input.Length - 1 && operators != ' ')
                    {
                        if (operators == '+')
                            sum += Int32.Parse(inputOperand);
                        if (operators == '-')
                            sum -= Int32.Parse(inputOperand);
                        if (operators == '*')
                            sum *= Int32.Parse(inputOperand);
                        if (operators == '/')
                            sum /= Int32.Parse(inputOperand);
                        if (operators == '&')
                            sum &= Int32.Parse(inputOperand);
                        if (operators == '|')
                            sum |= Int32.Parse(inputOperand);
                        if (operators == '^')
                            sum ^= Int32.Parse(inputOperand);
                    }
                }
                // Finds first avaible slot for a new variable and value
                for (int i = 0; i < DIR_SIZE; i++)  
                {   
                    // Resets value is same variable is chosen 
                    if (variable[i] == varName)
                    {
                        variable[i] = varName;
                        variableValue[i] = sum;
                        break;
                    }
                    // Places variable and value into empty into 1st empty index
                    else if (variable[i] == null)
                    {
                        variable[i] = varName;
                        variableValue[i] = sum;
                        break;
                    }
                }
            }
            // IF INPUT IS OUT  
            else if (input.ToLower().Substring(0, 4) == "out ")
            {
                // Sets the variable to the the chosen variable
                string varName = input.Substring(4, input.Length); 
                // Looks for value and outputs value for selected variable
                for (int i = 0; i < DIR_SIZE; i++)
                {
                    if (variable[i] == varName)
                    {
                        Console.Write(variableValue[i]);
                        break;
                    }
                }
            }
            else //unknown command
            {
                Console.Write("'" + input + "'" + " is not a recognized command");
            }
        }
    }
}
