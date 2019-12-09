using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR_code_challenges.Lib;
using OCR_code_challenges.Challenges._1_20;
using System.Text.RegularExpressions;

namespace OCR_code_challenge_Tests.Tests._1_20
{
	[TestClass]
	public class SpeedTracker_tests
	{
		public const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		public const string numbers = "0123456789";
		public const string characters = letters + numbers + " ";
		public const string testDataOutputPath = @"..\TestData\SpeedTracker\test_cameradata.txt";

		public DateTime[][] getRandomisedCameraTimes(Random random, int amount)
		{
			DateTime[][] results = new DateTime[amount][];

			for (int i = 0; i < amount; i++)
			{
				int firstCameraTicks = random.Next();
				int secondCameraTicks = random.Next(firstCameraTicks + 1);
				results[i] = new DateTime[] { new DateTime(firstCameraTicks), new DateTime(secondCameraTicks) };
			}

			return results;
		}

		public string[] getRandomisedNumberplates(Random random, int amount, bool areValid)
		{
			string[] results = new string[amount];

			if (areValid)
			{
				for(int i = 0; i < amount; i++)
				{
					results[i] = letters.randomSlice(2);
					results[i] += numbers.randomSlice(2);

					if (random.Next(1) == 1)
						results[i] += " ";

					results[i] += letters.randomSlice(3);
				}
			}
			else
			{
				for (int i = 0; i < amount; i++)
				{
					do
						results[i] = characters.randomSlice(random.Next(characters.Length));
					while (Regex.IsMatch(results[i], @"^[A-z]{2}\d{2}\s?[A-z]{3}$"));
				}
			}

			return results;
		}

		[TestMethod]
		public void test_times()
		{
			DateTime firstCameraTime = new DateTime(2019, 12, 3, 15, 2, 0);

			DateTime[] secondCameraTimes = new DateTime[]
			{
				firstCameraTime.AddHours(1),
				firstCameraTime.AddHours(0.5),
				firstCameraTime.AddHours(2),
				firstCameraTime.AddHours(10),
				firstCameraTime.AddHours(0.4),
				firstCameraTime.AddHours(0.0125),
				firstCameraTime.AddHours(0.01)
			};

			double[] expectedSpeeds = new double[] { 1, 2, 0.5, 0.1, 2.5, 80, 100 };

			for(int i = 0; i < secondCameraTimes.Length; i++)
			{
				DateTime secondTime = secondCameraTimes[i];
				double output = SpeedTracker.averageSpeed(firstCameraTime, secondTime);
				Assert.AreEqual(expectedSpeeds[i], output);
			}
		}
		
		[TestMethod]
		public void test_times_random()
		{
			int iterations = 1000;
			Random random = new Random();

			DateTime[][] randomisedTimes = getRandomisedCameraTimes(random, iterations);

			foreach(DateTime[] times in randomisedTimes)
			{
				double expectedSpeed = SpeedTracker.cameraDistanceInMiles / times[1].Subtract(times[0]).TotalHours;

				double result = SpeedTracker.averageSpeed(times[0], times[1]);

				Assert.AreEqual(expectedSpeed, result);
			}
		}

		[TestMethod]
		public void test_numberplates()
		{
			string[] validNumberPlates = new string[]
			{
				"aa00bbb",
				"ZZ99ZZZ",
				"AB56 JgE",
				"gF01 uuu",
				"eu10DPO",
				"HG67JhM",
				"EE33 EEE"
			};

			string[] invalidNumberPlates = new string[]
			{
				"aa00bb",
				"aa 00bbb",
				"01gh123",
				"a022abc",
				"gh75yeht",
				"abc12dfg",
				"hi 01 abc",
				"itu66 abc",
				"",
				"ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			};

			foreach(string numberplate in validNumberPlates)
				Assert.IsTrue(SpeedTracker.validNumberPlate(numberplate));

			foreach (string numberplate in invalidNumberPlates)
				Assert.IsFalse(SpeedTracker.validNumberPlate(numberplate));
		}
		
		[TestMethod]
		public void test_numberplates_random()
		{
			int iterations = 1000;
			Random random = new Random();

			string[] validNumberPlates = getRandomisedNumberplates(random, iterations, true);
			string[] invalidNumberPlates = getRandomisedNumberplates(random, iterations, false);

			foreach(string numberplate in validNumberPlates)
				Assert.IsTrue(SpeedTracker.validNumberPlate(numberplate));

			foreach(string numberplate in invalidNumberPlates)
				Assert.IsFalse(SpeedTracker.validNumberPlate(numberplate));
		}

		[TestMethod]
		public void test_offenders()
		{

		}
	}
}
