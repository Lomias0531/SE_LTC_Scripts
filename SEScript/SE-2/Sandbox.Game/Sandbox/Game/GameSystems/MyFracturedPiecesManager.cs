using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyFracturedPiecesManager : MySessionComponentBase
	{
		private struct Bodies
		{
			public HkRigidBody Rigid;

			public HkdBreakableBody Breakable;
		}

		public const int FakePieceLayer = 14;

		public static MyFracturedPiecesManager Static;

		private static float LIFE_OF_CUBIC_PIECE = 300f;

		private Queue<MyFracturedPiece> m_piecesPool = new Queue<MyFracturedPiece>();

		private const int MAX_ALLOC_PER_FRAME = 50;

		private int m_allocatedThisFrame;

		private HashSet<HkdBreakableBody> m_tmpToReturn = new HashSet<HkdBreakableBody>();

		private HashSet<long> m_dbgCreated = new HashSet<long>();

		private HashSet<long> m_dbgRemoved = new HashSet<long>();

		private List<HkBodyCollision> m_rigidList = new List<HkBodyCollision>();

		private int m_addedThisFrame;

		private Queue<Bodies> m_bodyPool = new Queue<Bodies>();

		private const int PREALLOCATE_PIECES = 400;

		private const int PREALLOCATE_BODIES = 400;

		public HashSet<HkRigidBody> m_givenRBs = new HashSet<HkRigidBody>(InstanceComparer<HkRigidBody>.Default);

		public override bool IsRequiredByGame => MyPerGameSettings.Destruction;

		public override void LoadData()
		{
			base.LoadData();
			InitPools();
			Static = this;
		}

		private MyFracturedPiece AllocatePiece()
		{
			m_allocatedThisFrame++;
			MyFracturedPiece obj = MyEntities.CreateEntity(new MyDefinitionId(typeof(MyObjectBuilder_FracturedPiece)), fadeIn: false) as MyFracturedPiece;
			obj.Physics = new MyPhysicsBody(obj, RigidBodyFlag.RBF_DEBRIS);
			obj.Physics.CanUpdateAccelerations = true;
			return obj;
		}

		protected override void UnloadData()
		{
			foreach (Bodies item in m_bodyPool)
			{
				item.Breakable.ClearListener();
			}
			m_bodyPool.Clear();
			m_piecesPool.Clear();
			base.UnloadData();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			foreach (HkdBreakableBody item in m_tmpToReturn)
			{
				ReturnToPoolInternal(item);
			}
			m_tmpToReturn.Clear();
			while (m_bodyPool.Count < 400 && m_allocatedThisFrame < 50)
			{
				m_bodyPool.Enqueue(AllocateBodies());
			}
			while (m_piecesPool.Count < 400 && m_allocatedThisFrame < 50)
			{
				m_piecesPool.Enqueue(AllocatePiece());
			}
			m_allocatedThisFrame = 0;
		}

		private void RemoveInternal(MyFracturedPiece fp, bool fromServer = false)
		{
			if (fp.Physics != null && fp.Physics.RigidBody != null && fp.Physics.RigidBody.IsDisposed)
			{
				fp.Physics.BreakableBody = fp.Physics.BreakableBody;
			}
			if (fp.Physics == null || fp.Physics.RigidBody == null || fp.Physics.RigidBody.IsDisposed)
			{
				MyEntities.Remove(fp);
				return;
			}
			if (!fp.Physics.RigidBody.IsActive)
			{
				fp.Physics.RigidBody.Activate();
			}
			MyPhysics.RemoveDestructions(fp.Physics.RigidBody);
			HkdBreakableBody breakableBody = fp.Physics.BreakableBody;
			breakableBody.AfterReplaceBody -= fp.Physics.FracturedBody_AfterReplaceBody;
			ReturnToPool(breakableBody);
			fp.Physics.Enabled = false;
			MyEntities.Remove(fp);
			fp.Physics.BreakableBody = null;
			fp.Render.ClearModels();
			fp.OriginalBlocks.Clear();
			_ = Sync.IsServer;
			fp.EntityId = 0L;
			fp.Physics.BreakableBody = null;
			m_piecesPool.Enqueue(fp);
		}

		public MyFracturedPiece GetPieceFromPool(long entityId, bool fromServer = false)
		{
			_ = Sync.IsServer;
			MyFracturedPiece myFracturedPiece = (m_piecesPool.Count != 0) ? m_piecesPool.Dequeue() : AllocatePiece();
			if (Sync.IsServer)
			{
				myFracturedPiece.EntityId = MyEntityIdentifier.AllocateId();
			}
			return myFracturedPiece;
		}

		public void GetFracturesInSphere(ref BoundingSphereD searchSphere, ref List<MyFracturedPiece> output)
		{
			HkShape shape = new HkSphereShape((float)searchSphere.Radius);
			try
			{
				MyPhysics.GetPenetrationsShape(shape, ref searchSphere.Center, ref Quaternion.Identity, m_rigidList, 12);
				foreach (HkBodyCollision rigid in m_rigidList)
				{
					MyFracturedPiece myFracturedPiece = rigid.GetCollisionEntity() as MyFracturedPiece;
					if (myFracturedPiece != null)
					{
						output.Add(myFracturedPiece);
					}
				}
			}
			finally
			{
				m_rigidList.Clear();
				shape.RemoveReference();
			}
		}

		public void GetFracturesInBox(ref BoundingBoxD searchBox, List<MyFracturedPiece> output)
		{
			m_rigidList.Clear();
			HkShape shape = new HkBoxShape(searchBox.HalfExtents);
			try
			{
				Vector3D translation = searchBox.Center;
				MyPhysics.GetPenetrationsShape(shape, ref translation, ref Quaternion.Identity, m_rigidList, 12);
				foreach (HkBodyCollision rigid in m_rigidList)
				{
					MyFracturedPiece myFracturedPiece = rigid.GetCollisionEntity() as MyFracturedPiece;
					if (myFracturedPiece != null)
					{
						output.Add(myFracturedPiece);
					}
				}
			}
			finally
			{
				m_rigidList.Clear();
				shape.RemoveReference();
			}
		}

		private Bodies AllocateBodies()
		{
			m_allocatedThisFrame++;
			Bodies result = default(Bodies);
			result.Rigid = null;
			result.Breakable = HkdBreakableBody.Allocate();
			return result;
		}

		public void InitPools()
		{
			for (int i = 0; i < 400; i++)
			{
				m_piecesPool.Enqueue(AllocatePiece());
			}
			for (int j = 0; j < 400; j++)
			{
				m_bodyPool.Enqueue(AllocateBodies());
			}
		}

		public HkdBreakableBody GetBreakableBody(HkdBreakableBodyInfo bodyInfo)
		{
			Bodies bodies = (m_bodyPool.Count != 0) ? m_bodyPool.Dequeue() : AllocateBodies();
			bodies.Breakable.Initialize(bodyInfo, bodies.Rigid);
			return bodies.Breakable;
		}

		public void RemoveFracturePiece(MyFracturedPiece piece, float blendTimeSeconds, bool fromServer = false, bool sync = true)
		{
			if (blendTimeSeconds == 0f)
			{
				RemoveInternal(piece, fromServer);
			}
		}

		public void RemoveFracturesInBox(ref BoundingBoxD box, float blendTimeSeconds)
		{
			if (Sync.IsServer)
			{
				List<MyFracturedPiece> list = new List<MyFracturedPiece>();
				GetFracturesInBox(ref box, list);
				foreach (MyFracturedPiece item in list)
				{
					RemoveFracturePiece(item, blendTimeSeconds);
				}
			}
		}

		public void RemoveFracturesInSphere(Vector3D center, float radius)
		{
			float num = radius * radius;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (entity is MyFracturedPiece && (radius <= 0f || (center - entity.Physics.CenterOfMassWorld).LengthSquared() < (double)num))
				{
					Static.RemoveFracturePiece(entity as MyFracturedPiece, 2f);
				}
			}
		}

		public void ReturnToPool(HkdBreakableBody body)
		{
			m_tmpToReturn.Add(body);
		}

		private void ReturnToPoolInternal(HkdBreakableBody body)
		{
			HkRigidBody rigidBody = body.GetRigidBody();
			if (!(rigidBody == null))
			{
				rigidBody.ContactPointCallbackEnabled = false;
				m_givenRBs.Remove(rigidBody);
				foreach (Bodies item2 in m_bodyPool)
				{
					if (!(body == item2.Breakable))
					{
						_ = (rigidBody == item2.Rigid);
					}
				}
				body.BreakableShape.ClearConnections();
				body.Clear();
				Bodies item = default(Bodies);
				item.Rigid = rigidBody;
				item.Breakable = body;
				body.InitListener();
				m_bodyPool.Enqueue(item);
			}
		}

		internal void DbgCheck(long createdId, long removedId)
		{
		}
	}
}
