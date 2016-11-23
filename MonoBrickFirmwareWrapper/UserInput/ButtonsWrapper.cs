using System;
using System.Threading;
using MonoBrickFirmware.UserInput;

namespace MonoBrickFirmwareWrapper.UserInput
{
	/// <summary>
	/// A wrapper class of <see cref="Buttons"/>.
	/// </summary>
	public static class ButtonsWrapper
	{
		/// <summary>
		/// A wrapper of <see cref="Buttons.GetStates"/>.
		/// </summary>
		public static Func<Buttons.ButtonStates> GetStates { get; } = Buttons.GetStates;

		/// <summary>
		/// A wrapper of <see cref="Buttons.WaitForKeyRelease(CancellationToken)"/>.
		/// </summary>
		public static Action<CancellationToken> WaitForKeyReleaseCancelable { get; } = Buttons.WaitForKeyRelease;

		/// <summary>
		/// A wrapper of <see cref="Buttons.WaitForKeyRelease"/>.
		/// </summary>
		public static Action WaitForKeyRelease { get; } = Buttons.WaitForKeyRelease;

		/// <summary>
		/// A wrapper of <see cref="Buttons.GetKeypress(CancellationToken)"/>.
		/// </summary>
		public static Func<CancellationToken, Buttons.ButtonStates> GetKeypressCancelable { get; } = Buttons.GetKeypress;

		/// <summary>
		/// A wrapper of <see cref="Buttons.GetKeypress"/>.
		/// </summary>
		public static Func<Buttons.ButtonStates> GetKeypress { get; } = Buttons.GetKeypress;

		/// <summary>
		/// A wrapper of <see cref="Buttons.LedPattern(int)"/>
		/// </summary>
		public static Action<int> LedPattern { get; } = Buttons.LedPattern;
	}
}
