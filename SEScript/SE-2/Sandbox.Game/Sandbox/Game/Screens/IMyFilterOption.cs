namespace Sandbox.Game.Screens
{
	public interface IMyFilterOption
	{
		string SerializedValue
		{
			get;
		}

		void Configure(string value);
	}
}
