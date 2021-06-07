using VRage;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MyCubePart
	{
		public MyCubeInstanceData InstanceData;

		public MyModel Model;

		public MyStringHash SkinSubtypeId;

		public void Init(MyModel model, MyStringHash skinSubtypeId, Matrix matrix, float rescaleModel = 1f)
		{
			Model = model;
			model.Rescale(rescaleModel);
			model.LoadData();
			SkinSubtypeId = skinSubtypeId;
			InstanceData.LocalMatrix = matrix;
		}
	}
}
