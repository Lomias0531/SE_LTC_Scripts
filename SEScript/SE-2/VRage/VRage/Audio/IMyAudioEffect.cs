namespace VRage.Audio
{
	public interface IMyAudioEffect
	{
		bool AutoUpdate
		{
			get;
			set;
		}

		IMySourceVoice OutputSound
		{
			get;
		}

		bool Finished
		{
			get;
		}

		void Update(int stepInMsec);

		void SetPosition(float msecs);

		void SetPositionRelative(float position);
	}
}
