//
// SampleTest.cs
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
using MonoBrickFirmware.Movement;
using MonoBrickFirmwareWrapper;

namespace MonoBrickFirmware.Tests
{
	/// <summary>
	/// Sample test class
	/// </summary>
	/// <remarks>
	/// It was described below URL how to use XUnit
	/// https://xunit.github.io/docs/comparisons.html
	/// </remarks>
	public class MainClassTests
	{
		[Xunit.Fact(DisplayName = "We can describe test summary by this attribute."), Xunit.Trait("Category", "Sample")]
		public void MainTest()
		{
			Xunit.Assert.True(true);
			Xunit.Assert.Equal(10, MyClass.SampleMethod());
			var foo = new Object();
			var same = foo;
			Xunit.Assert.Same(foo, same); // Verify their variables are same object.
		}


		[Xunit.Fact(Skip = "If you want to ignore test case, you set this attribute to the test case.")]
		public void IgnoreTest()
		{
			Xunit.Assert.True(false, "Expected this test case does not be executed.");
			MyClass.Main(new string[] { "" });
			Motor dummy = new Motor(MotorPort.OutA);
			dummy.GetSpeed();
		}
	}
}

