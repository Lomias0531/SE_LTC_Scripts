using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[PreloadRequired]
	public static class MyCubeGridDefinitions
	{
		public class TableEntry
		{
			public MyRotationOptionsEnum RotationOptions;

			public MyTileDefinition[] Tiles;

			public MyEdgeDefinition[] Edges;
		}

		public static readonly Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>> TileGridOrientations;

		public static readonly Dictionary<Vector3I, MyEdgeOrientationInfo> EdgeOrientations;

		private static TableEntry[] m_tileTable;

		private static MatrixI[] m_allPossible90rotations;

		private static MatrixI[][] m_uniqueTopologyRotationTable;

		public static MatrixI[] AllPossible90rotations => m_allPossible90rotations;

		static MyCubeGridDefinitions()
		{
			Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>> dictionary = new Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>>();
			MyStringId orCompute = MyStringId.GetOrCompute("Square");
			Dictionary<Vector3I, MyTileDefinition> dictionary2 = new Dictionary<Vector3I, MyTileDefinition>();
			Vector3I up = Vector3I.Up;
			MyTileDefinition myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true
			};
			dictionary2.Add(up, myTileDefinition);
			Vector3I forward = Vector3I.Forward;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true
			};
			dictionary2.Add(forward, myTileDefinition);
			Vector3I backward = Vector3I.Backward;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true
			};
			dictionary2.Add(backward, myTileDefinition);
			Vector3I down = Vector3I.Down;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true
			};
			dictionary2.Add(down, myTileDefinition);
			Vector3I right = Vector3I.Right;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true
			};
			dictionary2.Add(right, myTileDefinition);
			Vector3I left = Vector3I.Left;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true
			};
			dictionary2.Add(left, myTileDefinition);
			dictionary.Add(orCompute, dictionary2);
			MyStringId orCompute2 = MyStringId.GetOrCompute("Slope");
			Dictionary<Vector3I, MyTileDefinition> dictionary3 = new Dictionary<Vector3I, MyTileDefinition>();
			Vector3I key = new Vector3I(0, 1, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			dictionary3.Add(key, myTileDefinition);
			Vector3I key2 = new Vector3I(0, 1, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, -1f)),
				IsEmpty = true
			};
			dictionary3.Add(key2, myTileDefinition);
			Vector3I key3 = new Vector3I(-1, 1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key3, myTileDefinition);
			Vector3I key4 = new Vector3I(1, 1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key4, myTileDefinition);
			Vector3I key5 = new Vector3I(0, -1, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			dictionary3.Add(key5, myTileDefinition);
			Vector3I key6 = new Vector3I(0, -1, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, -1f)),
				IsEmpty = true
			};
			dictionary3.Add(key6, myTileDefinition);
			Vector3I key7 = new Vector3I(-1, -1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key7, myTileDefinition);
			Vector3I key8 = new Vector3I(1, -1, 0);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key8, myTileDefinition);
			Vector3I key9 = new Vector3I(-1, 0, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Forward),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			dictionary3.Add(key9, myTileDefinition);
			Vector3I key10 = new Vector3I(-1, 0, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Backward),
				Normal = Vector3.Normalize(new Vector3(0f, 1f, -1f)),
				IsEmpty = true
			};
			dictionary3.Add(key10, myTileDefinition);
			Vector3I key11 = new Vector3I(1, 0, -1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Forward),
				Normal = Vector3.Normalize(new Vector3(1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key11, myTileDefinition);
			Vector3I key12 = new Vector3I(1, 0, 1);
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Backward),
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0f)),
				IsEmpty = true
			};
			dictionary3.Add(key12, myTileDefinition);
			dictionary.Add(orCompute2, dictionary3);
			TileGridOrientations = dictionary;
			EdgeOrientations = new Dictionary<Vector3I, MyEdgeOrientationInfo>(new Vector3INormalEqualityComparer())
			{
				{
					new Vector3I(0, 0, 1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(0, 0, -1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(1, 0, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY(MathF.E * 449f / 777f), MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(-1, 0, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY(MathF.E * 449f / 777f), MyCubeEdgeType.Horizontal)
				},
				{
					new Vector3I(0, 1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX(MathF.E * 449f / 777f), MyCubeEdgeType.Vertical)
				},
				{
					new Vector3I(0, -1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX(MathF.E * 449f / 777f), MyCubeEdgeType.Vertical)
				},
				{
					new Vector3I(-1, 0, -1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ(MathF.E * 449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(1, 0, 1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ(MathF.E * 449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(-1, 0, 1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ(MathF.E * -449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(1, 0, -1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationZ(MathF.E * -449f / 777f), MyCubeEdgeType.Horizontal_Diagonal)
				},
				{
					new Vector3I(0, 1, -1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(0, -1, 1),
					new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(-1, -1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY(MathF.E * -449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(0, -1, -1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX(MathF.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(1, -1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY(MathF.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(-1, 1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY(MathF.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(1, 1, 0),
					new MyEdgeOrientationInfo(Matrix.CreateRotationY(MathF.E * -449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				},
				{
					new Vector3I(0, 1, 1),
					new MyEdgeOrientationInfo(Matrix.CreateRotationX(MathF.E * 449f / 777f), MyCubeEdgeType.Vertical_Diagonal)
				}
			};
			TableEntry[] array = new TableEntry[19];
			TableEntry tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.None
			};
			TableEntry tableEntry2 = tableEntry;
			MyTileDefinition[] array2 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array2[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array2[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array2[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array2[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array2[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array2[5] = myTileDefinition;
			tableEntry2.Tiles = array2;
			TableEntry tableEntry3 = tableEntry;
			MyEdgeDefinition[] array3 = new MyEdgeDefinition[12];
			MyEdgeDefinition myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array3[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, 1, 1),
				Side0 = 0,
				Side1 = 5
			};
			array3[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 4
			};
			array3[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array3[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array3[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array3[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array3[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array3[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array3[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array3[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 2
			};
			array3[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 4,
				Side1 = 2
			};
			array3[11] = myEdgeDefinition;
			tableEntry3.Edges = array3;
			array[0] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry4 = tableEntry;
			MyTileDefinition[] array4 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true,
				Id = MyStringId.GetOrCompute("Slope")
			};
			array4[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -1f, -1f)
			};
			array4[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array4[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array4[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array4[4] = myTileDefinition;
			tableEntry4.Tiles = array4;
			TableEntry tableEntry5 = tableEntry;
			MyEdgeDefinition[] array5 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 4
			};
			array5[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 0,
				Side1 = 1
			};
			array5[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array5[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array5[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array5[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array5[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array5[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array5[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array5[8] = myEdgeDefinition;
			tableEntry5.Edges = array5;
			array[1] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry6 = tableEntry;
			MyTileDefinition[] array6 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -1f, 0f)
			};
			array6[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array6[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
				IsEmpty = true
			};
			array6[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f)
			};
			array6[3] = myTileDefinition;
			tableEntry6.Tiles = array6;
			TableEntry tableEntry7 = tableEntry;
			MyEdgeDefinition[] array7 = new MyEdgeDefinition[6];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array7[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 3
			};
			array7[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 2
			};
			array7[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 1
			};
			array7[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array7[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array7[5] = myEdgeDefinition;
			tableEntry7.Edges = array7;
			array[2] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry8 = tableEntry;
			MyTileDefinition[] array8 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(1f, -1f, -1f)),
				IsEmpty = true
			};
			array8[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, 1f, 1f)
			};
			array8[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(-1f, 1f, 0f)
			};
			array8[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(-1f, 0f, 1f)
			};
			array8[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array8[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array8[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(MathF.E * 449f / 777f),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array8[6] = myTileDefinition;
			tableEntry8.Tiles = array8;
			TableEntry tableEntry9 = tableEntry;
			MyEdgeDefinition[] array9 = new MyEdgeDefinition[12];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array9[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 5
			};
			array9[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 0
			};
			array9[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 1
			};
			array9[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 6,
				Side1 = 1
			};
			array9[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array9[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array9[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 6,
				Side1 = 3
			};
			array9[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 5,
				Side1 = 3
			};
			array9[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 6
			};
			array9[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 5,
				Side1 = 4
			};
			array9[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 6
			};
			array9[11] = myEdgeDefinition;
			tableEntry9.Edges = array9;
			array[3] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry10 = tableEntry;
			MyTileDefinition[] array10 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true
			};
			array10[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true
			};
			array10[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true
			};
			array10[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true
			};
			array10[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true
			};
			array10[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true
			};
			array10[5] = myTileDefinition;
			tableEntry10.Tiles = array10;
			TableEntry tableEntry11 = tableEntry;
			MyEdgeDefinition[] array11 = new MyEdgeDefinition[12];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array11[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, 1, 1),
				Side0 = 0,
				Side1 = 5
			};
			array11[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 4
			};
			array11[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array11[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array11[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array11[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array11[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array11[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array11[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array11[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 2
			};
			array11[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 4,
				Side1 = 2
			};
			array11[11] = myEdgeDefinition;
			tableEntry11.Edges = array11;
			array[4] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry12 = tableEntry;
			MyTileDefinition[] array12 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = true
			};
			array12[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true
			};
			array12[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = true
			};
			array12[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true
			};
			array12[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = true
			};
			array12[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = true
			};
			array12[5] = myTileDefinition;
			tableEntry12.Tiles = array12;
			TableEntry tableEntry13 = tableEntry;
			MyEdgeDefinition[] array13 = new MyEdgeDefinition[12];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array13[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, 1, 1),
				Side0 = 0,
				Side1 = 5
			};
			array13[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 4
			};
			array13[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array13[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array13[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array13[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array13[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array13[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array13[8] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array13[9] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 2
			};
			array13[10] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 4,
				Side1 = 2
			};
			array13[11] = myEdgeDefinition;
			tableEntry13.Edges = array13;
			array[5] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry14 = tableEntry;
			MyTileDefinition[] array14 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true,
				Id = MyStringId.GetOrCompute("Slope")
			};
			array14[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -1f, -1f),
				IsRounded = true
			};
			array14[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f),
				IsRounded = true
			};
			array14[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array14[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array14[4] = myTileDefinition;
			tableEntry14.Tiles = array14;
			TableEntry tableEntry15 = tableEntry;
			MyEdgeDefinition[] array15 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 4
			};
			array15[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array15[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array15[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array15[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array15[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array15[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array15[6] = myEdgeDefinition;
			tableEntry15.Edges = array15;
			array[6] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry16 = tableEntry;
			MyTileDefinition[] array16 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -1f, 0f),
				IsRounded = true
			};
			array16[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f),
				IsRounded = true
			};
			array16[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
				IsEmpty = true
			};
			array16[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f),
				IsRounded = true
			};
			array16[3] = myTileDefinition;
			tableEntry16.Tiles = array16;
			TableEntry tableEntry17 = tableEntry;
			MyEdgeDefinition[] array17 = new MyEdgeDefinition[3];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array17[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 3
			};
			array17[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array17[2] = myEdgeDefinition;
			tableEntry17.Edges = array17;
			array[7] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry18 = tableEntry;
			MyTileDefinition[] array18 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(-1f, 0f, 1f),
				IsRounded = true
			};
			array18[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, 1f, 1f),
				IsRounded = true
			};
			array18[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(-1f, 1f, 0f),
				IsRounded = true
			};
			array18[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array18[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array18[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(MathF.E * 449f / 777f),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array18[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(1f, -1f, -1f)),
				IsEmpty = true
			};
			array18[6] = myTileDefinition;
			tableEntry18.Tiles = array18;
			TableEntry tableEntry19 = tableEntry;
			MyEdgeDefinition[] array19 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 3
			};
			array19[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array19[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 3,
				Side1 = 1
			};
			array19[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 5,
				Side1 = 1
			};
			array19[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 0
			};
			array19[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 4,
				Side1 = 0
			};
			array19[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 4,
				Side1 = 5
			};
			array19[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 4,
				Side1 = 3
			};
			array19[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array19[8] = myEdgeDefinition;
			tableEntry19.Edges = array19;
			array[8] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry20 = tableEntry;
			MyTileDefinition[] array20 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			array20[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -1f, -1f)
			};
			array20[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array20[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array20[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array20[4] = myTileDefinition;
			tableEntry20.Tiles = array20;
			TableEntry tableEntry21 = tableEntry;
			MyEdgeDefinition[] array21 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 4
			};
			array21[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 0,
				Side1 = 1
			};
			array21[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 2
			};
			array21[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array21[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array21[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array21[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array21[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array21[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array21[8] = myEdgeDefinition;
			tableEntry21.Edges = array21;
			array[9] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry22 = tableEntry;
			MyTileDefinition[] array22 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -1f, 0f)
			};
			array22[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, -1f)
			};
			array22[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
				IsEmpty = true
			};
			array22[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f)
			};
			array22[3] = myTileDefinition;
			tableEntry22.Tiles = array22;
			TableEntry tableEntry23 = tableEntry;
			MyEdgeDefinition[] array23 = new MyEdgeDefinition[6];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 1
			};
			array23[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 0,
				Side1 = 3
			};
			array23[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 0,
				Side1 = 2
			};
			array23[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 1
			};
			array23[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array23[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array23[5] = myEdgeDefinition;
			tableEntry23.Edges = array23;
			array[10] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry24 = tableEntry;
			MyTileDefinition[] array24 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up),
				Normal = Vector3.Normalize(new Vector3(0f, 2f, 1f)),
				IsEmpty = true
			};
			array24[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array24[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward
			};
			array24[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array24[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
				Normal = Vector3.Right,
				Up = new Vector3(0f, -2f, -1f),
				DontOffsetTexture = true
			};
			array24[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				Up = new Vector3(0f, -2f, -1f),
				DontOffsetTexture = true
			};
			array24[5] = myTileDefinition;
			tableEntry24.Tiles = array24;
			TableEntry tableEntry25 = tableEntry;
			MyEdgeDefinition[] array25 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3(-1f, 1f, -1f),
				Point1 = new Vector3(1f, 1f, -1f),
				Side0 = 0,
				Side1 = 1
			};
			array25[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 1
			};
			array25[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 3,
				Side1 = 5
			};
			array25[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 4
			};
			array25[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 3,
				Side1 = 2
			};
			array25[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 5
			};
			array25[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array25[6] = myEdgeDefinition;
			tableEntry25.Edges = array25;
			array[11] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry26 = tableEntry;
			MyTileDefinition[] array26 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 2f, 1f)),
				IsEmpty = true
			};
			array26[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, -2f, -1f),
				IsEmpty = true
			};
			array26[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -2f, -1f),
				IsEmpty = true
			};
			array26[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array26[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward
			};
			array26[4] = myTileDefinition;
			tableEntry26.Tiles = array26;
			TableEntry tableEntry27 = tableEntry;
			MyEdgeDefinition[] array27 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 0,
				Side1 = 3
			};
			array27[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array27[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array27[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 2,
				Side1 = 3
			};
			array27[3] = myEdgeDefinition;
			tableEntry27.Edges = array27;
			array[12] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry28 = tableEntry;
			MyTileDefinition[] array28 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(2f, 1f, 1f)),
				IsEmpty = true,
				DontOffsetTexture = true
			};
			array28[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				Up = new Vector3(0f, 1f, 1f),
				DontOffsetTexture = true
			};
			array28[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, 1f),
				IsEmpty = true,
				DontOffsetTexture = true
			};
			array28[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(-2f, 0f, 1f),
				DontOffsetTexture = true
			};
			array28[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				Up = new Vector3(-2f, 1f, 0f),
				DontOffsetTexture = true
			};
			array28[4] = myTileDefinition;
			tableEntry28.Tiles = array28;
			TableEntry tableEntry29 = tableEntry;
			MyEdgeDefinition[] array29 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 0,
				Side1 = 1
			};
			array29[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 4
			};
			array29[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array29[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 1,
				Side1 = 4
			};
			array29[3] = myEdgeDefinition;
			tableEntry29.Edges = array29;
			array[13] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry30 = tableEntry;
			MyTileDefinition[] array30 = new MyTileDefinition[4];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(1f, -2f, 0f),
				IsEmpty = true
			};
			array30[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Right,
				Up = new Vector3(0f, -2f, -1f),
				IsEmpty = true
			};
			array30[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(-1f, 2f, 1f)),
				IsEmpty = true
			};
			array30[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, -1f),
				IsEmpty = true
			};
			array30[3] = myTileDefinition;
			tableEntry30.Tiles = array30;
			TableEntry tableEntry31 = tableEntry;
			MyEdgeDefinition[] array31 = new MyEdgeDefinition[1];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 1,
				Side1 = 3
			};
			array31[0] = myEdgeDefinition;
			tableEntry31.Edges = array31;
			array[14] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry32 = tableEntry;
			MyTileDefinition[] array32 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(2f, -2f, -1f)),
				IsEmpty = true
			};
			array32[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, -1f, 2f)
			};
			array32[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(2f, 0f, -1f)
			};
			array32[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(1f, 0f, 2f)
			};
			array32[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array32[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array32[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(MathF.E * 449f / 777f),
				Normal = Vector3.Backward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array32[6] = myTileDefinition;
			tableEntry32.Tiles = array32;
			TableEntry tableEntry33 = tableEntry;
			MyEdgeDefinition[] array33 = new MyEdgeDefinition[9];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array33[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 5
			};
			array33[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 1
			};
			array33[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, 1),
				Point1 = new Vector3I(1, -1, 1),
				Side0 = 6,
				Side1 = 1
			};
			array33[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 6,
				Side1 = 3
			};
			array33[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 5,
				Side1 = 3
			};
			array33[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 6
			};
			array33[6] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 5,
				Side1 = 4
			};
			array33[7] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 6
			};
			array33[8] = myEdgeDefinition;
			tableEntry33.Edges = array33;
			array[15] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry34 = tableEntry;
			MyTileDefinition[] array34 = new MyTileDefinition[7];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(2f, -2f, -1f)),
				IsEmpty = true
			};
			array34[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Right,
				Up = new Vector3(0f, 1f, 1f)
			};
			array34[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				Up = new Vector3(0f, -2f, -1f)
			};
			array34[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * Matrix.CreateRotationX(3.141593f),
				Normal = Vector3.Down,
				Up = new Vector3(2f, 0f, -1f)
			};
			array34[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Up,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array34[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(MathF.E * 449f / 777f),
				Normal = Vector3.Left,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array34[5] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(MathF.E * 449f / 777f),
				Normal = Vector3.Backward
			};
			array34[6] = myTileDefinition;
			tableEntry34.Tiles = array34;
			TableEntry tableEntry35 = tableEntry;
			MyEdgeDefinition[] array35 = new MyEdgeDefinition[7];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 2,
				Side1 = 4
			};
			array35[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 2,
				Side1 = 5
			};
			array35[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, 1, -1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 1
			};
			array35[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, 1),
				Point1 = new Vector3I(-1, -1, -1),
				Side0 = 5,
				Side1 = 3
			};
			array35[3] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, -1, 1),
				Side0 = 5,
				Side1 = 6
			};
			array35[4] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 5,
				Side1 = 4
			};
			array35[5] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, 1),
				Point1 = new Vector3I(1, 1, 1),
				Side0 = 4,
				Side1 = 6
			};
			array35[6] = myEdgeDefinition;
			tableEntry35.Edges = array35;
			array[16] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Horizontal
			};
			TableEntry tableEntry36 = tableEntry;
			MyTileDefinition[] array36 = new MyTileDefinition[6];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Right),
				Normal = Vector3.Right,
				FullQuad = false
			};
			array36[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
				Normal = Vector3.Backward,
				FullQuad = false,
				IsEmpty = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array36[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up),
				Normal = Vector3.Up,
				FullQuad = false
			};
			array36[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
				Normal = Vector3.Left,
				FullQuad = false
			};
			array36[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
				Normal = Vector3.Forward,
				FullQuad = true,
				Id = MyStringId.GetOrCompute("Square")
			};
			array36[4] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Down),
				Normal = Vector3.Down,
				FullQuad = false
			};
			array36[5] = myTileDefinition;
			tableEntry36.Tiles = array36;
			TableEntry tableEntry37 = tableEntry;
			MyEdgeDefinition[] array37 = new MyEdgeDefinition[4];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 4,
				Side1 = 5
			};
			array37[0] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(-1, 1, -1),
				Side0 = 4,
				Side1 = 3
			};
			array37[1] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(1, -1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 4,
				Side1 = 0
			};
			array37[2] = myEdgeDefinition;
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, 1, -1),
				Point1 = new Vector3I(1, 1, -1),
				Side0 = 4,
				Side1 = 2
			};
			array37[3] = myEdgeDefinition;
			tableEntry37.Edges = array37;
			array[17] = tableEntry;
			tableEntry = new TableEntry
			{
				RotationOptions = MyRotationOptionsEnum.Both
			};
			TableEntry tableEntry38 = tableEntry;
			MyTileDefinition[] array38 = new MyTileDefinition[5];
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(MathF.E * -449f / 777f),
				Normal = Vector3.Forward,
				IsEmpty = true
			};
			array38[0] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Normalize(new Vector3(0f, 1f, 1f)),
				IsEmpty = true
			};
			array38[1] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.Identity,
				Normal = Vector3.Left,
				IsEmpty = true
			};
			array38[2] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationZ(3.141593f),
				Normal = Vector3.Down,
				IsEmpty = true
			};
			array38[3] = myTileDefinition;
			myTileDefinition = new MyTileDefinition
			{
				LocalMatrix = Matrix.CreateRotationX(MathF.E * -449f / 777f) * Matrix.CreateRotationY(3.141593f),
				Normal = Vector3.Right,
				IsEmpty = true
			};
			array38[4] = myTileDefinition;
			tableEntry38.Tiles = array38;
			TableEntry tableEntry39 = tableEntry;
			MyEdgeDefinition[] array39 = new MyEdgeDefinition[1];
			myEdgeDefinition = new MyEdgeDefinition
			{
				Point0 = new Vector3I(-1, -1, -1),
				Point1 = new Vector3I(1, -1, -1),
				Side0 = 3,
				Side1 = 0
			};
			array39[0] = myEdgeDefinition;
			tableEntry39.Edges = array39;
			array[18] = tableEntry;
			m_tileTable = array;
			InitTopologyUniqueRotationsMatrices();
		}

		public static MyTileDefinition[] GetCubeTiles(MyCubeBlockDefinition block)
		{
			if (block.CubeDefinition == null)
			{
				return null;
			}
			return m_tileTable[(int)block.CubeDefinition.CubeTopology].Tiles;
		}

		public static TableEntry GetTopologyInfo(MyCubeTopology topology)
		{
			return m_tileTable[(int)topology];
		}

		public static MyRotationOptionsEnum GetCubeRotationOptions(MyCubeBlockDefinition block)
		{
			if (block.CubeDefinition == null)
			{
				return MyRotationOptionsEnum.Both;
			}
			return m_tileTable[(int)block.CubeDefinition.CubeTopology].RotationOptions;
		}

		public static void GetRotatedBlockSize(MyCubeBlockDefinition block, ref Matrix rotation, out Vector3I size)
		{
			Vector3I.TransformNormal(ref block.Size, ref rotation, out size);
		}

		private static void InitTopologyUniqueRotationsMatrices()
		{
			m_allPossible90rotations = new MatrixI[24]
			{
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Forward),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Right),
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Forward),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Left),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Forward),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Down),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Backward),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Up),
				new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Forward)
			};
			m_uniqueTopologyRotationTable = new MatrixI[Enum.GetValues(typeof(MyCubeTopology)).Length][];
			m_uniqueTopologyRotationTable[0] = null;
			FillRotationsForTopology(MyCubeTopology.Slope, 0);
			FillRotationsForTopology(MyCubeTopology.Corner, 2);
			FillRotationsForTopology(MyCubeTopology.InvCorner, 0);
			FillRotationsForTopology(MyCubeTopology.StandaloneBox, -1);
			FillRotationsForTopology(MyCubeTopology.RoundedSlope, -1);
			FillRotationsForTopology(MyCubeTopology.RoundSlope, 0);
			FillRotationsForTopology(MyCubeTopology.RoundCorner, 2);
			FillRotationsForTopology(MyCubeTopology.RoundInvCorner, -1);
			FillRotationsForTopology(MyCubeTopology.RotatedSlope, -1);
			FillRotationsForTopology(MyCubeTopology.RotatedCorner, -1);
			FillRotationsForTopology(MyCubeTopology.HalfBox, 1);
			FillRotationsForTopology(MyCubeTopology.Slope2Base, -1);
			FillRotationsForTopology(MyCubeTopology.Slope2Tip, -1);
			FillRotationsForTopology(MyCubeTopology.Corner2Base, -1);
			FillRotationsForTopology(MyCubeTopology.Corner2Tip, -1);
			FillRotationsForTopology(MyCubeTopology.InvCorner2Base, -1);
			FillRotationsForTopology(MyCubeTopology.InvCorner2Tip, -1);
			FillRotationsForTopology(MyCubeTopology.HalfSlopeBox, -1);
		}

		private static void FillRotationsForTopology(MyCubeTopology topology, int mainTile)
		{
			Vector3[] array = new Vector3[m_allPossible90rotations.Length];
			m_uniqueTopologyRotationTable[(int)topology] = new MatrixI[m_allPossible90rotations.Length];
			for (int i = 0; i < m_allPossible90rotations.Length; i++)
			{
				int num = -1;
				if (mainTile != -1)
				{
					Vector3.TransformNormal(ref m_tileTable[(int)topology].Tiles[mainTile].Normal, ref m_allPossible90rotations[i], out Vector3 result);
					array[i] = result;
					for (int j = 0; j < i; j++)
					{
						if (Vector3.Dot(array[j], result) > 0.98f)
						{
							num = j;
							break;
						}
					}
				}
				if (num != -1)
				{
					m_uniqueTopologyRotationTable[(int)topology][i] = m_uniqueTopologyRotationTable[(int)topology][num];
				}
				else
				{
					m_uniqueTopologyRotationTable[(int)topology][i] = m_allPossible90rotations[i];
				}
			}
		}

		public static MyBlockOrientation GetTopologyUniqueOrientation(MyCubeTopology myCubeTopology, MyBlockOrientation orientation)
		{
			if (m_uniqueTopologyRotationTable[(int)myCubeTopology] == null)
			{
				return MyBlockOrientation.Identity;
			}
			for (int i = 0; i < m_allPossible90rotations.Length; i++)
			{
				MatrixI matrixI = m_allPossible90rotations[i];
				if (matrixI.Forward == orientation.Forward && matrixI.Up == orientation.Up)
				{
					return m_uniqueTopologyRotationTable[(int)myCubeTopology][i].GetBlockOrientation();
				}
			}
			return MyBlockOrientation.Identity;
		}
	}
}
