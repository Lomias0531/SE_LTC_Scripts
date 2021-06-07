using System;

namespace VRage.Data
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class ModdableContentFileAttribute : Attribute
	{
		public string[] FileExtensions;

		public ModdableContentFileAttribute(string fileExtension)
		{
			FileExtensions = new string[1];
			FileExtensions[0] = fileExtension;
		}

		public ModdableContentFileAttribute(params string[] fileExtensions)
		{
			FileExtensions = fileExtensions;
		}
	}
}
