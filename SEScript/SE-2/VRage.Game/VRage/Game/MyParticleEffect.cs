using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using VRage.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyParticleEffect
	{
		private class VRage_Game_MyParticleEffect_003C_003EActor : IActivator, IActivator<MyParticleEffect>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleEffect();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleEffect CreateInstance()
			{
				return new MyParticleEffect();
			}

			MyParticleEffect IActivator<MyParticleEffect>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int Version;

		private int m_particleID;

		private float m_elapsedTime;

		private string m_name;

		private float m_length = 90f;

		private bool m_isStopped;

		private bool m_isSimulationPaused;

		private bool m_isEmittingStopped;

		private bool m_loop;

		private float m_durationActual;

		private float m_durationMin;

		private float m_durationMax;

		private float m_timer;

		private MatrixD m_worldMatrix = MatrixD.Identity;

		private MatrixD m_lastWorldMatrix;

		public int ParticlesCount;

		private float m_distance;

		private readonly List<IMyParticleGeneration> m_generations = new List<IMyParticleGeneration>();

		private MyConcurrentList<MyParticleEffect> m_instances;

		private readonly List<MyParticleLight> m_particleLights = new List<MyParticleLight>();

		private readonly List<MyParticleSound> m_particleSounds = new List<MyParticleSound>();

		private BoundingBoxD m_AABB;

		private const int GRAVITY_UPDATE_DELAY = 200;

		private int m_updateCounter;

		public bool EnableLods;

		private float m_userEmitterScale;

		private float m_userScale;

		public Vector3 UserAxisScale;

		private uint m_parentID;

		private float m_userBirthMultiplier;

		private float m_userRadiusMultiplier;

		private Vector4 m_userColorMultiplier;

		public bool UserDraw;

		private int m_showOnlyThisGeneration = -1;

		public bool CalculateDeltaMatrix;

		public MatrixD DeltaMatrix;

		public uint RenderCounter;

		private Vector3 m_velocity;

		private bool m_velocitySet;

		private bool m_newLoop;

		private bool m_instantStop;

		private bool m_anyCpuGeneration;

		public bool TransformDirty
		{
			get;
			private set;
		}

		public float UserEmitterScale
		{
			get
			{
				return m_userEmitterScale;
			}
			set
			{
				m_userEmitterScale = value;
				TransformDirty = true;
			}
		}

		public float UserScale
		{
			get
			{
				return m_userScale;
			}
			set
			{
				m_userScale = value;
				TransformDirty = true;
			}
		}

		public uint ParentID
		{
			get
			{
				return m_parentID;
			}
			set
			{
				m_parentID = value;
				SetAnimDirty();
			}
		}

		public float UserBirthMultiplier
		{
			get
			{
				return m_userBirthMultiplier;
			}
			set
			{
				if (m_userBirthMultiplier != value)
				{
					m_userBirthMultiplier = value;
					SetAnimDirty();
				}
			}
		}

		public float UserRadiusMultiplier
		{
			get
			{
				return m_userRadiusMultiplier;
			}
			set
			{
				m_userRadiusMultiplier = value;
				SetDirty();
			}
		}

		public Vector4 UserColorMultiplier
		{
			get
			{
				return m_userColorMultiplier;
			}
			set
			{
				m_userColorMultiplier = value;
				SetDirty();
			}
		}

		[Browsable(false)]
		public int ShowOnlyThisGeneration => m_showOnlyThisGeneration;

		public Vector3 Velocity
		{
			get
			{
				return m_velocity;
			}
			set
			{
				m_velocity = value;
				m_velocitySet = true;
			}
		}

		public Vector3 Gravity
		{
			get;
			private set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public int ID
		{
			get
			{
				return m_particleID;
			}
			set
			{
				SetID(value);
			}
		}

		public float DistanceMax
		{
			get;
			set;
		}

		public float Length
		{
			get
			{
				return m_length;
			}
			set
			{
				m_length = value;
				if (m_instances != null)
				{
					foreach (MyParticleEffect instance in m_instances)
					{
						instance.Length = value;
					}
				}
			}
		}

		[Browsable(false)]
		public float Duration => m_durationActual;

		public float DurationMin
		{
			get
			{
				return m_durationMin;
			}
			set
			{
				SetDurationMin(value);
			}
		}

		public float DurationMax
		{
			get
			{
				return m_durationMax;
			}
			set
			{
				SetDurationMax(value);
			}
		}

		public bool Loop
		{
			get
			{
				return m_loop;
			}
			set
			{
				SetLoop(value);
			}
		}

		[Browsable(false)]
		public MatrixD WorldMatrix
		{
			get
			{
				return m_worldMatrix;
			}
			set
			{
				if (!value.EqualsFast(ref m_worldMatrix, 0.001))
				{
					TransformDirty = true;
					m_worldMatrix = value;
				}
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				SetName(value);
			}
		}

		[Browsable(false)]
		public float Distance => m_distance;

		[Browsable(false)]
		public object Tag
		{
			get;
			set;
		}

		[Browsable(false)]
		public bool IsStopped => m_isStopped;

		public bool IsSimulationPaused => m_isSimulationPaused;

		public bool IsEmittingStopped => m_isEmittingStopped;

		public event EventHandler OnDelete;

		public event EventHandler OnUpdate;

		public void SetShowOnlyThisGeneration(IMyParticleGeneration generation)
		{
			SetDirty();
			for (int i = 0; i < m_generations.Count; i++)
			{
				if (m_generations[i] == generation)
				{
					SetShowOnlyThisGeneration(i);
					return;
				}
			}
			SetShowOnlyThisGeneration(-1);
		}

		public void SetShowOnlyThisGeneration(int generationIndex)
		{
			m_showOnlyThisGeneration = generationIndex;
			for (int i = 0; i < m_generations.Count; i++)
			{
				m_generations[i].Show = (generationIndex < 0 || i == generationIndex);
			}
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.SetShowOnlyThisGeneration(generationIndex);
				}
			}
		}

		public MyParticleEffect()
		{
			Enabled = true;
		}

		public void Start(int particleID, string particleName)
		{
			m_particleID = particleID;
			m_name = particleName;
			m_parentID = uint.MaxValue;
			m_isStopped = false;
			m_isEmittingStopped = false;
			m_isSimulationPaused = false;
			m_distance = 0f;
			UserEmitterScale = 1f;
			UserBirthMultiplier = 1f;
			UserRadiusMultiplier = 1f;
			UserScale = 1f;
			UserAxisScale = Vector3.One;
			UserColorMultiplier = Vector4.One;
			UserDraw = false;
			Enabled = true;
			EnableLods = true;
			m_instantStop = false;
			m_velocitySet = false;
			WorldMatrix = MatrixD.Identity;
			DeltaMatrix = MatrixD.Identity;
			CalculateDeltaMatrix = false;
			RenderCounter = 0u;
			m_updateCounter = 0;
		}

		public void Restart()
		{
			m_elapsedTime = 0f;
		}

		public void Close(bool notify, bool forceInstant)
		{
			if (notify && this.OnDelete != null)
			{
				this.OnDelete(this, null);
			}
			Clear();
			foreach (IMyParticleGeneration generation in m_generations)
			{
				if (forceInstant || m_instantStop)
				{
					generation.Done();
				}
				else
				{
					generation.Close();
				}
				generation.Deallocate();
			}
			m_generations.Clear();
			foreach (MyParticleLight particleLight in m_particleLights)
			{
				if (forceInstant)
				{
					particleLight.Done();
				}
				else
				{
					particleLight.Close();
				}
				MyParticlesManager.LightsPool.Deallocate(particleLight);
			}
			m_particleLights.Clear();
			foreach (MyParticleSound particleSound in m_particleSounds)
			{
				if (forceInstant)
				{
					particleSound.Done();
				}
				else
				{
					particleSound.Close();
				}
				MyParticlesManager.SoundsPool.Deallocate(particleSound);
			}
			m_particleSounds.Clear();
			if (m_instances != null)
			{
				while (m_instances.Count > 0)
				{
					MyParticlesManager.RemoveParticleEffect(m_instances[0]);
				}
			}
			this.OnDelete = null;
			this.OnUpdate = null;
			Tag = null;
		}

		public void Clear()
		{
			m_elapsedTime = 0f;
			ParticlesCount = 0;
			foreach (IMyParticleGeneration generation in m_generations)
			{
				if (generation != null)
				{
					generation.Clear();
				}
				else
				{
					string msg = "Error: MyParticleGeneration should not be null!";
					MyLog.Default.WriteLine(msg);
				}
			}
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					if (instance != null)
					{
						instance.Clear();
					}
					else
					{
						string msg2 = "Error: MyParticleEffect should not be null!";
						MyLog.Default.WriteLine(msg2);
					}
				}
			}
		}

		public MyParticleEffect CreateInstance(ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentId)
		{
			MyParticleEffect myParticleEffect = null;
			if (MyParticlesManager.DISTANCE_CHECK_ENABLE && DistanceMax > 0f && !m_loop)
			{
				Vector3D value = MyTransparentGeometry.Camera.Translation;
				Vector3D.DistanceSquared(ref worldPosition, ref value, out double result);
				if (result <= (double)(DistanceMax * DistanceMax))
				{
					myParticleEffect = MyParticlesManager.EffectsPool.Allocate(nullAllowed: true);
				}
			}
			else
			{
				myParticleEffect = MyParticlesManager.EffectsPool.Allocate(nullAllowed: true);
			}
			if (myParticleEffect != null)
			{
				myParticleEffect.Start(m_particleID, m_name);
				myParticleEffect.ParentID = parentId;
				myParticleEffect.Enabled = Enabled;
				myParticleEffect.DistanceMax = DistanceMax;
				myParticleEffect.Length = Length;
				myParticleEffect.Loop = m_loop;
				myParticleEffect.DurationMin = m_durationMin;
				myParticleEffect.DurationMax = m_durationMax;
				myParticleEffect.SetRandomDuration();
				myParticleEffect.WorldMatrix = effectMatrix;
				myParticleEffect.m_anyCpuGeneration = false;
				foreach (IMyParticleGeneration generation in m_generations)
				{
					IMyParticleGeneration myParticleGeneration = generation.CreateInstance(myParticleEffect);
					if (myParticleGeneration != null)
					{
						myParticleEffect.AddGeneration(myParticleGeneration);
						myParticleEffect.m_anyCpuGeneration |= (myParticleGeneration is MyParticleGeneration);
					}
				}
				foreach (MyParticleLight particleLight in m_particleLights)
				{
					MyParticleLight myParticleLight = particleLight.CreateInstance(myParticleEffect);
					if (myParticleLight != null)
					{
						myParticleEffect.AddParticleLight(myParticleLight);
					}
				}
				foreach (MyParticleSound particleSound in m_particleSounds)
				{
					MyParticleSound myParticleSound = particleSound.CreateInstance(myParticleEffect);
					if (myParticleSound != null)
					{
						myParticleEffect.AddParticleSound(myParticleSound);
					}
				}
				if (m_instances == null)
				{
					m_instances = new MyConcurrentList<MyParticleEffect>();
				}
				m_instances.Add(myParticleEffect);
			}
			return myParticleEffect;
		}

		public void Stop(bool instant = true)
		{
			m_isStopped = true;
			m_isEmittingStopped = true;
			m_instantStop = instant;
			SetDirty();
		}

		/// <summary>
		/// This method restores effect
		/// </summary>
		public void Play()
		{
			m_isSimulationPaused = false;
			m_isEmittingStopped = false;
			SetDirty();
		}

		/// <summary>
		/// This methods freezes effect and particles
		/// </summary>
		public void Pause()
		{
			m_isSimulationPaused = true;
			m_isEmittingStopped = true;
			SetDirty();
		}

		/// <summary>
		/// This method stops generating any new particles
		/// </summary>
		public void StopEmitting(float timeout = 0f)
		{
			m_isEmittingStopped = true;
			m_timer = timeout;
			SetDirty();
		}

		/// <summary>
		/// This method stops all lights
		/// </summary>
		public void StopLights()
		{
			foreach (MyParticleLight particleLight in m_particleLights)
			{
				if (particleLight.Enabled != null)
				{
					particleLight.Enabled.SetValue(val: false);
				}
			}
		}

		public void SetDirty()
		{
			foreach (IMyParticleGeneration generation in m_generations)
			{
				generation.SetDirty();
			}
		}

		public void SetAnimDirty()
		{
			foreach (IMyParticleGeneration generation in m_generations)
			{
				generation.SetAnimDirty();
			}
		}

		public void SetDirtyInstances()
		{
			foreach (IMyParticleGeneration generation in m_generations)
			{
				generation.SetDirty();
			}
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.SetDirtyInstances();
				}
			}
		}

		public void RemoveInstance(MyParticleEffect effect)
		{
			if (m_instances != null && m_instances.Contains(effect))
			{
				m_instances.Remove(effect);
			}
		}

		internal MyConcurrentList<MyParticleEffect> GetInstances()
		{
			return m_instances;
		}

		public MyParticleEffect Duplicate()
		{
			MyParticleEffect myParticleEffect = MyParticlesManager.EffectsPool.Allocate();
			myParticleEffect.Start(0, Name);
			myParticleEffect.m_length = m_length;
			myParticleEffect.DurationMin = m_durationMin;
			myParticleEffect.DurationMax = m_durationMax;
			myParticleEffect.Loop = m_loop;
			foreach (IMyParticleGeneration generation2 in m_generations)
			{
				IMyParticleGeneration generation = generation2.Duplicate(myParticleEffect);
				myParticleEffect.AddGeneration(generation);
			}
			foreach (MyParticleLight particleLight2 in m_particleLights)
			{
				MyParticleLight particleLight = particleLight2.Duplicate(myParticleEffect);
				myParticleEffect.AddParticleLight(particleLight);
			}
			foreach (MyParticleSound particleSound2 in m_particleSounds)
			{
				MyParticleSound particleSound = particleSound2.Duplicate(myParticleEffect);
				myParticleEffect.AddParticleSound(particleSound);
			}
			return myParticleEffect;
		}

		public MatrixD GetDeltaMatrix()
		{
			MatrixD matrix = MatrixD.Invert(m_lastWorldMatrix);
			MatrixD.Multiply(ref matrix, ref m_worldMatrix, out DeltaMatrix);
			return DeltaMatrix;
		}

		public bool Update()
		{
			if (!Enabled)
			{
				return m_isStopped;
			}
			if (WorldMatrix == MatrixD.Zero)
			{
				return true;
			}
			if (ParentID == uint.MaxValue)
			{
				float num = 100f;
				if (MyParticlesManager.CalculateGravityInPoint != null && m_updateCounter == 0)
				{
					Vector3 vector = MyParticlesManager.CalculateGravityInPoint(WorldMatrix.Translation);
					float num2 = vector.Length();
					if (num2 > num)
					{
						vector = vector / num2 * num;
					}
					Gravity = vector;
				}
				m_updateCounter++;
				if (m_updateCounter > 200)
				{
					m_updateCounter = 0;
				}
			}
			if (m_velocitySet)
			{
				Vector3D translation = m_worldMatrix.Translation;
				translation += m_velocity * 0.0166666675f;
				m_worldMatrix.Translation = translation;
				TransformDirty = true;
			}
			if (m_anyCpuGeneration)
			{
				m_distance = (float)Vector3D.Distance(MyTransparentGeometry.Camera.Translation, WorldMatrix.Translation) / 100f;
				ParticlesCount = 0;
				m_AABB = BoundingBoxD.CreateInvalid();
				for (int i = 0; i < m_generations.Count; i++)
				{
					if (m_showOnlyThisGeneration < 0 || i == m_showOnlyThisGeneration)
					{
						m_generations[i].Update();
						m_generations[i].MergeAABB(ref m_AABB);
					}
				}
			}
			if (!MyParticlesManager.Paused)
			{
				foreach (MyParticleLight particleLight in m_particleLights)
				{
					particleLight.Update();
				}
				foreach (MyParticleSound particleSound in m_particleSounds)
				{
					particleSound.Update();
				}
			}
			if (this.OnUpdate != null)
			{
				this.OnUpdate(this, null);
			}
			return UpdateLife();
		}

		public bool UpdateLife()
		{
			m_elapsedTime += 0.0166666675f;
			if (m_timer > 0f)
			{
				m_timer -= 0.0166666675f;
				if (m_timer <= 0f)
				{
					return true;
				}
			}
			if (m_loop && m_elapsedTime >= m_durationActual)
			{
				m_elapsedTime = 0f;
				SetRandomDuration();
			}
			if (m_isStopped)
			{
				return ParticlesCount == 0;
			}
			if (m_durationActual > 0f)
			{
				return m_elapsedTime > m_durationActual;
			}
			return false;
		}

		public void SetRandomDuration()
		{
			m_durationActual = ((m_durationMax > m_durationMin) ? MyUtils.GetRandomFloat(m_durationMin, m_durationMax) : m_durationMin);
		}

		private void SetDurationMin(float duration)
		{
			m_durationMin = duration;
			SetRandomDuration();
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.SetDurationMin(duration);
				}
			}
		}

		private void SetDurationMax(float duration)
		{
			m_durationMax = duration;
			SetRandomDuration();
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.SetDurationMax(duration);
				}
			}
		}

		private void SetLoop(bool loop)
		{
			m_loop = loop;
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.SetLoop(loop);
				}
			}
		}

		public float GetScale()
		{
			return UserScale;
		}

		public float GetEmitterScale()
		{
			return UserScale * UserEmitterScale;
		}

		public Vector3 GetEmitterAxisScale()
		{
			return UserAxisScale * UserEmitterScale;
		}

		public float GetElapsedTime()
		{
			return m_elapsedTime;
		}

		public int GetID()
		{
			return m_particleID;
		}

		public int GetParticlesCount()
		{
			return ParticlesCount;
		}

		public void SetID(int id)
		{
			if (m_particleID != id)
			{
				int particleID = m_particleID;
				m_particleID = id;
				MyParticlesLibrary.UpdateParticleEffectID(particleID);
			}
		}

		public string GetName()
		{
			return m_name;
		}

		public void SetName(string name)
		{
			if (m_name != name)
			{
				string name2 = m_name;
				m_name = name;
				if (name2 != null)
				{
					MyParticlesLibrary.UpdateParticleEffectName(name2);
				}
			}
		}

		public void SetTranslation(Vector3D trans)
		{
			TransformDirty = true;
			m_worldMatrix.Translation = trans;
		}

		public void AddGeneration(IMyParticleGeneration generation)
		{
			m_generations.Add(generation);
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					IMyParticleGeneration myParticleGeneration = generation.CreateInstance(instance);
					if (myParticleGeneration != null)
					{
						instance.AddGeneration(myParticleGeneration);
					}
				}
			}
		}

		public void RemoveGeneration(int index)
		{
			IMyParticleGeneration myParticleGeneration = m_generations[index];
			m_generations.Remove(myParticleGeneration);
			myParticleGeneration.Close();
			myParticleGeneration.Deallocate();
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.RemoveGeneration(index);
				}
			}
		}

		public void RemoveGeneration(IMyParticleGeneration generation)
		{
			int index = m_generations.IndexOf(generation);
			RemoveGeneration(index);
		}

		public List<IMyParticleGeneration> GetGenerations()
		{
			return m_generations;
		}

		public BoundingBoxD GetAABB()
		{
			return m_AABB;
		}

		public void AddParticleLight(MyParticleLight particleLight)
		{
			m_particleLights.Add(particleLight);
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.AddParticleLight(particleLight.CreateInstance(instance));
				}
			}
		}

		public void RemoveParticleLight(int index)
		{
			MyParticleLight myParticleLight = m_particleLights[index];
			m_particleLights.Remove(myParticleLight);
			myParticleLight.Close();
			MyParticlesManager.LightsPool.Deallocate(myParticleLight);
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.RemoveParticleLight(index);
				}
			}
		}

		public void RemoveParticleLight(MyParticleLight particleLight)
		{
			int index = m_particleLights.IndexOf(particleLight);
			RemoveParticleLight(index);
		}

		public List<MyParticleLight> GetParticleLights()
		{
			return m_particleLights;
		}

		public void AddParticleSound(MyParticleSound particleSound)
		{
			m_particleSounds.Add(particleSound);
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.AddParticleSound(particleSound.CreateInstance(instance));
				}
			}
		}

		public void RemoveParticleSound(int index)
		{
			MyParticleSound myParticleSound = m_particleSounds[index];
			m_particleSounds.Remove(myParticleSound);
			myParticleSound.Close();
			MyParticlesManager.SoundsPool.Deallocate(myParticleSound);
			if (m_instances != null)
			{
				foreach (MyParticleEffect instance in m_instances)
				{
					instance.RemoveParticleSound(index);
				}
			}
		}

		public void RemoveParticleSound(MyParticleSound particleSound)
		{
			int index = m_particleSounds.IndexOf(particleSound);
			RemoveParticleSound(index);
		}

		public List<MyParticleSound> GetParticleSounds()
		{
			return m_particleSounds;
		}

		public unsafe void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ParticleEffect");
			writer.WriteAttributeString("xsi", "type", null, "MyObjectBuilder_ParticleEffect");
			writer.WriteStartElement("Id");
			writer.WriteElementString("TypeId", "ParticleEffect");
			writer.WriteElementString("SubtypeId", Name);
			writer.WriteEndElement();
			writer.WriteElementString("Version", ((int)Version).ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("ParticleId", ((int*)(&m_particleID))->ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("Length", m_length.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("DurationMin", m_durationMin.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("DurationMax", m_durationMax.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("DistanceMax", DistanceMax.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("Loop", m_loop.ToString(CultureInfo.InvariantCulture).ToLower());
			writer.WriteStartElement("ParticleGenerations");
			foreach (IMyParticleGeneration generation in m_generations)
			{
				generation.Serialize(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("ParticleLights");
			foreach (MyParticleLight particleLight in m_particleLights)
			{
				particleLight.Serialize(writer);
			}
			writer.WriteEndElement();
			writer.WriteStartElement("ParticleSounds");
			foreach (MyParticleSound particleSound in m_particleSounds)
			{
				particleSound.Serialize(writer);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public void Deserialize(XmlReader reader)
		{
			m_name = reader.GetAttribute("name");
			Convert.ToInt32(reader.GetAttribute("version"), CultureInfo.InvariantCulture);
			reader.ReadStartElement();
			m_particleID = reader.ReadElementContentAsInt();
			m_length = reader.ReadElementContentAsFloat();
			if (reader.Name == "LowRes")
			{
				reader.ReadElementContentAsBoolean();
			}
			if (reader.Name == "Scale")
			{
				reader.ReadElementContentAsFloat();
			}
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			while (reader.NodeType != XmlNodeType.EndElement && !isEmptyElement)
			{
				if (reader.Name == "ParticleGeneration" && MyParticlesManager.EnableCPUGenerations)
				{
					MyParticlesManager.GenerationsPool.AllocateOrCreate(out MyParticleGeneration item);
					item.Start(this);
					item.Init();
					item.Deserialize(reader);
					AddGeneration(item);
				}
				else if (reader.Name == "ParticleGPUGeneration")
				{
					MyParticlesManager.GPUGenerationsPool.AllocateOrCreate(out MyParticleGPUGeneration item2);
					item2.Start(this);
					item2.Init();
					item2.Deserialize(reader);
					AddGeneration(item2);
				}
				else
				{
					reader.Read();
				}
			}
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
			if (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.IsEmptyElement)
				{
					reader.Read();
				}
				else
				{
					reader.ReadStartElement();
					while (reader.NodeType != XmlNodeType.EndElement)
					{
						MyParticlesManager.LightsPool.AllocateOrCreate(out MyParticleLight item3);
						item3.Start(this);
						item3.Init();
						item3.Deserialize(reader);
						AddParticleLight(item3);
					}
					reader.ReadEndElement();
				}
			}
			if (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.IsEmptyElement)
				{
					reader.Read();
				}
				else
				{
					reader.ReadStartElement();
					while (reader.NodeType != XmlNodeType.EndElement)
					{
						MyParticlesManager.SoundsPool.AllocateOrCreate(out MyParticleSound item4);
						item4.Start(this);
						item4.Init();
						item4.Deserialize(reader);
						AddParticleSound(item4);
					}
					reader.ReadEndElement();
				}
			}
			reader.ReadEndElement();
		}

		public void DeserializeFromObjectBuilder(MyObjectBuilder_ParticleEffect builder)
		{
			m_name = builder.Id.SubtypeName;
			m_particleID = builder.ParticleId;
			m_length = builder.Length;
			m_loop = builder.Loop;
			m_durationMin = builder.DurationMin;
			m_durationMax = builder.DurationMax;
			DistanceMax = builder.DistanceMax;
			SetRandomDuration();
			foreach (ParticleGeneration particleGeneration in builder.ParticleGenerations)
			{
				string generationType = particleGeneration.GenerationType;
				if (!(generationType == "CPU"))
				{
					if (generationType == "GPU")
					{
						MyParticlesManager.GPUGenerationsPool.AllocateOrCreate(out MyParticleGPUGeneration item);
						item.Start(this);
						item.Init();
						item.DeserializeFromObjectBuilder(particleGeneration);
						AddGeneration(item);
					}
				}
				else if (MyParticlesManager.EnableCPUGenerations)
				{
					MyParticlesManager.GenerationsPool.AllocateOrCreate(out MyParticleGeneration item2);
					item2.Start(this);
					item2.Init();
					item2.DeserializeFromObjectBuilder(particleGeneration);
					AddGeneration(item2);
				}
			}
			foreach (ParticleLight particleLight in builder.ParticleLights)
			{
				MyParticlesManager.LightsPool.AllocateOrCreate(out MyParticleLight item3);
				item3.Start(this);
				item3.Init();
				item3.DeserializeFromObjectBuilder(particleLight);
				AddParticleLight(item3);
			}
			foreach (ParticleSound particleSound in builder.ParticleSounds)
			{
				MyParticlesManager.SoundsPool.AllocateOrCreate(out MyParticleSound item4);
				item4.Start(this);
				item4.Init();
				item4.DeserializeFromObjectBuilder(particleSound);
				AddParticleSound(item4);
			}
		}

		public void Draw(List<MyBillboard> collectedBillboards)
		{
			foreach (IMyParticleGeneration generation in m_generations)
			{
				generation.Draw(collectedBillboards);
			}
			if (TransformDirty)
			{
				m_lastWorldMatrix = m_worldMatrix;
				TransformDirty = false;
			}
		}

		public void DebugDraw()
		{
			MyRenderProxy.DebugDrawAxis(WorldMatrix, 1f, depthRead: false);
			foreach (IMyParticleGeneration generation in m_generations)
			{
				if (generation is MyParticleGeneration)
				{
					(generation as MyParticleGeneration).DebugDraw();
				}
			}
			Color color = (!m_isStopped) ? Color.White : Color.Red;
			MyRenderProxy.DebugDrawText3D(WorldMatrix.Translation, Name + "(" + (int)GetID() + ") [" + (int)GetParticlesCount() + "]", color, 0.8f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			MyRenderProxy.DebugDrawAABB(m_AABB, color);
		}

		public override string ToString()
		{
			return Name + " (" + (int)ID + ")";
		}
	}
}
