using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FlareDefinition), null)]
	public class MyFlareDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyFlareDefinition_003C_003EActor : IActivator, IActivator<MyFlareDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFlareDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFlareDefinition CreateInstance()
			{
				return new MyFlareDefinition();
			}

			MyFlareDefinition IActivator<MyFlareDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Intensity;

		public Vector2 Size;

		public MySubGlare[] SubGlares;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_FlareDefinition myObjectBuilder_FlareDefinition = (MyObjectBuilder_FlareDefinition)builder;
			Intensity = (myObjectBuilder_FlareDefinition.Intensity ?? 1f);
			Size = (myObjectBuilder_FlareDefinition.Size ?? new Vector2(1f, 1f));
			SubGlares = new MySubGlare[myObjectBuilder_FlareDefinition.SubGlares.Length];
			int num = 0;
			MySubGlare[] subGlares = myObjectBuilder_FlareDefinition.SubGlares;
			for (int i = 0; i < subGlares.Length; i++)
			{
				MySubGlare mySubGlare = subGlares[i];
				SubGlares[num] = mySubGlare;
				SubGlares[num].Color = mySubGlare.Color.ToLinearRGB();
				num++;
			}
		}
	}
}
