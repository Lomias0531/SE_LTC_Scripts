using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// An animation clip is a set of keyframes with associated bones.
	/// VRAGE TODO: Any link to actual animation collection? Or at least to rest pose? Please?
	/// </summary>
	public class MyAnimationClip
	{
		/// <summary>
		/// BoneState is a rotation and translation of the bone.
		/// It would be easy to extend this to include scaling as well.
		/// </summary>
		public class BoneState
		{
			public Quaternion Rotation = Quaternion.Identity;

			public Vector3 Translation;
		}

		/// <summary>
		/// An Keyframe extends rotation and translation of the bone by specifying time of the event.
		/// </summary>
		/// Beware, this class is used also in MWM builder and changing it to struct may cause problems during MWM generation.
		public class Keyframe : BoneState
		{
			public double Time;

			public double InvTimeDiff;
		}

		/// <summary>
		/// Keyframes are grouped per bone for an animation clip
		/// </summary>
		public class Bone
		{
			/// <summary>
			/// Each bone has a name so we can associate it with a runtime model
			/// </summary>
			private string m_name = "";

			/// <summary>
			/// The keyframes for this bone
			/// </summary>
			private readonly List<Keyframe> m_keyframes = new List<Keyframe>();

			/// <summary>
			/// The bone name for these keyframes
			/// </summary>
			public string Name
			{
				get
				{
					return m_name;
				}
				set
				{
					m_name = value;
				}
			}

			/// <summary>
			/// The keyframes for this bone
			/// </summary>
			public List<Keyframe> Keyframes => m_keyframes;

			public override string ToString()
			{
				return m_name + " (" + Keyframes.Count + " keys)";
			}
		}

		/// <summary>
		/// The bones for this animation
		/// </summary>
		private List<Bone> bones = new List<Bone>();

		/// <summary>
		/// Name of the animation clip
		/// </summary>
		public string Name;

		/// <summary>
		/// Duration of the animation clip
		/// </summary>
		public double Duration;

		/// <summary>
		/// The bones for this animation clip with their keyframes
		/// </summary>
		public List<Bone> Bones => bones;
	}
}
