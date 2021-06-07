namespace VRage.Library.Collections
{
	public interface IBitSerializable
	{
		/// <summary>
		/// When reading, returns false when validation was required and failed, otherwise true.
		/// </summary>
		bool Serialize(BitStream stream, bool validate, bool acceptAndSetValue = true);
	}
}
