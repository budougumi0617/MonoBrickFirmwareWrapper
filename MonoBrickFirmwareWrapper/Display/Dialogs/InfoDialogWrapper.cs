//
// InfoDialogWrapper.cs
//
// Author:
//       budougumi0617 <budougumi0617@gmail.com>
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
using MonoBrickFirmware.Display.Dialogs;

namespace MonoBrickFirmwareWrapper
{

	/// <summary>
	/// Wrap InfoDialog
	/// </summary>
	// UNDONE Add comments.
	// UNDONE Need to validate logic by tests.
	public class InfoDialogWrapper :IInfoDialog
	{
		InfoDialog instance;

		public InfoDialogWrapper(string message, string title)
		{
			instance = new InfoDialog (message,title);
			// Set implementation from instance.
			onShow = instance.OnShow;
			onExit = instance.OnExit;
			hide = instance.Hide;
			show = instance.Show;
		}

		private Action onShow;
		public Action OnShow
		{
			get
			{
				return onShow;
			}
			set
			{
				onShow += value;
			}
		}

		private Action onExit;
		public Action OnExit
		{
			get
			{
				return onExit;
			}
			set
			{
				onExit += value;
			}
		}

		private readonly Func<bool> show;
		public bool Show()
		{
			return show();
		}

		private readonly Action hide;
		public void Hide()
		{
			hide();
		}
	}
}