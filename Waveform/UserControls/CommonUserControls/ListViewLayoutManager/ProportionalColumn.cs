using System;
using System.Windows;
using System.Windows.Controls;

namespace Schematix.Waveform.ListViewLayout
{

	// ------------------------------------------------------------------------
	public sealed class ProportionalColumn : LayoutColumn
	{

		// ----------------------------------------------------------------------
		public static readonly DependencyProperty WidthProperty = 
			DependencyProperty.RegisterAttached(
				"Width",
				typeof( double ),
				typeof( ProportionalColumn ) );

		// ----------------------------------------------------------------------
		private ProportionalColumn()
		{
		} // ProportionalColumn

		// ----------------------------------------------------------------------
		public static double GetWidth( DependencyObject obj )
		{
			return (double)obj.GetValue( WidthProperty );
		} // GetWidth

		// ----------------------------------------------------------------------
		public static void SetWidth( DependencyObject obj, double width )
		{
			obj.SetValue( WidthProperty, width );
		} // SetWidth

		// ----------------------------------------------------------------------
		public static bool IsProportionalColumn( GridViewColumn column )
		{
			if ( column == null )
			{
				return false;
			}
			return HasPropertyValue( column, WidthProperty );
		} // IsProportionalColumn

		// ----------------------------------------------------------------------
		public static double? GetProportionalWidth( GridViewColumn column )
		{
			return GetColumnWidth( column, WidthProperty );
		} // GetProportionalWidth

		// ----------------------------------------------------------------------
		public static GridViewColumn ApplyWidth( GridViewColumn gridViewColumn, double width )
		{
			SetWidth( gridViewColumn, width );
			return gridViewColumn;
		} // ApplyWidth

	} // class ProportionalColumn

}
