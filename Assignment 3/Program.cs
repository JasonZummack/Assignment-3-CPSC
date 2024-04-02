﻿
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml;

/// <summary>
/// Assignment 3
/// 
/// Author: 
/// Date: 
/// Purpose: Allows user to enter/save/load/edit/view daily sales values
///          from a file. Allows and displays simple data analysis
///          (mean/max/min/graph) of sales values for a given month.
/// </summary>
string mainMenuChoice;
string analysisMenuChoice;
bool displayMainMenu = true;
bool displayAnalysisMenu;
bool quit;

int physicalSize = 31;
int logicalSize = 0;

double[] sales = new double[physicalSize];

string[] dates = new string[physicalSize];

string fileName = "";

bool goAgain = true;
	while (goAgain)
	{
		try
		{
			DisplayMainMenu();
			string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
			if (mainMenuChoice == "L")
				logicalSize = LoadFileValuesToMemory(fileName, dates, sales);
			if (mainMenuChoice == "S")
				SaveMemoryValuesToFile(fileName, dates, sales, logicalSize);
			if (mainMenuChoice == "D")
				DisplayMemoryValues(dates, sales, logicalSize);
			if (mainMenuChoice == "A")
				logicalSize = AddMemoryValues(dates, sales, logicalSize);
			if (mainMenuChoice == "E")
				EditMemoryValues(dates, sales, logicalSize);
			if (mainMenuChoice == "Q")
			{
				goAgain = false;
				throw new Exception("Bye, hope to see you again");
			}
			if (mainMenuChoice == "R")
			{
				while (true)
				{
					if (logicalSize == 0)
						throw new Exception("No entries loaded. Please load a file into memory");
					DisplayAnalysisMenu();
					string analysisMenuChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
					if (analysisMenuChoice == "A")
						FindAverageOfValuesInMemory(values, logicalSize);
					if (analysisMenuChoice == "H")
						FindHighestValueInMemory(values, logicalSize);
					if (analysisMenuChoice == "L")
						FindLowestValueInMemory(values, logicalSize);
					if (analysisMenuChoice == "G")
						GraphValuesInMemory(dates, values, logicalSize);
					if (analysisMenuChoice == "R")
						throw new ExceptionO("Returning to Main Menu");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"{ex.Message}");
		}
	}

void DisplayMainMenu()
{
	Console.WriteLine("\nMain Menu");
	Console.WriteLine("L) Load Values from File to Memory");
	Console.WriteLine("S) Save Values from Memory to File");
	Console.WriteLine("D) Display Values in Memory");
	Console.WriteLine("A) Add Value in Memory");
	Console.WriteLine("E) Edit Value in Memory");
	Console.WriteLine("R) Analysis Menu");
	Console.WriteLine("Q) Quit");
}

void DisplayAnalysisMenu()
{
	Console.WriteLine("\nAnalysis Menu");
	Console.WriteLine("A) Find Average of Values in Memory");
	Console.WriteLine("H) Find Highest Value in Memory");
	Console.WriteLine("L) Find Lowest Value in Memory");
	Console.WriteLine("G) Graph Values in Memory");
	Console.WriteLine("R) Return to Main Menu");
}

string Prompt(string prompt)
{
	string response = "";
	Console.Write(prompt);
	response = Console.ReadLine();
	return response;
}

int LoadFileValuesToMemory(string filename, string[] dates, double[] values)
{
	string fileName = GetFileName();
	int logicalSize = 0;
	string filePath = $"./data/{fileName}";
	if (!File.Exists(filePath))
		throw new Exception($"The file {fileName} does not exist. ");
	string[] cvsFileInput = File.ReadAllLines(filePath);
	for(int i = 0; i < cvsFileInput.Length; i++)
	{
		Console.WriteLine($"lineIndex: {i}; line: {cvsFileInput[i]}");
		string[] items = cvsFileInput[i].Split(',');
		for (int j = 0; j < items.Length; j++)
		{
			Console.WriteLine($"itemindex: {j}; item: {items[j]}");
		}
		if (i != 0)
		{
			dates[logicalSize] = items[0];
			values[logicalSize] = double.Parse(items[1]);
			logicalSize++;
		}
	}
	Console.WriteLine($"Load complete. {fileName} has {logicalSize} data entries");
	return logicalSize;
}

void DisplayMemoryValues(string[] dates, double[] values, int logicalSize)
{
	if (logicalSize == 0)
		throw new Exception("No entries loaded. Please load a file into memory or add a value in memory.");
	Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
	Console.WriteLine($"	Date	Value");
	for (int i = 0; i < logicalSize; i++)
		Console.WriteLine($"{dates[i]}	{values[i]}");
}

double FindHighestValueInMemory(double[] values, int logicalSize)
{
	Console.WriteLine("Not Implemented Yet");
	return 0;
}

double FindLowestValueInMemory(double[] values, int logicalSize)
{
	Console.WriteLine("Not Implemented Yet");
	return 0;
}

double FindAverageValueInMemory(double[] values, int logicalSize)
{
	Console.WriteLine("Not Implemented Yet");
}

void SaveMemoryValuesToFile(string filename, string[] dates, double[] values, int logicalSize)
{
	string fileName = Prompt("Enter file name including .csv or .txt: ");
	string filePath = $"./data/{fileName}";
	if (logicalSize == 0)
		throw new Exception("No entries loaded. Please load a file into memory.");
	if (logicalSize > 1)
		Array.Sort(dates, values, 0, logicalSize);

	string[] cvsLines = new string[logicalSize + 1];
	cvsLines[0] = "dates,values";
	for (int i  = 1; i < logicalSize; i++)
	{
		cvsLines[i] = $"{dates[i - 1]},{values[i - 1]}";
	}

	File.WriteAllLines(filePath, cvsLines);
	Console.WriteLine($"Save complete. {filename} has {logicalSize} entries.");
}

int AddMemoryValues(string[] dates, double[] values, int logicalSize)
{
	double value = 0.0;
	string dataString = "";

	dataString = PromptDate("Enter data format mm-dd-yyyy (eg: 05-07-2004):");
	bool found = false;
	for (int i = 0; i < logicalSize; i++)
		if (dates[i].Equals(dataString))
			found = true;
	if (found == true)
		throw new Exception($"{dataString} is already in memory. Edit entry instead.");
	value = PromptDoubleBetweenMinMax($"Enter a double value", minValue, maxValue);
	dates[logicalSize] = dataString;
	values[logicalSize] = value;
	logicalSize++;
	return logicalSize;
}

double PromptDoubleBetweenMinMax(string prompt, double min, double max)
{
	bool invalidInput = true;
	double num = 0;
	while (invalidInput)
	{
		try
		{
			Console.WriteLine($"{prompt} between {min:n2} and {max:n2}: ");
			num = double.Parse(Console.ReadLine());
			if (num < min || num > max)
				throw new Exception($"Invalid. must be between {min} and {max}.");
			invalidInput = false;
		
		}
		catch (Exception ex)
		{
			Console.WriteLine($"{ex.Message}");
		}
	}
	return num;
}

string PromptDate(string prompt)
{
	bool invalidInput = true;
	DateTime date = DateTime.Today;
	Console.WriteLine(date);
	while (invalidInput)
	{
		try
		{
			Console.WriteLine(Prompt);
			date = DateTime.Parse(Console.ReadLine());
			Console.WriteLine(date);
			invalidInput = false;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"{ex.Message}");
		}
	}
	return date.ToString("mm-dd-yyyy");
}

