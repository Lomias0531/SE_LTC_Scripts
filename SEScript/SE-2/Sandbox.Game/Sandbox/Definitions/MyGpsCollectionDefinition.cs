using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GpsCollectionDefinition), null)]
	public class MyGpsCollectionDefinition : MyDefinitionBase
	{
		public struct MyGpsAction
		{
			public string BlockName;

			public string ActionId;
		}

		public struct MyGpsCoordinate
		{
			public string Name;

			public Vector3D Coords;

			public List<MyGpsAction> Actions;
		}

		private class Sandbox_Definitions_MyGpsCollectionDefinition_003C_003EActor : IActivator, IActivator<MyGpsCollectionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGpsCollectionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGpsCollectionDefinition CreateInstance()
			{
				return new MyGpsCollectionDefinition();
			}

			MyGpsCollectionDefinition IActivator<MyGpsCollectionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyGpsCoordinate> Positions;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GpsCollectionDefinition myObjectBuilder_GpsCollectionDefinition = builder as MyObjectBuilder_GpsCollectionDefinition;
			Positions = new List<MyGpsCoordinate>();
			if (myObjectBuilder_GpsCollectionDefinition.Positions == null || myObjectBuilder_GpsCollectionDefinition.Positions.Length == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			Vector3D coords = Vector3D.Zero;
			StringBuilder stringBuilder2 = new StringBuilder();
			string[] positions = myObjectBuilder_GpsCollectionDefinition.Positions;
			for (int i = 0; i < positions.Length; i++)
			{
				if (!MyGpsCollection.ParseOneGPSExtended(positions[i], stringBuilder, ref coords, stringBuilder2))
				{
					continue;
				}
				MyGpsCoordinate myGpsCoordinate = default(MyGpsCoordinate);
				myGpsCoordinate.Name = stringBuilder.ToString();
				myGpsCoordinate.Coords = coords;
				MyGpsCoordinate item = myGpsCoordinate;
				string text = stringBuilder2.ToString();
				if (!string.IsNullOrWhiteSpace(text))
				{
					string[] array = text.Split(new char[1]
					{
						':'
					});
					for (int j = 0; j < array.Length / 2; j++)
					{
						string text2 = array[2 * j];
						string text3 = array[2 * j + 1];
						if (!string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3))
						{
							if (item.Actions == null)
							{
								item.Actions = new List<MyGpsAction>();
							}
							item.Actions.Add(new MyGpsAction
							{
								BlockName = text2,
								ActionId = text3
							});
						}
					}
				}
				Positions.Add(item);
			}
		}
	}
}
