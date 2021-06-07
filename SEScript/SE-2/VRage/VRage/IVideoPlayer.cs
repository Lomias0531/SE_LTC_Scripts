using System;

namespace VRage
{
	public interface IVideoPlayer
	{
		int VideoWidth
		{
			get;
		}

		int VideoHeight
		{
			get;
		}

		float Volume
		{
			get;
			set;
		}

		VideoState CurrentState
		{
			get;
		}

		IntPtr TextureSrv
		{
			get;
		}

		void Init(string filename);

		void Dispose();

		void Play();

		void Stop();

		void Update();
	}
}
