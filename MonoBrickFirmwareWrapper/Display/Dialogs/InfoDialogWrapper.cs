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
	public class InfoDialogWrapper : InfoDialog
	{


		public InfoDialogWrapper(string message, string title) : base(message, title)
		{
			// Set implementation from Super class.
			onShow = base.OnShow;
			onExit = base.OnExit;
			hide = base.Hide;
			show = base.Show;
		}

		private Action onShow;
		new public Action OnShow
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
		new public Action OnExit
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
		new public bool Show()
		{
			return show();
		}

		private readonly Action hide;
		new public void Hide()
		{
			hide();
		}


		#region Wrap protected methods
		// TODO Do not need below wrapper methods?
		new protected void OnDrawContent()
		{
			base.OnDrawContent();
		}

		new protected void DrawText()
		{
			base.DrawText();
		}

		new protected void WriteTextOnLine(string text, int lineIndex, bool color = true, Lcd.Alignment alignment = Lcd.Alignment.Center)
		{
			base.WriteTextOnLine(text, lineIndex, color, alignment);
		}

		new protected void DrawCenterButton(string text, bool color)
		{
			base.DrawCenterButton(text, color);
		}
		new protected void DrawCenterButton(string text, bool color, int textSize)
		{
			base.DrawCenterButton(text, color, textSize);
		}
		new protected void DrawLeftButton(string text, bool color)
		{
			base.DrawLeftButton(text, color);
		}

		new protected void DrawLeftButton(string text, bool color, int textSize)
		{
			base.DrawLeftButton(text, color, textSize);
		}

		new protected void DrawRightButton(string text, bool color)
		{
			base.DrawRightButton(text, color);
		}

		new protected void DrawRightButton(string text, bool color, int textSize)
		{
			base.DrawRightButton(text, color, textSize);
		}

		new protected void WriteTitle()
		{
			base.WriteTitle();
		}

		new protected void WriteTextOnDialog(string text)
		{
			base.WriteTextOnDialog(text);
		}

		new protected void ClearContent()
		{
			base.ClearContent();
		}
		#endregion
	}
}