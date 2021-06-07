using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Input;
using VRageMath;
using VRageRender;

namespace Sandbox.Common
{
	public class MyRenderDebugInputComponent : MyDebugComponent
	{
		public static List<object> CheckedObjects = new List<object>();

		public static List<Tuple<BoundingBoxD, Color>> AABBsToDraw = new List<Tuple<BoundingBoxD, Color>>();

		public static List<Tuple<Matrix, Color>> MatricesToDraw = new List<Tuple<Matrix, Color>>();

		public static List<Tuple<CapsuleD, Color>> CapsulesToDraw = new List<Tuple<CapsuleD, Color>>();

		public static List<Tuple<Vector3, Vector3, Color>> LinesToDraw = new List<Tuple<Vector3, Vector3, Color>>();

		public static event Action OnDraw;

		public MyRenderDebugInputComponent()
		{
			AddShortcut(MyKeys.C, newPress: true, control: true, shift: false, alt: false, () => "Clears the drawed objects", () => ClearObjects());
		}

		private bool ClearObjects()
		{
			Clear();
			return true;
		}

		public override void Draw()
		{
			base.Draw();
			if (MyRenderDebugInputComponent.OnDraw != null)
			{
				try
				{
					MyRenderDebugInputComponent.OnDraw();
				}
				catch (Exception)
				{
					MyRenderDebugInputComponent.OnDraw = null;
				}
			}
			foreach (Tuple<BoundingBoxD, Color> item in AABBsToDraw)
			{
				MyRenderProxy.DebugDrawAABB(item.Item1, item.Item2, 1f, 1f, depthRead: false);
			}
			foreach (Tuple<Matrix, Color> item2 in MatricesToDraw)
			{
				MyRenderProxy.DebugDrawAxis(item2.Item1, 1f, depthRead: false);
				MyRenderProxy.DebugDrawOBB(item2.Item1, item2.Item2, 1f, depthRead: false, smooth: false);
			}
			foreach (Tuple<Vector3, Vector3, Color> item3 in LinesToDraw)
			{
				MyRenderProxy.DebugDrawLine3D(item3.Item1, item3.Item2, item3.Item3, item3.Item3, depthRead: false);
			}
		}

		public static void Clear()
		{
			AABBsToDraw.Clear();
			MatricesToDraw.Clear();
			CapsulesToDraw.Clear();
			LinesToDraw.Clear();
			MyRenderDebugInputComponent.OnDraw = null;
		}

		public static void AddMatrix(Matrix mat, Color col)
		{
			MatricesToDraw.Add(new Tuple<Matrix, Color>(mat, col));
		}

		public static void AddAABB(BoundingBoxD aabb, Color col)
		{
			AABBsToDraw.Add(new Tuple<BoundingBoxD, Color>(aabb, col));
		}

		public static void AddCapsule(CapsuleD capsule, Color col)
		{
			CapsulesToDraw.Add(new Tuple<CapsuleD, Color>(capsule, col));
		}

		public static void AddLine(Vector3 from, Vector3 to, Color color)
		{
			LinesToDraw.Add(new Tuple<Vector3, Vector3, Color>(from, to, color));
		}

		public override string GetName()
		{
			return "Render";
		}

		public static void BreakIfChecked(object objectToCheck)
		{
			if (CheckedObjects.Contains(objectToCheck))
			{
				Debugger.Break();
			}
		}
	}
}
