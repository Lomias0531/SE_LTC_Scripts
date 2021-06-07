using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
	public static class MySimpleObjectDraw
	{
		private struct FaceInfo
		{
			public bool? Front;

			public MyQuadD Quad;

			public Vector3D Pos;

			public Color Col;
		}

		private static readonly MyStringId ID_CONTAINER_BORDER;

		private static readonly MyStringId ID_GIZMO_DRAW_LINE;

		private static FaceInfo[] temporary_faces;

		private static List<LineD> m_lineBuffer;

		private static readonly List<Vector3D> m_verticesBuffer;

		static MySimpleObjectDraw()
		{
			ID_CONTAINER_BORDER = MyStringId.GetOrCompute("ContainerBorder");
			ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");
			temporary_faces = new FaceInfo[6];
			m_lineBuffer = new List<LineD>(2000);
			m_verticesBuffer = new List<Vector3D>(2000);
		}

		public static void DrawTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, MySimpleObjectRasterizer rasterization, int wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			DrawTransparentBox(ref worldMatrix, ref localbox, ref color, ref color, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
		}

		public static void DrawTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, ref Color frontFaceColor, MySimpleObjectRasterizer rasterization, int wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			DrawTransparentBox(ref worldMatrix, ref localbox, ref color, ref frontFaceColor, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
		}

		public static void DrawTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color faceX_P, ref Color faceY_P, ref Color faceZ_P, ref Color faceX_N, ref Color faceY_N, ref Color faceZ_N, ref Color wire, MySimpleObjectRasterizer rasterization, int wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			DrawTransparentBox(ref worldMatrix, ref localbox, ref faceX_P, ref faceY_P, ref faceZ_P, ref faceX_N, ref faceY_N, ref faceZ_N, ref wire, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
		}

		public static void DrawAttachedTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, uint renderObjectID, ref MatrixD worldToLocal, MySimpleObjectRasterizer rasterization, int wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false)
		{
			DrawAttachedTransparentBox(ref worldMatrix, ref localbox, ref color, renderObjectID, ref worldToLocal, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces);
		}

		public static bool FaceVisible(Vector3D center, Vector3D normal)
		{
			return Vector3D.Dot(Vector3D.Normalize(center - MyTransparentGeometry.Camera.Translation), normal) < 0.0;
		}

		public static bool FaceVisibleRelative(Vector3D center, Vector3D normal)
		{
			return Vector3D.Dot(Vector3D.Normalize(center), normal) < 0.0;
		}

		/// <summary>
		/// DrawTransparentBox
		/// </summary>
		public static void DrawTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, ref Color frontFaceColor, MySimpleObjectRasterizer rasterization, Vector3I wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (!faceMaterial.HasValue || faceMaterial == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
			{
				_ = (Vector3)localbox.Min;
				_ = (Vector3)localbox.Max;
				MatrixD identity = MatrixD.Identity;
				identity.Forward = worldMatrix.Forward;
				identity.Up = worldMatrix.Up;
				identity.Right = worldMatrix.Right;
				Vector3D value = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
				float num = (float)(localbox.Max.X - localbox.Min.X) / 2f;
				float height = (float)(localbox.Max.Y - localbox.Min.Y) / 2f;
				float num2 = (float)(localbox.Max.Z - localbox.Min.Z) / 2f;
				Vector3D vector3D = Vector3D.TransformNormal(Vector3.Forward, identity);
				vector3D *= (double)num2;
				Vector3D position = value + vector3D;
				MyQuadD quad;
				if (!onlyFrontFaces || FaceVisible(position, vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, height, ref worldMatrix);
					MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, frontFaceColor, ref position, customViewProjection, blendType, persistentBillboards);
				}
				position = value - vector3D;
				if (!onlyFrontFaces || FaceVisible(position, -vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, height, ref worldMatrix);
					MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType, persistentBillboards);
				}
				MatrixD matrix = MatrixD.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix;
				vector3D = Vector3.TransformNormal(Vector3.Left, worldMatrix);
				vector3D *= (double)num;
				position = value + vector3D;
				if (!onlyFrontFaces || FaceVisible(position, vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num2, height, ref matrix);
					MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType, persistentBillboards);
				}
				position = value - vector3D;
				if (!onlyFrontFaces || FaceVisible(position, -vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num2, height, ref matrix);
					MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType, persistentBillboards);
				}
				matrix = (MatrixD)Matrix.CreateRotationX(MathHelper.ToRadians(90f)) * worldMatrix;
				vector3D = Vector3.TransformNormal(Vector3.Up, worldMatrix);
				vector3D *= (localbox.Max.Y - localbox.Min.Y) / 2.0;
				position = value + vector3D;
				if (!onlyFrontFaces || FaceVisible(position, vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num2, ref matrix);
					MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType, persistentBillboards);
				}
				position = value - vector3D;
				if (!onlyFrontFaces || FaceVisible(position, -vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num2, ref matrix);
					MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType, persistentBillboards);
				}
			}
			if (rasterization == MySimpleObjectRasterizer.Wireframe || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
			{
				Color color2 = color;
				color2 *= 1.3f;
				DrawWireFramedBox(ref worldMatrix, ref localbox, ref color2, lineWidth, wireDivideRatio, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
			}
		}

		/// <summary>
		/// Definitely not thread safe due to use shared temporary_faces to avoid reinitializations
		/// </summary>
		/// <param name="worldMatrix"></param>
		/// <param name="localbox"></param>
		/// <param name="faceX_P"></param>
		/// <param name="faceY_P"></param>
		/// <param name="faceZ_P"></param>
		/// <param name="faceX_N"></param>
		/// <param name="faceY_N"></param>
		/// <param name="faceZ_N"></param>
		/// <param name="wire"></param>
		/// <param name="rasterization"></param>
		/// <param name="wireDivideRatio"></param>
		/// <param name="lineWidth"></param>
		/// <param name="faceMaterial"></param>
		/// <param name="lineMaterial"></param>
		/// <param name="onlyFrontFaces"></param>
		/// <param name="customViewProjection"></param>
		/// <param name="blendType"></param>
		/// <param name="intensity"></param>
		/// <param name="persistentBillboards"></param>
		public static void DrawTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color faceX_P, ref Color faceY_P, ref Color faceZ_P, ref Color faceX_N, ref Color faceY_N, ref Color faceZ_N, ref Color wire, MySimpleObjectRasterizer rasterization, Vector3I wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (!faceMaterial.HasValue || faceMaterial == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
			{
				_ = (Vector3)localbox.Min;
				_ = (Vector3)localbox.Max;
				MatrixD identity = MatrixD.Identity;
				identity.Forward = worldMatrix.Forward;
				identity.Up = worldMatrix.Up;
				identity.Right = worldMatrix.Right;
				Vector3D value = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
				float num = (float)(localbox.Max.X - localbox.Min.X) / 2f;
				float height = (float)(localbox.Max.Y - localbox.Min.Y) / 2f;
				float num2 = (float)(localbox.Max.Z - localbox.Min.Z) / 2f;
				Vector3D vector3D = Vector3D.TransformNormal(Vector3.Forward, identity);
				vector3D *= (double)num2;
				Vector3D position = value + vector3D;
				bool flag = FaceVisibleRelative(position, vector3D);
				MyQuadD quad;
				if (!onlyFrontFaces || flag)
				{
					MyUtils.GenerateQuad(out quad, ref position, num, height, ref worldMatrix);
					temporary_faces[0].Front = flag;
					temporary_faces[0].Quad = quad;
					temporary_faces[0].Pos = position;
					temporary_faces[0].Col = faceZ_N;
				}
				else
				{
					temporary_faces[0].Front = null;
				}
				position = value - vector3D;
				flag = FaceVisibleRelative(position, -vector3D);
				if (!onlyFrontFaces || flag)
				{
					MyUtils.GenerateQuad(out quad, ref position, num, height, ref worldMatrix);
					temporary_faces[1].Front = flag;
					temporary_faces[1].Quad = quad;
					temporary_faces[1].Pos = position;
					temporary_faces[1].Col = faceZ_P;
				}
				else
				{
					temporary_faces[1].Front = null;
				}
				MatrixD matrix = MatrixD.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix;
				vector3D = Vector3.TransformNormal(Vector3.Left, worldMatrix);
				vector3D *= (double)num;
				position = value + vector3D;
				flag = FaceVisibleRelative(position, vector3D);
				if (!onlyFrontFaces || flag)
				{
					MyUtils.GenerateQuad(out quad, ref position, num2, height, ref matrix);
					temporary_faces[2].Front = flag;
					temporary_faces[2].Quad = quad;
					temporary_faces[2].Pos = position;
					temporary_faces[2].Col = faceX_N;
				}
				else
				{
					temporary_faces[2].Front = null;
				}
				position = value - vector3D;
				flag = FaceVisibleRelative(position, -vector3D);
				if (!onlyFrontFaces || flag)
				{
					MyUtils.GenerateQuad(out quad, ref position, num2, height, ref matrix);
					temporary_faces[3].Front = flag;
					temporary_faces[3].Quad = quad;
					temporary_faces[3].Pos = position;
					temporary_faces[3].Col = faceX_P;
				}
				else
				{
					temporary_faces[3].Front = null;
				}
				matrix = (MatrixD)Matrix.CreateRotationX(MathHelper.ToRadians(90f)) * worldMatrix;
				vector3D = Vector3.TransformNormal(Vector3.Up, worldMatrix);
				vector3D *= (localbox.Max.Y - localbox.Min.Y) / 2.0;
				position = value + vector3D;
				flag = FaceVisibleRelative(position, vector3D);
				if (!onlyFrontFaces || flag)
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num2, ref matrix);
					temporary_faces[4].Front = flag;
					temporary_faces[4].Quad = quad;
					temporary_faces[4].Pos = position;
					temporary_faces[4].Col = faceY_P;
				}
				else
				{
					temporary_faces[4].Front = null;
				}
				position = value - vector3D;
				flag = FaceVisibleRelative(position, -vector3D);
				if (!onlyFrontFaces || flag)
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num2, ref matrix);
					temporary_faces[5].Front = flag;
					temporary_faces[5].Quad = quad;
					temporary_faces[5].Pos = position;
					temporary_faces[5].Col = faceY_N;
				}
				else
				{
					temporary_faces[5].Front = null;
				}
				for (int i = 0; i < 6; i++)
				{
					if (temporary_faces[i].Front.HasValue && !temporary_faces[i].Front.Value)
					{
						MyTransparentGeometry.AddQuad(faceMaterial.Value, ref temporary_faces[i].Quad, temporary_faces[i].Col, ref temporary_faces[i].Pos, customViewProjection, blendType, persistentBillboards);
					}
				}
				for (int j = 0; j < 6; j++)
				{
					if (temporary_faces[j].Front.HasValue && temporary_faces[j].Front.Value)
					{
						MyTransparentGeometry.AddQuad(faceMaterial.Value, ref temporary_faces[j].Quad, temporary_faces[j].Col, ref temporary_faces[j].Pos, customViewProjection, blendType, persistentBillboards);
					}
				}
			}
			if (rasterization == MySimpleObjectRasterizer.Wireframe || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
			{
				Color color = wire;
				color *= 1.3f;
				DrawWireFramedBox(ref worldMatrix, ref localbox, ref color, lineWidth, wireDivideRatio, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
			}
		}

		public static void DrawTransparentRamp(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, MyStringId? faceMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
		{
			if (!faceMaterial.HasValue || faceMaterial == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			MatrixD identity = MatrixD.Identity;
			identity.Forward = worldMatrix.Forward;
			identity.Up = worldMatrix.Up;
			identity.Right = worldMatrix.Right;
			Vector3D value = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
			float num = (float)(localbox.Max.X - localbox.Min.X) / 2f;
			float height = (float)(localbox.Max.Y - localbox.Min.Y) / 2f;
			float num2 = (float)(localbox.Max.Z - localbox.Min.Z) / 2f;
			Vector3D vector3D = Vector3D.TransformNormal(Vector3D.Forward, identity) * num2;
			Vector3D position = value - vector3D;
			MyQuadD quad;
			if (!onlyFrontFaces || FaceVisible(position, -vector3D))
			{
				MyUtils.GenerateQuad(out quad, ref position, num, height, ref worldMatrix);
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType);
			}
			MatrixD matrix = MatrixD.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix;
			vector3D = Vector3.TransformNormal(Vector3.Left, worldMatrix);
			vector3D *= (double)num;
			position = value + vector3D;
			if (!onlyFrontFaces || FaceVisible(position, vector3D))
			{
				MyUtils.GenerateQuad(out quad, ref position, num2, height, ref matrix);
				quad.Point3 = quad.Point0;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType);
			}
			position = value - vector3D;
			if (!onlyFrontFaces || FaceVisible(position, -vector3D))
			{
				MyUtils.GenerateQuad(out quad, ref position, num2, height, ref matrix);
				quad.Point3 = quad.Point0;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType);
			}
			Vector3D point = Vector3D.One;
			Vector3D point2 = Vector3D.One;
			matrix = (MatrixD)Matrix.CreateRotationX(MathHelper.ToRadians(90f)) * worldMatrix;
			vector3D = Vector3.TransformNormal(Vector3.Up, worldMatrix);
			vector3D *= (localbox.Max.Y - localbox.Min.Y) / 2.0;
			position = value - vector3D;
			if (!onlyFrontFaces || FaceVisible(position, -vector3D))
			{
				MyUtils.GenerateQuad(out quad, ref position, num, num2, ref matrix);
				point = quad.Point1;
				point2 = quad.Point2;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType);
			}
			position = value + vector3D;
			if (!onlyFrontFaces || FaceVisible(position, vector3D))
			{
				MyUtils.GenerateQuad(out quad, ref position, num, num2, ref matrix);
				quad.Point1 = point;
				quad.Point2 = point2;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref position, customViewProjection, blendType);
			}
		}

		public static void DrawTransparentRoundedCorner(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, MyStringId? faceMaterial = null, int customViewProjection = -1)
		{
			if (!faceMaterial.HasValue || faceMaterial == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			MyQuadD quad = default(MyQuadD);
			quad.Point0 = localbox.Min;
			quad.Point0.Z = localbox.Max.Z;
			quad.Point1 = localbox.Max;
			quad.Point1.Y = localbox.Min.Y;
			quad.Point2 = localbox.Max;
			quad.Point3 = localbox.Max;
			quad.Point3.X = localbox.Min.X;
			quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
			quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
			quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
			quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
			Vector3D vctPos = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
			MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref vctPos, customViewProjection);
			quad.Point0 = localbox.Min;
			quad.Point0.X = localbox.Max.X;
			quad.Point1 = localbox.Max;
			quad.Point1.Z = localbox.Min.Z;
			quad.Point2 = localbox.Max;
			quad.Point3 = localbox.Max;
			quad.Point3.Y = localbox.Min.Y;
			quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
			quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
			quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
			quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
			vctPos = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
			MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref vctPos, customViewProjection);
			float num = MathF.PI / 20f;
			float num2 = 0f;
			float num3 = (float)(localbox.Max.X - localbox.Min.X);
			float num4 = num3 * 0.5f;
			Vector3D translation = (quad.Point2 + quad.Point3) * 0.5;
			Vector3D translation2 = worldMatrix.Translation;
			worldMatrix.Translation = translation;
			for (int i = 20; i < 30; i++)
			{
				num2 = (float)i * num;
				float num5 = (float)((double)num3 * Math.Cos(num2));
				float num6 = (float)((double)num3 * Math.Sin(num2));
				quad.Point0.X = num5;
				quad.Point0.Z = num6;
				quad.Point3.X = num5;
				quad.Point3.Z = num6;
				num2 = (float)(i + 1) * num;
				num5 = (float)((double)num3 * Math.Cos(num2));
				num6 = (float)((double)num3 * Math.Sin(num2));
				quad.Point1.X = num5;
				quad.Point1.Z = num6;
				quad.Point2.X = num5;
				quad.Point2.Z = num6;
				quad.Point0.Y = 0f - num4;
				quad.Point1.Y = 0f - num4;
				quad.Point2.Y = num4;
				quad.Point3.Y = num4;
				quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
				quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
				quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
				quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
				vctPos = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref vctPos, customViewProjection);
			}
			worldMatrix.Translation = translation2;
		}

		public static void DrawAttachedTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, uint renderObjectID, ref MatrixD worldToLocal, MySimpleObjectRasterizer rasterization, Vector3I wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
		{
			if (!faceMaterial.HasValue || faceMaterial == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
			{
				_ = (Vector3)localbox.Min;
				_ = (Vector3)localbox.Max;
				MatrixD identity = MatrixD.Identity;
				identity.Forward = worldMatrix.Forward;
				identity.Up = worldMatrix.Up;
				identity.Right = worldMatrix.Right;
				Vector3D value = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
				float num = (float)(localbox.Max.X - localbox.Min.X) / 2f;
				float num2 = (float)(localbox.Max.Y - localbox.Min.Y) / 2f;
				float num3 = (float)(localbox.Max.Z - localbox.Min.Z) / 2f;
				Vector3D vector3D = Vector3D.TransformNormal(Vector3D.Forward, identity);
				Vector3D position = value + vector3D * num3;
				MyQuadD quad;
				if (!onlyFrontFaces || FaceVisible(position, vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num2, ref worldMatrix);
					Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
					Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
					Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
					Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
					MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, color, ref position, renderObjectID, blendType);
				}
				position = value - vector3D * num3;
				if (!onlyFrontFaces || FaceVisible(position, -vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num2, ref worldMatrix);
					Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
					Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
					Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
					Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
					MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, color, ref position, renderObjectID, blendType);
				}
				MatrixD matrix = (MatrixD)Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix;
				vector3D = Vector3D.TransformNormal(Vector3D.Left, worldMatrix);
				position = value + vector3D * num;
				if (!onlyFrontFaces || FaceVisible(position, vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num3, num2, ref matrix);
					Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
					Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
					Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
					Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
					MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, color, ref position, renderObjectID, blendType);
				}
				position = value - vector3D * num;
				if (!onlyFrontFaces || FaceVisible(position, -vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num3, num2, ref matrix);
					Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
					Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
					Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
					Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
					MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, color, ref position, renderObjectID, blendType);
				}
				matrix = MatrixD.CreateRotationX(MathHelper.ToRadians(90f)) * worldMatrix;
				vector3D = Vector3D.TransformNormal(Vector3D.Up, worldMatrix);
				position = value + vector3D * num2;
				if (!onlyFrontFaces || FaceVisible(position, vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num3, ref matrix);
					Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
					Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
					Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
					Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
					MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, color, ref position, renderObjectID, blendType);
				}
				position = value - vector3D * num2;
				if (!onlyFrontFaces || FaceVisible(position, -vector3D))
				{
					MyUtils.GenerateQuad(out quad, ref position, num, num3, ref matrix);
					Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
					Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
					Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
					Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
					MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, color, ref position, renderObjectID, blendType);
				}
			}
			if (rasterization == MySimpleObjectRasterizer.Wireframe || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
			{
				Vector4 vctColor = color;
				vctColor *= 1.3f;
				DrawAttachedWireFramedBox(ref worldMatrix, ref localbox, renderObjectID, ref worldToLocal, ref vctColor, lineWidth, wireDivideRatio, lineMaterial, onlyFrontFaces, blendType);
			}
		}

		/// <summary>
		/// DrawWireFramedBox
		/// </summary>
		/// <param name="worldMatrix"></param>
		/// <param name="localbox"></param>
		/// <param name="color"></param>
		/// <param name="bWireFramed"></param>
		/// <param name="wireDivideRatio"></param>
		/// <param name="wireDivideRatio"></param>
		private static void DrawWireFramedBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, float fThickRatio, Vector3I wireDivideRatio, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, int customViewProjection = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f, List<MyBillboard> persistentBillboards = null)
		{
			if (!lineMaterial.HasValue || lineMaterial == MyStringId.NullOrEmpty)
			{
				lineMaterial = MyTransparentMaterials.ErrorMaterial.Id;
			}
			m_lineBuffer.Clear();
			MatrixD identity = MatrixD.Identity;
			identity.Forward = worldMatrix.Forward;
			identity.Up = worldMatrix.Up;
			identity.Right = worldMatrix.Right;
			Vector3D.Dot(identity.Forward, MyTransparentGeometry.Camera.Forward);
			Vector3D.Dot(identity.Right, MyTransparentGeometry.Camera.Forward);
			Vector3D.Dot(identity.Up, MyTransparentGeometry.Camera.Forward);
			Vector3D forward = identity.Forward;
			Vector3D right = identity.Right;
			Vector3D up = identity.Up;
			float num = (float)localbox.Size.X;
			float num2 = (float)localbox.Size.Y;
			float num3 = (float)localbox.Size.Z;
			Vector3D value = Vector3D.Transform(localbox.Center, worldMatrix);
			Vector3D center = value + forward * (num3 * 0.5f);
			Vector3D center2 = value - forward * (num3 * 0.5f);
			Vector3D min = localbox.Min;
			Vector3D vctEnd = min + Vector3.Up * num2;
			Vector3D vctSideStep = Vector3.Right * (num / (float)wireDivideRatio.X);
			if (!onlyFrontFaces || FaceVisible(center, forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			min += Vector3.Backward * num3;
			vctEnd = min + Vector3.Up * num2;
			if (!onlyFrontFaces || FaceVisible(center2, -forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			min = localbox.Min;
			vctEnd = min + Vector3.Right * num;
			vctSideStep = Vector3.Up * (num2 / (float)wireDivideRatio.Y);
			if (!onlyFrontFaces || FaceVisible(center, forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			min += Vector3.Backward * num3;
			vctEnd += Vector3.Backward * num3;
			if (!onlyFrontFaces || FaceVisible(center2, -forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			_ = (Matrix)(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix);
			center = value - right * (num * 0.5f);
			center2 = value + right * (num * 0.5f);
			min = localbox.Min;
			vctEnd = min + Vector3.Backward * num3;
			vctSideStep = Vector3.Up * (num2 / (float)wireDivideRatio.Y);
			if (!onlyFrontFaces || FaceVisible(center, -right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			min = localbox.Min;
			min += Vector3.Right * num;
			vctEnd = min + Vector3.Backward * num3;
			if (!onlyFrontFaces || FaceVisible(center2, right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			min = localbox.Min;
			vctEnd = min + Vector3.Up * num2;
			vctSideStep = Vector3.Backward * (num3 / (float)wireDivideRatio.Z);
			if (!onlyFrontFaces || FaceVisible(center, -right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			min += Vector3.Right * num;
			vctEnd += Vector3.Right * num;
			if (!onlyFrontFaces || FaceVisible(center2, right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			center = value - up * (num2 * 0.5f);
			center2 = value + up * (num2 * 0.5f);
			min = localbox.Min;
			vctEnd = min + Vector3.Right * num;
			vctSideStep = Vector3.Backward * (num3 / (float)wireDivideRatio.Z);
			if (!onlyFrontFaces || FaceVisible(center, -up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			min += Vector3.Up * num2;
			vctEnd += Vector3.Up * num2;
			if (!onlyFrontFaces || FaceVisible(center2, up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			min = localbox.Min;
			vctEnd = min + Vector3.Backward * num3;
			vctSideStep = Vector3.Right * (num / (float)wireDivideRatio.X);
			if (!onlyFrontFaces || FaceVisible(center, -up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			min += Vector3.Up * num2;
			vctEnd += Vector3.Up * num2;
			if (!onlyFrontFaces || FaceVisible(center2, up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			Vector3 vector = new Vector3(localbox.Max.X - localbox.Min.X, localbox.Max.Y - localbox.Min.Y, localbox.Max.Z - localbox.Min.Z);
			float num4 = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector.X, vector.Y), vector.Z));
			num4 *= fThickRatio;
			foreach (LineD item in m_lineBuffer)
			{
				MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, color, item.From, item.Direction, (float)item.Length, num4, blendType, customViewProjection, intensity, persistentBillboards);
			}
		}

		private static void DrawAttachedWireFramedBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, uint renderObjectID, ref MatrixD worldToLocal, ref Vector4 vctColor, float fThickRatio, Vector3I wireDivideRatio, MyStringId? lineMaterial = null, bool onlyFrontFaces = false, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
		{
			if (!lineMaterial.HasValue || lineMaterial == MyStringId.NullOrEmpty)
			{
				lineMaterial = MyTransparentMaterials.ErrorMaterial.Id;
			}
			m_lineBuffer.Clear();
			MatrixD identity = MatrixD.Identity;
			identity.Forward = worldMatrix.Forward;
			identity.Up = worldMatrix.Up;
			identity.Right = worldMatrix.Right;
			Vector3D.Dot(identity.Forward, MyTransparentGeometry.Camera.Forward);
			Vector3D.Dot(identity.Right, MyTransparentGeometry.Camera.Forward);
			Vector3D.Dot(identity.Up, MyTransparentGeometry.Camera.Forward);
			Vector3D forward = identity.Forward;
			Vector3D right = identity.Right;
			Vector3D up = identity.Up;
			float num = (float)localbox.Size.X;
			float num2 = (float)localbox.Size.Y;
			float num3 = (float)localbox.Size.Z;
			Vector3D value = Vector3D.Transform(localbox.Center, worldMatrix);
			Vector3D center = value + forward * (num3 * 0.5f);
			Vector3D center2 = value - forward * (num3 * 0.5f);
			Vector3D min = localbox.Min;
			Vector3D vctEnd = min + Vector3.Up * num2;
			Vector3D vctSideStep = Vector3.Right * (num / (float)wireDivideRatio.X);
			if (!onlyFrontFaces || FaceVisible(center, forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			min += Vector3.Backward * num3;
			vctEnd = min + Vector3.Up * num2;
			if (!onlyFrontFaces || FaceVisible(center2, -forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			min = localbox.Min;
			vctEnd = min + Vector3.Right * num;
			vctSideStep = Vector3.Up * (num2 / (float)wireDivideRatio.Y);
			if (!onlyFrontFaces || FaceVisible(center, forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			min += Vector3.Backward * num3;
			vctEnd += Vector3.Backward * num3;
			if (!onlyFrontFaces || FaceVisible(center2, -forward))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			_ = (Matrix)(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix);
			center = value - right * (num * 0.5f);
			center2 = value + right * (num * 0.5f);
			min = localbox.Min;
			vctEnd = min + Vector3.Backward * num3;
			vctSideStep = Vector3.Up * (num2 / (float)wireDivideRatio.Y);
			if (!onlyFrontFaces || FaceVisible(center, -right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			min = localbox.Min;
			min += Vector3.Right * num;
			vctEnd = min + Vector3.Backward * num3;
			if (!onlyFrontFaces || FaceVisible(center2, right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Y);
			}
			min = localbox.Min;
			vctEnd = min + Vector3.Up * num2;
			vctSideStep = Vector3.Backward * (num3 / (float)wireDivideRatio.Z);
			if (!onlyFrontFaces || FaceVisible(center, -right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			min += Vector3.Right * num;
			vctEnd += Vector3.Right * num;
			if (!onlyFrontFaces || FaceVisible(center2, right))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			center = value - up * (num2 * 0.5f);
			center2 = value + up * (num2 * 0.5f);
			min = localbox.Min;
			vctEnd = min + Vector3.Right * num;
			vctSideStep = Vector3.Backward * (num3 / (float)wireDivideRatio.Z);
			if (!onlyFrontFaces || FaceVisible(center, -up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			min += Vector3.Up * num2;
			vctEnd += Vector3.Up * num2;
			if (!onlyFrontFaces || FaceVisible(center2, up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.Z);
			}
			min = localbox.Min;
			vctEnd = min + Vector3.Backward * num3;
			vctSideStep = Vector3.Right * (num / (float)wireDivideRatio.X);
			if (!onlyFrontFaces || FaceVisible(center, -up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			min += Vector3.Up * num2;
			vctEnd += Vector3.Up * num2;
			if (!onlyFrontFaces || FaceVisible(center2, up))
			{
				GenerateLines(min, vctEnd, ref vctSideStep, ref worldMatrix, ref m_lineBuffer, wireDivideRatio.X);
			}
			Vector3 vector = new Vector3(localbox.Max.X - localbox.Min.X, localbox.Max.Y - localbox.Min.Y, localbox.Max.Z - localbox.Min.Z);
			float num4 = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector.X, vector.Y), vector.Z));
			num4 *= fThickRatio;
			foreach (LineD item in m_lineBuffer)
			{
				MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, vctColor, item.From, renderObjectID, ref worldToLocal, item.Direction, (float)item.Length, num4, blendType);
			}
		}

		public static void DrawTransparentSphere(List<Vector3D> verticesBuffer, float radius, ref Color color, MySimpleObjectRasterizer rasterization, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, float lineThickness = -1f, int customViewProjectionMatrix = -1, List<MyBillboard> persistentBillboards = null, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f)
		{
			Vector3D vctPos = Vector3D.Zero;
			float thickness = radius * 0.01f;
			if (lineThickness > -1f)
			{
				thickness = lineThickness;
			}
			int num = 0;
			MyQuadD quad = default(MyQuadD);
			for (num = 0; num < verticesBuffer.Count; num += 4)
			{
				quad.Point0 = verticesBuffer[num + 1];
				quad.Point1 = verticesBuffer[num + 3];
				quad.Point2 = verticesBuffer[num + 2];
				quad.Point3 = verticesBuffer[num];
				if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
				{
					MyTransparentGeometry.AddQuad(faceMaterial ?? ID_CONTAINER_BORDER, ref quad, color, ref vctPos, customViewProjectionMatrix, blendType, persistentBillboards);
				}
				if (rasterization == MySimpleObjectRasterizer.Wireframe || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
				{
					Vector3D point = quad.Point0;
					Vector3 vec = quad.Point1 - point;
					float num2 = vec.Length();
					if (num2 > 0.1f)
					{
						vec = MyUtils.Normalize(vec);
						MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, color, point, vec, num2, thickness, blendType, customViewProjectionMatrix, intensity, persistentBillboards);
					}
					point = quad.Point1;
					vec = quad.Point2 - point;
					num2 = vec.Length();
					if (num2 > 0.1f)
					{
						vec = MyUtils.Normalize(vec);
						MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, color, point, vec, num2, thickness, blendType, customViewProjectionMatrix, intensity, persistentBillboards);
					}
				}
			}
		}

		/// <summary>
		/// DrawTransparentSphere
		/// </summary>
		/// <param name="vctPos"></param>
		/// <param name="radius"></param>
		/// <param name="color"></param>
		/// <param name="bWireFramed"></param>
		/// <param name="wireDivideRatio"></param>
		public static void DrawTransparentSphere(ref MatrixD worldMatrix, float radius, ref Color color, MySimpleObjectRasterizer rasterization, int wireDivideRatio, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, float lineThickness = -1f, int customViewProjectionMatrix = -1, List<MyBillboard> persistentBillboards = null, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float intensity = 1f)
		{
			if (!lineMaterial.HasValue || lineMaterial == MyStringId.NullOrEmpty)
			{
				lineMaterial = MyTransparentMaterials.ErrorMaterial.Id;
			}
			m_verticesBuffer.Clear();
			MyMeshHelper.GenerateSphere(ref worldMatrix, radius, wireDivideRatio, m_verticesBuffer);
			DrawTransparentSphere(m_verticesBuffer, radius, ref color, rasterization, faceMaterial, lineMaterial, lineThickness, customViewProjectionMatrix, persistentBillboards, blendType, intensity);
		}

		public static void DrawTransparentCapsule(ref MatrixD worldMatrix, float radius, float height, ref Color color, int wireDivideRatio, MyStringId? faceMaterial = null, int customViewProjectionMatrix = -1, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
		{
			if (!faceMaterial.HasValue || faceMaterial == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			float num = height * 0.5f;
			_ = worldMatrix.Translation;
			MatrixD worldMatrix2 = MatrixD.CreateRotationX(-1.570796012878418);
			worldMatrix2.Translation = new Vector3D(0.0, num, 0.0);
			worldMatrix2 *= worldMatrix;
			m_verticesBuffer.Clear();
			MyMeshHelper.GenerateSphere(ref worldMatrix2, radius, wireDivideRatio, m_verticesBuffer);
			Vector3D vctPos = Vector3D.Zero;
			int num2 = m_verticesBuffer.Count / 2;
			MyQuadD quad = default(MyQuadD);
			for (int i = 0; i < num2; i += 4)
			{
				quad.Point0 = m_verticesBuffer[i + 1];
				quad.Point1 = m_verticesBuffer[i + 3];
				quad.Point2 = m_verticesBuffer[i + 2];
				quad.Point3 = m_verticesBuffer[i];
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref vctPos, customViewProjectionMatrix, blendType);
			}
			MatrixD worldMatrix3 = MatrixD.CreateRotationX(-1.570796012878418);
			worldMatrix3.Translation = new Vector3D(0.0, 0f - num, 0.0);
			worldMatrix3 *= worldMatrix;
			m_verticesBuffer.Clear();
			MyMeshHelper.GenerateSphere(ref worldMatrix3, radius, wireDivideRatio, m_verticesBuffer);
			MyQuadD quad2 = default(MyQuadD);
			for (int j = num2; j < m_verticesBuffer.Count; j += 4)
			{
				quad2.Point0 = m_verticesBuffer[j + 1];
				quad2.Point1 = m_verticesBuffer[j + 3];
				quad2.Point2 = m_verticesBuffer[j + 2];
				quad2.Point3 = m_verticesBuffer[j];
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad2, color, ref vctPos, customViewProjectionMatrix, blendType);
			}
			float num3 = MathF.PI * 2f / (float)wireDivideRatio;
			float num4 = 0f;
			MyQuadD quad3 = default(MyQuadD);
			for (int k = 0; k < wireDivideRatio; k++)
			{
				num4 = (float)k * num3;
				float num5 = (float)((double)radius * Math.Cos(num4));
				float num6 = (float)((double)radius * Math.Sin(num4));
				quad3.Point0.X = num5;
				quad3.Point0.Z = num6;
				quad3.Point3.X = num5;
				quad3.Point3.Z = num6;
				num4 = (float)(k + 1) * num3;
				num5 = (float)((double)radius * Math.Cos(num4));
				num6 = (float)((double)radius * Math.Sin(num4));
				quad3.Point1.X = num5;
				quad3.Point1.Z = num6;
				quad3.Point2.X = num5;
				quad3.Point2.Z = num6;
				quad3.Point0.Y = 0f - num;
				quad3.Point1.Y = 0f - num;
				quad3.Point2.Y = num;
				quad3.Point3.Y = num;
				quad3.Point0 = Vector3D.Transform(quad3.Point0, worldMatrix);
				quad3.Point1 = Vector3D.Transform(quad3.Point1, worldMatrix);
				quad3.Point2 = Vector3D.Transform(quad3.Point2, worldMatrix);
				quad3.Point3 = Vector3D.Transform(quad3.Point3, worldMatrix);
				Vector3D vctPos2 = (quad3.Point0 + quad3.Point1 + quad3.Point2 + quad3.Point3) * 0.25;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad3, color, ref vctPos2, customViewProjectionMatrix, blendType);
			}
		}

		public static void DrawTransparentCone(ref MatrixD worldMatrix, float radius, float height, ref Color color, int wireDivideRatio, MyStringId? faceMaterial = null, int customViewProjectionMatrix = -1)
		{
			DrawTransparentCone(worldMatrix.Translation, (Vector3)worldMatrix.Forward * height, (Vector3)worldMatrix.Up * radius, color, wireDivideRatio, faceMaterial, customViewProjectionMatrix);
		}

		private static void DrawTransparentCone(Vector3D apexPosition, Vector3 directionVector, Vector3 baseVector, Color color, int wireDivideRatio, MyStringId? faceMaterial = null, int customViewProjectionMatrix = -1)
		{
			if (!faceMaterial.HasValue || faceMaterial.Value == MyStringId.NullOrEmpty)
			{
				faceMaterial = ID_CONTAINER_BORDER;
			}
			Vector3 axis = directionVector;
			axis.Normalize();
			float num = (float)(Math.PI * 2.0 / (double)wireDivideRatio);
			MyQuadD quad = default(MyQuadD);
			for (int i = 0; i < wireDivideRatio; i++)
			{
				float angle = (float)i * num;
				float angle2 = (float)(i + 1) * num;
				Vector3D point = apexPosition + directionVector + Vector3.Transform(baseVector, Matrix.CreateFromAxisAngle(axis, angle));
				Vector3D point2 = apexPosition + directionVector + Vector3.Transform(baseVector, Matrix.CreateFromAxisAngle(axis, angle2));
				quad.Point0 = point;
				quad.Point1 = point2;
				quad.Point2 = apexPosition;
				quad.Point3 = apexPosition;
				MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, color, ref Vector3D.Zero);
			}
		}

		public static void DrawTransparentCuboid(ref MatrixD worldMatrix, MyCuboid cuboid, ref Vector4 vctColor, bool bWireFramed, float thickness, MyStringId? lineMaterial = null)
		{
			foreach (Line uniqueLine in cuboid.UniqueLines)
			{
				Vector3D start = Vector3D.Transform(uniqueLine.From, worldMatrix);
				Vector3D end = Vector3D.Transform(uniqueLine.To, worldMatrix);
				DrawLine(start, end, lineMaterial ?? ID_GIZMO_DRAW_LINE, ref vctColor, thickness);
			}
		}

		public static void DrawLine(Vector3D start, Vector3D end, MyStringId? material, ref Vector4 color, float thickness, MyBillboard.BlendTypeEnum blendtype = MyBillboard.BlendTypeEnum.Standard)
		{
			Vector3 vec = end - start;
			float num = vec.Length();
			if (num > 0.1f)
			{
				vec = MyUtils.Normalize(vec);
				MyTransparentGeometry.AddLineBillboard(material ?? ID_GIZMO_DRAW_LINE, color, start, vec, num, thickness, blendtype);
			}
		}

		public static void DrawTransparentCylinder(ref MatrixD worldMatrix, float radius1, float radius2, float length, ref Vector4 vctColor, bool bWireFramed, int wireDivideRatio, float thickness, MyStringId? lineMaterial = null)
		{
			Vector3D vector3D = Vector3.Zero;
			Vector3D vector3D2 = Vector3.Zero;
			Vector3D start = Vector3.Zero;
			Vector3D start2 = Vector3.Zero;
			float num = 360f / (float)wireDivideRatio;
			float num2 = 0f;
			for (int i = 0; i <= wireDivideRatio; i++)
			{
				num2 = (float)i * num;
				vector3D.X = (float)((double)radius1 * Math.Cos(MathHelper.ToRadians(num2)));
				vector3D.Y = length / 2f;
				vector3D.Z = (float)((double)radius1 * Math.Sin(MathHelper.ToRadians(num2)));
				vector3D2.X = (float)((double)radius2 * Math.Cos(MathHelper.ToRadians(num2)));
				vector3D2.Y = (0f - length) / 2f;
				vector3D2.Z = (float)((double)radius2 * Math.Sin(MathHelper.ToRadians(num2)));
				vector3D = Vector3D.Transform(vector3D, worldMatrix);
				vector3D2 = Vector3D.Transform(vector3D2, worldMatrix);
				DrawLine(vector3D2, vector3D, lineMaterial ?? ID_GIZMO_DRAW_LINE, ref vctColor, thickness);
				if (i > 0)
				{
					DrawLine(start2, vector3D2, lineMaterial ?? ID_GIZMO_DRAW_LINE, ref vctColor, thickness);
					DrawLine(start, vector3D, lineMaterial ?? ID_GIZMO_DRAW_LINE, ref vctColor, thickness);
				}
				start2 = vector3D2;
				start = vector3D;
			}
		}

		public static void DrawTransparentPyramid(ref Vector3D start, ref MyQuad backQuad, ref Vector4 vctColor, int divideRatio, float thickness, MyStringId? lineMaterial = null)
		{
			_ = Vector3.Zero;
			m_lineBuffer.Clear();
			GenerateLines(start, backQuad.Point0, backQuad.Point1, ref m_lineBuffer, divideRatio);
			GenerateLines(start, backQuad.Point1, backQuad.Point2, ref m_lineBuffer, divideRatio);
			GenerateLines(start, backQuad.Point2, backQuad.Point3, ref m_lineBuffer, divideRatio);
			GenerateLines(start, backQuad.Point3, backQuad.Point0, ref m_lineBuffer, divideRatio);
			foreach (LineD item in m_lineBuffer)
			{
				Vector3 vec = item.To - item.From;
				float num = vec.Length();
				if (num > 0.1f)
				{
					vec = MyUtils.Normalize(vec);
					MyTransparentGeometry.AddLineBillboard(lineMaterial ?? ID_GIZMO_DRAW_LINE, vctColor, item.From, vec, num, thickness);
				}
			}
		}

		private static void GenerateLines(Vector3D start, Vector3D end1, Vector3D end2, ref List<LineD> lineBuffer, int divideRatio)
		{
			Vector3D value = (end2 - end1) / divideRatio;
			for (int i = 0; i < divideRatio; i++)
			{
				LineD item = new LineD(start, end1 + i * value);
				lineBuffer.Add(item);
			}
		}

		/// <summary>
		/// GenerateLines
		/// </summary>
		/// <param name="vctStart"></param>
		/// <param name="vctEnd"></param>
		/// <param name="vctSideStep"></param>
		/// <param name="worldMatrix"></param>
		/// <param name="lineBuffer"></param>
		/// <param name="divideRatio"></param>
		private static void GenerateLines(Vector3D vctStart, Vector3D vctEnd, ref Vector3D vctSideStep, ref MatrixD worldMatrix, ref List<LineD> lineBuffer, int divideRatio)
		{
			for (int i = 0; i <= divideRatio; i++)
			{
				Vector3D from = Vector3D.Transform(vctStart, worldMatrix);
				Vector3D to = Vector3D.Transform(vctEnd, worldMatrix);
				if (lineBuffer.Count < lineBuffer.Capacity)
				{
					LineD item = new LineD(from, to);
					lineBuffer.Add(item);
					vctStart += vctSideStep;
					vctEnd += vctSideStep;
				}
			}
		}
	}
}
