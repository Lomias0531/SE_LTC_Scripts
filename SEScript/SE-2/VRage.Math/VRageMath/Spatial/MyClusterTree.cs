using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;

namespace VRageMath.Spatial
{
	public class MyClusterTree
	{
		public interface IMyActivationHandler
		{
			/// <summary>
			/// If true, than this object is not calculated into cluster division algorithm. It is just added or removed if dynamic object is in range.
			/// </summary>
			bool IsStaticForCluster
			{
				get;
			}

			/// <summary>
			/// Called when standalone object is added to cluster
			/// </summary>
			/// <param name="userData"></param>
			/// <param name="clusterObjectID"></param>
			void Activate(object userData, ulong clusterObjectID);

			/// <summary>
			/// Called when standalone object is removed from cluster
			/// </summary>
			/// <param name="userData"></param>
			void Deactivate(object userData);

			/// <summary>
			/// Called when multiple objects are added to cluster.
			/// </summary>
			/// <param name="userData"></param>
			/// <param name="clusterObjectID"></param>
			void ActivateBatch(object userData, ulong clusterObjectID);

			/// <summary>
			/// Called when multiple objects are removed from cluster.
			/// </summary>
			/// <param name="userData"></param>
			void DeactivateBatch(object userData);

			/// <summary>
			/// Called when adding multiple objects is finished.
			/// </summary>
			void FinishAddBatch();

			/// <summary>
			/// Called when removing multiple objects is finished.
			/// </summary>
			void FinishRemoveBatch(object userData);
		}

		public class MyCluster
		{
			public int ClusterId;

			public BoundingBoxD AABB;

			public HashSet<ulong> Objects;

			public object UserData;

			public override string ToString()
			{
				return "MyCluster" + ClusterId + ": " + AABB.Center;
			}
		}

		private class MyObjectData
		{
			public ulong Id;

			public MyCluster Cluster;

			public IMyActivationHandler ActivationHandler;

			public BoundingBoxD AABB;

			public int StaticId;

			public string Tag;

			public long EntityId;
		}

		public struct MyClusterQueryResult
		{
			public BoundingBoxD AABB;

			public object UserData;
		}

		private class AABBComparerX : IComparer<MyObjectData>
		{
			public static AABBComparerX Static = new AABBComparerX();

			public int Compare(MyObjectData x, MyObjectData y)
			{
				return x.AABB.Min.X.CompareTo(y.AABB.Min.X);
			}
		}

		private class AABBComparerY : IComparer<MyObjectData>
		{
			public static AABBComparerY Static = new AABBComparerY();

			public int Compare(MyObjectData x, MyObjectData y)
			{
				return x.AABB.Min.Y.CompareTo(y.AABB.Min.Y);
			}
		}

		private class AABBComparerZ : IComparer<MyObjectData>
		{
			public static AABBComparerZ Static = new AABBComparerZ();

			public int Compare(MyObjectData x, MyObjectData y)
			{
				return x.AABB.Min.Z.CompareTo(y.AABB.Min.Z);
			}
		}

		private struct MyClusterDescription
		{
			public BoundingBoxD AABB;

			public List<MyObjectData> DynamicObjects;

			public List<MyObjectData> StaticObjects;
		}

		public Func<int, BoundingBoxD, object> OnClusterCreated;

		public Action<object> OnClusterRemoved;

		public Action<object> OnFinishBatch;

		public Action OnClustersReordered;

		public Func<long, bool> GetEntityReplicableExistsById;

		public const ulong CLUSTERED_OBJECT_ID_UNITIALIZED = ulong.MaxValue;

		public static Vector3 IdealClusterSize = new Vector3(20000f);

		public static Vector3 IdealClusterSizeHalfSqr = IdealClusterSize * IdealClusterSize / 4f;

		public static Vector3 MinimumDistanceFromBorder = IdealClusterSize / 100f;

		public static Vector3 MaximumForSplit = IdealClusterSize * 2f;

