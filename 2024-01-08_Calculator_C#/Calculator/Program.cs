/*
 * Name: Kaci Craycraft
 * South Hills Username: kcraycraft45
 */

using System.Numerics;

namespace Calculator
{
    public class Program
    {
        public const String EQUATIONS_SOLUTIONS = "EquationsAndSolutions.txt";
        public const String SOLUTIONS = "Solutions.txt";

        public const ConsoleColor EQUATION_COLOR = ConsoleColor.DarkMagenta;
        public const ConsoleColor SOLUTION_COLOR = ConsoleColor.Magenta;
        public const ConsoleColor SECONDARY_COLOR = ConsoleColor.DarkGray;
        public const ConsoleColor FONT_COLOR = ConsoleColor.White;
        public const ConsoleColor TABLE_COLOR = ConsoleColor.DarkGray;
        public static void Main()
        {
            Random rand = new();
            Console.Title = "Calculator";
            ErrorsEncountered://Just in case an error snuck in past any Parsing / TryParsing
                     //Will jump back to this point and essentially restart the program without crashing.
            
            
            while (true)//Keep going unless user types 'quit'
            {
                Console.Clear();//Keeps console the same between calculations.
                BeginningPrint();

                if (!File.Exists(EQUATIONS_SOLUTIONS)) { File.Create(EQUATIONS_SOLUTIONS).Close(); }
                if (!File.Exists(SOLUTIONS)) { File.Create(SOLUTIONS).Close(); }
                //Create the files necessary if they do not exist

                String[] memoryItem = File.ReadAllLines(SOLUTIONS);//Solutions as array
                String memoryItemOrNot;
                double result = 0;

                Console.ForegroundColor = FONT_COLOR;Console.Write("\nInput an arithmetic equation: ");
                Console.ForegroundColor = SOLUTION_COLOR; String input = Console.ReadLine().Trim(); Console.ForegroundColor = FONT_COLOR;

                ForceQuit(input);

                while (!input.Contains(' ') || input.Equals("history", StringComparison.OrdinalIgnoreCase))
                {//While there are no spaces, or user typed 'history'
                    while (input.Equals("history", StringComparison.OrdinalIgnoreCase))
                    {//Can keep repeating history, but cannot leave until they choose a different input type
                        String[] contents = File.ReadAllLines(EQUATIONS_SOLUTIONS);
                        //Equations are Odd
                        //Answers are even

                        Console.ForegroundColor = EQUATION_COLOR;
                        for (int i = 0; i < contents.Length; i++)
                        {
                            if (i % 2 != 0)//If i is even
                            {
                                Console.WriteLine("mem" + ((i + 1) / 2) + " = " + contents[i] + "\n");//Show the name of the solution to the user for use
                            }
                            else
                            {
                                Console.WriteLine(contents[i]);
                            }
                            Thread.Sleep(50);
                        }

                        Thread.Sleep(1000);
                        Console.ForegroundColor = FONT_COLOR; Console.Write("Input an arithmetic equation: ");
                        Console.ForegroundColor = SOLUTION_COLOR; input = Console.ReadLine().Trim(); Console.ForegroundColor = FONT_COLOR;
                    }

                    if (!input.Contains(' '))//If there are no spaces, the input will not be accepted.
                    {
                        Console.Write("Invalid equation, please follow the specified format: ");
                        Console.ForegroundColor = SOLUTION_COLOR; input = Console.ReadLine().Trim(); Console.ForegroundColor = FONT_COLOR;
                    }

                    ForceQuit(input);
                    
                }

                List<String> inputAsList = [.. input.Split(' ')];//Split by spaces
                bool showWork = inputAsList[^1].Equals("s", StringComparison.OrdinalIgnoreCase) && inputAsList.Remove("s");
                //If user wishes for the calculator to show their work, it will, and then the flag will be removed from the input as List

                inputAsList = InputValidation(inputAsList);

                for (int i = 0; i < inputAsList.Count; i++)
                {
                    bool isInteger = false;
                    int memVal = 0;
                    String validFileChoice;
                    bool savedSolutionPrint;

                    try { memoryItemOrNot = inputAsList[i][..3];}
                    catch {memoryItemOrNot = "not";}//Catches error upon List value being of a length less than 3
                    if (memoryItemOrNot.Equals("mem", StringComparison.OrdinalIgnoreCase))//If variable is a saved solution
                    {
                        validFileChoice = inputAsList[i][3..];//Get the saved solution's number
                        try
                        {
                            isInteger = int.TryParse(validFileChoice, out memVal);//Check that it is a number
                                                                                  //If it is, assign that number to memVal
                        }
                        catch
                        {
                            ErrorGoTo();
                            goto ErrorsEncountered;
                        }
                        
                        while (memVal > memoryItem.Length || memVal < 1 || !isInteger) 
                        {   //While choosing a value past what is saved, OR
                            //While choosing a value less than 1, OR
                            //While the value following 'mem' is not an integer
                            Console.WriteLine($"You do not have the saved value ({inputAsList[i]}).\nYou have {memoryItem.Length} saved solutions.");
                                //Tell user how many saved solutions they have
                            Console.Write("Would you like to see your saved solutions? (y/n): ");
                            Console.ForegroundColor = SOLUTION_COLOR; savedSolutionPrint = (Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase)); Console.ForegroundColor = FONT_COLOR;
                            
                            if(memoryItem.Length == 0)
                            {
                                Console.WriteLine("You have no saved solutions.\nPress any key to try a new equation");
                                Console.ReadKey();
                                goto ErrorsEncountered;
                            }
                            else if (savedSolutionPrint && memoryItem.Length !=0)//only print their saved solutions from memory if they type 'y' or 'Y'
                            {
                                for (int j = 0; j < memoryItem.Length; j++)//Print saved solutions and their variable name for use
                                {
                                    Console.WriteLine($"mem{j + 1}\t{memoryItem[j]}");
                                    Thread.Sleep(50);//Roll it out 
                                }
                            }

                            Thread.Sleep(1000);
                            if (memoryItem.Length == 1)
                            {
                                Console.Write($"Please choose a different solution to replace {inputAsList[i]} (i.e. 'mem1'): ");

                            }
                            else
                            {
                                Console.Write($"Please choose a different solution to replace {inputAsList[i]} (i.e. 'mem{rand.Next(1, memoryItem.Length / 2)}'): ");
                                //Clarify which value the user chose is invalid
                                //Choose a random valid mem value as an example to hinder confusion
                            }
                            Console.ForegroundColor = SOLUTION_COLOR; inputAsList[i] = Console.ReadLine(); Console.ForegroundColor = FONT_COLOR;
                            validFileChoice = inputAsList[i][3..];//Get the saved solution's number
                            try
                            {
                                isInteger = int.TryParse(validFileChoice, out memVal); //Check if it is numeric and assign to memVal if so
                            }
                            catch
                            {
                                ErrorGoTo();
                                goto ErrorsEncountered;
                            }
                        }
                        inputAsList[i] = memoryItem[memVal - 1];//Aligns the user facing mem value with computer speak.
                                                                //human 1 = computer 0
                    }
                }
                String finalList = String.Join(" ", inputAsList); //Need to save the equation's VALID original String
                                                                  //Because we are about to modify their valid input when in list form.

                if (showWork)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine(Environment.NewLine + "Here is my work!\n");
                    Thread.Sleep(500);
                    Console.ForegroundColor = EQUATION_COLOR;PrintEquation(inputAsList);
                    Console.ForegroundColor = SECONDARY_COLOR;
                }
                
