namespace VRage.Serialization
{
	/// <summary>
	/// Primitive flags are passed down the object hierarchy.
	/// </summary>
	public enum MyPrimitiveFlags
	{
		/// <summary>
		/// No primitive flags.
		/// </summary>
		None = 0,
		/// <summary>
		/// Flag which indicates whether primitive is signed.
		/// </summary>
		Signed = 1,
		/// <summary>
		/// Flag which indicates whether primitive is normalized.
		/// Exact behavior depends on actual type (0..1, -1..1, normalized vector, etc)
		/// </summary>
		Normalized = 2,
		/// <summary>
		/// Serialize member as unsigned variant (variable length integer, 0-127...1-byte, 127-32767...2-byte, etc)
		/// </summary>
		Variant = 4,
		/// <summary>
		/// Serialize member as unsigned signed (-63..64, 1-byte etc, -32767..32768, 2-byte, etc)
		/// </summary>
		VariantSigned = 5,
		/// <summary>
		/// Serialize string in ascii encoding
		/// </summary>
		Ascii = 8,
		/// <summary>
		/// Serialize string in UTF8 encoding
		/// </summary>
		Utf8 = 0x10,
		/// <summary>
		/// Fixed point 8-bit precision, use with normalized
		/// </summary>
		FixedPoint8 = 0x20,
		/// <summary>
		/// Fixed point 16-bit precision, use with normalized
		/// </summary>
		FixedPoint16 = 0x40
	}
}
