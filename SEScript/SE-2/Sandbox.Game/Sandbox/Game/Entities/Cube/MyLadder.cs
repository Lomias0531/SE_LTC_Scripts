using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Ladder2))]
	public class MyLadder : MyCubeBlock
	{
		private class Sandbox_Game_Entities_Cube_MyLadder_003C_003EActor : IActivator, IActivator<MyLadder>
		{
			private sealed override object CreateInstance()
			{
				return new MyLadder();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLadder CreateInstance()
			{
				return new MyLadder();
			}

			MyLadder IActivator<MyLadder>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Matrix m_detectorBox = Matrix.Identity;

		public Matrix StartMatrix
		{
			get;
			private set;
		}

		public Matrix StopMatrix
		{
			get;
			private set;
		}

		public float DistanceBetweenPoles
		{
			get;
			private set;
		}

		public event Action<MyCubeGrid> CubeGridChanged;

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			StartMatrix = Matrix.Identity;
			OnModelChange();
			AddDebugRenderComponent(new MyDebugRenderComponentLadder(this));
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.Model.Dummies.TryGetValue("astronaut", out MyModelDummy value))
			{
				StartMatrix = value.Matrix;
			}
			if (base.Model.Dummies.TryGetValue("detector_ladder_01", out value))
			{
				m_detectorBox = value.Matrix;
			}
			if (base.Model.Dummies.TryGetValue("TopLadder", out value))
			{
				StopMatrix = value.Matrix;
			}
			if (base.Model.Dummies.TryGetValue("pole_1", out value) && base.Model.Dummies.TryGetValue("pole_2", out MyModelDummy value2))
			{
				DistanceBetweenPoles = Math.Abs(value.Matrix.Translation.Y - value2.Matrix.Translation.Y);
			}
		}

		public override bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			MatrixD matrix = (MatrixD)m_detectorBox * base.PositionComp.WorldMatrix;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(matrix);
			t = null;
			double? num = myOrientedBoundingBoxD.Intersects(ref line);
			if (num.HasValue)
			{
				MyIntersectionResultLineTriangleEx value = default(MyIntersectionResultLineTriangleEx);
				value.Entity = this;
				value.IntersectionPointInWorldSpace = line.From + (num.Value + 0.2) * line.Direction;
				value.IntersectionPointInObjectSpace = Vector3D.Transform(value.IntersectionPointInWorldSpace, base.PositionComp.WorldMatrixInvScaled);
				value.NormalInWorldSpace = -line.Direction;
				value.NormalInObjectSpace = Vector3D.TransformNormal(value.NormalInWorldSpace, base.PositionComp.WorldMatrixInvScaled);
				t = value;
			}
			return t.HasValue;
		}

		public void Use(UseActionEnum actionEnum, IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null && MyEntities.IsInsideWorld(myCharacter.PositionComp.GetPosition()) && MyEntities.IsInsideWorld(base.PositionComp.GetPosition()))
			{
				myCharacter.GetOnLadder(this);
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			if (this.CubeGridChanged != null)
			{
				this.CubeGridChanged(oldGrid);
			}
		}
	}
}
