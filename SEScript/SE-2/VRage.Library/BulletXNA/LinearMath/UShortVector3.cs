namespace BulletXNA.LinearMath
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UShortVector3
    {
        public ushort X;
        public ushort Y;
        public ushort Z;
        public ushort this[int i]
        {
            get
            {
                if (i == 0)
                {
                    return this.X;
                }
                if (i == 1)
                {
                    return this.Y;
                }
                if (i == 2)
                {
                    return this.Z;
                }
                return 0;
            }
            set
            {
                if (i == 0)
                {
                    this.X = value;
                }
                else if (i == 1)
                {
                    this.Y = value;
                }
                else if (i == 2)
                {
                    this.Z = value;
                }
            }
        }
    }
}

