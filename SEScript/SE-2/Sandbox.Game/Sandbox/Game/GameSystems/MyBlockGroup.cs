using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyBlockGroup : Sandbox.ModAPI.IMyBlockGroup, Sandbox.ModAPI.Ingame.IMyBlockGroup
	{
		public StringBuilder Name = new StringBuilder();

		internal readonly HashSet<MyTerminalBlock> Blocks = new HashSet<MyTerminalBlock>();

		string Sandbox.ModAPI.Ingame.IMyBlockGroup.Name => Name.ToString();

		internal MyBlockGroup()
		{
		}

		internal void Init(MyCubeGrid grid, MyObjectBuilder_BlockGroup builder)
		{
			Name.Clear().Append(builder.Name);
			foreach (Vector3I block in builder.Blocks)
			{
				MySlimBlock cubeBlock = grid.GetCubeBlock(block);
				if (cubeBlock != null)
				{
					MyTerminalBlock myTerminalBlock = cubeBlock.FatBlock as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						Blocks.Add(myTerminalBlock);
					}
				}
			}
		}

		internal MyObjectBuilder_BlockGroup GetObjectBuilder()
		{
			MyObjectBuilder_BlockGroup myObjectBuilder_BlockGroup = new MyObjectBuilder_BlockGroup();
			myObjectBuilder_BlockGroup.Name = Name.ToString();
			foreach (MyTerminalBlock block in Blocks)
			{
				myObjectBuilder_BlockGroup.Blocks.Add(block.Position);
			}
			return myObjectBuilder_BlockGroup;
		}

		public override string ToString()
		{
			return $"{Name} - {Blocks.Count} blocks";
		}

		void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocks(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in Blocks)
			{
				if (block.IsAccessibleForProgrammableBlock && (collect == null || collect(block)))
				{
					blocks?.Add(block);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocksOfType<T>(List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in Blocks)
			{
				if (block as T != null && block.IsAccessibleForProgrammableBlock && (collect == null || collect(block)))
				{
					blocks?.Add(block);
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in Blocks)
			{
				T val = block as T;
				if (val != null && block.IsAccessibleForProgrammableBlock && (collect == null || collect(val)))
				{
					blocks?.Add(val);
				}
			}
		}

		void Sandbox.ModAPI.IMyBlockGroup.GetBlocks(List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in Blocks)
			{
				if (collect == null || collect(block))
				{
					blocks?.Add(block);
				}
			}
		}

		void Sandbox.ModAPI.IMyBlockGroup.GetBlocksOfType<T>(List<Sandbox.ModAPI.IMyTerminalBlock> blocks, Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
		{
			blocks?.Clear();
			foreach (MyTerminalBlock block in Blocks)
			{
				if (block as T != null && (collect == null || collect(block)))
				{
					blocks?.Add(block);
				}
			}
		}
	}
}
