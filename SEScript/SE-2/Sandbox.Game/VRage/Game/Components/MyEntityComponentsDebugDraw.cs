using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Components
{
	[PreloadRequired]
	public class MyEntityComponentsDebugDraw
	{
		public static void DebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_ENTITY_COMPONENTS && MySector.MainCamera != null)
			{
				double num = 1.5;
				double num2 = num * 0.045;
				double scaleFactor = 0.5;
				Vector3D position = MySector.MainCamera.Position;
				Vector3D vector3D = MySector.MainCamera.WorldMatrix.Up;
				Vector3D right = MySector.MainCamera.WorldMatrix.Right;
				Vector3D vector3D2 = MySector.MainCamera.ForwardVector;
				BoundingSphereD boundingSphere = new BoundingSphereD(position, 5.0);
				List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				Vector3D value = Vector3D.Zero;
				Vector3D zero = Vector3D.Zero;
				MatrixD viewProjectionMatrix = MySector.MainCamera.ViewProjectionMatrix;
				Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
				float num3 = (float)safeGuiRectangle.Height / (float)safeGuiRectangle.Width;
				float num4 = 600f;
				float num5 = num4 * num3;
				Vector3D vector3D3 = position + 1.0 * vector3D2;
				Vector3D vector3D4 = Vector3D.Transform(vector3D3, viewProjectionMatrix);
				Vector3D vector3D5 = Vector3D.Transform(vector3D3 + Vector3D.Right * 0.10000000149011612, viewProjectionMatrix);
				Vector3D vector3D6 = Vector3D.Transform(vector3D3 + Vector3D.Up * 0.10000000149011612, viewProjectionMatrix);
				Vector3D vector3D7 = Vector3D.Transform(vector3D3 + Vector3D.Backward * 0.10000000149011612, viewProjectionMatrix);
				Vector2 value2 = new Vector2((float)vector3D4.X * num4, (float)vector3D4.Y * (0f - num5) * num3);
				Vector2 value3 = new Vector2((float)vector3D5.X * num4, (float)vector3D5.Y * (0f - num5) * num3) - value2;
				Vector2 value4 = new Vector2((float)vector3D6.X * num4, (float)vector3D6.Y * (0f - num5) * num3) - value2;
				Vector2 value5 = new Vector2((float)vector3D7.X * num4, (float)vector3D7.Y * (0f - num5) * num3) - value2;
				float num6 = 150f;
				Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(new Vector2(1f, 1f));
				_ = screenCoordinateFromNormalizedCoordinate + new Vector2(0f - num6, 0f);
				_ = screenCoordinateFromNormalizedCoordinate + new Vector2(0f, 0f - num6);
				Vector2 value6 = screenCoordinateFromNormalizedCoordinate + new Vector2(0f - num6, 0f - num6);
				Vector2 vector = (screenCoordinateFromNormalizedCoordinate + value6) * 0.5f;
				MyRenderProxy.DebugDrawLine2D(vector, vector + value3, Color.Red, Color.Red);
				MyRenderProxy.DebugDrawLine2D(vector, vector + value4, Color.Green, Color.Green);
				MyRenderProxy.DebugDrawLine2D(vector, vector + value5, Color.Blue, Color.Blue);
				MyRenderProxy.DebugDrawText2D(vector + value3, "World X", Color.Red, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawText2D(vector + value4, "World Y", Color.Green, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawText2D(vector + value5, "World Z", Color.Blue, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				MyComponentsDebugInputComponent.DetectedEntities.Clear();
				foreach (MyEntity item in entitiesInSphere)
				{
					if (item.PositionComp != null)
					{
						Vector3D position2 = item.PositionComp.GetPosition();
						Vector3D vector3D8 = position2 + vector3D * 0.10000000149011612;
						Vector3D vector3D9 = vector3D8 - right * scaleFactor;
						if (Vector3D.Dot(Vector3D.Normalize(position2 - position), vector3D2) < 0.9995)
						{
							Vector3D value7 = item.PositionComp.WorldMatrix.Right * 0.30000001192092896;
							Vector3D value8 = item.PositionComp.WorldMatrix.Up * 0.30000001192092896;
							Vector3D value9 = item.PositionComp.WorldMatrix.Backward * 0.30000001192092896;
							MyRenderProxy.DebugDrawSphere(position2, 0.01f, Color.White, 1f, depthRead: false);
							MyRenderProxy.DebugDrawArrow3D(position2, position2 + value7, Color.Red, Color.Red, depthRead: false, 0.1, "X");
							MyRenderProxy.DebugDrawArrow3D(position2, position2 + value8, Color.Green, Color.Green, depthRead: false, 0.1, "Y");
							MyRenderProxy.DebugDrawArrow3D(position2, position2 + value9, Color.Blue, Color.Blue, depthRead: false, 0.1, "Z");
						}
						else
						{
							if (Vector3D.Distance(position2, value) < 0.01)
							{
								zero += right * 0.30000001192092896;
								vector3D = -vector3D;
								vector3D8 = position2 + vector3D * 0.10000000149011612;
								vector3D9 = vector3D8 - right * scaleFactor;
							}
							value = position2;
							double val = Vector3D.Distance(vector3D9, position);
							double num7 = Math.Atan(num / Math.Max(val, 0.001));
							float num8 = 0f;
							Dictionary<Type, MyComponentBase>.ValueCollection.Enumerator enumerator2 = item.Components.GetEnumerator();
							MyComponentBase component = null;
							while (enumerator2.MoveNext())
							{
								component = enumerator2.Current;
								num8 += (float)GetComponentLines(component);
							}
							num8 += 1f;
							num8 -= (float)GetComponentLines(component);
							enumerator2.Dispose();
							Vector3D vector3D10 = vector3D9 + (num8 + 0.5f) * vector3D * num2;
							Vector3D worldCoord = vector3D9 + (num8 + 1f) * vector3D * num2 + 0.0099999997764825821 * right;
							MyRenderProxy.DebugDrawLine3D(position2, vector3D8, Color.White, Color.White, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector3D9, vector3D8, Color.White, Color.White, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector3D9, vector3D10, Color.White, Color.White, depthRead: false);
							MyRenderProxy.DebugDrawLine3D(vector3D10, vector3D10 + right * 1.0, Color.White, Color.White, depthRead: false);
							MyRenderProxy.DebugDrawText3D(worldCoord, item.GetType().ToString() + " - " + item.DisplayName, Color.Orange, (float)num7, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
							MyComponentsDebugInputComponent.DetectedEntities.Add(item);
							foreach (MyComponentBase component2 in item.Components)
							{
								worldCoord = vector3D9 + num8 * vector3D * num2;
								DebugDrawComponent(component2, worldCoord, right, vector3D, num2, (float)num7);
								MyEntityComponentBase myEntityComponentBase = component2 as MyEntityComponentBase;
								string text = (myEntityComponentBase == null) ? "" : myEntityComponentBase.ComponentTypeDebugString;
								MyRenderProxy.DebugDrawText3D(worldCoord - 0.019999999552965164 * right, text, Color.Yellow, (float)num7, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
								num8 -= (float)GetComponentLines(component2);
							}
						}
					}
				}
				entitiesInSphere.Clear();
			}
		}

		private static int GetComponentLines(MyComponentBase component, bool countAll = true)
		{
			int num = 1;
			if (component is IMyComponentAggregate)
			{
				int count = (component as IMyComponentAggregate).ChildList.Reader.Count;
				int num2 = 0;
				{
					foreach (MyComponentBase item in (component as IMyComponentAggregate).ChildList.Reader)
					{
						num2++;
						num = ((!(num2 < count || countAll)) ? (num + 1) : (num + GetComponentLines(item)));
					}
					return num;
				}
			}
			return num;
		}

		private static void DebugDrawComponent(MyComponentBase component, Vector3D origin, Vector3D rightVector, Vector3D upVector, double lineSize, float textSize)
		{
			Vector3D value = rightVector * 0.02500000037252903;
			Vector3D vector3D = origin + value * 3.5;
			Vector3D worldCoord = origin + 2.0 * value + rightVector * 0.014999999664723873;
			MyRenderProxy.DebugDrawLine3D(origin, origin + 2.0 * value, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, component.ToString(), Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			if (component is IMyComponentAggregate && (component as IMyComponentAggregate).ChildList.Reader.Count != 0)
			{
				int num = GetComponentLines(component, countAll: false) - 1;
				MyRenderProxy.DebugDrawLine3D(vector3D - 0.5 * lineSize * upVector, vector3D - (double)num * lineSize * upVector, Color.White, Color.White, depthRead: false);
				vector3D -= 1.0 * lineSize * upVector;
				foreach (MyComponentBase item in (component as IMyComponentAggregate).ChildList.Reader)
				{
					int componentLines = GetComponentLines(item);
					DebugDrawComponent(item, vector3D, rightVector, upVector, lineSize, textSize);
					vector3D -= (double)componentLines * lineSize * upVector;
				}
			}
		}
	}
}
