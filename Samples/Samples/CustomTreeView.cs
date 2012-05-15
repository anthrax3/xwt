using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xwt;
using Xwt.Drawing;

namespace Samples
{
	// EPIC FAIL: Unfortunately, XWT's TreeView doesn't support adding widgets to a cell, so
	//            we have come up with this temporary workaround, which works very well anyway
	public class CustomTreeView : Table {

		public CustomTreeView ()
		{
			columns = new ColumnCollection (this);
		}

		public class Column {
			internal string Title { get; set; }

			internal DataField Field { get; set; }
		}

		public class ColumnCollection : System.Collections.Generic.IEnumerable<Column> {

			CustomTreeView parent;
			internal ColumnCollection (CustomTreeView parent) {
				this.parent = parent;
			}

			List<Column> columns = new List<Column> ();

			public void Add (string title, DataField field)
			{
				columns.Add (new Column { Title = title, Field = field });
			}

			#region IEnumerable implementation
			IEnumerator IEnumerable.GetEnumerator ()
			{
				return columns.GetEnumerator ();
			}
			IEnumerator<Column> IEnumerable<Column>.GetEnumerator ()
			{
				return columns.GetEnumerator ();
			}
			#endregion
		}

		ColumnCollection columns;
		public ColumnCollection Columns {
			get {
				return columns;
			}
		}

		TreeStore dataSource;
		public TreeStore DataSource {
			get {
				return dataSource;
			}
			set {
				if (dataSource != value) {
					dataSource = value;

					Build ();
				}
			}
		}

		private void Build () {
			int left = 0;

			var columnPositions = new Dictionary<Column, int>();

			foreach (Column column in columns)
			{
				columnPositions[column] = left;
				Attach (CreateHeader (column.Title), left, 0);
				left++;
			}

			int top = 1;
			foreach(var node in dataSource){
				foreach (var column in columns)
				{
					var value = node.GetValue(column.Field);
					Widget widget = null;
					if (value is string){
						widget = new Label((string)value);
						if (top % 2 == 1)
						{
							widget.BackgroundColor = Colors.White;
						} else {
							widget.BackgroundColor = Colors.LightGray;
						}
					}
					else if (value is Widget)
						widget = (Widget) value;
					else
						throw new NotSupportedException(value.GetType().FullName);
					Attach(widget, columnPositions[column], top);


				}
				top ++;
			}

			this.DefaultRowSpacing = 0;
			this.DefaultColumnSpacing = 0;
		}

		Widget CreateHeader (string text) {
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


		public class TreeStore : IEnumerable<TreeNavigator>
		{
			DataField[] fields;

			readonly List<TreeNavigator> nodes = new List<TreeNavigator>();

			public TreeStore(params DataField[] fields)
			{
				this.fields = fields;
			}

			public TreeNavigator AddNode()
			{
				var nav = new TreeNavigator(fields);
				nodes.Add(nav);
				return nav;
			}

			#region IEnumerable implementation
			IEnumerator IEnumerable.GetEnumerator()
			{
				return nodes.GetEnumerator();
			}
			IEnumerator<TreeNavigator> IEnumerable<TreeNavigator>.GetEnumerator()
			{
				return nodes.GetEnumerator();
			}
			#endregion
		}

		public class TreeNavigator
		{
			readonly Dictionary<DataField, object> values;

			public TreeNavigator (IEnumerable<DataField> fields)
			{
				values = new Dictionary<DataField, object>();
				foreach (var field in fields)
				{
					values[field] = null;
				}
			}

			public TreeNavigator SetValue<T>(DataField field, T value)
			{
				if (!values.ContainsKey(field))
				{
					throw new InvalidOperationException("Field not found");
				}
				values[field] = value;
				return this;
			}

			public object GetValue(DataField field)
			{
				if (!values.ContainsKey(field))
				{
					throw new InvalidOperationException("Field not found");
				}
				return values[field];
			}
		}
	}
}