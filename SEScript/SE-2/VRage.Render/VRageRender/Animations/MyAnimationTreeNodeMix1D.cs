using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Mixing between animation nodes on 1D axis.
	/// </summary>
	public class MyAnimationTreeNodeMix1D : MyAnimationTreeNode
	{
		public struct MyParameterNodeMapping
		{
			public float ParamValueBinding;

			public MyAnimationTreeNode Child;
		}

		private float? m_lastKnownParamValue;

		private int m_lastKnownFrameCounter = -1;

		public List<MyParameterNodeMapping> ChildMappings = new List<MyParameterNodeMapping>(4);

		public MyStringId ParameterName;

		public bool Circular;

		public float Sensitivity = 1f;

		public float MaxChange = float.PositiveInfinity;

		public override void Update(ref MyAnimationUpdateData data)
		{
			if (ChildMappings.Count == 0)
			{
				data.AddVisitedTreeNodesPathPoint(-1);
				return;
			}
			float num = ComputeParamValue(ref data);
			int num2 = -1;
			for (int num3 = ChildMappings.Count - 1; num3 >= 0; num3--)
			{
				if (ChildMappings[num3].ParamValueBinding <= num)
				{
					num2 = num3;
					break;
				}
			}
			if (num2 == -1)
			{
				if (ChildMappings[0].Child != null)
				{
					data.AddVisitedTreeNodesPathPoint(1);
					ChildMappings[0].Child.Update(ref data);
					PushLocalTimeToSlaves(0);
				}
				else
				{
					data.BonesResult = data.Controller.ResultBonesPool.Alloc();
				}
			}
			else if (num2 == ChildMappings.Count - 1)
			{
				if (ChildMappings[num2].Child != null)
				{
					data.AddVisitedTreeNodesPathPoint(ChildMappings.Count - 1 + 1);
					ChildMappings[num2].Child.Update(ref data);
					PushLocalTimeToSlaves(num2);
				}
				else
				{
					data.BonesResult = data.Controller.ResultBonesPool.Alloc();
				}
			}
			else
			{
				int num4 = num2 + 1;
				float num5 = ChildMappings[num4].ParamValueBinding - ChildMappings[num2].ParamValueBinding;
				float num6 = (num - ChildMappings[num2].ParamValueBinding) / num5;
				if (num6 > 0.5f)
				{
					num2++;
					num4--;
					num6 = 1f - num6;
				}
				if (num6 < 0.001f)
				{
					num6 = 0f;
				}
				else if (num6 > 0.999f)
				{
					num6 = 1f;
				}
				MyAnimationTreeNode child = ChildMappings[num2].Child;
				MyAnimationTreeNode child2 = ChildMappings[num4].Child;
				if (child != null && num6 < 1f)
				{
					data.AddVisitedTreeNodesPathPoint(num2 + 1);
					child.Update(ref data);
					PushLocalTimeToSlaves(num2);
				}
				else
				{
					data.BonesResult = data.Controller.ResultBonesPool.Alloc();
				}
				MyAnimationUpdateData data2 = data;
				if (child2 != null && num6 > 0f)
				{
					data2.DeltaTimeInSeconds = 0.0;
					data2.AddVisitedTreeNodesPathPoint(num4 + 1);
					child2.Update(ref data2);
					data.VisitedTreeNodesCounter = data2.VisitedTreeNodesCounter;
				}
				else
				{
					data2.BonesResult = data2.Controller.ResultBonesPool.Alloc();
				}
				for (int i = 0; i < data.BonesResult.Count; i++)
				{
					if (data.LayerBoneMask[i])
					{
						data.BonesResult[i].Rotation = Quaternion.Slerp(data.BonesResult[i].Rotation, data2.BonesResult[i].Rotation, num6);
						data.BonesResult[i].Translation = Vector3.Lerp(data.BonesResult[i].Translation, data2.BonesResult[i].Translation, num6);
					}
				}
				data.Controller.ResultBonesPool.Free(data2.BonesResult);
			}
			m_lastKnownFrameCounter = data.Controller.FrameCounter;
			data.AddVisitedTreeNodesPathPoint(-1);
		}

		private void PushLocalTimeToSlaves(int masterIndex)
		{
			float localTimeNormalized = (ChildMappings[masterIndex].Child != null) ? ChildMappings[masterIndex].Child.GetLocalTimeNormalized() : 0f;
			for (int i = 0; i < ChildMappings.Count; i++)
			{
				if (i != masterIndex && ChildMappings[i].Child != null)
				{
					ChildMappings[i].Child.SetLocalTimeNormalized(localTimeNormalized);
				}
			}
		}

		private float ComputeParamValue(ref MyAnimationUpdateData data)
		{
			float paramValueBinding = ChildMappings[0].ParamValueBinding;
			float paramValueBinding2 = ChildMappings[ChildMappings.Count - 1].ParamValueBinding;
			float num = 0.001f * (paramValueBinding2 - paramValueBinding);
			data.Controller.Variables.GetValue(ParameterName, out float value);
			if (m_lastKnownParamValue.HasValue && data.Controller.FrameCounter - m_lastKnownFrameCounter <= 1)
			{
				if (Circular)
				{
					float value2 = m_lastKnownParamValue.Value;
					float num2 = value - m_lastKnownParamValue.Value;
					num2 *= num2;
					float num3 = num2;
					float num4 = value - (m_lastKnownParamValue.Value + paramValueBinding2 - paramValueBinding);
					if (num4 * num4 < num2)
					{
						value2 = m_lastKnownParamValue.Value + paramValueBinding2 - paramValueBinding;
						num3 = num4 * num4;
					}
					float num5 = value - (m_lastKnownParamValue.Value - paramValueBinding2 + paramValueBinding);
					if (num5 * num5 < num3)
					{
						value2 = m_lastKnownParamValue.Value - paramValueBinding2 + paramValueBinding;
						num3 = num5 * num5;
					}
					float num6;
					for (num6 = ((num3 <= MaxChange * MaxChange) ? MathHelper.Lerp(value2, value, Sensitivity) : value); num6 < paramValueBinding; num6 += paramValueBinding2 - paramValueBinding)
					{
					}
					while (num6 > paramValueBinding2)
					{
						num6 -= paramValueBinding2 - paramValueBinding;
					}
					if ((m_lastKnownParamValue.Value - num6) * (m_lastKnownParamValue.Value - num6) > num * num)
					{
						m_lastKnownParamValue = num6;
					}
				}
				else
				{
					float num7 = value - m_lastKnownParamValue.Value;
					float num8 = (num7 * num7 <= MaxChange * MaxChange) ? MathHelper.Lerp(m_lastKnownParamValue.Value, value, Sensitivity) : value;
					if ((m_lastKnownParamValue.Value - num8) * (m_lastKnownParamValue.Value - num8) > num * num)
					{
						m_lastKnownParamValue = num8;
					}
				}
			}
			else
			{
				m_lastKnownParamValue = value;
			}
			return m_lastKnownParamValue.Value;
		}

		public override float GetLocalTimeNormalized()
		{
			foreach (MyParameterNodeMapping childMapping in ChildMappings)
			{
				if (childMapping.Child != null)
				{
					return childMapping.Child.GetLocalTimeNormalized();
				}
			}
			return 0f;
		}

		public override void SetLocalTimeNormalized(float normalizedTime)
		{
			foreach (MyParameterNodeMapping childMapping in ChildMappings)
			{
				if (childMapping.Child != null)
				{
					childMapping.Child.SetLocalTimeNormalized(normalizedTime);
				}
			}
		}
	}
}
