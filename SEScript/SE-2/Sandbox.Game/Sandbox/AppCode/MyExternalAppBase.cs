using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.AppCode
{
	public class MyExternalAppBase : IExternalApp
	{
		public static MySandboxGame Static;

		private static bool m_isEditorActive;

		private static bool m_isPresent;

		public static bool IsEditorActive
		{
			get
			{
				return m_isEditorActive;
			}
			set
			{
				m_isEditorActive = value;
			}
		}

		public static bool IsPresent
		{
			get
			{
				return m_isPresent;
			}
			set
			{
				m_isPresent = value;
			}
		}

		public void Run(IntPtr windowHandle, bool customRenderLoop = false, MySandboxGame game = null)
		{
			MyLog.Default = MySandboxGame.Log;
			MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = false;
			if (game == null)
			{
				Static = new MySandboxExternal(this, null, windowHandle);
			}
			else
			{
				Static = game;
			}
			Initialize(Static);
			Static.OnGameLoaded += GameLoaded;
			Static.OnGameExit += GameExit;
			MySession.AfterLoading += MySession_AfterLoading;
			MySession.BeforeLoading += MySession_BeforeLoading;
			Static.Run(customRenderLoop);
			if (!customRenderLoop)
			{
				Dispose();
			}
		}

		public virtual void GameExit()
		{
		}

		public void Dispose()
		{
			Static.Dispose();
			Static = null;
		}

		public void RunSingleFrame()
		{
			Static.RunSingleFrame();
		}

		public void EndLoop()
		{
			Static.EndLoop();
		}

		void IExternalApp.Draw()
		{
			Draw(canDraw: false);
		}

		void IExternalApp.Update()
		{
			Update(canDraw: true);
		}

		void IExternalApp.UpdateMainThread()
		{
			UpdateMainThread();
		}

		public virtual void Initialize(Sandbox.Engine.Platform.Game game)
		{
		}

		public virtual void UpdateMainThread()
		{
		}

		public virtual void Update(bool canDraw)
		{
		}

		public virtual void Draw(bool canDraw)
		{
		}

		public virtual void GameLoaded(object sender, EventArgs e)
		{
			IsEditorActive = true;
			IsPresent = true;
		}

		public virtual void MySession_AfterLoading()
		{
		}

		public virtual void MySession_BeforeLoading()
		{
		}

		public MyParticleEffect CreateParticle(string name, MatrixD worldMatrix)
		{
			Vector3D worldPosition = worldMatrix.Translation;
			return MyParticlesLibrary.CreateParticleEffect(name, ref worldMatrix, ref worldPosition, uint.MaxValue);
		}

		public void RemoveParticle(MyParticleEffect effect)
		{
			MyParticlesLibrary.RemoveParticleEffectInstance(effect);
		}

		public MatrixD GetSpectatorMatrix()
		{
			if (MySpectatorCameraController.Static != null)
			{
				return MatrixD.Invert(MySpectatorCameraController.Static.GetViewMatrix());
			}
			return MatrixD.Identity;
		}

		public MyParticleGeneration AllocateGeneration()
		{
			MyParticlesManager.GenerationsPool.AllocateOrCreate(out MyParticleGeneration item);
			return item;
		}

		public MyParticleGPUGeneration AllocateGPUGeneration()
		{
			MyParticlesManager.GPUGenerationsPool.AllocateOrCreate(out MyParticleGPUGeneration item);
			return item;
		}

		public MyParticleLight AllocateParticleLight()
		{
			MyParticlesManager.LightsPool.AllocateOrCreate(out MyParticleLight item);
			return item;
		}

		public MyParticleSound AllocateParticleSound()
		{
			MyParticlesManager.SoundsPool.AllocateOrCreate(out MyParticleSound item);
			return item;
		}

		public MyParticleEffect CreateLibraryEffect()
		{
			return MyParticlesManager.EffectsPool.Allocate();
		}

		public void AddParticleToLibrary(MyParticleEffect effect)
		{
			MyParticlesLibrary.AddParticleEffect(effect);
		}

		public void RemoveParticleFromLibrary(string name)
		{
			MyParticlesLibrary.RemoveParticleEffect(name);
		}

		public IReadOnlyDictionary<int, MyParticleEffect> GetLibraryEffects()
		{
			return MyParticlesLibrary.GetParticleEffectsById();
		}

		public IReadOnlyDictionary<string, MyParticleEffect> GetParticleEffectsByName()
		{
			return MyParticlesLibrary.GetParticleEffectsByName();
		}

		public void SaveParticlesLibrary(string file)
		{
			MyParticlesLibrary.Serialize(file);
		}

		public void LoadParticlesLibrary(string file)
		{
			if (file.Contains(".mwl"))
			{
				MyParticlesLibrary.Deserialize(file);
				return;
			}
			MyDataIntegrityChecker.HashInFile(file);
			MyObjectBuilder_Definitions objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(file, out objectBuilder);
			if (objectBuilder != null && objectBuilder.ParticleEffects != null)
			{
				MyParticlesLibrary.Close();
				MyObjectBuilder_ParticleEffect[] particleEffects = objectBuilder.ParticleEffects;
				foreach (MyObjectBuilder_ParticleEffect builder in particleEffects)
				{
					MyParticleEffect myParticleEffect = MyParticlesManager.EffectsPool.Allocate();
					myParticleEffect.DeserializeFromObjectBuilder(builder);
					MyParticlesLibrary.AddParticleEffect(myParticleEffect);
				}
			}
		}

		public void FlushParticles()
		{
			foreach (string item in new List<string>(MyParticlesLibrary.GetParticleEffectsNames()))
			{
				MyParticlesLibrary.RemoveParticleEffect(item);
			}
		}

		public void LoadDefinitions()
		{
			MyDefinitionManager.Static.LoadData(new List<MyObjectBuilder_Checkpoint.ModItem>());
		}

		public float GetStepInSeconds()
		{
			return 0.0166666675f;
		}
	}
}
