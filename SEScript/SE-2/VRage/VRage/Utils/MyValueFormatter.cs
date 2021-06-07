using System;
using System.Globalization;
using System.Text;

namespace VRage.Utils
{
	public class MyValueFormatter
	{
		private static NumberFormatInfo m_numberFormatInfoHelper;

		private static readonly string[] m_genericUnitNames;

		private static readonly float[] m_genericUnitMultipliers;

		private static readonly int[] m_genericUnitDigits;

		private static readonly string[] m_forceUnitNames;

		private static readonly float[] m_forceUnitMultipliers;

		private static readonly int[] m_forceUnitDigits;

		private static readonly string[] m_torqueUnitNames;

		private static readonly float[] m_torqueUnitMultipliers;

		private static readonly int[] m_torqueUnitDigits;

		private static readonly string[] m_workUnitNames;

		private static readonly float[] m_workUnitMultipliers;

		private static readonly int[] m_workUnitDigits;

		private static readonly string[] m_workHoursUnitNames;

		private static readonly float[] m_workHoursUnitMultipliers;

		private static readonly int[] m_workHoursUnitDigits;

		private static readonly string[] m_timeUnitNames;

		private static readonly float[] m_timeUnitMultipliers;

		private static readonly int[] m_timeUnitDigits;

		private static readonly string[] m_weightUnitNames;

		private static readonly float[] m_weightUnitMultipliers;

		private static readonly int[] m_weightUnitDigits;

		private static readonly string[] m_volumeUnitNames;

		private static readonly float[] m_volumeUnitMultipliers;

		private static readonly int[] m_volumeUnitDigits;

		private static readonly string[] m_distanceUnitNames;

		private static readonly float[] m_distanceUnitMultipliers;

		private static readonly int[] m_distanceUnitDigits;

		static MyValueFormatter()
		{
			m_genericUnitNames = new string[5]
			{
				"",
				"k",
				"M",
				"G",
				"T"
			};
			m_genericUnitMultipliers = new float[5]
			{
				1f,
				1000f,
				1000000f,
				1E+09f,
				1E+12f
			};
			m_genericUnitDigits = new int[5]
			{
				1,
				1,
				1,
				1,
				1
			};
			m_forceUnitNames = new string[9]
			{
				"N",
				"kN",
				"MN",
				"GN",
				"TN",
				"PN",
				"EN",
				"ZN",
				"YN"
			};
			m_forceUnitMultipliers = new float[9]
			{
				1f,
				1000f,
				1000000f,
				1E+09f,
				1E+12f,
				1E+15f,
				1E+18f,
				1E+21f,
				1E+24f
			};
			m_forceUnitDigits = new int[9]
			{
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1
			};
			m_torqueUnitNames = new string[3]
			{
				"Nm",
				"kNm",
				"MNm"
			};
			m_torqueUnitMultipliers = new float[3]
			{
				1f,
				1000f,
				1000000f
			};
			m_torqueUnitDigits = new int[3]
			{
				0,
				1,
				1
			};
			m_workUnitNames = new string[4]
			{
				"W",
				"kW",
				"MW",
				"GW"
			};
			m_workUnitMultipliers = new float[4]
			{
				1E-06f,
				0.001f,
				1f,
				1000f
			};
			m_workUnitDigits = new int[4]
			{
				0,
				2,
				2,
				2
			};
			m_workHoursUnitNames = new string[4]
			{
				"Wh",
				"kWh",
				"MWh",
				"GWh"
			};
			m_workHoursUnitMultipliers = new float[4]
			{
				1E-06f,
				0.001f,
				1f,
				1000f
			};
			m_workHoursUnitDigits = new int[4]
			{
				0,
				2,
				2,
				2
			};
			m_timeUnitNames = new string[5]
			{
				"Unit_sec",
				"Unit_min",
				"Unit_hours",
				"Unit_days",
				"Unit_years"
			};
			m_timeUnitMultipliers = new float[5]
			{
				1f,
				60f,
				3600f,
				86400f,
				3.1536E+07f
			};
			m_timeUnitDigits = new int[5];
			m_weightUnitNames = new string[4]
			{
				"g",
				"kg",
				"T",
				"MT"
			};
			m_weightUnitMultipliers = new float[4]
			{
				0.001f,
				1f,
				1000f,
				1000000f
			};
			m_weightUnitDigits = new int[4]
			{
				0,
				2,
				2,
				2
			};
			m_volumeUnitNames = new string[6]
			{
				"mL",
				"cL",
				"dL",
				"L",
				"hL",
				"m³"
			};
			m_volumeUnitMultipliers = new float[6]
			{
				1E-06f,
				1E-05f,
				0.0001f,
				0.001f,
				0.1f,
				1f
			};
			m_volumeUnitDigits = new int[6]
			{
				0,
				0,
				0,
				0,
				2,
				1
			};
			m_distanceUnitNames = new string[4]
			{
				"mm",
				"cm",
				"m",
				"km"
			};
			m_distanceUnitMultipliers = new float[4]
			{
				0.001f,
				0.01f,
				1f,
				1000f
			};
			m_distanceUnitDigits = new int[4]
			{
				0,
				1,
				2,
				2
			};
			m_numberFormatInfoHelper = new NumberFormatInfo();
			m_numberFormatInfoHelper.NumberDecimalSeparator = ".";
			m_numberFormatInfoHelper.NumberGroupSeparator = " ";
		}

