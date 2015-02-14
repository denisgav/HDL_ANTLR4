using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;

namespace Schematix.Waveform.ListViewLayout
{

	// ------------------------------------------------------------------------
	public abstract class ImageGridViewColumn : GridViewColumn, IValueConverter
	{

		// ----------------------------------------------------------------------
		protected ImageGridViewColumn() :
			this( Stretch.None )
		{
		} // ImageGridViewColumn

		// ----------------------------------------------------------------------
		protected ImageGridViewColumn( Stretch imageStretch )
		{
			FrameworkElementFactory imageElement = new FrameworkElementFactory( typeof( Image ) );

			// image source
			Binding imageSourceBinding = new Binding();
			imageSourceBinding.Converter = this;
			imageSourceBinding.Mode = BindingMode.OneWay;
			imageElement.SetBinding( Image.SourceProperty, imageSourceBinding );

			// image stretching
			Binding imageStretchBinding = new Binding();
			imageStretchBinding.Source = imageStretch;
			imageElement.SetBinding( Image.StretchProperty, imageStretchBinding );

			DataTemplate template = new DataTemplate();
			template.VisualTree = imageElement;
			CellTemplate = template;
		} // ImageGridViewColumn

		// ----------------------------------------------------------------------
		protected abstract ImageSource GetImageSource( object value );

		// ----------------------------------------------------------------------
		object IValueConverter.Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return GetImageSource( value );
		} // Convert

		// ----------------------------------------------------------------------
		object IValueConverter.ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		} // ConvertBack

	} // class ImageGridViewColumn

} // namespace Itenso.Windows.Controls.ListViewLayout
// -- EOF -------------------------------------------------------------------
