using System;

namespace VRageRender.Utils
{
	[Flags]
	public enum MyWEMDebugDrawMode
	{
		NONE = 0x0,
		LINES = 0x1,
		EDGES = 0x2,
		LINES_DEPTH = 0x4,
		FACES = 0x8,
		VERTICES = 0x10,
		VERTICES_DETAILED = 0x20,
		NORMALS = 0x40
	}
}
