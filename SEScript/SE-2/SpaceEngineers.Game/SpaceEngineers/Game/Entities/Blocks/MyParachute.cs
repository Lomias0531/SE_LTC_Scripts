using Havok;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Parachute))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyParachute),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)
	})]
	public class MyParachute : MyDoorBase, SpaceEngineers.Game.ModAPI.IMyParachute, SpaceEngineers.Game.ModAPI.Ingame.IMyParachute, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyConveyorEndpointBlock
	{
		protected sealed class AutoDeployRequest_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<MyParachute, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyParachute @this, in bool autodeploy, in long identityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AutoDeployRequest(autodeploy, identityId);
			}
		}

		protected sealed class DeployHeightRequest_003C_003ESystem_Single_0023System_Int64 : ICallSite<MyParachute, float, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyParachute @this, in float deployHeight, in long identityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DeployHeightRequest(deployHeight, identityId);
			}
		}

		protected sealed class DoDeployChute_003C_003E : ICallSite<MyParachute, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyParachute @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoDeployChute();
			}
		}

		protected class m_autoDeploy_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoDeploy;
				ISyncType result = autoDeploy = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyParachute)P_0).m_autoDeploy = (Sync<bool, SyncDirection.BothWays>)autoDeploy;
				return result;
			}
		}

		protected class m_deployHeight_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType deployHeight;
				ISyncType result = deployHeight = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyParachute)P_0).m_deployHeight = (Sync<float, SyncDirection.BothWays>)deployHeight;
				return result;
			}
		}

		private static readonly float EPSILON = 1E-09f;

		private const float MIN_DEPLOYHEIGHT = 10f;

		private const float MAX_DEPLOYHEIGHT = 10000f;

		private const double DENSITYOFAIRINONEATMO = 1.225;

		private const float NO_DRAG_SPEED_SQRD = 0.1f;

		private const float NO_DRAG_SPEED_RANGE = 20f;

		private int m_lastUpdateTime;

		private float m_time;

		private float m_totalTime = 99999f;

		private bool m_stateChange;

		private List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();

		private List<int> m_subpartIDs = new List<int>();

		private List<float> m_currentOpening = new List<float>();

		private List<float> m_currentSpeed = new List<float>();

		private List<MyEntity3DSoundEmitter> m_emitter = new List<MyEntity3DSoundEmitter>();

		private List<Vector3> m_hingePosition = new List<Vector3>();

		private List<MyObjectBuilder_ParachuteDefinition.Opening> m_openingSequence = new List<MyObjectBuilder_ParachuteDefinition.Opening>();

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private Matrix[] transMat = new Matrix[1];

		private Matrix[] rotMat = new Matrix[1];

		private int m_sequenceCount;

		private int m_subpartCount;

		protected readonly Sync<bool, SyncDirection.BothWays> m_autoDeploy;

		protected readonly Sync<float, SyncDirection.BothWays> m_deployHeight;

		private MyPlanet m_nearPlanetCache;

		private MyEntitySubpart m_parachuteSubpart;

		private Vector3 m_lastParachuteVelocityVector = Vector3.Zero;

		private Vector3 m_lastParachuteScale = Vector3.Zero;

		private Vector3 m_gravityCache = Vector3.Zero;

		private Vector3D m_chuteScale = Vector3D.Zero;

		private Vector3D? m_closestPointCache;

		private int m_parachuteAnimationState;

		private int m_cutParachuteTimer;

		private bool m_canDeploy;

		private bool m_canCheckAutoDeploy;

		private bool m_atmosphereDirty = true;

		private float m_minAtmosphere = 0.2f;

		private float m_dragCoefficient = 1f;

		private float m_atmosphereDensityCache;

		private MyFixedPoint m_requiredItemsInInventory = 0;

		private Quaternion m_lastParachuteRotation = Quaternion.Identity;

		private Matrix m_lastParachuteLocalMatrix = Matrix.Identity;

		private MatrixD m_lastParachuteWorldMatrix = MatrixD.Identity;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		DoorStatus SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.Status
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

		public bool AutoDeploy
		{
			get
			{
				return m_autoDeploy;
			}
			set
			{
				if ((bool)m_autoDeploy != value)
				{
					m_autoDeploy.Value = value;
				}
			}
		}

		public float DeployHeight
		{
			get
			{
				return m_deployHeight;
			}
			set
			{
				value = MathHelper.Clamp(value, 10f, 10000f);
				if ((float)m_deployHeight != value)
				{
					m_deployHeight.Value = value;
				}
			}
		}

		public float DragCoefficient => m_dragCoefficient;

		public bool CanDeploy
		{
			get
			{
				return m_canDeploy;
			}
			set
			{
				m_canDeploy = value;
			}
		}

		public float Atmosphere
		{
			get
			{
				if (!m_atmosphereDirty)
				{
					return m_atmosphereDensityCache;
				}
				m_atmosphereDirty = false;
				if (m_nearPlanetCache == null)
				{
					return m_atmosphereDensityCache = 0f;
				}
				return m_atmosphereDensityCache = m_nearPlanetCache.GetAirDensity(base.WorldMatrix.Translation);
			}
		}

		private new MyParachuteDefinition BlockDefinition => (MyParachuteDefinition)base.BlockDefinition;

		private event Action<bool> DoorStateChanged;

		event Action<bool> SpaceEngineers.Game.ModAPI.IMyParachute.DoorStateChanged
		{
			add
			{
				DoorStateChanged += value;
			}
			remove
			{
				DoorStateChanged -= value;
			}
		}

		private event Action<bool> ParachuteStateChanged;

		event Action<bool> SpaceEngineers.Game.ModAPI.IMyParachute.ParachuteStateChanged
		{
			add
			{
				ParachuteStateChanged += value;
			}
			remove
			{
				ParachuteStateChanged -= value;
			}
		}

		public MyParachute()
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

		void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.OpenDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).Status;
				if ((uint)status > 1u)
				{
					((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).ToggleDoor();
				}
			}
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.CloseDoor()
		{
			if (base.IsWorking)
			{
				DoorStatus status = ((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).Status;
				if ((uint)(status - 2) > 1u)
				{
					((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).ToggleDoor();
				}
			}
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.ToggleDoor()
		{
			if (base.IsWorking)
			{
				SetOpenRequest(!base.Open, base.OwnerId);
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
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

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyParachute>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyParachute> obj = new MyTerminalControlCheckbox<MyParachute>("AutoDeploy", MySpaceTexts.Parachute_AutoDeploy, MySpaceTexts.Parachute_AutoDeployTooltip, MySpaceTexts.Parachute_AutoDeployOn, MySpaceTexts.Parachute_AutoDeployOff)
				{
					Getter = ((MyParachute x) => x.AutoDeploy),
					Setter = delegate(MyParachute x, bool v)
					{
						x.SetAutoDeployRequest(v, x.OwnerId);
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlSlider<MyParachute> myTerminalControlSlider = new MyTerminalControlSlider<MyParachute>("AutoDeployHeight", MySpaceTexts.Parachute_DeployHeightTitle, MySpaceTexts.Parachute_DeployHeightTooltip);
				myTerminalControlSlider.Getter = ((MyParachute x) => x.DeployHeight);
				myTerminalControlSlider.Setter = delegate(MyParachute x, float v)
				{
					x.SetDeployHeightRequest(v, x.OwnerId);
				};
				myTerminalControlSlider.Writer = delegate(MyParachute b, StringBuilder v)
				{
					v.Append($"{b.DeployHeight:N0} m");
				};
				myTerminalControlSlider.SetLogLimits(10f, 10000f);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

		public void SetAutoDeployRequest(bool autodeploy, long identityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyParachute x) => x.AutoDeployRequest, autodeploy, identityId);
		}

		[Event(null, 338)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void AutoDeployRequest(bool autodeploy, long identityId)
		{
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(identityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyPlayer myPlayer = (myIdentity != null && myIdentity.Character != null) ? MyPlayer.GetPlayerFromCharacter(myIdentity.Character) : null;
			bool flag = false;
			if (myPlayer != null && !userRelationToOwner.IsFriendly() && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out AdminSettingsEnum value))
			{
				flag = value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			if (userRelationToOwner.IsFriendly() || flag)
			{
				AutoDeploy = autodeploy;
			}
		}

		public void SetDeployHeightRequest(float deployHeight, long identityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyParachute x) => x.DeployHeightRequest, deployHeight, identityId);
		}

		[Event(null, 364)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DeployHeightRequest(float deployHeight, long identityId)
		{
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(identityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyPlayer myPlayer = (myIdentity != null && myIdentity.Character != null) ? MyPlayer.GetPlayerFromCharacter(myIdentity.Character) : null;
			bool flag = false;
			if (myPlayer != null && !userRelationToOwner.IsFriendly() && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out AdminSettingsEnum value))
			{
				flag = value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			if (userRelationToOwner.IsFriendly() || flag)
			{
				DeployHeight = deployHeight;
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
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds - 1;
			UpdateCurrentOpening();
			UpdateDoorPosition();
			if ((bool)m_open)
			{
				this.DoorStateChanged?.Invoke(m_open);
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
			MyObjectBuilder_Parachute myObjectBuilder_Parachute = (MyObjectBuilder_Parachute)builder;
			m_open.Value = myObjectBuilder_Parachute.Open;
			m_deployHeight.Value = myObjectBuilder_Parachute.DeployHeight;
			m_autoDeploy.Value = myObjectBuilder_Parachute.AutoDeploy;
			m_parachuteAnimationState = myObjectBuilder_Parachute.ParachuteState;
			if (m_parachuteAnimationState > 50)
			{
				m_parachuteAnimationState = 0;
			}
			m_dragCoefficient = BlockDefinition.DragCoefficient;
			m_minAtmosphere = BlockDefinition.MinimumAtmosphereLevel;
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
			InitializeConveyorEndpoint();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
			MyInventory myInventory = this.GetInventory();
			MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(BlockDefinition.MaterialDefinitionId);
			if (myInventory == null)
			{
				Vector3 one = Vector3.One;
				myInventory = new MyInventory(componentDefinition.Volume * (float)BlockDefinition.MaterialDeployCost, one, MyInventoryFlags.CanReceive);
				base.Components.Add((MyInventoryBase)myInventory);
			}
			inventory_ContentsChanged(myInventory);
			myInventory.ContentsChanged += inventory_ContentsChanged;
			MyInventoryConstraint myInventoryConstraint = new MyInventoryConstraint(MySpaceTexts.Parachute_ConstraintItem);
			myInventoryConstraint.Add(BlockDefinition.MaterialDefinitionId);
			myInventoryConstraint.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_COMPONENT;
			myInventory.Constraint = myInventoryConstraint;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		private MyEntitySubpart LoadSubpartFromName(string name)
		{
			if (base.Subparts.TryGetValue(name, out MyEntitySubpart value))
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
					subpart.Physics = null;
					if (subpart != null && subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
					{
						HkShape[] havokCollisionShapes = subpart.ModelCollision.HavokCollisionShapes;
						HkListShape shape = new HkListShape(havokCollisionShapes, havokCollisionShapes.Length, HkReferencePolicy.None);
						subpart.Physics = new MyPhysicsBody(subpart, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_DOUBLED_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
						subpart.Physics.IsPhantom = false;
						(subpart.Physics as MyPhysicsBody).CreateFromCollisionObject(shape, Vector3.Zero, base.WorldMatrix, null, 17);
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
			if (m_subparts != null && m_subparts.Count != 0 && obj.Physics != null && m_subparts[0].Physics != null && obj.GetPhysicsBody().HavokCollisionSystemID != m_subparts[0].GetPhysicsBody().HavokCollisionSystemID)
			{
				UpdateHavokCollisionSystemID(obj.GetPhysicsBody().HavokCollisionSystemID);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Parachute obj = (MyObjectBuilder_Parachute)base.GetObjectBuilderCubeBlock(copy);
			obj.Open = m_open;
			obj.AutoDeploy = m_autoDeploy;
			obj.DeployHeight = m_deployHeight;
			obj.ParachuteState = m_parachuteAnimationState;
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

		public override void UpdateOnceBeforeFrame()
		{
			UpdateNearPlanet();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.Physics != null)
			{
				m_atmosphereDirty = true;
				UpdateDoorPosition();
				UpdateParachutePosition();
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			if (base.CubeGrid.Physics != null)
			{
				m_gravityCache = GetTotalGravity();
				m_canCheckAutoDeploy = false;
				UpdateNearPlanet();
				if (!CanDeploy)
				{
					AttemptPullRequiredInventoryItems();
				}
				if (AutoDeploy && CanDeploy && base.CubeGrid.Physics.LinearVelocity.LengthSquared() > 2f && Atmosphere > m_minAtmosphere && Vector3.Dot(m_gravityCache, base.CubeGrid.Physics.LinearVelocity) > 0.6f)
				{
					m_canCheckAutoDeploy = TryGetClosestPoint(out m_closestPointCache);
				}
			}
			base.UpdateBeforeSimulation10();
		}

		public override void UpdateBeforeSimulation()
		{
			m_atmosphereDirty = true;
			if (FullyClosed)
			{
				m_time = 0f;
				UpdateCutChute();
				if (m_parachuteSubpart != null && m_parachuteSubpart.Render.RenderObjectIDs[0] != uint.MaxValue)
				{
					m_parachuteSubpart.Render.Visible = false;
				}
			}
			else if (FullyOpen)
			{
				if (m_totalTime != m_time)
				{
					m_totalTime = m_time;
				}
				m_time = m_totalTime;
				UpdateParachute();
			}
			else
			{
				UpdateParachute();
			}
			if (Sync.IsServer && m_canCheckAutoDeploy)
			{
				CheckAutoDeploy();
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
					this.DoorStateChanged?.Invoke(m_open);
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
					if (m_openingSequence[i].SequenceType == MyObjectBuilder_ParachuteDefinition.Opening.Sequence.Linear)
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
				MyObjectBuilder_ParachuteDefinition.Opening.MoveType move = m_openingSequence[j].Move;
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
				case MyObjectBuilder_ParachuteDefinition.Opening.MoveType.Slide:
					transMat[num2] *= Matrix.CreateTranslation(m_openingSequence[j].SlideDirection * new Vector3(num));
					break;
				case MyObjectBuilder_ParachuteDefinition.Opening.MoveType.Rotate:
				{
					float num3 = m_openingSequence[j].InvertRotation ? (-1f) : 1f;
					float radians = 0f;
					float radians2 = 0f;
					float radians3 = 0f;
					if (m_openingSequence[j].RotationAxis == MyObjectBuilder_ParachuteDefinition.Opening.Rotation.X)
					{
						radians = MathHelper.ToRadians(num * num3);
					}
					else if (m_openingSequence[j].RotationAxis == MyObjectBuilder_ParachuteDefinition.Opening.Rotation.Y)
					{
						radians2 = MathHelper.ToRadians(num * num3);
					}
					else if (m_openingSequence[j].RotationAxis == MyObjectBuilder_ParachuteDefinition.Opening.Rotation.Z)
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

		private bool CheckDeployChute()
		{
			if (base.CubeGrid.Physics == null)
			{
				return false;
			}
			if (!CanDeploy)
			{
				return false;
			}
			if (m_parachuteAnimationState > 0)
			{
				return false;
			}
			if (Atmosphere < m_minAtmosphere)
			{
				return false;
			}
			if (!MySession.Static.CreativeMode)
			{
				if (!(this.GetInventory().GetItemAmount(BlockDefinition.MaterialDefinitionId) >= BlockDefinition.MaterialDeployCost))
				{
					CanDeploy = false;
					return false;
				}
				this.GetInventory().RemoveItemsOfType(BlockDefinition.MaterialDeployCost, BlockDefinition.MaterialDefinitionId);
			}
			MyMultiplayer.RaiseEvent(this, (MyParachute x) => x.DoDeployChute);
			return true;
		}

		[Event(null, 997)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private void DoDeployChute()
		{
			m_parachuteAnimationState = 1;
			m_lastParachuteRotation = Quaternion.Identity;
			m_lastParachuteScale = Vector3.Zero;
			m_cutParachuteTimer = 0;
			if (m_parachuteSubpart == null)
			{
				m_parachuteSubpart = LoadSubpartFromName(BlockDefinition.ParachuteSubpartName);
			}
			m_parachuteSubpart.Render.Visible = true;
			if (this.ParachuteStateChanged != null)
			{
				this.ParachuteStateChanged(obj: true);
			}
		}

		private void RemoveChute()
		{
			m_parachuteAnimationState = 0;
			if (m_parachuteSubpart != null)
			{
				m_parachuteSubpart.Render.Visible = false;
			}
		}

		/// <summary>
		/// Called from game update. only called when door is opened fully closing or opening.
		/// </summary>
		private void UpdateParachute()
		{
			if (base.CubeGrid.Physics == null)
			{
				return;
			}
			if (m_parachuteAnimationState > 50)
			{
				if (!Sync.IsServer || !CanDeploy || !FullyOpen || !CheckDeployChute())
				{
					UpdateCutChute();
				}
				return;
			}
			if (m_parachuteAnimationState == 0 && Sync.IsServer && CanDeploy && FullyOpen)
			{
				CheckDeployChute();
			}
			if (m_parachuteAnimationState > 0 && m_parachuteAnimationState < 50)
			{
				m_parachuteAnimationState++;
			}
			Vector3 zero = Vector3.Zero;
			bool flag = false;
			float num = base.CubeGrid.Physics.LinearVelocity.LengthSquared();
			if (num > 2f)
			{
				zero = base.CubeGrid.Physics.LinearVelocity;
				m_cutParachuteTimer = 0;
			}
			else if (0.1f > num)
			{
				flag = true;
				zero = Vector3.Lerp(m_lastParachuteVelocityVector, -m_gravityCache, 0.05f);
				if (Vector3.Distance(zero, -m_gravityCache) < 0.05f)
				{
					m_cutParachuteTimer++;
					if (m_cutParachuteTimer > 60)
					{
						if (Sync.IsServer)
						{
							((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).CloseDoor();
						}
						UpdateCutChute();
						return;
					}
				}
			}
			else
			{
				flag = true;
				zero = base.CubeGrid.Physics.LinearVelocity;
			}
			double num2 = 10.0 * (double)(Atmosphere - BlockDefinition.ReefAtmosphereLevel) * ((double)m_parachuteAnimationState / 50.0);
			if (num2 <= 0.5 || double.IsNaN(num2))
			{
				num2 = 0.5;
			}
			else
			{
				num2 = Math.Log(num2 - 0.99) + 5.0;
				if (num2 < 0.5 || double.IsNaN(num2))
				{
					num2 = 0.5;
				}
			}
			m_chuteScale.Z = Math.Log((double)m_parachuteAnimationState / 1.5) * (double)base.CubeGrid.GridSize * 20.0;
			m_chuteScale.X = (m_chuteScale.Y = num2 * (double)BlockDefinition.RadiusMultiplier * (double)base.CubeGrid.GridSize);
			m_lastParachuteVelocityVector = zero;
			Vector3D vector3D = Vector3D.Normalize(zero);
			Quaternion quaternion = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromDir(vector3D, new Vector3(0f, 1f, 0f)).GetOrientation());
			quaternion = Quaternion.Lerp(m_lastParachuteRotation, quaternion, 0.02f);
			m_chuteScale = Vector3D.Lerp(m_lastParachuteScale, m_chuteScale, 0.02);
			double num3 = m_chuteScale.X / 2.0;
			m_lastParachuteScale = m_chuteScale;
			m_lastParachuteRotation = quaternion;
			MatrixD matrix = MatrixD.Invert(base.WorldMatrix);
			m_lastParachuteWorldMatrix = MatrixD.CreateFromTransformScale(m_lastParachuteRotation, base.WorldMatrix.Translation + base.WorldMatrix.Up * ((double)base.CubeGrid.GridSize / 2.0), m_lastParachuteScale);
			m_lastParachuteLocalMatrix = m_lastParachuteWorldMatrix * matrix;
			if (!(num3 <= 0.0 || flag) && !(zero.LengthSquared() <= 1f))
			{
				Vector3D value = -vector3D;
				double num4 = Math.PI * num3 * num3;
				double num5 = 2.5 * ((double)Atmosphere * 1.225) * (double)zero.LengthSquared() * num4 * (double)DragCoefficient;
				if (num5 > 0.0 && !base.CubeGrid.Physics.IsStatic)
				{
					base.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, Vector3D.Multiply(value, num5), base.WorldMatrix.Translation, Vector3.Zero);
				}
			}
		}

		private void UpdateParachutePosition()
		{
			if (m_parachuteSubpart != null && m_parachuteAnimationState > 0)
			{
				m_parachuteSubpart.PositionComp.SetLocalMatrix(ref m_lastParachuteLocalMatrix, null, updateWorld: true);
			}
		}

		/// <summary>
		/// Called each tick when door is closed or called from UpdateParachutePosition if the door is opening/closing/fullyopen after being closed. 
		/// </summary>
		private void UpdateCutChute()
		{
			if (base.CubeGrid.Physics == null || m_parachuteAnimationState == 0)
			{
				return;
			}
			if (m_parachuteAnimationState > 100)
			{
				RemoveChute();
				return;
			}
			if (m_parachuteAnimationState < 50)
			{
				m_parachuteAnimationState = 50;
			}
			if (m_parachuteAnimationState == 50 && this.ParachuteStateChanged != null)
			{
				this.ParachuteStateChanged(obj: false);
			}
			m_parachuteAnimationState++;
			if (m_parachuteSubpart != null)
			{
				m_lastParachuteWorldMatrix.Translation += m_gravityCache * 0.05f;
				Matrix localMatrix = m_lastParachuteWorldMatrix * MatrixD.Invert(base.WorldMatrix);
				m_parachuteSubpart.PositionComp.SetLocalMatrix(ref localMatrix, null, updateWorld: true);
			}
		}

		private void CheckAutoDeploy()
		{
			if (m_closestPointCache.HasValue && Vector3D.Distance(m_closestPointCache.Value, base.WorldMatrix.Translation) < (double)DeployHeight)
			{
				((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).OpenDoor();
			}
		}

		private void UpdateNearPlanet()
		{
			BoundingBoxD box = base.PositionComp.WorldAABB;
			m_nearPlanetCache = MyGamePruningStructure.GetClosestPlanet(ref box);
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			oldGrid.OnHavokSystemIDChanged -= CubeGrid_HavokSystemIDChanged;
			base.CubeGrid.OnHavokSystemIDChanged += CubeGrid_HavokSystemIDChanged;
			oldGrid.OnPhysicsChanged -= CubeGrid_OnPhysicsChanged;
			base.CubeGrid.OnPhysicsChanged += CubeGrid_OnPhysicsChanged;
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
					uint collisionFilterInfo = HkGroupFilter.CalcFilterInfo(17, HavokCollisionSystemID, 1, 1);
					subpart.Physics.RigidBody.SetCollisionFilterInfo(collisionFilterInfo);
					collisionFilterInfo = HkGroupFilter.CalcFilterInfo(16, HavokCollisionSystemID, 1, 1);
					subpart.Physics.RigidBody2.SetCollisionFilterInfo(collisionFilterInfo);
					if (subpart.GetPhysicsBody().HavokWorld != null)
					{
						subpart.GetPhysicsBody().HavokWorld.RefreshCollisionFilterOnEntity(subpart.Physics.RigidBody);
						subpart.GetPhysicsBody().HavokWorld.RefreshCollisionFilterOnEntity(subpart.Physics.RigidBody2);
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

		public void AttemptPullRequiredInventoryItems()
		{
			if (BlockDefinition.MaterialDeployCost > m_requiredItemsInInventory)
			{
				base.CubeGrid.GridSystems.ConveyorSystem.PullItem(BlockDefinition.MaterialDefinitionId, BlockDefinition.MaterialDeployCost - m_requiredItemsInInventory, this, this.GetInventory(), remove: false, calcImmediately: false);
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateEmissivity();
		}

		private void inventory_ContentsChanged(MyInventoryBase obj)
		{
			if (MySession.Static.CreativeMode)
			{
				CanDeploy = true;
				return;
			}
			m_requiredItemsInInventory = obj.GetItemAmount(BlockDefinition.MaterialDefinitionId);
			if (m_requiredItemsInInventory >= BlockDefinition.MaterialDeployCost)
			{
				CanDeploy = true;
			}
			else
			{
				CanDeploy = false;
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				ItemDefinition = BlockDefinition.MaterialDefinitionId
			};
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		public bool TryGetClosestPoint(out Vector3D? closestPoint)
		{
			closestPoint = null;
			if (!MyGravityProviderSystem.IsPositionInNaturalGravity(base.PositionComp.GetPosition()))
			{
				return false;
			}
			BoundingBoxD box = base.PositionComp.WorldAABB;
			m_nearPlanetCache = MyGamePruningStructure.GetClosestPlanet(ref box);
			if (m_nearPlanetCache == null)
			{
				return false;
			}
			Vector3D globalPos = base.CubeGrid.Physics.CenterOfMassWorld;
			closestPoint = m_nearPlanetCache.GetClosestSurfacePointGlobal(ref globalPos);
			return true;
		}

		public Vector3D GetVelocity()
		{
			MyPhysicsComponentBase myPhysicsComponentBase = (base.Parent != null) ? base.Parent.Physics : null;
			if (myPhysicsComponentBase != null)
			{
				return new Vector3D(myPhysicsComponentBase.GetVelocityAtPoint(base.PositionComp.GetPosition()));
			}
			return Vector3D.Zero;
		}

		public Vector3D GetNaturalGravity()
		{
			return MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.WorldMatrix.Translation);
		}

		public Vector3D GetArtificialGravity()
		{
			return MyGravityProviderSystem.CalculateArtificialGravityInPoint(base.WorldMatrix.Translation);
		}

		public Vector3D GetTotalGravity()
		{
			return MyGravityProviderSystem.CalculateTotalGravityInPoint(base.WorldMatrix.Translation);
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}
	}
}
