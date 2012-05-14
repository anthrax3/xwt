// 
// ProgressBarSample.cs
//  
// Author:
//	   Andres G. Aragoneses <knocte@gmail.com>
// 
// Copyright (c) 2012 Andres G. Aragoneses
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
using System.IO;
using System.Timers;
using Xwt;
using Xwt.Drawing;

namespace Samples
{
	public class ProgressBarSample : VBox
	{
		Timer timer = new Timer (100);
		ProgressBar determinateProgressBar;
		ProgressBar indeterminateProgressBar;

		public ProgressBarSample ()
		{
			indeterminateProgressBar = new ProgressBar();
			PackStart(indeterminateProgressBar, BoxMode.FillAndExpand);
			indeterminateProgressBar.Indeterminate = true;

			timer.Elapsed += Increase;
			determinateProgressBar = new ProgressBar();
			determinateProgressBar.Fraction = 0.0;
			PackStart(determinateProgressBar, BoxMode.FillAndExpand);
			timer.Start();

			var table = new Table ();
			var artistX = new Label (" Artist X");
			artistX.BackgroundColor = Xwt.Drawing.Colors.White;
			var artistY = new Label (" Artist Y");
			artistY.BackgroundColor = Xwt.Drawing.Colors.LightGray;
			var artistZ = new Label (" Artist Z");
			artistZ.BackgroundColor = Xwt.Drawing.Colors.White;

			var titleX = new Label (" Title X");
			titleX.BackgroundColor = Xwt.Drawing.Colors.White;
			var titleY = new Label (" Title Y");
			titleY.BackgroundColor = Xwt.Drawing.Colors.LightGray;
			var titleZ = new Label (" Title Z");
			titleZ.BackgroundColor = Xwt.Drawing.Colors.White;

			var pgBarX = new ProgressBar ();
			pgBarX.Indeterminate = true;
			var pgBarY = new ProgressBar ();
			pgBarY.Indeterminate = true;
			var pgBarZ = new ProgressBar ();
			pgBarZ.Indeterminate = true;

			var fA = CreateFrame (" Artist");
			table.Attach (fA, 0, 0);

			var fT = CreateFrame (" Title");
			table.Attach (fT, 1, 0);

			var fS = CreateFrame (" Status");
			table.Attach (fS, 2, 0);

			table.Attach (artistX, 0, 1);
			table.Attach (titleX, 1, 1);
			table.Attach (pgBarX, 2, 1);

			table.Attach (artistY, 0, 2);
			table.Attach (titleY, 1, 2);
			table.Attach (pgBarY, 2, 2);

			table.Attach (artistZ, 0, 3);
			table.Attach (titleZ, 1, 3);
			table.Attach (pgBarZ, 2, 3);

			table.DefaultRowSpacing = 0;
			table.DefaultColumnSpacing = 0;
			PackStart (table, BoxMode.Fill);
		}

		public void Increase (object sender, ElapsedEventArgs args)
		{
			double nextFraction;
			double? currentFraction = determinateProgressBar.Fraction;
			if (currentFraction != null && currentFraction.Value >= 0.0 && currentFraction.Value <= 0.9) {
				nextFraction = currentFraction.Value + 0.1;
			} else {
				nextFraction = 0.0;
			}
			Application.Invoke ( () => determinateProgressBar.Fraction = nextFraction );
		}

		Widget CreateFrame(string text) {
			Widget widget = null;
			if (RunningPlatform () == Platform.Linux) {
				widget = new Frame ();
				((Frame)widget).BorderColor = Xwt.Drawing.Colors.Gray;
				((Frame)widget).Content = new Label(text);
			}
			else if (RunningPlatform () == Platform.Mac) {
				widget = new Frame (FrameType.Custom);
				((Frame)widget).BorderColor = Xwt.Drawing.Colors.LightGray;
				((Frame)widget).Content = new Label(text);
			}
			else if (RunningPlatform() == Platform.Windows)
			{
				widget = new Button(text);
			}
			
			return widget;
		}

		public enum Platform
		{
			Windows,
			Linux,
			Mac
		}

		public static Platform RunningPlatform()
		{
			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.Unix:
					// Well, there are chances MacOSX is reported as Unix instead of MacOSX.
					// Instead of platform check, we'll do a feature checks (Mac specific root folders)
					if (Directory.Exists("/Applications")
						& Directory.Exists("/System")
						& Directory.Exists("/Users")
						& Directory.Exists("/Volumes"))
						return Platform.Mac;
					else
						return Platform.Linux;
	
				case PlatformID.MacOSX:
					return Platform.Mac;
	
				default:
					return Platform.Windows;
			}
		}
	}
}