                Console.ForegroundColor = SECONDARY_COLOR;
                //We are going to remove operators and their surrounding operands as we calculate the equation
                //Therefore, while there are certain operators, we must continue to calculate their results.
                while (inputAsList.Contains("*") || inputAsList.Contains("/"))
                {
                    if (showWork) { PrintEquation(inputAsList); }
                    for (int i = 1; i < inputAsList.Count; i += 2)//Looking only at operators
                                                                  //Input validation ensures their positions are correct
                    {
                        if (inputAsList[i].Equals("*", StringComparison.OrdinalIgnoreCase))//If we find an applicable operator,
                                                  //We perform its function on the operands surrounding it
                                                  //Then we remove all 3 elements from the equation
                                                  //And replace them with one element: the quotient/product
                        {
                            try
                            {
                                inputAsList[i - 1] = (double.Parse(inputAsList[i - 1]) * double.Parse(inputAsList[i + 1])).ToString();
                                inputAsList.RemoveRange(i, 2);
                            }
                            
                            catch 
                            {
                                ErrorGoTo();
                                goto ErrorsEncountered;
                            }
                            break;
                        }

                        else if (inputAsList[i].Equals("/", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                inputAsList[i - 1] = (double.Parse(inputAsList[i - 1]) / double.Parse(inputAsList[i + 1])).ToString();
                                inputAsList.RemoveRange(i, 2);
                            }
                            catch 
                            {
                                ErrorGoTo();
                                goto ErrorsEncountered;
                            }
                            break;
                        }
                    }

                }//No more multiplicative or divisive operators

