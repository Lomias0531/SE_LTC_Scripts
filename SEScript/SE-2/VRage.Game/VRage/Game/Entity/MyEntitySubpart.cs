using System.IO;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.Entity
{
	public class MyEntitySubpart : MyEntity
	{
		public struct Data
		{
			public string Name;

			public string File;

			public Matrix InitialTransform;
		}

		private class VRage_Game_Entity_MyEntitySubpart_003C_003EActor : IActivator, IActivator<MyEntitySubpart>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntitySubpart();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntitySubpart CreateInstance()
			{
				return new MyEntitySubpart();
			}

			MyEntitySubpart IActivator<MyEntitySubpart>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static bool GetSubpartFromDummy(string modelPath, string dummyName, MyModelDummy dummy, ref Data outData)
		{
			if (!dummyName.Contains("subpart_"))
			{
				return false;
			}
			if (dummy.CustomData.ContainsKey("file"))
			{
				string str = Path.Combine(Path.GetDirectoryName(modelPath), (string)dummy.CustomData["file"]);
				str += ".mwm";
				outData = new Data
				{
					Name = dummyName.Substring("subpart_".Length),
					File = str,
					InitialTransform = Matrix.Normalize(dummy.Matrix)
				};
				return true;
			}
			return false;
		}

		public MyEntitySubpart()
		{
			base.Save = false;
		}
	}
}
