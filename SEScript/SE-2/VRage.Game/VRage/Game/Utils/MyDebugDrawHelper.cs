using System;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Utils
{
	/// <summary>
	/// Set of helper functions for debug drawing.
	/// </summary>
	public static class MyDebugDrawHelper
	{
		/// <summary>
		/// Debug draw a point with an explanation name next to it.
		/// </summary>
		/// <param name="pos">world space coordinates of the point</param>
		/// <param name="name">point name, caption</param>
		/// <param name="color">point color, pass null to keep default white</param>
		/// <param name="cameraViewMatrix">helper camera matrix, pass null to ignore camera transform - this just makes the debug draw prettier</param>
		public static void DrawNamedPoint(Vector3D pos, string name, Color? color = null, MatrixD? cameraViewMatrix = null)
		{
			if (!cameraViewMatrix.HasValue)
			{
				cameraViewMatrix = MatrixD.Identity;
			}
			if (!color.HasValue)
			{
				color = Color.White;
			}
			MatrixD matrix = cameraViewMatrix.Value;
			matrix = MatrixD.Invert(ref matrix);
			int hashCode = name.GetHashCode();
			Vector3D value = 0.5 * matrix.Right * Math.Cos(hashCode) + matrix.Up * (0.75 + 0.25 * Math.Abs(Math.Sin(hashCode)));
			MyRenderProxy.DebugDrawText3D(pos + value, name, color.Value, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			DrawDashedLine(pos + value, pos, color.Value);
			MyRenderProxy.DebugDrawSphere(pos, 0.025f, color.Value, 1f, depthRead: false);
		}

		public static void DrawDashedLine(Vector3D pos1, Vector3D pos2, Color colorValue)
		{
			float num = (float)(0.1 / (pos1 - pos2).Length());
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				Vector3D.Lerp(ref pos1, ref pos2, num2, out Vector3D result);
				Vector3D.Lerp(ref pos1, ref pos2, num2 + 0.3f * num, out Vector3D result2);
				MyRenderProxy.DebugDrawLine3D(result, result2, colorValue, colorValue, depthRead: false);
			}
		}

		/// <summary>
		/// Draw colored named axis.
		/// </summary>
		/// <param name="matrix">matrix containing axes</param>
		/// <param name="axisLengthScale">axis visualization length</param>
		/// <param name="name">helper label</param>
		/// <param name="color">helper color</param>
		public static void DrawNamedColoredAxis(MatrixD matrix, float axisLengthScale = 1f, string name = null, Color? color = null)
		{
			if (!color.HasValue)
			{
				color = Color.White;
			}
			MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Right * (axisLengthScale * 0.8f), color.Value, color.Value, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(matrix.Translation + matrix.Right * (axisLengthScale * 0.8f), matrix.Translation + matrix.Right * axisLengthScale, Color.Red, Color.Red, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Up * (axisLengthScale * 0.8f), color.Value, color.Value, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(matrix.Translation + matrix.Up * (axisLengthScale * 0.8f), matrix.Translation + matrix.Up * axisLengthScale, Color.Green, Color.Green, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Forward * (axisLengthScale * 0.8f), color.Value, color.Value, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(matrix.Translation + matrix.Forward * (axisLengthScale * 0.8f), matrix.Translation + matrix.Forward * axisLengthScale, Color.Blue, Color.Blue, depthRead: false);
			DrawNamedPoint(matrix.Translation, name, color);
		}
	}
}