                while (inputAsList.Contains("-"))
                {
                    for (int i = 1; i < inputAsList.Count; i += 2)//Same as comments for mult/div
                    {
                        if (inputAsList[i] == "-")
                        {
                            if (!inputAsList[i + 1].Contains('-'))//If an operand is preceded by a subtraction operator
                                                                  //We want to flip its sign so that we can just add up all the remaining list elements at the end
                            {
                                inputAsList[i + 1] = "-" + inputAsList[i + 1];
                                inputAsList[i] = "+";
                            }
                            else
                            {
                                inputAsList[i + 1] = inputAsList[i + 1][1..];//Cannot prepend a negative with "-", must flip it to positive.
                            }
                        }
                    }
                }//Equation no longer contains subtraction symbols
                 //Only negative doubles instead
                
                for (int i = 0; i < inputAsList.Count; i += 2)//For every operand
                {
                    try { result += double.Parse(inputAsList[i]); }
                    catch 
                    {
                        ErrorGoTo();
                        goto ErrorsEncountered; 
                    }//Will restart the program if any errors are caught.
                    if (showWork)
                    {
                        Thread.Sleep(50);//RollOut
                        Console.Write(result);//Print the initial operand
                        for (int j = i + 1; j < inputAsList.Count; j += 2)//For every operator
                        {
                            Thread.Sleep(100);//RollOut
                            Console.Write(" " + inputAsList[j]);//Print the operator space separated

                            if (j != inputAsList.Count - 1)
                            {
                                Console.Write(" " + inputAsList[j + 1]);//Print the following operand of that operator
                            }
                        }
                        Console.WriteLine(Environment.NewLine);//All was written on one line, so we need new line characters between each equation in calculations
                    }
                }

                File.AppendAllText(SOLUTIONS, result.ToString() + Environment.NewLine);//Add solution to Solutions.txt
                File.AppendAllLines(EQUATIONS_SOLUTIONS, [finalList, result.ToString()]);//Add Equation and solution to EquationsAndSolutions.txt

