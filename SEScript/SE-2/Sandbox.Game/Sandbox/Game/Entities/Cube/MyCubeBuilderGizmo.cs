using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.DebugScreens;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyCubeBuilderGizmo
	{
		public class MyGizmoSpaceProperties
		{
			public bool Enabled;

			public MyGizmoSpaceEnum SourceSpace;

			public MySymmetrySettingModeEnum SymmetryPlane;

			public Vector3I SymmetryPlanePos;

			public bool SymmetryIsOdd;

			public MatrixD m_worldMatrixAdd = Matrix.Identity;

			public Matrix m_localMatrixAdd = Matrix.Identity;

			public Vector3I m_addDir = Vector3I.Up;

			public Vector3I m_addPos;

			public Vector3I m_min;

			public Vector3I m_max;

			public Vector3I m_centerPos;

			public Vector3I m_removePos;

			public MySlimBlock m_removeBlock;

			public ushort? m_blockIdInCompound;

			public Vector3I? m_startBuild;

			public Vector3I? m_continueBuild;

			public Vector3I? m_startRemove;

			public List<Vector3I> m_positions = new List<Vector3I>();

			public List<Vector3> m_cubeNormals = new List<Vector3>();

			public List<Vector4UByte> m_patternOffsets = new List<Vector4UByte>();

			public Vector3? m_addPosSmallOnLarge;

			public Vector3 m_minSmallOnLarge;

			public Vector3 m_maxSmallOnLarge;

			public Vector3 m_centerPosSmallOnLarge;

			public List<Vector3> m_positionsSmallOnLarge = new List<Vector3>();

			public List<string> m_cubeModels = new List<string>();

			public List<MatrixD> m_cubeMatrices = new List<MatrixD>();

			public List<string> m_cubeModelsTemp = new List<string>();

			public List<MatrixD> m_cubeMatricesTemp = new List<MatrixD>();

			public bool m_buildAllowed;

			public bool m_showGizmoCube;

			public Quaternion m_rotation;

			public Vector3I m_mirroringOffset;

			public MyCubeBlockDefinition m_blockDefinition;

			public bool m_dynamicBuildAllowed;

			public HashSet<Tuple<MySlimBlock, ushort?>> m_removeBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();

			public MatrixD m_animationLastMatrix = MatrixD.Identity;

			public Vector3D m_animationLastPosition = Vector3D.Zero;

			public float m_animationProgress = 1f;

			public Quaternion LocalOrientation => Quaternion.CreateFromRotationMatrix(m_localMatrixAdd);

			public void Clear()
			{
				m_startBuild = null;
				m_startRemove = null;
				m_removeBlock = null;
				m_blockIdInCompound = null;
				m_positions.Clear();
				m_cubeNormals.Clear();
				m_patternOffsets.Clear();
				m_cubeModels.Clear();
				m_cubeMatrices.Clear();
				m_cubeModelsTemp.Clear();
				m_cubeMatricesTemp.Clear();
				m_mirroringOffset = Vector3I.Zero;
				m_addPosSmallOnLarge = null;
				m_positionsSmallOnLarge.Clear();
				m_dynamicBuildAllowed = false;
				m_removeBlocksInMultiBlock.Clear();
			}
		}

		private MyGizmoSpaceProperties[] m_spaces = new MyGizmoSpaceProperties[8];

		public MyRotationOptionsEnum RotationOptions;

		public static MySymmetryAxisEnum CurrentBlockMirrorAxis;

		public static MySymmetryAxisEnum CurrentBlockMirrorOption;

		public MyGizmoSpaceProperties SpaceDefault => m_spaces[0];

		public MyGizmoSpaceProperties[] Spaces => m_spaces;

		public MyCubeBuilderGizmo()
		{
			for (int i = 0; i < 8; i++)
			{
				m_spaces[i] = new MyGizmoSpaceProperties();
			}
			m_spaces[0].Enabled = true;
			m_spaces[1].SourceSpace = MyGizmoSpaceEnum.Default;
			m_spaces[1].SymmetryPlane = MySymmetrySettingModeEnum.NoPlane;
			m_spaces[1].SourceSpace = MyGizmoSpaceEnum.Default;
			m_spaces[1].SymmetryPlane = MySymmetrySettingModeEnum.XPlane;
			m_spaces[2].SourceSpace = MyGizmoSpaceEnum.Default;
			m_spaces[2].SymmetryPlane = MySymmetrySettingModeEnum.YPlane;
			m_spaces[3].SourceSpace = MyGizmoSpaceEnum.Default;
			m_spaces[3].SymmetryPlane = MySymmetrySettingModeEnum.ZPlane;
			m_spaces[4].SourceSpace = MyGizmoSpaceEnum.SymmetryX;
			m_spaces[4].SymmetryPlane = MySymmetrySettingModeEnum.YPlane;
			m_spaces[5].SourceSpace = MyGizmoSpaceEnum.SymmetryY;
			m_spaces[5].SymmetryPlane = MySymmetrySettingModeEnum.ZPlane;
			m_spaces[6].SourceSpace = MyGizmoSpaceEnum.SymmetryX;
			m_spaces[6].SymmetryPlane = MySymmetrySettingModeEnum.ZPlane;
			m_spaces[7].SourceSpace = MyGizmoSpaceEnum.SymmetryXZ;
			m_spaces[7].SymmetryPlane = MySymmetrySettingModeEnum.YPlane;
		}

		public void Clear()
		{
			MyGizmoSpaceProperties[] spaces = m_spaces;
			for (int i = 0; i < spaces.Length; i++)
			{
				spaces[i].Clear();
			}
		}

		public void RotateAxis(ref MatrixD rotatedMatrix)
		{
			SpaceDefault.m_localMatrixAdd = rotatedMatrix;
			SpaceDefault.m_localMatrixAdd.Forward = Vector3I.Round(SpaceDefault.m_localMatrixAdd.Forward);
			SpaceDefault.m_localMatrixAdd.Up = Vector3I.Round(SpaceDefault.m_localMatrixAdd.Up);
			SpaceDefault.m_localMatrixAdd.Right = Vector3I.Round(SpaceDefault.m_localMatrixAdd.Right);
		}

		public void SetupLocalAddMatrix(MyGizmoSpaceProperties gizmoSpace, Vector3I normal)
		{
			Vector3I value = -normal;
			Matrix matrix = Matrix.CreateWorld(Vector3.Zero, value, Vector3I.Shift(value));
			Matrix matrix2 = Matrix.Invert(matrix);
			Vector3I vector3I = Vector3I.Round((matrix * gizmoSpace.m_localMatrixAdd).Up);
			if (vector3I == gizmoSpace.m_addDir || vector3I == -gizmoSpace.m_addDir)
			{
				vector3I = Vector3I.Shift(vector3I);
			}
			Matrix matrix3 = Matrix.CreateWorld(Vector3.Zero, gizmoSpace.m_addDir, vector3I);
			gizmoSpace.m_localMatrixAdd = matrix2 * matrix3;
		}

		public void UpdateGizmoCubeParts(MyGizmoSpaceProperties gizmoSpace, MyBlockBuilderRenderData renderData, ref MatrixD invGridWorldMatrix, MyCubeBlockDefinition definition = null)
		{
			RemoveGizmoCubeParts(gizmoSpace);
			AddGizmoCubeParts(gizmoSpace, renderData, ref invGridWorldMatrix, definition);
		}

		private void AddGizmoCubeParts(MyGizmoSpaceProperties gizmoSpace, MyBlockBuilderRenderData renderData, ref MatrixD invGridWorldMatrix, MyCubeBlockDefinition definition)
		{
			Vector3UByte[] array = null;
			MyTileDefinition[] array2 = null;
			MatrixD orientation = invGridWorldMatrix.GetOrientation();
			float num = 1f;
			if (definition != null && definition.Skeleton != null)
			{
				array2 = MyCubeGridDefinitions.GetCubeTiles(definition);
				num = MyDefinitionManager.Static.GetCubeSize(definition.CubeSize);
			}
			for (int i = 0; i < gizmoSpace.m_cubeModelsTemp.Count; i++)
			{
				string text = gizmoSpace.m_cubeModelsTemp[i];
				gizmoSpace.m_cubeModels.Add(text);
				gizmoSpace.m_cubeMatrices.Add(gizmoSpace.m_cubeMatricesTemp[i]);
				if (array2 != null)
				{
					int num2 = i % array2.Length;
					MatrixD matrix = Matrix.Transpose(array2[num2].LocalMatrix) * gizmoSpace.m_cubeMatricesTemp[i].GetOrientation() * orientation;
					array = new Vector3UByte[9];
					for (int j = 0; j < 9; j++)
					{
						array[j] = new Vector3UByte(128, 128, 128);
					}
					MyModel model = MyModels.GetModel(text);
					if (model.BoneMapping != null)
					{
						for (int k = 0; k < Math.Min(model.BoneMapping.Length, 9); k++)
						{
							Vector3I v = Vector3I.Round(Vector3.Transform(model.BoneMapping[k] - Vector3.One, array2[num2].LocalMatrix) + Vector3.One);
							for (int l = 0; l < definition.Skeleton.Count; l++)
							{
								BoneInfo boneInfo = definition.Skeleton[l];
								if (boneInfo.BonePosition == (SerializableVector3I)v)
								{
									Vector3 vec = Vector3.Transform(Vector3UByte.Denormalize(boneInfo.BoneOffset, num), matrix);
									array[k] = Vector3UByte.Normalize(vec, num);
									break;
								}
							}
						}
					}
				}
				int id = MyModel.GetId(text);
				MatrixD matrix2 = gizmoSpace.m_cubeMatricesTemp[i];
				Vector3UByte[] bones = array;
				float gridSize = num;
				renderData.AddInstance(id, matrix2, ref invGridWorldMatrix, MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), bones, gridSize);
			}
		}

		public void RemoveGizmoCubeParts()
		{
			MyGizmoSpaceProperties[] spaces = m_spaces;
			foreach (MyGizmoSpaceProperties gizmoSpace in spaces)
			{
				RemoveGizmoCubeParts(gizmoSpace);
			}
		}

		private void RemoveGizmoCubeParts(MyGizmoSpaceProperties gizmoSpace)
		{
			gizmoSpace.m_cubeMatrices.Clear();
			gizmoSpace.m_cubeModels.Clear();
		}

		public void AddFastBuildParts(MyGizmoSpaceProperties gizmoSpace, MyCubeBlockDefinition cubeBlockDefinition, MyCubeGrid grid)
		{
			if (cubeBlockDefinition == null || !gizmoSpace.m_startBuild.HasValue || !gizmoSpace.m_continueBuild.HasValue)
			{
				return;
			}
			Vector3I vector3I = Vector3I.Min(gizmoSpace.m_startBuild.Value, gizmoSpace.m_continueBuild.Value);
			Vector3I vector3I2 = Vector3I.Max(gizmoSpace.m_startBuild.Value, gizmoSpace.m_continueBuild.Value);
			Vector3I vector3I3 = default(Vector3I);
			int count = gizmoSpace.m_cubeMatricesTemp.Count;
			vector3I3.X = vector3I.X;
			while (vector3I3.X <= vector3I2.X)
			{
				vector3I3.Y = vector3I.Y;
				while (vector3I3.Y <= vector3I2.Y)
				{
					vector3I3.Z = vector3I.Z;
					while (vector3I3.Z <= vector3I2.Z)
					{
						if (!(vector3I3 - gizmoSpace.m_startBuild.Value == Vector3.Zero))
						{
							Vector3D translation = (grid != null) ? Vector3D.Transform(vector3I3 * grid.GridSize, grid.WorldMatrix) : ((Vector3D)vector3I3 * (double)MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize));
							for (int i = 0; i < count; i++)
							{
								gizmoSpace.m_cubeModelsTemp.Add(gizmoSpace.m_cubeModelsTemp[i]);
								MatrixD item = gizmoSpace.m_cubeMatricesTemp[i];
								item.Translation = translation;
								gizmoSpace.m_cubeMatricesTemp.Add(item);
							}
						}
						vector3I3.Z += cubeBlockDefinition.Size.Z;
					}
					vector3I3.Y += cubeBlockDefinition.Size.Y;
				}
				vector3I3.X += cubeBlockDefinition.Size.X;
			}
		}

		public static bool DefaultGizmoCloseEnough(ref MatrixD invGridWorldMatrix, BoundingBoxD gizmoBox, float gridSize, float intersectionDistance)
		{
			MatrixD matrix = invGridWorldMatrix;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return false;
			}
			Vector3D translation = localCharacter.GetHeadMatrix(includeY: true).Translation;
			Vector3D position = MySector.MainCamera.Position;
			Vector3 forwardVector = MySector.MainCamera.ForwardVector;
			double num = (translation - MySector.MainCamera.Position).Length();
			Vector3 v = Vector3D.Transform(translation, matrix);
			Vector3 v2 = Vector3D.Transform(position, matrix);
			Vector3 v3 = Vector3D.Transform(position + forwardVector * (intersectionDistance + (float)num), matrix);
			LineD line = new LineD(v2, v3);
			float num2 = 0.025f * gridSize;
			gizmoBox.Inflate(num2);
			double distance = double.MaxValue;
			if (gizmoBox.Intersects(ref line, out distance))
			{
				double num3 = gizmoBox.Distance(v);
				if (MySession.Static.ControlledEntity is MyShipController)
				{
					if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition.CubeSize == MyCubeSize.Large)
					{
						return num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistLargeSurvivalShip;
					}
					return num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistSmallSurvivalShip;
				}
				if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition.CubeSize == MyCubeSize.Large)
				{
					return num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistLargeSurvivalCharacter;
				}
				return num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistSmallSurvivalCharacter;
			}
			return false;
		}

		private void GetGizmoPointTestVariables(ref MatrixD invGridWorldMatrix, float gridSize, out BoundingBoxD bb, out MatrixD m, MyGizmoSpaceEnum gizmo, float inflate = 0f, bool onVoxel = false, bool dynamicMode = false)
		{
			m = invGridWorldMatrix * MatrixD.CreateScale(1f / gridSize);
			MyGizmoSpaceProperties myGizmoSpaceProperties = m_spaces[(int)gizmo];
			if (dynamicMode)
			{
				m = invGridWorldMatrix;
				bb = new BoundingBoxD(-myGizmoSpaceProperties.m_blockDefinition.Size * gridSize * 0.5f, myGizmoSpaceProperties.m_blockDefinition.Size * gridSize * 0.5f);
			}
			else if (onVoxel)
			{
				m = invGridWorldMatrix;
				Vector3D value = MyCubeGrid.StaticGlobalGrid_UGToWorld(myGizmoSpaceProperties.m_min, gridSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter) - Vector3D.Half * gridSize;
				Vector3D value2 = MyCubeGrid.StaticGlobalGrid_UGToWorld(myGizmoSpaceProperties.m_max, gridSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter) + Vector3D.Half * gridSize;
				bb = new BoundingBoxD(value - new Vector3D(inflate * gridSize), value2 + new Vector3D(inflate * gridSize));
			}
			else if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && myGizmoSpaceProperties.m_addPosSmallOnLarge.HasValue)
			{
				float num = MyDefinitionManager.Static.GetCubeSize(myGizmoSpaceProperties.m_blockDefinition.CubeSize) / gridSize;
				Vector3 value3 = myGizmoSpaceProperties.m_minSmallOnLarge - new Vector3(0.5f * num);
				Vector3 value4 = myGizmoSpaceProperties.m_maxSmallOnLarge + new Vector3(0.5f * num);
				bb = new BoundingBoxD(value3 - new Vector3(inflate), value4 + new Vector3(inflate));
			}
			else
			{
				Vector3 value5 = myGizmoSpaceProperties.m_min - new Vector3(0.5f);
				Vector3 value6 = myGizmoSpaceProperties.m_max + new Vector3(0.5f);
				bb = new BoundingBoxD(value5 - new Vector3(inflate), value6 + new Vector3(inflate));
			}
		}

		public bool PointsAABBIntersectsGizmo(List<Vector3D> points, MyGizmoSpaceEnum gizmo, ref MatrixD invGridWorldMatrix, float gridSize, float inflate = 0f, bool onVoxel = false, bool dynamicMode = false)
		{
			MatrixD m = default(MatrixD);
			BoundingBoxD bb = default(BoundingBoxD);
			GetGizmoPointTestVariables(ref invGridWorldMatrix, gridSize, out bb, out m, gizmo, inflate, onVoxel, dynamicMode);
			BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
			foreach (Vector3D point2 in points)
			{
				Vector3D point = Vector3D.Transform(point2, m);
				if (bb.Contains(point) == ContainmentType.Contains)
				{
					return true;
				}
				boundingBoxD.Include(point);
			}
			return boundingBoxD.Intersects(ref bb);
		}

		public bool PointInsideGizmo(Vector3D point, MyGizmoSpaceEnum gizmo, ref MatrixD invGridWorldMatrix, float gridSize, float inflate = 0f, bool onVoxel = false, bool dynamicMode = false)
		{
			MatrixD m = default(MatrixD);
			BoundingBoxD bb = default(BoundingBoxD);
			GetGizmoPointTestVariables(ref invGridWorldMatrix, gridSize, out bb, out m, gizmo, inflate, onVoxel, dynamicMode);
			Vector3D point2 = Vector3D.Transform(point, m);
			return bb.Contains(point2) == ContainmentType.Contains;
		}

		private void EnableGizmoSpace(MyGizmoSpaceEnum gizmoSpaceEnum, bool enable, Vector3I? planePos, bool isOdd, MyCubeBlockDefinition cubeBlockDefinition, MyCubeGrid cubeGrid)
		{
			MyGizmoSpaceProperties myGizmoSpaceProperties = m_spaces[(int)gizmoSpaceEnum];
			myGizmoSpaceProperties.Enabled = enable;
			if (!enable)
			{
				return;
			}
			if (planePos.HasValue)
			{
				myGizmoSpaceProperties.SymmetryPlanePos = planePos.Value;
			}
			myGizmoSpaceProperties.SymmetryIsOdd = isOdd;
			myGizmoSpaceProperties.m_buildAllowed = false;
			if (cubeBlockDefinition != null)
			{
				Quaternion q = myGizmoSpaceProperties.LocalOrientation;
				MyBlockOrientation value = new MyBlockOrientation(ref q);
				MyCubeGridDefinitions.GetRotatedBlockSize(cubeBlockDefinition, ref myGizmoSpaceProperties.m_localMatrixAdd, out Vector3I size);
				Vector3I normal = cubeBlockDefinition.Center;
				Vector3I.TransformNormal(ref normal, ref myGizmoSpaceProperties.m_localMatrixAdd, out Vector3I result);
				Vector3I b = new Vector3I((Math.Sign(size.X) == Math.Sign(myGizmoSpaceProperties.m_addDir.X)) ? result.X : (Math.Sign(myGizmoSpaceProperties.m_addDir.X) * (Math.Abs(size.X) - Math.Abs(result.X) - 1)), (Math.Sign(size.Y) == Math.Sign(myGizmoSpaceProperties.m_addDir.Y)) ? result.Y : (Math.Sign(myGizmoSpaceProperties.m_addDir.Y) * (Math.Abs(size.Y) - Math.Abs(result.Y) - 1)), (Math.Sign(size.Z) == Math.Sign(myGizmoSpaceProperties.m_addDir.Z)) ? result.Z : (Math.Sign(myGizmoSpaceProperties.m_addDir.Z) * (Math.Abs(size.Z) - Math.Abs(result.Z) - 1)));
				myGizmoSpaceProperties.m_positions.Clear();
				myGizmoSpaceProperties.m_positionsSmallOnLarge.Clear();
				if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && myGizmoSpaceProperties.m_addPosSmallOnLarge.HasValue)
				{
					float num = MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize) / cubeGrid.GridSize;
					myGizmoSpaceProperties.m_minSmallOnLarge = Vector3.MaxValue;
					myGizmoSpaceProperties.m_maxSmallOnLarge = Vector3.MinValue;
					myGizmoSpaceProperties.m_centerPosSmallOnLarge = myGizmoSpaceProperties.m_addPosSmallOnLarge.Value + num * b;
					myGizmoSpaceProperties.m_buildAllowed = true;
					Vector3I a = default(Vector3I);
					a.X = 0;
					while (a.X < cubeBlockDefinition.Size.X)
					{
						a.Y = 0;
						while (a.Y < cubeBlockDefinition.Size.Y)
						{
							a.Z = 0;
							while (a.Z < cubeBlockDefinition.Size.Z)
							{
								Vector3I normal2 = a - normal;
								Vector3I.TransformNormal(ref normal2, ref myGizmoSpaceProperties.m_localMatrixAdd, out Vector3I result2);
								Vector3 vector = myGizmoSpaceProperties.m_addPosSmallOnLarge.Value + num * (result2 + b);
								myGizmoSpaceProperties.m_minSmallOnLarge = Vector3.Min(vector, myGizmoSpaceProperties.m_minSmallOnLarge);
								myGizmoSpaceProperties.m_maxSmallOnLarge = Vector3.Max(vector, myGizmoSpaceProperties.m_maxSmallOnLarge);
								myGizmoSpaceProperties.m_positionsSmallOnLarge.Add(vector);
								a.Z++;
							}
							a.Y++;
						}
						a.X++;
					}
				}
				else
				{
					myGizmoSpaceProperties.m_min = Vector3I.MaxValue;
					myGizmoSpaceProperties.m_max = Vector3I.MinValue;
					myGizmoSpaceProperties.m_centerPos = myGizmoSpaceProperties.m_addPos + b;
					myGizmoSpaceProperties.m_buildAllowed = true;
					Vector3I a2 = default(Vector3I);
					a2.X = 0;
					while (a2.X < cubeBlockDefinition.Size.X)
					{
						a2.Y = 0;
						while (a2.Y < cubeBlockDefinition.Size.Y)
						{
							a2.Z = 0;
							while (a2.Z < cubeBlockDefinition.Size.Z)
							{
								Vector3I normal3 = a2 - normal;
								Vector3I.TransformNormal(ref normal3, ref myGizmoSpaceProperties.m_localMatrixAdd, out Vector3I result3);
								Vector3I vector3I = myGizmoSpaceProperties.m_addPos + result3 + b;
								myGizmoSpaceProperties.m_min = Vector3I.Min(vector3I, myGizmoSpaceProperties.m_min);
								myGizmoSpaceProperties.m_max = Vector3I.Max(vector3I, myGizmoSpaceProperties.m_max);
								if (cubeGrid != null && cubeBlockDefinition.CubeSize == cubeGrid.GridSizeEnum && !cubeGrid.CanAddCube(vector3I, value, cubeBlockDefinition))
								{
									myGizmoSpaceProperties.m_buildAllowed = false;
								}
								myGizmoSpaceProperties.m_positions.Add(vector3I);
								a2.Z++;
							}
							a2.Y++;
						}
						a2.X++;
					}
				}
			}
			if (myGizmoSpaceProperties.SymmetryPlane != 0)
			{
				MirrorGizmoSpace(myGizmoSpaceProperties, m_spaces[(int)myGizmoSpaceProperties.SourceSpace], myGizmoSpaceProperties.SymmetryPlane, planePos.Value, isOdd, cubeBlockDefinition, cubeGrid);
			}
		}

		public void EnableGizmoSpaces(MyCubeBlockDefinition cubeBlockDefinition, MyCubeGrid cubeGrid, bool useSymmetry)
		{
			EnableGizmoSpace(MyGizmoSpaceEnum.Default, enable: true, null, isOdd: false, cubeBlockDefinition, cubeGrid);
			if (cubeGrid != null)
			{
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryX, useSymmetry && cubeGrid.XSymmetryPlane.HasValue, cubeGrid.XSymmetryPlane, cubeGrid.XSymmetryOdd, cubeBlockDefinition, cubeGrid);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryY, useSymmetry && cubeGrid.YSymmetryPlane.HasValue, cubeGrid.YSymmetryPlane, cubeGrid.YSymmetryOdd, cubeBlockDefinition, cubeGrid);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryZ, useSymmetry && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.ZSymmetryPlane, cubeGrid.ZSymmetryOdd, cubeBlockDefinition, cubeGrid);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXY, useSymmetry && cubeGrid.XSymmetryPlane.HasValue && cubeGrid.YSymmetryPlane.HasValue, cubeGrid.YSymmetryPlane, cubeGrid.YSymmetryOdd, cubeBlockDefinition, cubeGrid);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryYZ, useSymmetry && cubeGrid.YSymmetryPlane.HasValue && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.ZSymmetryPlane, cubeGrid.ZSymmetryOdd, cubeBlockDefinition, cubeGrid);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXZ, useSymmetry && cubeGrid.XSymmetryPlane.HasValue && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.ZSymmetryPlane, cubeGrid.ZSymmetryOdd, cubeBlockDefinition, cubeGrid);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXYZ, useSymmetry && cubeGrid.XSymmetryPlane.HasValue && cubeGrid.YSymmetryPlane.HasValue && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.YSymmetryPlane, cubeGrid.YSymmetryOdd, cubeBlockDefinition, cubeGrid);
			}
			else
			{
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryX, enable: false, null, isOdd: false, cubeBlockDefinition, null);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryY, enable: false, null, isOdd: false, cubeBlockDefinition, null);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryZ, enable: false, null, isOdd: false, cubeBlockDefinition, null);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXY, enable: false, null, isOdd: false, cubeBlockDefinition, null);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryYZ, enable: false, null, isOdd: false, cubeBlockDefinition, null);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXZ, enable: false, null, isOdd: false, cubeBlockDefinition, null);
				EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXYZ, enable: false, null, isOdd: false, cubeBlockDefinition, null);
			}
		}

		private static Vector3I MirrorBlockByPlane(MySymmetrySettingModeEnum mirror, Vector3I mirrorPosition, bool isOdd, Vector3I sourcePosition)
		{
			Vector3I result = sourcePosition;
			if (mirror == MySymmetrySettingModeEnum.XPlane)
			{
				result = new Vector3I(mirrorPosition.X - (sourcePosition.X - mirrorPosition.X), sourcePosition.Y, sourcePosition.Z);
				if (isOdd)
				{
					result.X--;
				}
			}
			if (mirror == MySymmetrySettingModeEnum.YPlane)
			{
				result = new Vector3I(sourcePosition.X, mirrorPosition.Y - (sourcePosition.Y - mirrorPosition.Y), sourcePosition.Z);
				if (isOdd)
				{
					result.Y--;
				}
			}
			if (mirror == MySymmetrySettingModeEnum.ZPlane)
			{
				result = new Vector3I(sourcePosition.X, sourcePosition.Y, mirrorPosition.Z - (sourcePosition.Z - mirrorPosition.Z));
				if (isOdd)
				{
					result.Z++;
				}
			}
			return result;
		}

		private static Vector3I MirrorDirByPlane(MySymmetrySettingModeEnum mirror, Vector3I mirrorDir, bool isOdd, Vector3I sourceDir)
		{
			Vector3I result = sourceDir;
			if (mirror == MySymmetrySettingModeEnum.XPlane)
			{
				result = new Vector3I(-sourceDir.X, sourceDir.Y, sourceDir.Z);
			}
			if (mirror == MySymmetrySettingModeEnum.YPlane)
			{
				result = new Vector3I(sourceDir.X, -sourceDir.Y, sourceDir.Z);
			}
			if (mirror == MySymmetrySettingModeEnum.ZPlane)
			{
				result = new Vector3I(sourceDir.X, sourceDir.Y, -sourceDir.Z);
			}
			return result;
		}

		private void MirrorGizmoSpace(MyGizmoSpaceProperties targetSpace, MyGizmoSpaceProperties sourceSpace, MySymmetrySettingModeEnum mirrorPlane, Vector3I mirrorPosition, bool isOdd, MyCubeBlockDefinition cubeBlockDefinition, MyCubeGrid cubeGrid)
		{
			targetSpace.m_addPos = MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_addPos);
			targetSpace.m_localMatrixAdd.Translation = targetSpace.m_addPos;
			targetSpace.m_addDir = MirrorDirByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_addDir);
			targetSpace.m_removePos = MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_removePos);
			targetSpace.m_removeBlock = cubeGrid.GetCubeBlock(targetSpace.m_removePos);
			if (sourceSpace.m_startBuild.HasValue)
			{
				targetSpace.m_startBuild = MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_startBuild.Value);
			}
			else
			{
				targetSpace.m_startBuild = null;
			}
			if (sourceSpace.m_continueBuild.HasValue)
			{
				targetSpace.m_continueBuild = MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_continueBuild.Value);
			}
			else
			{
				targetSpace.m_continueBuild = null;
			}
			if (sourceSpace.m_startRemove.HasValue)
			{
				targetSpace.m_startRemove = MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_startRemove.Value);
			}
			else
			{
				targetSpace.m_startRemove = null;
			}
			Vector3 vector = Vector3.Zero;
			switch (mirrorPlane)
			{
			case MySymmetrySettingModeEnum.XPlane:
				vector = Vector3.Right;
				break;
			case MySymmetrySettingModeEnum.YPlane:
				vector = Vector3.Up;
				break;
			case MySymmetrySettingModeEnum.ZPlane:
				vector = Vector3.Forward;
				break;
			}
			CurrentBlockMirrorAxis = MySymmetryAxisEnum.None;
			if (MyUtils.IsZero(Math.Abs(Vector3.Dot(sourceSpace.m_localMatrixAdd.Right, vector)) - 1f))
			{
				CurrentBlockMirrorAxis = MySymmetryAxisEnum.X;
			}
			else if (MyUtils.IsZero(Math.Abs(Vector3.Dot(sourceSpace.m_localMatrixAdd.Up, vector)) - 1f))
			{
				CurrentBlockMirrorAxis = MySymmetryAxisEnum.Y;
			}
			else if (MyUtils.IsZero(Math.Abs(Vector3.Dot(sourceSpace.m_localMatrixAdd.Forward, vector)) - 1f))
			{
				CurrentBlockMirrorAxis = MySymmetryAxisEnum.Z;
			}
			CurrentBlockMirrorOption = MySymmetryAxisEnum.None;
			MySymmetryAxisEnum mySymmetryAxisEnum = MyGuiScreenDebugCubeBlocks.DebugXMirroringAxis.HasValue ? MyGuiScreenDebugCubeBlocks.DebugXMirroringAxis.Value : cubeBlockDefinition.SymmetryX;
			MySymmetryAxisEnum mySymmetryAxisEnum2 = MyGuiScreenDebugCubeBlocks.DebugYMirroringAxis.HasValue ? MyGuiScreenDebugCubeBlocks.DebugYMirroringAxis.Value : cubeBlockDefinition.SymmetryY;
			MySymmetryAxisEnum mySymmetryAxisEnum3 = MyGuiScreenDebugCubeBlocks.DebugZMirroringAxis.HasValue ? MyGuiScreenDebugCubeBlocks.DebugZMirroringAxis.Value : cubeBlockDefinition.SymmetryZ;
			switch (CurrentBlockMirrorAxis)
			{
			case MySymmetryAxisEnum.X:
				CurrentBlockMirrorOption = mySymmetryAxisEnum;
				break;
			case MySymmetryAxisEnum.Y:
				CurrentBlockMirrorOption = mySymmetryAxisEnum2;
				break;
			case MySymmetryAxisEnum.Z:
				CurrentBlockMirrorOption = mySymmetryAxisEnum3;
				break;
			}
			switch (CurrentBlockMirrorOption)
			{
			case MySymmetryAxisEnum.X:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.Y:
			case MySymmetryAxisEnum.YThenOffsetX:
			case MySymmetryAxisEnum.YThenOffsetXOdd:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.Z:
			case MySymmetryAxisEnum.ZThenOffsetX:
			case MySymmetryAxisEnum.ZThenOffsetXOdd:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.OffsetX:
			case MySymmetryAxisEnum.OffsetXOddTest:
				targetSpace.m_localMatrixAdd = sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.HalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * -449f / 777f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.HalfY:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(MathF.E * -449f / 777f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.HalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.XHalfY:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.YHalfY:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.ZHalfY:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.XHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * -449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.YHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * -449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.ZHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * -449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.XHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.YHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.ZHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * -449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.XMinusHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.YMinusHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.ZMinusHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.XMinusHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.YMinusHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.ZMinusHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * 449f / 777f) * targetSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.MinusHalfX:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(MathF.E * 449f / 777f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.MinusHalfY:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(MathF.E * 449f / 777f) * sourceSpace.m_localMatrixAdd;
				break;
			case MySymmetryAxisEnum.MinusHalfZ:
				targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(MathF.E * 449f / 777f) * sourceSpace.m_localMatrixAdd;
				break;
			default:
				targetSpace.m_localMatrixAdd = sourceSpace.m_localMatrixAdd;
				break;
			}
			if (!string.IsNullOrEmpty(sourceSpace.m_blockDefinition.MirroringBlock))
			{
				targetSpace.m_blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId(sourceSpace.m_blockDefinition.Id.TypeId, sourceSpace.m_blockDefinition.MirroringBlock));
			}
			else
			{
				targetSpace.m_blockDefinition = sourceSpace.m_blockDefinition;
			}
			if (mySymmetryAxisEnum == MySymmetryAxisEnum.None && mySymmetryAxisEnum2 == MySymmetryAxisEnum.None && mySymmetryAxisEnum3 == MySymmetryAxisEnum.None)
			{
				Vector3 min = sourceSpace.m_min * cubeGrid.GridSize - new Vector3(cubeGrid.GridSize / 2f);
				Vector3 max = sourceSpace.m_max * cubeGrid.GridSize + new Vector3(cubeGrid.GridSize / 2f);
				BoundingBox boundingBox = new BoundingBox(min, max);
				if (boundingBox.Size.X > 1f * cubeGrid.GridSize || boundingBox.Size.Y > 1f * cubeGrid.GridSize || boundingBox.Size.Z > 1f * cubeGrid.GridSize)
				{
					Vector3 vector2 = sourceSpace.m_addPos * cubeGrid.GridSize;
					Vector3D value = Vector3D.Transform(vector2, cubeGrid.WorldMatrix);
					Vector3 vector3 = (mirrorPosition - sourceSpace.m_addPos) * cubeGrid.GridSize;
					if (isOdd)
					{
						vector3.X -= cubeGrid.GridSize / 2f;
						vector3.Y -= cubeGrid.GridSize / 2f;
						vector3.Z += cubeGrid.GridSize / 2f;
					}
					Vector3 vector4 = vector3;
					Vector3 vector5 = Vector3.Clamp(vector2 + vector3, boundingBox.Min, boundingBox.Max) - vector2;
					Vector3 vector6 = Vector3.Clamp(vector2 + vector3 * 100f, boundingBox.Min, boundingBox.Max) - vector2;
					Vector3 vector7 = Vector3.Clamp(vector2 - vector3 * 100f, boundingBox.Min, boundingBox.Max) - vector2;
					switch (mirrorPlane)
					{
					case MySymmetrySettingModeEnum.XPlane:
					case MySymmetrySettingModeEnum.XPlaneOdd:
						vector7.Y = 0f;
						vector7.Z = 0f;
						vector5.Y = 0f;
						vector5.Z = 0f;
						vector4.Y = 0f;
						vector4.Z = 0f;
						vector6.Y = 0f;
						vector6.Z = 0f;
						break;
					case MySymmetrySettingModeEnum.YPlane:
					case MySymmetrySettingModeEnum.YPlaneOdd:
						vector7.X = 0f;
						vector7.Z = 0f;
						vector5.X = 0f;
						vector5.Z = 0f;
						vector4.X = 0f;
						vector4.Z = 0f;
						vector6.X = 0f;
						vector6.Z = 0f;
						break;
					case MySymmetrySettingModeEnum.ZPlane:
					case MySymmetrySettingModeEnum.ZPlaneOdd:
						vector7.Y = 0f;
						vector7.X = 0f;
						vector5.Y = 0f;
						vector5.X = 0f;
						vector4.Y = 0f;
						vector4.X = 0f;
						vector6.Y = 0f;
						vector6.X = 0f;
						break;
					}
					Vector3 vector8 = vector4 - vector5;
					Vector3D.TransformNormal(vector5, cubeGrid.WorldMatrix);
					Vector3D value2 = Vector3D.TransformNormal(vector4, cubeGrid.WorldMatrix);
					Vector3D.TransformNormal(vector7, cubeGrid.WorldMatrix);
					bool flag = false;
					if (vector4.LengthSquared() < vector6.LengthSquared())
					{
						flag = true;
					}
					Vector3 vector9 = -vector7;
					_ = (Vector3D)Vector3.TransformNormal(vector8, cubeGrid.WorldMatrix);
					_ = (Vector3D)Vector3.TransformNormal(vector9, cubeGrid.WorldMatrix);
					_ = value + value2;
					Vector3 vector10 = vector8 + vector9;
					Vector3D.TransformNormal(vector10, cubeGrid.WorldMatrix);
					Vector3 vector11 = sourceSpace.m_addPos + (vector4 + vector10) / cubeGrid.GridSize;
					if (!flag)
					{
						Vector3D.TransformNormal(vector11, cubeGrid.WorldMatrix);
						Vector3 xyz = vector11;
						targetSpace.m_mirroringOffset = new Vector3I(xyz) - targetSpace.m_addPos;
						targetSpace.m_addPos += targetSpace.m_mirroringOffset;
						targetSpace.m_addDir = sourceSpace.m_addDir;
						targetSpace.m_localMatrixAdd.Translation = targetSpace.m_addPos;
						if (targetSpace.m_startBuild.HasValue)
						{
							targetSpace.m_startBuild += targetSpace.m_mirroringOffset;
						}
					}
					else
					{
						targetSpace.m_mirroringOffset = Vector3I.Zero;
						targetSpace.m_addPos = sourceSpace.m_addPos;
						targetSpace.m_removePos = sourceSpace.m_removePos;
						targetSpace.m_removeBlock = cubeGrid.GetCubeBlock(sourceSpace.m_removePos);
					}
				}
			}
			Vector3I vector3I = Vector3I.Zero;
			switch (CurrentBlockMirrorOption)
			{
			case MySymmetryAxisEnum.ZThenOffsetX:
			case MySymmetryAxisEnum.YThenOffsetX:
			case MySymmetryAxisEnum.OffsetX:
				vector3I = new Vector3I(targetSpace.m_localMatrixAdd.Left);
				break;
			case MySymmetryAxisEnum.YThenOffsetXOdd:
			{
				Vector3 vector13 = Vector3.Left;
				switch (mirrorPlane)
				{
				case MySymmetrySettingModeEnum.XPlane:
					vector13 = Vector3.Up;
					break;
				case MySymmetrySettingModeEnum.YPlane:
					vector13 = Vector3.Forward;
					break;
				case MySymmetrySettingModeEnum.ZPlane:
					vector13 = Vector3.Left;
					break;
				}
				if (Math.Abs(Vector3.Dot(targetSpace.m_localMatrixAdd.Left, vector13)) > 0.9f)
				{
					vector3I = Vector3I.Round(targetSpace.m_localMatrixAdd.Left);
				}
				break;
			}
			case MySymmetryAxisEnum.ZThenOffsetXOdd:
			{
				Vector3 vector14 = Vector3.Left;
				switch (mirrorPlane)
				{
				case MySymmetrySettingModeEnum.XPlane:
					vector14 = Vector3.Up;
					break;
				case MySymmetrySettingModeEnum.YPlane:
					vector14 = Vector3.Forward;
					break;
				case MySymmetrySettingModeEnum.ZPlane:
					vector14 = Vector3.Left;
					break;
				}
				if (Math.Abs(Vector3.Dot(targetSpace.m_localMatrixAdd.Left, vector14)) > 0.9f)
				{
					vector3I = new Vector3I(targetSpace.m_localMatrixAdd.Left);
				}
				break;
			}
			case MySymmetryAxisEnum.OffsetXOddTest:
			{
				Vector3 vector12 = Vector3.Left;
				switch (CurrentBlockMirrorAxis)
				{
				case MySymmetryAxisEnum.X:
					vector12 = Vector3.Forward;
					break;
				case MySymmetryAxisEnum.Y:
					vector12 = Vector3.Up;
					break;
				case MySymmetryAxisEnum.Z:
					vector12 = Vector3.Left;
					break;
				}
				if (Math.Abs(Vector3.Dot(targetSpace.m_localMatrixAdd.Left, vector12)) > 0.9f)
				{
					vector3I = Vector3I.Round(targetSpace.m_localMatrixAdd.Left);
				}
				break;
			}
			}
			MySymmetryAxisEnum currentBlockMirrorOption = CurrentBlockMirrorOption;
			if (currentBlockMirrorOption == MySymmetryAxisEnum.None || (uint)(currentBlockMirrorOption - 25) <= 5u)
			{
				targetSpace.m_mirroringOffset = vector3I;
				targetSpace.m_addPos += targetSpace.m_mirroringOffset;
				targetSpace.m_removePos += targetSpace.m_mirroringOffset;
				targetSpace.m_removeBlock = cubeGrid.GetCubeBlock(targetSpace.m_removePos);
				targetSpace.m_localMatrixAdd.Translation += (Vector3)vector3I;
			}
			targetSpace.m_worldMatrixAdd = targetSpace.m_localMatrixAdd * cubeGrid.WorldMatrix;
		}
	}
}
