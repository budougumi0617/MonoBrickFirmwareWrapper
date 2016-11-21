//
// IMotor.cs
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
namespace MonoBrickFirmwareWrapper.Movement
{

	/// <summary>
	/// Interface for Motor classes.
	/// <seealso cref="MonoBrickFirmware.Movement.MotorBase"/>
	/// </summary>
	public interface IMotor
	{
		/// <summary>
		/// Determines whether the motor(s) are running.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this motor is moving; otherwise, <c>false</c>.
		/// </returns>
		bool IsRunning();

		/// <summary>
		/// Brake the motor (is still on but does not move)
		/// </summary>
		void Brake();

		/// <summary>
		/// Turn the motor off
		/// </summary>
		void Off();

		/// <summary>
		/// Sets the power of the motor.
		/// </summary>
		/// <param name="power">Power to use.</param>
		void SetPower(sbyte power);

	}
}
