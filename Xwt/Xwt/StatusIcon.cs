// 
// StatusIcon.cs
//  
// Author:
//       Andres G. Aragoneses <knocte@gmail.com>
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
using System.Collections.ObjectModel;

using Xwt.Drawing;
using Xwt.Backends;

namespace Xwt
{
	public class StatusIcon : Menu
	{
		public StatusIcon (string pathToImage, Collection<MenuItem> menuItems)
		{
			if (menuItems == null) {
				throw new ArgumentNullException ("menuItems");
			}
			if (menuItems.Count == 0) {
				throw new ArgumentException ("menuItems must contain at least one item", "menuItems");
			}
			
			foreach (MenuItem menuItem in menuItems) {
				InsertItem (this.Items.Count, menuItem);
			}
			Backend.SetComponents (pathToImage);
		}
		
		IStatusIconBackend Backend {
			get { return (IStatusIconBackend) BackendHost.Backend; }
		}
	}
}

