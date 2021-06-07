using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_FracturedPiece), true)]
	public class MyFracturedPiece : MyEntity, IMyDestroyableObject, IMyEventProxy, IMyEventOwner
	{
		public class HitInfo
		{
			public Vector3D Position;

			public Vector3 Impulse;
		}

		private class MyFracturedPieceDebugDraw : MyDebugRenderComponentBase
		{
			private MyFracturedPiece m_piece;

			public MyFracturedPieceDebugDraw(MyFracturedPiece piece)
			{
				m_piece = piece;
			}

			public override void DebugDraw()
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_FRACTURED_PIECES)
				{
					MyRenderProxy.DebugDrawAxis(m_piece.WorldMatrix, 1f, depthRead: false);
					if (m_piece.Physics != null && m_piece.Physics.RigidBody != null)
					{
						MyPhysicsBody physics = m_piece.Physics;
						HkRigidBody rigidBody = physics.RigidBody;
						Vector3 vector = physics.ClusterToWorld(rigidBody.CenterOfMassWorld);
						new BoundingBoxD(vector - Vector3D.One * 0.10000000149011612, vector + Vector3D.One * 0.10000000149011612);
						string text = $"{rigidBody.GetMotionType()}\n, {physics.Friction}\n{physics.Entity.EntityId.ToString().Substring(0, 5)}";
						MyRenderProxy.DebugDrawText3D(vector, text, Color.White, 0.6f, depthRead: false);
					}
				}
			}

			public override void DebugDrawInvalidTriangles()
			{
			}
		}

		protected class m_fallSoundShouldPlay_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fallSoundShouldPlay;
				ISyncType result = fallSoundShouldPlay = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyFracturedPiece)P_0).m_fallSoundShouldPlay = (Sync<bool, SyncDirection.FromServer>)fallSoundShouldPlay;
				return result;
			}
		}

		protected class m_fallSoundString_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fallSoundString;
				ISyncType result = fallSoundString = new Sync<string, SyncDirection.FromServer>(P_1, P_2);
				((MyFracturedPiece)P_0).m_fallSoundString = (Sync<string, SyncDirection.FromServer>)fallSoundString;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyFracturedPiece_003C_003EActor : IActivator, IActivator<MyFracturedPiece>
		{
			private sealed override object CreateInstance()
			{
				return new MyFracturedPiece();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFracturedPiece CreateInstance()
			{
				return new MyFracturedPiece();
			}

			MyFracturedPiece IActivator<MyFracturedPiece>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public HkdBreakableShape Shape;

		public HitInfo InitialHit;

		private float m_hitPoints;

		public List<MyDefinitionId> OriginalBlocks = new List<MyDefinitionId>();

		private List<HkdShapeInstanceInfo> m_children = new List<HkdShapeInstanceInfo>();

		private List<MyObjectBuilder_FracturedPiece.Shape> m_shapes = new List<MyObjectBuilder_FracturedPiece.Shape>();

		private List<HkdShapeInstanceInfo> m_shapeInfos = new List<HkdShapeInstanceInfo>();

		private MyTimeSpan m_markedBreakImpulse = MyTimeSpan.Zero;

		private HkEasePenetrationAction m_easePenetrationAction;

		private MyEntity3DSoundEmitter m_soundEmitter;

		private DateTime m_soundStart;

		private bool m_obstacleContact;

		private bool m_groundContact;

		private Sync<bool, SyncDirection.FromServer> m_fallSoundShouldPlay;

		private MySoundPair m_fallSound;

		private Sync<string, SyncDirection.FromServer> m_fallSoundString;

		private bool m_contactSet;

		public new MyRenderComponentFracturedPiece Render => (MyRenderComponentFracturedPiece)base.Render;

		public new MyPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public float Integrity => m_hitPoints;

		public bool UseDamageSystem
		{
			get;
			private set;
		}

		public event Action<MyEntity> OnRemove;

		public MyFracturedPiece()
		{
			base.SyncFlag = true;
			base.PositionComp = new MyFracturePiecePositionComponent();
			base.Render = new MyRenderComponentFracturedPiece();
			base.Render.NeedsDraw = true;
			base.Render.PersistentFlags = MyPersistentEntityFlags2.Enabled;
			AddDebugRenderComponent(new MyFracturedPieceDebugDraw(this));
			UseDamageSystem = false;
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			m_fallSoundString.SetLocalValue("");
			m_fallSoundString.ValueChanged += delegate
			{
				SetFallSound();
			};
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_FracturedPiece myObjectBuilder_FracturedPiece = base.GetObjectBuilder(copy) as MyObjectBuilder_FracturedPiece;
			foreach (MyDefinitionId originalBlock in OriginalBlocks)
			{
				myObjectBuilder_FracturedPiece.BlockDefinitions.Add(originalBlock);
			}
			MyObjectBuilder_FracturedPiece.Shape shape;
			if (Physics == null)
			{
				foreach (MyObjectBuilder_FracturedPiece.Shape shape2 in m_shapes)
				{
					List<MyObjectBuilder_FracturedPiece.Shape> shapes = myObjectBuilder_FracturedPiece.Shapes;
					shape = new MyObjectBuilder_FracturedPiece.Shape
					{
						Name = shape2.Name,
						Orientation = shape2.Orientation
					};
					shapes.Add(shape);
				}
				return myObjectBuilder_FracturedPiece;
			}
			if (Physics.BreakableBody.BreakableShape.IsCompound() || string.IsNullOrEmpty(Physics.BreakableBody.BreakableShape.Name))
			{
				Physics.BreakableBody.BreakableShape.GetChildren(m_children);
				if (m_children.Count == 0)
				{
					return myObjectBuilder_FracturedPiece;
				}
				int count = m_children.Count;
				for (int i = 0; i < count; i++)
				{
					HkdShapeInstanceInfo hkdShapeInstanceInfo = m_children[i];
					if (string.IsNullOrEmpty(hkdShapeInstanceInfo.ShapeName))
					{
						hkdShapeInstanceInfo.GetChildren(m_children);
					}
				}
				foreach (HkdShapeInstanceInfo child in m_children)
				{
					string shapeName = child.ShapeName;
					if (!string.IsNullOrEmpty(shapeName))
					{
						shape = default(MyObjectBuilder_FracturedPiece.Shape);
						shape.Name = shapeName;
						shape.Orientation = Quaternion.CreateFromRotationMatrix(child.GetTransform().GetOrientation());
						shape.Fixed = MyDestructionHelper.IsFixed(child.Shape);
						MyObjectBuilder_FracturedPiece.Shape item = shape;
						myObjectBuilder_FracturedPiece.Shapes.Add(item);
					}
				}
				if (Physics.IsInWorld)
				{
					Vector3D v = Physics.ClusterToWorld(Vector3.Transform(m_children[0].GetTransform().Translation, Physics.RigidBody.GetRigidBodyMatrix()));
					MyPositionAndOrientation value = myObjectBuilder_FracturedPiece.PositionAndOrientation.Value;
					value.Position = v;
					myObjectBuilder_FracturedPiece.PositionAndOrientation = value;
				}
				m_children.Clear();
			}
			else
			{
				List<MyObjectBuilder_FracturedPiece.Shape> shapes2 = myObjectBuilder_FracturedPiece.Shapes;
				shape = new MyObjectBuilder_FracturedPiece.Shape
				{
					Name = Physics.BreakableBody.BreakableShape.Name
				};
				shapes2.Add(shape);
			}
			return myObjectBuilder_FracturedPiece;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_FracturedPiece myObjectBuilder_FracturedPiece = objectBuilder as MyObjectBuilder_FracturedPiece;
			if (myObjectBuilder_FracturedPiece.Shapes.Count == 0)
			{
				return;
			}
			foreach (MyObjectBuilder_FracturedPiece.Shape shape in myObjectBuilder_FracturedPiece.Shapes)
			{
				Render.AddPiece(shape.Name, Matrix.CreateFromQuaternion(shape.Orientation));
			}
			OriginalBlocks.Clear();
			foreach (SerializableDefinitionId blockDefinition in myObjectBuilder_FracturedPiece.BlockDefinitions)
			{
				string text = null;
				if (MyDefinitionManager.Static.TryGetDefinition(blockDefinition, out MyPhysicalModelDefinition definition))
				{
					text = definition.Model;
				}
				MyCubeBlockDefinition definition2 = null;
				MyDefinitionManager.Static.TryGetDefinition(blockDefinition, out definition2);
				if (text != null)
				{
					text = definition.Model;
					if (MyModels.GetModelOnlyData(text).HavokBreakableShapes == null)
					{
						MyDestructionData.Static.LoadModelDestruction(text, definition, Vector3.One);
					}
					HkdBreakableShape hkdBreakableShape = MyModels.GetModelOnlyData(text).HavokBreakableShapes[0];
					HkdShapeInstanceInfo item = new HkdShapeInstanceInfo(hkdBreakableShape, null, null);
					m_children.Add(item);
					hkdBreakableShape.GetChildren(m_children);
					if (definition2 != null && definition2.BuildProgressModels != null)
					{
						MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = definition2.BuildProgressModels;
						for (int i = 0; i < buildProgressModels.Length; i++)
						{
							text = buildProgressModels[i].File;
							if (MyModels.GetModelOnlyData(text).HavokBreakableShapes == null)
							{
								MyDestructionData.Static.LoadModelDestruction(text, definition2, Vector3.One);
							}
							hkdBreakableShape = MyModels.GetModelOnlyData(text).HavokBreakableShapes[0];
							item = new HkdShapeInstanceInfo(hkdBreakableShape, null, null);
							m_children.Add(item);
							hkdBreakableShape.GetChildren(m_children);
						}
					}
					OriginalBlocks.Add(blockDefinition);
				}
			}
			m_shapes.AddRange(myObjectBuilder_FracturedPiece.Shapes);
			Vector3? vector = null;
			int index = 0;
			for (int j = 0; j < m_children.Count; j++)
			{
				HkdShapeInstanceInfo child = m_children[j];
				Func<MyObjectBuilder_FracturedPiece.Shape, bool> predicate = (MyObjectBuilder_FracturedPiece.Shape s) => s.Name == child.ShapeName;
				IEnumerable<MyObjectBuilder_FracturedPiece.Shape> source = m_shapes.Where(predicate);
				if (source.Count() > 0)
				{
					MyObjectBuilder_FracturedPiece.Shape item2 = source.First();
					Matrix transform = Matrix.CreateFromQuaternion(item2.Orientation);
					if (!vector.HasValue && item2.Name == m_shapes[0].Name)
					{
						vector = child.GetTransform().Translation;
						index = m_shapeInfos.Count;
					}
					transform.Translation = child.GetTransform().Translation;
					HkdShapeInstanceInfo item3 = new HkdShapeInstanceInfo(child.Shape.Clone(), transform);
					if (item2.Fixed)
					{
						item3.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
					}
					m_shapeInfos.Add(item3);
					m_shapes.Remove(item2);
				}
				else
				{
					child.GetChildren(m_children);
				}
			}
			if (m_shapeInfos.Count == 0)
			{
				List<string> list = new List<string>();
				foreach (MyObjectBuilder_FracturedPiece.Shape shape2 in myObjectBuilder_FracturedPiece.Shapes)
				{
					list.Add(shape2.Name);
				}
				string text2 = list.Aggregate((string str1, string str2) => str1 + ", " + str2);
				OriginalBlocks.Aggregate("", (string str, MyDefinitionId defId) => str + ", " + defId.ToString());
				throw new Exception("No relevant shape was found for fractured piece. It was probably reexported and names changed. Shapes: " + text2 + ". Original blocks: " + text2);
			}
			if (vector.HasValue)
			{
				for (int k = 0; k < m_shapeInfos.Count; k++)
				{
					Matrix m = m_shapeInfos[k].GetTransform();
					m.Translation -= vector.Value;
					m_shapeInfos[k].SetTransform(ref m);
				}
				Matrix m2 = m_shapeInfos[index].GetTransform();
				m2.Translation = Vector3.Zero;
				m_shapeInfos[index].SetTransform(ref m2);
			}
			if (m_shapeInfos.Count > 0)
			{
				if (m_shapeInfos.Count == 1)
				{
					Shape = m_shapeInfos[0].Shape;
				}
				else
				{
					Shape = new HkdCompoundBreakableShape(null, m_shapeInfos);
					((HkdCompoundBreakableShape)Shape).RecalcMassPropsFromChildren();
				}
				Shape.SetStrenght(MyDestructionConstants.STRENGTH);
				HkMassProperties massProperties = default(HkMassProperties);
				Shape.BuildMassProperties(ref massProperties);
				Shape.SetChildrenParent(Shape);
				Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_DEBRIS);
				Physics.CanUpdateAccelerations = true;
				Physics.InitialSolverDeactivation = HkSolverDeactivation.High;
				Physics.CreateFromCollisionObject(Shape.GetShape(), Vector3.Zero, base.PositionComp.WorldMatrix, massProperties);
				Physics.BreakableBody = new HkdBreakableBody(Shape, Physics.RigidBody, null, base.PositionComp.WorldMatrix);
				Physics.BreakableBody.AfterReplaceBody += Physics.FracturedBody_AfterReplaceBody;
				if (OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetDefinition(OriginalBlocks[0], out MyPhysicalModelDefinition definition3))
				{
					Physics.MaterialType = definition3.PhysicalMaterial.Id.SubtypeId;
				}
				HkRigidBody rigidBody = Physics.RigidBody;
				if (MyDestructionHelper.IsFixed(Physics.BreakableBody.BreakableShape))
				{
					rigidBody.UpdateMotionType(HkMotionType.Fixed);
					rigidBody.LinearVelocity = Vector3.Zero;
					rigidBody.AngularVelocity = Vector3.Zero;
				}
				Physics.Enabled = true;
			}
			m_children.Clear();
			m_shapeInfos.Clear();
		}

		internal void InitFromBreakableBody(HkdBreakableBody b, MatrixD worldMatrix, MyCubeBlock block)
		{
			OriginalBlocks.Clear();
			if (block != null)
			{
				if (block is MyCompoundCubeBlock)
				{
					foreach (MySlimBlock block2 in (block as MyCompoundCubeBlock).GetBlocks())
					{
						OriginalBlocks.Add(block2.BlockDefinition.Id);
					}
				}
				else if (block is MyFracturedBlock)
				{
					OriginalBlocks.AddRange((block as MyFracturedBlock).OriginalBlocks);
				}
				else
				{
					OriginalBlocks.Add(block.BlockDefinition.Id);
				}
			}
			HkRigidBody rigidBody = b.GetRigidBody();
			bool flag = MyDestructionHelper.IsFixed(b.BreakableShape);
			if (flag)
			{
				rigidBody.UpdateMotionType(HkMotionType.Fixed);
				rigidBody.LinearVelocity = Vector3.Zero;
				rigidBody.AngularVelocity = Vector3.Zero;
			}
			if (base.SyncFlag)
			{
				CreateSync();
			}
			base.PositionComp.WorldMatrix = worldMatrix;
			Physics.Flags = ((flag || !Sync.IsServer) ? RigidBodyFlag.RBF_STATIC : RigidBodyFlag.RBF_DEBRIS);
			Physics.BreakableBody = b;
			rigidBody.UserObject = Physics;
			if (!flag)
			{
				rigidBody.Motion.SetDeactivationClass(HkSolverDeactivation.High);
				rigidBody.EnableDeactivation = true;
				if (MyFakes.REDUCE_FRACTURES_COUNT)
				{
					if (b.BreakableShape.Volume < 1f && MyRandom.Instance.Next(6) > 1)
					{
						rigidBody.Layer = 14;
					}
					else
					{
						rigidBody.Layer = 15;
					}
				}
				else
				{
					rigidBody.Layer = 15;
				}
			}
			else
			{
				rigidBody.Layer = 13;
			}
			Physics.BreakableBody.AfterReplaceBody += Physics.FracturedBody_AfterReplaceBody;
			if (OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetDefinition(OriginalBlocks[0], out MyPhysicalModelDefinition definition))
			{
				Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
			}
			Physics.Enabled = true;
			MyDestructionHelper.FixPosition(this);
			SetDataFromHavok(b.BreakableShape);
			_ = b.GetRigidBody().CenterOfMassLocal;
			_ = b.GetRigidBody().CenterOfMassWorld;
			Vector3 coM = b.BreakableShape.CoM;
			b.GetRigidBody().CenterOfMassLocal = coM;
			b.BreakableShape.RemoveReference();
		}

		public void SetDataFromHavok(HkdBreakableShape shape)
		{
			Shape = shape;
			Shape.AddReference();
			if (Render != null)
			{
				if (shape.IsCompound() || string.IsNullOrEmpty(shape.Name))
				{
					shape.GetChildren(m_shapeInfos);
					foreach (HkdShapeInstanceInfo shapeInfo in m_shapeInfos)
					{
						if (shapeInfo.IsValid())
						{
							Render.AddPiece(shapeInfo.ShapeName, shapeInfo.GetTransform());
						}
					}
					m_shapeInfos.Clear();
				}
				else
				{
					Render.AddPiece(shape.Name, Matrix.Identity);
				}
			}
			m_hitPoints = Shape.Volume * 100f;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			Physics.Enabled = true;
			Physics.RigidBody.Activate();
			Physics.RigidBody.ContactPointCallbackDelay = 0;
			Physics.RigidBody.ContactSoundCallbackEnabled = true;
			if (InitialHit != null)
			{
				Physics.ApplyImpulse(InitialHit.Impulse, Physics.CenterOfMassWorld);
				MyPhysics.FractureImpactDetails details = default(MyPhysics.FractureImpactDetails);
				details.Entity = this;
				details.World = Physics.HavokWorld;
				details.ContactInWorld = InitialHit.Position;
				HkdFractureImpactDetails hkdFractureImpactDetails = HkdFractureImpactDetails.Create();
				hkdFractureImpactDetails.SetBreakingBody(Physics.RigidBody);
				hkdFractureImpactDetails.SetContactPoint(Physics.WorldToCluster(InitialHit.Position));
				hkdFractureImpactDetails.SetDestructionRadius(0.05f);
				hkdFractureImpactDetails.SetBreakingImpulse(30000f);
				hkdFractureImpactDetails.SetParticleVelocity(InitialHit.Impulse);
				hkdFractureImpactDetails.SetParticlePosition(Physics.WorldToCluster(InitialHit.Position));
				hkdFractureImpactDetails.SetParticleMass(500f);
				details.Details = hkdFractureImpactDetails;
				MyPhysics.EnqueueDestruction(details);
			}
			Vector3 gravity = MyGravityProviderSystem.CalculateTotalGravityInPoint(base.PositionComp.GetPosition());
			Physics.RigidBody.Gravity = gravity;
		}

		public void RegisterObstacleContact(ref HkContactPointEvent e)
		{
			if (!m_obstacleContact && m_fallSoundShouldPlay.Value && (DateTime.UtcNow - m_soundStart).TotalSeconds >= 1.0)
			{
				m_obstacleContact = true;
			}
		}

		private void SetFallSound()
		{
			if (OriginalBlocks != null)
			{
				MyObjectBuilderType typeId = OriginalBlocks[0].TypeId;
				if (typeId.ToString().Equals("MyObjectBuilder_Tree"))
				{
					m_fallSound = new MySoundPair(m_fallSoundString.Value);
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		public void StartFallSound(string sound)
		{
			m_groundContact = false;
			m_obstacleContact = false;
			m_fallSoundString.Value = sound;
			m_soundStart = DateTime.UtcNow;
			m_fallSoundShouldPlay.Value = true;
			if (!m_contactSet && (Sandbox.Engine.Platform.Game.IsDedicated || MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer))
			{
				Physics.RigidBody.ContactSoundCallback += RegisterObstacleContact;
			}
			m_contactSet = true;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_markedBreakImpulse != MyTimeSpan.Zero)
			{
				UnmarkEntityBreakable(checkTime: true);
			}
			if (!m_fallSoundShouldPlay.Value && Physics.LinearVelocity.LengthSquared() > 25f && (DateTime.UtcNow - m_soundStart).TotalSeconds >= 1.0)
			{
				m_fallSoundShouldPlay.Value = true;
				m_obstacleContact = false;
				m_groundContact = false;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (m_fallSoundShouldPlay.Value)
				{
					if (m_soundEmitter == null)
					{
						m_soundEmitter = new MyEntity3DSoundEmitter(this);
					}
					if (!m_soundEmitter.IsPlaying && m_fallSound != null && m_fallSound != MySoundPair.Empty)
					{
						m_soundEmitter.PlaySound(m_fallSound, stopPrevious: true, skipIntro: true);
					}
				}
				else if (m_soundEmitter != null && m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.StopSound(forced: false);
				}
			}
			if (m_obstacleContact && !m_groundContact)
			{
				if (Physics.LinearVelocity.Y > 0f || Physics.LinearVelocity.LengthSquared() < 9f)
				{
					m_groundContact = true;
					m_fallSoundShouldPlay.Value = false;
					m_soundStart = DateTime.UtcNow;
				}
				else
				{
					m_obstacleContact = false;
				}
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
				if (m_soundEmitter.IsPlaying && (DateTime.UtcNow - m_soundStart).TotalSeconds >= 15.0)
				{
					m_fallSoundShouldPlay.Value = false;
				}
			}
			Vector3 gravity = MyGravityProviderSystem.CalculateTotalGravityInPoint(base.PositionComp.GetPosition());
			Physics.RigidBody.Gravity = gravity;
		}

		private void UnmarkEntityBreakable(bool checkTime)
		{
			if (!(m_markedBreakImpulse != MyTimeSpan.Zero) || (checkTime && !(MySandboxGame.Static.TotalTime - m_markedBreakImpulse > MyTimeSpan.FromSeconds(1.5))))
			{
				return;
			}
			m_markedBreakImpulse = MyTimeSpan.Zero;
			if (Physics != null && Physics.HavokWorld != null)
			{
				Physics.HavokWorld.BreakOffPartsUtil.UnmarkEntityBreakable(Physics.RigidBody);
				if (checkTime)
				{
					CreateEasyPenetrationAction(1f);
				}
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			MyCubeBlockDefinition blockDefinition = null;
			if (Physics.HavokWorld != null && OriginalBlocks.Count != 0 && MyDefinitionManager.Static.TryGetCubeBlockDefinition(OriginalBlocks[0], out blockDefinition) && blockDefinition.CubeSize == MyCubeSize.Large)
			{
				float maxImpulse = Physics.Mass * 0.4f;
				Physics.HavokWorld.BreakOffPartsUtil.MarkEntityBreakable(Physics.RigidBody, maxImpulse);
				m_markedBreakImpulse = MySandboxGame.Static.TotalTime;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			UnmarkEntityBreakable(checkTime: false);
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			if (this.OnRemove != null)
			{
				this.OnRemove(this);
			}
		}

		private void CreateEasyPenetrationAction(float duration)
		{
			if (Physics != null && Physics.RigidBody != null)
			{
				m_easePenetrationAction = new HkEasePenetrationAction(Physics.RigidBody, duration);
				m_easePenetrationAction.InitialAllowedPenetrationDepthMultiplier = 5f;
				m_easePenetrationAction.InitialAdditionalAllowedPenetrationDepth = 2f;
			}
		}

		public void OnDestroy()
		{
			if (Sync.IsServer)
			{
				MyFracturedPiecesManager.Static.RemoveFracturePiece(this, 2f);
			}
		}

		public bool DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
		{
			if (Sync.IsServer)
			{
				MyDamageInformation info = new MyDamageInformation(isDeformation: false, damage, damageType, attackerId);
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseBeforeDamageApplied(this, ref info);
				}
				m_hitPoints -= info.Amount;
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseAfterDamageApplied(this, info);
				}
				if (m_hitPoints <= 0f)
				{
					MyFracturedPiecesManager.Static.RemoveFracturePiece(this, 2f);
					if (UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseDestroyed(this, info);
					}
				}
			}
			return true;
		}

		public void DebugCheckValidShapes()
		{
			bool flag = false;
			HashSet<Tuple<string, float>> hashSet = new HashSet<Tuple<string, float>>();
			HashSet<Tuple<string, float>> hashSet2 = new HashSet<Tuple<string, float>>();
			foreach (MyDefinitionId originalBlock in OriginalBlocks)
			{
				if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(originalBlock, out MyCubeBlockDefinition blockDefinition))
				{
					flag = true;
					MyFracturedBlock.GetAllBlockBreakableShapeNames(blockDefinition, hashSet);
				}
			}
			MyFracturedBlock.GetAllBlockBreakableShapeNames(Shape, hashSet2, 0f);
			foreach (Tuple<string, float> item in hashSet2)
			{
				bool flag2 = false;
				foreach (Tuple<string, float> item2 in hashSet)
				{
					if (item.Item1 == item2.Item1)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && flag)
				{
					item.Item1.ToLower().Contains("compound");
				}
			}
		}
	}
}
