using System;
using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContainerDropSystemDefinition), null)]
	public class MyContainerDropSystemDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MyContainerDropSystemDefinition_003C_003EActor : IActivator, IActivator<MyContainerDropSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContainerDropSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContainerDropSystemDefinition CreateInstance()
			{
				return new MyContainerDropSystemDefinition();
			}

			MyContainerDropSystemDefinition IActivator<MyContainerDropSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[Obsolete]
		public float PersonalContainerRatio;

		[Obsolete]
		public int ContainerDropTime;

		public float PersonalContainerDistMin;

		public float PersonalContainerDistMax;

		public float CompetetiveContainerDistMin;

		public float CompetetiveContainerDistMax;

		public int CompetetiveContainerGPSTimeOut;

		public int CompetetiveContainerGridTimeOut;

		public int PersonalContainerGridTimeOut;

		public Color CompetetiveContainerGPSColorFree;

		public Color CompetetiveContainerGPSColorClaimed;

		public Color PersonalContainerGPSColor;

		public string ContainerAudioCue;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContainerDropSystemDefinition myObjectBuilder_ContainerDropSystemDefinition = (MyObjectBuilder_ContainerDropSystemDefinition)builder;
			PersonalContainerRatio = 0.9f;
			ContainerDropTime = (int)Math.Max(myObjectBuilder_ContainerDropSystemDefinition.ContainerDropTime * 60f, 1f);
			PersonalContainerDistMin = myObjectBuilder_ContainerDropSystemDefinition.PersonalContainerDistMin * 1000f;
			PersonalContainerDistMax = myObjectBuilder_ContainerDropSystemDefinition.PersonalContainerDistMax * 1000f;
			CompetetiveContainerDistMin = myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerDistMin * 1000f;
			CompetetiveContainerDistMax = myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerDistMax * 1000f;
			CompetetiveContainerGPSTimeOut = (int)myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSTimeOut * 60;
			CompetetiveContainerGridTimeOut = (int)myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGridTimeOut * 60;
			PersonalContainerGridTimeOut = (int)myObjectBuilder_ContainerDropSystemDefinition.PersonalContainerGridTimeOut * 60;
			CompetetiveContainerGPSColorFree = new Color(myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSColorFree.R, myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSColorFree.G, myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSColorFree.B);
			CompetetiveContainerGPSColorClaimed = new Color(myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSColorClaimed.R, myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSColorClaimed.G, myObjectBuilder_ContainerDropSystemDefinition.CompetetiveContainerGPSColorClaimed.B);
			PersonalContainerGPSColor = new Color(myObjectBuilder_ContainerDropSystemDefinition.PersonalContainerGPSColor.R, myObjectBuilder_ContainerDropSystemDefinition.PersonalContainerGPSColor.G, myObjectBuilder_ContainerDropSystemDefinition.PersonalContainerGPSColor.B);
			ContainerAudioCue = myObjectBuilder_ContainerDropSystemDefinition.ContainerAudioCue;
		}
	}
}
