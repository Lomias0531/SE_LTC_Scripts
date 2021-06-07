using VRage.Game;
using VRage.ObjectBuilders;

namespace VRage.Core
{
	public interface IMyObject
	{
		MyDefinitionId DefinitionId
		{
			get;
		}

		bool NeedsSerialize
		{
			get;
		}

		void Deserialize(MyObjectBuilder_Base builder);

		MyObjectBuilder_Base Serialize();
	}
}
