//
// LcdWrapper.cs
//
// Author:
//       budougumi0617
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
    /// A wrapper class of <see cref="Lcd"/>.
    /// We are able to change return value by <see cref="System.Reflection"/> classes.
    /// 
    /// TODO Need to add description to each members.
    /// TASK  I think better design called <see cref="EV3Lcd"/> directly...
    /// </summary>
    public static class LcdWrapper
    {
        private static readonly Func<int> width = () => Lcd.Width;
        public static int Width { get { return width(); } }

        private static readonly Func<int> height = () => Lcd.Height;
        public static int Height { get { return height(); } }

        private static readonly Action<int, int, bool> setPixel = Lcd.SetPixel;
        public static Action<int, int, bool> SetPixel { get { return setPixel; } }

        private static readonly Func<int, int, bool> isPixelSet = Lcd.IsPixelSet;
        public static Func<int, int, bool> IsPixelSet { get { return isPixelSet; } }

        private static readonly Action<int> update = Lcd.Update;
        public static Action<int> Update { get { return update; } }

        private static readonly Action saveScreen = Lcd.SaveScreen;
        public static Action SaveScreen { get { return saveScreen; } }

        private static readonly Action loadScreen = Lcd.LoadScreen;
        public static Action LoadScreen { get { return loadScreen; } }

        private static readonly Action<byte[]> showPicture = Lcd.ShowPicture;
        public static Action<byte[]> ShowPicture { get { return showPicture; } }


        private static readonly Action takeScreenShot = Lcd.TakeScreenShot;
        public static void TakeScreenShot()
        {
            takeScreenShot();
        }

        private static readonly Action<string, string> takeScreenShotWithParam = Lcd.TakeScreenShot;
        public static void TakeScreenShot(string directory, string fileName)
        {
            takeScreenShotWithParam(directory, fileName);
        }

        private static readonly Action<int, int> clearLines = Lcd.ClearLines;
        public static Action<int, int> ClearLines { get { return clearLines; } }

        private static readonly Action clear = Lcd.Clear;
        public static Action Clear { get { return clear; } }

        private static readonly Action<Point, int, bool> drawVLine = Lcd.DrawVLine;
        public static Action<Point, int, bool> DrawVLine { get { return drawVLine; } }

        private static readonly Action<Point, int, bool> drawHLine = Lcd.DrawHLine;
        public static Action<Point, int, bool> DrawHLine { get { return drawHLine; } }


        private static readonly Action<Bitmap, Point> drawBitmapToBitmap = Lcd.DrawBitmap;
        public static void DrawBitmap(Bitmap bm, Point p)
        {
            drawBitmapToBitmap(bm, p);
        }

        private static readonly Action<BitStreamer, Point, uint, uint, bool> drawBitmapToBitStreamer = Lcd.DrawBitmap;
        public static void DrawBitmap(BitStreamer bs, Point p, uint xsize, uint ysize, bool color)
        {
            drawBitmapToBitStreamer(bs, p, xsize, ysize, color);
        }


        private static readonly Func<Font, string, int> textWidth = Lcd.TextWidth;
        public static Func<Font, string, int> TextWidth { get { return textWidth; } }

        private static Action<Font, Point, string, bool> writeText = Lcd.WriteText;
        public static Action<Font, Point, string, bool> WriteTextAction { get { return writeText; } }

        private static Action<Rectangle, Lcd.ArrowOrientation, bool> drawArrow = Lcd.DrawArrow;
        public static Action<Rectangle, Lcd.ArrowOrientation, bool> DrawArrow { get { return drawArrow; } }

        private static readonly Action<Font, Rectangle, string, bool> writeTextBox = Lcd.WriteTextBox;
        public static void WriteTextBox(Font f, Rectangle r, string text, bool color)
        {
            writeTextBox(f, r, text, color);
        }

        private static readonly Action<Font, Rectangle, string, bool, Lcd.Alignment> writeTextBoxWithAlignment = Lcd.WriteTextBox;
        public static void WriteTextBox(Font f, Rectangle r, string text, bool color, Lcd.Alignment aln)
        {
            writeTextBoxWithAlignment(f, r, text, color, aln);
        }

        private static readonly Action<Point, ushort, bool, bool> drawCircle = Lcd.DrawCircle;
        public static Action<Point, ushort, bool, bool> DrawCircle { get { return drawCircle; } }

        private static readonly Action<Point, ushort, ushort, bool, bool> drawEllipse = Lcd.DrawEllipse;
        public static Action<Point, ushort, ushort, bool, bool> DrawEllipse { get { return drawEllipse; } }


        private static readonly Action<Point, Point, bool> drawLine = Lcd.DrawLine;
        public static Action<Point, Point, bool> DrawLine { get { return drawLine; } }

        private static readonly Action<Rectangle, bool, bool> drawRectangle = Lcd.DrawRectangle;
        public static Action<Rectangle, bool, bool> DrawRectangle { get { return drawRectangle; } }
    }
}
