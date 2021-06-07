using System;

namespace VRage.Game.Components
{
	public struct MyResourceSinkInfo
	{
		public MyDefinitionId ResourceTypeId;

		public float MaxRequiredInput;

		public Func<float> RequiredInputFunc;
	}
}
