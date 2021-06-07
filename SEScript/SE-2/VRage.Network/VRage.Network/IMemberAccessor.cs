namespace VRage.Network
{
	public interface IMemberAccessor
	{
	}
	public interface IMemberAccessor<TOwner, TMember> : IMemberAccessor
	{
		void Set(ref TOwner owner, in TMember value);

		void Get(ref TOwner owner, out TMember value);
	}
}
