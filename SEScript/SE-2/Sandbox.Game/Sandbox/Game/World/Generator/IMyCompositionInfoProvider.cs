using VRage.Game;

namespace Sandbox.Game.World.Generator
{
	internal interface IMyCompositionInfoProvider
	{
		IMyCompositeDeposit[] Deposits
		{
			get;
		}

		IMyCompositeShape[] FilledShapes
		{
			get;
		}

		IMyCompositeShape[] RemovedShapes
		{
			get;
		}

		MyVoxelMaterialDefinition DefaultMaterial
		{
			get;
		}

		void Close();
	}
}
