using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.GameServices
{
	[Serializable]
	[XmlRoot("ModMetadata")]
	public class ModMetadataFile
	{
		protected class VRage_GameServices_ModMetadataFile_003C_003EModVersion_003C_003EAccessor : IMemberAccessor<ModMetadataFile, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ModMetadataFile owner, in string value)
			{
				owner.ModVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ModMetadataFile owner, out string value)
			{
				value = owner.ModVersion;
			}
		}

		protected class VRage_GameServices_ModMetadataFile_003C_003EMinGameVersion_003C_003EAccessor : IMemberAccessor<ModMetadataFile, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ModMetadataFile owner, in string value)
			{
				owner.MinGameVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ModMetadataFile owner, out string value)
			{
				value = owner.MinGameVersion;
			}
		}

		protected class VRage_GameServices_ModMetadataFile_003C_003EMaxGameVersion_003C_003EAccessor : IMemberAccessor<ModMetadataFile, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ModMetadataFile owner, in string value)
			{
				owner.MaxGameVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ModMetadataFile owner, out string value)
			{
				value = owner.MaxGameVersion;
			}
		}

		[XmlElement("ModVersion")]
		public string ModVersion;

		[XmlElement("MinGameVersion")]
		public string MinGameVersion;

		[XmlElement("MaxGameVersion")]
		public string MaxGameVersion;
	}
}
