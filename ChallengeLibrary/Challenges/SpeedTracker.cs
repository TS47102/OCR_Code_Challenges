using System;
using System.IO;
using System.Text.RegularExpressions;
using PixelLib.ConsoleHelpers;

namespace ChallengeLibrary.Challenges
{
	/// <summary>
	/// Create a program that takes a time for a car going past a speed camera, the time going past the next one and the distance between them to calculate the average speed for the car
	/// in mph.The cameras are one mile apart.
	/// Extensions:
	/// 1. Speed cameras know the timings of each car going past, through number plate recognition. Valid number plates are two letters, two numbers and three letters afterwards, for
	/// example XX77 787. Produce a part of the program that checks whether a number plate matches the given pattern. Tell the user either way.
	/// 2. Create a program for creating a file of details for vehicles exceeding the speed limit set for a section of road. You will need to create a suitable file with test data, including
	/// randomised number plates and times. You will then use the code you’ve already written to process this list to determine who is breaking the speed limit (70mph) and who has
	/// invalid number plates.
	/// </summary>
	public class SpeedTracker : IConsoleChallenge
	{
		public const double CAMERA_DISTANCE_MILES = 1;
		public const double SPEEDLIMIT_MPH = 70;
		public static readonly char RECORD_FIELD_SEPARATOR = ',';
		public static readonly string OUTPUTFOLDER_PATH = @"..\ProgramData\SpeedTracker\";
		public static readonly string OUTPUTFILE_EXTENSION = "txt";

		public static double averageSpeed (DateTime firstCameraTime, DateTime secondCameraTime)
		{
			return CAMERA_DISTANCE_MILES / secondCameraTime.Subtract (firstCameraTime).TotalHours;
		}

		public static bool validNumberPlate (string numberPlate)
		{
			return Regex.IsMatch (numberPlate, @"^[A-z]{2}\d{2}\s?[A-z]{3}$");
		}

		public static void createOffendersFile (string inputFilePath)
		{
			if (!File.Exists (inputFilePath))
				throw new FileNotFoundException ($"File {inputFilePath} does not exist.");

			createOffendersFile (inputFilePath, OUTPUTFOLDER_PATH + Regex.Match (inputFilePath, @".+\\{1}(.+)\.{1}.+$").Value + "_offenders." + OUTPUTFILE_EXTENSION);
		}

#pragma warning disable IDE0063 // Use simple 'using' statement - Keeping brackets on a single-statement statement blocks is fine if the single statement is in itself a statement block.
		public static void createOffendersFile (string inputFilePath, string outputFilePath)
		{
			using (StreamWriter writer = File.CreateText (outputFilePath))
			{
				foreach (string line in File.ReadLines (inputFilePath))
				{
					string [] details = line.Split (RECORD_FIELD_SEPARATOR);
					if (details.Length != 2)
						throw new IOException ($"Line '{line}' in file '{inputFilePath}' has malformed format. (Incorrect number of fields)");

					if (!double.TryParse (details [0], out double speed))
						throw new IOException ($"Line '{line}' in file '{inputFilePath}' has malformed format. (Speed was not a number)");

					OffenceType offence = OffenceType.none;

					if (speed > SPEEDLIMIT_MPH)
						offence = OffenceType.speeding;

					if (!validNumberPlate (details [1]))
						offence = offence == OffenceType.speeding ? OffenceType.both : OffenceType.badNumberPlate;

					if (offence != OffenceType.none)
						writer.WriteLine (offence.ToString () + RECORD_FIELD_SEPARATOR + speed + RECORD_FIELD_SEPARATOR + details [1]);
				}
			}
		}
#pragma warning restore IDE0063 // Use simple 'using' statement

		public void execute (CustomConsole console, string [] args)
		{
			throw new NotImplementedException ("TODO: Implement");
		}

		public enum OffenceType
		{
			speeding,
			badNumberPlate,
			both,
			none
		}
	}
}
