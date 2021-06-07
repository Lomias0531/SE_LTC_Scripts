using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Lights;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentCharacter : MyRenderComponentSkinnedEntity
	{
		public class MyJetpackThrust
		{
			public int Bone;

			public Vector3 Forward;

			public float Offset;

			public MyLight Light;

			public float ThrustLength;

			public float ThrustRadius;

			public float ThrustThickness;

			public Matrix ThrustMatrix;

			public MyStringId ThrustPointMaterial;

			public MyStringId ThrustLengthMaterial;

			public float ThrustGlareSize;
		}

		private class Sandbox_Game_Components_MyRenderComponentCharacter_003C_003EActor : IActivator, IActivator<MyRenderComponentCharacter>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentCharacter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentCharacter CreateInstance()
			{
				return new MyRenderComponentCharacter();
			}

			MyRenderComponentCharacter IActivator<MyRenderComponentCharacter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyStringId ID_REFLECTOR_CONE = MyStringId.GetOrCompute("ReflectorConeCharacter");

		private static readonly MyStringId ID_REFLECTOR_GLARE = MyStringId.GetOrCompute("ReflectorGlareAlphaBlended");

		private static readonly MyStringHash ID_CHARACTER = MyStringHash.GetOrCompute("Character");

		private static readonly int MAX_DISCONNECT_ICON_DISTANCE = 50;

		private int m_lastWalkParticleCheckTime;

		private int m_walkParticleSpawnCounterMs = 1000;

		private const int m_walkParticleGravityDelay = 10000;

		private const int m_walkParticleJetpackOffDelay = 2000;

		private const int m_walkParticleDefaultDelay = 1000;

		private uint m_cullRenderId = uint.MaxValue;

		private List<MyJetpackThrust> m_jetpackThrusts = new List<MyJetpackThrust>(8);

		private MyLight m_light;

		private MyLight m_flareLeft;

		private MyLight m_flareRight;

		private Vector3D m_leftGlarePosition;

		private Vector3D m_rightGlarePosition;

		private int m_leftLightIndex = -1;

		private int m_rightLightIndex = -1;

		private float m_oldReflectorAngle = -1f;

		private Vector3 m_lightLocalPosition;

		private const float HIT_INDICATOR_LENGTH = 0.8f;

		private float m_currentHitIndicatorCounter;

		public static float JETPACK_LIGHT_INTENSITY_BASE = 9f;

		public static float JETPACK_LIGHT_INTENSITY_LENGTH = 200f;

		public static float JETPACK_LIGHT_RANGE_RADIUS = 1.2f;

		public static float JETPACK_LIGHT_RANGE_LENGTH = 0.3f;

		public static float JETPACK_GLARE_INTENSITY_BASE = 0.06f;

		public static float JETPACK_GLARE_INTENSITY_LENGTH = 0f;

		public static float JETPACK_GLARE_SIZE_RADIUS = 2.49f;

		public static float JETPACK_GLARE_SIZE_LENGTH = 0.4f;

		public static float JETPACK_THRUST_INTENSITY_BASE = 0.6f;

		public static float JETPACK_THRUST_INTENSITY = 10f;

		public static float JETPACK_THRUST_THICKNESS = 0.5f;

		public static float JETPACK_THRUST_LENGTH = 0.6f;

		public static float JETPACK_THRUST_OFFSET = -0.22f;

		private readonly MyFlareDefinition m_flareJetpack;

		private readonly MyFlareDefinition m_flareHeadlamp;

		public MyRenderComponentCharacter()
		{
			m_lastWalkParticleCheckTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), "Jetpack");
			MyFlareDefinition myFlareDefinition = MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition;
			m_flareJetpack = (myFlareDefinition ?? new MyFlareDefinition());
			id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), "Headlamp");
			myFlareDefinition = (MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition);
			m_flareHeadlamp = (myFlareDefinition ?? new MyFlareDefinition());
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			m_cullRenderId = MyRenderProxy.CreateManualCullObject(base.Entity.DisplayName + " ManualCullObject", base.Entity.WorldMatrix);
			SetParent(0, m_cullRenderId, Matrix.Identity);
			BoundingBox localAABB = base.Entity.LocalAABB;
			localAABB.Scale(new Vector3(1.5f, 2f, 1.5f));
			MyRenderProxy.UpdateRenderObject(GetRenderObjectID(), null, localAABB);
		}

		public override void InvalidateRenderObjects()
		{
			if (m_cullRenderId != uint.MaxValue)
			{
				MatrixD worldMatrix = base.Container.Entity.PositionComp.WorldMatrix;
				MyRenderProxy.UpdateRenderObject(m_cullRenderId, worldMatrix, null, LastMomentUpdateIndex);
			}
		}

		public override void RemoveRenderObjects()
		{
			base.RemoveRenderObjects();
			if (m_cullRenderId != uint.MaxValue)
			{
				MyRenderProxy.RemoveRenderObject(m_cullRenderId, MyRenderProxy.ObjectType.ManualCull);
			}
			m_cullRenderId = uint.MaxValue;
		}

		public void UpdateWalkParticles()
		{
			TrySpawnWalkingParticles();
		}

		internal void TrySpawnWalkingParticles(ref HkContactPointEvent value)
		{
			if (!MyFakes.ENABLE_WALKING_PARTICLES)
			{
				return;
			}
			int lastWalkParticleCheckTime = m_lastWalkParticleCheckTime;
			m_lastWalkParticleCheckTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_walkParticleSpawnCounterMs -= m_lastWalkParticleCheckTime - lastWalkParticleCheckTime;
			if (m_walkParticleSpawnCounterMs > 0)
			{
				return;
			}
			if (MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(base.Entity.PositionComp.WorldMatrix.Translation) <= 0f)
			{
				m_walkParticleSpawnCounterMs = 10000;
				return;
			}
			MyCharacter myCharacter = base.Entity as MyCharacter;
			if (myCharacter.JetpackRunning)
			{
				m_walkParticleSpawnCounterMs = 2000;
				return;
			}
			MyCharacterMovementEnum currentMovementState = myCharacter.GetCurrentMovementState();
			if (currentMovementState.GetDirection() == 0 || currentMovementState == MyCharacterMovementEnum.Falling)
			{
				m_walkParticleSpawnCounterMs = 1000;
				return;
			}
			MyVoxelPhysicsBody myVoxelPhysicsBody = value.GetOtherEntity(myCharacter).Physics as MyVoxelPhysicsBody;
			if (myVoxelPhysicsBody != null)
			{
				MyStringId type;
				switch (currentMovementState.GetSpeed())
				{
				case 0:
					type = MyMaterialPropertiesHelper.CollisionType.Walk;
					m_walkParticleSpawnCounterMs = 500;
					break;
				case 1024:
					type = MyMaterialPropertiesHelper.CollisionType.Run;
					m_walkParticleSpawnCounterMs = 275;
					break;
				case 2048:
					type = MyMaterialPropertiesHelper.CollisionType.Sprint;
					m_walkParticleSpawnCounterMs = 250;
					break;
				default:
					type = MyMaterialPropertiesHelper.CollisionType.Walk;
					m_walkParticleSpawnCounterMs = 1000;
					break;
				}
				Vector3D worldPosition = myVoxelPhysicsBody.ClusterToWorld(value.ContactPoint.Position);
				MyVoxelMaterialDefinition materialAt = myVoxelPhysicsBody.m_voxelMap.GetMaterialAt(ref worldPosition);
				if (materialAt != null)
				{
					MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(type, worldPosition, value.ContactPoint.Normal, ID_CHARACTER, materialAt.MaterialTypeNameHash, null);
				}
			}
		}

		internal void TrySpawnWalkingParticles()
		{
			if (!MyFakes.ENABLE_WALKING_PARTICLES)
			{
				return;
			}
			int lastWalkParticleCheckTime = m_lastWalkParticleCheckTime;
			m_lastWalkParticleCheckTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_walkParticleSpawnCounterMs -= m_lastWalkParticleCheckTime - lastWalkParticleCheckTime;
			if (m_walkParticleSpawnCounterMs > 0)
			{
				return;
			}
			if (MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(base.Entity.PositionComp.WorldMatrix.Translation) <= 0f)
			{
				m_walkParticleSpawnCounterMs = 10000;
				return;
			}
			MyCharacter myCharacter = base.Entity as MyCharacter;
			if (myCharacter.JetpackRunning)
			{
				m_walkParticleSpawnCounterMs = 2000;
				return;
			}
			MyCharacterMovementEnum currentMovementState = myCharacter.GetCurrentMovementState();
			if (currentMovementState.GetDirection() == 0 || currentMovementState == MyCharacterMovementEnum.Falling)
			{
				m_walkParticleSpawnCounterMs = 1000;
				return;
			}
			Vector3D up = base.Entity.PositionComp.WorldMatrix.Up;
			Vector3D vector3D = base.Entity.PositionComp.WorldMatrix.Translation + 0.2 * up;
			Vector3D to = vector3D - 0.5 * up;
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(vector3D, to, 28);
			if (!hitInfo.HasValue)
			{
				return;
			}
			MyVoxelPhysicsBody myVoxelPhysicsBody = hitInfo.Value.HkHitInfo.GetHitEntity().Physics as MyVoxelPhysicsBody;
			if (myVoxelPhysicsBody != null)
			{
				MyStringId type;
				switch (currentMovementState.GetSpeed())
				{
				case 0:
					type = MyMaterialPropertiesHelper.CollisionType.Walk;
					m_walkParticleSpawnCounterMs = 500;
					break;
				case 1024:
					type = MyMaterialPropertiesHelper.CollisionType.Run;
					m_walkParticleSpawnCounterMs = 275;
					break;
				case 2048:
					type = MyMaterialPropertiesHelper.CollisionType.Sprint;
					m_walkParticleSpawnCounterMs = 250;
					break;
				default:
					type = MyMaterialPropertiesHelper.CollisionType.Walk;
					m_walkParticleSpawnCounterMs = 1000;
					break;
				}
				Vector3D worldPosition = hitInfo.Value.Position;
				MyVoxelMaterialDefinition materialAt = myVoxelPhysicsBody.m_voxelMap.GetMaterialAt(ref worldPosition);
				if (materialAt != null)
				{
					MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(type, worldPosition, up, ID_CHARACTER, materialAt.MaterialTypeNameHash, null);
				}
			}
		}

		public override void Draw()
		{
			base.Draw();
			if (m_light == null)
			{
				return;
			}
			bool flag = true;
			MyCharacter myCharacter = m_skinnedEntity as MyCharacter;
			float num = Vector3.DistanceSquared(myCharacter.PositionComp.GetPosition(), MySector.MainCamera.Position);
			if (num < 1600f)
			{
				Vector3D position = m_light.Position;
				Vector3 reflectorDirection = m_light.ReflectorDirection;
				float num2 = 2.56f;
				float num3 = 0.48f;
				Vector3 value = new Vector3(m_light.ReflectorColor);
				Vector3D value2 = position + reflectorDirection * 0.28f;
				float num4 = Vector3.Dot(Vector3.Normalize(MySector.MainCamera.Position - value2), reflectorDirection);
				float num5 = 1f - Math.Abs(num4);
				float num6 = (1f - (float)Math.Pow(1f - num5, 30.0)) * 0.5f;
				float currentLightPower = myCharacter.CurrentLightPower;
				num6 *= currentLightPower;
				if ((myCharacter != MySession.Static.LocalCharacter || (!MySession.Static.CameraController.ForceFirstPersonCamera && !MySession.Static.CameraController.IsInFirstPersonView)) && currentLightPower > 0f && m_leftLightIndex != -1 && m_rightLightIndex != -1)
				{
					float num7 = 1296f;
					float num8 = 1f - MathHelper.Clamp((num - num7) / (1600f - num7), 0f, 1f);
					if (num2 > 0f && num3 > 0f && num8 > 0f)
					{
						MyTransparentGeometry.AddLineBillboard(ID_REFLECTOR_CONE, new Vector4(value, 1f) * num6 * num8, m_leftGlarePosition - reflectorDirection * 0.05f, reflectorDirection, num2, num3, MyBillboard.BlendTypeEnum.AdditiveBottom);
						MyTransparentGeometry.AddLineBillboard(ID_REFLECTOR_CONE, new Vector4(value, 1f) * num6 * num8, m_rightGlarePosition - reflectorDirection * 0.05f, reflectorDirection, num2, num3, MyBillboard.BlendTypeEnum.AdditiveBottom);
					}
					if (num4 > 0f)
					{
						flag = false;
						if (m_flareLeft != null)
						{
							m_flareLeft.GlareOn = true;
							m_flareLeft.Position = m_leftGlarePosition;
							m_flareLeft.ReflectorDirection = reflectorDirection;
							m_flareLeft.UpdateLight();
						}
						if (m_flareRight != null)
						{
							m_flareRight.GlareOn = true;
							m_flareRight.Position = m_rightGlarePosition;
							m_flareRight.ReflectorDirection = reflectorDirection;
							m_flareRight.UpdateLight();
						}
					}
				}
				if (MySession.Static.ControlledEntity == myCharacter || (MySession.Static.ControlledEntity is MyCockpit && ((MyCockpit)MySession.Static.ControlledEntity).Pilot == myCharacter) || (MySession.Static.ControlledEntity is MyLargeTurretBase && ((MyLargeTurretBase)MySession.Static.ControlledEntity).Pilot == myCharacter))
				{
					if (myCharacter.IsDead && myCharacter.CurrentRespawnCounter > 0f)
					{
						DrawBlood(1f);
					}
					if (!myCharacter.IsDead && m_currentHitIndicatorCounter > 0f)
					{
						m_currentHitIndicatorCounter -= 0.0166666675f;
						if (m_currentHitIndicatorCounter < 0f)
						{
							m_currentHitIndicatorCounter = 0f;
						}
						float alpha = m_currentHitIndicatorCounter / 0.8f;
						DrawBlood(alpha);
					}
					if (myCharacter.StatComp != null)
					{
						float healthRatio = myCharacter.StatComp.HealthRatio;
						if (healthRatio <= MyCharacterStatComponent.HEALTH_RATIO_CRITICAL && !myCharacter.IsDead)
						{
							float alpha2 = MathHelper.Clamp(MyCharacterStatComponent.HEALTH_RATIO_CRITICAL - healthRatio, 0f, 1f) / MyCharacterStatComponent.HEALTH_RATIO_CRITICAL + 0.3f;
							DrawBlood(alpha2);
						}
					}
				}
			}
			if (flag && m_flareRight != null && m_flareLeft.GlareOn)
			{
				m_flareLeft.GlareOn = false;
				m_flareLeft.UpdateLight();
				m_flareRight.GlareOn = false;
				m_flareRight.UpdateLight();
			}
			DrawJetpackThrusts(myCharacter.UpdateCalled());
			DrawDisconnectedIndicator();
		}

		private void DrawBlood(float alpha)
		{
			RectangleF destination = new RectangleF(0f, 0f, MyGuiManager.GetFullscreenRectangle().Width, MyGuiManager.GetFullscreenRectangle().Height);
			MyRenderProxy.DrawSprite("Textures\\Gui\\Blood.dds", ref destination, null, new Color(new Vector4(1f, 1f, 1f, alpha)), 0f);
		}

		private void DrawJetpackThrusts(bool updateCalled)
		{
			MyCharacter myCharacter = m_skinnedEntity as MyCharacter;
			if (myCharacter == null || myCharacter.GetCurrentMovementState() == MyCharacterMovementEnum.Died)
			{
				return;
			}
			MyCharacterJetpackComponent jetpackComp = myCharacter.JetpackComp;
			if (jetpackComp != null && jetpackComp.CanDrawThrusts)
			{
				MyEntityThrustComponent myEntityThrustComponent = base.Container.Get<MyEntityThrustComponent>();
				if (myEntityThrustComponent != null)
				{
					MatrixD worldToLocal = MatrixD.Invert(base.Container.Entity.PositionComp.WorldMatrix);
					foreach (MyJetpackThrust jetpackThrust in m_jetpackThrusts)
					{
						Vector3D vector3D = Vector3D.Zero;
						if (jetpackComp.TurnedOn && jetpackComp.IsPowered && (!myCharacter.IsInFirstPersonView || myCharacter != MySession.Static.LocalCharacter || MyVRage.Platform.Ansel.IsSessionRunning))
						{
							MatrixD matrix = (MatrixD)jetpackThrust.ThrustMatrix * base.Container.Entity.PositionComp.WorldMatrix;
							Vector3D vector3D2 = Vector3D.TransformNormal(jetpackThrust.Forward, matrix);
							vector3D = matrix.Translation;
							vector3D += vector3D2 * jetpackThrust.Offset;
							float num = 0.05f;
							if (updateCalled)
							{
								jetpackThrust.ThrustRadius = MyUtils.GetRandomFloat(0.9f, 1.1f) * num;
							}
							float num2 = Vector3.Dot(vector3D2, -Vector3.Transform(myEntityThrustComponent.FinalThrust, base.Entity.WorldMatrix.GetOrientation()) / myCharacter.BaseMass);
							num2 = MathHelper.Clamp(num2 * 0.09f, 0.1f, 1f);
							Vector4 zero = Vector4.Zero;
							Vector4 zero2 = Vector4.Zero;
							if (num2 > 0f && jetpackThrust.ThrustRadius > 0f)
							{
								float num3 = 1f - Math.Abs(Vector3.Dot(MyUtils.Normalize(MySector.MainCamera.Position - vector3D), vector3D2));
								float num4 = (1f - (float)Math.Pow(1f - num3, 30.0)) * 0.5f;
								if (updateCalled)
								{
									jetpackThrust.ThrustLength = num2 * 12f * MyUtils.GetRandomFloat(1.6f, 2f) * num;
									jetpackThrust.ThrustThickness = MyUtils.GetRandomFloat(jetpackThrust.ThrustRadius * 1.9f, jetpackThrust.ThrustRadius);
								}
								zero = jetpackThrust.Light.Color.ToVector4() * new Vector4(1f, 1f, 1f, 0.4f);
								MyTransparentGeometry.AddLineBillboard(jetpackThrust.ThrustLengthMaterial, zero, vector3D + vector3D2 * JETPACK_THRUST_OFFSET, GetRenderObjectID(), ref worldToLocal, vector3D2, jetpackThrust.ThrustLength * JETPACK_THRUST_LENGTH, jetpackThrust.ThrustThickness * JETPACK_THRUST_THICKNESS, MyBillboard.BlendTypeEnum.Standard, -1, num4 * (JETPACK_THRUST_INTENSITY_BASE + num2 * JETPACK_THRUST_INTENSITY));
							}
							if (jetpackThrust.ThrustRadius > 0f)
							{
								zero2 = jetpackThrust.Light.Color.ToVector4() * new Vector4(1f, 1f, 1f, 0.4f);
								MyTransparentGeometry.AddPointBillboard(jetpackThrust.ThrustPointMaterial, zero2, vector3D, GetRenderObjectID(), ref worldToLocal, jetpackThrust.ThrustRadius * JETPACK_THRUST_THICKNESS, 0f, -1, MyBillboard.BlendTypeEnum.Standard, JETPACK_THRUST_INTENSITY_BASE + num2 * JETPACK_THRUST_INTENSITY);
							}
						}
						else if (updateCalled || myCharacter.IsUsing != null)
						{
							jetpackThrust.ThrustRadius = 0f;
						}
						if (jetpackThrust.Light != null)
						{
							if (jetpackThrust.ThrustRadius > 0f)
							{
								jetpackThrust.Light.LightOn = true;
								jetpackThrust.Light.Intensity = JETPACK_LIGHT_INTENSITY_BASE + jetpackThrust.ThrustLength * JETPACK_LIGHT_INTENSITY_LENGTH;
								jetpackThrust.Light.Range = jetpackThrust.ThrustRadius * JETPACK_LIGHT_RANGE_RADIUS + jetpackThrust.ThrustLength * JETPACK_LIGHT_RANGE_LENGTH;
								jetpackThrust.Light.Position = Vector3D.Transform(vector3D, base.Container.Entity.PositionComp.WorldMatrixNormalizedInv);
								jetpackThrust.Light.ParentID = m_cullRenderId;
								jetpackThrust.Light.GlareOn = true;
								jetpackThrust.Light.GlareIntensity = (JETPACK_GLARE_INTENSITY_BASE + jetpackThrust.ThrustLength * JETPACK_GLARE_INTENSITY_LENGTH) * m_flareJetpack.Intensity;
								jetpackThrust.Light.GlareType = MyGlareTypeEnum.Normal;
								jetpackThrust.Light.GlareSize = m_flareJetpack.Size * (jetpackThrust.ThrustRadius * JETPACK_GLARE_SIZE_RADIUS + jetpackThrust.ThrustLength * JETPACK_GLARE_SIZE_LENGTH) * jetpackThrust.ThrustGlareSize;
								jetpackThrust.Light.SubGlares = m_flareJetpack.SubGlares;
								jetpackThrust.Light.GlareQuerySize = 0.1f;
								jetpackThrust.Light.UpdateLight();
							}
							else
							{
								jetpackThrust.Light.GlareOn = false;
								jetpackThrust.Light.LightOn = false;
								jetpackThrust.Light.UpdateLight();
							}
						}
					}
				}
			}
		}

		public void InitJetpackThrusts(MyCharacterDefinition definition)
		{
			m_jetpackThrusts.Clear();
			if (definition.Jetpack != null)
			{
				foreach (MyJetpackThrustDefinition thrust in definition.Jetpack.Thrusts)
				{
					if (m_skinnedEntity.AnimationController.FindBone(thrust.ThrustBone, out int index) != null)
					{
						InitJetpackThrust(index, Vector3.Forward, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
						InitJetpackThrust(index, Vector3.Left, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
						InitJetpackThrust(index, Vector3.Right, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
						InitJetpackThrust(index, Vector3.Backward, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
						InitJetpackThrust(index, Vector3.Up, thrust.FrontFlameOffset, ref definition.Jetpack.ThrustProperties);
					}
				}
			}
		}

		private void InitJetpackThrust(int bone, Vector3 forward, float offset, ref MyObjectBuilder_ThrustDefinition thrustProperties)
		{
			MyJetpackThrust myJetpackThrust = new MyJetpackThrust
			{
				Bone = bone,
				Forward = forward,
				Offset = offset,
				ThrustPointMaterial = MyStringId.GetOrCompute(thrustProperties.FlamePointMaterial),
				ThrustLengthMaterial = MyStringId.GetOrCompute(thrustProperties.FlameLengthMaterial),
				ThrustGlareSize = 1f
			};
			myJetpackThrust.Light = MyLights.AddLight();
			if (myJetpackThrust.Light != null)
			{
				myJetpackThrust.Light.ReflectorDirection = base.Container.Entity.PositionComp.WorldMatrix.Forward;
				myJetpackThrust.Light.ReflectorUp = base.Container.Entity.PositionComp.WorldMatrix.Up;
				myJetpackThrust.Light.ReflectorRange = 1f;
				myJetpackThrust.Light.Color = thrustProperties.FlameIdleColor;
				myJetpackThrust.Light.Start(base.Entity.DisplayName + " Jetpack " + m_jetpackThrusts.Count);
				myJetpackThrust.Light.Falloff = 2f;
				m_jetpackThrusts.Add(myJetpackThrust);
			}
		}

		public void InitLight(MyCharacterDefinition definition)
		{
			m_light = MyLights.AddLight();
			if (m_light != null)
			{
				m_light.Start(base.Entity.DisplayName + " Reflector");
				m_light.ReflectorOn = true;
				m_light.ReflectorTexture = "Textures\\Lights\\dual_reflector_2.png";
				UpdateLightBasics();
				m_flareLeft = CreateFlare("left");
				m_flareRight = CreateFlare("right");
				m_skinnedEntity.AnimationController.FindBone(definition.LeftLightBone, out m_leftLightIndex);
				m_skinnedEntity.AnimationController.FindBone(definition.RightLightBone, out m_rightLightIndex);
			}
		}

		private void UpdateLightBasics()
		{
			m_light.ReflectorColor = MyCharacter.REFLECTOR_COLOR;
			m_light.ReflectorConeMaxAngleCos = 0.373f;
			m_light.ReflectorRange = 35f;
			m_light.ReflectorFalloff = MyCharacter.REFLECTOR_FALLOFF;
			m_light.ReflectorGlossFactor = MyCharacter.REFLECTOR_GLOSS_FACTOR;
			m_light.ReflectorDiffuseFactor = MyCharacter.REFLECTOR_DIFFUSE_FACTOR;
			m_light.Color = MyCharacter.POINT_COLOR;
			m_light.Range = MyCharacter.POINT_LIGHT_RANGE;
			m_light.Falloff = MyCharacter.POINT_FALLOFF;
			m_light.GlossFactor = MyCharacter.POINT_GLOSS_FACTOR;
			m_light.DiffuseFactor = MyCharacter.POINT_DIFFUSE_FACTOR;
		}

		private MyLight CreateFlare(string debugName)
		{
			MyLight myLight = MyLights.AddLight();
			if (myLight != null)
			{
				myLight.Start(base.Entity.DisplayName + " Reflector " + debugName + " Flare");
				myLight.ReflectorOn = false;
				myLight.LightOn = false;
				myLight.Color = MyCharacter.POINT_COLOR;
				myLight.GlareOn = true;
				myLight.GlareIntensity = m_flareHeadlamp.Intensity;
				myLight.GlareSize = m_flareHeadlamp.Size;
				myLight.SubGlares = m_flareHeadlamp.SubGlares;
				myLight.GlareQuerySize = 0.05f;
				myLight.GlareMaxDistance = 40f;
				myLight.GlareType = MyGlareTypeEnum.Directional;
			}
			return myLight;
		}

		public void UpdateLightProperties(float currentLightPower)
		{
			if (m_light != null)
			{
				m_light.ReflectorIntensity = MyCharacter.REFLECTOR_INTENSITY * currentLightPower;
				m_light.Intensity = MyCharacter.POINT_LIGHT_INTENSITY * currentLightPower;
				m_light.UpdateLight();
				float glareIntensity = m_flareHeadlamp.Intensity * currentLightPower;
				m_flareLeft.GlareIntensity = glareIntensity;
				m_flareRight.GlareIntensity = glareIntensity;
			}
		}

		public void UpdateLightPosition()
		{
			if (m_light != null)
			{
				MyCharacter myCharacter = m_skinnedEntity as MyCharacter;
				m_lightLocalPosition = myCharacter.Definition.LightOffset;
				MatrixD headMatrix = myCharacter.GetHeadMatrix(includeY: false, includeX: true, forceHeadAnim: false, forceHeadBone: true);
				m_light.ReflectorDirection = headMatrix.Forward;
				m_light.ReflectorUp = headMatrix.Up;
				m_light.Position = Vector3D.Transform(m_lightLocalPosition, headMatrix);
				m_light.UpdateLight();
				Matrix[] boneAbsoluteTransforms = myCharacter.BoneAbsoluteTransforms;
				if (m_leftLightIndex != -1)
				{
					m_leftGlarePosition = (MatrixD.Normalize(boneAbsoluteTransforms[m_leftLightIndex]) * m_skinnedEntity.PositionComp.WorldMatrix).Translation;
				}
				if (m_rightLightIndex != -1)
				{
					m_rightGlarePosition = (MatrixD.Normalize(boneAbsoluteTransforms[m_rightLightIndex]) * m_skinnedEntity.PositionComp.WorldMatrix).Translation;
				}
			}
		}

		public void UpdateLight(float lightPower, bool updateRenderObject, bool updateBasicLight)
		{
			if (m_light != null && base.RenderObjectIDs[0] != uint.MaxValue)
			{
				bool flag = lightPower > 0f;
				if (m_light.ReflectorOn != flag)
				{
					m_light.ReflectorOn = flag;
					m_light.LightOn = flag;
					MyRenderProxy.UpdateColorEmissivity(base.RenderObjectIDs[0], 0, "Headlight", Color.White, flag ? 1f : 0f);
				}
				if (updateBasicLight)
				{
					UpdateLightBasics();
				}
				if (updateRenderObject || updateBasicLight)
				{
					UpdateLightPosition();
					UpdateLightProperties(lightPower);
				}
			}
		}

		public void UpdateThrustMatrices(Matrix[] boneMatrices)
		{
			foreach (MyJetpackThrust jetpackThrust in m_jetpackThrusts)
			{
				jetpackThrust.ThrustMatrix = Matrix.Normalize(boneMatrices[jetpackThrust.Bone]);
			}
		}

		public void UpdateShadowIgnoredObjects()
		{
			if (m_light != null)
			{
				MyRenderProxy.ClearLightShadowIgnore(m_light.RenderObjectID);
				MyRenderProxy.SetLightShadowIgnore(m_light.RenderObjectID, base.RenderObjectIDs[0]);
			}
		}

		public void UpdateShadowIgnoredObjects(IMyEntity Parent)
		{
			if (m_light != null)
			{
				uint[] renderObjectIDs = Parent.Render.RenderObjectIDs;
				foreach (uint ignoreId in renderObjectIDs)
				{
					MyRenderProxy.SetLightShadowIgnore(m_light.RenderObjectID, ignoreId);
				}
			}
		}

		public void Damage()
		{
			m_currentHitIndicatorCounter = 0.8f;
		}

		public void CleanLights()
		{
			if (m_light != null)
			{
				MyLights.RemoveLight(m_light);
				m_light = null;
			}
			if (m_flareLeft != null)
			{
				MyLights.RemoveLight(m_flareLeft);
				m_flareLeft = null;
			}
			if (m_flareRight != null)
			{
				MyLights.RemoveLight(m_flareRight);
				m_flareRight = null;
			}
			foreach (MyJetpackThrust jetpackThrust in m_jetpackThrusts)
			{
				MyLights.RemoveLight(jetpackThrust.Light);
			}
			m_jetpackThrusts.Clear();
		}

		private void DrawDisconnectedIndicator()
		{
			MyPlayer.PlayerId? savedPlayer = (base.Entity as MyCharacter).SavedPlayer;
			if (!savedPlayer.HasValue || savedPlayer.Value.SerialId != 0 || MySession.Static.Players.TryGetPlayerById(savedPlayer.Value, out MyPlayer _))
			{
				return;
			}
			Vector3D vector3D = base.Entity.PositionComp.GetPosition() + base.Entity.PositionComp.LocalAABB.Height * base.Entity.PositionComp.WorldMatrix.Up + base.Entity.PositionComp.WorldMatrix.Up * 0.20000000298023224;
			double num = Vector3D.Distance(MySector.MainCamera.Position, vector3D);
			if (!(num > (double)MAX_DISCONNECT_ICON_DISTANCE))
			{
				Color white = Color.White;
				white.A = (byte)((float)(int)white.A * (float)Math.Min(1.0, Math.Max(0.0, ((double)MAX_DISCONNECT_ICON_DISTANCE - num) / 10.0)));
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
				MyGuiPaddedTexture tEXTURE_DISCONNECTED_PLAYER = MyGuiConstants.TEXTURE_DISCONNECTED_PLAYER;
				MatrixD matrix = MySector.MainCamera.ViewMatrix * MySector.MainCamera.ProjectionMatrix;
				Vector3D vector3D2 = Vector3D.Transform(vector3D, matrix);
				if (vector3D2.Z < 1.0)
				{
					Vector2 hudPos = new Vector2((float)vector3D2.X, (float)vector3D2.Y);
					hudPos = hudPos * 0.5f + 0.5f * Vector2.One;
					hudPos.Y = 1f - hudPos.Y;
					Vector2 normalizedCoord = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
					MyGuiManager.DrawSpriteBatch(tEXTURE_DISCONNECTED_PLAYER.Texture, normalizedCoord, tEXTURE_DISCONNECTED_PLAYER.SizeGui * 0.5f * (1f - (float)num / (float)MAX_DISCONNECT_ICON_DISTANCE), white, drawAlign);
				}
			}
		}
	}
}
