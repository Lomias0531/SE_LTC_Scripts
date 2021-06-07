namespace VRage.Profiler
{
	public enum RenderProfilerCommand
	{
		/// <summary>
		/// Only enables the profiler, used by the developer debug window
		/// </summary>
		Enable,
		/// <summary>
		/// Toggles profiler enabled/disabled state, doesn't reset profiler level
		/// </summary>
		ToggleEnabled,
		JumpToLevel,
		/// <summary>
		/// Jumps all the way to the root element
		/// </summary>
		JumpToRoot,
		Pause,
		NextFrame,
		PreviousFrame,
		/// <summary>
		/// Disables the current selection again
		/// </summary>
		DisableFrameSelection,
		NextThread,
		PreviousThread,
		IncreaseLevel,
		DecreaseLevel,
		IncreaseLocalArea,
		DecreaseLocalArea,
		IncreaseRange,
		DecreaseRange,
		Reset,
		SetLevel,
		/// <summary>
		/// Changes the profiler's sorting order, see ProfilerSortingOptions for the possible sorting options
		/// </summary>
		ChangeSortingOrder,
		/// <summary>
		/// Copies the current path to clipboard
		/// </summary>
		CopyPathToClipboard,
		/// <summary>
		/// Tries to navigate to the path in the clipboard
		/// </summary>
		TryGoToPathInClipboard,
		GetFomServer,
		GetFromClient,
		SaveToFile,
		LoadFromFile,
		SwapBlockOptimized,
		ToggleOptimizationsEnabled,
		ResetAllOptimizations,
		SwitchBlockRender,
		SwitchGraphContent,
		SwitchShallowProfile,
		ToggleAutoScale,
		SwitchAverageTimes,
		SubtractFromFile,
		EnableAutoScale
	}
}
