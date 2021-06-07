using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyOxygenRoom
	{
		public int Index
		{
			get;
			set;
		}

		public bool IsAirtight
		{
			get;
			set;
		}

		public float EnvironmentOxygen
		{
			get;
			set;
		}

		public float OxygenAmount
		{
			get;
			set;
		}

		public int BlockCount
		{
			get;
			set;
		}

		public int DepressurizationTime
		{
			get;
			set;
		}

		[XmlIgnore]
		public MyOxygenRoomLink Link
		{
			get;
			set;
		}

		public bool IsDirty
		{
			get;
			set;
		}

		public HashSet<Vector3I> Blocks
		{
			get;
			set;
		}

		public Vector3I StartingPosition
		{
			get;
			set;
		}

		public MyOxygenRoom()
		{
			IsAirtight = true;
		}

		public MyOxygenRoom(int index)
		{
			IsAirtight = true;
			EnvironmentOxygen = 0f;
			Index = index;
			OxygenAmount = 0f;
			BlockCount = 0;
			DepressurizationTime = 0;
		}

		public float OxygenLevel(float gridSize)
		{
			return OxygenAmount / MaxOxygen(gridSize);
		}

		public float MissingOxygen(float gridSize)
		{
			return (float)Math.Max(MaxOxygen(gridSize) - OxygenAmount, 0.0);
		}

		public float MaxOxygen(float gridSize)
		{
			return (float)BlockCount * gridSize * gridSize * gridSize;
		}
	}
}
