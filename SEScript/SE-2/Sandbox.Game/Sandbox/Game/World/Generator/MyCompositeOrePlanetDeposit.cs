using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	internal class MyCompositeOrePlanetDeposit : MyCompositeShapeOreDeposit
	{
		private float m_minDepth;

		private const float DEPOSIT_MAX_SIZE = 1000f;

		private int m_numDeposits;

		private Dictionary<Vector3I, MyCompositeShapeOreDeposit> m_deposits = new Dictionary<Vector3I, MyCompositeShapeOreDeposit>();

		private Dictionary<string, List<MyVoxelMaterialDefinition>> m_materialsByOreType = new Dictionary<string, List<MyVoxelMaterialDefinition>>();

		public float MinDepth => m_minDepth;

		public MyCompositeOrePlanetDeposit(MyCsgShapeBase baseShape, int seed, float minDepth, float maxDepth, MyOreProbability[] oreProbabilties, MyVoxelMaterialDefinition material)
			: base(baseShape, material)
		{
			m_minDepth = minDepth;
			double num = 12.566371917724609 * Math.Pow(minDepth, 3.0) / 3.0;
			double num2 = 12.566371917724609 * Math.Pow(maxDepth, 3.0) / 3.0;
			double num3 = 12.566371917724609 * Math.Pow(1000.0, 3.0) / 3.0;
			double num4 = num - num2;
			m_numDeposits = ((oreProbabilties.Length != 0) ? ((int)Math.Floor(num4 * 0.40000000596046448 / num3)) : 0);
			_ = minDepth / 1000f;
			MyRandom instance = MyRandom.Instance;
			FillMaterialCollections();
			Vector3D value = -new Vector3D(500.0);
			using (instance.PushSeed(seed))
			{
				for (int i = 0; i < m_numDeposits; i++)
				{
					Vector3D randomDirection = MyProceduralWorldGenerator.GetRandomDirection(instance);
					float num5 = instance.NextFloat(maxDepth, minDepth);
					Vector3D value2 = randomDirection * num5;
					Vector3I vector3I = Vector3I.Ceiling((Shape.Center() + value2) / 1000.0);
					if (!m_deposits.TryGetValue(vector3I, out MyCompositeShapeOreDeposit value3))
					{
						MyOreProbability ore = GetOre(instance.NextFloat(0f, 1f), oreProbabilties);
						MyVoxelMaterialDefinition material2 = m_materialsByOreType[ore.OreName][instance.Next() % m_materialsByOreType[ore.OreName].Count];
						value3 = new MyCompositeShapeOreDeposit(new MyCsgSimpleSphere(vector3I * 1000f + value, instance.NextFloat(64f, 500f)), material2);
						m_deposits[vector3I] = value3;
					}
				}
			}
			m_materialsByOreType.Clear();
		}

		public override MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 pos, float lodSize)
		{
			Vector3I key = Vector3I.Ceiling(pos / 1000f);
			if (m_deposits.TryGetValue(key, out MyCompositeShapeOreDeposit value) && value.Shape.SignedDistance(ref pos, lodSize, null, null) == -1f)
			{
				return value.GetMaterialForPosition(ref pos, lodSize);
			}
			return null;
		}

		private MyOreProbability GetOre(float probability, MyOreProbability[] probalities)
		{
			foreach (MyOreProbability myOreProbability in probalities)
			{
				if (myOreProbability.CummulativeProbability >= probability)
				{
					return myOreProbability;
				}
			}
			return null;
		}

		private void FillMaterialCollections()
		{
			foreach (MyVoxelMaterialDefinition voxelMaterialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
			{
				if (voxelMaterialDefinition.MinedOre != "Organic")
				{
					if (!m_materialsByOreType.TryGetValue(voxelMaterialDefinition.MinedOre, out List<MyVoxelMaterialDefinition> value))
					{
						value = new List<MyVoxelMaterialDefinition>();
					}
					value.Add(voxelMaterialDefinition);
					m_materialsByOreType[voxelMaterialDefinition.MinedOre] = value;
				}
			}
		}

		public override void DebugDraw(ref MatrixD translation, Color materialColor)
		{
			foreach (KeyValuePair<Vector3I, MyCompositeShapeOreDeposit> deposit in m_deposits)
			{
				deposit.Value.DebugDraw(ref translation, materialColor);
			}
		}
	}
}