void EditMemoryValues(string[] dates, double[] values, int logicalSize)
{
	double value = 0.0;
	string dataString = "";
	int foundIndex = 0;

	if (logicalSize == 0)
		throw new Exception($"No entries loaded. Please load a file to memory or add a value in memory.");
		dataString = PromptDate("Enter data format mm-dd-yyyy (eg: 05-07-2004):");
	bool found = false;
	for (int i = 0; i < logicalSize; i++)
		if (dates[i].Equals(dataString))
		{
			found = true;
			foundIndex = i;
		}
	if (found == false)
		throw new Exception($"{dataString} is not in memory. Add entry instead.");
	value = PromptDoubleBetweenMinMax($"Enter a double value", minValue, maxValue);
}

void GraphValuesInMemory(string[] dates, double[] values, int logicalSize)
{
	Console.WriteLine("Not Implemented Yet");
}







string month;
string year;
string filename;
int count = 0;
bool proceed;
double mean;
double largest;
double smallest;

DisplayProgramIntro();

DisplayMainMenu();

while (displayMainMenu)
{
	mainMenuChoice = Prompt("Enter MAIN MENU option ('D' to display menu): ").ToUpper();
	Console.WriteLine();


	//MAIN MENU Switch statement
	switch (mainMenuChoice)
	{
		case "N": //[N]ew Daily Sales Entry

			proceed = NewEntryDisclaimer();

			if (proceed)
			{
				count = EnterSalesEntries();
				Console.WriteLine();
				Console.WriteLine($"Entries completed. {count} records in temporary memory.");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("Cancelling new data entry. Returning to MAIN MENU.");
			}
			break;
		case "S": //[S]ave Entries to File
			if (count == 0)
			{
				Console.WriteLine("Sorry, LOAD data or enter NEW data before SAVING.");
			}
			else
			{
				proceed = SaveEntryDisclaimer();

				if (proceed)
				{
					filename = PromptForFilename();
					SaveSalesFile();
				}
				else
				{
					Console.WriteLine("Cancelling save operation. Returning to MAIN MENU.");
				}
			}
			break;
		case "E": //[E]dit Sales Entries
			if (count == 0)
			{
				Console.WriteLine("Sorry, LOAD data or enter NEW data before EDITING.");
			}
			else
			{
				proceed = EditEntryDisclaimer();

				if (proceed)
				{
					EditEntries();
				}
				else
				{
					Console.WriteLine("Cancelling EDIT operation. Returning to MAIN MENU.");
				}
			}
			break;
		case "L": //[L]oad Sales File
			proceed = LoadEntryDisclaimer();
			if (proceed)
			{
				count = EnterSalesEntries();
				Console.WriteLine($"{count} records were loaded.");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("Cancelling LOAD operation. Returning to MAIN MENU.");
			}
			break;
		case "V":
			if (count == 0)
			{
				Console.WriteLine("Sorry, LOAD data or enter NEW data before VIEWING.");
			}
			else
			{
				DisplayEntries();
			}
			break;
		case "M": //[M]onthly Statistics
			if (count == 0)
			{
				Console.WriteLine("Sorry, LOAD data or enter NEW data before ANALYSIS.");
			}
			else
			{
				displayAnalysisMenu = true;
				while (displayAnalysisMenu)
				{
					DisplayAnalysisMenu();


					analysisMenuChoice = Prompt("Enter ANALYSIS sub-menu option: ").ToUpper();
					Console.WriteLine();

					switch (analysisMenuChoice)
					{
						case "A": //[A]verage Sales
							mean = FindAverageValueInMemory();
							mean = Mean(sales, count);
							month = dates[0].Substring(0, 3);
							year = dates[0].Substring(7, 4);
							Console.WriteLine($"The mean sales for {month} {year} is: {mean:C}");
							Console.WriteLine();
							break;
						case "H": //[H]ighest Sales
							largest = FindHighestValueInMemory();
							month = dates[0].Substring(0, 3);
							year = dates[0].Substring(7, 4);
							Console.WriteLine($"The largest sales for {month} {year} is: {largest:C}");
							Console.WriteLine();
							break;
						case "L": //[L]owest Sales
							
							smallest = FindLowestValueInMemory();
							month = dates[0].Substring(0, 3);
							year = dates[0].Substring(7, 4);
							Console.WriteLine($"The smallest sales for {month} {year} is: {smallest:C}");
							Console.WriteLine();
							break;
						case "G": //[G]raph Sales
							DisplayChart();


							Prompt("Press <enter> to continue...");
							break;
						case "R": //[R]eturn to MAIN MENU
							displayAnalysisMenu = false;
							break;
						default: //invalid entry. Reprompt.
							Console.WriteLine("Invalid reponse. Enter one of the letters to choose a submenu option.");
							break;
					}
				}
			}
			break;
		case "D": //[D]isplay Main Menu
			DisplayMainMenu();

			break;
		case "Q": //[Q]uit Program
			quit = Prompt("Are you sure you want to quit (y/N)? ").ToLower().Equals("y");
			Console.WriteLine();
			if (quit)
			{
				displayMainMenu = false;
			}
			break;
		default: //invalid entry. Reprompt.
			Console.WriteLine("Invalid reponse. Enter one of the letters to choose a menu option.");
			break;
	}
}

