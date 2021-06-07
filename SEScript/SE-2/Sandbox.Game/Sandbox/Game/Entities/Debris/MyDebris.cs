using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Debris
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyDebris : MySessionComponentBase
	{
		private struct MyModelShapeInfo
		{
			public MyModel Model;

			public HkShapeType ShapeType;

			public float Scale;
		}

		private enum DebrisType
		{
			Voxel,
			Random
		}

		private struct DebrisCreationInfo
		{
			public DebrisType Type;

			public float Ammount;

			public Vector3 Velocity;

			public Vector3D Position;

			public MyVoxelMaterialDefinition Material;
		}

		private class MyCreateDebrisWork : AbstractWork
		{
			private struct DebrisData
			{
				public MyDebrisBase Object;

				public Vector3 InitialVelocity;

				public Vector3 StartPos;
			}

			private class Sandbox_Game_Entities_Debris_MyDebris_003C_003EMyCreateDebrisWork_003C_003EActor : IActivator, IActivator<MyCreateDebrisWork>
			{
				private sealed override object CreateInstance()
				{
					return new MyCreateDebrisWork();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyCreateDebrisWork CreateInstance()
				{
					return new MyCreateDebrisWork();
				}

				MyCreateDebrisWork IActivator<MyCreateDebrisWork>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private static Stack<MyCreateDebrisWork> m_pool = new Stack<MyCreateDebrisWork>();

			public readonly Action CompletionCallback;

			private readonly List<DebrisData> m_pieces = new List<DebrisData>();

			public MyDebris Context;

			public int debrisPieces;

			public float initialSpeed;

			public float minSourceDistance;

			public float maxSourceDistance;

			public float minDeviationAngle;

			public float maxDeviationAngle;

			public Vector3 offsetDirection;

			public Vector3 sourceWorldPosition;

			public static MyCreateDebrisWork Create()
			{
				if (m_pool.Count == 0)
				{
					return new MyCreateDebrisWork
					{
						Options = Parallel.DefaultOptions
					};
				}
				return m_pool.Pop();
			}

			private void Release()
			{
				Context = null;
				m_pieces.Clear();
				m_pool.Push(this);
			}

			private MyCreateDebrisWork()
			{
				CompletionCallback = OnWorkCompleted;
			}

			public override void DoWork(WorkData unused)
			{
				if (!MySession.Static.Ready)
				{
					return;
				}
				MyEntityIdentifier.InEntityCreationBlock = true;
				MyEntityIdentifier.LazyInitPerThreadStorage(2048);
				for (int i = 0; i < debrisPieces; i++)
				{
					MyDebrisBase myDebrisBase = Context.CreateRandomDebris();
					if (myDebrisBase == null)
					{
						break;
					}
					float randomFloat = MyUtils.GetRandomFloat(minSourceDistance, maxSourceDistance);
					float randomFloat2 = MyUtils.GetRandomFloat(minDeviationAngle, maxDeviationAngle);
					float randomFloat3 = MyUtils.GetRandomFloat(minDeviationAngle, maxDeviationAngle);
					Matrix matrix = Matrix.CreateRotationX(randomFloat2) * Matrix.CreateRotationY(randomFloat3);
					Vector3 value = Vector3.Transform(offsetDirection, matrix);
					Vector3 startPos = sourceWorldPosition + value * randomFloat;
					Vector3 initialVelocity = value * initialSpeed;
					m_pieces.Add(new DebrisData
					{
						Object = myDebrisBase,
						StartPos = startPos,
						InitialVelocity = initialVelocity
					});
				}
				MyEntityIdentifier.ClearPerThreadEntities();
				MyEntityIdentifier.InEntityCreationBlock = false;
			}

			private void OnWorkCompleted()
			{
				if (MySession.Static.Ready)
				{
					MyEntityIdentifier.InEntityCreationBlock = true;
					foreach (DebrisData piece in m_pieces)
					{
						MyDebrisBase @object = piece.Object;
						MyEntityIdentifier.AddEntityWithId(@object);
						@object.Debris.Start(piece.StartPos, piece.InitialVelocity);
					}
					MyEntityIdentifier.InEntityCreationBlock = false;
					Release();
				}
			}
		}

		private static MyDebris m_static;

		private List<Vector3D> m_positionBuffer;

		private List<Vector3> m_voxelDebrisOffsets;

		private static string[] m_debrisModels;

		private static MyDebrisDefinition[] m_debrisVoxels;

		public static readonly float VoxelDebrisModelVolume = 0.15f;

		private MyConcurrentDictionary<MyModelShapeInfo, HkShape> m_shapes = new MyConcurrentDictionary<MyModelShapeInfo, HkShape>();

		private const int MaxDebrisCount = 33;

		private int m_debrisCount;

		private Queue<DebrisCreationInfo> m_creationBuffer = new Queue<DebrisCreationInfo>(33);

		private MyDebrisBaseDescription m_desc = new MyDebrisBaseDescription();

		private int m_debrisModelIndex;

		public static MyDebris Static
		{
			get
			{
				return m_static;
			}
			private set
			{
				m_static = value;
			}
		}

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyPhysics)
		};

		public bool TooManyDebris => m_debrisCount > 33;

		public MyDebris()
		{
			m_debrisModels = (from x in MyDefinitionManager.Static.GetDebrisDefinitions()
				where x.Type == MyDebrisType.Model
				select x.Model).ToArray();
			m_debrisVoxels = (from x in MyDefinitionManager.Static.GetDebrisDefinitions()
				where x.Type == MyDebrisType.Voxel
				orderby x.MinAmount descending
				select x).ToArray();
		}

		private void EnqueueDebrisCreation(DebrisCreationInfo info)
		{
			while (m_creationBuffer.Count >= 33)
			{
				m_creationBuffer.Dequeue();
			}
			if (MyFakes.ENABLE_DEBRIS)
			{
				m_creationBuffer.Enqueue(info);
			}
		}

		public HkShape GetDebrisShape(MyModel model, HkShapeType shapeType, float scale)
		{
			MyModelShapeInfo key = default(MyModelShapeInfo);
			key.Model = model;
			key.ShapeType = shapeType;
			key.Scale = scale;
			if (!m_shapes.TryGetValue(key, out HkShape value))
			{
				value = CreateShape(model, shapeType, scale);
				if (!m_shapes.TryAdd(key, value))
				{
					value.RemoveReference();
					value = m_shapes.GetValueOrDefault(key, HkShape.Empty);
				}
			}
			return value;
		}

		private HkShape CreateShape(MyModel model, HkShapeType shapeType, float scale)
		{
			if (model.HavokCollisionShapes != null && model.HavokCollisionShapes.Length != 0)
			{
				HkShape result;
				if (model.HavokCollisionShapes.Length == 1)
				{
					result = model.HavokCollisionShapes[0];
					result.AddReference();
				}
				else
				{
					result = new HkListShape(model.HavokCollisionShapes, HkReferencePolicy.None);
				}
				return result;
			}
			switch (shapeType)
			{
			case HkShapeType.Box:
				return new HkBoxShape(Vector3.Max(scale * (model.BoundingBox.Max - model.BoundingBox.Min) / 2f - 0.05f, new Vector3(0.025f)), 0.02f);
			case HkShapeType.Sphere:
				return new HkSphereShape(scale * model.BoundingSphere.Radius);
			case HkShapeType.ConvexVertices:
			{
				Vector3[] array = new Vector3[model.GetVerticesCount()];
				for (int i = 0; i < model.GetVerticesCount(); i++)
				{
					array[i] = scale * model.GetVertex(i);
				}
				return new HkConvexVerticesShape(array, array.Length, shrink: true, 0.1f);
			}
			default:
				throw new InvalidOperationException("This shape is not supported");
			}
		}

		public override void LoadData()
		{
			m_positionBuffer = new List<Vector3D>(24);
			m_voxelDebrisOffsets = new List<Vector3>(8);
			m_desc.LifespanMinInMiliseconds = 10000;
			m_desc.LifespanMaxInMiliseconds = 20000;
			m_desc.OnCloseAction = OnDebrisClosed;
			GenerateVoxelDebrisPositionOffsets(m_voxelDebrisOffsets);
			Static = this;
			string[] debrisModels = m_debrisModels;
			for (int i = 0; i < debrisModels.Length; i++)
			{
				MyModels.GetModelOnlyData(debrisModels[i]);
			}
			MyDebrisDefinition[] debrisVoxels = m_debrisVoxels;
			for (int i = 0; i < debrisVoxels.Length; i++)
			{
				MyModels.GetModelOnlyData(debrisVoxels[i].Model);
			}
		}

		private void OnDebrisClosed(MyDebrisBase obj)
		{
			Interlocked.Decrement(ref m_debrisCount);
		}

		protected override void UnloadData()
		{
			if (Static != null)
			{
				foreach (KeyValuePair<MyModelShapeInfo, HkShape> shape in m_shapes)
				{
					shape.Value.RemoveReference();
				}
				m_shapes.Clear();
				m_positionBuffer = null;
				Static = null;
				m_creationBuffer.Clear();
			}
		}

		public void CreateDirectedDebris(Vector3 sourceWorldPosition, Vector3 offsetDirection, float minSourceDistance, float maxSourceDistance, float minDeviationAngle, float maxDeviationAngle, int debrisPieces, float initialSpeed)
		{
			MyCreateDebrisWork myCreateDebrisWork = MyCreateDebrisWork.Create();
			myCreateDebrisWork.sourceWorldPosition = sourceWorldPosition;
			myCreateDebrisWork.offsetDirection = offsetDirection;
			myCreateDebrisWork.minSourceDistance = minSourceDistance;
			myCreateDebrisWork.maxSourceDistance = maxSourceDistance;
			myCreateDebrisWork.minDeviationAngle = minDeviationAngle;
			myCreateDebrisWork.maxDeviationAngle = maxDeviationAngle;
			myCreateDebrisWork.debrisPieces = debrisPieces;
			myCreateDebrisWork.initialSpeed = initialSpeed;
			myCreateDebrisWork.Context = this;
			Parallel.Start(myCreateDebrisWork, myCreateDebrisWork.CompletionCallback);
		}

		public void CreateDirectedDebris(Vector3 sourceWorldPosition, Vector3 offsetDirection, float minSourceDistance, float maxSourceDistance, float minDeviationAngle, float maxDeviationAngle, int debrisPieces, float initialSpeed, float maxAmount, MyVoxelMaterialDefinition material)
		{
			for (int i = 0; i < debrisPieces; i++)
			{
				float randomFloat = MyUtils.GetRandomFloat(minSourceDistance, maxSourceDistance);
				float randomFloat2 = MyUtils.GetRandomFloat(minDeviationAngle, maxDeviationAngle);
				float randomFloat3 = MyUtils.GetRandomFloat(minDeviationAngle, maxDeviationAngle);
				Matrix matrix = Matrix.CreateRotationX(randomFloat2) * Matrix.CreateRotationY(randomFloat3);
				Vector3 value = Vector3.Transform(offsetDirection, matrix);
				Vector3 v = sourceWorldPosition + value * randomFloat;
				Vector3 velocity = value * initialSpeed;
				EnqueueDebrisCreation(new DebrisCreationInfo
				{
					Type = DebrisType.Voxel,
					Position = v,
					Velocity = velocity,
					Material = material,
					Ammount = maxAmount
				});
			}
		}

		public void CreateExplosionDebris(ref BoundingSphereD explosionSphere, MyEntity entity)
		{
			BoundingBoxD bb = entity.PositionComp.WorldAABB;
			CreateExplosionDebris(ref explosionSphere, entity, ref bb);
		}

		public void CreateExplosionDebris(ref BoundingSphereD explosionSphere, MyEntity entity, ref BoundingBoxD bb, float scaleMultiplier = 1f, bool applyVelocity = true)
		{
			MyUtils.GetRandomVector3Normalized();
			MyUtils.GetRandomFloat(0f, (float)explosionSphere.Radius);
			GeneratePositions(bb, m_positionBuffer);
			Vector3 value = (entity.Physics != null) ? entity.Physics.LinearVelocity : Vector3.Zero;
			foreach (Vector3D item in m_positionBuffer)
			{
				Vector3 velocity = applyVelocity ? (MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(4f, 8f) + value) : Vector3.Zero;
				EnqueueDebrisCreation(new DebrisCreationInfo
				{
					Type = DebrisType.Random,
					Position = item,
					Velocity = velocity
				});
			}
		}

		public void CreateExplosionDebris(ref BoundingSphereD explosionSphere, float voxelsCountInPercent, MyVoxelMaterialDefinition voxelMaterial, MyVoxelBase voxelMap)
		{
			MatrixD matrix = MatrixD.CreateRotationX(MyUtils.GetRandomRadian()) * MatrixD.CreateRotationY(MyUtils.GetRandomRadian());
			_ = m_voxelDebrisOffsets.Count;
			int count = m_voxelDebrisOffsets.Count;
			for (int i = 0; i < count; i++)
			{
				MyDebrisVoxel myDebrisVoxel = CreateVoxelDebris((float)explosionSphere.Radius * 100f, (float)explosionSphere.Radius * 1000f);
				if (myDebrisVoxel != null)
				{
					Vector3D position = m_voxelDebrisOffsets[i] * (float)explosionSphere.Radius * 0.5780347f;
					Vector3D.Transform(ref position, ref matrix, out position);
					position += explosionSphere.Center;
					Vector3 randomVector3Normalized = MyUtils.GetRandomVector3Normalized();
					if (!(randomVector3Normalized == Vector3.Zero))
					{
						randomVector3Normalized *= MyUtils.GetRandomFloat(4f, 8f);
						(myDebrisVoxel.Debris as MyDebrisVoxel.MyDebrisVoxelLogic).Start(position, randomVector3Normalized, voxelMaterial);
					}
					continue;
				}
				break;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			int num = m_creationBuffer.Count;
			if (num > 20)
			{
				num /= 10;
			}
			while (num-- > 0)
			{
				DebrisCreationInfo debrisCreationInfo = m_creationBuffer.Dequeue();
				switch (debrisCreationInfo.Type)
				{
				case DebrisType.Random:
					CreateRandomDebris()?.Debris.Start(debrisCreationInfo.Position, debrisCreationInfo.Velocity);
					break;
				case DebrisType.Voxel:
					(CreateVoxelDebris(50f, debrisCreationInfo.Ammount).Debris as MyDebrisVoxel.MyDebrisVoxelLogic).Start(debrisCreationInfo.Position, debrisCreationInfo.Velocity, debrisCreationInfo.Material);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void GeneratePositions(BoundingBoxD boundingBox, List<Vector3D> positionBuffer)
		{
			positionBuffer.Clear();
			Vector3D value = boundingBox.Max - boundingBox.Min;
			double num = value.X * value.Y * value.Z;
			int num2 = 24;
			if (num < 1.0)
			{
				num2 = 1;
			}
			else if (num < 10.0)
			{
				num2 = 12;
			}
			else if (num > 100.0)
			{
				num2 = 48;
			}
			double scaleFactor = Math.Pow((double)num2 / num, 0.3333333432674408);
			Vector3D vector3D = value * scaleFactor;
			int num3 = (int)Math.Ceiling(vector3D.X);
			int num4 = (int)Math.Ceiling(vector3D.Y);
			int num5 = (int)Math.Ceiling(vector3D.Z);
			Vector3D value2 = new Vector3D(value.X / (double)num3, value.Y / (double)num4, value.Z / (double)num5);
			Vector3D value3 = boundingBox.Min + 0.5 * value2;
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num4; j++)
				{
					for (int k = 0; k < num5; k++)
					{
						Vector3D item = value3 + new Vector3D((double)i * value2.X, (double)j * value2.Y, (double)k * value2.Z);
						positionBuffer.Add(item);
					}
				}
			}
		}

		private void GenerateVoxelDebrisPositionOffsets(List<Vector3> offsetBuffer)
		{
			offsetBuffer.Clear();
			Vector3 value = new Vector3(-0.7f);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						Vector3 item = value + new Vector3((float)i * 1.4f, (float)j * 1.4f, (float)k * 1.4f);
						offsetBuffer.Add(item);
					}
				}
			}
		}

		public static string GetRandomDebrisModel()
		{
			return m_debrisModels.GetRandomItem();
		}

		public static string GetRandomDebrisVoxel()
		{
			return m_debrisVoxels.GetRandomItem().Model;
		}

		public static string GetAmountBasedDebrisVoxel(float amount)
		{
			MyDebrisDefinition[] debrisVoxels = m_debrisVoxels;
			foreach (MyDebrisDefinition myDebrisDefinition in debrisVoxels)
			{
				if (!(myDebrisDefinition.MinAmount > amount))
				{
					return myDebrisDefinition.Model;
				}
			}
			return m_debrisVoxels[0].Model;
		}

		public static string GetAnyAmountLessDebrisVoxel(float minAmount, float maxAmount)
		{
			int num = 0;
			int num2 = 0;
			MyDebrisDefinition[] debrisVoxels = m_debrisVoxels;
			foreach (MyDebrisDefinition obj in debrisVoxels)
			{
				if (obj.MinAmount > maxAmount)
				{
					num++;
				}
				if (obj.MinAmount > minAmount)
				{
					num2++;
				}
			}
			int num3 = MyUtils.GetRandomInt(num2 - num + 1) + num;
			return m_debrisVoxels[num3].Model;
		}

		private MyDebrisVoxel CreateVoxelDebris(float minAmount, float maxAmount)
		{
			MyDebrisVoxel myDebrisVoxel = new MyDebrisVoxel();
			m_desc.Model = GetAnyAmountLessDebrisVoxel(minAmount, maxAmount);
			myDebrisVoxel.Debris.Init(m_desc);
			Interlocked.Increment(ref m_debrisCount);
			return myDebrisVoxel;
		}

		private MyDebrisBase CreateRandomDebris()
		{
			MyDebrisBase result = null;
			if (m_debrisModelIndex < m_debrisModels.Length)
			{
				int num = m_debrisModelIndex;
				if (num > m_debrisModels.Length)
				{
					num = (m_debrisModelIndex = num % m_debrisModels.Length);
				}
				result = (MyDebrisBase)CreateDebris(m_debrisModels[num]);
				m_debrisModelIndex++;
			}
			return result;
		}

		public MyEntity CreateDebris(string model)
		{
			if (!MyFakes.ENABLE_DEBRIS)
			{
				return null;
			}
			MyDebrisBase myDebrisBase = new MyDebrisBase();
			m_desc.Model = model;
			myDebrisBase.Debris.Init(m_desc);
			Interlocked.Increment(ref m_debrisCount);
			m_desc.LifespanMinInMiliseconds = 4000;
			m_desc.LifespanMaxInMiliseconds = 7000;
			return myDebrisBase;
		}

		public MyEntity CreateTreeDebris(string model)
		{
			MyDebrisTree myDebrisTree = new MyDebrisTree();
			m_desc.Model = model;
			myDebrisTree.Debris.Init(m_desc);
			Interlocked.Increment(ref m_debrisCount);
			m_desc.LifespanMinInMiliseconds = 4000;
			m_desc.LifespanMaxInMiliseconds = 7000;
			return myDebrisTree;
		}
	}
}
