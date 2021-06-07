using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyModel
	{
		int UniqueId
		{
			get;
		}

		int DataVersion
		{
			get;
		}

		BoundingSphere BoundingSphere
		{
			get;
		}

		BoundingBox BoundingBox
		{
			get;
		}

		Vector3 BoundingBoxSize
		{
			get;
		}

		Vector3 BoundingBoxSizeHalf
		{
			get;
		}

		Vector3I[] BoneMapping
		{
			get;
		}

		float PatternScale
		{
			get;
		}

		float ScaleFactor
		{
			get;
		}

		string AssetName
		{
			get;
		}

		int GetTrianglesCount();

		int GetVerticesCount();

		/// <summary>
		/// Gets the dummies from the model
		/// </summary>
		/// <param name="dummies">Dictionary of dummies, can be null to just return count</param>
		/// <returns>Number of dummies in model</returns>
		int GetDummies(IDictionary<string, IMyModelDummy> dummies);
	}
}