DisplayProgramOutro();

// ================================================================================================ //
//                                                                                                  //
//                                              METHODS                                             //
//                                                                                                  //
// ================================================================================================ //

// ++++++++++++++++++++++++++++++++++++ Difficulty 1 ++++++++++++++++++++++++++++++++++++

static string Prompt(string prompt)
{
	string inputValue = "";
	Console.Write($"{prompt}\t");
	inputValue = Console.ReadLine();
	return inputValue;
}

static double PromptDouble(string prompt)
{
    string inputValue = "";
	double inputDouble = 0.0;
	bool valid = false;

	do
	{
		Console.Write($"{prompt}\t");
		inputValue = Console.ReadLine();
		if (double.TryParse(inputValue, out inputDouble))
		{
			valid = true;
		}
		else
		{
			Console.WriteLine($"\n\tInput value >{inputValue}< is not an acceptable numeric value\n");
		}
	} while (!valid);
   

    return inputDouble;
}
static int PromptInt(string prompt)
{
    string inputValue = "";
    int inputInt = 0;
    bool valid = false;

    do
    {
        Console.Write($"{prompt}\t");
        inputValue = Console.ReadLine();
        if (int.TryParse(inputValue, out inputInt))
        {
            valid = true;
        }
        else
        {
            Console.WriteLine($"\n\tInput value >{inputValue}< is not an acceptable numeric value\n");
        }
    } while (!valid);


    return inputInt;
}

