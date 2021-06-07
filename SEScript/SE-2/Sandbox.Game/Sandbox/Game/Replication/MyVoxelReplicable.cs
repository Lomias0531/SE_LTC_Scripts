using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Replication
{
	internal class MyVoxelReplicable : MyEntityReplicableBaseEvent<MyVoxelBase>, IMyStreamableReplicable
	{
		private List<MyEntity> m_entities;

		private Action<MyVoxelBase> m_loadingDoneHandler;

		private MyStreamingEntityStateGroup<MyVoxelReplicable> m_streamingGroup;

		private MyVoxelBase Voxel => base.Instance;

		public override bool IncludeInIslands => false;

		public bool NeedsToBeStreamed => true;

		protected override void OnHook()
		{
			base.OnHook();
			if (Sync.IsServer)
			{
				MyReplicationServer server = MyMultiplayer.GetReplicationServer();
				if (server != null)
				{
					Voxel.RangeChanged += delegate
					{
						server.InvalidateClientCache(this, Voxel.StorageName);
					};
				}
			}
		}

		public override bool ShouldReplicate(MyClientInfo client)
		{
			if (Voxel == null || Voxel.Storage == null || Voxel.Closed)
			{
				return false;
			}
			if (Voxel is MyPlanet)
			{
				return true;
			}
			if (!Voxel.Save && !Voxel.ContentChanged && !Voxel.BeforeContentChanged)
			{
				return false;
			}
			return true;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			return false;
		}

		protected override void OnLoad(BitStream stream, Action<MyVoxelBase> loadingDoneHandler)
		{
			bool flag = MySerializer.CreateAndRead<bool>(stream);
			bool flag2 = MySerializer.CreateAndRead<bool>(stream);
			bool num = MySerializer.CreateAndRead<bool>(stream);
			bool flag3 = MySerializer.CreateAndRead<bool>(stream);
			byte[] array = null;
			string asteroid = null;
			if (num)
			{
				array = MySerializer.CreateAndRead<byte[]>(stream);
			}
			else if (flag)
			{
				asteroid = MySerializer.CreateAndRead<string>(stream);
			}
			MyLog.Default.WriteLine("MyVoxelReplicable.OnLoad - isUserCreated:" + flag + " isFromPrefab:" + flag2 + " contentChanged:" + flag3 + " data?: " + (array != null));
			MyVoxelBase entity;
			if (flag2)
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MySerializer.CreateAndRead<MyObjectBuilder_EntityBase>(stream, MyObjectBuilderSerializer.Dynamic);
				if (array != null)
				{
					IMyStorage storage = MyStorageBase.Load(array);
					if (MyEntities.TryGetEntityById(myObjectBuilder_EntityBase.EntityId, out entity))
					{
						if (entity is MyVoxelMap)
						{
							((MyVoxelMap)entity).Storage = storage;
						}
						else if (entity is MyPlanet)
						{
							MyPlanet obj = entity as MyPlanet;
							obj.Storage = storage;
							obj.VoxelStorageUpdated();
						}
					}
					else
					{
						entity = (MyVoxelBase)MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_EntityBase);
						if (entity is MyVoxelMap)
						{
							((MyVoxelMap)entity).Init(myObjectBuilder_EntityBase, storage);
						}
						else if (entity is MyPlanet)
						{
							MyPlanet obj2 = entity as MyPlanet;
							obj2.Init(myObjectBuilder_EntityBase, storage);
							obj2.VoxelStorageUpdated();
						}
						if (entity != null)
						{
							MyEntities.Add(entity);
						}
					}
					entity.Save = true;
				}
				else if (!flag3)
				{
					if (myObjectBuilder_EntityBase is MyObjectBuilder_Planet)
					{
						if (MyEntities.TryGetEntityById(myObjectBuilder_EntityBase.EntityId, out entity))
						{
						}
					}
					else if (flag)
					{
						TryRemoveExistingEntity(myObjectBuilder_EntityBase.EntityId);
						IMyStorage storage2 = MyStorageBase.CreateAsteroidStorage(asteroid);
						entity = (MyVoxelBase)MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_EntityBase);
						MyVoxelMap myVoxelMap;
						if ((myVoxelMap = (entity as MyVoxelMap)) != null)
						{
							myVoxelMap.Init(myObjectBuilder_EntityBase, storage2);
						}
						if (entity != null)
						{
							MyEntities.Add(entity);
						}
					}
					else
					{
						TryRemoveExistingEntity(myObjectBuilder_EntityBase.EntityId);
						GenerateFromObjectBuilder(myObjectBuilder_EntityBase, out entity);
					}
				}
				else
				{
					GenerateFromObjectBuilder(myObjectBuilder_EntityBase, out entity);
				}
			}
			else
			{
				MyEntities.TryGetEntityById(MySerializer.CreateAndRead<long>(stream), out entity);
			}
			loadingDoneHandler(entity);
		}

		private void GenerateFromObjectBuilder(MyObjectBuilder_EntityBase builder, out MyVoxelBase voxelMap)
		{
			voxelMap = null;
			try
			{
				MyObjectBuilder_VoxelMap myObjectBuilder_VoxelMap = builder as MyObjectBuilder_VoxelMap;
				if (myObjectBuilder_VoxelMap != null)
				{
					if (MyEntities.TryGetEntityById(builder.EntityId, out voxelMap))
					{
						MyStorageBase storage = MyStorageBase.Load(myObjectBuilder_VoxelMap.StorageName);
						if (voxelMap is MyVoxelMap)
						{
							((MyVoxelMap)voxelMap).Storage = storage;
						}
						else if (voxelMap is MyPlanet)
						{
							((MyPlanet)voxelMap).Storage = storage;
						}
					}
					else
					{
						TryRemoveExistingEntity(builder.EntityId);
						voxelMap = (MyVoxelBase)MyEntities.CreateFromObjectBuilderNoinit(builder);
						if (voxelMap != null)
						{
							voxelMap.Init(builder);
							MyEntities.Add(voxelMap);
						}
					}
				}
			}
			catch
			{
				voxelMap = null;
				MyObjectBuilder_VoxelMap myObjectBuilder_VoxelMap2 = (MyObjectBuilder_VoxelMap)builder;
				if (myObjectBuilder_VoxelMap2 != null)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyMultiplayerBase.InvalidateVoxelCache, myObjectBuilder_VoxelMap2.StorageName);
				}
				MyLog.Default.WriteLine("Failed to load voxel from cache.");
			}
		}

		public override void OnDestroyClient()
		{
			if (Voxel != null)
			{
				if (Voxel.Storage != null)
				{
					Voxel.Storage.Save(out byte[] outCompressedData);
					MyMultiplayer.Static.VoxelMapData.Write(Voxel.StorageName, outCompressedData);
				}
				MyPlanet myPlanet = Voxel as MyPlanet;
				if (Voxel.Save && myPlanet == null)
				{
					Voxel.Close();
				}
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
			if (m_streamingGroup != null)
			{
				resultList.Add(m_streamingGroup);
			}
			base.GetStateGroups(resultList);
		}

		public void OnLoadBegin(Action<bool> loadingDoneHandler)
		{
			m_loadingDoneHandler = delegate(MyVoxelBase instance)
			{
				OnLoadDone(instance, loadingDoneHandler);
			};
		}

		public void CreateStreamingStateGroup()
		{
			m_streamingGroup = new MyStreamingEntityStateGroup<MyVoxelReplicable>(this, this);
		}

		public IMyStateGroup GetStreamingStateGroup()
		{
			return m_streamingGroup;
		}

		public void Serialize(BitStream stream, HashSet<string> cachedData, Endpoint forClient, Action writeData)
		{
			if (!Voxel.Closed)
			{
				bool isUserCreated = Voxel.CreatedByUser && Voxel.AsteroidName != null;
				bool isFromPrefab = Voxel.Save;
				bool contentChanged = Voxel.ContentChanged || Voxel.BeforeContentChanged;
				bool sendContent = (cachedData == null || !cachedData.Contains(Voxel.StorageName)) && (contentChanged || (isFromPrefab && !isUserCreated));
				sendContent |= (Voxel.AsteroidName == null);
				string asteroidName = Voxel.AsteroidName;
				long entityId = Voxel.EntityId;
				byte[] data = null;
				MyObjectBuilder_EntityBase builder = null;
				if (sendContent)
				{
					Voxel.Storage.Save(out data);
				}
				if (isFromPrefab)
				{
					builder = Voxel.GetObjectBuilder();
				}
				Parallel.Start(delegate
				{
					MySerializer.Write(stream, ref isUserCreated);
					MySerializer.Write(stream, ref isFromPrefab);
					MySerializer.Write(stream, ref sendContent);
					MySerializer.Write(stream, ref contentChanged);
					if (sendContent)
					{
						MySerializer.Write(stream, ref data);
					}
					else if (isUserCreated)
					{
						MySerializer.Write(stream, ref asteroidName);
					}
					if (isFromPrefab)
					{
						MySerializer.Write(stream, ref builder, MyObjectBuilderSerializer.Dynamic);
					}
					else
					{
						MySerializer.Write(stream, ref entityId);
					}
					writeData();
				});
				cachedData?.Add(Voxel.StorageName);
			}
		}

		public void LoadDone(BitStream stream)
		{
			OnLoad(stream, m_loadingDoneHandler);
		}

		public void LoadCancel()
		{
			m_loadingDoneHandler(null);
		}

		public override BoundingBoxD GetAABB()
		{
			BoundingBoxD worldAABB = base.Instance.PositionComp.WorldAABB;
			if (Voxel is MyPlanet)
			{
				return worldAABB;
			}
			worldAABB.Inflate(Voxel.SizeInMetres.Length() * 50f);
			return worldAABB;
		}

		protected override void RaiseDestroyed()
		{
			MyPlanet myPlanet = base.Instance as MyPlanet;
			base.RaiseDestroyed();
			if (Sync.IsServer && myPlanet != null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyEntities.ForceCloseEntityOnClients, myPlanet.EntityId);
			}
		}
	}
}
