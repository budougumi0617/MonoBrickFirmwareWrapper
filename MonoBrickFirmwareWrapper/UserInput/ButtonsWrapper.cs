using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonoBrickFirmware.UserInput;

namespace MonoBrickFirmwareWrapper.UserInput
{
	public static class ButtonsWrapper
	{
		public static Func<Buttons.ButtonStates> GetStates { get; } = Buttons.GetStates;

		public static Action<CancellationToken> WaitForKeyReleaseCancelable { get; } = Buttons.WaitForKeyRelease;

		public static Action WaitForKeyRelease { get; } = Buttons.WaitForKeyRelease;

		public static Func<CancellationToken, Buttons.ButtonStates> GetKeypressCancelable { get; } = Buttons.GetKeypress;

		public static Func<Buttons.ButtonStates> GetKeypress { get; } = Buttons.GetKeypress;

		public static Action<int> LedPattern { get; } = Buttons.LedPattern;
	}
}