// TODO: create the Largest method


// TODO: create the Smallest method


// TODO: create the Mean method


// ++++++++++++++++++++++++++++++++++++ Difficulty 2 ++++++++++++++++++++++++++++++++++++


// TODO: create the DisplayEntries method


// TODO: create the EnterSalesEntries method


// TODO: create the LoadSalesFile method


// TODO: create the SaveSalesFile method


// ++++++++++++++++++++++++++++++++++++ Difficulty 3 ++++++++++++++++++++++++++++++++++++

// TODO: create the EditEntries method


// ++++++++++++++++++++++++++++++++++++ Difficulty 4 ++++++++++++++++++++++++++++++++++++

// TODO: create the DisplaySalesChart method


// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// ++++++++++++++++++++++++++++++++++++ Additional Provided Methods ++++++++++++++++++++++++++++++++++++
// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

// NOTE: Many of the following methods depend on the Prompt method and will operate correctly once
// that method has been implemented.

/// <summary>
/// Displays the Program intro.
/// </summary>
static void DisplayProgramIntro()
{
	Console.WriteLine("========================================");
	Console.WriteLine("=                                      =");
	Console.WriteLine("=            Monthly  Sales            =");
	Console.WriteLine("=                                      =");
	Console.WriteLine("========================================");
	Console.WriteLine();
}

