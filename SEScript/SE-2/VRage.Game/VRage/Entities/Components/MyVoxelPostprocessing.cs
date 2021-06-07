using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Voxels;

namespace VRage.Entities.Components
{
	public abstract class MyVoxelPostprocessing
	{
		private static MyObjectFactory<VoxelPostprocessingAttribute, MyVoxelPostprocessing> m_objectFactory;

		public static MyObjectFactory<VoxelPostprocessingAttribute, MyVoxelPostprocessing> Factory => m_objectFactory;

		public bool UseForPhysics
		{
			get;
			set;
		}

		static MyVoxelPostprocessing()
		{
			m_objectFactory = new MyObjectFactory<VoxelPostprocessingAttribute, MyVoxelPostprocessing>();
			m_objectFactory.RegisterFromCreatedObjectAssembly();
		}

		/// <summary>
		/// Retrieve the native postprocessing step associated with this.
		/// </summary>
		/// <param name="lod">Prepare the step fo the given lod.</param>
		/// <param name="postprocess"></param>
		/// <returns>The native postprocessing step.</returns>
		public abstract bool Get(int lod, out VrPostprocessing postprocess);

		protected internal virtual void Init(MyObjectBuilder_VoxelPostprocessing builder)
		{
			UseForPhysics = builder.ForPhysics;
		}
	}
}
