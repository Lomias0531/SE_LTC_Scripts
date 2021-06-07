using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Input;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
	public class MyAlexDebugInputComponent : MyDebugComponent
	{
		public struct LineInfo
		{
			public Vector3 From;

			public Vector3 To;

			public Color ColorFrom;

			public Color ColorTo;

			public bool DepthRead;

			public LineInfo(Vector3 from, Vector3 to, Color colorFrom, Color colorTo, bool depthRead)
			{
				From = from;
				To = to;
				ColorFrom = colorFrom;
				ColorTo = colorTo;
				DepthRead = depthRead;
			}

			public LineInfo(Vector3 from, Vector3 to, Color colorFrom, bool depthRead)
				: this(from, to, colorFrom, colorFrom, depthRead)
			{
			}
		}

		private static bool ShowDebugDrawTests;

		private List<LineInfo> m_lines = new List<LineInfo>();

		public static MyAlexDebugInputComponent Static
		{
			get;
			private set;
		}

		public MyAlexDebugInputComponent()
		{
			Static = this;
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Clear lines", delegate
			{
				Clear();
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "SuitOxygenLevel = 0.35f", delegate
			{
				MySession.Static.LocalCharacter.OxygenComponent.SuitOxygenLevel = 0.35f;
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "SuitOxygenLevel = 0f", delegate
			{
				MySession.Static.LocalCharacter.OxygenComponent.SuitOxygenLevel = 0f;
				return true;
			});
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "SuitOxygenLevel -= 0.05f", delegate
			{
				MySession.Static.LocalCharacter.OxygenComponent.SuitOxygenLevel -= 0.05f;
				return true;
			});
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Deplete battery", delegate
			{
				MySession.Static.LocalCharacter.SuitBattery.DebugDepleteBattery();
				return true;
			});
			AddShortcut(MyKeys.Add, newPress: true, control: true, shift: false, alt: false, () => "SunRotationIntervalMinutes = 1", delegate
			{
				MySession.Static.Settings.SunRotationIntervalMinutes = 1f;
				return true;
			});
			AddShortcut(MyKeys.Subtract, newPress: true, control: true, shift: false, alt: false, () => "SunRotationIntervalMinutes = 1", delegate
			{
				MySession.Static.Settings.SunRotationIntervalMinutes = -1f;
				return true;
			});
			AddShortcut(MyKeys.Space, newPress: true, control: true, shift: false, alt: false, () => "Enable sun rotation: " + (MySession.Static != null && MySession.Static.Settings.EnableSunRotation), delegate
			{
				if (MySession.Static == null)
				{
					return false;
				}
				MySession.Static.Settings.EnableSunRotation = !MySession.Static.Settings.EnableSunRotation;
				return true;
			});
			AddShortcut(MyKeys.D, newPress: true, control: true, shift: false, alt: false, () => "Show debug draw tests: " + ShowDebugDrawTests, delegate
			{
				ShowDebugDrawTests = !ShowDebugDrawTests;
				return true;
			});
		}

		public void AddDebugLine(LineInfo line)
		{
			m_lines.Add(line);
		}

		public override string GetName()
		{
			return "Alex";
		}

		private void ModifyOxygenBottleAmount(float amount)
		{
			foreach (MyPhysicalInventoryItem item in MySession.Static.LocalCharacter.GetInventory().GetItems())
			{
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
				{
					MyOxygenContainerDefinition myOxygenContainerDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_GasContainerObject) as MyOxygenContainerDefinition;
					if ((!(amount > 0f) || myObjectBuilder_GasContainerObject.GasLevel != 1f) && (!(amount < 0f) || myObjectBuilder_GasContainerObject.GasLevel != 0f))
					{
						myObjectBuilder_GasContainerObject.GasLevel += amount / myOxygenContainerDefinition.Capacity;
						if (myObjectBuilder_GasContainerObject.GasLevel < 0f)
						{
							myObjectBuilder_GasContainerObject.GasLevel = 0f;
						}
						if (myObjectBuilder_GasContainerObject.GasLevel > 1f)
						{
							myObjectBuilder_GasContainerObject.GasLevel = 1f;
						}
					}
				}
			}
		}

		public void Clear()
		{
			m_lines.Clear();
		}

		public override void Draw()
		{
			base.Draw();
			foreach (LineInfo line in m_lines)
			{
				MyRenderProxy.DebugDrawLine3D(line.From, line.To, line.ColorFrom, line.ColorTo, line.DepthRead);
			}
			if (ShowDebugDrawTests)
			{
				Vector3D vector3D = new Vector3D(1000000000.0, 1000000000.0, 1000000000.0);
				MyRenderProxy.DebugDrawLine3D(vector3D, vector3D + Vector3D.Up, Color.Red, Color.Blue, depthRead: true);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawLine3D(vector3D, vector3D + Vector3D.Up, Color.Red, Color.Blue, depthRead: false);
				MyRenderProxy.DebugDrawLine2D(new Vector2(10f, 10f), new Vector2(50f, 50f), Color.Red, Color.Blue);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawPoint(vector3D, Color.White, depthRead: true);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawPoint(vector3D, Color.White, depthRead: false);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawSphere(vector3D, 0.5f, Color.White);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawAABB(new BoundingBoxD(vector3D - Vector3D.One * 0.5, vector3D + Vector3D.One * 0.5), Color.White);
				vector3D += Vector3D.Left;
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawAxis(MatrixD.CreateFromTransformScale(Quaternion.Identity, vector3D, Vector3D.One * 0.5), 1f, depthRead: true);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(vector3D, Vector3D.One * 0.5, Quaternion.Identity), Color.White, 1f, depthRead: true, smooth: false);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawCylinder(MatrixD.CreateFromTransformScale(Quaternion.Identity, vector3D, Vector3D.One * 0.5), Color.White, 1f, depthRead: true, smooth: true);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawTriangle(vector3D, vector3D + Vector3D.Up, vector3D + Vector3D.Left, Color.White, smooth: true, depthRead: true);
				vector3D += Vector3D.Left;
				MyRenderMessageDebugDrawTriangles myRenderMessageDebugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
				myRenderMessageDebugDrawTriangles.AddTriangle(vector3D, vector3D + Vector3D.Up, vector3D + Vector3D.Left);
				myRenderMessageDebugDrawTriangles.AddTriangle(vector3D, vector3D + Vector3D.Left, vector3D - Vector3D.Up);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawCapsule(vector3D, vector3D + Vector3D.Up, 0.5f, Color.White, depthRead: true);
				MyRenderProxy.DebugDrawText2D(new Vector2(100f, 100f), "text", Color.Green, 1f);
				vector3D += Vector3D.Left;
				MyRenderProxy.DebugDrawText3D(vector3D, "3D Text", Color.Blue, 1f, depthRead: true);
			}
		}
	}
}
