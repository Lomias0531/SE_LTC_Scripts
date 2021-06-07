namespace VRageRender.Messages
{
	public struct MyTextureChange
	{
		public string ColorMetalFileName;

		public string NormalGlossFileName;

		public string ExtensionsFileName;

		public string AlphamaskFileName;

		public bool IsDefault()
		{
			if (ColorMetalFileName == null && NormalGlossFileName == null && ExtensionsFileName == null)
			{
				return AlphamaskFileName == null;
			}
			return false;
		}
	}
}
