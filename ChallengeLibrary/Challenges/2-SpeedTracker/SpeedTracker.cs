using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ChallengeLibrary.Challenges._2_SpeedTracker
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
	public static class SpeedTracker
	{
		public const double cameraDistanceInMiles = 1;
		public const double speedLimitMph = 70;
		public const string outputFileExtension = "txt";
		public static readonly char recordFieldSeperator = ',';
		public static readonly string outputFolderPath = @"..\ProgramData\SpeedTracker\";

		public static double averageSpeed(DateTime firstCameraTime, DateTime secondCameraTime)
		{
			return cameraDistanceInMiles / secondCameraTime.Subtract(firstCameraTime).TotalHours;
		}

		public static bool validNumberPlate(string numberPlate)
		{
			return Regex.IsMatch(numberPlate, @"^[A-z]{2}\d{2}\s?[A-z]{3}$");
		}

		public static void createOffendersFile(string inputFilePath)
		{
			if (!File.Exists(inputFilePath))
				throw new FileNotFoundException($"File {inputFilePath} does not exist.");

			createOffendersFile(inputFilePath, outputFolderPath + Regex.Match(inputFilePath, @".+\\{1}(.+)\.{1}.+$").Value + "_offenders." + outputFileExtension);
		}

		public static void createOffendersFile(string inputFilePath, string outputFilePath)
		{
			using (StreamWriter writer = File.CreateText(outputFilePath))
			{
				foreach (string line in File.ReadLines(inputFilePath))
				{
					string[] details = line.Split(recordFieldSeperator);
					if (details.Length != 2)
						throw new IOException($"Line '{line}' in file '{inputFilePath}' has malformed format. (Incorrect number of fields)");

					double speed;
					if (!double.TryParse(details[0], out speed))
						throw new IOException($"Line '{line}' in file '{inputFilePath}' has malformed format. (Speed was not a number)");

					OffenceTypes offence = OffenceTypes.none;

					if (speed > speedLimitMph)
						offence = OffenceTypes.speeding;

					if (!validNumberPlate(details[1]))
						if (offence == OffenceTypes.speeding)
							offence = OffenceTypes.both;
						else
							offence = OffenceTypes.badNumberPlate;

					if (offence != OffenceTypes.none)
						writer.WriteLine(offence.ToString() + recordFieldSeperator + speed + recordFieldSeperator + details[1]);
				}
			}
		}

		public enum OffenceTypes
		{
			speeding,
			badNumberPlate,
			both,
			none
		}
	}
}
