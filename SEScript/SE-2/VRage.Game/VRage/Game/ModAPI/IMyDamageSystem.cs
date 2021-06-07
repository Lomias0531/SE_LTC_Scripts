using System;

namespace VRage.Game.ModAPI
{
	public interface IMyDamageSystem
	{
		/// <summary>
		/// Registers a handler for when an object in game is destroyed.
		/// </summary>
		/// <param name="priority">Priority level.  Lower means higher priority.</param>
		/// <param name="handler">Actual handler delegate</param>
		void RegisterDestroyHandler(int priority, Action<object, MyDamageInformation> handler);

		/// <summary>
		/// Registers a handler that is called before an object in game is damaged.  The damage can be modified in this handler.
		/// </summary>
		/// <param name="priority">Priority level.  Lower means higher priority.</param>
		/// <param name="handler">Actual handler delegate</param>
		void RegisterBeforeDamageHandler(int priority, BeforeDamageApplied handler);

		/// <summary>
		/// Registers a handler that is called after an object in game is damaged.
		/// </summary>
		/// <param name="priority">Priority level.  Lower means higher priority.</param>
		/// <param name="handler">Actual handler delegate</param>
		void RegisterAfterDamageHandler(int priority, Action<object, MyDamageInformation> handler);
	}
}
