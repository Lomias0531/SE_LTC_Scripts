using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.ModAPI
{
	public interface IMyEntity : VRage.Game.ModAPI.Ingame.IMyEntity
	{
		new MyEntityComponentContainer Components
		{
			get;
		}

		MyPhysicsComponentBase Physics
		{
			get;
			set;
		}

		MyPositionComponentBase PositionComp
		{
			get;
			set;
		}

		MyRenderComponentBase Render
		{
			get;
			set;
		}

		MyEntityComponentBase GameLogic
		{
			get;
			set;
		}

		MyHierarchyComponentBase Hierarchy
		{
			get;
			set;
		}

		MySyncComponentBase SyncObject
		{
			get;
		}

		/// <summary>
		/// Custom storage for mods. Shared with all mods.
		/// </summary>
		/// <remarks>Not synced, but saved with blueprints.
		/// Only use set accessor if value is null.
		/// </remarks>
		MyModStorageComponentBase Storage
		{
			get;
			set;
		}

		EntityFlags Flags
		{
			get;
			set;
		}

		new long EntityId
		{
			get;
			set;
		}

		new string Name
		{
			get;
			set;
		}

		bool MarkedForClose
		{
			get;
		}

		bool Closed
		{
			get;
		}

		bool DebugAsyncLoading
		{
			get;
		}

		bool Save
		{
			get;
			set;
		}

		MyPersistentEntityFlags2 PersistentFlags
		{
			get;
			set;
		}

		IMyModel Model
		{
			get;
		}

		/// <summary>
		/// Gets or sets if the entity should be synced.
		/// </summary>
		bool Synchronized
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets how often the entity should be updated.
		/// </summary>
		MyEntityUpdateEnum NeedsUpdate
		{
			get;
			set;
		}

		IMyEntity Parent
		{
			get;
		}

		Matrix LocalMatrix
		{
			get;
			set;
		}

		bool NearFlag
		{
			get;
			set;
		}

		bool CastShadows
		{
			get;
			set;
		}

		bool FastCastShadowResolve
		{
			get;
			set;
		}

		bool NeedsResolveCastShadow
		{
			get;
			set;
		}

		float MaxGlassDistSq
		{
			get;
		}

		bool NeedsDraw
		{
			get;
			set;
		}

		bool NeedsDrawFromParent
		{
			get;
			set;
		}

		bool Transparent
		{
			get;
			set;
		}

		bool ShadowBoxLod
		{
			get;
			set;
		}

		bool SkipIfTooSmall
		{
			get;
			set;
		}

		bool Visible
		{
			get;
			set;
		}

		bool NeedsWorldMatrix
		{
			get;
			set;
		}

		bool InScene
		{
			get;
			set;
		}

		bool InvalidateOnMove
		{
			get;
		}

		new MatrixD WorldMatrix
		{
			get;
			set;
		}

		MatrixD WorldMatrixInvScaled
		{
			get;
		}

		MatrixD WorldMatrixNormalizedInv
		{
			get;
		}

		bool IsVolumetric
		{
			get;
		}

		BoundingBox LocalAABB
		{
			get;
			set;
		}

		BoundingBox LocalAABBHr
		{
			get;
		}

		BoundingSphere LocalVolume
		{
			get;
			set;
		}

		Vector3 LocalVolumeOffset
		{
			get;
			set;
		}

		[Obsolete]
		Vector3D LocationForHudMarker
		{
			get;
		}

		[Obsolete]
		bool IsCCDForProjectiles
		{
			get;
		}

		new string DisplayName
		{
			get;
			set;
		}

		event Action<IMyEntity> OnClose;

		event Action<IMyEntity> OnClosing;

		event Action<IMyEntity> OnMarkForClose;

		event Action<IMyEntity> OnPhysicsChanged;

		string GetFriendlyName();

		void Close();

		void Delete();

		MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false);

		void BeforeSave();

		IMyEntity GetTopMostParent(Type type = null);

		void SetLocalMatrix(Matrix localMatrix, object source = null);

		void GetChildren(List<IMyEntity> children, Func<IMyEntity, bool> collect = null);

		MyEntitySubpart GetSubpart(string name);

		bool TryGetSubpart(string name, out MyEntitySubpart subpart);

		Vector3 GetDiffuseColor();

		bool IsVisible();

		void DebugDraw();

		void DebugDrawInvalidTriangles();

		void EnableColorMaskForSubparts(bool enable);

		void SetColorMaskForSubparts(Vector3 colorMaskHsv);

		void SetTextureChangesForSubparts(Dictionary<string, MyTextureChange> value);

		/// <summary>
		/// Sets the emissive value of a specific emissive material on entity.
		/// </summary>
		/// <param name="emissiveName">The name of the emissive material (ie. "Emissive0")</param>
		/// <param name="emissivity">Level of emissivity (0 is off, 1 is full brightness)</param>
		/// <param name="emissivePartColor">Color to emit</param>
		void SetEmissiveParts(string emissiveName, Color emissivePartColor, float emissivity);

		/// <summary>
		/// Sets the emissive value of a specific emissive material on all entity subparts.
		/// </summary>
		/// <param name="emissiveName">The name of the emissive material (ie. "Emissive0")</param>
		/// <param name="emissivity">Level of emissivity (0 is off, 1 is full brightness).</param>
		/// <param name="emissivePartColor">Color to emit</param>
		void SetEmissivePartsForSubparts(string emissiveName, Color emissivePartColor, float emissivity);

		float GetDistanceBetweenCameraAndBoundingSphere();

		float GetDistanceBetweenCameraAndPosition();

		float GetLargestDistanceBetweenCameraAndBoundingSphere();

		float GetSmallestDistanceBetweenCameraAndBoundingSphere();

		void OnRemovedFromScene(object source);

		void OnAddedToScene(object source);

		MatrixD GetViewMatrix();

		MatrixD GetWorldMatrixNormalizedInv();

		void SetWorldMatrix(MatrixD worldMatrix, object source = null);

		void SetPosition(Vector3D pos);

		void Teleport(MatrixD pos, object source = null, bool ignoreAssert = false);

		bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? tri, IntersectionFlags flags);

		Vector3D? GetIntersectionWithLineAndBoundingSphere(ref LineD line, float boundingSphereRadiusMultiplier);

		bool GetIntersectionWithSphere(ref BoundingSphereD sphere);

		bool GetIntersectionWithAABB(ref BoundingBoxD aabb);

		void GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles);

		bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos);

		/// <summary>
		/// Simply get the MyInventoryBase component stored in this entity.
		/// </summary>
		/// <returns></returns>
		new VRage.Game.ModAPI.IMyInventory GetInventory();

		/// <summary>
		/// Search for inventory component with maching index.
		/// </summary>
		new VRage.Game.ModAPI.IMyInventory GetInventory(int index);

		[Obsolete("Only used during Sandbox removal.")]
		void AddToGamePruningStructure();

		[Obsolete("Only used during Sandbox removal.")]
		void RemoveFromGamePruningStructure();

		[Obsolete("Only used during Sandbox removal.")]
		void UpdateGamePruningStructure();
	}
}
