using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGps : IMyGps
	{
		internal static readonly int DROP_NONFINAL_AFTER_SEC = 180;

		private Vector3D m_coords;

		private IMyEntity m_entity;

		private bool m_unupdatedEntityLocation;

		private long m_entityId;

		private long m_contractId;

		private string m_displayName = string.Empty;

		public string Name
		{
			get;
			set;
		}

		public bool IsObjective
		{
			get;
			set;
		}

		public string DisplayName
		{
			get
			{
				if (m_unupdatedEntityLocation)
				{
					return m_displayName + " (last known location)";
				}
				return m_displayName;
			}
			set
			{
				m_displayName = value;
			}
		}

		public string Description
		{
			get;
			set;
		}

		public Vector3D Coords
		{
			get
			{
				if (CoordsFunc != null)
				{
					return CoordsFunc();
				}
				if (m_entityId != 0L && m_entity == null)
				{
					IMyEntity entityById = MyEntities.GetEntityById(m_entityId);
					if (entityById != null)
					{
						m_unupdatedEntityLocation = false;
						SetEntity(entityById);
					}
					else
					{
						m_unupdatedEntityLocation = true;
					}
				}
				return m_coords;
			}
			set
			{
				m_coords = value;
			}
		}

		public Color GPSColor
		{
			get;
			set;
		}

		public bool ShowOnHud
		{
			get;
			set;
		}

		public bool AlwaysVisible
		{
			get;
			set;
		}

		public TimeSpan? DiscardAt
		{
			get;
			set;
		}

		public bool IsLocal
		{
			get;
			set;
		}

		public Func<Vector3D> CoordsFunc
		{
			get;
			set;
		}

		public long EntityId => m_entityId;

		public long ContractId
		{
			get
			{
				return m_contractId;
			}
			set
			{
				m_contractId = value;
			}
		}

		public bool IsContainerGPS
		{
			get;
			set;
		}

		public string ContainerRemainingTime
		{
			get;
			set;
		}

		public int Hash
		{
			get;
			private set;
		}

		string IMyGps.Name
		{
			get
			{
				return Name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Value must not be null!");
				}
				Name = value;
			}
		}

		string IMyGps.Description
		{
			get
			{
				return Description;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Value must not be null!");
				}
				Description = value;
			}
		}

		Vector3D IMyGps.Coords
		{
			get
			{
				return Coords;
			}
			set
			{
				Coords = value;
			}
		}

		bool IMyGps.ShowOnHud
		{
			get
			{
				return ShowOnHud;
			}
			set
			{
				ShowOnHud = value;
			}
		}

		TimeSpan? IMyGps.DiscardAt
		{
			get
			{
				return DiscardAt;
			}
			set
			{
				DiscardAt = value;
			}
		}

		public MyGps(MyObjectBuilder_Gps.Entry builder)
		{
			Name = builder.name;
			DisplayName = builder.DisplayName;
			Description = builder.description;
			Coords = builder.coords;
			ShowOnHud = builder.showOnHud;
			AlwaysVisible = builder.alwaysVisible;
			IsObjective = builder.isObjective;
			ContractId = builder.contractId;
			if (builder.color != Color.Transparent && builder.color != Color.Black)
			{
				GPSColor = builder.color;
			}
			else
			{
				GPSColor = new Color(117, 201, 241);
			}
			if (!builder.isFinal)
			{
				SetDiscardAt();
			}
			SetEntityId(builder.entityId);
			UpdateHash();
		}

		public MyGps()
		{
			GPSColor = new Color(117, 201, 241);
			SetDiscardAt();
		}

		public void SetDiscardAt()
		{
			DiscardAt = TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double)DROP_NONFINAL_AFTER_SEC);
		}

		public void SetEntity(IMyEntity entity)
		{
			if (entity != null)
			{
				m_entity = entity;
				m_entityId = entity.EntityId;
				m_entity.PositionComp.OnPositionChanged += PositionComp_OnPositionChanged;
				m_entity.NeedsWorldMatrix = true;
				m_entity.OnClose += m_entity_OnClose;
				Coords = m_entity.PositionComp.GetPosition();
			}
		}

		public void SetEntityId(long entityId)
		{
			if (entityId != 0L)
			{
				m_entityId = entityId;
			}
		}

		private void m_entity_OnClose(IMyEntity obj)
		{
			if (m_entity != null)
			{
				m_entity.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
				m_entity.OnClose -= m_entity_OnClose;
				m_entity = null;
			}
		}

		private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
		{
			if (m_entity != null)
			{
				Coords = m_entity.PositionComp.GetPosition();
			}
		}

		public void Close()
		{
			if (m_entity != null)
			{
				m_entity.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
				m_entity.OnClose -= m_entity_OnClose;
			}
		}

		public void UpdateHash()
		{
			int hash = MyUtils.GetHash(Name);
			if (m_entityId == 0L)
			{
				hash = MyUtils.GetHash(Coords.X, hash);
				hash = MyUtils.GetHash(Coords.Y, hash);
				hash = MyUtils.GetHash(Coords.Z, hash);
			}
			else
			{
				hash *= m_entityId.GetHashCode();
			}
			Hash = hash;
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return ConvertToString(this);
		}

		internal static string ConvertToString(MyGps gps)
		{
			return ConvertToString(gps.Name, gps.Coords);
		}

		internal static string ConvertToString(string name, Vector3D coords)
		{
			StringBuilder stringBuilder = new StringBuilder("GPS:", 256);
			stringBuilder.Append(name);
			stringBuilder.Append(":");
			stringBuilder.Append(coords.X.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(":");
			stringBuilder.Append(coords.Y.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(":");
			stringBuilder.Append(coords.Z.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(":");
			return stringBuilder.ToString();
		}

		public void ToClipboard()
		{
			MyVRage.Platform.Clipboard = ToString();
		}
	}
}
