using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
	public abstract class MyAttachableTopBlockBase : MySyncedBlock, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock
	{
		private long? m_parentId;

		private MyMechanicalConnectionBlockBase m_parentBlock;

		public Vector3 WheelDummy
		{
			get;
			private set;
		}

		public MyMechanicalConnectionBlockBase Stator => m_parentBlock;

		Sandbox.ModAPI.IMyMechanicalConnectionBlock Sandbox.ModAPI.IMyAttachableTopBlock.Base => Stator;

		bool Sandbox.ModAPI.Ingame.IMyAttachableTopBlock.IsAttached => Stator != null;

		Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock Sandbox.ModAPI.Ingame.IMyAttachableTopBlock.Base => Stator;

		public virtual void Attach(MyMechanicalConnectionBlockBase parent)
		{
			m_parentBlock = parent;
		}

		public virtual void Detach(bool isWelding)
		{
			if (!isWelding)
			{
				m_parentBlock = null;
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			MyObjectBuilder_AttachableTopBlockBase myObjectBuilder_AttachableTopBlockBase = builder as MyObjectBuilder_AttachableTopBlockBase;
			if (myObjectBuilder_AttachableTopBlockBase != null)
			{
				if (!myObjectBuilder_AttachableTopBlockBase.YieldLastComponent)
				{
					SlimBlock.DisableLastComponentYield();
				}
				if (myObjectBuilder_AttachableTopBlockBase.ParentEntityId != 0L)
				{
					MyEntities.TryGetEntityById(myObjectBuilder_AttachableTopBlockBase.ParentEntityId, out MyEntity entity);
					(entity as MyMechanicalConnectionBlockBase)?.MarkForReattach();
				}
			}
			LoadDummies();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CubeBlock objectBuilderCubeBlock = base.GetObjectBuilderCubeBlock(copy);
			MyObjectBuilder_AttachableTopBlockBase myObjectBuilder_AttachableTopBlockBase = objectBuilderCubeBlock as MyObjectBuilder_AttachableTopBlockBase;
			if (myObjectBuilder_AttachableTopBlockBase != null)
			{
				myObjectBuilder_AttachableTopBlockBase.ParentEntityId = ((m_parentBlock != null) ? m_parentBlock.EntityId : 0);
				myObjectBuilder_AttachableTopBlockBase.YieldLastComponent = SlimBlock.YieldLastComponent;
			}
			return objectBuilderCubeBlock;
		}

		private void LoadDummies()
		{
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(base.BlockDefinition.Model).Dummies)
			{
				if (dummy.Key.ToLower().Contains("wheel"))
				{
					WheelDummy = (Matrix.Normalize(dummy.Value.Matrix) * base.PositionComp.LocalMatrix).Translation;
					break;
				}
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			if (Stator != null)
			{
				Stator.OnTopBlockCubeGridChanged(oldGrid);
			}
			base.OnCubeGridChanged(oldGrid);
		}
	}
}
