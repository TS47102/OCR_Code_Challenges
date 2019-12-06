using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR_code_challenges.Challenges._1_20;

namespace OCR_code_challenge_Tests.Tests._1_20
{
	[TestClass]
	public class SpeedTracker_tests
	{
		[TestMethod]
		public void test_times()
		{
			DateTime firstCameraTime = new DateTime(2019, 12, 3, 15, 2, 0);

			List<DateTime> secondCameraTimes = new List<DateTime>() {
				firstCameraTime.AddHours(1),
				firstCameraTime.AddHours(0.5),
				firstCameraTime.AddHours(2),
				firstCameraTime.AddHours(10),
				firstCameraTime.AddHours(0.4),
				firstCameraTime.AddHours(0.0125),
				firstCameraTime.AddHours(0.01)
			};

			List<double> expectedSpeeds = new List<double>() { 1, 2, 0.5, 0.1, 2.5, 80, 100 };

			var outputs = secondCameraTimes.Select(time => SpeedTracker.averageSpeed(firstCameraTime, time));

			Assert.IsTrue(expectedSpeeds.SequenceEqual(outputs));
		}

		[TestMethod]
		public void test_numberplates()
		{
			List<string> validNumberPlates = new List<string>()
			{
				"aa00bbb",
				"ZZ99ZZZ",
				"AB56 JgE",
				"gF01 uuu",
				"eu10DPO",
				"HG67JhM",
				"EE33 EEE"
			};

			List<string> invalidNumberPlates = new List<string>()
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

			Assert.IsTrue(validNumberPlates.TrueForAll(plate => SpeedTracker.validNumberPlate(plate)));
			Assert.IsTrue(invalidNumberPlates.TrueForAll(plate => !SpeedTracker.validNumberPlate(plate)));
		}
	}
}
