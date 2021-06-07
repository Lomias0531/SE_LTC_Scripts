using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MyIdentity : IMyIdentity
	{
		public class Friend
		{
			public virtual MyIdentity CreateNewIdentity(string name, string model = null, Vector3? colorMask = null)
			{
				return new MyIdentity(name, MyEntityIdentifier.ID_OBJECT_TYPE.IDENTITY, model, colorMask);
			}

			public virtual MyIdentity CreateNewIdentity(string name, long identityId, string model, Vector3? colorMask)
			{
				return new MyIdentity(name, identityId, model, colorMask);
			}

			public virtual MyIdentity CreateNewIdentity(MyObjectBuilder_Identity objectBuilder)
			{
				return new MyIdentity(objectBuilder);
			}
		}

		public class BuildPlanItem
		{
			public class Component
			{
				public MyComponentDefinition ComponentDefinition;

				public int Count;

				public Component Clone()
				{
					return new Component
					{
						ComponentDefinition = ComponentDefinition,
						Count = Count
					};
				}
			}

			public MyCubeBlockDefinition BlockDefinition;

			public bool IsInProgress;

			public List<Component> Components;

			public BuildPlanItem Clone()
			{
				BuildPlanItem buildPlanItem = new BuildPlanItem();
				buildPlanItem.Components = new List<Component>();
				buildPlanItem.IsInProgress = IsInProgress;
				buildPlanItem.BlockDefinition = BlockDefinition;
				foreach (Component component in Components)
				{
					buildPlanItem.Components.Add(component.Clone());
				}
				return buildPlanItem;
			}
		}

		private MyBlockLimits m_blockLimits;

		private static readonly Dictionary<string, short> EmptyBlockTypeLimitDictionary = new Dictionary<string, short>();

		private List<BuildPlanItem> m_buildPlanner = new List<BuildPlanItem>();

		public List<long> RespawnShips = new List<long>();

		public List<long> ActiveContracts = new List<long>();

		public long IdentityId
		{
			get;
			private set;
		}

		public string DisplayName
		{
			get;
			private set;
		}

		public MyCharacter Character
		{
			get;
			private set;
		}

		public HashSet<long> SavedCharacters
		{
			get;
			private set;
		}

		public string Model
		{
			get;
			private set;
		}

		public Vector3? ColorMask
		{
			get;
			private set;
		}

		public bool IsDead
		{
			get;
			private set;
		}

		public Vector3D? LastDeathPosition
		{
			get;
			private set;
		}

		public TimeSpan LastRespawnTime
		{
			get;
			private set;
		}

		public bool FirstSpawnDone
		{
			get;
			private set;
		}

		public DateTime LastLoginTime
		{
			get;
			set;
		}

		public DateTime LastLogoutTime
		{
			get;
			set;
		}

		public MyBlockLimits BlockLimits
		{
			get
			{
				if (MyPirateAntennas.GetPiratesId() == IdentityId)
				{
					return MySession.Static.PirateBlockLimits;
				}
				switch (MySession.Static.BlockLimitsEnabled)
				{
				case MyBlockLimitsEnabledEnum.GLOBALLY:
					return MySession.Static.GlobalBlockLimits;
				case MyBlockLimitsEnabledEnum.PER_FACTION:
				{
					MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(IdentityId) as MyFaction;
					if (myFaction != null)
					{
						return myFaction.BlockLimits;
					}
					return MyBlockLimits.Empty;
				}
				default:
					return m_blockLimits;
				}
			}
		}

		public IReadOnlyList<BuildPlanItem> BuildPlanner => m_buildPlanner;

		long IMyIdentity.PlayerId => IdentityId;

		long IMyIdentity.IdentityId => IdentityId;

		string IMyIdentity.DisplayName => DisplayName;

		string IMyIdentity.Model => Model;

		Vector3? IMyIdentity.ColorMask => ColorMask;

		bool IMyIdentity.IsDead => IsDead;

		public event Action<MyCharacter, MyCharacter> CharacterChanged;

		public event Action<MyFaction, MyFaction> FactionChanged;

		event Action<IMyCharacter, IMyCharacter> IMyIdentity.CharacterChanged
		{
			add
			{
				CharacterChanged += GetDelegate(value);
			}
			remove
			{
				CharacterChanged -= GetDelegate(value);
			}
		}

		private MyIdentity(string name, MyEntityIdentifier.ID_OBJECT_TYPE identityType, string model = null, Vector3? colorMask = null)
		{
			IdentityId = MyEntityIdentifier.AllocateId(identityType, MyEntityIdentifier.ID_ALLOCATION_METHOD.SERIAL_START_WITH_1);
			Init(name, IdentityId, model, colorMask);
		}

		private MyIdentity(string name, long identityId, string model, Vector3? colorMask)
		{
			identityId = MyEntityIdentifier.FixObsoleteIdentityType(identityId);
			Init(name, identityId, model, colorMask);
			MyEntityIdentifier.MarkIdUsed(identityId);
		}

		private MyIdentity(MyObjectBuilder_Identity objectBuilder)
		{
			Init(objectBuilder.DisplayName, MyEntityIdentifier.FixObsoleteIdentityType(objectBuilder.IdentityId), objectBuilder.Model, objectBuilder.ColorMask, objectBuilder.BlockLimitModifier, objectBuilder.TransferedPCUDelta, objectBuilder.LastLoginTime, objectBuilder.LastLogoutTime);
			MyEntityIdentifier.MarkIdUsed(IdentityId);
			if (objectBuilder.ColorMask.HasValue)
			{
				ColorMask = objectBuilder.ColorMask;
			}
			IsDead = true;
			MyEntities.TryGetEntityById(objectBuilder.CharacterEntityId, out MyEntity entity);
			if (entity is MyCharacter)
			{
				Character = (entity as MyCharacter);
			}
			if (objectBuilder.SavedCharacters != null)
			{
				SavedCharacters = objectBuilder.SavedCharacters;
			}
			if (objectBuilder.RespawnShips != null)
			{
				RespawnShips = objectBuilder.RespawnShips;
			}
			if (objectBuilder.ActiveContracts != null)
			{
				ActiveContracts = objectBuilder.ActiveContracts;
			}
			LastDeathPosition = objectBuilder.LastDeathPosition;
		}

		public MyObjectBuilder_Identity GetObjectBuilder()
		{
			return new MyObjectBuilder_Identity
			{
				IdentityId = IdentityId,
				DisplayName = DisplayName,
				CharacterEntityId = ((Character == null) ? 0 : Character.EntityId),
				Model = Model,
				ColorMask = ColorMask,
				BlockLimitModifier = BlockLimits.BlockLimitModifier,
				TransferedPCUDelta = BlockLimits.TransferedDelta,
				LastLoginTime = LastLoginTime,
				LastLogoutTime = LastLogoutTime,
				SavedCharacters = SavedCharacters,
				RespawnShips = RespawnShips,
				LastDeathPosition = LastDeathPosition,
				ActiveContracts = ActiveContracts
			};
		}

		private void Init(string name, long identityId, string model, Vector3? colormask, int blockLimitModifier = 0, int transferedPCUDelta = 0, DateTime? loginTime = null, DateTime? logoutTime = null)
		{
			DisplayName = name;
			IdentityId = identityId;
			IsDead = true;
			Model = model;
			ColorMask = colormask;
			m_blockLimits = new MyBlockLimits(GetInitialPCU(), blockLimitModifier, transferedPCUDelta);
			if (MySession.Static.Players.IdentityIsNpc(identityId))
			{
				LastLoginTime = DateTime.Now;
			}
			else
			{
				LastLoginTime = (loginTime ?? DateTime.Now);
			}
			LastLogoutTime = (logoutTime ?? DateTime.Now);
			SavedCharacters = new HashSet<long>();
		}

		public int GetInitialPCU()
		{
			return MyBlockLimits.GetInitialPCU(IdentityId);
		}

		public int GetMaxPCU()
		{
			return MyBlockLimits.GetMaxPCU(this);
		}

		public void SetColorMask(Vector3 color)
		{
			ColorMask = color;
		}

		public void ChangeCharacter(MyCharacter character)
		{
			MyCharacter character2 = Character;
			if (character2 != null)
			{
				character2.OnClosing -= OnCharacterClosing;
				character2.CharacterDied -= OnCharacterDied;
			}
			Character = character;
			if (character != null)
			{
				character.OnClosing += OnCharacterClosing;
				character.CharacterDied += OnCharacterDied;
				SaveModelAndColorFromCharacter();
				IsDead = character.IsDead;
				if (!SavedCharacters.Contains(character.EntityId))
				{
					SavedCharacters.Add(character.EntityId);
					character.OnClosing += OnSavedCharacterClosing;
				}
			}
			this.CharacterChanged.InvokeIfNotNull(character2, Character);
		}

		private void OnCharacterDied(MyCharacter character)
		{
			LastDeathPosition = character.PositionComp.GetPosition();
		}

		private void SaveModelAndColorFromCharacter()
		{
			Model = Character.ModelName;
			ColorMask = Character.ColorMask;
		}

		public void SetDead(bool dead)
		{
			IsDead = dead;
		}

		public void PerformFirstSpawn()
		{
			FirstSpawnDone = true;
		}

		public void LogRespawnTime()
		{
			LastRespawnTime = MySession.Static.ElapsedGameTime;
		}

		public void SetDisplayName(string name)
		{
			DisplayName = name;
		}

		private void OnCharacterClosing(MyEntity character)
		{
			Character.OnClosing -= OnCharacterClosing;
			Character.CharacterDied -= OnCharacterDied;
			Character = null;
		}

		private void OnSavedCharacterClosing(MyEntity character)
		{
			character.OnClosing -= OnSavedCharacterClosing;
			SavedCharacters.Remove(character.EntityId);
		}

		public void RaiseFactionChanged(MyFaction oldFaction, MyFaction newFaction)
		{
			this.FactionChanged.InvokeIfNotNull(oldFaction, newFaction);
		}

		internal bool AddToBuildPlanner(MyCubeBlockDefinition block, int index = -1, List<BuildPlanItem.Component> components = null)
		{
			if (m_buildPlanner.Count >= 8 && (index == -1 || index > 7))
			{
				MyHud.Notifications.Add(MyNotificationSingletons.BuildPlannerCapacityReached);
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				return false;
			}
			BuildPlanItem buildPlanItem = new BuildPlanItem();
			buildPlanItem.BlockDefinition = block;
			buildPlanItem.IsInProgress = false;
			if (components == null)
			{
				buildPlanItem.Components = new List<BuildPlanItem.Component>();
				MyCubeBlockDefinition.Component[] components2 = block.Components;
				foreach (MyCubeBlockDefinition.Component component in components2)
				{
					BuildPlanItem.Component component2 = new BuildPlanItem.Component();
					component2.ComponentDefinition = component.Definition;
					component2.Count = component.Count;
					buildPlanItem.Components.Add(component2);
				}
			}
			else
			{
				buildPlanItem.Components = components;
				buildPlanItem.IsInProgress = true;
			}
			if (index == -1 || index >= m_buildPlanner.Count)
			{
				m_buildPlanner.Add(buildPlanItem);
			}
			else if (index >= 0 && index < m_buildPlanner.Count)
			{
				m_buildPlanner.RemoveAt(index);
				m_buildPlanner.Insert(index, buildPlanItem);
			}
			return true;
		}

		internal void RemoveAtBuildPlanner(int index)
		{
			if (index >= 0 && index < m_buildPlanner.Count)
			{
				m_buildPlanner.RemoveAt(index);
			}
		}

		internal void CleanFinishedBuildPlanner()
		{
			m_buildPlanner.RemoveAll((BuildPlanItem x) => x.Components.Count == 0);
		}

		internal void LoadBuildPlanner(MyObjectBuilder_Character.BuildPlanItem[] buildPlanner)
		{
			m_buildPlanner = new List<BuildPlanItem>();
			for (int i = 0; i < buildPlanner.Length; i++)
			{
				MyObjectBuilder_Character.BuildPlanItem buildPlanItem = buildPlanner[i];
				BuildPlanItem buildPlanItem2 = new BuildPlanItem();
				buildPlanItem2.BlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(buildPlanItem.BlockId);
				buildPlanItem2.IsInProgress = buildPlanItem.IsInProgress;
				buildPlanItem2.Components = new List<BuildPlanItem.Component>();
				foreach (MyObjectBuilder_Character.ComponentItem component2 in buildPlanItem.Components)
				{
					BuildPlanItem.Component component = new BuildPlanItem.Component();
					component.ComponentDefinition = MyDefinitionManager.Static.GetComponentDefinition(component2.ComponentId);
					component.Count = component2.Count;
					buildPlanItem2.Components.Add(component);
				}
				m_buildPlanner.Add(buildPlanItem2);
			}
		}

		private static MyBlueprintDefinitionBase MakeBlueprintFromBuildPlanItem(BuildPlanItem buildPlanItem)
		{
			MyObjectBuilder_CompositeBlueprintDefinition myObjectBuilder_CompositeBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompositeBlueprintDefinition>();
			myObjectBuilder_CompositeBlueprintDefinition.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), buildPlanItem.BlockDefinition.Id.ToString().Replace("MyObjectBuilder_", "BuildPlanItem_"));
			Dictionary<MyDefinitionId, MyFixedPoint> dictionary = new Dictionary<MyDefinitionId, MyFixedPoint>();
			foreach (BuildPlanItem.Component component in buildPlanItem.Components)
			{
				MyDefinitionId id = component.ComponentDefinition.Id;
				if (!dictionary.ContainsKey(id))
				{
					dictionary[id] = 0;
				}
				dictionary[id] += (MyFixedPoint)component.Count;
			}
			myObjectBuilder_CompositeBlueprintDefinition.Blueprints = new BlueprintItem[dictionary.Count];
			int num = 0;
			foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> item in dictionary)
			{
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = null;
				if ((myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(item.Key)) == null)
				{
					return null;
				}
				myObjectBuilder_CompositeBlueprintDefinition.Blueprints[num] = new BlueprintItem
				{
					Id = new SerializableDefinitionId(myBlueprintDefinitionBase.Id.TypeId, myBlueprintDefinitionBase.Id.SubtypeName),
					Amount = item.Value.ToString()
				};
				num++;
			}
			myObjectBuilder_CompositeBlueprintDefinition.Icons = buildPlanItem.BlockDefinition.Icons;
			myObjectBuilder_CompositeBlueprintDefinition.DisplayName = (buildPlanItem.BlockDefinition.DisplayNameEnum.HasValue ? buildPlanItem.BlockDefinition.DisplayNameEnum.Value.ToString() : buildPlanItem.BlockDefinition.DisplayNameText);
			myObjectBuilder_CompositeBlueprintDefinition.Public = buildPlanItem.BlockDefinition.Public;
			MyCompositeBlueprintDefinition myCompositeBlueprintDefinition = new MyCompositeBlueprintDefinition();
			myCompositeBlueprintDefinition.Init(myObjectBuilder_CompositeBlueprintDefinition, MyModContext.BaseGame);
			myCompositeBlueprintDefinition.Postprocess();
			return myCompositeBlueprintDefinition;
		}

		private MyObjectBuilder_Character.BuildPlanItem[] SaveBuildPlanner()
		{
			List<MyObjectBuilder_Character.BuildPlanItem> list = new List<MyObjectBuilder_Character.BuildPlanItem>();
			foreach (BuildPlanItem item3 in BuildPlanner)
			{
				MyObjectBuilder_Character.BuildPlanItem item = default(MyObjectBuilder_Character.BuildPlanItem);
				item.BlockId = item3.BlockDefinition.Id;
				item.IsInProgress = item3.IsInProgress;
				item.Components = new List<MyObjectBuilder_Character.ComponentItem>();
				foreach (BuildPlanItem.Component component in item3.Components)
				{
					MyObjectBuilder_Character.ComponentItem item2 = default(MyObjectBuilder_Character.ComponentItem);
					item2.ComponentId = component.ComponentDefinition.Id;
					item2.Count = component.Count;
					item.Components.Add(item2);
				}
				list.Add(item);
			}
			return list.ToArray();
		}

		private Action<MyCharacter, MyCharacter> GetDelegate(Action<IMyCharacter, IMyCharacter> value)
		{
			return (Action<MyCharacter, MyCharacter>)Delegate.CreateDelegate(typeof(Action<MyCharacter, MyCharacter>), value.Target, value.Method);
		}
	}
}
