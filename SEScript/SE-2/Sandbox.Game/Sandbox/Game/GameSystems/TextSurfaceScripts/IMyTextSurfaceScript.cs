using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	public interface IMyTextSurfaceScript : IDisposable
	{
		IMyTextSurface Surface
		{
			get;
		}

		IMyCubeBlock Block
		{
			get;
		}

		ScriptUpdate NeedsUpdate
		{
			get;
		}

		Color ForegroundColor
		{
			get;
		}

		Color BackgroundColor
		{
			get;
		}

		void Run();
	}
}
