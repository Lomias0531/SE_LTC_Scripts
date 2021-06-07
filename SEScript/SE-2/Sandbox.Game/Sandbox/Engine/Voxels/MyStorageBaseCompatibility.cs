using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	internal class MyStorageBaseCompatibility
	{
		private const int MAX_ENCODED_NAME_LENGTH = 256;

		private readonly byte[] m_encodedNameBuffer = new byte[256];

		public MyStorageBase Compatibility_LoadCellStorage(int fileVersion, Stream stream)
		{
			Vector3I vector3I = default(Vector3I);
			vector3I.X = stream.ReadInt32();
			vector3I.Y = stream.ReadInt32();
			vector3I.Z = stream.ReadInt32();
			MyOctreeStorage myOctreeStorage = new MyOctreeStorage(null, vector3I);
			Vector3I b = default(Vector3I);
			b.X = stream.ReadInt32();
			b.Y = stream.ReadInt32();
			b.Z = stream.ReadInt32();
			Vector3I vector3I2 = vector3I / b;
			Dictionary<byte, MyVoxelMaterialDefinition> mapping = null;
			if (fileVersion == 2)
			{
				mapping = Compatibility_LoadMaterialIndexMapping(stream);
			}
			else
			{
				_ = 1;
			}
			Vector3I zero = Vector3I.Zero;
			Vector3I end = new Vector3I(7);
			MyStorageData myStorageData = new MyStorageData();
			myStorageData.Resize(Vector3I.Zero, end);
			Vector3I a = default(Vector3I);
			a.X = 0;
			Vector3I p = default(Vector3I);
			while (a.X < vector3I2.X)
			{
				a.Y = 0;
				while (a.Y < vector3I2.Y)
				{
					a.Z = 0;
					while (a.Z < vector3I2.Z)
					{
						switch (stream.ReadByteNoAlloc())
						{
						case 0:
							myStorageData.ClearContent(0);
							break;
						case 1:
							myStorageData.ClearContent(byte.MaxValue);
							break;
						case 2:
							p.X = 0;
							while (p.X < 8)
							{
								p.Y = 0;
								while (p.Y < 8)
								{
									p.Z = 0;
									while (p.Z < 8)
									{
										myStorageData.Content(ref p, stream.ReadByteNoAlloc());
										p.Z++;
									}
									p.Y++;
								}
								p.X++;
							}
							break;
						}
						zero = a * 8;
						end = zero + 7;
						myOctreeStorage.WriteRange(myStorageData, MyStorageDataTypeFlags.Content, zero, end);
						a.Z++;
					}
					a.Y++;
				}
				a.X++;
			}
			try
			{
				a.X = 0;
				Vector3I p2 = default(Vector3I);
				while (a.X < vector3I2.X)
				{
					a.Y = 0;
					while (a.Y < vector3I2.Y)
					{
						a.Z = 0;
						while (a.Z < vector3I2.Z)
						{
							bool num = stream.ReadByteNoAlloc() == 1;
							MyVoxelMaterialDefinition myVoxelMaterialDefinition = null;
							if (num)
							{
								myVoxelMaterialDefinition = Compatibility_LoadCellVoxelMaterial(stream, mapping);
								myStorageData.ClearMaterials(myVoxelMaterialDefinition.Index);
							}
							else
							{
								p2.X = 0;
								while (p2.X < 8)
								{
									p2.Y = 0;
									while (p2.Y < 8)
									{
										p2.Z = 0;
										while (p2.Z < 8)
										{
											myVoxelMaterialDefinition = Compatibility_LoadCellVoxelMaterial(stream, mapping);
											stream.ReadByteNoAlloc();
											myStorageData.Material(ref p2, myVoxelMaterialDefinition.Index);
											p2.Z++;
										}
										p2.Y++;
									}
									p2.X++;
								}
							}
							zero = a * 8;
							end = zero + 7;
							myOctreeStorage.WriteRange(myStorageData, MyStorageDataTypeFlags.Material, zero, end);
							a.Z++;
						}
						a.Y++;
					}
					a.X++;
				}
				return myOctreeStorage;
			}
			catch (EndOfStreamException ex)
			{
				MySandboxGame.Log.WriteLine(ex);
				return myOctreeStorage;
			}
		}

		private MyVoxelMaterialDefinition Compatibility_LoadCellVoxelMaterial(Stream stream, Dictionary<byte, MyVoxelMaterialDefinition> mapping)
		{
			MyVoxelMaterialDefinition myVoxelMaterialDefinition = null;
			byte b = stream.ReadByteNoAlloc();
			if (b != byte.MaxValue)
			{
				myVoxelMaterialDefinition = ((mapping == null) ? MyDefinitionManager.Static.GetVoxelMaterialDefinition(b) : mapping[b]);
			}
			else
			{
				Encoding uTF = Encoding.UTF8;
				byte count = stream.ReadByteNoAlloc();
				stream.Read(m_encodedNameBuffer, 0, count);
				string @string = uTF.GetString(m_encodedNameBuffer, 0, count);
				myVoxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(@string);
			}
			if (myVoxelMaterialDefinition == null)
			{
				myVoxelMaterialDefinition = MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition();
			}
			return myVoxelMaterialDefinition;
		}

		private Dictionary<byte, MyVoxelMaterialDefinition> Compatibility_LoadMaterialIndexMapping(Stream stream)
		{
			int num = stream.Read7BitEncodedInt();
			Dictionary<byte, MyVoxelMaterialDefinition> dictionary = new Dictionary<byte, MyVoxelMaterialDefinition>(num);
			for (int i = 0; i < num; i++)
			{
				byte key = stream.ReadByteNoAlloc();
				string name = stream.ReadString();
				if (!MyDefinitionManager.Static.TryGetVoxelMaterialDefinition(name, out MyVoxelMaterialDefinition definition))
				{
					definition = MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition();
				}
				dictionary.Add(key, definition);
			}
			return dictionary;
		}
	}
}
