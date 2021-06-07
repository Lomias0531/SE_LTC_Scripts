using System.Text;

namespace Sandbox.Game
{
	public class MyCreditsPerson
	{
		public StringBuilder Name;

		public MyCreditsPerson(string name)
		{
			Name = new StringBuilder(name);
		}
	}
}
