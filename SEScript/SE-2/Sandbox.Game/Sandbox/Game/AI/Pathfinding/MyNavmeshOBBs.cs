using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyNavmeshOBBs
	{
		public struct OBBCoords
		{
			public Vector2I Coords;

			public MyOrientedBoundingBoxD OBB;
		}

		public enum OBBCorner
		{
			UpperFrontLeft,
			UpperBackLeft,
			LowerBackLeft,
			LowerFrontLeft,
			UpperFrontRight,
			UpperBackRight,
			LowerBackRight,
			LowerFrontRight
		}

		private const int NEIGHBOUR_OVERLAP_TILES = 2;

		private MyOrientedBoundingBoxD?[][] m_obbs;

		private float m_tileHalfSize;

		private float m_tileHalfHeight;

		private MyPlanet m_planet;

		private int m_middleCoord;

		public int OBBsPerLine
		{
			get;
			private set;
		}

		public MyOrientedBoundingBoxD BaseOBB
		{
			get;
			private set;
		}

		public MyOrientedBoundingBoxD CenterOBB
		{
			get
			{
				return m_obbs[m_middleCoord][m_middleCoord].Value;
			}
			private set
			{
				m_obbs[m_middleCoord][m_middleCoord] = value;
			}
		}

		public List<Vector3D> NeighboursCenters
		{
			get;
			private set;
		}

		public MyNavmeshOBBs(MyPlanet planet, Vector3D centerPoint, Vector3D forwardDirection, int obbsPerLine, int tileSize, int tileHeight)
		{
			m_planet = planet;
			OBBsPerLine = obbsPerLine;
			if (OBBsPerLine % 2 == 0)
			{
				OBBsPerLine++;
			}
			m_middleCoord = (OBBsPerLine - 1) / 2;
			m_tileHalfSize = (float)tileSize * 0.5f;
			m_tileHalfHeight = (float)tileHeight * 0.5f;
			m_obbs = new MyOrientedBoundingBoxD?[OBBsPerLine][];
			for (int i = 0; i < OBBsPerLine; i++)
			{
				m_obbs[i] = new MyOrientedBoundingBoxD?[OBBsPerLine];
			}
			Initialize(centerPoint, forwardDirection);
			BaseOBB = GetBaseOBB();
		}

		public MyOrientedBoundingBoxD? GetOBB(int coordX, int coordY)
		{
			if (coordX < 0 || coordX >= OBBsPerLine || coordY < 0 || coordY >= OBBsPerLine)
			{
				return null;
			}
			return m_obbs[coordX][coordY];
		}

		public MyOrientedBoundingBoxD? GetOBB(Vector3D worldPosition)
		{
			MyOrientedBoundingBoxD?[][] obbs = m_obbs;
			foreach (MyOrientedBoundingBoxD?[] array in obbs)
			{
				for (int j = 0; j < array.Length; j++)
				{
					MyOrientedBoundingBoxD? result = array[j];
					if (result.Value.Contains(ref worldPosition))
					{
						return result;
					}
				}
			}
			return null;
		}

		public OBBCoords? GetOBBCoord(int coordX, int coordY)
		{
			if (coordX < 0 || coordX >= OBBsPerLine || coordY < 0 || coordY >= OBBsPerLine)
			{
				return null;
			}
			OBBCoords value = default(OBBCoords);
			value.OBB = m_obbs[coordX][coordY].Value;
			value.Coords = new Vector2I(coordX, coordY);
			return value;
		}

		public OBBCoords? GetOBBCoord(Vector3D worldPosition)
		{
			for (int i = 0; i < m_obbs.Length; i++)
			{
				for (int j = 0; j < m_obbs[0].Length; j++)
				{
					MyOrientedBoundingBoxD value = m_obbs[i][j].Value;
					if (value.Contains(ref worldPosition))
					{
						OBBCoords value2 = default(OBBCoords);
						value2.OBB = value;
						value2.Coords = new Vector2I(i, j);
						return value2;
					}
				}
			}
			return null;
		}

		public List<OBBCoords> GetIntersectedOBB(LineD line)
		{
			Dictionary<OBBCoords, double> dictionary = new Dictionary<OBBCoords, double>();
			for (int i = 0; i < m_obbs.Length; i++)
			{
				for (int j = 0; j < m_obbs[0].Length; j++)
				{
					if (m_obbs[i][j].Value.Contains(ref line.From) || m_obbs[i][j].Value.Contains(ref line.To) || m_obbs[i][j].Value.Intersects(ref line).HasValue)
					{
						dictionary.Add(new OBBCoords
						{
							OBB = m_obbs[i][j].Value,
							Coords = new Vector2I(i, j)
						}, Vector3D.Distance(line.From, m_obbs[i][j].Value.Center));
					}
				}
			}
			return (from d in dictionary
				orderby d.Value
				select d into kvp
				select kvp.Key).ToList();
		}

		public void DebugDraw()
		{
			for (int i = 0; i < m_obbs.Length; i++)
			{
				for (int j = 0; j < m_obbs[0].Length; j++)
				{
					if (m_obbs[i][j].HasValue)
					{
						MyRenderProxy.DebugDrawOBB(m_obbs[i][j].Value, Color.Red, 0f, depthRead: true, smooth: false);
					}
				}
			}
			MyRenderProxy.DebugDrawOBB(BaseOBB, Color.White, 0f, depthRead: true, smooth: false);
			if (m_obbs[0][0].HasValue)
			{
				MyRenderProxy.DebugDrawSphere(m_obbs[0][0].Value.Center, 5f, Color.Yellow, 0f);
			}
			if (m_obbs[0][OBBsPerLine - 1].HasValue)
			{
				MyRenderProxy.DebugDrawSphere(m_obbs[0][OBBsPerLine - 1].Value.Center, 5f, Color.Green, 0f);
			}
			if (m_obbs[OBBsPerLine - 1][OBBsPerLine - 1].HasValue)
			{
				MyRenderProxy.DebugDrawSphere(m_obbs[OBBsPerLine - 1][OBBsPerLine - 1].Value.Center, 5f, Color.Blue, 0f);
			}
			if (m_obbs[OBBsPerLine - 1][0].HasValue)
			{
				MyRenderProxy.DebugDrawSphere(m_obbs[OBBsPerLine - 1][0].Value.Center, 5f, Color.White, 0f);
			}
			MyOrientedBoundingBoxD? myOrientedBoundingBoxD = m_obbs[0][0];
			MyOrientedBoundingBoxD? myOrientedBoundingBoxD2 = m_obbs[OBBsPerLine - 1][0];
			MyOrientedBoundingBoxD? myOrientedBoundingBoxD3 = m_obbs[OBBsPerLine - 1][OBBsPerLine - 1];
			MyRenderProxy.DebugDrawSphere(GetOBBCorner(myOrientedBoundingBoxD.Value, OBBCorner.LowerBackLeft), 5f, Color.White, 0f);
			MyRenderProxy.DebugDrawSphere(GetOBBCorner(myOrientedBoundingBoxD2.Value, OBBCorner.LowerBackRight), 5f, Color.White, 0f);
			MyRenderProxy.DebugDrawSphere(GetOBBCorner(myOrientedBoundingBoxD3.Value, OBBCorner.LowerFrontRight), 5f, Color.White, 0f);
		}

		public void Clear()
		{
			for (int i = 0; i < m_obbs.Length; i++)
			{
				Array.Clear(m_obbs[i], 0, m_obbs.Length);
			}
			Array.Clear(m_obbs, 0, m_obbs.Length);
			m_obbs = null;
		}

		private void Initialize(Vector3D initialPoint, Vector3D forwardDirection)
		{
			CenterOBB = GetCenterOBB(initialPoint, forwardDirection, out double angle);
			m_obbs[m_middleCoord][m_middleCoord] = CenterOBB;
			Fill(angle);
			double neigboursCenter = angle * (double)Math.Max(2 * m_middleCoord - 1, 1);
			SetNeigboursCenter(neigboursCenter);
		}

		private void Fill(double angle)
		{
			Vector2I currentIndex = new Vector2I(m_middleCoord, 0);
			for (int i = 0; i < OBBsPerLine; i++)
			{
				MyOrientedBoundingBoxD lineCenterOBB = (!m_obbs[currentIndex.Y][currentIndex.X].HasValue) ? CreateOBB(NewTransformedPoint(CenterOBB.Center, CenterOBB.Orientation.Forward, (float)angle * (float)(i - m_middleCoord)), CenterOBB.Orientation.Forward) : m_obbs[currentIndex.Y][currentIndex.X].Value;
				FillOBBHorizontalLine(lineCenterOBB, currentIndex, angle);
				currentIndex.Y++;
			}
		}

		private void FillOBBHorizontalLine(MyOrientedBoundingBoxD lineCenterOBB, Vector2I currentIndex, double angle)
		{
			m_obbs[currentIndex.Y][currentIndex.X] = lineCenterOBB;
			for (int i = 0; i < OBBsPerLine; i++)
			{
				if (i != currentIndex.X)
				{
					MyOrientedBoundingBoxD value = CreateOBB(NewTransformedPoint(lineCenterOBB.Center, lineCenterOBB.Orientation.Right, (float)(angle * (double)(i - m_middleCoord))), lineCenterOBB.Orientation.Right);
					m_obbs[currentIndex.Y][i] = value;
				}
			}
		}

		private MyOrientedBoundingBoxD GetCenterOBB(Vector3D initialPoint, Vector3D forwardDirection, out double angle)
		{
			Vector3D center = m_planet.PositionComp.WorldAABB.Center;
			double num = (initialPoint - center).Length();
			double num2 = Math.Asin((double)m_tileHalfSize / num);
			angle = num2 * 2.0;
			return CreateOBB(initialPoint, forwardDirection);
		}

		private Vector3D NewTransformedPoint(Vector3D point, Vector3 rotationVector, float angle)
		{
			Vector3D center = m_planet.PositionComp.WorldAABB.Center;
			Quaternion rotation = Quaternion.CreateFromAxisAngle(rotationVector, angle);
			return Vector3D.Transform(point - center, rotation) + center;
		}

		private MyOrientedBoundingBoxD CreateOBB(Vector3D center, Vector3D perpedicularVector)
		{
			Vector3D v = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
			return new MyOrientedBoundingBoxD(center, new Vector3D(m_tileHalfSize, m_tileHalfHeight, m_tileHalfSize), Quaternion.CreateFromForwardUp(perpedicularVector, v));
		}

		private MyOrientedBoundingBoxD GetBaseOBB()
		{
			MyOrientedBoundingBoxD? myOrientedBoundingBoxD = m_obbs[0][0];
			MyOrientedBoundingBoxD? myOrientedBoundingBoxD2 = m_obbs[OBBsPerLine - 1][0];
			MyOrientedBoundingBoxD? myOrientedBoundingBoxD3 = m_obbs[OBBsPerLine - 1][OBBsPerLine - 1];
			Vector3D oBBCorner = GetOBBCorner(myOrientedBoundingBoxD.Value, OBBCorner.LowerBackLeft);
			Vector3D oBBCorner2 = GetOBBCorner(myOrientedBoundingBoxD2.Value, OBBCorner.LowerBackRight);
			Vector3D oBBCorner3 = GetOBBCorner(myOrientedBoundingBoxD3.Value, OBBCorner.LowerFrontRight);
			Vector3D center = (oBBCorner + oBBCorner3) / 2.0;
			double num = (oBBCorner - oBBCorner2).Length() / 2.0;
			double y = 0.01;
			return new MyOrientedBoundingBoxD(center, new Vector3D(num, y, num), CenterOBB.Orientation);
		}

		private void SetNeigboursCenter(double angle)
		{
			NeighboursCenters = new List<Vector3D>();
			Vector3D item = NewTransformedPoint(CenterOBB.Center, CenterOBB.Orientation.Forward, (float)angle);
			Vector3D item2 = NewTransformedPoint(CenterOBB.Center, CenterOBB.Orientation.Forward, 0f - (float)angle);
			Vector3D vector3D = NewTransformedPoint(CenterOBB.Center, CenterOBB.Orientation.Right, (float)angle);
			Vector3D vector3D2 = NewTransformedPoint(CenterOBB.Center, CenterOBB.Orientation.Right, 0f - (float)angle);
			NeighboursCenters.Add(item);
			NeighboursCenters.Add(item2);
			NeighboursCenters.Add(vector3D);
			NeighboursCenters.Add(vector3D2);
			Vector3D item3 = NewTransformedPoint(vector3D2, CenterOBB.Orientation.Forward, 0f - (float)angle);
			Vector3D item4 = NewTransformedPoint(vector3D2, CenterOBB.Orientation.Forward, (float)angle);
			Vector3D item5 = NewTransformedPoint(vector3D, CenterOBB.Orientation.Forward, 0f - (float)angle);
			Vector3D item6 = NewTransformedPoint(vector3D, CenterOBB.Orientation.Forward, (float)angle);
			NeighboursCenters.Add(item3);
			NeighboursCenters.Add(item4);
			NeighboursCenters.Add(item5);
			NeighboursCenters.Add(item6);
		}

		public static Vector3D GetOBBCorner(MyOrientedBoundingBoxD obb, OBBCorner corner)
		{
			Vector3D[] array = new Vector3D[8];
			obb.GetCorners(array, 0);
			return array[(int)corner];
		}
	}
}
