using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.Entity.UseObject
{
	public interface IMyUseObject
	{
		IMyEntity Owner
		{
			get;
		}

		MyModelDummy Dummy
		{
			get;
		}

		/// <summary>
		/// Consider object as being in interactive range only if distance from character is smaller or equal to this value
		/// </summary>
		float InteractiveDistance
		{
			get;
		}

		/// <summary>
		/// Matrix of object, scale represents size of object
		/// </summary>
		MatrixD ActivationMatrix
		{
			get;
		}

		/// <summary>
		/// Matrix of object, scale represents size of object
		/// </summary>
		MatrixD WorldMatrix
		{
			get;
		}

		/// <summary>
		/// Render ID of objects 
		/// </summary>
		uint RenderObjectID
		{
			get;
		}

		/// <summary>
		/// Instance ID of objects (this should mostly be unused
		/// </summary>
		int InstanceID
		{
			get;
		}

		/// <summary>
		/// Show overlay (semitransparent bounding box)
		/// </summary>
		bool ShowOverlay
		{
			get;
		}

		/// <summary>
		/// Returns supported actions
		/// </summary>
		UseActionEnum SupportedActions
		{
			get;
		}

		/// <summary>
		/// Main action of this use object
		/// </summary>
		UseActionEnum PrimaryAction
		{
			get;
		}

		/// <summary>
		/// Secondary action of this use object
		/// </summary>
		UseActionEnum SecondaryAction
		{
			get;
		}

		/// <summary>
		/// When true, use will be called every frame
		/// </summary>
		bool ContinuousUsage
		{
			get;
		}

		bool PlayIndicatorSound
		{
			get;
		}

		/// <summary>
		/// Uses object by specified action
		/// Caller calls this method only on supported actions
		/// </summary>
		void Use(UseActionEnum actionEnum, IMyEntity user);

		/// <summary>
		/// Gets action text
		/// Caller calls this method only on supported actions
		/// </summary>
		MyActionDescription GetActionInfo(UseActionEnum actionEnum);

		bool HandleInput();

		void OnSelectionLost();

		void SetRenderID(uint id);

		void SetInstanceID(int id);
	}
}
