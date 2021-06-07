namespace VRage.Game.Entity.EntityComponents.Interfaces
{
	/// <summary>
	/// This interface is for internal use only.
	/// This basically a way to use internal members without the InternalsVisibleTo attribute.
	/// The members defined here help us hide some internal logic from modders.
	/// DO NOT WHITELIST THIS!
	/// </summary>
	public interface IMyGameLogicComponent
	{
		/// <summary>
		/// Components with this flag will update at the same time as their parent entity.
		/// When the parent entity stops updating, so does the component.
		/// This flag basically reverts to the old system.
		/// </summary>
		bool EntityUpdate
		{
			get;
			set;
		}

		void UpdateOnceBeforeFrame(bool entityUpdate);

		void UpdateBeforeSimulation(bool entityUpdate);

		void UpdateBeforeSimulation10(bool entityUpdate);

		void UpdateBeforeSimulation100(bool entityUpdate);

		void UpdateAfterSimulation(bool entityUpdate);

		void UpdateAfterSimulation10(bool entityUpdate);

		void UpdateAfterSimulation100(bool entityUpdate);

		void RegisterForUpdate();

		void UnregisterForUpdate();

		void Close();
	}
}
