using System;

namespace VRage.Library.Extensions
{
	public static class MyArrayHelpers
	{
		public static void ResizeNoCopy<T>(ref T[] array, int newSize)
		{
			if (array == null || array.Length != newSize)
			{
				array = new T[newSize];
			}
		}

		public static void Reserve<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
		{
			if (array.Length < size)
			{
				int num = (size == 0) ? 1 : size;
				Array.Resize(ref array, (num < threshold) ? (num * 2) : ((int)((float)num * allocScale)));
			}
		}

		public static void ReserveNoCopy<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
		{
			if (array.Length < size)
			{
				int num = (size == 0) ? 1 : size;
				array = new T[(num < threshold) ? (num * 2) : ((int)((float)num * allocScale))];
			}
		}

		public static void InitOrReserve<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
		{
			if (array == null)
			{
				array = new T[size];
			}
			else
			{
				Reserve(ref array, size, threshold, allocScale);
			}
		}

		public static void InitOrReserveNoCopy<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
		{
			if (array == null)
			{
				array = new T[size];
			}
			else
			{
				ReserveNoCopy(ref array, size, threshold, allocScale);
			}
		}
	}
}
