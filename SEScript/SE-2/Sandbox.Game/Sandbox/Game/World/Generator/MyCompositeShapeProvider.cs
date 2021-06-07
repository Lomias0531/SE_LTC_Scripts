using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Noise;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
	[MyStorageDataProvider(10002)]
	internal sealed class MyCompositeShapeProvider : IMyStorageDataProvider
	{
		public class MyCombinedCompositeInfoProvider : MyProceduralCompositeInfoProvider, IMyCompositionInfoProvider
		{
			private new readonly IMyCompositeShape[] m_filledShapes;

			private new readonly IMyCompositeShape[] m_removedShapes;

			IMyCompositeShape[] IMyCompositionInfoProvider.FilledShapes => m_filledShapes;

			IMyCompositeShape[] IMyCompositionInfoProvider.RemovedShapes => m_removedShapes;

			public MyCombinedCompositeInfoProvider(ref ConstructionData data, IMyCompositeShape[] filledShapes, IMyCompositeShape[] removedShapes)
				: base(ref data)
			{
				m_filledShapes = base.m_filledShapes.Concat(filledShapes).ToArray();
				m_removedShapes = base.m_removedShapes.Concat(removedShapes).ToArray();
			}

			public new void UpdateMaterials(MyVoxelMaterialDefinition defaultMaterial, MyCompositeShapeOreDeposit[] deposits)
			{
				base.UpdateMaterials(defaultMaterial, deposits);
			}
		}

		private struct State
		{
			public uint Version;

			public int Generator;

			public int Seed;

			public float Size;

			public uint UnusedCompat;

			public int GeneratorSeed;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct MaxOp : MyStorageData.IOperator
		{
			public void Op(ref byte a, byte b)
			{
				a = Math.Max(a, b);
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct DiffOp : MyStorageData.IOperator
		{
			public void Op(ref byte a, byte b)
			{
				a = (byte)Math.Min(a, 255 - b);
			}
		}

		public class MyProceduralCompositeInfoProvider : IMyCompositionInfoProvider
		{
			public struct ConstructionData
			{
				public IMyModule MacroModule;

				public IMyModule DetailModule;

				public MyCsgShapeBase[] FilledShapes;

				public MyCsgShapeBase[] RemovedShapes;

				public MyCompositeShapeOreDeposit[] Deposits;

				public MyVoxelMaterialDefinition DefaultMaterial;
			}

			protected class ProceduralCompositeShape : IMyCompositeShape
			{
				private MyCsgShapeBase m_shape;

				private MyProceduralCompositeInfoProvider m_context;

				public ProceduralCompositeShape(MyProceduralCompositeInfoProvider context, MyCsgShapeBase shape)
				{
					m_shape = shape;
					m_context = context;
				}

				public ContainmentType Contains(ref BoundingBox queryBox, ref BoundingSphere querySphere, int lodVoxelSize)
				{
					return m_shape.Contains(ref queryBox, ref querySphere, lodVoxelSize);
				}

				public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
				{
					return m_shape.SignedDistance(ref localPos, lodVoxelSize, m_context.MacroModule, m_context.DetailModule);
				}

				public unsafe void ComputeContent(MyStorageData target, int lodIndex, Vector3I minInLod, Vector3I maxInLod, int lodVoxelSize)
				{
					Vector3I a = minInLod;
					Vector3I vector3I = a * lodVoxelSize;
					Vector3I vector3I2 = vector3I;
					fixed (byte* ptr = target[MyStorageDataTypeEnum.Content])
					{
						byte* ptr2 = ptr;
						_ = target.SizeLinear;
						a.Z = minInLod.Z;
						while (a.Z <= maxInLod.Z)
						{
							a.Y = minInLod.Y;
							while (a.Y <= maxInLod.Y)
							{
								a.X = minInLod.X;
								while (a.X <= maxInLod.X)
								{
									Vector3 localPos = new Vector3(vector3I);
									float signedDistance = SignedDistance(ref localPos, lodVoxelSize);
									*ptr2 = SignedDistanceToContent(signedDistance);
									ptr2 += target.StepLinear;
									vector3I.X += lodVoxelSize;
									a.X++;
								}
								vector3I.Y += lodVoxelSize;
								vector3I.X = vector3I2.X;
								a.Y++;
							}
							vector3I.Z += lodVoxelSize;
							vector3I.Y = vector3I2.Y;
							a.Z++;
						}
					}
				}

				public void DebugDraw(ref MatrixD worldMatrix, Color color)
				{
					m_shape.DebugDraw(ref worldMatrix, color);
				}

				public void Close()
				{
				}
			}

			protected class ProceduralCompositeOreDeposit : ProceduralCompositeShape, IMyCompositeDeposit, IMyCompositeShape
			{
				private readonly MyCompositeShapeOreDeposit m_deposit;

				public ProceduralCompositeOreDeposit(MyProceduralCompositeInfoProvider context, MyCompositeShapeOreDeposit deposit)
					: base(context, deposit.Shape)
				{
					m_deposit = deposit;
				}

				public MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 localPos, float lodVoxelSize)
				{
					return m_deposit.GetMaterialForPosition(ref localPos, lodVoxelSize);
				}

				public new void DebugDraw(ref MatrixD worldMatrix, Color color)
				{
					m_deposit.DebugDraw(ref worldMatrix, color);
				}
			}

			public readonly IMyModule MacroModule;

			public readonly IMyModule DetailModule;

			protected ProceduralCompositeOreDeposit[] m_deposits;

			protected MyVoxelMaterialDefinition m_defaultMaterial;

			protected readonly ProceduralCompositeShape[] m_filledShapes;

			protected readonly ProceduralCompositeShape[] m_removedShapes;

			IMyCompositeDeposit[] IMyCompositionInfoProvider.Deposits => m_deposits;

			IMyCompositeShape[] IMyCompositionInfoProvider.FilledShapes => m_filledShapes;

			IMyCompositeShape[] IMyCompositionInfoProvider.RemovedShapes => m_removedShapes;

			MyVoxelMaterialDefinition IMyCompositionInfoProvider.DefaultMaterial => m_defaultMaterial;

			public MyProceduralCompositeInfoProvider(ref ConstructionData data)
			{
				MacroModule = data.MacroModule;
				DetailModule = data.DetailModule;
				m_defaultMaterial = data.DefaultMaterial;
				m_deposits = data.Deposits.Select((MyCompositeShapeOreDeposit x) => new ProceduralCompositeOreDeposit(this, x)).ToArray();
				m_filledShapes = data.FilledShapes.Select((MyCsgShapeBase x) => new ProceduralCompositeShape(this, x)).ToArray();
				m_removedShapes = data.RemovedShapes.Select((MyCsgShapeBase x) => new ProceduralCompositeShape(this, x)).ToArray();
			}

			void IMyCompositionInfoProvider.Close()
			{
			}

			protected void UpdateMaterials(MyVoxelMaterialDefinition defaultMaterial, MyCompositeShapeOreDeposit[] deposits)
			{
				m_defaultMaterial = defaultMaterial;
				m_deposits = deposits.Select((MyCompositeShapeOreDeposit x) => new ProceduralCompositeOreDeposit(this, x)).ToArray();
			}
		}

		private const uint CURRENT_VERSION = 3u;

		private const uint VERSION_WITHOUT_PLANETS = 1u;

		private const uint VERSION_WITHOUT_GENERATOR_SEED = 2u;

		private State m_state;

		private IMyCompositionInfoProvider m_infoProvider;

		[ThreadStatic]
		private static List<IMyCompositeDeposit> m_overlappedDeposits;

		[ThreadStatic]
		private static List<IMyCompositeShape> m_overlappedFilledShapes;

		[ThreadStatic]
		private static List<IMyCompositeShape> m_overlappedRemovedShapes;

		[ThreadStatic]
		private static MyStorageData m_storageCache;

		unsafe int IMyStorageDataProvider.SerializedSize => sizeof(State);

		private void InitFromState(State state)
		{
			m_state = state;
			MyCompositeShapeGeneratorDelegate myCompositeShapeGeneratorDelegate = MyCompositeShapes.AsteroidGenerators[state.Generator];
			m_infoProvider = myCompositeShapeGeneratorDelegate(state.GeneratorSeed, state.Seed, state.Size);
		}

		bool IMyStorageDataProvider.Intersect(ref LineD line, out double startOffset, out double endOffset)
		{
			startOffset = 0.0;
			endOffset = 0.0;
			return true;
		}

		public ContainmentType Intersect(BoundingBoxI box, int lod)
		{
			return Intersect(m_infoProvider, box, lod);
		}

		public static ContainmentType Intersect(IMyCompositionInfoProvider infoProvider, BoundingBoxI box, int lod)
		{
			ContainmentType containmentType = ContainmentType.Disjoint;
			BoundingBox queryBox = new BoundingBox(box);
			BoundingSphere querySphere = new BoundingSphere(queryBox.Center, queryBox.Extents.Length() / 2f);
			IMyCompositeShape[] filledShapes = infoProvider.FilledShapes;
			for (int i = 0; i < filledShapes.Length; i++)
			{
				ContainmentType containmentType2 = filledShapes[i].Contains(ref queryBox, ref querySphere, 1);
				switch (containmentType2)
				{
				case ContainmentType.Contains:
					break;
				case ContainmentType.Intersects:
					containmentType = ContainmentType.Intersects;
					continue;
				default:
					continue;
				}
				containmentType = containmentType2;
				break;
			}
			if (containmentType != 0)
			{
				filledShapes = infoProvider.RemovedShapes;
				for (int i = 0; i < filledShapes.Length; i++)
				{
					switch (filledShapes[i].Contains(ref queryBox, ref querySphere, 1))
					{
					case ContainmentType.Contains:
						break;
					case ContainmentType.Intersects:
						containmentType = ContainmentType.Intersects;
						continue;
					default:
						continue;
					}
					containmentType = ContainmentType.Disjoint;
					break;
				}
			}
			return containmentType;
		}

		void IMyStorageDataProvider.ReadRange(ref MyVoxelDataRequest req, bool detectOnly = false)
		{
			if (req.RequestedData.Requests(MyStorageDataTypeEnum.Content))
			{
				req.Flags = ReadContentRange(req.Target, ref req.Offset, req.Lod, ref req.MinInLod, ref req.MaxInLod, detectOnly);
			}
			else
			{
				req.Flags = ReadMaterialRange(req.Target, ref req.Offset, req.Lod, ref req.MinInLod, ref req.MaxInLod, detectOnly, (req.RequestFlags & MyVoxelRequestFlags.ConsiderContent) > (MyVoxelRequestFlags)0);
			}
			req.Flags |= (req.RequestFlags & MyVoxelRequestFlags.RequestFlags);
		}

		void IMyStorageDataProvider.ReadRange(MyStorageData target, MyStorageDataTypeFlags dataType, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod)
		{
			if (dataType.Requests(MyStorageDataTypeEnum.Content))
			{
				ReadContentRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, detectOnly: false);
			}
			else
			{
				ReadMaterialRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, detectOnly: false, considerContent: false);
			}
		}

		void IMyStorageDataProvider.DebugDraw(ref MatrixD worldMatrix)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_SEEDS)
			{
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Size: " + m_state.Size + Environment.NewLine + "Seed: " + m_state.Seed + Environment.NewLine + "GeneratorSeed: " + m_state.GeneratorSeed, Color.Red, 0.7f, depthRead: false);
			}
			Color green = Color.Green;
			Color red = Color.Red;
			Color cornflowerBlue = Color.CornflowerBlue;
			IMyCompositeShape[] filledShapes = m_infoProvider.FilledShapes;
			for (int i = 0; i < filledShapes.Length; i++)
			{
				filledShapes[i].DebugDraw(ref worldMatrix, green);
			}
			filledShapes = m_infoProvider.RemovedShapes;
			for (int i = 0; i < filledShapes.Length; i++)
			{
				filledShapes[i].DebugDraw(ref worldMatrix, red);
			}
			IMyCompositeDeposit[] deposits = m_infoProvider.Deposits;
			for (int i = 0; i < deposits.Length; i++)
			{
				deposits[i].DebugDraw(ref worldMatrix, cornflowerBlue);
			}
		}

		void IMyStorageDataProvider.ReindexMaterials(Dictionary<byte, byte> oldToNewIndexMap)
		{
		}

		void IMyStorageDataProvider.PostProcess(VrVoxelMesh mesh, MyStorageDataTypeFlags dataTypes)
		{
		}

		void IMyStorageDataProvider.Close()
		{
			foreach (IMyCompositeShape item in m_infoProvider.Deposits.Concat(m_infoProvider.FilledShapes).Concat(m_infoProvider.RemovedShapes))
			{
				item.Close();
			}
			m_infoProvider.Close();
			m_infoProvider = null;
		}

		private static MyStorageData GetTempStorage(ref Vector3I min, ref Vector3I max)
		{
			MyStorageData myStorageData = m_storageCache;
			if (myStorageData == null)
			{
				myStorageData = (m_storageCache = new MyStorageData(MyStorageDataTypeFlags.Content));
			}
			myStorageData.Resize(min, max);
			return myStorageData;
		}

		internal MyVoxelRequestFlags ReadContentRange(MyStorageData target, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, bool detectOnly)
		{
			SetupReading(lodIndex, ref minInLod, ref maxInLod, out int lodVoxelSize, out BoundingBox queryBox, out BoundingSphere querySphere);
			using (MyUtils.ReuseCollection(ref m_overlappedFilledShapes))
			{
				using (MyUtils.ReuseCollection(ref m_overlappedRemovedShapes))
				{
					List<IMyCompositeShape> overlappedFilledShapes = m_overlappedFilledShapes;
					List<IMyCompositeShape> overlappedRemovedShapes = m_overlappedRemovedShapes;
					ContainmentType containmentType = ContainmentType.Disjoint;
					IMyCompositeShape[] removedShapes = m_infoProvider.RemovedShapes;
					foreach (IMyCompositeShape myCompositeShape in removedShapes)
					{
						switch (myCompositeShape.Contains(ref queryBox, ref querySphere, lodVoxelSize))
						{
						case ContainmentType.Contains:
							break;
						case ContainmentType.Intersects:
							containmentType = ContainmentType.Intersects;
							overlappedRemovedShapes.Add(myCompositeShape);
							continue;
						default:
							continue;
						}
						containmentType = ContainmentType.Contains;
						break;
					}
					if (containmentType == ContainmentType.Contains)
					{
						if (!detectOnly)
						{
							target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), 0);
						}
						return MyVoxelRequestFlags.EmptyData;
					}
					ContainmentType containmentType2 = ContainmentType.Disjoint;
					removedShapes = m_infoProvider.FilledShapes;
					foreach (IMyCompositeShape myCompositeShape2 in removedShapes)
					{
						switch (myCompositeShape2.Contains(ref queryBox, ref querySphere, lodVoxelSize))
						{
						case ContainmentType.Contains:
							break;
						case ContainmentType.Intersects:
							overlappedFilledShapes.Add(myCompositeShape2);
							containmentType2 = ContainmentType.Intersects;
							continue;
						default:
							continue;
						}
						overlappedFilledShapes.Clear();
						containmentType2 = ContainmentType.Contains;
						break;
					}
					if (containmentType2 == ContainmentType.Disjoint)
					{
						if (!detectOnly)
						{
							target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), 0);
						}
						return MyVoxelRequestFlags.EmptyData;
					}
					if (containmentType == ContainmentType.Disjoint && containmentType2 == ContainmentType.Contains)
					{
						if (!detectOnly)
						{
							target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), byte.MaxValue);
						}
						return MyVoxelRequestFlags.FullContent;
					}
					if (detectOnly)
					{
						return (MyVoxelRequestFlags)0;
					}
					MyStorageData tempStorage = GetTempStorage(ref minInLod, ref maxInLod);
					bool flag = containmentType2 == ContainmentType.Contains;
					target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), (byte)(flag ? byte.MaxValue : 0));
					if (!flag)
					{
						foreach (IMyCompositeShape item in overlappedFilledShapes)
						{
							item.ComputeContent(tempStorage, lodIndex, minInLod, maxInLod, lodVoxelSize);
							target.OpRange<MaxOp>(tempStorage, Vector3I.Zero, maxInLod - minInLod, writeOffset, MyStorageDataTypeEnum.Content);
						}
					}
					if (containmentType != 0)
					{
						foreach (IMyCompositeShape item2 in overlappedRemovedShapes)
						{
							item2.ComputeContent(tempStorage, lodIndex, minInLod, maxInLod, lodVoxelSize);
							target.OpRange<DiffOp>(tempStorage, Vector3I.Zero, maxInLod - minInLod, writeOffset, MyStorageDataTypeEnum.Content);
						}
					}
				}
			}
			return (MyVoxelRequestFlags)0;
		}

		internal MyVoxelRequestFlags ReadMaterialRange(MyStorageData target, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, bool detectOnly, bool considerContent)
		{
			SetupReading(lodIndex, ref minInLod, ref maxInLod, out int lodVoxelSize, out BoundingBox queryBox, out BoundingSphere querySphere);
			using (MyUtils.ReuseCollection(ref m_overlappedDeposits))
			{
				List<IMyCompositeDeposit> overlappedDeposits = m_overlappedDeposits;
				MyVoxelMaterialDefinition defaultMaterial = m_infoProvider.DefaultMaterial;
				ContainmentType containmentType = ContainmentType.Disjoint;
				IMyCompositeDeposit[] deposits = m_infoProvider.Deposits;
				foreach (IMyCompositeDeposit myCompositeDeposit in deposits)
				{
					if (myCompositeDeposit.Contains(ref queryBox, ref querySphere, lodVoxelSize) != 0)
					{
						overlappedDeposits.Add(myCompositeDeposit);
						containmentType = ContainmentType.Intersects;
					}
				}
				if (containmentType == ContainmentType.Disjoint)
				{
					if (!detectOnly)
					{
						if (considerContent)
						{
							target.BlockFillMaterialConsiderContent(writeOffset, writeOffset + (maxInLod - minInLod), defaultMaterial.Index);
						}
						else
						{
							target.BlockFillMaterial(writeOffset, writeOffset + (maxInLod - minInLod), defaultMaterial.Index);
						}
					}
					return MyVoxelRequestFlags.EmptyData;
				}
				if (detectOnly)
				{
					return (MyVoxelRequestFlags)0;
				}
				Vector3I a = default(Vector3I);
				a.Z = minInLod.Z;
				while (a.Z <= maxInLod.Z)
				{
					a.Y = minInLod.Y;
					while (a.Y <= maxInLod.Y)
					{
						a.X = minInLod.X;
						while (a.X <= maxInLod.X)
						{
							Vector3I p = a - minInLod + writeOffset;
							if (considerContent && target.Content(ref p) == 0)
							{
								target.Material(ref p, byte.MaxValue);
							}
							else
							{
								Vector3 localPos = a * lodVoxelSize;
								float num = 1f;
								byte materialIdx = defaultMaterial.Index;
								if (!MyFakes.DISABLE_COMPOSITE_MATERIAL)
								{
									foreach (IMyCompositeDeposit item in overlappedDeposits)
									{
										float num2 = item.SignedDistance(ref localPos, 1);
										if (num2 < 0f && num2 <= num)
										{
											num = num2;
											materialIdx = (item.GetMaterialForPosition(ref localPos, lodVoxelSize)?.Index ?? defaultMaterial.Index);
										}
									}
								}
								target.Material(ref p, materialIdx);
							}
							a.X++;
						}
						a.Y++;
					}
					a.Z++;
				}
			}
			return (MyVoxelRequestFlags)0;
		}

		void IMyStorageDataProvider.WriteTo(Stream stream)
		{
			stream.WriteNoAlloc(m_state.Version);
			stream.WriteNoAlloc(m_state.Generator);
			stream.WriteNoAlloc(m_state.Seed);
			stream.WriteNoAlloc(m_state.Size);
			stream.WriteNoAlloc(m_state.UnusedCompat);
			stream.WriteNoAlloc(m_state.GeneratorSeed);
		}

		void IMyStorageDataProvider.ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat)
		{
			State state = default(State);
			state.Version = stream.ReadUInt32();
			if (state.Version != 3)
			{
				isOldFormat = true;
			}
			state.Generator = stream.ReadInt32();
			state.Seed = stream.ReadInt32();
			state.Size = stream.ReadFloat();
			if (state.Version == 1)
			{
				state.UnusedCompat = 0u;
				state.GeneratorSeed = 0;
			}
			else
			{
				state.UnusedCompat = stream.ReadUInt32();
				if (state.UnusedCompat == 1)
				{
					throw new InvalidBranchException();
				}
				if (state.Version <= 2)
				{
					isOldFormat = true;
					state.GeneratorSeed = 0;
				}
				else
				{
					state.GeneratorSeed = stream.ReadInt32();
				}
			}
			InitFromState(state);
			m_state.Version = 3u;
		}

		private static void SetupReading(int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, out int lodVoxelSize, out BoundingBox queryBox, out BoundingSphere querySphere)
		{
			float num = 0.5f * (float)(1 << lodIndex);
			lodVoxelSize = (int)(num * 2f);
			Vector3I voxelCoord = minInLod << lodIndex;
			Vector3I voxelCoord2 = maxInLod << lodIndex;
			MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord, out Vector3 localPosition);
			Vector3 min = localPosition;
			MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord2, out localPosition);
			Vector3 max = localPosition;
			min -= num;
			max += num;
			queryBox = new BoundingBox(min, max);
			BoundingSphere.CreateFromBoundingBox(ref queryBox, out querySphere);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float ContentToSignedDistance(byte content)
		{
			return ((float)(int)content / 255f - 0.5f) * -2f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte SignedDistanceToContent(float signedDistance)
		{
			signedDistance = MathHelper.Clamp(signedDistance, -1f, 1f);
			return (byte)((signedDistance / -2f + 0.5f) * 255f);
		}

		public static MyCompositeShapeProvider CreateAsteroidShape(int seed, float size, int generatorSeed = 0, int? generator = null)
		{
			State state = default(State);
			state.Version = 3u;
			state.Generator = generator.GetValueOrDefault(MySession.Static.Settings.VoxelGeneratorVersion);
			state.Seed = seed;
			state.Size = size;
			state.UnusedCompat = 0u;
			state.GeneratorSeed = generatorSeed;
			MyCompositeShapeProvider myCompositeShapeProvider = new MyCompositeShapeProvider();
			myCompositeShapeProvider.InitFromState(state);
			return myCompositeShapeProvider;
		}
	}
}