/// <summary>
/// Displays the Program outro.
/// </summary>
static void DisplayProgramOutro()
{
	Console.Write("Program terminated. Press ENTER to exit program...");
	Console.ReadLine();
}

/// <summary>
/// Displays a disclaimer for NEW entry option.
/// </summary>
/// <returns>Boolean, if user wishes to proceed (true) or not (false).</returns>
bool NewEntryDisclaimer()
{
	bool response;
	Console.WriteLine("Disclaimer: proceeding will overwrite all unsaved data.");
	Console.WriteLine("Hint: Select EDIT from the main menu instead, to change individual days.");
	Console.WriteLine("Hint: You'll need to enter data for the whole month.");
	Console.WriteLine();
	response = Prompt("Do you wish to proceed anyway? (y/N) ").ToLower().Equals("y");
	Console.WriteLine();
	return response;
}

/// <summary>
/// Displays a disclaimer for SAVE entry option.
/// </summary>
/// <returns>Boolean, if user wishes to proceed (true) or not (false).</returns>
bool SaveEntryDisclaimer()
{
	bool response;
	Console.WriteLine("Disclaimer: saving to an EXISTING file will overwrite data currently on that file.");
	Console.WriteLine("Hint: Files will be saved to this program's directory by default.");
	Console.WriteLine("Hint: If the file does not yet exist, it will be created.");
	Console.WriteLine();
	response = Prompt("Do you wish to proceed anyway? (y/N) ").ToLower().Equals("y");
	Console.WriteLine();
	return response;
}

/// <summary>
/// Displays a disclaimer for EDIT entry option.
/// </summary>
/// <returns>Boolean, if user wishes to proceed (true) or not (false).</returns>
bool EditEntryDisclaimer()
{
	bool response;
	Console.WriteLine("Disclaimer: editing will overwrite unsaved sales values.");
	Console.WriteLine("Hint: Save to a file before editing.");
	Console.WriteLine();
	response = Prompt("Do you wish to proceed anyway? (y/N) ").ToLower().Equals("y");
	Console.WriteLine();
	return response;
}

/// <summary>
/// Displays a disclaimer for LOAD entry option.
/// </summary>
/// <returns>Boolean, if user wishes to proceed (true) or not (false).</returns>
bool LoadEntryDisclaimer()
{
	bool response;
	Console.WriteLine("Disclaimer: proceeding will overwrite all unsaved data.");
	Console.WriteLine("Hint: If you entered New Daily sales entries, save them first!");
	Console.WriteLine();
	response = Prompt("Do you wish to proceed anyway? (y/N) ").ToLower().Equals("y");
	Console.WriteLine();
	return response;
}

/// <summary>
/// Displays prompt for a filename, and returns a valid filename. 
/// Includes exception handling.
/// </summary>
/// <returns>User-entered string, representing valid filename (.txt or .csv)</returns>
string PromptForFilename()
{
	string filename = "";
	bool isValidFilename = true;
	const string CsvFileExtension = ".csv";
	const string TxtFileExtension = ".txt";

	do
	{
		filename = Prompt("Enter name of .csv or .txt file to save to (e.g. JAN-2024-sales.csv): ");
		if (filename == "")
		{
			isValidFilename = false;
			Console.WriteLine("Please try again. The filename cannot be blank or just spaces.");
		}
		else
		{
			if (!filename.EndsWith(CsvFileExtension) && !filename.EndsWith(TxtFileExtension)) //if filename does not end with .txt or .csv.
			{
				filename = filename + CsvFileExtension; //append .csv to filename
				Console.WriteLine("It looks like your filename does not end in .csv or .txt, so it will be treated as a .csv file.");
				isValidFilename = true;
			}
			else
			{
				Console.WriteLine("It looks like your filename ends in .csv or .txt, which is good!");
				isValidFilename = true;
			}
		}
	} while (!isValidFilename);
	return filename;
}