using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCR_code_challenges.Challenges._1_20
{
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

					offenceTypes offence = offenceTypes.none;

					if (speed > speedLimitMph)
						offence = offenceTypes.speeding;

					if (!validNumberPlate(details[1]))
						if (offence == offenceTypes.speeding)
							offence = offenceTypes.both;
						else
							offence = offenceTypes.badNumberPlate;

					if (offence != offenceTypes.none)
						writer.WriteLine(offence.ToString() + recordFieldSeperator + speed + recordFieldSeperator + details[1]);
				}
			}
		}

		enum offenceTypes
		{
			speeding,
			badNumberPlate,
			both,
			none
		}
	}
}
