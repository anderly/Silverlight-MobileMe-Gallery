using System;

namespace Gallery.Contracts
{
	public interface IPlayer
	{
		bool IsSmoothStreaming { get; set; }
		Uri Source { get; set; }
		double BufferingTime { get; set; }
		bool AutoLoad { get; set; }

		void Initialize();
	}
}
