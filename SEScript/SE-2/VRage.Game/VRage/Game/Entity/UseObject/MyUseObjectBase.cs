using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.Entity.UseObject
{
	public abstract class MyUseObjectBase : IMyUseObject
	{
		public IMyEntity Owner
		{
			get;
			private set;
		}

		public MyModelDummy Dummy
		{
			get;
			private set;
		}

		public abstract float InteractiveDistance
		{
			get;
		}

		public abstract MatrixD ActivationMatrix
		{
			get;
		}

		public abstract MatrixD WorldMatrix
		{
			get;
		}

		public abstract uint RenderObjectID
		{
			get;
		}

		public virtual int InstanceID => -1;

		public abstract bool ShowOverlay
		{
			get;
		}

		public abstract UseActionEnum SupportedActions
		{
			get;
		}

		public abstract UseActionEnum PrimaryAction
		{
			get;
		}

		public abstract UseActionEnum SecondaryAction
		{
			get;
		}

		public abstract bool ContinuousUsage
		{
			get;
		}

		public abstract bool PlayIndicatorSound
		{
			get;
		}

		public MyUseObjectBase(IMyEntity owner, MyModelDummy dummy)
		{
			Owner = owner;
			Dummy = dummy;
		}

		public abstract void Use(UseActionEnum actionEnum, IMyEntity user);

		public abstract MyActionDescription GetActionInfo(UseActionEnum actionEnum);

		public abstract bool HandleInput();

		public abstract void OnSelectionLost();

		public virtual void SetRenderID(uint id)
		{
		}

		public virtual void SetInstanceID(int id)
		{
		}
	}
}
