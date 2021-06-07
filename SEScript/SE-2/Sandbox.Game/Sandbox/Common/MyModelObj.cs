using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Common
{
	internal class MyModelObj
	{
		public List<Vector3> Vertexes;

		public List<Vector3> Normals;

		public List<MyTriangleVertexIndices> Triangles;

		public MyModelObj(string filename)
		{
			Vertexes = new List<Vector3>();
			Normals = new List<Vector3>();
			Triangles = new List<MyTriangleVertexIndices>();
			foreach (string[] lineToken in GetLineTokens(filename))
			{
				ParseObjLine(lineToken);
			}
		}

		private IEnumerable<string[]> GetLineTokens(string filename)
		{
			using (StreamReader reader = new StreamReader(filename))
			{
				int lineNumber = 1;
				while (!reader.EndOfStream)
				{
					string[] array = Regex.Split(reader.ReadLine().Trim(), "\\s+");
					if (array.Length != 0 && array[0] != string.Empty && !array[0].StartsWith("#"))
					{
						yield return array;
					}
					lineNumber++;
				}
			}
		}

		private void ParseObjLine(string[] lineTokens)
		{
			switch (lineTokens[0].ToLower())
			{
			case "v":
				Vertexes.Add(ParseVector3(lineTokens));
				break;
			case "vn":
				Normals.Add(ParseVector3(lineTokens));
				break;
			case "f":
			{
				int[] array = new int[3];
				for (int i = 1; i <= 3; i++)
				{
					string[] array2 = lineTokens[i].Split(new char[1]
					{
						'/'
					});
					if (array2.Length != 0)
					{
						array[i - 1] = int.Parse(array2[0], CultureInfo.InvariantCulture);
					}
				}
				Triangles.Add(new MyTriangleVertexIndices(array[0] - 1, array[1] - 1, array[2] - 1));
				break;
			}
			}
		}

		private static Vector3 ParseVector3(string[] lineTokens)
		{
			return new Vector3(float.Parse(lineTokens[1], CultureInfo.InvariantCulture), float.Parse(lineTokens[2], CultureInfo.InvariantCulture), float.Parse(lineTokens[3], CultureInfo.InvariantCulture));
		}
	}
}
