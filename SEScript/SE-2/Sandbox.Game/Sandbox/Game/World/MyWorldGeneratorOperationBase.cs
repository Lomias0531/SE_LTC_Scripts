using VRage.Game;

namespace Sandbox.Game.World
{
	public abstract class MyWorldGeneratorOperationBase
	{
		public string FactionTag;

		public abstract void Apply();

		public virtual void Init(MyObjectBuilder_WorldGeneratorOperation builder)
		{
			FactionTag = builder.FactionTag;
		}

		public virtual MyObjectBuilder_WorldGeneratorOperation GetObjectBuilder()
		{
			MyObjectBuilder_WorldGeneratorOperation myObjectBuilder_WorldGeneratorOperation = MyWorldGenerator.OperationFactory.CreateObjectBuilder(this);
			myObjectBuilder_WorldGeneratorOperation.FactionTag = FactionTag;
			return myObjectBuilder_WorldGeneratorOperation;
		}
	}
}
