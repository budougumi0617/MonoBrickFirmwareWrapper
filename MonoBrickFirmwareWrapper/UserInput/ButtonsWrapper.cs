//
// LcdConsoleWrapper.cs
//
// Author:
//       Geroshabu
//
// Copyright (c) 2016 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
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
