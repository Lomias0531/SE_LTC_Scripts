using System.Collections.Generic;
using System.IO;
using VRageMath;

namespace VRage.Voxels
{
	public interface IMyStorageDataProvider
	{
		int SerializedSize
		{
			get;
		}

		void WriteTo(Stream stream);

		void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat);

		void ReadRange(MyStorageData target, MyStorageDataTypeFlags dataType, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod);

		/// Read range of data.
		///
		/// The data for the request, providing any optimizations that are requested in there.
		void ReadRange(ref MyVoxelDataRequest request, bool detectOnly = false);

		void DebugDraw(ref MatrixD worldMatrix);

		void ReindexMaterials(Dictionary<byte, byte> oldToNewIndexMap);

		ContainmentType Intersect(BoundingBoxI box, int lod);

		/// Intersect line with storage.
		///
		/// Returnas the tightest line interval that does intersect the storage.
		/// The precision of this method varies from storage to storage.
		///
		/// The offsets are normalised.
		bool Intersect(ref LineD line, out double startOffset, out double endOffset);

		void Close();

		/// <summary>
		/// Post-process the mesh generated from the data in this storage.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="dataTypes">The types of data requested for the mesh.</param>
		void PostProcess(VrVoxelMesh mesh, MyStorageDataTypeFlags dataTypes);
	}
}