                Console.ForegroundColor = SOLUTION_COLOR; Console.WriteLine(result); Console.ForegroundColor = FONT_COLOR;
                if(double.IsInfinity(result) || (double.IsInfinity(-1*result)))//Make user aware that infinite results may not actually be infinite
                                                                               //Since a double's maximum value is 1.79769313486231570e+308d
                                                                               //It is possible that a very large value will evaluate as ∞,
                                                                                    //Even if the resulting value would be within a double's range of validity.
                {
                    Console.WriteLine("This result may not produce accurate answers when performing further calculations.");
                }
                Thread.Sleep(1000);
                Console.Write("\nPress any key to perform another calculation.");//Pause to allow user to see the solution
                Console.ReadKey();
            }
            Console.ForegroundColor = SECONDARY_COLOR;//Not unreachable, despite warning
                                                      //Can confirm because terminal exit print is the proper color
        }
        public static void ErrorGoTo()
        {
            Console.ForegroundColor = SECONDARY_COLOR;
            Console.WriteLine("Errors Encountered");
            Thread.Sleep(1000);
            Console.WriteLine("Restarting Calculator...");
            Thread.Sleep(1500);
        }
        public static void PrintEquation(List<String> inputAsList) //Used in cases like mult/div in which all three elements are removed, and result replaces them
        {
            Thread.Sleep(1000);
            for (int i = 0; i < inputAsList.Count; i++)
            {
                Console.Write(inputAsList[i] + " ");
                Thread.Sleep(100);
            }
            Console.WriteLine(Environment.NewLine);
        }
        public static List<String> InputValidation(List<String> input)
        {
            bool equationIsRightAmtOfCharacters = false;
            bool oddsAreOperators = false;
            bool evensAreOperands = false;
            
            while ((!oddsAreOperators || !evensAreOperands || !equationIsRightAmtOfCharacters) && !input[0].Equals("history", StringComparison.OrdinalIgnoreCase))
            {//(While operators are incorrectly placed, OR
             //Operands are incorrectly placed, OR
             //Equation has an amt of operators greater than or equal to the amt of operands)
             //AND
             //user did not request their previous equations and solutions
                equationIsRightAmtOfCharacters = false;
                oddsAreOperators = false;
                evensAreOperands = false;

                if (input.Count % 2 == 1)//input array must be odd for the correct amount of operators.
                {
                    equationIsRightAmtOfCharacters = true;
                }

                for (int i = 1; i < input.Count; i += 2)//For every operator position
                {
                    if (input[i].Equals("+", StringComparison.OrdinalIgnoreCase) || input[i].Equals("-") || input[i].Equals("*") || input[i].Equals("/"))
                    {//String must be an acceptable operator
                        oddsAreOperators = true;
                    }
                }

                for (int i = 0; i < input.Count; i += 2)//For every operand
                {
                    try//Try to parse operand to a double
                    {
                        bool isInteger = double.TryParse(input[i], out double notRelevant);
                        if (isInteger || input[i].Substring(0, 3).Equals("mem", StringComparison.OrdinalIgnoreCase))//Gets the number following 'mem'
                        {
                            evensAreOperands = true;
                        }
                    }
                    catch//If it cannot be parsed, it is not an integer
                         //Try catch is here (despite the TryParse) because an error kept being thrown regardless if input = "* * *" (for example)
                    {
                        evensAreOperands = false;
                        break;
                    }
                }

                if (!oddsAreOperators || !evensAreOperands ||!equationIsRightAmtOfCharacters)//If any of the input validation checks came back false
                                                                                             //Need to reprompt the user
                {
                    Console.Write("Invalid equation, please follow the specified format: ");
                    Console.ForegroundColor = SOLUTION_COLOR; String inputAsString = Console.ReadLine().Trim(); Console.ForegroundColor = FONT_COLOR;
                    ForceQuit(inputAsString);
                    input = [.. inputAsString.Split(' ')];
                }
            }//Outside of while loop, input *should be* valid
            return input;
        }
        public static void ForceQuit(String prompt)
        {
            if (prompt.Equals("quit", StringComparison.OrdinalIgnoreCase))//If user decides to quit the application
            {
                Console.ForegroundColor = SECONDARY_COLOR;//Changes exit print font color
                Environment.Exit(0);
            }
        }

        public static void BeginningPrint()
        {
            String consoleTitle = "calculator".ToUpper();
            String[] left = //Must be organized from smallest to largest in length.
            ["Quit Program",
            "Saved Solutions",
            "Equation Format",
            "Show Calculations",
            "Using Saved Solutions"];
            String[] right = //These two arrays are used to show the valid input formats to the user upon every calculation 
                ["quit",
                "history",
                "1 + -2 - 3 * 4 / -5",
                "1 + -2 - 3 * 4 / -5 S",
                "1 + mem2 - 3 * mem4 / -5" ];

            int leftPadding = (left[3] + "\t|\t" + right[^1]).Length / 2 - consoleTitle.Length / 2;
            Console.Write(new String(' ', leftPadding));//These two lines center the title on the table below it.
            Console.ForegroundColor = FONT_COLOR;Console.Write("««");
            Console.ForegroundColor = EQUATION_COLOR;Console.Write(consoleTitle);
            Console.ForegroundColor = FONT_COLOR;Console.Write("»»\n\n");
            
            int[] lengths = new int[left.Length];
            for (int i = 0; i < left.Length; i++)
            {//Part of the code below that only works in this format.....??
                lengths[i] = left[i].Length;
            }

            int tabThreshold = Array.IndexOf(left, "Saved Solutions");//I know this uses a magic string.
                                                                      //Not quite sure how to differentiate the barrier any other way
                                                                      //This is the first String that is large enough to warrant an extra tab in output
            Console.ForegroundColor = TABLE_COLOR;
            foreach (String sentence in left)
            {
                if (sentence.Length > lengths[tabThreshold])
                {//Just makes the middle seperator for the directions even.
                    Console.WriteLine(sentence + "\t|\t" + right[Array.IndexOf(left, sentence)]);
                }
                else
                {
                    Console.WriteLine(sentence + "\t\t|\t" + right[Array.IndexOf(left, sentence)]);
                }
            }
        }
    }
}
