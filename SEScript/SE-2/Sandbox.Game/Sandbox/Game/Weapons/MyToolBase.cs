using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	public class MyToolBase : MyDeviceBase
	{
		protected Vector3 m_positionMuzzleLocal;

		protected Vector3D m_positionMuzzleWorld;

		public MyToolBase()
			: this(Vector3.Zero, MatrixD.Identity)
		{
		}

		public MyToolBase(Vector3 localMuzzlePosition, MatrixD matrix)
		{
			m_positionMuzzleLocal = localMuzzlePosition;
			OnWorldPositionChanged(matrix);
		}

		public void OnWorldPositionChanged(MatrixD matrix)
		{
			m_positionMuzzleWorld = Vector3D.Transform(m_positionMuzzleLocal, matrix);
		}

		public override bool CanSwitchAmmoMagazine()
		{
			return false;
		}

		public override bool SwitchToNextAmmoMagazine()
		{
			return false;
		}

		public override bool SwitchAmmoMagazineToNextAvailable()
		{
			return false;
		}

		public override Vector3D GetMuzzleLocalPosition()
		{
			return m_positionMuzzleLocal;
		}

		public override Vector3D GetMuzzleWorldPosition()
		{
			return m_positionMuzzleWorld;
		}

		public MyObjectBuilder_ToolBase GetObjectBuilder()
		{
			MyObjectBuilder_ToolBase myObjectBuilder_ToolBase = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolBase>();
			myObjectBuilder_ToolBase.InventoryItemId = base.InventoryItemId;
			return myObjectBuilder_ToolBase;
		}

		public void Init(MyObjectBuilder_ToolBase objectBuilder)
		{
			Init((MyObjectBuilder_DeviceBase)objectBuilder);
		}
	}
}
