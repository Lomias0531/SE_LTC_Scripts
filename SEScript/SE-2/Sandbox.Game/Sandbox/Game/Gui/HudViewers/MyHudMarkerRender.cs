using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Generics;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI.HudViewers
{
	public class MyHudMarkerRender : MyHudMarkerRenderBase
	{
		public enum SignalMode
		{
			DefaultMode,
			FullDisplay,
			NoNames,
			Off,
			MaxSignalModes
		}

		private class PointOfInterest
		{
			public enum PointOfInterestState
			{
				NonDirectional,
				Directional
			}

			public enum PointOfInterestType
			{
				Unknown,
				Target,
				Group,
				Ore,
				Hack,
				UnknownEntity,
				Character,
				SmallEntity,
				LargeEntity,
				StaticEntity,
				GPS,
				ButtonMarker,
				Objective,
				Scenario,
				ContractGPS
			}

			public const double ClusterAngle = 10.0;

			public const int MaxTextLength = 64;

			public const double ClusterNearDistance = 3500.0;

			public const double ClusterScaleDistance = 20000.0;

			public const double MinimumTargetRange = 2000.0;

			public const double OreDistance = 200.0;

			private const double AngleConversion = Math.PI / 360.0;

			public Color DefaultColor = new Color(117, 201, 241);

			public List<PointOfInterest> m_group = new List<PointOfInterest>(10);

			private bool m_alwaysVisible;

			public Vector3D WorldPosition
			{
				get;
				private set;
			}

			public PointOfInterestType POIType
			{
				get;
				private set;
			}

			public MyRelationsBetweenPlayerAndBlock Relationship
			{
				get;
				private set;
			}

			public MyEntity Entity
			{
				get;
				private set;
			}

			public StringBuilder Text
			{
				get;
				private set;
			}

			public double Distance
			{
				get;
				private set;
			}

			public double DistanceToCam
			{
				get;
				private set;
			}

			public string ContainerRemainingTime
			{
				get;
				set;
			}

			public bool AlwaysVisible
			{
				get
				{
					if (POIType == PointOfInterestType.Ore && Distance < 200.0)
					{
						return true;
					}
					return m_alwaysVisible;
				}
				set
				{
					m_alwaysVisible = value;
				}
			}

			public bool AllowsCluster
			{
				get
				{
					if (AlwaysVisible)
					{
						return false;
					}
					if (POIType == PointOfInterestType.Target)
					{
						return false;
					}
					if (POIType == PointOfInterestType.Ore && Distance < 200.0)
					{
						return false;
					}
					return true;
				}
			}

			public PointOfInterest()
			{
				WorldPosition = Vector3D.Zero;
				POIType = PointOfInterestType.Unknown;
				Relationship = MyRelationsBetweenPlayerAndBlock.Owner;
				Text = new StringBuilder(64, 64);
			}

			public override string ToString()
			{
				return string.Concat(POIType.ToString(), ": ", Text, " (", Distance, ")");
			}

			public void Reset()
			{
				WorldPosition = Vector3D.Zero;
				POIType = PointOfInterestType.Unknown;
				Relationship = MyRelationsBetweenPlayerAndBlock.Owner;
				Entity = null;
				Text.Clear();
				m_group.Clear();
				Distance = 0.0;
				DistanceToCam = 0.0;
				AlwaysVisible = false;
				ContainerRemainingTime = null;
			}

			public void SetState(Vector3D position, PointOfInterestType type, MyRelationsBetweenPlayerAndBlock relationship)
			{
				WorldPosition = position;
				POIType = type;
				Relationship = relationship;
				Distance = (position - GetDistanceMeasuringMatrix().Translation).Length();
				DistanceToCam = (WorldPosition - CameraMatrix.Translation).Length();
			}

			public void SetEntity(MyEntity entity)
			{
				Entity = entity;
			}

			public void SetText(StringBuilder text)
			{
				Text.Clear();
				if (text != null)
				{
					Text.AppendSubstring(text, 0, Math.Min(text.Length, 64));
				}
			}

			public void SetText(string text)
			{
				Text.Clear();
				if (!string.IsNullOrWhiteSpace(text))
				{
					Text.Append(text, 0, Math.Min(text.Length, 64));
				}
			}

			public bool AddPOI(PointOfInterest poi)
			{
				if (POIType != PointOfInterestType.Group)
				{
					return false;
				}
				Vector3D worldPosition = WorldPosition;
				worldPosition *= (double)m_group.Count;
				m_group.Add(poi);
				Text.Clear();
				Text.Append(m_group.Count);
				switch (GetGroupRelation())
				{
				case MyRelationsBetweenPlayerAndBlock.Owner:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Own));
					break;
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Friendly));
					break;
				case MyRelationsBetweenPlayerAndBlock.Neutral:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Neutral));
					break;
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Enemy));
					break;
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
					Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Mixed));
					break;
				}
				worldPosition += poi.WorldPosition;
				WorldPosition = worldPosition / m_group.Count;
				Distance = (WorldPosition - GetDistanceMeasuringMatrix().Translation).Length();
				DistanceToCam = (WorldPosition - CameraMatrix.Translation).Length();
				if (poi.Relationship > Relationship)
				{
					Relationship = poi.Relationship;
				}
				return true;
			}

			public bool IsPOINearby(PointOfInterest poi, Vector3D cameraPosition, double angle = 10.0)
			{
				Vector3D value = 0.5 * (WorldPosition - poi.WorldPosition);
				double num = value.LengthSquared();
				double num2 = (cameraPosition - (poi.WorldPosition + value)).Length();
				double num3 = Math.Sin(angle * (Math.PI / 360.0)) * num2;
				double num4 = num3 * num3;
				return num <= num4;
			}

			public void GetColorAndFontForRelationship(MyRelationsBetweenPlayerAndBlock relationship, out Color color, out Color fontColor, out string font)
			{
				color = Color.White;
				fontColor = Color.White;
				font = "White";
				switch (relationship)
				{
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
				case MyRelationsBetweenPlayerAndBlock.Neutral:
					break;
				case MyRelationsBetweenPlayerAndBlock.Owner:
					color = new Color(117, 201, 241);
					fontColor = new Color(117, 201, 241);
					font = "Blue";
					break;
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
				case MyRelationsBetweenPlayerAndBlock.Friends:
					color = new Color(101, 178, 90);
					font = "Green";
					break;
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					color = new Color(227, 62, 63);
					font = "Red";
					break;
				}
			}

			public void GetPOIColorAndFontInformation(out Color poiColor, out Color fontColor, out string font)
			{
				poiColor = Color.White;
				fontColor = Color.White;
				font = "White";
				switch (POIType)
				{
				default:
					GetColorAndFontForRelationship(Relationship, out poiColor, out fontColor, out font);
					break;
				case PointOfInterestType.Ore:
					poiColor = Color.Khaki;
					font = "White";
					fontColor = Color.Khaki;
					break;
				case PointOfInterestType.Unknown:
					poiColor = Color.White;
					font = "White";
					fontColor = Color.White;
					break;
				case PointOfInterestType.Group:
				{
					bool flag = true;
					PointOfInterestType pointOfInterestType = PointOfInterestType.Unknown;
					if (m_group.Count > 0)
					{
						m_group[0].GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
						pointOfInterestType = m_group[0].POIType;
					}
					for (int i = 1; i < m_group.Count; i++)
					{
						if (m_group[i].POIType != pointOfInterestType)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						MyRelationsBetweenPlayerAndBlock groupRelation = GetGroupRelation();
						GetColorAndFontForRelationship(groupRelation, out poiColor, out fontColor, out font);
					}
					break;
				}
				case PointOfInterestType.GPS:
				case PointOfInterestType.ContractGPS:
					poiColor = DefaultColor;
					fontColor = DefaultColor;
					font = "Blue";
					break;
				case PointOfInterestType.Objective:
					poiColor = DefaultColor * 1.3f;
					fontColor = DefaultColor * 1.3f;
					font = "Blue";
					break;
				case PointOfInterestType.Scenario:
					poiColor = Color.DarkOrange;
					fontColor = Color.DarkOrange;
					font = "White";
					break;
				}
			}

			private MyRelationsBetweenPlayerAndBlock GetGroupRelation()
			{
				if (m_group == null || m_group.Count == 0)
				{
					return MyRelationsBetweenPlayerAndBlock.NoOwnership;
				}
				MyRelationsBetweenPlayerAndBlock myRelationsBetweenPlayerAndBlock = m_group[0].Relationship;
				for (int i = 1; i < m_group.Count; i++)
				{
					if (m_group[i].Relationship == myRelationsBetweenPlayerAndBlock)
					{
						continue;
					}
					if (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Owner && m_group[i].Relationship == MyRelationsBetweenPlayerAndBlock.FactionShare)
					{
						myRelationsBetweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.FactionShare;
						continue;
					}
					if (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.FactionShare && m_group[i].Relationship == MyRelationsBetweenPlayerAndBlock.Owner)
					{
						myRelationsBetweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.FactionShare;
						continue;
					}
					return MyRelationsBetweenPlayerAndBlock.NoOwnership;
				}
				if (myRelationsBetweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.NoOwnership)
				{
					return MyRelationsBetweenPlayerAndBlock.Neutral;
				}
				return myRelationsBetweenPlayerAndBlock;
			}

			public void Draw(MyHudMarkerRender renderer, float alphaMultiplierMarker = 1f, float alphaMultiplierText = 1f, float scale = 1f, bool drawBox = true)
			{
				Vector2 projectedPoint2D = Vector2.Zero;
				bool isBehind = false;
				if (!TryComputeScreenPoint(WorldPosition, out projectedPoint2D, out isBehind))
				{
					return;
				}
				Vector2 vector = new Vector2(MyGuiManager.GetSafeFullscreenRectangle().Width, MyGuiManager.GetSafeFullscreenRectangle().Height);
				Vector2 hudSize = MyGuiManager.GetHudSize();
				Vector2 hudSizeHalf = MyGuiManager.GetHudSizeHalf();
				float num = vector.Y / 1080f;
				projectedPoint2D *= hudSize;
				Color poiColor = Color.White;
				Color fontColor = Color.White;
				string font = "White";
				GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
				Vector2 vector2 = projectedPoint2D - hudSizeHalf;
				Vector3D vector3D = Vector3D.Transform(WorldPosition, MySector.MainCamera.ViewMatrix);
				float num2 = 0.04f;
				if (projectedPoint2D.X < num2 || projectedPoint2D.X > hudSize.X - num2 || projectedPoint2D.Y < num2 || projectedPoint2D.Y > hudSize.Y - num2 || vector3D.Z > 0.0)
				{
					if (POIType == PointOfInterestType.Target)
					{
						return;
					}
					Vector2 value = Vector2.Normalize(vector2);
					projectedPoint2D = hudSizeHalf + hudSizeHalf * value * 0.77f;
					vector2 = projectedPoint2D - hudSizeHalf;
					if (!(vector2.LengthSquared() > 9.99999944E-11f))
					{
						vector2 = new Vector2(1f, 0f);
					}
					else
					{
						vector2.Normalize();
					}
					float num3 = 0.0053336f;
					num3 /= num;
					num3 /= num;
					renderer.AddTexturedQuad(MyHudTexturesEnum.DirectionIndicator, projectedPoint2D, vector2, poiColor, num3, num3);
					projectedPoint2D -= vector2 * 0.006667f * 2f;
				}
				else
				{
					float num4 = scale * 0.006667f / num;
					num4 /= num;
					if (POIType == PointOfInterestType.Target)
					{
						renderer.AddTexturedQuad(MyHudTexturesEnum.TargetTurret, projectedPoint2D, -Vector2.UnitY, Color.White, num4, num4);
						return;
					}
					if (drawBox)
					{
						renderer.AddTexturedQuad(MyHudTexturesEnum.Target_neutral, projectedPoint2D, -Vector2.UnitY, poiColor, num4, num4);
					}
				}
				float num5 = 0.03f;
				float num6 = 0.07f;
				float num7 = 0.15f;
				int num8 = 0;
				float num9 = 1f;
				float num10 = 1f;
				float num11 = vector2.Length();
				if (num11 <= num5)
				{
					num9 = 1f;
					num10 = 1f;
					num8 = 0;
				}
				else if (num11 > num5 && num11 < num6)
				{
					float num12 = num7 - num5;
					num9 = 1f - (num11 - num5) / num12;
					num9 *= num9;
					num12 = num6 - num5;
					num10 = 1f - (num11 - num5) / num12;
					num10 *= num10;
					num8 = 1;
				}
				else if (num11 >= num6 && num11 < num7)
				{
					float num13 = num7 - num5;
					num9 = 1f - (num11 - num5) / num13;
					num9 *= num9;
					num13 = num7 - num6;
					num10 = 1f - (num11 - num6) / num13;
					num10 *= num10;
					num8 = 2;
				}
				else
				{
					num9 = 0f;
					num10 = 0f;
					num8 = 2;
				}
				float value2 = (num11 - 0.2f) / 0.5f;
				value2 = MathHelper.Clamp(value2, 0f, 1f);
				num9 = MyMath.Clamp(num9, 0f, 1f);
				if (m_disableFading || SignalDisplayMode == SignalMode.FullDisplay || AlwaysVisible)
				{
					num9 = 1f;
					num10 = 1f;
					value2 = 1f;
					num8 = 0;
				}
				Vector2 vector3 = new Vector2(0f, scale * num * 24f / (float)MyGuiManager.GetFullscreenRectangle().Width);
				if ((SignalDisplayMode != SignalMode.NoNames || POIType == PointOfInterestType.ButtonMarker || m_disableFading || AlwaysVisible) && num9 > float.Epsilon && Text.Length > 0)
				{
					MyHudText myHudText = renderer.m_hudScreen.AllocateText();
					if (myHudText != null)
					{
						fontColor.A = (byte)(255f * alphaMultiplierText * num9);
						myHudText.Start(font, projectedPoint2D - vector3, fontColor, 0.7f / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						myHudText.Append(Text);
					}
				}
				MyHudText myHudText2 = null;
				if (POIType != PointOfInterestType.Group)
				{
					byte a = poiColor.A;
					poiColor.A = (byte)(255f * alphaMultiplierMarker * value2);
					DrawIcon(renderer, POIType, Relationship, projectedPoint2D, poiColor, scale);
					poiColor.A = a;
					myHudText2 = renderer.m_hudScreen.AllocateText();
					if (myHudText2 != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						AppendDistance(stringBuilder, Distance);
						fontColor.A = (byte)(alphaMultiplierText * 255f);
						myHudText2.Start(font, projectedPoint2D + vector3 * (0.7f + 0.3f * num9), fontColor, (0.5f + 0.2f * num9) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						myHudText2.Append(stringBuilder);
					}
					if (!string.IsNullOrEmpty(ContainerRemainingTime))
					{
						MyHudText myHudText3 = renderer.m_hudScreen.AllocateText();
						myHudText3.Start(font, projectedPoint2D + vector3 * (1.6f + 0.3f * num9), fontColor, (0.5f + 0.2f * num9) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						myHudText3.Append(ContainerRemainingTime);
					}
					return;
				}
				Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> significantGroupPOIs = GetSignificantGroupPOIs();
				Vector2[] array = new Vector2[5]
				{
					new Vector2(-6f, -4f),
					new Vector2(6f, -4f),
					new Vector2(-6f, 4f),
					new Vector2(6f, 4f),
					new Vector2(0f, 12f)
				};
				Vector2[] array2 = new Vector2[5]
				{
					new Vector2(16f, -4f),
					new Vector2(16f, 4f),
					new Vector2(16f, 12f),
					new Vector2(16f, 20f),
					new Vector2(16f, 28f)
				};
				for (int i = 0; i < array.Length; i++)
				{
					float num14 = (num8 < 2) ? 1f : num10;
					float y = array[i].Y;
					array[i].X = (array[i].X + 22f * num14) / (float)MyGuiManager.GetFullscreenRectangle().Width;
					array[i].Y = y / 1080f / num;
					if (MyVideoSettingsManager.IsTripleHead())
					{
						array[i].X /= 0.33f;
					}
					if (array[i].Y <= float.Epsilon)
					{
						array[i].Y = y / 1080f;
					}
					y = array2[i].Y;
					array2[i].X = array2[i].X / (float)MyGuiManager.GetFullscreenRectangle().Width / num;
					array2[i].Y = y / 1080f / num;
					if (MyVideoSettingsManager.IsTripleHead())
					{
						array2[i].X /= 0.33f;
					}
					if (array2[i].Y <= float.Epsilon)
					{
						array2[i].Y = y / 1080f;
					}
				}
				int num15 = 0;
				if (significantGroupPOIs.Count > 1)
				{
					MyRelationsBetweenPlayerAndBlock[] array3 = new MyRelationsBetweenPlayerAndBlock[4]
					{
						MyRelationsBetweenPlayerAndBlock.Owner,
						MyRelationsBetweenPlayerAndBlock.FactionShare,
						MyRelationsBetweenPlayerAndBlock.Neutral,
						MyRelationsBetweenPlayerAndBlock.Enemies
					};
					foreach (MyRelationsBetweenPlayerAndBlock myRelationsBetweenPlayerAndBlock in array3)
					{
						if (!significantGroupPOIs.ContainsKey(myRelationsBetweenPlayerAndBlock))
						{
							continue;
						}
						List<PointOfInterest> list = significantGroupPOIs[myRelationsBetweenPlayerAndBlock];
						if (list.Count == 0)
						{
							continue;
						}
						PointOfInterest pointOfInterest = list[0];
						if (pointOfInterest == null)
						{
							continue;
						}
						if (pointOfInterest.POIType == PointOfInterestType.ContractGPS)
						{
							pointOfInterest.GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
						}
						else
						{
							GetColorAndFontForRelationship(myRelationsBetweenPlayerAndBlock, out poiColor, out fontColor, out font);
						}
						float amount = (num8 == 0) ? 1f : num10;
						if (num8 >= 2)
						{
							amount = 0f;
						}
						Vector2 value3 = Vector2.Lerp(array[num15], array2[num15], amount);
						string iconForRelationship = GetIconForRelationship(myRelationsBetweenPlayerAndBlock);
						poiColor.A = (byte)(alphaMultiplierMarker * (float)(int)poiColor.A);
						DrawIcon(renderer, iconForRelationship, projectedPoint2D + value3, poiColor, 0.75f / num);
						if (IsPoiAtHighAlert(pointOfInterest))
						{
							Color white = Color.White;
							white.A = (byte)(alphaMultiplierMarker * 255f);
							DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", projectedPoint2D + value3, white, 0.75f / num);
						}
						if ((SignalDisplayMode != SignalMode.NoNames || m_disableFading || AlwaysVisible) && pointOfInterest.Text.Length > 0)
						{
							MyHudText myHudText4 = renderer.m_hudScreen.AllocateText();
							if (myHudText4 != null)
							{
								float num16 = 1f;
								if (num8 == 1)
								{
									num16 = num10;
								}
								else if (num8 > 1)
								{
									num16 = 0f;
								}
								fontColor.A = (byte)(255f * alphaMultiplierText * num16);
								Vector2 value4 = new Vector2(8f / (float)MyGuiManager.GetFullscreenRectangle().Width, 0f);
								value4.X /= num;
								myHudText4.Start(font, projectedPoint2D + value3 + value4, fontColor, 0.55f / num, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
								myHudText4.Append(pointOfInterest.Text);
							}
						}
						num15++;
					}
				}
				else
				{
					foreach (KeyValuePair<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> item in significantGroupPOIs)
					{
						MyRelationsBetweenPlayerAndBlock key = item.Key;
						if (significantGroupPOIs.ContainsKey(key))
						{
							List<PointOfInterest> value5 = item.Value;
							for (int k = 0; k < 4 && k < value5.Count; k++)
							{
								PointOfInterest pointOfInterest2 = value5[k];
								if (pointOfInterest2 != null)
								{
									if (pointOfInterest2.POIType == PointOfInterestType.Scenario || pointOfInterest2.POIType == PointOfInterestType.ContractGPS || pointOfInterest2.POIType == PointOfInterestType.Ore)
									{
										pointOfInterest2.GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
									}
									else
									{
										GetColorAndFontForRelationship(key, out poiColor, out fontColor, out font);
									}
									float amount2 = (num8 == 0) ? 1f : num10;
									if (num8 >= 2)
									{
										amount2 = 0f;
									}
									Vector2 value6 = Vector2.Lerp(array[num15], array2[num15], amount2);
									string centerIconSprite = (pointOfInterest2.POIType != PointOfInterestType.Scenario) ? GetIconForRelationship(key) : "Textures\\HUD\\marker_scenario.dds";
									poiColor.A = (byte)(alphaMultiplierMarker * (float)(int)poiColor.A);
									DrawIcon(renderer, centerIconSprite, projectedPoint2D + value6, poiColor, 0.75f / num);
									if (ShouldDrawHighAlertMark(pointOfInterest2))
									{
										Color white2 = Color.White;
										white2.A = (byte)(alphaMultiplierMarker * 255f);
										DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", projectedPoint2D + value6, white2, 0.75f / num);
									}
									if ((SignalDisplayMode != SignalMode.NoNames || m_disableFading || AlwaysVisible) && pointOfInterest2.Text.Length > 0)
									{
										MyHudText myHudText5 = renderer.m_hudScreen.AllocateText();
										if (myHudText5 != null)
										{
											float num17 = 1f;
											if (num8 == 1)
											{
												num17 = num10;
											}
											else if (num8 > 1)
											{
												num17 = 0f;
											}
											fontColor.A = (byte)(255f * alphaMultiplierText * num17);
											Vector2 value7 = new Vector2(8f / (float)MyGuiManager.GetFullscreenRectangle().Width, 0f);
											value7.X /= num;
											myHudText5.Start(font, projectedPoint2D + value6 + value7, fontColor, 0.55f / num, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
											myHudText5.Append(pointOfInterest2.Text);
										}
									}
									num15++;
								}
							}
						}
					}
				}
				GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
				float amount3 = (num8 == 0) ? 1f : num10;
				if (num8 >= 2)
				{
					amount3 = 0f;
				}
				Vector2 value8 = Vector2.Lerp(array[4], array2[num15], amount3);
				Vector2 value9 = Vector2.Lerp(Vector2.Zero, new Vector2(0.0222222228f / num, 0.00370370364f / num), amount3);
				myHudText2 = renderer.m_hudScreen.AllocateText();
				if (myHudText2 != null)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					AppendDistance(stringBuilder2, Distance);
					fontColor.A = (byte)(alphaMultiplierText * 255f);
					myHudText2.Start(font, projectedPoint2D + value8 + value9, fontColor, (0.5f + 0.2f * num9) / num, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					myHudText2.Append(stringBuilder2);
				}
			}

			private Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> GetSignificantGroupPOIs()
			{
				Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>> dictionary = new Dictionary<MyRelationsBetweenPlayerAndBlock, List<PointOfInterest>>();
				if (m_group == null || m_group.Count == 0)
				{
					return dictionary;
				}
				bool flag = true;
				MyRelationsBetweenPlayerAndBlock relationship = m_group[0].Relationship;
				for (int i = 1; i < m_group.Count; i++)
				{
					if (m_group[i].Relationship != relationship)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					m_group.Sort(ComparePointOfInterest);
					dictionary[relationship] = new List<PointOfInterest>();
					for (int num = m_group.Count - 1; num >= 0; num--)
					{
						dictionary[relationship].Add(m_group[num]);
						if (dictionary[relationship].Count >= 4)
						{
							break;
						}
					}
				}
				else
				{
					for (int j = 0; j < m_group.Count; j++)
					{
						PointOfInterest pointOfInterest = m_group[j];
						relationship = pointOfInterest.Relationship;
						if (relationship == MyRelationsBetweenPlayerAndBlock.NoOwnership)
						{
							relationship = MyRelationsBetweenPlayerAndBlock.Neutral;
						}
						if (dictionary.ContainsKey(relationship))
						{
							if (ComparePointOfInterest(pointOfInterest, dictionary[relationship][0]) > 0)
							{
								dictionary[relationship].Clear();
								dictionary[relationship].Add(pointOfInterest);
							}
						}
						else
						{
							dictionary[relationship] = new List<PointOfInterest>();
							dictionary[relationship].Add(pointOfInterest);
						}
					}
				}
				return dictionary;
			}

			private bool IsRelationHostile(MyRelationsBetweenPlayerAndBlock relationshipA, MyRelationsBetweenPlayerAndBlock relationshipB)
			{
				if (relationshipA == MyRelationsBetweenPlayerAndBlock.Owner || relationshipA == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					return relationshipB == MyRelationsBetweenPlayerAndBlock.Enemies;
				}
				if (relationshipB == MyRelationsBetweenPlayerAndBlock.Owner || relationshipB == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					return relationshipA == MyRelationsBetweenPlayerAndBlock.Enemies;
				}
				return false;
			}

			private bool IsPoiAtHighAlert(PointOfInterest poi)
			{
				if (poi.Relationship == MyRelationsBetweenPlayerAndBlock.Neutral)
				{
					return false;
				}
				if (poi.POIType == PointOfInterestType.Scenario)
				{
					return true;
				}
				foreach (PointOfInterest item in m_group)
				{
					if (IsRelationHostile(poi.Relationship, item.Relationship) && ((Vector3)(item.WorldPosition - poi.WorldPosition)).LengthSquared() < 1000000f)
					{
						return true;
					}
				}
				return false;
			}

			private bool ShouldDrawHighAlertMark(PointOfInterest poi)
			{
				if (poi.POIType != PointOfInterestType.Scenario)
				{
					return IsPoiAtHighAlert(poi);
				}
				return false;
			}

			private bool IsGrid()
			{
				if (POIType != PointOfInterestType.SmallEntity && POIType != PointOfInterestType.LargeEntity)
				{
					return POIType == PointOfInterestType.StaticEntity;
				}
				return true;
			}

			private static void DrawIcon(MyHudMarkerRender renderer, PointOfInterestType poiType, MyRelationsBetweenPlayerAndBlock relationship, Vector2 screenPosition, Color markerColor, float sizeScale = 1f)
			{
				MyHudTexturesEnum myHudTexturesEnum = MyHudTexturesEnum.corner;
				string empty = string.Empty;
				Vector2 vector = new Vector2(12f, 12f);
				switch (poiType)
				{
				default:
					return;
				case PointOfInterestType.Hack:
					myHudTexturesEnum = MyHudTexturesEnum.hit_confirmation;
					break;
				case PointOfInterestType.Target:
					myHudTexturesEnum = MyHudTexturesEnum.TargetTurret;
					break;
				case PointOfInterestType.Ore:
					myHudTexturesEnum = MyHudTexturesEnum.HudOre;
					markerColor = Color.Khaki;
					break;
				case PointOfInterestType.Unknown:
				case PointOfInterestType.UnknownEntity:
				case PointOfInterestType.Character:
				case PointOfInterestType.SmallEntity:
				case PointOfInterestType.LargeEntity:
				case PointOfInterestType.StaticEntity:
				{
					string iconForRelationship = GetIconForRelationship(relationship);
					DrawIcon(renderer, iconForRelationship, screenPosition, markerColor, sizeScale);
					return;
				}
				case PointOfInterestType.Scenario:
				{
					string centerIconSprite2 = "Textures\\HUD\\marker_scenario.dds";
					DrawIcon(renderer, centerIconSprite2, screenPosition, markerColor, sizeScale);
					return;
				}
				case PointOfInterestType.GPS:
				case PointOfInterestType.Objective:
				{
					string centerIconSprite = "Textures\\HUD\\marker_gps.dds";
					DrawIcon(renderer, centerIconSprite, screenPosition, markerColor, sizeScale);
					return;
				}
				}
				if (!string.IsNullOrWhiteSpace(empty))
				{
					vector *= sizeScale;
					renderer.AddTexturedQuad(empty, screenPosition, -Vector2.UnitY, markerColor, vector.X, vector.Y);
				}
				else
				{
					float num = 0.0053336f * sizeScale;
					renderer.AddTexturedQuad(myHudTexturesEnum, screenPosition, -Vector2.UnitY, markerColor, num, num);
				}
			}

			public static string GetIconForRelationship(MyRelationsBetweenPlayerAndBlock relationship)
			{
				string result = string.Empty;
				switch (relationship)
				{
				case MyRelationsBetweenPlayerAndBlock.Owner:
					result = "Textures\\HUD\\marker_self.dds";
					break;
				case MyRelationsBetweenPlayerAndBlock.FactionShare:
				case MyRelationsBetweenPlayerAndBlock.Friends:
					result = "Textures\\HUD\\marker_friendly.dds";
					break;
				case MyRelationsBetweenPlayerAndBlock.NoOwnership:
				case MyRelationsBetweenPlayerAndBlock.Neutral:
					result = "Textures\\HUD\\marker_neutral.dds";
					break;
				case MyRelationsBetweenPlayerAndBlock.Enemies:
					result = "Textures\\HUD\\marker_enemy.dds";
					break;
				}
				return result;
			}

			private static void DrawIcon(MyHudMarkerRender renderer, string centerIconSprite, Vector2 screenPosition, Color markerColor, float sizeScale = 1f)
			{
				Vector2 vector = new Vector2(8f, 8f);
				vector *= sizeScale;
				renderer.AddTexturedQuad(centerIconSprite, screenPosition, -Vector2.UnitY, markerColor, vector.X, vector.Y);
			}

			public static bool TryComputeScreenPoint(Vector3D worldPosition, out Vector2 projectedPoint2D, out bool isBehind)
			{
				Vector3D position = Vector3D.Transform(worldPosition, MySector.MainCamera.ViewMatrix);
				Vector4D vector4D = Vector4D.Transform(position, MySector.MainCamera.ProjectionMatrix);
				if (position.Z > 0.0)
				{
					vector4D.X *= -1.0;
					vector4D.Y *= -1.0;
				}
				if (vector4D.W == 0.0)
				{
					projectedPoint2D = Vector2.Zero;
					isBehind = false;
					return false;
				}
				projectedPoint2D = new Vector2((float)(vector4D.X / vector4D.W / 2.0) + 0.5f, (float)((0.0 - vector4D.Y) / vector4D.W) / 2f + 0.5f);
				if (MyVideoSettingsManager.IsTripleHead())
				{
					projectedPoint2D.X = (projectedPoint2D.X - 0.333333343f) / 0.333333343f;
				}
				Vector3D vector = worldPosition - CameraMatrix.Translation;
				vector.Normalize();
				double num = Vector3D.Dot(MySector.MainCamera.ForwardVector, vector);
				isBehind = (num < 0.0);
				return true;
			}

			private int ComparePointOfInterest(PointOfInterest poiA, PointOfInterest poiB)
			{
				bool flag = IsPoiAtHighAlert(poiA);
				bool value = IsPoiAtHighAlert(poiB);
				int num = flag.CompareTo(value);
				if (num != 0)
				{
					return num;
				}
				if (poiA.POIType >= PointOfInterestType.UnknownEntity && poiB.POIType >= PointOfInterestType.UnknownEntity)
				{
					int num2 = poiA.POIType.CompareTo(poiB.POIType);
					if (num2 != 0)
					{
						return num2;
					}
				}
				if (poiA.IsGrid() && poiB.IsGrid())
				{
					MyCubeBlock myCubeBlock = poiA.Entity as MyCubeBlock;
					MyCubeBlock myCubeBlock2 = poiB.Entity as MyCubeBlock;
					if (myCubeBlock != null && myCubeBlock2 != null)
					{
						int num3 = myCubeBlock.CubeGrid.BlocksCount.CompareTo(myCubeBlock2.CubeGrid.BlocksCount);
						if (num3 != 0)
						{
							return num3;
						}
					}
				}
				return poiB.Distance.CompareTo(poiA.Distance);
			}
		}

		private static float m_friendAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private static bool m_disableFading = false;

		private bool m_disableFadingToggle;

		private static MyHudNotification m_signalModeNotification = null;

		private static float m_ownerAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private static float m_enemyAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;

		private MyDynamicObjectPool<PointOfInterest> m_pointOfInterestPool = new MyDynamicObjectPool<PointOfInterest>(32);

		private List<PointOfInterest> m_pointsOfInterest = new List<PointOfInterest>();

		public static SignalMode SignalDisplayMode
		{
			get;
			private set;
		}

		public static float FriendAntennaRange
		{
			get
			{
				return NormalizeLog(m_friendAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
			}
			set
			{
				m_friendAntennaRange = Denormalize(value);
			}
		}

		public static float OwnerAntennaRange
		{
			get
			{
				return NormalizeLog(m_ownerAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
			}
			set
			{
				m_ownerAntennaRange = Denormalize(value);
			}
		}

		public static float EnemyAntennaRange
		{
			get
			{
				return NormalizeLog(m_enemyAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
			}
			set
			{
				m_enemyAntennaRange = Denormalize(value);
			}
		}

		private static MatrixD? ControlledEntityMatrix
		{
			get
			{
				if (MySession.Static.ControlledEntity != null)
				{
					return MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrix;
				}
				return null;
			}
		}

		private static MatrixD? LocalCharacterMatrix
		{
			get
			{
				if (MySession.Static.LocalCharacter != null)
				{
					return MySession.Static.LocalCharacter.WorldMatrix;
				}
				return null;
			}
		}

		private static MatrixD CameraMatrix => MySector.MainCamera.WorldMatrix;

		public override void Update()
		{
			MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			m_disableFading = MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED);
			_ = MySession.Static.ControlledEntity;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.TOGGLE_SIGNALS) && !MyInput.Static.IsAnyCtrlKeyPressed() && MyScreenManager.FocusedControl == null)
			{
				ChangeSignalMode();
			}
		}

		public static void ChangeSignalMode()
		{
			if (!MyHud.IsHudMinimal && !MyHud.MinimalHud)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				SignalDisplayMode++;
				if (SignalDisplayMode >= SignalMode.MaxSignalModes)
				{
					SignalDisplayMode = SignalMode.DefaultMode;
				}
				if (m_signalModeNotification != null)
				{
					MyHud.Notifications.Remove(m_signalModeNotification);
					m_signalModeNotification = null;
				}
				switch (SignalDisplayMode)
				{
				case SignalMode.DefaultMode:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_DefaultMode, 1000);
					break;
				case SignalMode.FullDisplay:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_FullDisplay, 1000);
					break;
				case SignalMode.NoNames:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_NoNames, 1000);
					break;
				case SignalMode.Off:
					m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_Off, 1000);
					break;
				}
				if (m_signalModeNotification != null)
				{
					MyHud.Notifications.Add(m_signalModeNotification);
				}
			}
		}

		public MyHudMarkerRender(MyGuiScreenHudBase hudScreen)
			: base(hudScreen)
		{
		}

		public override void DrawLocationMarkers(MyHudLocationMarkers locationMarkers)
		{
			if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Identity != null)
			{
				float num = m_ownerAntennaRange * m_ownerAntennaRange;
				float num2 = m_friendAntennaRange * m_friendAntennaRange;
				float num3 = m_enemyAntennaRange * m_enemyAntennaRange;
				foreach (MyHudEntityParams value in locationMarkers.MarkerEntities.Values)
				{
					if (value.ShouldDraw == null || value.ShouldDraw())
					{
						double num4 = (value.Position - GetDistanceMeasuringMatrix().Translation).LengthSquared();
						MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(value.Owner, MySession.Static.LocalHumanPlayer.Identity.IdentityId, value.Share);
						switch (relationPlayerBlock)
						{
						case MyRelationsBetweenPlayerAndBlock.Owner:
							if (num4 > (double)num)
							{
								continue;
							}
							break;
						case MyRelationsBetweenPlayerAndBlock.NoOwnership:
						case MyRelationsBetweenPlayerAndBlock.FactionShare:
						case MyRelationsBetweenPlayerAndBlock.Friends:
							if (num4 > (double)num2)
							{
								continue;
							}
							break;
						case MyRelationsBetweenPlayerAndBlock.Neutral:
						case MyRelationsBetweenPlayerAndBlock.Enemies:
							if (num4 > (double)num3)
							{
								continue;
							}
							break;
						}
						MyEntity myEntity = value.Entity as MyEntity;
						if (myEntity != null)
						{
							AddEntity(myEntity, relationPlayerBlock, value.Text, IsScenarioObjective(myEntity));
						}
						else
						{
							AddProxyEntity(value.Position, relationPlayerBlock, value.Text);
						}
					}
				}
				m_hudScreen.DrawTexts();
			}
		}

		private static bool IsScenarioObjective(MyEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (entity.Name != null && entity.Name.Length >= 13 && entity.Name.Substring(0, 13).Equals("MissionStart_"))
			{
				return true;
			}
			return false;
		}

		public static MatrixD GetDistanceMeasuringMatrix()
		{
			MatrixD? controlledEntityMatrix = ControlledEntityMatrix;
			if (!controlledEntityMatrix.HasValue || (!MySession.Static.CameraOnCharacter && MySession.Static.IsCameraUserControlledSpectator()))
			{
				return CameraMatrix;
			}
			MatrixD? localCharacterMatrix = LocalCharacterMatrix;
			if (MySession.Static.CameraOnCharacter && localCharacterMatrix.HasValue)
			{
				return localCharacterMatrix.Value;
			}
			return controlledEntityMatrix.Value;
		}

		public void AddPOI(Vector3D worldPosition, StringBuilder name, MyRelationsBetweenPlayerAndBlock relationship)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.GPS, relationship);
				pointOfInterest.SetText(name);
			}
		}

		public void AddEntity(MyEntity entity, MyRelationsBetweenPlayerAndBlock relationship, StringBuilder entityName, bool IsScenarioMarker = false)
		{
			if (SignalDisplayMode == SignalMode.Off || entity == null)
			{
				return;
			}
			Vector3D position = entity.PositionComp.GetPosition();
			PointOfInterest.PointOfInterestType type = PointOfInterest.PointOfInterestType.UnknownEntity;
			if (entity is MyCharacter)
			{
				if (entity == MySession.Static.LocalCharacter)
				{
					return;
				}
				type = PointOfInterest.PointOfInterestType.Character;
				position += entity.WorldMatrix.Up * 1.2999999523162842;
			}
			else
			{
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null && myCubeBlock.CubeGrid != null)
				{
					type = ((myCubeBlock.CubeGrid.GridSizeEnum != MyCubeSize.Small) ? (myCubeBlock.CubeGrid.IsStatic ? PointOfInterest.PointOfInterestType.StaticEntity : PointOfInterest.PointOfInterestType.LargeEntity) : PointOfInterest.PointOfInterestType.SmallEntity);
				}
			}
			PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
			m_pointsOfInterest.Add(pointOfInterest);
			pointOfInterest.Reset();
			if (IsScenarioMarker)
			{
				type = PointOfInterest.PointOfInterestType.Scenario;
			}
			pointOfInterest.SetState(position, type, relationship);
			pointOfInterest.SetEntity(entity);
			pointOfInterest.SetText(entityName);
		}

		public void AddGPS(MyGps gps)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.DefaultColor = gps.GPSColor;
				pointOfInterest.Reset();
				pointOfInterest.SetState(gps.Coords, (gps.ContractId != 0L) ? PointOfInterest.PointOfInterestType.ContractGPS : (gps.IsObjective ? PointOfInterest.PointOfInterestType.Objective : PointOfInterest.PointOfInterestType.GPS), MyRelationsBetweenPlayerAndBlock.Owner);
				if (string.IsNullOrEmpty(gps.DisplayName))
				{
					pointOfInterest.SetText(gps.Name);
				}
				else
				{
					pointOfInterest.SetText(gps.DisplayName);
				}
				pointOfInterest.AlwaysVisible = gps.AlwaysVisible;
				pointOfInterest.ContainerRemainingTime = gps.ContainerRemainingTime;
			}
		}

		public void AddButtonMarker(Vector3D worldPosition, string name)
		{
			PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
			pointOfInterest.Reset();
			pointOfInterest.AlwaysVisible = true;
			pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.ButtonMarker, MyRelationsBetweenPlayerAndBlock.Owner);
			pointOfInterest.SetText(name);
			m_pointsOfInterest.Add(pointOfInterest);
		}

		public void AddOre(Vector3D worldPosition, string name)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.Ore, MyRelationsBetweenPlayerAndBlock.NoOwnership);
				pointOfInterest.SetText(name);
			}
		}

		public void AddTarget(Vector3D worldPosition)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.Target, MyRelationsBetweenPlayerAndBlock.Enemies);
			}
		}

		public void AddHacking(Vector3D worldPosition, StringBuilder name)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.Hack, MyRelationsBetweenPlayerAndBlock.Owner);
				pointOfInterest.SetText(name);
			}
		}

		public void AddProxyEntity(Vector3D worldPosition, MyRelationsBetweenPlayerAndBlock relationship, StringBuilder name)
		{
			if (SignalDisplayMode != SignalMode.Off)
			{
				PointOfInterest pointOfInterest = m_pointOfInterestPool.Allocate();
				m_pointsOfInterest.Add(pointOfInterest);
				pointOfInterest.Reset();
				pointOfInterest.SetState(worldPosition, PointOfInterest.PointOfInterestType.UnknownEntity, relationship);
				pointOfInterest.SetText(name);
			}
		}

		public static void AppendDistance(StringBuilder stringBuilder, double distance)
		{
			if (stringBuilder == null)
			{
				return;
			}
			distance = Math.Abs(distance);
			if (distance > 9.460730473E+15)
			{
				stringBuilder.AppendDecimal(Math.Round(distance / 9.460730473E+15, 2), 2);
				stringBuilder.Append("ly");
			}
			else if (distance > 299792458.00013667)
			{
				stringBuilder.AppendDecimal(Math.Round(distance / 299792458.00013667, 2), 2);
				stringBuilder.Append("ls");
			}
			else if (distance > 1000.0)
			{
				if (distance > 1000000.0)
				{
					stringBuilder.AppendDecimal(Math.Round(distance / 1000.0, 2), 1);
				}
				else
				{
					stringBuilder.AppendDecimal(Math.Round(distance / 1000.0, 2), 2);
				}
				stringBuilder.Append("km");
			}
			else
			{
				stringBuilder.AppendDecimal(Math.Round(distance, 2), 1);
				stringBuilder.Append("m");
			}
		}

		public override void Draw()
		{
			Vector3D position = MySector.MainCamera.Position;
			List<PointOfInterest> list = new List<PointOfInterest>();
			if (SignalDisplayMode == SignalMode.FullDisplay)
			{
				list.AddRange(m_pointsOfInterest);
			}
			else
			{
				for (int i = 0; i < m_pointsOfInterest.Count; i++)
				{
					PointOfInterest pointOfInterest = m_pointsOfInterest[i];
					PointOfInterest pointOfInterest2 = null;
					if (pointOfInterest.AlwaysVisible)
					{
						list.Add(pointOfInterest);
						continue;
					}
					if (pointOfInterest.AllowsCluster)
					{
						int num = i + 1;
						while (num < m_pointsOfInterest.Count)
						{
							PointOfInterest pointOfInterest3 = m_pointsOfInterest[num];
							if (pointOfInterest3 == pointOfInterest)
							{
								num++;
							}
							else if (!pointOfInterest3.AllowsCluster)
							{
								num++;
							}
							else if (pointOfInterest.IsPOINearby(pointOfInterest3, position))
							{
								if (pointOfInterest2 == null)
								{
									pointOfInterest2 = m_pointOfInterestPool.Allocate();
									pointOfInterest2.Reset();
									pointOfInterest2.SetState(Vector3D.Zero, PointOfInterest.PointOfInterestType.Group, MyRelationsBetweenPlayerAndBlock.NoOwnership);
									pointOfInterest2.AddPOI(pointOfInterest);
								}
								pointOfInterest2.AddPOI(pointOfInterest3);
								m_pointsOfInterest.RemoveAt(num);
							}
							else
							{
								num++;
							}
						}
					}
					else if (pointOfInterest.POIType == PointOfInterest.PointOfInterestType.Target && (position - pointOfInterest.WorldPosition).Length() > 2000.0)
					{
						continue;
					}
					if (pointOfInterest2 != null)
					{
						list.Add(pointOfInterest2);
					}
					else
					{
						list.Add(pointOfInterest);
					}
				}
			}
			list.Sort((PointOfInterest a, PointOfInterest b) => b.DistanceToCam.CompareTo(a.DistanceToCam));
			List<Vector2D> list2 = new List<Vector2D>(list.Count);
			List<Vector2> list3 = new List<Vector2>(list.Count);
			List<bool> list4 = new List<bool>(list.Count);
			if (!m_disableFading && SignalDisplayMode != SignalMode.FullDisplay)
			{
				for (int num2 = list.Count - 1; num2 >= 0; num2--)
				{
					Vector3D worldPos = list[num2].WorldPosition;
					worldPos = MySector.MainCamera.WorldToScreen(ref worldPos);
					Vector2D vector2D = new Vector2D(worldPos.X, worldPos.Y);
					bool flag = Vector3D.Dot(list[num2].WorldPosition - CameraMatrix.Translation, CameraMatrix.Forward) < 0.0;
					float num3 = float.MaxValue;
					for (int j = 0; j < list2.Count; j++)
					{
						if (flag == list4[j])
						{
							float num4 = (float)(list2[j] - vector2D).LengthSquared();
							if (num4 < num3)
							{
								num3 = num4;
							}
						}
					}
					float x;
					float y;
					if (num3 > 0.022f)
					{
						x = 1f;
						y = 1f;
					}
					else if (num3 > 0.011f)
					{
						x = 81.81f * num3 - 0.8f;
						y = 90f * num3 - 0.98f;
					}
					else
					{
						x = 0.1f;
						y = 0.01f;
					}
					list2.Add(vector2D);
					list3.Add(new Vector2(x, y));
					list4.Add(flag);
				}
			}
			if (m_disableFading || SignalDisplayMode == SignalMode.FullDisplay)
			{
				for (int k = 0; k < list.Count; k++)
				{
					_ = list.Count;
					list[k].Draw(this, 1f, 1f, (list[k].POIType != PointOfInterest.PointOfInterestType.Objective) ? 1 : 2, list[k].POIType != PointOfInterest.PointOfInterestType.Objective);
				}
			}
			else
			{
				for (int l = 0; l < list.Count; l++)
				{
					int index = list.Count - l - 1;
					list[l].Draw(this, list3[index].X, list3[index].Y, (list[l].POIType != PointOfInterest.PointOfInterestType.Objective) ? 1 : 2, list[l].POIType != PointOfInterest.PointOfInterestType.Objective);
				}
			}
			foreach (PointOfInterest item in m_pointsOfInterest)
			{
				item.Reset();
				m_pointOfInterestPool.Deallocate(item);
			}
			m_pointsOfInterest.Clear();
		}

		public static float Normalize(float value)
		{
			return NormalizeLog(value, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
		}

		public static float Denormalize(float value)
		{
			return DenormalizeLog(value, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
		}

		private static float NormalizeLog(float f, float min, float max)
		{
			return MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0f, 1f);
		}

		private static float DenormalizeLog(float f, float min, float max)
		{
			return MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max);
		}
	}
}
