using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public abstract class MyLogicalEnvironmentSectorBase : IMyEventProxy, IMyEventOwner
	{
		public delegate void ItemIterator(int index, ref ItemInfo item);

		public long Id;

		public Vector3D WorldPos;

		public Vector3D[] Bounds;

		public IMyEnvironmentOwner Owner
		{
			get;
			protected set;
		}

		public abstract string DebugData
		{
			get;
		}

		public abstract bool ServerOwned
		{
			get;
			internal set;
		}

		public int MinLod
		{
			get;
			protected set;
		}

		public event Action OnClose;

		public abstract void EnableItem(int itemId, bool enabled);

		public abstract void UpdateItemModel(int itemId, short modelId);

		public abstract void UpdateItemModelBatch(List<int> items, short newModelId);

		public abstract void GetItemDefinition(ushort index, out MyRuntimeEnvironmentItemInfo def);

		public abstract void RaiseItemEvent<T>(int logicalItem, ref MyDefinitionId modDef, T eventData, bool fromClient);

		public abstract void Init(MyObjectBuilder_EnvironmentSector sectorBuilder);

		public abstract MyObjectBuilder_EnvironmentSector GetObjectBuilder();

		public virtual void Close()
		{
			this.OnClose?.Invoke();
		}

		public abstract void DebugDraw(int lod);

		public abstract void DisableItemsInBox(Vector3D center, ref BoundingBoxD box);

		public abstract void GetItemsInAabb(ref BoundingBoxD aabb, List<int> itemsInBox);

		public abstract void GetItem(int logicalItem, out ItemInfo item);

		public abstract void IterateItems(ItemIterator action);

		public abstract void InvalidateItem(int itemId);
	}
}
