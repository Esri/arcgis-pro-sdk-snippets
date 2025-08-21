/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       https://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Editing;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
	/// <summary>
	/// Provides methods and tools for working with graphic overlays in a map view.
	/// </summary>
	/// <remarks>This class contains static methods and nested types that demonstrate how to add, update, and
	/// manage graphic overlays in an ArcGIS Pro map view. Examples include adding point graphics, updating overlay
	/// symbols, and working with text and image-based overlays. These methods utilize the ArcGIS Pro SDK's mapping and
	/// geometry APIs.</remarks>
	public static class ProSnippetsGraphicOverlay
	{
		#region ProSnippet Group: Graphic overlay
		#endregion

		// cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.CIM.CIMGraphic, System.Double)
		// cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.Geometry.Geometry, ArcGIS.Core.CIM.CIMSymbolReference, System.Double)
		// cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.Geometry.Geometry, ArcGIS.Core.CIM.CIMSymbolReference, System.Double, System.Double)
		#region Graphic Overlay
		/// <summary>
		/// Demonstrates how to add, update, and clear a graphic overlay on the current map view.
		/// </summary>
		/// <param name="_graphic">A disposable object representing the graphic overlay. This parameter is updated with the created overlay and
		/// should be disposed of to clear the overlay from the map view.</param>
		public static async void GraphicOverlaySnippetTest(IDisposable _graphic)
		{
			// get the current mapview and point
			var mapView = MapView.Active;
			if (mapView == null)
				return;
			var myextent = mapView.Extent;
			var point = myextent.Center;

			// add point graphic to the overlay at the center of the mapView
			_graphic = await QueuedTask.Run(() =>
			{
				//add these to the overlay
				return mapView.AddOverlay(point,
						SymbolFactory.Instance.ConstructPointSymbol(
										ColorFactory.Instance.RedRGB, 30.0, SimpleMarkerStyle.Star).MakeSymbolReference());
			});

			// update the overlay with new point graphic symbol
			MessageBox.Show("Now to update the overlay...");
			await QueuedTask.Run(() =>
			{
				mapView.UpdateOverlay(_graphic, point, SymbolFactory.Instance.ConstructPointSymbol(
																			ColorFactory.Instance.BlueRGB, 20.0, SimpleMarkerStyle.Circle).MakeSymbolReference());
			});

			// clear the overlay display by disposing of the graphic
			MessageBox.Show("Now to clear the overlay...");
			_graphic.Dispose();

		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.CIM.CIMGraphic, System.Double)
		// cref: ArcGIS.Core.CIM.CIMPictureGraphic
		// cref: ArcGIS.Core.CIM.CIMPictureGraphic.PictureURL
		// cref: ArcGIS.Core.CIM.CIMPictureGraphic.Shape
		#region Graphic Overlay with CIMPictureGraphic
		/// <summary>
		/// Adds a picture graphic overlay to the current map view using a specified geometry and envelope.
		/// </summary>
		/// <param name="geometry">The geometry defining the shape of the overlay.</param>
		/// <param name="envelope">The envelope defining the extent of the overlay.</param>
		public static void CIMPictureGraphicOverlay(Geometry geometry, ArcGIS.Core.Geometry.Envelope envelope)
		{
			// get the current mapview
			var mapView = MapView.Active;
			if (mapView == null)
				return;

			// Use SourceURL for the URL to the image content. For
			// a local file, use a file path. For a web/internet file
			// location, use its URL
			//
			// Supported image types are:
			// png, jpg, tiff, bmp, gif, svg

			var pictureGraphic = new CIMPictureGraphic
			{
				SourceURL = @"C:\Images\MyImage.png",
				Shape = envelope
			};

			IDisposable _graphic = mapView.AddOverlay(pictureGraphic);
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.MapTool.AddOverlayAsync(ArcGIS.Core.Geometry.Geometry, ArcGIS.Core.CIM.CIMSymbolReference, System.Double, System.Double)
		// cref: ArcGIS.Desktop.Mapping.MappingExtensions.AddOverlay(ArcGIS.Desktop.Mapping.MapView, ArcGIS.Core.CIM.CIMGraphic, System.Double)
		#region Add overlay graphic with text
		/// <summary>
		/// Adds an overlay graphic with text to the current map view.
		/// </summary>
		public class AddOverlayWithText : MapTool
		{
			private IDisposable _graphic = null;
			private CIMLineSymbol _lineSymbol = null;
			public AddOverlayWithText()
			{
				IsSketchTool = true;
				SketchType = SketchGeometryType.Line;
				SketchOutputMode = SketchOutputMode.Map;
			}

			protected override async Task<bool> OnSketchCompleteAsync(Geometry geometry)
			{
				//Add an overlay graphic to the map view
				_graphic = await this.AddOverlayAsync(geometry, _lineSymbol.MakeSymbolReference());

				//define the text symbol
				var textSymbol = new CIMTextSymbol();
				//define the text graphic
				var textGraphic = new CIMTextGraphic();

				await QueuedTask.Run(() =>
				{
					//Create a simple text symbol
					textSymbol = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 8.5, "Corbel", "Regular");
					//Sets the geometry of the text graphic
					textGraphic.Shape = geometry;
					//Sets the text string to use in the text graphic
					textGraphic.Text = "This is my line";
					//Sets symbol to use to draw the text graphic
					textGraphic.Symbol = textSymbol.MakeSymbolReference();
					//Draw the overlay text graphic
					_graphic = this.ActiveMapView.AddOverlay(textGraphic);
				});

				return true;
			}
		}
		#endregion
	}
}
