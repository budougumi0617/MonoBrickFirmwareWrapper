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
using MonoBrickFirmware.Display;

namespace MonoBrickFirmwareWrapper.Display
{
	/// <summary>
	/// A wrapper class of <see cref="LcdConsole"/>.
	/// </summary>
	public static class LcdConsoleWrapper
	{
		/// <summary>
		/// Delegate type of <see cref="LcdConsole.WriteLine"/>.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="arg">An array of objects to write using format.</param>
		public delegate void WriteLineDelegate(string format, params object[] arg);

		/// <summary>
		/// A wrapper of <see cref="LcdConsole.WriteLine"/>.
		/// </summary>
		public static WriteLineDelegate WriteLine { get; } = LcdConsole.WriteLine;

		/// <summary>
		/// A wrapper of <see cref="LcdConsole.Clear"/>.
		/// </summary>
		public static Action Clear { get; } = LcdConsole.Clear;
	}
}
