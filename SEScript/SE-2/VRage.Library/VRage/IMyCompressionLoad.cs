namespace VRage
{
	public interface IMyCompressionLoad
	{
		int GetInt32();

		byte GetByte();

		int GetBytes(int bytes, byte[] output);

		bool EndOfFile();
	}
}
