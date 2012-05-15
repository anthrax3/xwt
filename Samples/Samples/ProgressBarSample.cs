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

			var artist = new DataField<string> ();
			var title = new DataField<string> ();
			var status = new DataField<Widget> ();
			var view = new CustomTreeView ();
			var store = new CustomTreeView.TreeStore(artist, title, status);
		
			view.Columns.Add ("Artist", artist);
			view.Columns.Add ("Title", title);
			view.Columns.Add ("Status", status);

			var pg = new ProgressBar ();
			pg.Indeterminate = true;
			store.AddNode ().SetValue (artist, "Artist X").SetValue (title, "Title X").SetValue (status, pg);

			pg = new ProgressBar ();
			pg.Indeterminate = true;
			store.AddNode ().SetValue (artist, "Artist Y").SetValue (title, "Title Y").SetValue (status, pg);

			pg = new ProgressBar ();
			pg.Indeterminate = true;
			store.AddNode ().SetValue (artist, "Artist Z").SetValue (title, "Title Z").SetValue (status, pg);
			view.DataSource = store;
			PackStart (view);
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
	}
}

