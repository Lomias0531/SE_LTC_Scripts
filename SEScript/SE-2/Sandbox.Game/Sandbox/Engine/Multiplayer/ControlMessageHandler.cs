namespace Sandbox.Engine.Multiplayer
{
	public delegate void ControlMessageHandler<T>(ref T message, ulong sender) where T : struct;
}
