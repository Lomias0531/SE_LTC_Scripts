using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	public class MyCharacterRaycastDetectorComponent : MyCharacterDetectorComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyCharacterRaycastDetectorComponent_003C_003EActor : IActivator, IActivator<MyCharacterRaycastDetectorComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterRaycastDetectorComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterRaycastDetectorComponent CreateInstance()
			{
				return new MyCharacterRaycastDetectorComponent();
			}

			MyCharacterRaycastDetectorComponent IActivator<MyCharacterRaycastDetectorComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly List<MyUseObjectsComponentBase> m_hitUseComponents = new List<MyUseObjectsComponentBase>();

		protected override void DoDetection(bool useHead)
		{
			if (base.Character == MySession.Static.ControlledEntity)
			{
				MyHud.SelectedObjectHighlight.RemoveHighlight();
			}
			MatrixD headMatrix = base.Character.GetHeadMatrix(includeY: false);
			Vector3D vector3D = headMatrix.Translation - headMatrix.Forward * 0.3;
			Vector3D forward;
			Vector3D vector3D2;
			if (!useHead)
			{
				MatrixD worldMatrix = MySector.MainCamera.WorldMatrix;
				forward = worldMatrix.Forward;
				vector3D2 = MyUtils.LinePlaneIntersection(vector3D, forward, worldMatrix.Translation, forward);
			}
			else
			{
				forward = headMatrix.Forward;
				vector3D2 = vector3D;
			}
			Vector3D vector3D3 = vector3D2 + forward * MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
			base.StartPosition = vector3D2;
			LineD line = new LineD(vector3D2, vector3D3);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, base.Character, null, ignoreChildren: false, ignoreFloatingObjects: false);
			bool flag = false;
			if (intersectionWithLine.HasValue)
			{
				IMyEntity myEntity = intersectionWithLine.Value.Entity;
				Vector3D intersectionPointInWorldSpace = intersectionWithLine.Value.IntersectionPointInWorldSpace;
				if (myEntity is MyCubeGrid && intersectionWithLine.Value.UserObject != null)
				{
					MySlimBlock cubeBlock = (intersectionWithLine.Value.UserObject as MyCube).CubeBlock;
					if (cubeBlock != null && cubeBlock.FatBlock != null)
					{
						myEntity = cubeBlock.FatBlock;
					}
				}
				m_hitUseComponents.Clear();
				IMyUseObject myUseObject = myEntity as IMyUseObject;
				GetUseComponentsFromParentStructure(myEntity, m_hitUseComponents);
				if (myUseObject != null || m_hitUseComponents.Count > 0)
				{
					if (m_hitUseComponents.Count > 0)
					{
						float value = float.MaxValue;
						double num = Vector3D.Distance(vector3D2, intersectionPointInWorldSpace);
						MyUseObjectsComponentBase myUseObjectsComponentBase = null;
						foreach (MyUseObjectsComponentBase hitUseComponent in m_hitUseComponents)
						{
							float parameter;
							IMyUseObject myUseObject2 = hitUseComponent.RaycastDetectors(vector3D2, vector3D3, out parameter);
							parameter *= MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
							if (Math.Abs(parameter) < Math.Abs(value) && (double)parameter < num)
							{
								value = parameter;
								myUseObjectsComponentBase = hitUseComponent;
								myEntity = hitUseComponent.Entity;
								myUseObject = myUseObject2;
							}
						}
						if (myUseObjectsComponentBase != null)
						{
							MyPhysicsComponentBase detectorPhysics = myUseObjectsComponentBase.DetectorPhysics;
							base.HitMaterial = detectorPhysics.GetMaterialAt(base.HitPosition);
							base.HitBody = ((intersectionWithLine.Value.Entity.Physics != null) ? intersectionWithLine.Value.Entity.Physics.RigidBody : null);
							base.HitPosition = intersectionPointInWorldSpace;
							base.DetectedEntity = myEntity;
						}
					}
					else
					{
						base.HitMaterial = myEntity.Physics.GetMaterialAt(base.HitPosition);
						base.HitBody = myEntity.Physics.RigidBody;
					}
					if (myUseObject != null)
					{
						base.HitPosition = intersectionPointInWorldSpace;
						base.DetectedEntity = myEntity;
						if (base.Character == MySession.Static.ControlledEntity && myUseObject.SupportedActions != 0 && !base.Character.IsOnLadder)
						{
							MyCharacterDetectorComponent.HandleInteractiveObject(myUseObject);
							base.UseObject = myUseObject;
							flag = true;
						}
						if (base.Character.IsOnLadder)
						{
							base.UseObject = null;
						}
					}
				}
			}
			if (!flag)
			{
				base.UseObject = null;
			}
		}

		private void GetUseComponentsFromParentStructure(IMyEntity currentEntity, List<MyUseObjectsComponentBase> useComponents)
		{
			MyUseObjectsComponentBase myUseObjectsComponentBase = currentEntity.Components.Get<MyUseObjectsComponentBase>();
			if (myUseObjectsComponentBase != null)
			{
				useComponents.Add(myUseObjectsComponentBase);
			}
			if (currentEntity.Parent != null)
			{
				GetUseComponentsFromParentStructure(currentEntity.Parent, useComponents);
			}
		}
	}
}
