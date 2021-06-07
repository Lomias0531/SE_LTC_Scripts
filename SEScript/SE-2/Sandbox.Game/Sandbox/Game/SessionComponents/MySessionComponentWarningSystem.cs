using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Network;
using VRage.Scripting;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 1000, typeof(MyObjectBuilder_SessionComponent), null)]
	public class MySessionComponentWarningSystem : MySessionComponentBase
	{
		public enum Category
		{
			Graphics,
			Blocks,
			Other,
			UnsafeGrids,
			BlockLimits,
			Server,
			Performance,
			General
		}

		[Serializable]
		public struct WarningData
		{
			protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003C_003ELastOccurence_003C_003EAccessor : IMemberAccessor<WarningData, DateTime?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WarningData owner, in DateTime? value)
				{
					owner.LastOccurence = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WarningData owner, out DateTime? value)
				{
					value = owner.LastOccurence;
				}
			}

			protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003C_003ECategory_003C_003EAccessor : IMemberAccessor<WarningData, Category>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WarningData owner, in Category value)
				{
					owner.Category = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WarningData owner, out Category value)
				{
					value = owner.Category;
				}
			}

			protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003C_003ETitleIdKey_003C_003EAccessor : IMemberAccessor<WarningData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WarningData owner, in string value)
				{
					owner.TitleIdKey = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WarningData owner, out string value)
				{
					value = owner.TitleIdKey;
				}
			}

			protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003C_003ETitleString_003C_003EAccessor : IMemberAccessor<WarningData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WarningData owner, in string value)
				{
					owner.TitleString = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WarningData owner, out string value)
				{
					value = owner.TitleString;
				}
			}

			protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003C_003EDescriptionIdKey_003C_003EAccessor : IMemberAccessor<WarningData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WarningData owner, in string value)
				{
					owner.DescriptionIdKey = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WarningData owner, out string value)
				{
					value = owner.DescriptionIdKey;
				}
			}

			protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003C_003EDescriptionString_003C_003EAccessor : IMemberAccessor<WarningData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref WarningData owner, in string value)
				{
					owner.DescriptionString = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref WarningData owner, out string value)
				{
					value = owner.DescriptionString;
				}
			}

			public DateTime? LastOccurence;

			public Category Category;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string TitleIdKey;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string TitleString;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string DescriptionIdKey;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string DescriptionString;

			public WarningData(MyStringId title, MyStringId description, Category category)
				: this(title, description, category, DateTime.Now)
			{
			}

			public WarningData(MyStringId title, MyStringId description, Category category, DateTime? time)
			{
				Category = category;
				LastOccurence = time;
				TitleIdKey = title.String;
				DescriptionIdKey = description.String;
				TitleString = null;
				DescriptionString = null;
			}

			public WarningData(string titleString, string descriptionString, Category category)
			{
				Category = category;
				TitleString = titleString;
				DescriptionString = descriptionString;
				TitleIdKey = null;
				LastOccurence = null;
				DescriptionIdKey = null;
			}

			public WarningData(Category category, MyStringId title, string titleString, MyStringId description, string descriptionString, DateTime? lastOccurence)
			{
				Category = category;
				TitleIdKey = title.String;
				TitleString = titleString;
				LastOccurence = lastOccurence;
				DescriptionIdKey = description.String;
				DescriptionString = descriptionString;
			}

			public Warning ConstructWarning()
			{
				return new Warning(ConstructLocalizedString(TitleIdKey, TitleString), ConstructLocalizedString(DescriptionIdKey, DescriptionString), Category, LastOccurence);
			}

			private static string ConstructLocalizedString(string formatKey, string strData)
			{
				if (string.IsNullOrEmpty(formatKey))
				{
					return strData;
				}
				string @string = MyTexts.GetString(formatKey);
				if (strData == null)
				{
					return @string;
				}
				return string.Format(@string, strData);
			}
		}

		public struct WarningKey
		{
			private sealed class EqualityComparer : IEqualityComparer<WarningKey>
			{
				public bool Equals(WarningKey x, WarningKey y)
				{
					if (string.Equals(x.Title, y.Title) && string.Equals(x.Description, y.Description))
					{
						return x.Category == y.Category;
					}
					return false;
				}

				public int GetHashCode(WarningKey obj)
				{
					return (((((obj.Title != null) ? obj.Title.GetHashCode() : 0) * 397) ^ ((obj.Description != null) ? obj.Description.GetHashCode() : 0)) * 397) ^ (int)obj.Category;
				}
			}

			public readonly string Title;

			public readonly string Description;

			public readonly Category Category;

			public static IEqualityComparer<WarningKey> Comparer
			{
				get;
			} = new EqualityComparer();


			public WarningKey(string title, string description, Category category)
			{
				Title = title;
				Description = description;
				Category = category;
			}

			public bool Equals(WarningKey other)
			{
				if (string.Equals(Title, other.Title) && string.Equals(Description, other.Description))
				{
					return Category == other.Category;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				object obj2;
				if ((obj2 = obj) is WarningKey)
				{
					WarningKey other = (WarningKey)obj2;
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((((Title != null) ? Title.GetHashCode() : 0) * 397) ^ ((Description != null) ? Description.GetHashCode() : 0)) * 397) ^ (int)Category;
			}

			public static bool operator ==(WarningKey left, WarningKey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(WarningKey left, WarningKey right)
			{
				return !left.Equals(right);
			}
		}

		public struct Warning
		{
			public DateTime? Time;

			public readonly string Title;

			public readonly string Description;

			public readonly Category Category;

			public Warning(string title, string description, Category category, DateTime? time)
			{
				Time = time;
				Title = title;
				Category = category;
				Description = description;
			}

			public WarningKey GetKey()
			{
				return new WarningKey(Title, Description, Category);
			}
		}

		protected sealed class OnUpdateWarnings_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_SessionComponents_MySessionComponentWarningSystem_003C_003EWarningData_003E : ICallSite<IMyEventOwner, List<WarningData>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<WarningData> warnings, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnUpdateWarnings(warnings);
			}
		}

		private static MySessionComponentWarningSystem m_static;

		private bool m_warningsDirty;

		private Dictionary<WarningKey, Warning> m_warnings;

		private bool m_updateRequested;

		private List<WarningData> m_serverWarnings;

		private Dictionary<long, WarningData> m_warningData;

		private int m_updateCounter;

		private List<WarningData> m_cachedUpdateList;

		public static MySessionComponentWarningSystem Static => m_static;

		public Dictionary<WarningKey, Warning>.ValueCollection CurrentWarnings
		{
			get
			{
				if (m_warningsDirty)
				{
					m_warnings.Clear();
					m_warningsDirty = false;
					using (List<WarningData>.Enumerator enumerator = m_serverWarnings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MergeWarning(warning: enumerator.Current.ConstructWarning(), warnings: m_warnings);
						}
					}
					using (Dictionary<long, WarningData>.ValueCollection.Enumerator enumerator2 = m_warningData.Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MergeWarning(warning: enumerator2.Current.ConstructWarning(), warnings: m_warnings);
						}
					}
					UpdateImmediateWarnings(delegate(WarningData x)
					{
						Warning value = x.ConstructWarning();
						WarningKey key = value.GetKey();
						if (!m_static.m_warnings.ContainsKey(key))
						{
							m_static.m_warnings.Add(key, value);
						}
					});
				}
				return m_warnings.Values;
			}
		}

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyUnsafeGridsSessionComponent)
		};

		public override bool IsRequiredByGame => true;

		public override void LoadData()
		{
			base.LoadData();
			m_static = this;
			m_warningData = new Dictionary<long, WarningData>();
			m_serverWarnings = new List<WarningData>();
			m_warnings = new Dictionary<WarningKey, Warning>();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_static = null;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			m_updateCounter++;
			bool isServer = Sync.IsServer;
			bool isDedicated = Sync.IsDedicated;
			int num = isDedicated ? 60 : 10;
			if ((float)m_updateCounter >= 60f * (float)num || ((float)m_updateCounter >= 60f && m_updateRequested))
			{
				m_updateCounter = 0;
				m_updateRequested = false;
				if (isServer)
				{
					UpdateServerWarnings();
				}
				if (!isDedicated)
				{
					UpdateClientWarnings();
				}
				m_warningsDirty = true;
			}
		}

		private void UpdateServerWarnings()
		{
			MyUtils.ClearCollectionToken<List<WarningData>, WarningData> clearCollectionToken = MyUtils.ReuseCollection(ref m_cachedUpdateList);
			try
			{
				List<WarningData> collection = clearCollectionToken.Collection;
				UpdateImmediateWarnings(collection.Add);
				collection.AddRange(m_warningData.Values);
				DateTime now = DateTime.Now;
				for (int i = 0; i < collection.Count; i++)
				{
					WarningData value = collection[i];
					if (value.LastOccurence.HasValue)
					{
						TimeSpan timeSpan = now - value.LastOccurence.Value;
						if (timeSpan < TimeSpan.Zero)
						{
							timeSpan = TimeSpan.Zero;
						}
						value.LastOccurence = DateTime.MinValue + timeSpan;
						collection[i] = value;
					}
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnUpdateWarnings, collection);
			}
			finally
			{
				((IDisposable)clearCollectionToken).Dispose();
			}
		}

		[Event(null, 172)]
		[Reliable]
		[Broadcast]
		private static void OnUpdateWarnings(List<WarningData> warnings)
		{
			DateTime now = DateTime.Now;
			for (int i = 0; i < warnings.Count; i++)
			{
				WarningData value = warnings[i];
				if (value.LastOccurence.HasValue)
				{
					value.LastOccurence = now + (value.LastOccurence.Value - DateTime.MinValue);
					warnings[i] = value;
				}
			}
			MySessionComponentWarningSystem @static = Static;
			@static.m_serverWarnings = warnings;
			@static.RequestUpdate();
		}

		private void UpdateImmediateWarnings(Action<WarningData> add)
		{
			foreach (MyCubeGrid value2 in MyUnsafeGridsSessionComponent.UnsafeGrids.Values)
			{
				string descriptionString = string.Join(", ", value2.UnsafeBlocks.Select((MyCubeBlock x) => x.DisplayNameText));
				add(new WarningData(value2.DisplayName, descriptionString, Category.UnsafeGrids));
			}
			foreach (KeyValuePair<long, MyTuple<string, MyStringId>> warning in MyModWatchdog.Warnings)
			{
				MyTuple<string, MyStringId> value = warning.Value;
				add(new WarningData(Category.Other, MyStringId.NullOrEmpty, value.Item1, value.Item2, value.Item1, null));
			}
		}

		private void UpdateClientWarnings()
		{
			bool dEBUG_DRAW_SERVER_WARNINGS = MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS;
			if ((!Sync.IsServer && Sync.ServerSimulationRatio < 0.8f) || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssuesServer_Simspeed.Id, new WarningData(MyCommonTexts.PerformanceWarningAreaPhysics, MyCommonTexts.PerformanceWarningIssuesServer_Simspeed, Category.Server));
			}
			if (MySession.Static.ServerSaving || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssuesServer_Saving.Id, new WarningData(MyCommonTexts.PerformanceWarningHeading_Saving, MyCommonTexts.PerformanceWarningIssuesServer_Saving, Category.Server));
			}
			if ((!MySession.Static.MultiplayerAlive && !MySession.Static.ServerSaving) || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssuesServer_NoConnection.Id, new WarningData(MyCommonTexts.PerformanceWarningIssuesServer_NoConnection, MyCommonTexts.Multiplayer_NoConnection, Category.Server));
			}
			if (!MySession.Static.MultiplayerDirect || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssuesServer_Direct.Id, new WarningData(MyCommonTexts.PerformanceWarningIssuesServer_Direct, MyCommonTexts.Multiplayer_IndirectConnection, Category.Server));
			}
			if ((!Sync.IsServer && MySession.Static.MultiplayerPing.Milliseconds > 250.0) || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssuesServer_Latency.Id, new WarningData(MyCommonTexts.PerformanceWarningIssuesServer_Latency, MyCommonTexts.Multiplayer_HighPing, Category.Server));
			}
			if (MyGeneralStats.Static.LowNetworkQuality || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssuesServer_PoorConnection.Id, new WarningData(MyCommonTexts.PerformanceWarningIssuesServer_PoorConnection, MyCommonTexts.Multiplayer_PacketLossDescription, Category.Server));
			}
			if (!MySession.Static.HighSimulationQualityNotification || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssues_LowSimulationQuality.Id, new WarningData(MyCommonTexts.PerformanceWarningIssues_LowSimulationQuality, MyCommonTexts.Performance_LowSimulationQuality, Category.Performance));
			}
			if (MySession.Static.LowMemoryState || dEBUG_DRAW_SERVER_WARNINGS)
			{
				AddWarning(MyCommonTexts.PerformanceWarningIssues_LowOnMemory.Id, new WarningData(MyCommonTexts.PerformanceWarningIssues_LowOnMemory, MyCommonTexts.Performance_LowOnMemory, Category.Performance));
			}
		}

		public void RequestUpdate()
		{
			m_warningsDirty = true;
			m_updateRequested = true;
		}

		public void AddWarning(long id, WarningData warning)
		{
			m_warningData[id] = warning;
			RequestUpdate();
		}

		private static void MergeWarning(Dictionary<WarningKey, Warning> warnings, Warning warning)
		{
			WarningKey key = warning.GetKey();
			if (warnings.TryGetValue(key, out Warning value))
			{
				if (value.Time.HasValue == warning.Time.HasValue && warning.Time.HasValue && warning.Time.Value > value.Time.Value)
				{
					warnings[key] = warning;
				}
			}
			else
			{
				warnings.Add(key, warning);
			}
		}
	}
}
