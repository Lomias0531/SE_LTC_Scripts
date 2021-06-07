using VRage.Render.Scene.Components;
using VRageMath;

namespace VRage.Render.Scene
{
	public abstract class MyScene
	{
		public static long FrameCounter;

		public readonly IMyDebugDraw DebugDraw;

		public readonly IMyActorFactory ActorFactory;

		public readonly IMyBillboardsHelper BillboardsHelper;

		public readonly IMyComponentFactory ComponentFactory;

		public MyEnvironment Environment;

		public readonly MyActorUpdater Updater = new MyActorUpdater();

		public readonly MyDynamicAABBTreeD ManualCullTree;

		public readonly MyDynamicAABBTreeD MergeGroupsDBVH;

		public readonly MyDynamicAABBTreeD DynamicRenderablesDBVH;

		public readonly MyDynamicAABBTreeD DynamicRenderablesFarDBVH;

		public abstract float FadeOutTime
		{
			get;
		}

		protected MyScene(IMyDebugDraw debugDraw, IMyComponentFactory componentFactory, IMyActorFactory actorFactory, IMyBillboardsHelper billboardsHelper, MyDynamicAABBTreeD manualCullTree, MyDynamicAABBTreeD mergeGroupsDBVH, MyDynamicAABBTreeD dynamicRenderablesDBVH, MyDynamicAABBTreeD dynamicRenderablesFarDBVH)
		{
			DebugDraw = debugDraw;
			ActorFactory = actorFactory;
			ManualCullTree = manualCullTree;
			BillboardsHelper = billboardsHelper;
			MergeGroupsDBVH = mergeGroupsDBVH;
			ComponentFactory = componentFactory;
			DynamicRenderablesDBVH = dynamicRenderablesDBVH;
			DynamicRenderablesFarDBVH = dynamicRenderablesFarDBVH;
		}

		public abstract MyManualCullTreeData AllocateGroupData();

		public abstract void FreeGroupData(MyManualCullTreeData data);

		public abstract MyChildCullTreeData CompileCullData(MyChildCullTreeData data);

		public abstract void SetActorParent(MyActor parent, MyActor child, Matrix? childToParent);

		public virtual void Clear()
		{
			DynamicRenderablesDBVH.Clear();
			DynamicRenderablesFarDBVH.Clear();
			MergeGroupsDBVH.Clear();
		}
	}
}