		public static string GetFormatedFloat(float num, int decimalDigits)
		{
			m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
			return num.ToString("N", m_numberFormatInfoHelper);
		}

		public static string GetFormatedFloat(float num, int decimalDigits, string groupSeparator)
		{
			string numberGroupSeparator = m_numberFormatInfoHelper.NumberGroupSeparator;
			m_numberFormatInfoHelper.NumberGroupSeparator = groupSeparator;
			m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
			string result = num.ToString("N", m_numberFormatInfoHelper);
			m_numberFormatInfoHelper.NumberGroupSeparator = numberGroupSeparator;
			return result;
		}

		public static string GetFormatedDouble(double num, int decimalDigits)
		{
			m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
			return num.ToString("N", m_numberFormatInfoHelper);
		}

		public static string GetFormatedQP(decimal num)
		{
			return GetFormatedDecimal(num, 1);
		}

		public static string GetFormatedDecimal(decimal num, int decimalDigits)
		{
			m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
			return num.ToString("N", m_numberFormatInfoHelper);
		}

		public static string GetFormatedGameMoney(decimal num)
		{
			return GetFormatedDecimal(num, 2);
		}

		public static decimal GetDecimalFromString(string number, int decimalDigits)
		{
			try
			{
				m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
				return Math.Round(Convert.ToDecimal(number, m_numberFormatInfoHelper), decimalDigits);
			}
			catch
			{
			}
			return 0m;
		}

		public static float? GetFloatFromString(string number, int decimalDigits, string groupSeparator)
		{
			float? result = null;
			string numberGroupSeparator = m_numberFormatInfoHelper.NumberGroupSeparator;
			m_numberFormatInfoHelper.NumberGroupSeparator = groupSeparator;
			m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
			try
			{
				result = (float)Math.Round((float)Convert.ToDouble(number, m_numberFormatInfoHelper), decimalDigits);
			}
			catch
			{
			}
			m_numberFormatInfoHelper.NumberGroupSeparator = numberGroupSeparator;
			return result;
		}

		public static string GetFormatedLong(long l)
		{
			return l.ToString("#,0", CultureInfo.InvariantCulture);
		}

		public static string GetFormatedDateTimeOffset(DateTimeOffset dt)
		{
			return dt.ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
		}

		public static string GetFormatedDateTime(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
		}

		public static string GetFormatedDateTimeForFilename(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd-HH-mm-ss-fff", DateTimeFormatInfo.InvariantInfo);
		}

		public static string GetFormatedPriceEUR(decimal num)
		{
			return GetFormatedDecimal(num, 2) + " €";
		}

		public static string GetFormatedPriceUSD(decimal num)
		{
			return "$" + GetFormatedDecimal(num, 2);
		}

		public static string GetFormatedPriceUSD(decimal priceInEur, decimal exchangeRateEurToUsd)
		{
			return "~" + GetFormatedDecimal(decimal.Round(exchangeRateEurToUsd * priceInEur, 2), 2) + " $";
		}

		public static string GetFormatedInt(int i)
		{
			return i.ToString("#,0", CultureInfo.InvariantCulture);
		}

