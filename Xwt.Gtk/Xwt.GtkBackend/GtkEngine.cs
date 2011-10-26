// 
// GtkEngine.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
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
using Xwt.Engine;
using Xwt.Backends;

namespace Xwt.GtkBackend
{
	class GtkEngine: Xwt.Backends.EngineBackend
	{
		public override void InitializeApplication ()
		{
			Gtk.Application.Init ();
			
			WidgetRegistry.RegisterBackend (typeof(Xwt.Window), typeof(WindowBackend<Gtk.Window, IWindowEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Label), typeof(LabelBackend<Gtk.Label,IWidgetEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.HBox), typeof(BoxBackend<CustomContainer,IWidgetEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.VBox), typeof(BoxBackend<CustomContainer,IWidgetEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Button), typeof(ButtonBackend<Gtk.Button,IButtonEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Notebook), typeof(NotebookBackend<Gtk.Notebook,IWidgetEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.TreeView), typeof(TreeViewBackend<Gtk.TreeView,ITreeViewEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.TreeStore), typeof(TreeStoreBackend));
			WidgetRegistry.RegisterBackend (typeof(Xwt.ListView), typeof(ListViewBackend<Gtk.TreeView,IListViewEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.ListStore), typeof(ListStoreBackend));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Canvas), typeof(CanvasBackend<Gtk.DrawingArea,ICanvasEventSink>));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Drawing.Image), typeof(ImageHandler));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Drawing.Context), typeof(ContextBackendHandler));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Drawing.Gradient), typeof(GradientBackendHandler));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Drawing.TextLayout), typeof(TextLayoutBackendHandler));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Drawing.Font), typeof(FontBackendHandler));
			WidgetRegistry.RegisterBackend (typeof(Xwt.Menu), typeof(MenuBackend));
			WidgetRegistry.RegisterBackend (typeof(Xwt.MenuItem), typeof(MenuItemBackend));
		}

		public override void RunApplication ()
		{
			Gtk.Application.Run ();
		}

		public static void ReplaceChild (Gtk.Container cont, Gtk.Widget oldWidget, Gtk.Widget newWidget)
		{
			if (cont is IGtkContainer) {
				((IGtkContainer)cont).ReplaceChild (oldWidget, newWidget);
			}
			else if (cont is Gtk.Bin) {
				((Gtk.Bin)cont).Remove (oldWidget);
				((Gtk.Bin)cont).Child  = newWidget;
			}
		}
		
		public override void Invoke (Action action)
		{
			Gtk.Application.Invoke (delegate {
				action ();
			});
		}
		
		public override object TimeoutInvoke (Func<bool> action, TimeSpan timeSpan)
		{
			return GLib.Timeout.Add ((uint) timeSpan.TotalMilliseconds, delegate {
				return action ();
			});
		}
		
		public override void CancelTimeoutInvoke (object id)
		{
			GLib.Source.Remove ((uint)id);
		}
	}
	
	public interface IGtkContainer
	{
		void ReplaceChild (Gtk.Widget oldWidget, Gtk.Widget newWidget);
	}
}

