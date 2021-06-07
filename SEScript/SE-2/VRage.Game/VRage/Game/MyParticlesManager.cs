using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Generics;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyParticlesManager : MySessionComponentBase
	{
		public static readonly bool DISTANCE_CHECK_ENABLE;

		public static bool Enabled;

		private static bool m_paused;

		public static Func<Vector3D, Vector3> CalculateGravityInPoint;

		public static bool EnableCPUGenerations;

		public static MyObjectsPool<MyParticleGeneration> GenerationsPool;

		public static MyObjectsPool<MyParticleGPUGeneration> GPUGenerationsPool;

		public static MyObjectsPool<MyParticleLight> LightsPool;

		public static MyObjectsPool<MyParticleSound> SoundsPool;

		public static MyObjectsPool<MyParticleEffect> EffectsPool;

		public static List<MyGPUEmitter> GPUEmitters;

		public static List<MyGPUEmitterLite> GPUEmittersLite;

		public static List<MyGPUEmitterTransformUpdate> GPUEmitterTransforms;

		private static List<MyParticleEffect> m_effectsToDelete;

		private static List<MyParticleEffect> m_particleEffectsForUpdate;

		private static List<MyParticleEffect> m_particleEffectsAll;

		private static List<MyBillboard> m_collectedBillboards;

		private static FastResourceLock m_particlesLock;

		public static bool Paused
		{
			get
			{
				return m_paused;
			}
			set
			{
				if (m_paused != value)
				{
					m_paused = value;
					using (m_particlesLock.AcquireExclusiveUsing())
					{
						foreach (MyParticleEffect item in m_particleEffectsForUpdate)
						{
							item.SetDirty();
						}
					}
				}
			}
		}

		public static List<MyParticleEffect> ParticleEffectsForUpdate => m_particleEffectsForUpdate;

		static MyParticlesManager()
		{
			DISTANCE_CHECK_ENABLE = true;
			m_paused = false;
			EnableCPUGenerations = true;
			GenerationsPool = new MyObjectsPool<MyParticleGeneration>(4096);
			GPUGenerationsPool = new MyObjectsPool<MyParticleGPUGeneration>(4096);
			LightsPool = new MyObjectsPool<MyParticleLight>(32);
			SoundsPool = new MyObjectsPool<MyParticleSound>(512);
			EffectsPool = new MyObjectsPool<MyParticleEffect>(2048);
			GPUEmitters = new List<MyGPUEmitter>();
			GPUEmittersLite = new List<MyGPUEmitterLite>();
			GPUEmitterTransforms = new List<MyGPUEmitterTransformUpdate>();
			m_effectsToDelete = new List<MyParticleEffect>();
			m_particleEffectsForUpdate = new List<MyParticleEffect>();
			m_particleEffectsAll = new List<MyParticleEffect>();
			m_collectedBillboards = new List<MyBillboard>(16384);
			m_particlesLock = new FastResourceLock();
			Enabled = true;
		}

		[Obsolete("Use TryCreateParticleEffect with parenting instead")]
		public static bool TryCreateParticleEffect(string effectName, out MyParticleEffect effect)
		{
			return TryCreateParticleEffect(effectName, ref MatrixD.Identity, ref Vector3D.Zero, uint.MaxValue, out effect);
		}

		[Obsolete("Use TryCreateParticleEffect with parenting instead")]
		public static bool TryCreateParticleEffect(string effectName, MatrixD worldMatrix, out MyParticleEffect effect)
		{
			Vector3D worldPosition = worldMatrix.Translation;
			return TryCreateParticleEffect(effectName, ref worldMatrix, ref worldPosition, uint.MaxValue, out effect);
		}

		public static bool TryCreateParticleEffect(string effectName, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID, out MyParticleEffect effect)
		{
			using (m_particlesLock.AcquireExclusiveUsing())
			{
				if (string.IsNullOrEmpty(effectName) || !Enabled || !MyParticlesLibrary.EffectExists(effectName))
				{
					effect = null;
					return false;
				}
				effect = CreateParticleEffect(effectName, ref effectMatrix, ref worldPosition, parentID);
				return effect != null;
			}
		}

		[Obsolete("Use TryCreateParticleEffect with parenting instead")]
		public static bool TryCreateParticleEffect(int id, out MyParticleEffect effect, bool userDraw = false)
		{
			return TryCreateParticleEffect(id, out effect, ref MatrixD.Identity, ref Vector3D.Zero, uint.MaxValue, userDraw);
		}

		public static bool TryCreateParticleEffect(int id, out MyParticleEffect effect, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID, bool userDraw = false)
		{
			effect = null;
			using (m_particlesLock.AcquireExclusiveUsing())
			{
				if (MyParticlesLibrary.GetParticleEffectsID(id, out string name))
				{
					effect = CreateParticleEffect(name, ref effectMatrix, ref worldPosition, parentID, userDraw);
				}
				return effect != null;
			}
		}

		private static MyParticleEffect CreateParticleEffect(string name, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID, bool userDraw = false)
		{
			MyParticleEffect myParticleEffect = MyParticlesLibrary.CreateParticleEffect(name, ref effectMatrix, ref worldPosition, parentID);
			userDraw = false;
			if (myParticleEffect != null)
			{
				if (!userDraw)
				{
					m_particleEffectsForUpdate.Add(myParticleEffect);
				}
				myParticleEffect.UserDraw = userDraw;
				m_particleEffectsAll.Add(myParticleEffect);
			}
			return myParticleEffect;
		}

		public static void RemoveParticleEffect(MyParticleEffect effect, bool fromBackground = false)
		{
			if (effect != null)
			{
				if (!effect.UserDraw)
				{
					using (m_particlesLock.AcquireExclusiveUsing())
					{
						m_particleEffectsForUpdate.Remove(effect);
					}
				}
				m_particleEffectsAll.Remove(effect);
				MyParticlesLibrary.RemoveParticleEffectInstance(effect);
			}
		}

		protected override void UnloadData()
		{
			foreach (MyParticleEffect item in m_particleEffectsForUpdate)
			{
				m_effectsToDelete.Add(item);
			}
			foreach (MyParticleEffect item2 in m_effectsToDelete)
			{
				RemoveParticleEffect(item2);
			}
			m_effectsToDelete.Clear();
		}

		public override void LoadData()
		{
			base.LoadData();
			if (Enabled)
			{
				MyTransparentGeometry.LoadData();
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (Enabled)
			{
				UpdateEffects();
			}
		}

		private static void UpdateEffects()
		{
			using (m_particlesLock.AcquireExclusiveUsing())
			{
				foreach (MyParticleEffect item in m_particleEffectsForUpdate)
				{
					if (item.Update())
					{
						m_effectsToDelete.Add(item);
					}
				}
			}
			foreach (MyParticleEffect item2 in m_effectsToDelete)
			{
				RemoveParticleEffect(item2, fromBackground: true);
			}
			m_effectsToDelete.Clear();
		}

		public static void DrawStart()
		{
			GPUEmitters.Clear();
			GPUEmittersLite.Clear();
			GPUEmitterTransforms.Clear();
		}

		public static void DrawEnd()
		{
			if (GPUEmitters.Count > 0)
			{
				MyRenderProxy.UpdateGPUEmitters(ref GPUEmitters);
				GPUEmitters.AssertEmpty();
			}
			if (GPUEmitterTransforms.Count > 0)
			{
				MyRenderProxy.UpdateGPUEmittersTransform(ref GPUEmitterTransforms);
				GPUEmitterTransforms.AssertEmpty();
			}
			if (GPUEmittersLite.Count > 0)
			{
				MyRenderProxy.UpdateGPUEmittersLite(ref GPUEmittersLite);
				GPUEmittersLite.AssertEmpty();
			}
		}

		public override void Draw()
		{
			if (Enabled)
			{
				m_collectedBillboards.Clear();
				DrawStart();
				using (m_particlesLock.AcquireExclusiveUsing())
				{
					foreach (MyParticleEffect item in m_particleEffectsForUpdate)
					{
						item.Draw(m_collectedBillboards);
					}
				}
				DrawEnd();
				if (m_collectedBillboards.Count > 0)
				{
					MyRenderProxy.AddBillboards(m_collectedBillboards);
				}
			}
		}
	}
}
