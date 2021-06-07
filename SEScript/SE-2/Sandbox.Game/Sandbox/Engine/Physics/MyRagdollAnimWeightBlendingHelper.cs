using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Engine.Physics
{
	public class MyRagdollAnimWeightBlendingHelper
	{
		private struct BoneData
		{
			public MyStringId WeightId;

			public MyStringId BlendTimeId;

			public double BlendTimeMs;

			public double StartedMs;

			public float StartingWeight;

			public float TargetWeight;

			public float PrevWeight;

			public LayerData[] Layers;
		}

		private struct LayerData
		{
			public MyStringId LayerId;

			public MyStringId LayerBlendTimeId;
		}

		private const string RAGDOLL_WEIGHT_VARIABLE_PREFIX = "rd_weight_";

		private const string RAGDOLL_BLEND_TIME_VARIABLE_PREFIX = "rd_blend_time_";

		private const string RAGDOLL_DEFAULT_BLEND_TIME_VARIABLE_NAME = "rd_default_blend_time";

		private const float DEFAULT_BLEND_TIME = 2.5f;

		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private BoneData[] m_boneIndexToData;

		private float m_defaultBlendTime = 0.8f;

		private MyStringId m_defautlBlendTimeId;

		public bool Initialized
		{
			get;
			private set;
		}

		public void Init(MyCharacterBone[] bones, MyAnimationController controller)
		{
			List<MyAnimationStateMachine> list = new List<MyAnimationStateMachine>(controller.GetLayerCount());
			for (int i = 0; i < controller.GetLayerCount(); i++)
			{
				list.Add(controller.GetLayerByIndex(i));
			}
			m_boneIndexToData = new BoneData[bones.Length];
			foreach (MyCharacterBone bone in bones)
			{
				m_boneIndexToData[bone.Index] = new BoneData
				{
					WeightId = MyStringId.GetOrCompute("rd_weight_" + bone.Name),
					BlendTimeId = MyStringId.GetOrCompute("rd_blend_time_" + bone.Name),
					BlendTimeMs = -1.0,
					StartingWeight = 0f,
					TargetWeight = 0f,
					PrevWeight = 0f,
					Layers = list.Where((MyAnimationStateMachine layer) => layer.BoneMask[bone.Index]).Select(delegate(MyAnimationStateMachine layer)
					{
						LayerData result = default(LayerData);
						result.LayerId = MyStringId.GetOrCompute("rd_weight_" + layer.Name);
						result.LayerBlendTimeId = MyStringId.GetOrCompute("rd_blend_time_" + layer.Name);
						return result;
					}).ToArray()
				};
			}
			m_defautlBlendTimeId = MyStringId.GetOrCompute("rd_default_blend_time");
			Initialized = true;
		}

		public void BlendWeight(ref float weight, MyCharacterBone bone, IMyVariableStorage<float> controllerVariables)
		{
			if (m_boneIndexToData.Length <= bone.Index)
			{
				return;
			}
			BoneData boneData = m_boneIndexToData[bone.Index];
			if (!controllerVariables.GetValue(m_defautlBlendTimeId, out m_defaultBlendTime))
			{
				m_defaultBlendTime = 2.5f;
			}
			if (!controllerVariables.GetValue(boneData.WeightId, out float value) || value < 0f)
			{
				value = -1f;
			}
			if (!controllerVariables.GetValue(boneData.BlendTimeId, out float value2) || value2 < 0f)
			{
				value2 = -1f;
			}
			if (value < 0f || value2 < 0f)
			{
				float num = float.MaxValue;
				float num2 = float.MaxValue;
				LayerData[] layers = boneData.Layers;
				for (int i = 0; i < layers.Length; i++)
				{
					LayerData layerData = layers[i];
					if (controllerVariables.GetValue(layerData.LayerId, out float value3))
					{
						num = Math.Min(num, value3);
					}
					if (controllerVariables.GetValue(layerData.LayerBlendTimeId, out float value4))
					{
						num2 = Math.Min(num2, value4);
					}
				}
				if (value < 0f)
				{
					if (num == float.MaxValue)
					{
						return;
					}
					value = num;
				}
				if (value2 < 0f)
				{
					value2 = ((num2 == float.MaxValue) ? m_defaultBlendTime : num2);
				}
			}
			double totalMilliseconds = TIMER.ElapsedTimeSpan.TotalMilliseconds;
			boneData.BlendTimeMs = value2 * 1000f;
			if (value != boneData.TargetWeight)
			{
				boneData.StartedMs = totalMilliseconds;
				boneData.StartingWeight = ((boneData.PrevWeight == -1f) ? weight : boneData.PrevWeight);
				boneData.TargetWeight = value;
			}
			double amount = MathHelper.Clamp((totalMilliseconds - boneData.StartedMs) / boneData.BlendTimeMs, 0.0, 1.0);
			weight = (float)MathHelper.Lerp(boneData.StartingWeight, boneData.TargetWeight, amount);
			boneData.PrevWeight = weight;
			m_boneIndexToData[bone.Index] = boneData;
		}

		public void ResetWeights()
		{
			if (m_boneIndexToData != null)
			{
				for (int i = 0; i < m_boneIndexToData.Length; i++)
				{
					m_boneIndexToData[i].PrevWeight = 0f;
					m_boneIndexToData[i].TargetWeight = 0f;
				}
			}
		}
	}
}
