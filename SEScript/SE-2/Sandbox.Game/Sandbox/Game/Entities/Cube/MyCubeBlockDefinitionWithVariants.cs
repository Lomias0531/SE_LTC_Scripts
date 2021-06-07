using Sandbox.Definitions;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyCubeBlockDefinitionWithVariants
	{
		private MyCubeBlockDefinition m_baseDefinition;

		private int m_variantIndex = -1;

		public int VariantIndex => m_variantIndex;

		public MyCubeBlockDefinition Base => m_baseDefinition;

		public void Next()
		{
			if (m_baseDefinition.Variants != null && m_baseDefinition.Variants.Count > 0)
			{
				m_variantIndex++;
				m_variantIndex++;
				m_variantIndex %= m_baseDefinition.Variants.Count + 1;
				m_variantIndex--;
			}
		}

		public void Prev()
		{
			if (m_baseDefinition.Variants != null && m_baseDefinition.Variants.Count > 0)
			{
				m_variantIndex = (m_variantIndex + m_baseDefinition.Variants.Count + 1) % (m_baseDefinition.Variants.Count + 1);
				m_variantIndex--;
			}
		}

		public void Reset()
		{
			m_variantIndex = -1;
		}

		public MyCubeBlockDefinitionWithVariants(MyCubeBlockDefinition definition, int variantIndex)
		{
			m_baseDefinition = definition;
			m_variantIndex = variantIndex;
			if (m_baseDefinition.Variants == null || m_baseDefinition.Variants.Count == 0)
			{
				m_variantIndex = -1;
			}
			else if (m_variantIndex != -1)
			{
				m_variantIndex %= m_baseDefinition.Variants.Count;
			}
		}

		public static implicit operator MyCubeBlockDefinitionWithVariants(MyCubeBlockDefinition definition)
		{
			return new MyCubeBlockDefinitionWithVariants(definition, -1);
		}

		public static implicit operator MyCubeBlockDefinition(MyCubeBlockDefinitionWithVariants definition)
		{
			if (definition == null)
			{
				return null;
			}
			if (definition.m_variantIndex == -1)
			{
				return definition.m_baseDefinition;
			}
			return definition.m_baseDefinition.Variants[definition.m_variantIndex];
		}
	}
}
