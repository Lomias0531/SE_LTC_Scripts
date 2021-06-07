namespace BulletXNA.BulletCollision
{
    using BulletXNA.LinearMath;
    using System;

    public class BT_QUANTIZED_BVH_NODE
    {
        public UShortVector3 m_quantizedAabbMin;
        public UShortVector3 m_quantizedAabbMax;
        public int[] m_escapeIndexOrDataIndex;

        public int[] GetDataIndices() => 
            this.m_escapeIndexOrDataIndex;

        public int GetEscapeIndex()
        {
            if (this.m_escapeIndexOrDataIndex != null)
            {
                return -this.m_escapeIndexOrDataIndex[0];
            }
            return 0;
        }

        public bool IsLeafNode() => 
            (this.m_escapeIndexOrDataIndex[0] >= 0);

        public void SetDataIndices(int[] indices)
        {
            this.m_escapeIndexOrDataIndex = indices;
        }

        public void SetEscapeIndex(int index)
        {
            int[] numArray1 = new int[] { -index };
            this.m_escapeIndexOrDataIndex = numArray1;
        }

        public bool TestQuantizedBoxOverlapp(ref UShortVector3 quantizedMin, ref UShortVector3 quantizedMax) => 
            ((((this.m_quantizedAabbMin.X <= quantizedMax.X) && (this.m_quantizedAabbMax.X >= quantizedMin.X)) && ((this.m_quantizedAabbMin.Y <= quantizedMax.Y) && (this.m_quantizedAabbMax.Y >= quantizedMin.Y))) && ((this.m_quantizedAabbMin.Z <= quantizedMax.Z) && (this.m_quantizedAabbMax.Z >= quantizedMin.Z)));
    }
}

