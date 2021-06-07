using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World.Generator;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MyStation
	{
		public static readonly float SAFEZONE_SIZE = 150f;

		private MatrixD m_transformation;

		private MyStationTypeEnum m_type;

		private bool m_isDeepSpaceStation;

		public List<MyStoreItem> StoreItems
		{
			get;
			private set;
		}

		internal long SafeZoneEntityId
		{
			get;
			set;
		}

		public bool IsOnPlanetWithAtmosphere
		{
			get;
			set;
		}

		internal MyStationResourcesGenerator ResourcesGenerator
		{
			get;
			private set;
		}

		public long Id
		{
			get;
			private set;
		}

		public long StationEntityId
		{
			get;
			set;
		}

		public string PrefabName
		{
			get;
			private set;
		}

		public long FactionId
		{
			get;
			private set;
		}

		public MyStationTypeEnum Type => m_type;

		public Vector3D Position => m_transformation.Translation;

		public Vector3D Up => m_transformation.Up;

		public Vector3D Forward => m_transformation.Forward;

		public bool IsDeepSpaceStation => m_isDeepSpaceStation;

		public MyStation(long id, Vector3D position, MyStationTypeEnum type, MyFaction faction, string prefabName, MyDefinitionId? generatedItemsContainerTypeId, Vector3? up = null, Vector3? forward = null, bool isDeep = false)
		{
			Id = id;
			m_type = type;
			m_isDeepSpaceStation = isDeep;
			FactionId = faction.FactionId;
			PrefabName = prefabName;
			m_transformation = default(MatrixD);
			m_transformation.Translation = position;
			ResourcesGenerator = new MyStationResourcesGenerator(generatedItemsContainerTypeId);
			if (up.HasValue)
			{
				if (forward.HasValue)
				{
					m_transformation.Up = up.Value;
					m_transformation.Forward = forward.Value;
					m_transformation.Left = Vector3.Cross(m_transformation.Up, m_transformation.Forward);
				}
				else
				{
					m_transformation.Up = up.Value;
					m_transformation.Forward = Vector3.CalculatePerpendicularVector(m_transformation.Up);
					m_transformation.Left = Vector3.Cross(m_transformation.Up, m_transformation.Forward);
				}
			}
			else if (forward.HasValue)
			{
				m_transformation.Forward = forward.Value;
				m_transformation.Up = Vector3.CalculatePerpendicularVector(m_transformation.Forward);
				m_transformation.Left = Vector3.Cross(m_transformation.Up, m_transformation.Forward);
			}
			else
			{
				m_transformation.Up = Vector3.Normalize(MyUtils.GetRandomVector3());
				m_transformation.Forward = Vector3.CalculatePerpendicularVector(m_transformation.Up);
				m_transformation.Left = Vector3.Cross(m_transformation.Up, m_transformation.Forward);
			}
			StoreItems = new List<MyStoreItem>();
		}

		internal MySafeZone CreateSafeZone(IMyFaction faction)
		{
			MySafeZone mySafeZone = MySessionComponentSafeZones.CrateSafeZone(m_transformation, MySafeZoneShape.Sphere, MySafeZoneAccess.Whitelist, null, null, SAFEZONE_SIZE, enable: true, isVisible: false, new Vector3(0f, 0.09f, 0.196f), "SafeZone_Texture_Aura", 0L) as MySafeZone;
			string name = mySafeZone.DisplayName = string.Format(MyTexts.GetString(MySpaceTexts.SafeZone_Name_Station), faction.Tag, Id);
			mySafeZone.DisplayName = (mySafeZone.Name = name);
			SafeZoneEntityId = mySafeZone.EntityId;
			return mySafeZone;
		}

		public MyStation(MyObjectBuilder_Station obj)
		{
			m_transformation = MatrixD.CreateWorld(obj.Position, obj.Forward, obj.Up);
			Id = obj.Id;
			m_type = obj.StationType;
			m_isDeepSpaceStation = obj.IsDeepSpaceStation;
			StationEntityId = obj.StationEntityId;
			FactionId = obj.FactionId;
			PrefabName = obj.PrefabName;
			SafeZoneEntityId = obj.SafeZoneEntityId;
			IsOnPlanetWithAtmosphere = obj.IsOnPlanetWithAtmosphere;
			if (obj.StoreItems != null)
			{
				StoreItems = new List<MyStoreItem>(obj.StoreItems.Count);
				foreach (MyObjectBuilder_StoreItem storeItem in obj.StoreItems)
				{
					StoreItems.Add(new MyStoreItem(storeItem));
				}
			}
			else
			{
				StoreItems = new List<MyStoreItem>();
			}
			MyStationsListDefinition stationTypeDefinition = MyStationGenerator.GetStationTypeDefinition(Type);
			ResourcesGenerator = new MyStationResourcesGenerator(stationTypeDefinition.GeneratedItemsContainerType);
		}

		public MyObjectBuilder_Station GetObjectBuilder()
		{
			MyObjectBuilder_Station myObjectBuilder_Station = new MyObjectBuilder_Station();
			myObjectBuilder_Station.Position = m_transformation.Translation;
			myObjectBuilder_Station.Up = new SerializableVector3((float)m_transformation.Up.X, (float)m_transformation.Up.Y, (float)m_transformation.Up.Z);
			myObjectBuilder_Station.Forward = new SerializableVector3((float)m_transformation.Forward.X, (float)m_transformation.Forward.Y, (float)m_transformation.Forward.Z);
			myObjectBuilder_Station.Id = Id;
			myObjectBuilder_Station.StationType = m_type;
			myObjectBuilder_Station.IsDeepSpaceStation = m_isDeepSpaceStation;
			myObjectBuilder_Station.StationEntityId = StationEntityId;
			myObjectBuilder_Station.FactionId = FactionId;
			myObjectBuilder_Station.PrefabName = PrefabName;
			myObjectBuilder_Station.SafeZoneEntityId = SafeZoneEntityId;
			myObjectBuilder_Station.IsOnPlanetWithAtmosphere = IsOnPlanetWithAtmosphere;
			if (StoreItems != null)
			{
				myObjectBuilder_Station.StoreItems = new List<MyObjectBuilder_StoreItem>(StoreItems.Count);
				{
					foreach (MyStoreItem storeItem in StoreItems)
					{
						myObjectBuilder_Station.StoreItems.Add(storeItem.GetObjectBuilder());
					}
					return myObjectBuilder_Station;
				}
			}
			myObjectBuilder_Station.StoreItems = new List<MyObjectBuilder_StoreItem>();
			return myObjectBuilder_Station;
		}

		internal void StationGridSpawned()
		{
			MySession.Static.GetComponent<MySessionComponentContractSystem>().StationGridSpawned(this);
		}

		internal MyStoreItem GetStoreItemById(long id)
		{
			foreach (MyStoreItem storeItem in StoreItems)
			{
				if (storeItem.Id == id)
				{
					return storeItem;
				}
			}
			return null;
		}

		internal void RemoveStoreItem(MyStoreItem storeItem)
		{
			StoreItems.Remove(storeItem);
		}

		internal void Update(MyFaction faction)
		{
			ResourcesGenerator.UpdateStation(StationEntityId);
		}
	}
}
