using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	public class MyGridTargeting : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyGridTargeting_003C_003EActor : IActivator, IActivator<MyGridTargeting>
		{
			private sealed override object CreateInstance()
			{
				return new MyGridTargeting();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGridTargeting CreateInstance()
			{
				return new MyGridTargeting();
			}

			MyGridTargeting IActivator<MyGridTargeting>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyCubeGrid m_grid;

		private BoundingSphere m_queryLocal = new BoundingSphere(Vector3.Zero, float.MinValue);

		private List<MyEntity> m_targetRoots = new List<MyEntity>();

		private MyListDictionary<MyCubeGrid, MyEntity> m_targetBlocks = new MyListDictionary<MyCubeGrid, MyEntity>();

		private List<long> m_ownersB = new List<long>();

		private List<long> m_ownersA = new List<long>();

		private FastResourceLock m_scanLock = new FastResourceLock();

		private int m_lastScan;

		public bool AllowScanning = true;

		public FastResourceLock ScanLock => m_scanLock;

		public List<MyEntity> TargetRoots
		{
			get
			{
				if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
				{
					Scan();
				}
				return m_targetRoots;
			}
		}

		public MyListDictionary<MyCubeGrid, MyEntity> TargetBlocks
		{
			get
			{
				if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
				{
					Scan();
				}
				return m_targetBlocks;
			}
		}

		public override string ComponentTypeDebugString => "MyGridTargeting";

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_grid = (base.Entity as MyCubeGrid);
			m_grid.OnBlockAdded += m_grid_OnBlockAdded;
		}

		private void m_grid_OnBlockAdded(MySlimBlock obj)
		{
			MyLargeTurretBase myLargeTurretBase = obj.FatBlock as MyLargeTurretBase;
			if (myLargeTurretBase != null)
			{
				m_queryLocal.Include(new BoundingSphere(obj.FatBlock.PositionComp.LocalMatrix.Translation, myLargeTurretBase.SearchingRange));
				myLargeTurretBase.PropertiesChanged += TurretOnPropertiesChanged;
			}
		}

		private void TurretOnPropertiesChanged(MyTerminalBlock obj)
		{
			MyLargeTurretBase myLargeTurretBase = obj as MyLargeTurretBase;
			if (myLargeTurretBase != null)
			{
				m_queryLocal.Include(new BoundingSphere(obj.PositionComp.LocalMatrix.Translation, myLargeTurretBase.SearchingRange));
			}
		}

		private void Scan()
		{
			using (m_scanLock.AcquireExclusiveUsing())
			{
				if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
				{
					BoundingSphereD sphere = new BoundingSphereD(Vector3D.Transform(m_queryLocal.Center, m_grid.WorldMatrix), m_queryLocal.Radius);
					m_targetRoots.Clear();
					m_targetBlocks.Clear();
					MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_targetRoots);
					MyMissiles.GetAllMissilesInSphere(ref sphere, m_targetRoots);
					int count = m_targetRoots.Count;
					m_ownersA.AddRange(m_grid.SmallOwners);
					m_ownersA.AddRange(m_grid.BigOwners);
					for (int i = 0; i < count; i++)
					{
						MyCubeGrid myCubeGrid = m_targetRoots[i] as MyCubeGrid;
						if (myCubeGrid != null && (myCubeGrid.Physics == null || myCubeGrid.Physics.Enabled))
						{
							bool flag = false;
							if (myCubeGrid.BigOwners.Count == 0 && myCubeGrid.SmallOwners.Count == 0)
							{
								foreach (long item in m_ownersA)
								{
									if (MyIDModule.GetRelationPlayerBlock(item, 0L) == MyRelationsBetweenPlayerAndBlock.NoOwnership)
									{
										flag = true;
										break;
									}
								}
							}
							else
							{
								m_ownersB.AddRange(myCubeGrid.BigOwners);
								m_ownersB.AddRange(myCubeGrid.SmallOwners);
								foreach (long item2 in m_ownersA)
								{
									foreach (long item3 in m_ownersB)
									{
										if (MyIDModule.GetRelationPlayerBlock(item2, item3) == MyRelationsBetweenPlayerAndBlock.Enemies)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										break;
									}
								}
								m_ownersB.Clear();
							}
							if (flag)
							{
								List<MyEntity> orAdd = m_targetBlocks.GetOrAdd(myCubeGrid);
								using (myCubeGrid.Pin())
								{
									if (!myCubeGrid.MarkedForClose)
									{
										myCubeGrid.Hierarchy.QuerySphere(ref sphere, orAdd);
									}
								}
							}
							else
							{
								foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
								{
									IMyComponentOwner<MyIDModule> myComponentOwner = fatBlock as IMyComponentOwner<MyIDModule>;
									if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule _))
									{
										long ownerId = fatBlock.OwnerId;
										foreach (long item4 in m_ownersA)
										{
											if (MyIDModule.GetRelationPlayerBlock(item4, ownerId) == MyRelationsBetweenPlayerAndBlock.Enemies)
											{
												flag = true;
												break;
											}
										}
										if (flag)
										{
											break;
										}
									}
								}
								if (flag)
								{
									List<MyEntity> orAdd2 = m_targetBlocks.GetOrAdd(myCubeGrid);
									if (!myCubeGrid.Closed)
									{
										myCubeGrid.Hierarchy.QuerySphere(ref sphere, orAdd2);
									}
								}
							}
						}
					}
					m_ownersA.Clear();
					for (int num = m_targetRoots.Count - 1; num >= 0; num--)
					{
						MyEntity myEntity = m_targetRoots[num];
						if (myEntity is MyDebrisBase || myEntity is MyFloatingObject || (myEntity.Physics != null && !myEntity.Physics.Enabled) || myEntity.GetTopMostParent().Physics == null || !myEntity.GetTopMostParent().Physics.Enabled)
						{
							m_targetRoots.RemoveAtFast(num);
						}
					}
					m_lastScan = MySession.Static.GameplayFrameCounter;
				}
			}
		}

		private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
		{
			if (!potentialDescendant.IsSubclassOf(potentialBase))
			{
				return potentialDescendant == potentialBase;
			}
			return true;
		}

		public void RescanIfNeeded()
		{
			if (AllowScanning && MySession.Static.GameplayFrameCounter - m_lastScan > 100)
			{
				Scan();
			}
		}
	}
}
