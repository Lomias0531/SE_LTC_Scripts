using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public static class MyEntityList
	{
		[Serializable]
		public class MyEntityListInfoItem
		{
			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in string value)
				{
					owner.DisplayName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out string value)
				{
					value = owner.DisplayName;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EBlockCount_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in int value)
				{
					owner.BlockCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out int value)
				{
					value = owner.BlockCount;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EPCU_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, int?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in int? value)
				{
					owner.PCU = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out int? value)
				{
					value = owner.PCU;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EMass_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.Mass = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.Mass;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in Vector3D value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out Vector3D value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwnerName_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in string value)
				{
					owner.OwnerName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out string value)
				{
					value = owner.OwnerName;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in long value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out long value)
				{
					value = owner.Owner;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003ESpeed_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.Speed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.Speed;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EDistanceFromPlayers_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.DistanceFromPlayers = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.DistanceFromPlayers;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwnerLoginTime_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float value)
				{
					owner.OwnerLoginTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float value)
				{
					value = owner.OwnerLoginTime;
				}
			}

			protected class Sandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003C_003EOwnerLogoutTime_003C_003EAccessor : IMemberAccessor<MyEntityListInfoItem, float?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityListInfoItem owner, in float? value)
				{
					owner.OwnerLogoutTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityListInfoItem owner, out float? value)
				{
					value = owner.OwnerLogoutTime;
				}
			}

			public string DisplayName;

			public long EntityId;

			public int BlockCount;

			public int? PCU;

			public float Mass;

			public Vector3D Position;

			public string OwnerName;

			public long Owner;

			public float Speed;

			public float DistanceFromPlayers;

			public float OwnerLoginTime;

			public float? OwnerLogoutTime;

			public MyEntityListInfoItem()
			{
			}

			public MyEntityListInfoItem(string displayName, long entityId, int blockCount, int? pcu, float mass, Vector3D position, float speed, float distanceFromPlayers, string ownerName, long owner, float ownerLogin, float? ownerLogout)
			{
				if (string.IsNullOrEmpty(displayName))
				{
					DisplayName = "----";
				}
				else
				{
					DisplayName = ((displayName.Length < 50) ? displayName : displayName.Substring(0, 49));
				}
				EntityId = entityId;
				BlockCount = blockCount;
				PCU = pcu;
				Mass = mass;
				Position = position;
				OwnerName = ownerName;
				Owner = owner;
				Speed = speed;
				DistanceFromPlayers = distanceFromPlayers;
				OwnerLoginTime = ownerLogin;
				OwnerLogoutTime = ownerLogout;
			}

			public void Add(ref MyEntityListInfoItem item)
			{
				BlockCount += item.BlockCount;
				if (item.PCU.HasValue && item.PCU.HasValue)
				{
					PCU += item.PCU.Value;
				}
				Mass += item.Mass;
				OwnerLoginTime = Math.Min(item.OwnerLoginTime, OwnerLoginTime);
				if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue)
				{
					OwnerLogoutTime = Math.Min(item.OwnerLogoutTime.Value, OwnerLogoutTime.Value);
				}
			}
		}

		public enum MyEntityTypeEnum
		{
			Grids,
			SmallGrids,
			LargeGrids,
			Characters,
			FloatingObjects,
			Planets,
			Asteroids
		}

		public enum EntityListAction
		{
			Remove,
			Stop,
			Depower,
			Power
		}

		public enum MyEntitySortOrder
		{
			DisplayName,
			BlockCount,
			Mass,
			OwnerName,
			DistanceFromCenter,
			Speed,
			DistanceFromPlayers,
			OwnerLastLogout,
			PCU
		}

		[ThreadStatic]
		private static MyEntityListInfoItem m_gridItem;

		public static List<MyEntityListInfoItem> GetEntityList(MyEntityTypeEnum selectedType)
		{
			MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
			List<MyEntityListInfoItem> list = new List<MyEntityListInfoItem>(entities.Count);
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			switch (selectedType)
			{
			case MyEntityTypeEnum.Grids:
			case MyEntityTypeEnum.SmallGrids:
			case MyEntityTypeEnum.LargeGrids:
			{
				foreach (MyEntity item in entities)
				{
					MyCubeGrid myCubeGrid = item as MyCubeGrid;
					if (myCubeGrid != null && (selectedType != MyEntityTypeEnum.LargeGrids || myCubeGrid.GridSizeEnum != MyCubeSize.Small) && (selectedType != MyEntityTypeEnum.SmallGrids || myCubeGrid.GridSizeEnum != 0))
					{
						MyCubeGrid mechanicalRootGrid = GetMechanicalRootGrid(myCubeGrid);
						if (!myCubeGrid.Closed && myCubeGrid.Physics != null && mechanicalRootGrid == myCubeGrid)
						{
							CreateListInfoForGrid(myCubeGrid, out m_gridItem);
							AccountChildren(myCubeGrid);
							list.Add(m_gridItem);
						}
					}
				}
				return list;
			}
			case MyEntityTypeEnum.Characters:
			{
				foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
				{
					string text = allIdentity.DisplayName;
					if (Sync.Players.TryGetPlayerId(allIdentity.IdentityId, out MyPlayer.PlayerId result))
					{
						MyPlayer player = null;
						if (!Sync.Players.TryGetPlayerById(result, out player))
						{
							text = string.Concat(text, " (", MyTexts.Get(MyCommonTexts.OfflineStatus), ")");
						}
					}
					if (allIdentity.Character != null)
					{
						list.Add(new MyEntityListInfoItem(text, allIdentity.Character.EntityId, 0, allIdentity.BlockLimits.PCU, allIdentity.Character.CurrentMass, allIdentity.Character.PositionComp.GetPosition(), allIdentity.Character.Physics.LinearVelocity.Length(), 0f, allIdentity.DisplayName, allIdentity.IdentityId, (int)(DateTime.Now - allIdentity.LastLoginTime).TotalSeconds, (int)(DateTime.Now - allIdentity.LastLogoutTime).TotalSeconds));
					}
					else
					{
						foreach (long savedCharacter in allIdentity.SavedCharacters)
						{
							if (MyEntities.TryGetEntityById(savedCharacter, out MyCharacter entity))
							{
								list.Add(new MyEntityListInfoItem(text, savedCharacter, 0, null, entity.CurrentMass, entity.PositionComp.GetPosition(), entity.Physics.LinearVelocity.Length(), 0f, allIdentity.DisplayName, allIdentity.IdentityId, (int)(DateTime.Now - allIdentity.LastLoginTime).TotalSeconds, (int)(DateTime.Now - allIdentity.LastLogoutTime).TotalSeconds));
							}
						}
					}
				}
				return list;
			}
			case MyEntityTypeEnum.FloatingObjects:
			{
				foreach (MyEntity item2 in entities)
				{
					MyFloatingObject myFloatingObject = item2 as MyFloatingObject;
					if (myFloatingObject != null)
					{
						if (myFloatingObject.Closed || myFloatingObject.Physics == null)
						{
							continue;
						}
						list.Add(new MyEntityListInfoItem(myFloatingObject.DisplayName, myFloatingObject.EntityId, 0, null, myFloatingObject.Physics.Mass, myFloatingObject.PositionComp.GetPosition(), myFloatingObject.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance(myFloatingObject, onlinePlayers), "", 0L, 0f, null));
					}
					MyInventoryBagEntity myInventoryBagEntity = item2 as MyInventoryBagEntity;
					if (myInventoryBagEntity != null && !myInventoryBagEntity.Closed && myInventoryBagEntity.Physics != null)
					{
						MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(myInventoryBagEntity.OwnerIdentityId);
						string ownerName = "";
						float ownerLogin = 0f;
						float value = 0f;
						if (myIdentity != null)
						{
							ownerName = myIdentity.DisplayName;
							ownerLogin = (int)(DateTime.Now - myIdentity.LastLoginTime).TotalSeconds;
							value = (int)(DateTime.Now - myIdentity.LastLogoutTime).TotalSeconds;
						}
						list.Add(new MyEntityListInfoItem(myInventoryBagEntity.DisplayName, myInventoryBagEntity.EntityId, 0, null, myInventoryBagEntity.Physics.Mass, myInventoryBagEntity.PositionComp.GetPosition(), myInventoryBagEntity.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance(myInventoryBagEntity, onlinePlayers), ownerName, myInventoryBagEntity.OwnerIdentityId, ownerLogin, value));
					}
				}
				return list;
			}
			case MyEntityTypeEnum.Planets:
			{
				foreach (MyEntity item3 in entities)
				{
					MyPlanet myPlanet = item3 as MyPlanet;
					if (myPlanet != null && !myPlanet.Closed)
					{
						list.Add(new MyEntityListInfoItem(myPlanet.StorageName, myPlanet.EntityId, 0, null, 0f, myPlanet.PositionComp.GetPosition(), 0f, MySession.GetPlayerDistance(myPlanet, onlinePlayers), "", 0L, 0f, null));
					}
				}
				return list;
			}
			case MyEntityTypeEnum.Asteroids:
			{
				foreach (MyEntity item4 in entities)
				{
					MyVoxelBase myVoxelBase = item4 as MyVoxelBase;
					if (myVoxelBase != null && !(myVoxelBase is MyPlanet) && !myVoxelBase.Closed)
					{
						list.Add(new MyEntityListInfoItem(myVoxelBase.StorageName, myVoxelBase.EntityId, 0, null, 0f, myVoxelBase.PositionComp.GetPosition(), 0f, MySession.GetPlayerDistance(myVoxelBase, onlinePlayers), "", 0L, 0f, null));
					}
				}
				return list;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private static MyCubeGrid GetMechanicalRootGrid(MyCubeGrid grid)
		{
			MyCubeGrid myCubeGrid = null;
			foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.Mechanical.GetGroupNodes(grid))
			{
				if (myCubeGrid == null || groupNode.CubeBlocks.Count > myCubeGrid.CubeBlocks.Count)
				{
					myCubeGrid = groupNode;
				}
			}
			return myCubeGrid;
		}

		public static string GetDescriptionText(MyEntityListInfoItem item, bool isGrid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!isGrid)
			{
				stringBuilder.Append(string.Concat(MyEntitySortOrder.Mass, ": "));
				if (item.Mass > 0f)
				{
					MyValueFormatter.AppendWeightInBestUnit(item.Mass, stringBuilder);
				}
				else
				{
					stringBuilder.Append("-");
				}
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.DistanceFromCenter.ToString())), ": "));
				MyValueFormatter.AppendDistanceInBestUnit((float)item.Position.Length(), stringBuilder);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.Speed.ToString())), ": ", item.Speed, " m/s"));
			}
			else
			{
				stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.BlockCount.ToString())), ": ", item.BlockCount));
				if (item.PCU.HasValue && item.PCU.HasValue)
				{
					stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.PCU.ToString())), ": ", item.PCU.Value));
				}
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.Mass.ToString())), ": "));
				if (item.Mass > 0f)
				{
					MyValueFormatter.AppendWeightInBestUnit(item.Mass, stringBuilder);
				}
				else
				{
					stringBuilder.Append("-");
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.OwnerName.ToString())), ": ", item.OwnerName));
				stringBuilder.AppendLine(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.Speed.ToString())), ": ", item.Speed, " m/s"));
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.DistanceFromCenter.ToString())), ": "));
				MyValueFormatter.AppendDistanceInBestUnit((float)item.Position.Length(), stringBuilder);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.DistanceFromPlayers.ToString())), ": "));
				MyValueFormatter.AppendDistanceInBestUnit(item.DistanceFromPlayers, stringBuilder);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Concat(MyTexts.Get(MyStringId.GetOrCompute(MyEntitySortOrder.OwnerLastLogout.ToString())), ": "));
				if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue)
				{
					MyValueFormatter.AppendTimeInBestUnit(item.OwnerLogoutTime.Value, stringBuilder);
				}
			}
			return stringBuilder.ToString();
		}

		public static StringBuilder GetFormattedDisplayName(MyEntitySortOrder selectedOrder, MyEntityListInfoItem item, bool isGrid)
		{
			StringBuilder stringBuilder = new StringBuilder(item.DisplayName);
			switch (selectedOrder)
			{
			case MyEntitySortOrder.BlockCount:
				if (isGrid)
				{
					stringBuilder.Append(" | " + item.BlockCount);
				}
				break;
			case MyEntitySortOrder.PCU:
				if (item.PCU.HasValue && item.PCU.HasValue)
				{
					string str = (item.PCU.Value == int.MaxValue) ? "N/A" : item.PCU.Value.ToString();
					stringBuilder.Append(" | " + str);
				}
				break;
			case MyEntitySortOrder.Mass:
				stringBuilder.Append(" | ");
				if (item.Mass == 0f)
				{
					stringBuilder.Append("-");
				}
				else
				{
					MyValueFormatter.AppendWeightInBestUnit(item.Mass, stringBuilder);
				}
				break;
			case MyEntitySortOrder.OwnerName:
				if (isGrid)
				{
					stringBuilder.Append(" | " + (string.IsNullOrEmpty(item.OwnerName) ? MyTexts.GetString(MySpaceTexts.BlockOwner_Nobody) : item.OwnerName));
				}
				break;
			case MyEntitySortOrder.DistanceFromCenter:
				stringBuilder.Append(" | ");
				MyValueFormatter.AppendDistanceInBestUnit((float)item.Position.Length(), stringBuilder);
				break;
			case MyEntitySortOrder.Speed:
				stringBuilder.Append(" | " + item.Speed.ToString("0.### m/s"));
				break;
			case MyEntitySortOrder.DistanceFromPlayers:
				stringBuilder.Append(" | ");
				MyValueFormatter.AppendDistanceInBestUnit(item.DistanceFromPlayers, stringBuilder);
				break;
			case MyEntitySortOrder.OwnerLastLogout:
				if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.Value >= 0f)
				{
					if (item.OwnerName != item.DisplayName)
					{
						stringBuilder.Append(" | " + (string.IsNullOrEmpty(item.OwnerName) ? MyTexts.GetString(MySpaceTexts.BlockOwner_Nobody) : item.OwnerName));
						stringBuilder.Append(": ");
					}
					else
					{
						stringBuilder.Append(" | ");
					}
					MyValueFormatter.AppendTimeInBestUnit(item.OwnerLogoutTime.Value, stringBuilder);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case MyEntitySortOrder.DisplayName:
				break;
			}
			return stringBuilder;
		}

		public static void SortEntityList(MyEntitySortOrder selectedOrder, ref List<MyEntityListInfoItem> items, bool invertOrder)
		{
			switch (selectedOrder)
			{
			case MyEntitySortOrder.DisplayName:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num = string.Compare(a.DisplayName, b.DisplayName, StringComparison.CurrentCultureIgnoreCase);
					return invertOrder ? (-num) : num;
				});
				break;
			case MyEntitySortOrder.BlockCount:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num2 = b.BlockCount.CompareTo(a.BlockCount);
					return invertOrder ? (-num2) : num2;
				});
				break;
			case MyEntitySortOrder.PCU:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					if (b.PCU.HasValue && b.PCU.HasValue && a.PCU.HasValue && a.PCU.HasValue)
					{
						int num3 = b.PCU.Value.CompareTo(a.PCU.Value);
						if (invertOrder)
						{
							return -num3;
						}
						return num3;
					}
					return 1;
				});
				break;
			case MyEntitySortOrder.Mass:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					if (a.Mass == b.Mass)
					{
						return 0;
					}
					int num4 = (a.Mass == 0f) ? (-1) : ((b.Mass == 0f) ? 1 : b.Mass.CompareTo(a.Mass));
					return invertOrder ? (-num4) : num4;
				});
				break;
			case MyEntitySortOrder.OwnerName:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num5 = string.Compare(a.OwnerName, b.OwnerName, StringComparison.CurrentCultureIgnoreCase);
					return invertOrder ? (-num5) : num5;
				});
				break;
			case MyEntitySortOrder.DistanceFromCenter:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num6 = a.Position.LengthSquared().CompareTo(b.Position.LengthSquared());
					return invertOrder ? (-num6) : num6;
				});
				break;
			case MyEntitySortOrder.Speed:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num7 = b.Speed.CompareTo(a.Speed);
					return invertOrder ? (-num7) : num7;
				});
				break;
			case MyEntitySortOrder.DistanceFromPlayers:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					int num8 = b.DistanceFromPlayers.CompareTo(a.DistanceFromPlayers);
					return invertOrder ? (-num8) : num8;
				});
				break;
			case MyEntitySortOrder.OwnerLastLogout:
				items.Sort(delegate(MyEntityListInfoItem a, MyEntityListInfoItem b)
				{
					if (b.OwnerLogoutTime.HasValue && b.OwnerLogoutTime.HasValue && a.OwnerLogoutTime.HasValue && a.OwnerLogoutTime.HasValue)
					{
						int num9 = b.OwnerLogoutTime.Value.CompareTo(a.OwnerLogoutTime.Value);
						if (invertOrder)
						{
							return -num9;
						}
						return num9;
					}
					return 1;
				});
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static void ProceedEntityAction(MyEntity entity, EntityListAction action)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyCubeGrid myCubeGrid2 = myCubeGrid.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid2 == null)
				{
					myCubeGrid2 = myCubeGrid;
				}
				if (MySession.Static == null || MySession.Static.Factions.GetStationByGridId(myCubeGrid2.EntityId) == null)
				{
					if (action == EntityListAction.Remove)
					{
						myCubeGrid.DismountAllCockpits();
					}
					ProceedEntityActionHierarchy(MyGridPhysicalHierarchy.Static.GetRoot(myCubeGrid), action);
				}
			}
			else
			{
				ProceedEntityActionInternal(entity, action);
			}
		}

		private static void ProceedEntityActionHierarchy(MyCubeGrid grid, EntityListAction action)
		{
			MyGridPhysicalHierarchy.Static.ApplyOnChildren(grid, delegate(MyCubeGrid x)
			{
				ProceedEntityActionHierarchy(x, action);
			});
			ProceedEntityActionInternal(grid, action);
		}

		private static void ProceedEntityActionInternal(MyEntity entity, EntityListAction action)
		{
			switch (action)
			{
			case EntityListAction.Remove:
				entity.Close();
				break;
			case EntityListAction.Stop:
				Stop(entity);
				break;
			case EntityListAction.Depower:
				Depower(entity);
				break;
			case EntityListAction.Power:
				Power(entity);
				break;
			}
		}

		private static void Stop(MyEntity entity)
		{
			if (entity.Physics != null)
			{
				entity.Physics.LinearVelocity = Vector3.Zero;
				entity.Physics.AngularVelocity = Vector3.Zero;
			}
		}

		private static void Depower(MyEntity entity)
		{
			(entity as MyCubeGrid)?.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -1L);
		}

		private static void Power(MyEntity entity)
		{
			(entity as MyCubeGrid)?.ChangePowerProducerState(MyMultipleEnabledEnum.AllEnabled, -1L);
		}

		private static void AccountChildren(MyCubeGrid grid)
		{
			MyGridPhysicalHierarchy.Static.ApplyOnChildren(grid, delegate(MyCubeGrid childGrid)
			{
				CreateListInfoForGrid(childGrid, out MyEntityListInfoItem item);
				m_gridItem.Add(ref item);
				AccountChildren(childGrid);
			});
		}

		private static void CreateListInfoForGrid(MyCubeGrid grid, out MyEntityListInfoItem item)
		{
			long owner = 0L;
			string ownerName = string.Empty;
			if (grid.BigOwners.Count > 0)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(grid.BigOwners[0]);
				if (myIdentity != null)
				{
					ownerName = myIdentity.DisplayName;
					owner = grid.BigOwners[0];
				}
			}
			item = new MyEntityListInfoItem(grid.DisplayName, grid.EntityId, grid.BlocksCount, grid.BlocksPCU, grid.Physics.Mass, grid.PositionComp.GetPosition(), grid.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance(grid, MySession.Static.Players.GetOnlinePlayers()), ownerName, owner, MySession.GetOwnerLoginTimeSeconds(grid), MySession.GetOwnerLogoutTimeSeconds(grid));
		}
	}
}
