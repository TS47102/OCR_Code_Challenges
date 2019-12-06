using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCR_code_challenges.Challenges._1_20
{
	public static class SpeedTracker
	{
		public static readonly double cameraDistanceInMiles = 1;

		public static double averageSpeed(DateTime firstCameraTime, DateTime secondCameraTime)
		{
			return cameraDistanceInMiles / secondCameraTime.Subtract(firstCameraTime).TotalHours;
		}

		public static bool validNumberPlate(string numberPlate)
		{
			return Regex.IsMatch(numberPlate, @"^[A-z]{2}\d{2}\s?[A-z]{3}$");
		}
	}
}
