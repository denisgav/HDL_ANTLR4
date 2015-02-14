using System;
using System.Windows;
using System.Windows.Controls;

namespace Schematix.Waveform.ListViewLayout
{

	// ------------------------------------------------------------------------
	public abstract class LayoutColumn
	{

		// ----------------------------------------------------------------------
		protected static bool HasPropertyValue( GridViewColumn column, DependencyProperty dp )
		{
			if ( column == null )
			{
				throw new ArgumentNullException( "column" );
			}
			object value = column.ReadLocalValue( dp );
			if ( value != null && value.GetType() == dp.PropertyType )
			{
				return true;
			}

			return false;
		} // HasPropertyValue

		// ----------------------------------------------------------------------
		protected static double? GetColumnWidth( GridViewColumn column, DependencyProperty dp )
		{
			if ( column == null )
			{
				throw new ArgumentNullException( "column" );
			}
			object value = column.ReadLocalValue( dp );
			if ( value != null && value.GetType() == dp.PropertyType )
			{
				return (double)value;
			}

			return null;
		} // GetColumnWidth

	} // class LayoutColumn

} // namespace Itenso.Windows.Controls.ListViewLayout
// -- EOF -------------------------------------------------------------------
