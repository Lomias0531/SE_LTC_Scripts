using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyStatControls
	{
		private class StatBinding
		{
			public IMyStatControl Control;

			public IMyHudStat Stat;

			public MyObjectBuilder_StatVisualStyle Style;

			public bool LastVisibleConditionCheckResult;
		}

		private readonly List<StatBinding> m_bindings = new List<StatBinding>();

		private readonly Dictionary<VisualStyleCategory, float> m_alphaMultipliersByCategory = new Dictionary<VisualStyleCategory, float>();

		private MyObjectBuilder_StatControls m_objectBuilder;

		private float m_resolutionScaleRatio;

		private double m_lastDrawTimeMs;

		private IMyHudStat m_showStatesStat;

		private float m_uiScaleFactor;

		private Vector2 m_position;

		public float ChildrenScaleFactor => m_uiScaleFactor;

		public Vector2 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				OnPositionChanged(m_position, value);
				m_position = value;
			}
		}

		public MyStatControls(MyObjectBuilder_StatControls ob, float uiScale = 1f)
		{
			m_objectBuilder = ob;
			m_uiScaleFactor = uiScale;
			if (m_objectBuilder.StatStyles != null)
			{
				MyObjectBuilder_StatVisualStyle[] statStyles = m_objectBuilder.StatStyles;
				foreach (MyObjectBuilder_StatVisualStyle myObjectBuilder_StatVisualStyle in statStyles)
				{
					if (myObjectBuilder_StatVisualStyle.StatId != MyStringHash.NullOrEmpty)
					{
						IMyHudStat stat = MyHud.Stats.GetStat(myObjectBuilder_StatVisualStyle.StatId);
						if (stat != null)
						{
							AddControl(stat, myObjectBuilder_StatVisualStyle);
						}
					}
					else
					{
						AddControl(null, myObjectBuilder_StatVisualStyle);
					}
				}
			}
			if (ob.VisibleCondition != null)
			{
				InitConditions(ob.VisibleCondition);
			}
			m_showStatesStat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_show_states"));
			m_lastDrawTimeMs = MySession.Static.ElapsedGameTime.TotalMilliseconds;
			foreach (object enumValue in typeof(VisualStyleCategory).GetEnumValues())
			{
				m_alphaMultipliersByCategory[(VisualStyleCategory)enumValue] = 1f;
			}
		}

		private void InitStatControl(IMyStatControl control, IMyHudStat stat, MyObjectBuilder_StatVisualStyle style)
		{
			if (stat != null)
			{
				control.StatMaxValue = stat.MaxValue;
				control.StatMinValue = stat.MinValue;
				control.StatCurrent = stat.CurrentValue;
				control.StatString = stat.GetValueString();
			}
			if (style.Blink != null)
			{
				control.BlinkBehavior.Blink = style.Blink.Blink;
				control.BlinkBehavior.IntervalMs = style.Blink.IntervalMs;
				control.BlinkBehavior.MinAlpha = style.Blink.MinAlpha;
				control.BlinkBehavior.MaxAlpha = style.Blink.MaxAlpha;
				if (style.Blink.ColorMask.HasValue)
				{
					control.BlinkBehavior.ColorMask = style.Blink.ColorMask;
				}
			}
			if (style.FadeInTimeMs.HasValue)
			{
				control.FadeInTimeMs = style.FadeInTimeMs.Value;
			}
			if (style.FadeOutTimeMs.HasValue)
			{
				control.FadeOutTimeMs = style.FadeOutTimeMs.Value;
			}
			if (style.MaxOnScreenTimeMs.HasValue)
			{
				control.MaxOnScreenTimeMs = style.MaxOnScreenTimeMs.Value;
			}
			if (style.BlinkCondition != null)
			{
				InitConditions(style.BlinkCondition);
			}
			if (style.VisibleCondition != null)
			{
				InitConditions(style.VisibleCondition);
			}
			if (style.Category.HasValue)
			{
				control.Category = style.Category.Value;
			}
			else
			{
				style.Category = VisualStyleCategory.None;
			}
		}

		private static void InitConditions(ConditionBase conditionBase)
		{
			StatCondition statCondition = conditionBase as StatCondition;
			if (statCondition != null)
			{
				IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
				statCondition.SetStat(stat);
				return;
			}
			Condition condition = conditionBase as Condition;
			if (condition != null)
			{
				ConditionBase[] terms = condition.Terms;
				for (int i = 0; i < terms.Length; i++)
				{
					InitConditions(terms[i]);
				}
			}
		}

		private void AddControl(IMyHudStat stat, MyObjectBuilder_StatVisualStyle style)
		{
			IMyStatControl myStatControl = null;
			if (style is MyObjectBuilder_CircularProgressBarStatVisualStyle)
			{
				myStatControl = InitCircularProgressBar((MyObjectBuilder_CircularProgressBarStatVisualStyle)style);
			}
			else if (style is MyObjectBuilder_ProgressBarStatVisualStyle)
			{
				myStatControl = InitProgressBar((MyObjectBuilder_ProgressBarStatVisualStyle)style);
			}
			else if (style is MyObjectBuilder_TextStatVisualStyle)
			{
				myStatControl = InitText((MyObjectBuilder_TextStatVisualStyle)style);
			}
			else if (style is MyObjectBuilder_ImageStatVisualStyle)
			{
				myStatControl = InitImage((MyObjectBuilder_ImageStatVisualStyle)style);
			}
			if (myStatControl != null)
			{
				InitStatControl(myStatControl, stat, style);
				m_bindings.Add(new StatBinding
				{
					Control = myStatControl,
					Style = style,
					Stat = stat
				});
			}
		}

		private IMyStatControl InitCircularProgressBar(MyObjectBuilder_CircularProgressBarStatVisualStyle style)
		{
			MyObjectBuilder_GuiTexture texture = null;
			if (!MyGuiTextures.Static.TryGetTexture(style.SegmentTexture, out texture))
			{
				return null;
			}
			MyObjectBuilder_GuiTexture texture2 = null;
			if (style.BackgroudTexture.HasValue)
			{
				MyGuiTextures.Static.TryGetTexture(style.BackgroudTexture.Value, out texture2);
			}
			MyObjectBuilder_GuiTexture texture3 = null;
			if (style.FirstSegmentTexture.HasValue)
			{
				MyGuiTextures.Static.TryGetTexture(style.FirstSegmentTexture.Value, out texture3);
			}
			MyObjectBuilder_GuiTexture texture4 = null;
			if (style.LastSegmentTexture.HasValue)
			{
				MyGuiTextures.Static.TryGetTexture(style.LastSegmentTexture.Value, out texture4);
			}
			MyStatControlCircularProgressBar myStatControlCircularProgressBar = new MyStatControlCircularProgressBar(this, texture, texture2, texture3, texture4)
			{
				Position = Position + style.OffsetPx * m_uiScaleFactor,
				Size = style.SizePx * m_uiScaleFactor,
				SegmentOrigin = ((style.SegmentOrigin * m_uiScaleFactor) ?? (style.SegmentSizePx * m_uiScaleFactor / 2f)),
				SegmentSize = style.SegmentSizePx * m_uiScaleFactor
			};
			if (style.AngleOffset.HasValue)
			{
				myStatControlCircularProgressBar.TextureRotationOffset = style.AngleOffset.Value;
			}
			if (style.SpacingAngle.HasValue)
			{
				myStatControlCircularProgressBar.TextureRotationAngle = style.SpacingAngle.Value;
			}
			if (style.Animate.HasValue)
			{
				myStatControlCircularProgressBar.Animate = style.Animate.Value;
			}
			if (style.AnimatedSegmentColorMask.HasValue)
			{
				myStatControlCircularProgressBar.AnimatedSegmentColorMask = style.AnimatedSegmentColorMask.Value;
			}
			if (style.FullSegmentColorMask.HasValue)
			{
				myStatControlCircularProgressBar.FullSegmentColorMask = style.FullSegmentColorMask.Value;
			}
			if (style.EmptySegmentColorMask.HasValue)
			{
				myStatControlCircularProgressBar.EmptySegmentColorMask = style.EmptySegmentColorMask.Value;
			}
			if (style.AnimationDelayMs.HasValue)
			{
				myStatControlCircularProgressBar.AnimationDelay = style.AnimationDelayMs.Value;
			}
			if (style.AnimationSegmentDelayMs.HasValue)
			{
				myStatControlCircularProgressBar.SegmentAnimationMs = style.AnimationSegmentDelayMs.Value;
			}
			if (style.NumberOfSegments.HasValue)
			{
				myStatControlCircularProgressBar.NumberOfSegments = style.NumberOfSegments.Value;
			}
			if (style.ShowEmptySegments.HasValue)
			{
				myStatControlCircularProgressBar.ShowEmptySegments = style.ShowEmptySegments.Value;
			}
			return myStatControlCircularProgressBar;
		}

		private IMyStatControl InitProgressBar(MyObjectBuilder_ProgressBarStatVisualStyle style)
		{
			if (style.NineTiledStyle.HasValue)
			{
				if (!MyGuiTextures.Static.TryGetCompositeTexture(style.NineTiledStyle.Value.Texture, out MyObjectBuilder_CompositeTexture texture))
				{
					return null;
				}
				MyStatControlProgressBar myStatControlProgressBar = new MyStatControlProgressBar(this, texture)
				{
					Position = Position + style.OffsetPx * m_uiScaleFactor,
					Size = style.SizePx * m_uiScaleFactor
				};
				if (style.NineTiledStyle.Value.ColorMask.HasValue)
				{
					myStatControlProgressBar.ColorMask = style.NineTiledStyle.Value.ColorMask.Value;
				}
				if (style.Inverted.HasValue)
				{
					myStatControlProgressBar.Inverted = style.Inverted.Value;
				}
				return myStatControlProgressBar;
			}
			if (style.SimpleStyle.HasValue)
			{
				if (!MyGuiTextures.Static.TryGetTexture(style.SimpleStyle.Value.BackgroundTexture, out MyObjectBuilder_GuiTexture texture2) || !MyGuiTextures.Static.TryGetTexture(style.SimpleStyle.Value.ProgressTexture, out MyObjectBuilder_GuiTexture texture3))
				{
					return null;
				}
				MyStatControlProgressBar myStatControlProgressBar2 = new MyStatControlProgressBar(this, texture2, texture3, style.SimpleStyle.Value.ProgressTextureOffsetPx, style.SimpleStyle.Value.BackgroundColorMask, style.SimpleStyle.Value.ProgressColorMask)
				{
					Position = Position + style.OffsetPx * m_uiScaleFactor,
					Size = style.SizePx * m_uiScaleFactor
				};
				if (style.Inverted.HasValue)
				{
					myStatControlProgressBar2.Inverted = style.Inverted.Value;
				}
				return myStatControlProgressBar2;
			}
			return null;
		}

		private IMyStatControl InitText(MyObjectBuilder_TextStatVisualStyle style)
		{
			MyStatControlText myStatControlText = new MyStatControlText(this, style.Text)
			{
				Position = Position + style.OffsetPx * m_uiScaleFactor,
				Size = style.SizePx * m_uiScaleFactor,
				Font = style.Font,
				Scale = style.Scale * m_uiScaleFactor
			};
			if (style.ColorMask.HasValue)
			{
				myStatControlText.TextColorMask = style.ColorMask.Value;
			}
			if (style.TextAlign.HasValue)
			{
				myStatControlText.TextAlign = style.TextAlign.Value;
			}
			return myStatControlText;
		}

		private IMyStatControl InitImage(MyObjectBuilder_ImageStatVisualStyle style)
		{
			if (!MyGuiTextures.Static.TryGetTexture(style.Texture, out MyObjectBuilder_GuiTexture texture))
			{
				return null;
			}
			MyStatControlImage myStatControlImage = new MyStatControlImage(this)
			{
				Size = style.SizePx * m_uiScaleFactor,
				Position = Position + style.OffsetPx * m_uiScaleFactor
			};
			myStatControlImage.Texture = texture;
			if (style.ColorMask.HasValue)
			{
				myStatControlImage.ColorMask = style.ColorMask.Value;
			}
			return myStatControlImage;
		}

		public void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (m_objectBuilder.VisibleCondition == null || m_objectBuilder.VisibleCondition.Eval())
			{
				double num = MySession.Static.ElapsedGameTime.TotalMilliseconds - m_lastDrawTimeMs;
				m_lastDrawTimeMs = MySession.Static.ElapsedGameTime.TotalMilliseconds;
				foreach (StatBinding binding in m_bindings)
				{
					IMyStatControl control = binding.Control;
					if (binding.Stat != null)
					{
						control.StatCurrent = binding.Stat.CurrentValue;
						control.StatString = binding.Stat.GetValueString();
						control.StatMaxValue = binding.Stat.MaxValue;
						control.StatMinValue = binding.Stat.MinValue;
					}
					if (binding.Style.BlinkCondition != null)
					{
						control.BlinkBehavior.Blink = binding.Style.BlinkCondition.Eval();
					}
					bool flag = true;
					if (binding.Style.VisibleCondition != null)
					{
						flag = binding.Style.VisibleCondition.Eval();
					}
					binding.Control.SpentInStateTimeMs += (uint)num;
					float val = 1f;
					switch (binding.Control.State)
					{
					case MyStatControlState.FadingOut:
						if (!flag || binding.LastVisibleConditionCheckResult)
						{
							if (binding.Control.SpentInStateTimeMs > binding.Control.FadeOutTimeMs)
							{
								binding.Control.State = MyStatControlState.Invisible;
								binding.Control.SpentInStateTimeMs = 0u;
							}
							else
							{
								val = 1f - (float)binding.Control.SpentInStateTimeMs / (float)binding.Control.FadeOutTimeMs;
							}
							break;
						}
						binding.Control.State = MyStatControlState.FadingIn;
						binding.Control.SpentInStateTimeMs = binding.Control.MaxOnScreenTimeMs - binding.Control.SpentInStateTimeMs;
						goto case MyStatControlState.FadingIn;
					case MyStatControlState.FadingIn:
						if (flag)
						{
							if (binding.Control.SpentInStateTimeMs > binding.Control.FadeInTimeMs)
							{
								binding.Control.State = MyStatControlState.Visible;
								binding.Control.SpentInStateTimeMs = 0u;
							}
							else
							{
								val = (float)binding.Control.SpentInStateTimeMs / (float)binding.Control.FadeInTimeMs;
							}
							break;
						}
						binding.Control.State = MyStatControlState.FadingOut;
						binding.Control.SpentInStateTimeMs = binding.Control.MaxOnScreenTimeMs - binding.Control.SpentInStateTimeMs;
						goto case MyStatControlState.FadingOut;
					case MyStatControlState.Visible:
						if ((!flag || (binding.Control.MaxOnScreenTimeMs != 0 && binding.Control.MaxOnScreenTimeMs < binding.Control.SpentInStateTimeMs)) && !(m_showStatesStat.CurrentValue > 0.5f))
						{
							binding.Control.State = MyStatControlState.FadingOut;
							binding.Control.SpentInStateTimeMs = 0u;
						}
						break;
					case MyStatControlState.Invisible:
						if (flag && (!binding.LastVisibleConditionCheckResult || !(m_showStatesStat.CurrentValue < 0.5f)))
						{
							binding.Control.State = MyStatControlState.FadingIn;
							binding.Control.SpentInStateTimeMs = 0u;
							val = 0f;
						}
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					if ((binding.Control.State & (MyStatControlState.FadingOut | MyStatControlState.FadingIn | MyStatControlState.Visible)) != 0)
					{
						val = Math.Min(transitionAlpha, val);
						binding.Control.Draw(val * m_alphaMultipliersByCategory[binding.Control.Category]);
					}
					binding.LastVisibleConditionCheckResult = flag;
				}
			}
		}

		public void RegisterAlphaMultiplier(VisualStyleCategory category, float multiplier)
		{
			m_alphaMultipliersByCategory[category] = multiplier;
		}

		[Conditional("DEBUG")]
		private void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_HUD)
			{
				Vector2 pointTo = Position + new Vector2(50f, 0f);
				Vector2 pointTo2 = Position + new Vector2(0f, 50f);
				MyRenderProxy.DebugDrawLine2D(Position, pointTo, Color.Green, Color.Green);
				MyRenderProxy.DebugDrawLine2D(Position, pointTo2, Color.Green, Color.Green);
				foreach (StatBinding binding in m_bindings)
				{
					Vector2 position = binding.Control.Position;
					Vector2 position2 = binding.Control.Position;
					position2.X += binding.Control.Size.X;
					Vector2 vector = binding.Control.Position + binding.Control.Size;
					Vector2 position3 = binding.Control.Position;
					position3.Y += binding.Control.Size.Y;
					MyRenderProxy.DebugDrawLine2D(position, position2, Color.Red, Color.Red);
					MyRenderProxy.DebugDrawLine2D(position2, vector, Color.Red, Color.Red);
					MyRenderProxy.DebugDrawLine2D(vector, position3, Color.Red, Color.Red);
					MyRenderProxy.DebugDrawLine2D(position3, position, Color.Red, Color.Red);
				}
			}
		}

		protected void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
		{
			Vector2 vector = newPosition - oldPosition;
			foreach (StatBinding binding in m_bindings)
			{
				binding.Control.Position += vector;
			}
		}
	}
}
