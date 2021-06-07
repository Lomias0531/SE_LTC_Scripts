using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using VRage.Game.Components.Session;
using VRage.Game.VisualScripting.Utils;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting
{
	public static class MyVisualScriptLogicProvider
	{
		private const int m_cornflower = -10185235;

		private const int m_slateBlue = -9807155;

		[Display(Name = "Mission", Description = "When mission starts.")]
		public static SingleKeyMissionEvent MissionStarted;

		[Display(Name = "Mission", Description = "When mission finishes.")]
		public static SingleKeyMissionEvent MissionFinished;

		public static void Init()
		{
			MyVisualScriptingProxy.WhitelistExtensions(typeof(MyVSCollectionExtensions));
			Type typeFromHandle = typeof(List<>);
			MyVisualScriptingProxy.WhitelistMethod(typeFromHandle.GetMethod("Insert"), sequenceDependent: true);
			MyVisualScriptingProxy.WhitelistMethod(typeFromHandle.GetMethod("RemoveAt"), sequenceDependent: true);
			MyVisualScriptingProxy.WhitelistMethod(typeFromHandle.GetMethod("Clear"), sequenceDependent: true);
			MyVisualScriptingProxy.WhitelistMethod(typeFromHandle.GetMethod("Add"), sequenceDependent: true);
			MyVisualScriptingProxy.WhitelistMethod(typeFromHandle.GetMethod("Remove"), sequenceDependent: true);
			MyVisualScriptingProxy.WhitelistMethod(typeFromHandle.GetMethod("Contains"), sequenceDependent: false);
			MyVisualScriptingProxy.WhitelistMethod(typeof(string).GetMethod("Substring", new Type[2]
			{
				typeof(int),
				typeof(int)
			}), sequenceDependent: true);
		}

		[VisualScriptingMiscData("Shared Storage", "Stores string in the shared storage. This value is accessible from all scripts.", -10185235)]
		[VisualScriptingMember(true, false)]
		public static void StoreString(string key, string value)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				MySessionComponentScriptSharedStorage.Instance.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Shared Storage", "Stores boolean in the shared storage. This value is accessible from all scripts.", -10185235)]
		[VisualScriptingMember(true, false)]
		public static void StoreBool(string key, bool value)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				MySessionComponentScriptSharedStorage.Instance.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Shared Storage", "Stores integer in the shared storage. This value is accessible from all scripts.", -10185235)]
		[VisualScriptingMember(true, false)]
		public static void StoreInteger(string key, int value)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				MySessionComponentScriptSharedStorage.Instance.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Shared Storage", "Stores long integer in the shared storage. This value is accessible from all scripts.", -10185235)]
		[VisualScriptingMember(true, false)]
		public static void StoreLong(string key, long value)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				MySessionComponentScriptSharedStorage.Instance.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Shared Storage", "Stores float in the shared storage. This value is accessible from all scripts.", -10185235)]
		[VisualScriptingMember(true, false)]
		public static void StoreFloat(string key, float value)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				MySessionComponentScriptSharedStorage.Instance.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Shared Storage", "Stores Vector3 (doubles) in the shared storage. This value is accessible from all scripts.", -10185235)]
		[VisualScriptingMember(true, false)]
		public static void StoreVector(string key, Vector3D value)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				MySessionComponentScriptSharedStorage.Instance.Write(key, value);
			}
		}

		[VisualScriptingMiscData("Shared Storage", "Loads string from the shared storage.", -9807155)]
		[VisualScriptingMember(false, false)]
		public static string LoadString(string key)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				return MySessionComponentScriptSharedStorage.Instance.ReadString(key);
			}
			return null;
		}

		[VisualScriptingMiscData("Shared Storage", "Loads boolean from the shared storage.", -9807155)]
		[VisualScriptingMember(false, false)]
		public static bool LoadBool(string key)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				return MySessionComponentScriptSharedStorage.Instance.ReadBool(key);
			}
			return false;
		}

		[VisualScriptingMiscData("Shared Storage", "Loads integer from the shared storage.", -9807155)]
		[VisualScriptingMember(false, false)]
		public static int LoadInteger(string key)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				return MySessionComponentScriptSharedStorage.Instance.ReadInt(key);
			}
			return 0;
		}

		[VisualScriptingMiscData("Shared Storage", "Loads long integer from the shared storage.", -9807155)]
		[VisualScriptingMember(false, false)]
		public static long LoadLong(string key)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				return MySessionComponentScriptSharedStorage.Instance.ReadLong(key);
			}
			return 0L;
		}

		[VisualScriptingMiscData("Shared Storage", "Loads float from the shared storage.", -9807155)]
		[VisualScriptingMember(false, false)]
		public static float LoadFloat(string key)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				return MySessionComponentScriptSharedStorage.Instance.ReadFloat(key);
			}
			return 0f;
		}

		[VisualScriptingMiscData("Shared Storage", "Loads Vector3 (doubles) from the shared storage.", -9807155)]
		[VisualScriptingMember(false, false)]
		public static Vector3D LoadVector(string key)
		{
			if (MySessionComponentScriptSharedStorage.Instance != null)
			{
				return MySessionComponentScriptSharedStorage.Instance.ReadVector3D(key);
			}
			return Vector3D.Zero;
		}

		[VisualScriptingMiscData("Input", "Enables/Disables input control blacklist state.", -10510688)]
		[VisualScriptingMember(true, false)]
		public static void SetLocalInputBlacklistState(string controlStringId, bool enabled = false)
		{
			MyInput.Static.SetControlBlock(MyStringId.GetOrCompute(controlStringId), !enabled);
		}

		[VisualScriptingMiscData("Input", "Checks if input control is blacklisted.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsLocalInputBlacklisted(string controlStringId)
		{
			return MyInput.Static.IsControlBlocked(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control has been pressed this frame.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsNewGameControlPressed(string controlStringId)
		{
			return MyInput.Static.IsNewGameControlPressed(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control is currently pressed.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGameControlPressed(string controlStringId)
		{
			return MyInput.Static.IsGameControlPressed(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control has been released this frame.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsNewGameControlReleased(string controlStringId)
		{
			return MyInput.Static.IsNewGameControlReleased(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Checks if input control is currently released.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool IsGameControlReleased(string controlStringId)
		{
			return MyInput.Static.IsGameControlReleased(MyStringId.GetOrCompute(controlStringId));
		}

		[VisualScriptingMiscData("Input", "Returns X-coordinate of mouse position.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetMouseX()
		{
			return MyInput.Static.GetMouseXForGamePlayF();
		}

		[VisualScriptingMiscData("Input", "Returns Y-coordinate of mouse position.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float GetMouseY()
		{
			return MyInput.Static.GetMouseYForGamePlayF();
		}

		[VisualScriptingMiscData("String", "Gets substring of the specified string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string SubString(string value, int startIndex = 0, int length = 0)
		{
			if (value != null)
			{
				if (length > 0)
				{
					return value.Substring(startIndex, length);
				}
				return value.Substring(startIndex);
			}
			return null;
		}

		[VisualScriptingMiscData("String", "Gets length of the specified string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int StringLength(string value)
		{
			return value?.Length ?? 0;
		}

		[VisualScriptingMiscData("String", "Checks if string is null or empty.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool StringIsNullOrEmpty(string value)
		{
			if (value != null)
			{
				return value.Length == 0;
			}
			return true;
		}

		[VisualScriptingMiscData("String", "Replaces old value with the new value in the specified string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string StringReplace(string value, string oldValue, string newValue)
		{
			return value?.Replace(oldValue, newValue);
		}

		[VisualScriptingMiscData("String", "Converts int to string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public unsafe static string IntToString(int value)
		{
			return ((int*)(&value))->ToString();
		}

		[VisualScriptingMiscData("String", "Converts float to string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string FloatToString(float value)
		{
			return value.ToString();
		}

		[VisualScriptingMiscData("String", "Converts long integer to string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string LongToString(long value)
		{
			return value.ToString();
		}

		[VisualScriptingMiscData("String", "Converts Vector3 (doubles) to string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string Vector3DToString(Vector3D value)
		{
			return value.ToString();
		}

		[VisualScriptingMiscData("String", "Checks if string starts with another string (Invariant Culture).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool StringStartsWith(string value, string starting, bool ignoreCase = true)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			return value.StartsWith(starting, ignoreCase, CultureInfo.InvariantCulture);
		}

		[VisualScriptingMiscData("String", "Concatenates two strings.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static string StringConcat(string a, string b)
		{
			return a + b;
		}

		[VisualScriptingMiscData("String", "Checks if value contains another string.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static bool StringContains(string value, string contains)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			return value.Contains(contains);
		}

		[VisualScriptingMiscData("Math", "Rounds float value to int.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int Round(float value)
		{
			return (int)Math.Round(value);
		}

		[VisualScriptingMiscData("Math", "Calculates direction vector from the speed.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D DirectionVector(Vector3D speed)
		{
			if (speed == Vector3D.Zero)
			{
				return Vector3D.Forward;
			}
			return Vector3D.Normalize(speed);
		}

		[VisualScriptingMiscData("Math", "Calculates modulo of the number.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int Modulo(int number, int mod)
		{
			return number % mod;
		}

		[VisualScriptingMiscData("Math", "Calculates vector length.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float VectorLength(Vector3D speed)
		{
			return (float)speed.Length();
		}

		[VisualScriptingMiscData("Math", "Calculates ceiling function of the value.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int Ceil(float value)
		{
			return (int)Math.Ceiling(value);
		}

		[VisualScriptingMiscData("Math", "Calculates floor function of the value.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int Floor(float value)
		{
			return (int)Math.Floor(value);
		}

		[VisualScriptingMiscData("Math", "Calculates abs function of the value.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float Abs(float value)
		{
			return Math.Abs(value);
		}

		[VisualScriptingMiscData("Math", "Calculates minimum of the values.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float Min(float value1, float value2)
		{
			return Math.Min(value1, value2);
		}

		[VisualScriptingMiscData("Math", "Calculates maximum of the values.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float Max(float value1, float value2)
		{
			return Math.Max(value1, value2);
		}

		[VisualScriptingMiscData("Math", "Clamps the value.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float Clamp(float value, float min, float max)
		{
			return MathHelper.Clamp(value, min, max);
		}

		[VisualScriptingMiscData("Math", "Calculates distance of two vectors.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float DistanceVector3D(Vector3D posA, Vector3D posB)
		{
			return (float)Vector3D.Distance(posA, posB);
		}

		[VisualScriptingMiscData("Math", "Calculates distance of two vectors.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float DistanceVector3(Vector3 posA, Vector3 posB)
		{
			return Vector3.Distance(posA, posB);
		}

		[VisualScriptingMiscData("Math", "Creates Vector3 (double) value.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static Vector3D CreateVector3D(float x = 0f, float y = 0f, float z = 0f)
		{
			return new Vector3D(x, y, z);
		}

		[VisualScriptingMiscData("Math", "Gets X, Y, Z of the vector.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static void GetVector3DComponents(Vector3D vector, out float x, out float y, out float z)
		{
			x = (float)vector.X;
			y = (float)vector.Y;
			z = (float)vector.Z;
		}

		[VisualScriptingMiscData("Math", "Generates random float.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static float RandomFloat(float min, float max)
		{
			return MyUtils.GetRandomFloat(min, max);
		}

		[VisualScriptingMiscData("Math", "Generates random int.", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int RandomInt(int min, int max)
		{
			return MyUtils.GetRandomInt(min, max);
		}

		[VisualScriptingMiscData("Math", "Adds two numbers (integers).", -10510688)]
		[VisualScriptingMember(false, false)]
		public static int AddInt(int a, int b)
		{
			return a + b;
		}
	}
}
