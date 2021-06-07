using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Game
{
	public class MyCreditsDepartment
	{
		public StringBuilder Name;

		public List<MyCreditsPerson> Persons;

		public string LogoTexture;

		public Vector2? LogoTextureSize;

		public float? LogoScale;

		public float LogoOffsetPre = 0.07f;

		public float LogoOffsetPost = 0.07f;

		public MyCreditsDepartment(string name)
		{
			Name = new StringBuilder(name);
			Persons = new List<MyCreditsPerson>();
		}
	}
}