		public static string GetFormatedArray<T>(T[] array)
		{
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString();
				if (i < array.Length - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		public static void AppendFormattedValueInBestUnit(float value, string[] unitNames, float[] unitMultipliers, int unitDecimalDigits, StringBuilder output)
		{
			float num = Math.Abs(value);
			int i;
			for (i = 1; i < unitMultipliers.Length && !(num < unitMultipliers[i]); i++)
			{
			}
			i--;
			value /= unitMultipliers[i];
			output.AppendDecimal(Math.Round(value, unitDecimalDigits), unitDecimalDigits);
			output.Append(' ').Append(unitNames[i]);
		}

		public static void AppendFormattedValueInBestUnit(float value, string[] unitNames, float[] unitMultipliers, int[] unitDecimalDigits, StringBuilder output)
		{
			if (float.IsInfinity(value))
			{
				output.Append('-');
				return;
			}
			float num = Math.Abs(value);
			int i;
			for (i = 1; i < unitMultipliers.Length && !(num < unitMultipliers[i]); i++)
			{
			}
			i--;
			value /= unitMultipliers[i];
			output.AppendDecimal(Math.Round(value, unitDecimalDigits[i]), unitDecimalDigits[i]);
			output.Append(' ').Append((object)MyTexts.Get(MyStringId.GetOrCompute(unitNames[i])));
		}

		public static void AppendGenericInBestUnit(float genericInBase, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(genericInBase, m_genericUnitNames, m_genericUnitMultipliers, m_genericUnitDigits, output);
		}

		public static void AppendGenericInBestUnit(float genericInBase, int numDecimals, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(genericInBase, m_genericUnitNames, m_genericUnitMultipliers, numDecimals, output);
		}

		public static void AppendForceInBestUnit(float forceInNewtons, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(forceInNewtons, m_forceUnitNames, m_forceUnitMultipliers, m_forceUnitDigits, output);
		}

		public static void AppendTorqueInBestUnit(float torqueInNewtonMeters, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(torqueInNewtonMeters, m_torqueUnitNames, m_torqueUnitMultipliers, m_torqueUnitDigits, output);
		}

		public static void AppendWorkInBestUnit(float workInMegaWatts, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(workInMegaWatts, m_workUnitNames, m_workUnitMultipliers, m_workUnitDigits, output);
		}

		public static void AppendWorkHoursInBestUnit(float workInMegaWatts, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(workInMegaWatts, m_workHoursUnitNames, m_workHoursUnitMultipliers, m_workHoursUnitDigits, output);
		}

		public static void AppendTimeInBestUnit(float timeInSeconds, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(timeInSeconds, m_timeUnitNames, m_timeUnitMultipliers, m_timeUnitDigits, output);
		}

		public static void AppendWeightInBestUnit(float weightInKG, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(weightInKG, m_weightUnitNames, m_weightUnitMultipliers, m_weightUnitDigits, output);
		}

		public static void AppendVolumeInBestUnit(float volumeInCubicMeters, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(volumeInCubicMeters, m_volumeUnitNames, m_volumeUnitMultipliers, m_volumeUnitDigits, output);
		}

		public static string GetFormattedGasAmount(int amount)
		{
			return GetFormatedInt(amount) + " kL";
		}

		public static string GetFormattedPiecesAmount(int amount)
		{
			return GetFormatedInt(amount) + " pcs";
		}

		public static string GetFormattedOreAmount(int amount)
		{
			return GetFormatedInt(amount) + " " + m_weightUnitNames[1];
		}

		public static void AppendTimeExact(int timeInSeconds, StringBuilder output)
		{
			if (timeInSeconds >= 86400)
			{
				output.Append(timeInSeconds / 86400);
				output.Append("d ");
			}
			output.ConcatFormat("{0:00}", timeInSeconds / 3600 % 24);
			output.Append(":");
			output.ConcatFormat("{0:00}", timeInSeconds / 60 % 60);
			output.Append(":");
			output.ConcatFormat("{0:00}", timeInSeconds % 60);
		}

		public static void AppendTimeExactMinSec(int timeInSeconds, StringBuilder output)
		{
			output.ConcatFormat("{0:00}", timeInSeconds / 60 % 1440);
			output.Append(":");
			output.ConcatFormat("{0:00}", timeInSeconds % 60);
		}

		public static void AppendDistanceInBestUnit(float distanceInMeters, StringBuilder output)
		{
			AppendFormattedValueInBestUnit(distanceInMeters, m_distanceUnitNames, m_distanceUnitMultipliers, m_distanceUnitDigits, output);
		}
	}
}
