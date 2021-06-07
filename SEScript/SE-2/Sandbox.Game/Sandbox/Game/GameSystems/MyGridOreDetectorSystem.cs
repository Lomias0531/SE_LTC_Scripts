using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;

namespace Sandbox.Game.GameSystems
{
	public class MyGridOreDetectorSystem
	{
		public struct RegisteredOreDetectorData
		{
			public readonly MyCubeBlock Block;

			public readonly MyOreDetectorComponent Component;

			public RegisteredOreDetectorData(MyCubeBlock block, MyOreDetectorComponent comp)
			{
				this = default(RegisteredOreDetectorData);
				Block = block;
				Component = comp;
			}

			public override int GetHashCode()
			{
				return Block.EntityId.GetHashCode();
			}
		}

		private readonly MyCubeGrid m_cubeGrid;

		private readonly HashSet<RegisteredOreDetectorData> m_oreDetectors = new HashSet<RegisteredOreDetectorData>();

		public HashSetReader<RegisteredOreDetectorData> OreDetectors => new HashSetReader<RegisteredOreDetectorData>(m_oreDetectors);

		public MyGridOreDetectorSystem(MyCubeGrid cubeGrid)
		{
			m_cubeGrid = cubeGrid;
			m_cubeGrid.OnFatBlockAdded += CubeGridOnOnFatBlockAdded;
			m_cubeGrid.OnFatBlockRemoved += CubeGridOnOnFatBlockRemoved;
		}

		private void CubeGridOnOnFatBlockRemoved(MyCubeBlock block)
		{
			IMyComponentOwner<MyOreDetectorComponent> myComponentOwner = block as IMyComponentOwner<MyOreDetectorComponent>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out MyOreDetectorComponent component))
			{
				m_oreDetectors.Remove(new RegisteredOreDetectorData(block, component));
			}
		}

		private void CubeGridOnOnFatBlockAdded(MyCubeBlock block)
		{
			IMyComponentOwner<MyOreDetectorComponent> myComponentOwner = block as IMyComponentOwner<MyOreDetectorComponent>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out MyOreDetectorComponent component))
			{
				m_oreDetectors.Add(new RegisteredOreDetectorData(block, component));
			}
		}
	}
}
