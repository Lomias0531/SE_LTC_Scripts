using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_AdvancedDoor))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyAdvancedDoor),
		typeof(Sandbox.ModAPI.Ingame.IMyAdvancedDoor)
	})]
	public class MyAdvancedDoor : MyDoorBase, Sandbox.ModAPI.IMyAdvancedDoor, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAdvancedDoor
	{
		private class Sandbox_Game_Entities_MyAdvancedDoor_003C_003EActor : IActivator, IActivator<MyAdvancedDoor>
		{
			private sealed override object CreateInstance()
			{
				return new MyAdvancedDoor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAdvancedDoor CreateInstance()
			{
				return new MyAdvancedDoor();
			}

			MyAdvancedDoor IActivator<MyAdvancedDoor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const float CLOSED_DISSASEMBLE_RATIO = 3.3f;

		private static readonly float EPSILON = 1E-09f;

		private int m_lastUpdateTime;

		private float m_time;

		private float m_totalTime = 99999f;

		private bool m_stateChange;

		private readonly List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();

		private readonly List<int> m_subpartIDs = new List<int>();

		private readonly List<float> m_currentOpening = new List<float>();

		private readonly List<float> m_currentSpeed = new List<float>();

		private readonly List<MyEntity3DSoundEmitter> m_emitter = new List<MyEntity3DSoundEmitter>();

		private readonly List<Vector3> m_hingePosition = new List<Vector3>();

		private readonly List<MyObjectBuilder_AdvancedDoorDefinition.Opening> m_openingSequence = new List<MyObjectBuilder_AdvancedDoorDefinition.Opening>();

		private Matrix[] transMat = new Matrix[1];

		private Matrix[] rotMat = new Matrix[1];

		private int m_sequenceCount;

		private int m_subpartCount;

		public override float DisassembleRatio => base.DisassembleRatio * (base.Open ? 1f : 3.3f);

		DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status
		{
			get
			{
				float openRatio = OpenRatio;
				if ((bool)m_open)
				{
					if (!(1f - openRatio < EPSILON))
					{
						return DoorStatus.Opening;
					}
					return DoorStatus.Open;
				}
				if (!(openRatio < EPSILON))
				{
					return DoorStatus.Closing;
				}
				return DoorStatus.Closed;
			}
		}

		bool Sandbox.ModAPI.IMyDoor.IsFullyClosed => FullyClosed;

		[Obsolete("Use Sandbox.ModAPI.IMyDoor.IsFullyClosed")]
		public bool FullyClosed => m_currentOpening.FindAll((float v) => v > 0f).Count == 0;

		public bool FullyOpen
		{
			get
			{
				for (int i = 0; i < m_currentOpening.Count; i++)
				{
					if (m_openingSequence[i].MaxOpen != m_currentOpening[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		public float OpenRatio
		{
			get
			{
				for (int i = 0; i < m_currentOpening.Count; i++)
				{
					if (m_currentOpening[i] > 0f)
					{
						return m_currentOpening[i];
					}
				}
				return 0f;
			}
		}

		public float OpeningSpeed
		{
			get
			{
				for (int i = 0; i < m_currentSpeed.Count; i++)
				{
					if (m_currentSpeed[i] > 0f)
					{
						return m_currentSpeed[i];
					}
				}
				return 0f;
			}
		}

		private new MyAdvancedDoorDefinition BlockDefinition => (MyAdvancedDoorDefinition)base.BlockDefinition;

		public event Action<bool> DoorStateChanged;

		public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public MyAdvancedDoor()
		{
			m_subparts.Clear();
			m_subpartIDs.Clear();
			m_currentOpening.Clear();
			m_currentSpeed.Clear();
			m_emitter.Clear();
			m_hingePosition.Clear();
			m_openingSequence.Clear();
			m_open.ValueChanged += delegate
			{
				OnStateChange();
			};
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		private void UpdateEmissivity()
		{
			if (base.Enabled && base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 1f, Color.Green, Color.White);
				OnStateChange();
			}
			else
			{
				MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 0f, Color.Red, Color.White);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.OpenDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((Sandbox.ModAPI.Ingame.IMyDoor)this).Status;
				if ((uint)status > 1u)
				{
					((Sandbox.ModAPI.Ingame.IMyDoor)this).ToggleDoor();
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.CloseDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((Sandbox.ModAPI.Ingame.IMyDoor)this).Status;
				if ((uint)(status - 2) > 1u)
				{
					((Sandbox.ModAPI.Ingame.IMyDoor)this).ToggleDoor();
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyDoor.ToggleDoor()
		{
			if (base.IsWorking)
			{
				SetOpenRequest(!base.Open, base.OwnerId);
			}
		}

		private new static void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyAdvancedDoor>())
			{
				MyTerminalControlOnOffSwitch<MyAdvancedDoor> obj = new MyTerminalControlOnOffSwitch<MyAdvancedDoor>("Open", MySpaceTexts.Blank, default(MyStringId), MySpaceTexts.BlockAction_DoorOpen, MySpaceTexts.BlockAction_DoorClosed)
				{
					Getter = ((MyAdvancedDoor x) => x.Open),
					Setter = delegate(MyAdvancedDoor x, bool v)
					{
						x.SetOpenRequest(v, x.OwnerId);
					}
				};
				obj.EnableToggleAction();
				obj.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		private void OnStateChange()
		{
			for (int i = 0; i < m_openingSequence.Count; i++)
			{
				float speed = m_openingSequence[i].Speed;
				m_currentSpeed[i] = (m_open ? speed : (0f - speed));
			}
			base.ResourceSink.Update();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds - 1;
			UpdateCurrentOpening();
			UpdateDoorPosition();
			if ((bool)m_open)
			{
				this.DoorStateChanged.InvokeIfNotNull(m_open);
				this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
			}
			m_stateChange = true;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.PowerConsumptionMoving, UpdatePowerInput);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(builder, cubeGrid);
			MyObjectBuilder_AdvancedDoor myObjectBuilder_AdvancedDoor = (MyObjectBuilder_AdvancedDoor)builder;
			m_open.SetLocalValue(myObjectBuilder_AdvancedDoor.Open);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.Update();
			if (!base.Enabled || !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				UpdateDoorPosition();
			}
			OnStateChange();
			if ((bool)m_open)
			{
				UpdateDoorPosition();
			}
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
		}

		private MyEntitySubpart LoadSubpartFromName(string name)
		{
			base.Subparts.TryGetValue(name, out MyEntitySubpart value);
			if (value != null)
			{
				return value;
			}
			value = new MyEntitySubpart();
			string model = Path.Combine(Path.GetDirectoryName(base.Model.AssetName), name) + ".mwm";
			value.Render.EnableColorMaskHsv = base.Render.EnableColorMaskHsv;
			value.Render.ColorMaskHsv = base.Render.ColorMaskHsv;
			value.Render.TextureChanges = base.Render.TextureChanges;
			value.Render.MetalnessColorable = base.Render.MetalnessColorable;
			value.Init(null, model, this, null);
			base.Subparts[name] = value;
			if (base.InScene)
			{
				value.OnAddedToScene(this);
			}
			return value;
		}

		private void InitSubparts()
		{
			if (!base.CubeGrid.CreatePhysics)
			{
				return;
			}
			m_subparts.Clear();
			m_subpartIDs.Clear();
			m_currentOpening.Clear();
			m_currentSpeed.Clear();
			m_emitter.Clear();
			m_hingePosition.Clear();
			m_openingSequence.Clear();
			for (int i = 0; i < BlockDefinition.Subparts.Length; i++)
			{
				MyEntitySubpart myEntitySubpart = LoadSubpartFromName(BlockDefinition.Subparts[i].Name);
				if (myEntitySubpart == null)
				{
					continue;
				}
				m_subparts.Add(myEntitySubpart);
				if (!BlockDefinition.Subparts[i].PivotPosition.HasValue)
				{
					MyModelBone myModelBone = myEntitySubpart.Model.Bones.First((MyModelBone b) => !b.Name.Contains("Root"));
					if (myModelBone != null)
					{
						m_hingePosition.Add(myModelBone.Transform.Translation);
					}
				}
				else
				{
					m_hingePosition.Add(BlockDefinition.Subparts[i].PivotPosition.Value);
				}
			}
			int num = BlockDefinition.OpeningSequence.Length;
			for (int j = 0; j < num; j++)
			{
				if (!string.IsNullOrEmpty(BlockDefinition.OpeningSequence[j].IDs))
				{
					string[] array = BlockDefinition.OpeningSequence[j].IDs.Split(new char[1]
					{
						','
					});
					for (int k = 0; k < array.Length; k++)
					{
						string[] array2 = array[k].Split(new char[1]
						{
							'-'
						});
						if (array2.Length == 2)
						{
							for (int l = Convert.ToInt32(array2[0]); l <= Convert.ToInt32(array2[1]); l++)
							{
								m_openingSequence.Add(BlockDefinition.OpeningSequence[j]);
								m_subpartIDs.Add(l);
							}
						}
						else
						{
							m_openingSequence.Add(BlockDefinition.OpeningSequence[j]);
							m_subpartIDs.Add(Convert.ToInt32(array[k]));
						}
					}
				}
				else
				{
					m_openingSequence.Add(BlockDefinition.OpeningSequence[j]);
					m_subpartIDs.Add(BlockDefinition.OpeningSequence[j].ID);
				}
			}
			for (int m = 0; m < m_openingSequence.Count; m++)
			{
				m_currentOpening.Add(0f);
				m_currentSpeed.Add(0f);
				m_emitter.Add(new MyEntity3DSoundEmitter(this, useStaticList: true));
				if (m_openingSequence[m].MaxOpen < 0f)
				{
					m_openingSequence[m].MaxOpen *= -1f;
					m_openingSequence[m].InvertRotation = !m_openingSequence[m].InvertRotation;
				}
			}
			m_sequenceCount = m_openingSequence.Count;
			m_subpartCount = m_subparts.Count;
			Array.Resize(ref transMat, m_subpartCount);
			Array.Resize(ref rotMat, m_subpartCount);
			UpdateDoorPosition();
			if (base.CubeGrid.Projector == null)
			{
				foreach (MyEntitySubpart subpart in m_subparts)
				{
					if (subpart.Physics != null)
					{
						subpart.Physics.Close();
						subpart.Physics = null;
					}
					if (subpart != null && subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
					{
						HkShape[] havokCollisionShapes = subpart.ModelCollision.HavokCollisionShapes;
						HkListShape shape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
						subpart.Physics = new MyPhysicsBody(subpart, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
						subpart.Physics.IsPhantom = false;
						(subpart.Physics as MyPhysicsBody).CreateFromCollisionObject(shape, Vector3.Zero, base.WorldMatrix);
						subpart.Physics.Enabled = true;
						shape.Base.RemoveReference();
					}
				}
				base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
				base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_HavokSystemIDChanged;
				base.CubeGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
				base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
				if (base.CubeGrid.Physics != null)
				{
					UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
				}
			}
		}

		private void CubeGrid_OnPhysicsChanged(MyEntity obj)
		{
			if (m_subparts != null && m_subparts.Count != 0 && obj.Physics != null && m_subparts[0].Physics != null)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateHavokCollisionSystemID(base.CubeGrid);
		}

		private void UpdateHavokCollisionSystemID(MyEntity obj)
		{
			if (obj != null && !obj.MarkedForClose && obj.GetPhysicsBody() != null && m_subparts[0].GetPhysicsBody() != null && obj.GetPhysicsBody().HavokCollisionSystemID != m_subparts[0].GetPhysicsBody().HavokCollisionSystemID)
			{
				UpdateHavokCollisionSystemID(obj.GetPhysicsBody().HavokCollisionSystemID);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_AdvancedDoor obj = (MyObjectBuilder_AdvancedDoor)base.GetObjectBuilderCubeBlock(copy);
			obj.Open = m_open;
			return obj;
		}

		protected float UpdatePowerInput()
		{
			if (!base.Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			if (OpeningSpeed == 0f)
			{
				return BlockDefinition.PowerConsumptionIdle;
			}
			return BlockDefinition.PowerConsumptionMoving;
		}

		private void StartSound(int emitterId, MySoundPair cuePair)
		{
			if (m_emitter[emitterId].Sound == null || !m_emitter[emitterId].Sound.IsPlaying || (!(m_emitter[emitterId].SoundId == cuePair.Arcade) && !(m_emitter[emitterId].SoundId == cuePair.Realistic)))
			{
				m_emitter[emitterId].StopSound(forced: true);
				m_emitter[emitterId].PlaySingleSound(cuePair);
			}
		}

		public override void UpdateSoundEmitters()
		{
			for (int i = 0; i < m_emitter.Count; i++)
			{
				if (m_emitter[i] != null)
				{
					m_emitter[i].Update();
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.Physics != null)
			{
				UpdateDoorPosition();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (FullyClosed)
			{
				m_time = 0f;
			}
			else if (FullyOpen)
			{
				if (m_totalTime != m_time)
				{
					m_totalTime = m_time;
				}
				m_time = m_totalTime;
			}
			for (int i = 0; i < m_openingSequence.Count; i++)
			{
				float maxOpen = m_openingSequence[i].MaxOpen;
				if ((base.Open && m_currentOpening[i] == maxOpen) || (!base.Open && m_currentOpening[i] == 0f))
				{
					if (m_emitter[i] != null && m_emitter[i].IsPlaying && m_emitter[i].Loop)
					{
						m_emitter[i].StopSound(forced: false);
					}
					m_currentSpeed[i] = 0f;
				}
				if (base.Enabled && base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && m_currentSpeed[i] != 0f)
				{
					string text = "";
					text = ((!base.Open) ? m_openingSequence[i].CloseSound : m_openingSequence[i].OpenSound);
					if (!string.IsNullOrEmpty(text))
					{
						StartSound(i, new MySoundPair(text));
					}
				}
				else if (m_emitter[i] != null)
				{
					m_emitter[i].StopSound(forced: false);
				}
			}
			if (m_stateChange && (((bool)m_open && FullyOpen) || (!m_open && FullyClosed)))
			{
				base.ResourceSink.Update();
				RaisePropertiesChanged();
				if (!m_open)
				{
					this.DoorStateChanged.InvokeIfNotNull(m_open);
					this.OnDoorStateChanged.InvokeIfNotNull(this, m_open);
				}
				m_stateChange = false;
			}
			base.UpdateBeforeSimulation();
			UpdateCurrentOpening();
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void UpdateCurrentOpening()
		{
			if (!base.Enabled || base.ResourceSink == null || !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return;
			}
			float num = (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) / 1000f;
			m_time += (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime) / 1000f * (m_open ? 1f : (-1f));
			m_time = MathHelper.Clamp(m_time, 0f, m_totalTime);
			for (int i = 0; i < m_openingSequence.Count; i++)
			{
				float num2 = m_open ? m_openingSequence[i].OpenDelay : m_openingSequence[i].CloseDelay;
				if (((bool)m_open && m_time > num2) || (!m_open && m_time < m_totalTime - num2))
				{
					float num3 = m_currentSpeed[i] * num;
					float maxOpen = m_openingSequence[i].MaxOpen;
					if (m_openingSequence[i].SequenceType == MyObjectBuilder_AdvancedDoorDefinition.Opening.Sequence.Linear)
					{
						m_currentOpening[i] = MathHelper.Clamp(m_currentOpening[i] + num3, 0f, maxOpen);
					}
				}
			}
		}

		private void UpdateDoorPosition()
		{
			if (base.CubeGrid.Physics == null)
			{
				return;
			}
			for (int i = 0; i < m_subpartCount; i++)
			{
				transMat[i] = Matrix.Identity;
				rotMat[i] = Matrix.Identity;
			}
			for (int j = 0; j < m_sequenceCount; j++)
			{
				MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType move = m_openingSequence[j].Move;
				float num = m_currentOpening[j];
				int num2 = m_subpartIDs[j];
				if (m_subparts.Count == 0 || num2 < 0)
				{
					break;
				}
				if (m_subparts[num2] == null || m_subparts[num2].Physics == null)
				{
					continue;
				}
				switch (move)
				{
				case MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType.Slide:
					transMat[num2] *= Matrix.CreateTranslation(m_openingSequence[j].SlideDirection * new Vector3(num));
					break;
				case MyObjectBuilder_AdvancedDoorDefinition.Opening.MoveType.Rotate:
				{
					float num3 = m_openingSequence[j].InvertRotation ? (-1f) : 1f;
					float radians = 0f;
					float radians2 = 0f;
					float radians3 = 0f;
					if (m_openingSequence[j].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.X)
					{
						radians = MathHelper.ToRadians(num * num3);
					}
					else if (m_openingSequence[j].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.Y)
					{
						radians2 = MathHelper.ToRadians(num * num3);
					}
					else if (m_openingSequence[j].RotationAxis == MyObjectBuilder_AdvancedDoorDefinition.Opening.Rotation.Z)
					{
						radians3 = MathHelper.ToRadians(num * num3);
					}
					Vector3 vector = (!m_openingSequence[j].PivotPosition.HasValue) ? m_hingePosition[num2] : ((Vector3)m_openingSequence[j].PivotPosition.Value);
					rotMat[num2] *= Matrix.CreateTranslation(-vector) * (Matrix.CreateRotationX(radians) * Matrix.CreateRotationY(radians2) * Matrix.CreateRotationZ(radians3)) * Matrix.CreateTranslation(vector);
					break;
				}
				}
				if (m_subparts[num2].Physics.LinearVelocity != base.CubeGrid.Physics.LinearVelocity)
				{
					m_subparts[num2].Physics.LinearVelocity = base.CubeGrid.Physics.LinearVelocity;
				}
				if (m_subparts[num2].Physics.AngularVelocity != base.CubeGrid.Physics.AngularVelocity)
				{
					m_subparts[num2].Physics.AngularVelocity = base.CubeGrid.Physics.AngularVelocity;
				}
			}
			for (int k = 0; k < m_subpartCount; k++)
			{
				m_subparts[k].PositionComp.LocalMatrix = rotMat[k] * transMat[k];
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			oldGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_HavokSystemIDChanged;
			if (base.CubeGrid.Physics != null)
			{
				UpdateHavokCollisionSystemID(base.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
			}
			base.OnCubeGridChanged(oldGrid);
		}

		private void CubeGrid_HavokSystemIDChanged(int id)
		{
			UpdateHavokCollisionSystemID(id);
		}

		internal void UpdateHavokCollisionSystemID(int HavokCollisionSystemID)
		{
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				if (subpart != null && subpart.Physics != null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
				{
					uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(15, HavokCollisionSystemID, 1, 1);
					subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
					if (subpart.GetPhysicsBody().HavokWorld != null)
					{
						subpart.GetPhysicsBody().HavokWorld.RefreshCollisionFilterOnEntity(subpart.Physics.RigidBody);
					}
				}
			}
		}

		protected override void Closing()
		{
			for (int i = 0; i < m_emitter.Count; i++)
			{
				if (m_emitter[i] != null)
				{
					m_emitter[i].StopSound(forced: true);
				}
			}
			base.CubeGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
			base.Closing();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			InitSubparts();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateEmissivity();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}
	}
}
