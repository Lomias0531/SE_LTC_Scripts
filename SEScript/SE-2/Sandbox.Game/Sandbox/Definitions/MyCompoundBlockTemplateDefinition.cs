using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CompoundBlockTemplateDefinition), null)]
	public class MyCompoundBlockTemplateDefinition : MyDefinitionBase
	{
		public class MyCompoundBlockRotationBinding
		{
			public MyStringId BuildTypeReference;

			public MyBlockOrientation[] Rotations;
		}

		public class MyCompoundBlockBinding
		{
			public MyStringId BuildType;

			public bool Multiple;

			public MyCompoundBlockRotationBinding[] RotationBinds;
		}

		private class Sandbox_Definitions_MyCompoundBlockTemplateDefinition_003C_003EActor : IActivator, IActivator<MyCompoundBlockTemplateDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCompoundBlockTemplateDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCompoundBlockTemplateDefinition CreateInstance()
			{
				return new MyCompoundBlockTemplateDefinition();
			}

			MyCompoundBlockTemplateDefinition IActivator<MyCompoundBlockTemplateDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyCompoundBlockBinding[] Bindings;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CompoundBlockTemplateDefinition myObjectBuilder_CompoundBlockTemplateDefinition = builder as MyObjectBuilder_CompoundBlockTemplateDefinition;
			if (myObjectBuilder_CompoundBlockTemplateDefinition.Bindings != null)
			{
				Bindings = new MyCompoundBlockBinding[myObjectBuilder_CompoundBlockTemplateDefinition.Bindings.Length];
				for (int i = 0; i < myObjectBuilder_CompoundBlockTemplateDefinition.Bindings.Length; i++)
				{
					MyCompoundBlockBinding myCompoundBlockBinding = new MyCompoundBlockBinding();
					myCompoundBlockBinding.BuildType = MyStringId.GetOrCompute((myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].BuildType != null) ? myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].BuildType.ToLower() : null);
					myCompoundBlockBinding.Multiple = myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].Multiple;
					if (myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds != null && myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds.Length != 0)
					{
						myCompoundBlockBinding.RotationBinds = new MyCompoundBlockRotationBinding[myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds.Length];
						for (int j = 0; j < myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds.Length; j++)
						{
							if (myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].Rotations != null && myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].Rotations.Length != 0)
							{
								myCompoundBlockBinding.RotationBinds[j] = new MyCompoundBlockRotationBinding();
								myCompoundBlockBinding.RotationBinds[j].BuildTypeReference = MyStringId.GetOrCompute((myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].BuildTypeReference != null) ? myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].BuildTypeReference.ToLower() : null);
								myCompoundBlockBinding.RotationBinds[j].Rotations = new MyBlockOrientation[myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].Rotations.Length];
								for (int k = 0; k < myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].Rotations.Length; k++)
								{
									myCompoundBlockBinding.RotationBinds[j].Rotations[k] = myObjectBuilder_CompoundBlockTemplateDefinition.Bindings[i].RotationBinds[j].Rotations[k];
								}
							}
						}
					}
					Bindings[i] = myCompoundBlockBinding;
				}
			}
			else
			{
				Bindings = null;
			}
		}
	}
}