		public static float MaximumClusterSize = 100000f;

		public readonly BoundingBoxD? SingleCluster;

		public readonly bool ForcedClusters;

		private bool m_suppressClusterReorder;

		private FastResourceLock m_clustersLock = new FastResourceLock();

		private FastResourceLock m_clustersReorderLock = new FastResourceLock();

		private MyDynamicAABBTreeD m_clusterTree = new MyDynamicAABBTreeD(Vector3D.Zero);

		private MyDynamicAABBTreeD m_staticTree = new MyDynamicAABBTreeD(Vector3D.Zero);

		private Dictionary<ulong, MyObjectData> m_objectsData = new Dictionary<ulong, MyObjectData>();

		private List<MyCluster> m_clusters = new List<MyCluster>();

		private ulong m_clusterObjectCounter;

		private List<MyCluster> m_returnedClusters = new List<MyCluster>(1);

		private List<object> m_userObjects = new List<object>();

		[ThreadStatic]
		private static List<MyLineSegmentOverlapResult<MyCluster>> m_lineResultListPerThread;

		[ThreadStatic]
		private static List<MyCluster> m_resultListPerThread;

		[ThreadStatic]
		private static List<ulong> m_objectDataResultListPerThread;

		public bool SuppressClusterReorder
		{
			get
			{
				return m_suppressClusterReorder;
			}
			set
			{
				m_suppressClusterReorder = value;
			}
		}

		private static List<MyLineSegmentOverlapResult<MyCluster>> m_lineResultList
		{
			get
			{
				if (m_lineResultListPerThread == null)
				{
					m_lineResultListPerThread = new List<MyLineSegmentOverlapResult<MyCluster>>();
				}
				return m_lineResultListPerThread;
			}
		}

		private static List<MyCluster> m_resultList
		{
			get
			{
				if (m_resultListPerThread == null)
				{
					m_resultListPerThread = new List<MyCluster>();
				}
				return m_resultListPerThread;
			}
		}

		private static List<ulong> m_objectDataResultList
		{
			get
			{
				if (m_objectDataResultListPerThread == null)
				{
					m_objectDataResultListPerThread = new List<ulong>();
				}
				return m_objectDataResultListPerThread;
			}
		}

		public MyClusterTree(BoundingBoxD? singleCluster, bool forcedClusters)
		{
			SingleCluster = singleCluster;
			ForcedClusters = forcedClusters;
		}

