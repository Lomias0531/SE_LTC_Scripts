using System;
using VRage.Collections;
using VRage.ModAPI;

namespace VRage.Game.Components
{
	public abstract class MyResourceSinkComponentBase : MyEntityComponentBase
	{
		public abstract ListReader<MyDefinitionId> AcceptedResources
		{
			get;
		}

		public abstract IMyEntity TemporaryConnectedEntity
		{
			get;
			set;
		}

		public abstract float CurrentInputByType(MyDefinitionId resourceTypeId);

		public abstract bool IsPowerAvailable(MyDefinitionId resourceTypeId, float power);

		public abstract bool IsPoweredByType(MyDefinitionId resourceTypeId);

		public abstract float MaxRequiredInputByType(MyDefinitionId resourceTypeId);

		public abstract void SetMaxRequiredInputByType(MyDefinitionId resourceTypeId, float newMaxRequiredInput);

		public abstract float RequiredInputByType(MyDefinitionId resourceTypeId);

		public abstract void SetInputFromDistributor(MyDefinitionId resourceTypeId, float newResourceInput, bool isAdaptible, bool fireEvents = true);

		public abstract void SetRequiredInputByType(MyDefinitionId resourceTypeId, float newRequiredInput);

		/// <summary>
		/// Change the required input function (callback) for given type of resource. It does not call it immediatelly to update required input value.
		/// </summary>
		public abstract void SetRequiredInputFuncByType(MyDefinitionId resourceTypeId, Func<float> newRequiredInputFunc);

		public abstract float SuppliedRatioByType(MyDefinitionId resourceTypeId);
	}
}
