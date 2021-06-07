namespace VRageRender.Import
{
	public static class MyTechniqueExtensions
	{
		public static bool IsTransparent(this MyMeshDrawTechnique technique)
		{
			if (technique != MyMeshDrawTechnique.GLASS && technique != MyMeshDrawTechnique.HOLO && technique != MyMeshDrawTechnique.SHIELD)
			{
				return technique == MyMeshDrawTechnique.SHIELD_LIT;
			}
			return true;
		}
	}
}