		public ulong AddObject(BoundingBoxD bbox, IMyActivationHandler activationHandler, ulong? customId, string tag, long entityId, bool batch)
		{
			using (m_clustersLock.AcquireExclusiveUsing())
			{
				if (SingleCluster.HasValue && m_clusters.Count == 0)
				{
					BoundingBoxD clusterBB = SingleCluster.Value;
					clusterBB.Inflate(200.0);
					CreateCluster(ref clusterBB);
				}
				BoundingBoxD bbox2 = (!SingleCluster.HasValue && !ForcedClusters) ? bbox.GetInflated(IdealClusterSize / 100f) : bbox;
				m_clusterTree.OverlapAllBoundingBox(ref bbox2, m_returnedClusters);
				MyCluster myCluster = null;
				bool flag = false;
				if (m_returnedClusters.Count == 1)
				{
					if (m_returnedClusters[0].AABB.Contains(bbox2) == ContainmentType.Contains)
					{
						myCluster = m_returnedClusters[0];
					}
					else if (m_returnedClusters[0].AABB.Contains(bbox2) == ContainmentType.Intersects && activationHandler.IsStaticForCluster)
					{
						if (m_returnedClusters[0].AABB.Contains(bbox) != 0)
						{
							myCluster = m_returnedClusters[0];
						}
					}
					else
					{
						flag = true;
					}
				}
				else if (m_returnedClusters.Count > 1)
				{
					if (!activationHandler.IsStaticForCluster)
					{
						flag = true;
					}
				}
				else if (m_returnedClusters.Count == 0)
				{
					if (SingleCluster.HasValue)
					{
						return ulong.MaxValue;
					}
					if (!activationHandler.IsStaticForCluster)
					{
						BoundingBoxD bbox3 = new BoundingBoxD(bbox.Center - IdealClusterSize / 2f, bbox.Center + IdealClusterSize / 2f);
						m_clusterTree.OverlapAllBoundingBox(ref bbox3, m_returnedClusters);
						if (m_returnedClusters.Count == 0)
						{
							m_staticTree.OverlapAllBoundingBox(ref bbox3, m_objectDataResultList);
							myCluster = CreateCluster(ref bbox3);
							foreach (ulong objectDataResult in m_objectDataResultList)
							{
								if (m_objectsData[objectDataResult].Cluster == null)
								{
									AddObjectToCluster(myCluster, objectDataResult, batch: false);
								}
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				ulong num = customId.HasValue ? customId.Value : m_clusterObjectCounter++;
				int staticId = -1;
				m_objectsData[num] = new MyObjectData
				{
					Id = num,
					ActivationHandler = activationHandler,
					AABB = bbox,
					StaticId = staticId,
					Tag = tag,
					EntityId = entityId
				};
				if (flag && !SingleCluster.HasValue && !ForcedClusters)
				{
					ReorderClusters(bbox, num);
					_ = m_objectsData[num].ActivationHandler.IsStaticForCluster;
				}
				if (activationHandler.IsStaticForCluster)
				{
					staticId = m_staticTree.AddProxy(ref bbox, num, 0u);
					m_objectsData[num].StaticId = staticId;
				}
				if (myCluster != null)
				{
					return AddObjectToCluster(myCluster, num, batch);
				}
				return num;
			}
		}

		private ulong AddObjectToCluster(MyCluster cluster, ulong objectId, bool batch)
		{
			cluster.Objects.Add(objectId);
			MyObjectData myObjectData = m_objectsData[objectId];
			m_objectsData[objectId].Id = objectId;
			m_objectsData[objectId].Cluster = cluster;
			if (batch)
			{
				if (myObjectData.ActivationHandler != null)
				{
					myObjectData.ActivationHandler.ActivateBatch(cluster.UserData, objectId);
				}
			}
			else if (myObjectData.ActivationHandler != null)
			{
				myObjectData.ActivationHandler.Activate(cluster.UserData, objectId);
			}
			return objectId;
		}

		private MyCluster CreateCluster(ref BoundingBoxD clusterBB)
		{
			MyCluster myCluster = new MyCluster
			{
				AABB = clusterBB,
				Objects = new HashSet<ulong>()
			};
			myCluster.ClusterId = m_clusterTree.AddProxy(ref myCluster.AABB, myCluster, 0u);
			if (OnClusterCreated != null)
			{
				myCluster.UserData = OnClusterCreated(myCluster.ClusterId, myCluster.AABB);
			}
			m_clusters.Add(myCluster);
			m_userObjects.Add(myCluster.UserData);
			return myCluster;
		}

		public static BoundingBoxD AdjustAABBByVelocity(BoundingBoxD aabb, Vector3 velocity, float inflate = 1.1f)
		{
			if (velocity.LengthSquared() > 0.001f)
			{
				velocity.Normalize();
			}
			aabb.Inflate(inflate);
			BoundingBoxD box = aabb;
			box += (Vector3D)(velocity * 10f * inflate);
			aabb.Include(box);
			return aabb;
		}

		public void MoveObject(ulong id, BoundingBoxD aabb, Vector3 velocity)
		{
			using (m_clustersLock.AcquireExclusiveUsing())
			{
				if (m_objectsData.TryGetValue(id, out MyObjectData value))
				{
					BoundingBoxD aABB = value.AABB;
					value.AABB = aabb;
					if (!m_suppressClusterReorder)
					{
						aabb = AdjustAABBByVelocity(aabb, velocity, 0f);
						ContainmentType containmentType = value.Cluster.AABB.Contains(aabb);
						if (containmentType != ContainmentType.Contains && !SingleCluster.HasValue && !ForcedClusters)
						{
							if (containmentType == ContainmentType.Disjoint)
							{
								m_clusterTree.OverlapAllBoundingBox(ref aabb, m_returnedClusters);
								if (m_returnedClusters.Count == 1 && m_returnedClusters[0].AABB.Contains(aabb) == ContainmentType.Contains)
								{
									MyCluster cluster = value.Cluster;
									RemoveObjectFromCluster(value, batch: false);
									if (cluster.Objects.Count == 0)
									{
										RemoveCluster(cluster);
									}
									AddObjectToCluster(m_returnedClusters[0], value.Id, batch: false);
								}
								else
								{
									aabb.InflateToMinimum(IdealClusterSize);
									ReorderClusters(aabb.Include(aABB), id);
								}
							}
							else
							{
								aabb.InflateToMinimum(IdealClusterSize);
								ReorderClusters(aabb.Include(aABB), id);
							}
						}
					}
				}
			}
		}

		public void EnsureClusterSpace(BoundingBoxD aabb)
		{
			if (!SingleCluster.HasValue && !ForcedClusters)
			{
				using (m_clustersLock.AcquireExclusiveUsing())
				{
					m_clusterTree.OverlapAllBoundingBox(ref aabb, m_returnedClusters);
					bool flag = true;
					if (m_returnedClusters.Count == 1 && m_returnedClusters[0].AABB.Contains(aabb) == ContainmentType.Contains)
					{
						flag = false;
					}
					if (flag)
					{
						ulong num = m_clusterObjectCounter++;
						int staticId = -1;
						m_objectsData[num] = new MyObjectData
						{
							Id = num,
							Cluster = null,
							ActivationHandler = null,
							AABB = aabb,
							StaticId = staticId
						};
						ReorderClusters(aabb, num);
						RemoveObjectFromCluster(m_objectsData[num], batch: false);
						m_objectsData.Remove(num);
					}
				}
			}
		}

		public void RemoveObject(ulong id)
		{
			if (!m_objectsData.TryGetValue(id, out MyObjectData value))
			{
				return;
			}
			MyCluster cluster = value.Cluster;
			if (cluster != null)
			{
				RemoveObjectFromCluster(value, batch: false);
				if (cluster.Objects.Count == 0)
				{
					RemoveCluster(cluster);
				}
			}
			if (value.StaticId != -1)
			{
				m_staticTree.RemoveProxy(value.StaticId);
				value.StaticId = -1;
			}
			m_objectsData.Remove(id);
		}

		private void RemoveObjectFromCluster(MyObjectData objectData, bool batch)
		{
			objectData.Cluster.Objects.Remove(objectData.Id);
			if (batch)
			{
				if (objectData.ActivationHandler != null)
				{
					objectData.ActivationHandler.DeactivateBatch(objectData.Cluster.UserData);
				}
				return;
			}
			if (objectData.ActivationHandler != null)
			{
				objectData.ActivationHandler.Deactivate(objectData.Cluster.UserData);
			}
			m_objectsData[objectData.Id].Cluster = null;
		}

		private void RemoveCluster(MyCluster cluster)
		{
			m_clusterTree.RemoveProxy(cluster.ClusterId);
			m_clusters.Remove(cluster);
			m_userObjects.Remove(cluster.UserData);
			if (OnClusterRemoved != null)
			{
				OnClusterRemoved(cluster.UserData);
			}
		}

		public Vector3D GetObjectOffset(ulong id)
		{
			if (m_objectsData.TryGetValue(id, out MyObjectData value))
			{
				if (value.Cluster == null)
				{
					return Vector3D.Zero;
				}
				return value.Cluster.AABB.Center;
			}
			return Vector3D.Zero;
		}

		public MyCluster GetClusterForPosition(Vector3D pos)
		{
			BoundingSphereD sphere = new BoundingSphereD(pos, 1.0);
			m_clusterTree.OverlapAllBoundingSphere(ref sphere, m_returnedClusters);
			if (m_returnedClusters.Count <= 0)
			{
				return null;
			}
			return m_returnedClusters.Single();
		}

		public void Dispose()
		{
			foreach (MyCluster cluster in m_clusters)
			{
				if (OnClusterRemoved != null)
				{
					OnClusterRemoved(cluster.UserData);
				}
			}
			m_clusters.Clear();
			m_userObjects.Clear();
			m_clusterTree.Clear();
			m_objectsData.Clear();
			m_staticTree.Clear();
			m_clusterObjectCounter = 0uL;
		}

		public ListReader<object> GetList()
		{
			return new ListReader<object>(m_userObjects);
		}

		public ListReader<object> GetListCopy()
		{
			return new ListReader<object>(new List<object>(m_userObjects));
		}

		public ListReader<MyCluster> GetClusters()
		{
			return m_clusters;
		}

		public void CastRay(Vector3D from, Vector3D to, List<MyClusterQueryResult> results)
		{
			if (m_clusterTree != null && results != null)
			{
				LineD line = new LineD(from, to);
				m_clusterTree.OverlapAllLineSegment(ref line, m_lineResultList);
				foreach (MyLineSegmentOverlapResult<MyCluster> lineResult in m_lineResultList)
				{
					if (lineResult.Element != null)
					{
						results.Add(new MyClusterQueryResult
						{
							AABB = lineResult.Element.AABB,
							UserData = lineResult.Element.UserData
						});
					}
				}
			}
		}

		public void Intersects(Vector3D translation, List<MyClusterQueryResult> results)
		{
			BoundingBoxD bbox = new BoundingBoxD(translation - new Vector3D(1.0), translation + new Vector3D(1.0));
			m_clusterTree.OverlapAllBoundingBox(ref bbox, m_resultList);
			foreach (MyCluster result in m_resultList)
			{
				results.Add(new MyClusterQueryResult
				{
					AABB = result.AABB,
					UserData = result.UserData
				});
			}
		}

		public void GetAll(List<MyClusterQueryResult> results)
		{
			m_clusterTree.GetAll(m_resultList, clear: true);
			foreach (MyCluster result in m_resultList)
			{
				results.Add(new MyClusterQueryResult
				{
					AABB = result.AABB,
					UserData = result.UserData
				});
			}
		}

		public void ReorderClusters(BoundingBoxD aabb, ulong objectId = ulong.MaxValue)
		{
			using (m_clustersReorderLock.AcquireExclusiveUsing())
			{
				BoundingBoxD bbox = aabb.GetInflated(IdealClusterSize / 2f);
				bbox.InflateToMinimum(IdealClusterSize);
				m_clusterTree.OverlapAllBoundingBox(ref bbox, m_resultList);
				HashSet<MyObjectData> hashSet = new HashSet<MyObjectData>();
				bool flag = false;
				while (!flag)
				{
					hashSet.Clear();
					if (objectId != ulong.MaxValue)
					{
						hashSet.Add(m_objectsData[objectId]);
					}
					foreach (MyCluster collidedCluster in m_resultList)
					{
						foreach (MyObjectData item4 in from x in m_objectsData
							where collidedCluster.Objects.Contains(x.Key)
							select x.Value)
						{
							hashSet.Add(item4);
							bbox.Include(item4.AABB.GetInflated(IdealClusterSize / 2f));
						}
					}
					int count = m_resultList.Count;
					m_clusterTree.OverlapAllBoundingBox(ref bbox, m_resultList);
					flag = (count == m_resultList.Count);
					m_staticTree.OverlapAllBoundingBox(ref bbox, m_objectDataResultList);
					foreach (ulong objectDataResult in m_objectDataResultList)
					{
						if (m_objectsData[objectDataResult].Cluster != null && !m_resultList.Contains(m_objectsData[objectDataResult].Cluster))
						{
							bbox.Include(m_objectsData[objectDataResult].AABB.GetInflated(IdealClusterSize / 2f));
							flag = false;
						}
					}
				}
				m_staticTree.OverlapAllBoundingBox(ref bbox, m_objectDataResultList);
				foreach (ulong objectDataResult2 in m_objectDataResultList)
				{
					hashSet.Add(m_objectsData[objectDataResult2]);
				}
				int num = 8;
				bool flag2 = true;
				Stack<MyClusterDescription> stack = new Stack<MyClusterDescription>();
				List<MyClusterDescription> list = new List<MyClusterDescription>();
				List<MyObjectData> list2 = null;
				Vector3 idealClusterSize = IdealClusterSize;
				while (num > 0 && flag2)
				{
					stack.Clear();
					list.Clear();
					MyClusterDescription myClusterDescription = default(MyClusterDescription);
					myClusterDescription.AABB = bbox;
					myClusterDescription.DynamicObjects = hashSet.Where((MyObjectData x) => x.ActivationHandler == null || !x.ActivationHandler.IsStaticForCluster).ToList();
					myClusterDescription.StaticObjects = hashSet.Where((MyObjectData x) => x.ActivationHandler != null && x.ActivationHandler.IsStaticForCluster).ToList();
					MyClusterDescription item = myClusterDescription;
					stack.Push(item);
					list2 = item.StaticObjects.Where((MyObjectData x) => x.Cluster != null).ToList();
					_ = item.StaticObjects.Count;
					while (stack.Count > 0)
					{
						MyClusterDescription myClusterDescription2 = stack.Pop();
						if (myClusterDescription2.DynamicObjects.Count != 0)
						{
							BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
							for (int i = 0; i < myClusterDescription2.DynamicObjects.Count; i++)
							{
								BoundingBoxD inflated = myClusterDescription2.DynamicObjects[i].AABB.GetInflated(idealClusterSize / 2f);
								boundingBoxD.Include(inflated);
							}
							BoundingBoxD aABB = boundingBoxD;
							int num2 = boundingBoxD.Size.AbsMaxComponent();
							switch (num2)
							{
							case 0:
								myClusterDescription2.DynamicObjects.Sort(AABBComparerX.Static);
								break;
							case 1:
								myClusterDescription2.DynamicObjects.Sort(AABBComparerY.Static);
								break;
							case 2:
								myClusterDescription2.DynamicObjects.Sort(AABBComparerZ.Static);
								break;
							}
							bool flag3 = false;
							if (boundingBoxD.Size.AbsMax() >= (double)MaximumForSplit.AbsMax())
							{
								BoundingBoxD boundingBoxD2 = BoundingBoxD.CreateInvalid();
								double num3 = double.MinValue;
								for (int j = 1; j < myClusterDescription2.DynamicObjects.Count; j++)
								{
									MyObjectData myObjectData = myClusterDescription2.DynamicObjects[j - 1];
									MyObjectData myObjectData2 = myClusterDescription2.DynamicObjects[j];
									BoundingBoxD inflated2 = myObjectData.AABB.GetInflated(idealClusterSize / 2f);
									BoundingBoxD inflated3 = myObjectData2.AABB.GetInflated(idealClusterSize / 2f);
									double dim = inflated2.Max.GetDim(num2);
									if (dim > num3)
									{
										num3 = dim;
									}
									boundingBoxD2.Include(inflated2);
									double dim2 = inflated3.Min.GetDim(num2);
									double dim3 = inflated2.Max.GetDim(num2);
									if (dim2 - dim3 > 0.0 && num3 <= dim2)
									{
										flag3 = true;
										aABB = boundingBoxD2;
										break;
									}
								}
							}
							aABB.InflateToMinimum(idealClusterSize);
							myClusterDescription = default(MyClusterDescription);
							myClusterDescription.AABB = aABB;
							myClusterDescription.DynamicObjects = new List<MyObjectData>();
							myClusterDescription.StaticObjects = new List<MyObjectData>();
							MyClusterDescription item2 = myClusterDescription;
							foreach (MyObjectData item5 in myClusterDescription2.DynamicObjects.ToList())
							{
								if (aABB.Contains(item5.AABB) == ContainmentType.Contains)
								{
									item2.DynamicObjects.Add(item5);
									myClusterDescription2.DynamicObjects.Remove(item5);
								}
							}
							foreach (MyObjectData item6 in myClusterDescription2.StaticObjects.ToList())
							{
								ContainmentType containmentType = aABB.Contains(item6.AABB);
								if (containmentType == ContainmentType.Contains || containmentType == ContainmentType.Intersects)
								{
									item2.StaticObjects.Add(item6);
									myClusterDescription2.StaticObjects.Remove(item6);
								}
							}
							if (myClusterDescription2.DynamicObjects.Count > 0)
							{
								BoundingBoxD aABB2 = BoundingBoxD.CreateInvalid();
								foreach (MyObjectData dynamicObject in myClusterDescription2.DynamicObjects)
								{
									aABB2.Include(dynamicObject.AABB.GetInflated(idealClusterSize / 2f));
								}
								aABB2.InflateToMinimum(idealClusterSize);
								myClusterDescription = default(MyClusterDescription);
								myClusterDescription.AABB = aABB2;
								myClusterDescription.DynamicObjects = myClusterDescription2.DynamicObjects.ToList();
								myClusterDescription.StaticObjects = myClusterDescription2.StaticObjects.ToList();
								MyClusterDescription item3 = myClusterDescription;
								if (item3.AABB.Size.AbsMax() > (double)(2f * idealClusterSize.AbsMax()))
								{
									stack.Push(item3);
								}
								else
								{
									list.Add(item3);
								}
							}
							if (item2.AABB.Size.AbsMax() > (double)(2f * idealClusterSize.AbsMax()) && flag3)
							{
								stack.Push(item2);
							}
							else
							{
								list.Add(item2);
							}
						}
					}
					flag2 = false;
					foreach (MyClusterDescription item7 in list)
					{
						BoundingBoxD aABB3 = item7.AABB;
						if (aABB3.Size.AbsMax() > (double)MaximumClusterSize)
						{
							flag2 = true;
							idealClusterSize /= 2f;
							break;
						}
					}
					if (flag2)
					{
						num--;
					}
				}
				HashSet<MyCluster> hashSet2 = new HashSet<MyCluster>();
				HashSet<MyCluster> hashSet3 = new HashSet<MyCluster>();
				foreach (MyObjectData item8 in list2)
				{
					if (item8.Cluster != null)
					{
						hashSet2.Add(item8.Cluster);
						RemoveObjectFromCluster(item8, batch: true);
					}
				}
				foreach (MyObjectData item9 in list2)
				{
					if (item9.Cluster != null)
					{
						item9.ActivationHandler.FinishRemoveBatch(item9.Cluster.UserData);
						item9.Cluster = null;
					}
				}
				int num4 = 0;
				foreach (MyClusterDescription item10 in list)
				{
					BoundingBoxD clusterBB = item10.AABB;
					MyCluster myCluster = CreateCluster(ref clusterBB);
					foreach (MyObjectData dynamicObject2 in item10.DynamicObjects)
					{
						if (dynamicObject2.Cluster != null)
						{
							hashSet2.Add(dynamicObject2.Cluster);
							RemoveObjectFromCluster(dynamicObject2, batch: true);
						}
					}
					foreach (MyObjectData dynamicObject3 in item10.DynamicObjects)
					{
						if (dynamicObject3.Cluster != null)
						{
							dynamicObject3.ActivationHandler.FinishRemoveBatch(dynamicObject3.Cluster.UserData);
							dynamicObject3.Cluster = null;
						}
					}
					foreach (MyCluster item11 in hashSet2)
					{
						if (OnFinishBatch != null)
						{
							OnFinishBatch(item11.UserData);
						}
					}
					foreach (MyObjectData dynamicObject4 in item10.DynamicObjects)
					{
						AddObjectToCluster(myCluster, dynamicObject4.Id, batch: true);
					}
					foreach (MyObjectData staticObject in item10.StaticObjects)
					{
						if (myCluster.AABB.Contains(staticObject.AABB) != 0)
						{
							AddObjectToCluster(myCluster, staticObject.Id, batch: true);
							num4++;
						}
					}
					hashSet3.Add(myCluster);
				}
				foreach (MyCluster item12 in hashSet2)
				{
					RemoveCluster(item12);
				}
				foreach (MyCluster item13 in hashSet3)
				{
					if (OnFinishBatch != null)
					{
						OnFinishBatch(item13.UserData);
					}
					foreach (ulong @object in item13.Objects)
					{
						if (m_objectsData[@object].ActivationHandler != null)
						{
							m_objectsData[@object].ActivationHandler.FinishAddBatch();
						}
					}
				}
				if (OnClustersReordered != null)
				{
					OnClustersReordered();
				}
			}
		}

		public void GetAllStaticObjects(List<BoundingBoxD> staticObjects)
		{
			m_staticTree.GetAll(m_objectDataResultList, clear: true);
			staticObjects.Clear();
			foreach (ulong objectDataResult in m_objectDataResultList)
			{
				staticObjects.Add(m_objectsData[objectDataResult].AABB);
			}
		}

		public void Serialize(List<BoundingBoxD> list)
		{
			foreach (MyCluster cluster in m_clusters)
			{
				list.Add(cluster.AABB);
			}
		}

		public void Deserialize(List<BoundingBoxD> list)
		{
			foreach (MyObjectData value in m_objectsData.Values)
			{
				if (value.Cluster != null)
				{
					RemoveObjectFromCluster(value, batch: true);
				}
			}
			foreach (MyObjectData value2 in m_objectsData.Values)
			{
				if (value2.Cluster != null)
				{
					value2.ActivationHandler.FinishRemoveBatch(value2.Cluster.UserData);
					value2.Cluster = null;
				}
			}
			foreach (MyCluster cluster in m_clusters)
			{
				if (OnFinishBatch != null)
				{
					OnFinishBatch(cluster.UserData);
				}
			}
			while (m_clusters.Count > 0)
			{
				RemoveCluster(m_clusters[0]);
			}
			for (int i = 0; i < list.Count; i++)
			{
				BoundingBoxD clusterBB = list[i];
				CreateCluster(ref clusterBB);
			}
			foreach (KeyValuePair<ulong, MyObjectData> objectsDatum in m_objectsData)
			{
				m_clusterTree.OverlapAllBoundingBox(ref objectsDatum.Value.AABB, m_returnedClusters);
				if (m_returnedClusters.Count != 1 && !objectsDatum.Value.ActivationHandler.IsStaticForCluster)
				{
					throw new Exception($"Inconsistent objects and deserialized clusters. Entity name: {objectsDatum.Value.Tag}, Found clusters: {m_returnedClusters.Count}, Replicable exists: {GetEntityReplicableExistsById(objectsDatum.Value.EntityId)}");
				}
				if (m_returnedClusters.Count == 1)
				{
					AddObjectToCluster(m_returnedClusters[0], objectsDatum.Key, batch: true);
				}
			}
			foreach (MyCluster cluster2 in m_clusters)
			{
				if (OnFinishBatch != null)
				{
					OnFinishBatch(cluster2.UserData);
				}
				foreach (ulong @object in cluster2.Objects)
				{
					if (m_objectsData[@object].ActivationHandler != null)
					{
						m_objectsData[@object].ActivationHandler.FinishAddBatch();
					}
				}
			}
		}
	}
}
