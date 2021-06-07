using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_PrefabDefinition), null)]
	public class MyPrefabDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyPrefabDefinition_003C_003EActor : IActivator, IActivator<MyPrefabDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPrefabDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPrefabDefinition CreateInstance()
			{
				return new MyPrefabDefinition();
			}

			MyPrefabDefinition IActivator<MyPrefabDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyObjectBuilder_CubeGrid[] m_cubeGrids;

		private BoundingSphere m_boundingSphere;

		private BoundingBox m_boundingBox;

		public string PrefabPath;

		public bool Initialized;

		public MyObjectBuilder_CubeGrid[] CubeGrids
		{
			get
			{
				if (!Initialized)
				{
					MyDefinitionManager.Static.ReloadPrefabsFromFile(PrefabPath);
				}
				return m_cubeGrids;
			}
		}

		public BoundingSphere BoundingSphere
		{
			get
			{
				if (!Initialized)
				{
					MyDefinitionManager.Static.ReloadPrefabsFromFile(PrefabPath);
				}
				return m_boundingSphere;
			}
		}

		public BoundingBox BoundingBox
		{
			get
			{
				if (!Initialized)
				{
					MyDefinitionManager.Static.ReloadPrefabsFromFile(PrefabPath);
				}
				return m_boundingBox;
			}
		}

		public MyEnvironmentTypes EnvironmentType
		{
			get;
			set;
		}

		public string TooltipImage
		{
			get;
			set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase baseBuilder)
		{
			base.Init(baseBuilder);
			MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = baseBuilder as MyObjectBuilder_PrefabDefinition;
			PrefabPath = myObjectBuilder_PrefabDefinition.PrefabPath;
			Initialized = false;
		}

		public void InitLazy(MyObjectBuilder_DefinitionBase baseBuilder)
		{
			MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = baseBuilder as MyObjectBuilder_PrefabDefinition;
			Icons = myObjectBuilder_PrefabDefinition.Icons;
			DisplayNameString = baseBuilder.DisplayName;
			DescriptionString = baseBuilder.Description;
			EnvironmentType = myObjectBuilder_PrefabDefinition.EnvironmentType;
			TooltipImage = myObjectBuilder_PrefabDefinition.TooltipImage;
			if (myObjectBuilder_PrefabDefinition.CubeGrid != null || myObjectBuilder_PrefabDefinition.CubeGrids != null)
			{
				if (myObjectBuilder_PrefabDefinition.CubeGrid != null)
				{
					m_cubeGrids = new MyObjectBuilder_CubeGrid[1]
					{
						myObjectBuilder_PrefabDefinition.CubeGrid
					};
				}
				else
				{
					m_cubeGrids = myObjectBuilder_PrefabDefinition.CubeGrids;
				}
				m_boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
				m_boundingBox = BoundingBox.CreateInvalid();
				MyObjectBuilder_CubeGrid[] cubeGrids = m_cubeGrids;
				foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in cubeGrids)
				{
					BoundingBox boundingBox = myObjectBuilder_CubeGrid.CalculateBoundingBox();
					Matrix worldMatrix = myObjectBuilder_CubeGrid.PositionAndOrientation.HasValue ? ((Matrix)myObjectBuilder_CubeGrid.PositionAndOrientation.Value.GetMatrix()) : Matrix.Identity;
					m_boundingBox.Include(boundingBox.Transform(worldMatrix));
				}
				m_boundingSphere = BoundingSphere.CreateFromBoundingBox(m_boundingBox);
				cubeGrids = m_cubeGrids;
				foreach (MyObjectBuilder_CubeGrid obj in cubeGrids)
				{
					obj.CreatePhysics = true;
					obj.XMirroxPlane = null;
					obj.YMirroxPlane = null;
					obj.ZMirroxPlane = null;
				}
				Initialized = true;
			}
		}
	}
}
