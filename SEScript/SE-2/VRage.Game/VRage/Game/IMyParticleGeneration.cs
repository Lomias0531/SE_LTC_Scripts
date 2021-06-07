using System.Collections.Generic;
using System.Xml;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace VRage.Game
{
	public interface IMyParticleGeneration
	{
		string Name
		{
			get;
			set;
		}

		MyConstPropertyBool Enabled
		{
			get;
		}

		bool Show
		{
			get;
			set;
		}

		void Close();

		void Deallocate();

		IMyParticleGeneration CreateInstance(MyParticleEffect effect);

		IEnumerable<IMyConstProperty> GetProperties();

		MyParticleEmitter GetEmitter();

		MyParticleEffect GetEffect();

		void Serialize(XmlWriter writer);

		void Deserialize(XmlReader reader);

		void Clear();

		void Done();

		IMyParticleGeneration Duplicate(MyParticleEffect effect);

		void Update();

		void MergeAABB(ref BoundingBoxD aabb);

		void SetDirty();

		void SetAnimDirty();

		void Draw(List<MyBillboard> collectedBillboards);
	}
}
