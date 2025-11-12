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
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.DeviceLocation;
using ArcGIS.Desktop.Core.DeviceLocation.Events;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.DesktopService;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.DeviceLocation;
using ArcGIS.Desktop.Mapping.Offline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Polyline = ArcGIS.Core.Geometry.Polyline;
using QueryFilter = ArcGIS.Core.Data.QueryFilter;

namespace ProSnippets.MapAuthoringSnippets
{
  /// <summary>
  /// This methods has a collection of code snippets related to working with ArcGIS Pro Map Authoring functionality.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with Map Authoring functionality in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
  public static partial class ProSnippetsMapAuthoring
  {
    public static async void MapAuthoringProSnippets()
    {
      #region ignore - Variable initialization
      //Different ways to reference or create a map
      MapView mapView = MapView.Active;
      Map map = mapView?.Map; //Reference to the active map
      map = MapFactory.Instance.CreateMap("New Map", MapType.Map, MapViewingMode.Map, Basemap.ProjectDefault); //Create a new map      

      //Or create from a map uri or map package
      var mapFileUri = new Uri(@"C:\Maps\USNationalParks.mpkx");
      map = MapFactory.Instance.CreateMap(mapFileUri);
      //Reference to a map project item
      MapProjectItem mapProjectItem = Project.Current.GetItems<MapProjectItem>()
                                 .FirstOrDefault();
      //Note: Needs QueuedTask to run
      map = mapProjectItem?.GetMap(); //Get the map from a map project item

      //Styles
      StyleProjectItem styleProjectItem = Project.Current.GetItems<StyleProjectItem>()
                                 .FirstOrDefault();

      //StyleItem
      //You can generate a swatch for a text symbols also.
      CIMSymbol symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
      SymbolStyleItem symbolStyleItem = new SymbolStyleItem()
      {
        Symbol = symbol,
        PatchHeight = 64,
        PatchWidth = 64
      };
      //Reference a Layer
      Layer layer = mapView?.Map?.GetLayersAsFlattenedList().OfType<Layer>().FirstOrDefault();
      //How to reference a Feature Layer
      //Reference to the first feature layer in the active map
      FeatureLayer featureLayer = mapView.Map.GetLayersAsFlattenedList()
                                      .OfType<FeatureLayer>()
                                      .FirstOrDefault();
      //Reference to a feature layer by name
      featureLayer = mapView.Map.FindLayer("CIMPATH=map/u_s__states__generalized_.xml") as FeatureLayer;
      featureLayer = mapView.Map.FindLayers("US National Parks", true) as FeatureLayer;
      //Getting the first selected feature layer of the map view
      FeatureLayer selectedLayer = (FeatureLayer)MapView.Active.GetSelectedLayers()
                  .OfType<FeatureLayer>().FirstOrDefault();
      //Reference a Group Layer
      GroupLayer groupLayer = mapView.Map.GetLayersAsFlattenedList()
                                      .OfType<GroupLayer>()
                                      .FirstOrDefault();
      //Raster layer
      RasterLayer rasterLayer = LayerFactory.Instance.CreateLayer(new Uri("url"), map) as RasterLayer;
      //Mosaic layer
      MosaicLayer mosaicLayer = mapView.Map.GetLayersAsFlattenedList()
                                      .OfType<MosaicLayer>()
                                      .FirstOrDefault();
      //ImageService layer
      string url =
      @"http://imagery.arcgisonline.com/arcgis/services/LandsatGLS/GLS2010_Enhanced/ImageServer";
      var imageServiceLayer = LayerFactory.Instance.CreateLayer(new Uri(url), map) as ImageServiceLayer;
      //Or get the image service layer from a mosaic layer
      ImageServiceLayer mosaicImageSublayer = mosaicLayer.GetImageLayer();
      //Graphics layer
      GraphicsLayer graphicsLayer = map.TargetGraphicsLayer;
      //Or
      graphicsLayer = mapView.Map.GetLayersAsFlattenedList()
                                      .OfType<GraphicsLayer>()
                                      .FirstOrDefault();


      //Get a ColorRamp
      StyleProjectItem style =
              Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS Colors");
      var colorRampList = style.SearchColorRamps("Heat Map 4 - Semitransparent");

      CIMColorRamp colorRamp = colorRampList[0].ColorRamp;
      #endregion

      #region ProSnippet Group: Style Management
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
      #region How to get a style in project by name
      {
        //Get all styles in the project
        var ProjectStyles = Project.Current.GetItems<StyleProjectItem>();

        //Get a specific style in the project by name
        StyleProjectItem styleFound = ProjectStyles.First(x => x.Name == "NameOfTheStyle");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.StyleHelper.CreateStyle(ArcGIS.Desktop.Core.Project,System.String)
      #region How to create a new style
      {
        //Full path for the new style file (.stylx) to be created
        string styleToCreate = @"C:\Temp\NewStyle.stylx";
        //Note: Needs QueuedTask to run
        StyleHelper.CreateStyle(Project.Current, styleToCreate);
      }
      #endregion
      // cref:ArcGIS.Desktop.Mapping.StyleHelper.AddStyle(ArcGIS.Desktop.Core.Project,System.String)
      #region How to add a style to project
      {
        //For ArcGIS Pro system styles, just pass in the name of the style to add to the project
        //Note: Needs QueuedTask to run
        StyleHelper.AddStyle(Project.Current, "3D Vehicles");

        //For custom styles, pass in the full path to the style file on disk
        //Note: Needs QueuedTask to run
        string customStyleToAdd = @"C:\Temp\CustomStyle.stylx";
        StyleHelper.AddStyle(Project.Current, customStyleToAdd);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.RemoveStyle(ArcGIS.Desktop.Core.Project,System.String)
      #region How to remove a style from project
      {
        //For ArcGIS Pro system styles, just pass in the name of the style to remove from the project
        //Note: Needs QueuedTask to run
        StyleHelper.RemoveStyle(Project.Current, "3D Vehicles");

        //For custom styles, pass in the full path to the style file on disk
        //Note: Needs QueuedTask to run
        string customStyleToAdd = @"C:\Temp\CustomStyle.stylx";
        StyleHelper.RemoveStyle(Project.Current, customStyleToAdd);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.AddItem(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItem)
      #region How to add a style item to a style
      {
        //Create a new style item
        //You can generate a swatch for a text symbols also.  
        CIMSymbol symbolToAdd = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        SymbolStyleItem styleItemToAdd = new SymbolStyleItem()
        {
          Symbol = symbolToAdd,
          PatchHeight = 64,
          PatchWidth = 64
        };
        styleProjectItem.AddItem(styleItemToAdd);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.RemoveItem(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItem)
      #region How to remove a style item from a style
      {
        //Remove any item from style
        //Note: symbolStyleItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.RemoveItem(symbolStyleItem);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.CanUpgrade
      #region How to determine if a style can be upgraded
      {
        //Add the style to the current project
        string stylePath = @"C:\MyStyles\OldStyle.stylx";
        //Note: Needs QueuedTask to run
        StyleHelper.AddStyle(Project.Current, stylePath);
        StyleProjectItem styleToUpgrade = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

        //returns true if style can be upgraded
        bool canUpgrade = styleToUpgrade.CanUpgrade;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.IsReadOnly
      #region How to determine if a style is read-only
      {
        //Add the style to the current project
        string stylePath = @"C:\MyStyles\MyStyle.stylx";
        //Note: Needs QueuedTask to run
        StyleHelper.AddStyle(Project.Current, stylePath);
        StyleProjectItem styleToCheck = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);
        //returns true if style is read-only
        bool isReadOnly = styleToCheck.IsReadOnly;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.IsCurrent
      #region How to determine if a style is current
      {
        //Add the style to the current project
        //Note: Needs QueuedTask to run
        string stylePath = @"C:\MyStyles\MyStyle.stylx";
        StyleHelper.AddStyle(Project.Current, stylePath);
        StyleProjectItem styleToCheck = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);
        //returns true if style matches the current Pro version
        bool isCurrent = styleToCheck.IsCurrent;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.CanUpgrade
      // cref: ArcGIS.Desktop.Mapping.StyleHelper.UpgradeStyle(ArcGIS.Desktop.Mapping.StyleProjectItem)
      #region How to upgrade a style
      {
        bool success = false;
        //Add the style to the current project
        string stylePath = @"C:\MyStyles\OldStyle.stylx";
        //Note: Needs QueuedTask to run
        StyleHelper.AddStyle(Project.Current, stylePath);
        StyleProjectItem styleToUpgrade = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

        //Verify that style can be upgraded
        if (styleToUpgrade.CanUpgrade)
        {
          //Note: Needs QueuedTask to run
          success = StyleHelper.UpgradeStyle(styleToUpgrade);
        }
      }
      #endregion

      //construct point symbol
      #region ProSnippet Group: Symbols
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double)
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      #region How to construct a point symbol of a specific color and size
      {
        CIMPointSymbol pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 10.0);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      #region How to construct a point symbol of a specific color, size and shape
      {
        CIMPointSymbol starPointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 10.0, SimpleMarkerStyle.Star);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
      // cref: ArcGIS.Core.CIM.CIMMarker
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      #region How to construct a point symbol from a marker
      {
        CIMMarker marker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.GreenRGB, 8.0, SimpleMarkerStyle.Pushpin);
        CIMPointSymbol pointSymbolFromMarker = SymbolFactory.Instance.ConstructPointSymbol(marker);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarkerFromFile(System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
      // cref: ArcGIS.Core.CIM.CIMMarker
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      #region How to construct a point symbol from a file on disk
      {
        //The following file formats can be used to create the marker: DAE, 3DS, FLT, EMF, JPG, PNG, BMP, GIF
        //Note: Run within QueuedTask.Run
        CIMMarker markerFromFile = SymbolFactory.Instance.ConstructMarkerFromFile(@"C:\Temp\fileName.dae");

        CIMPointSymbol pointSymbolFromFile = SymbolFactory.Instance.ConstructPointSymbol(markerFromFile);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarkerFromStream(System.IO.Stream)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
      // cref: ArcGIS.Core.CIM.CIMMarker
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      #region How to construct a point symbol from a in memory graphic
      {
        //Create a stream for the image
        //At 3.0 you need https://www.nuget.org/packages/Microsoft.Windows.Compatibility
        //System.Drawing
        System.Drawing.Image newImage = System.Drawing.Image.FromFile(@"C:\PathToImage\Image.png");
        var stream = new System.IO.MemoryStream();
        newImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Position = 0;
        //Create marker using the stream
        CIMMarker markerFromStream = SymbolFactory.Instance.ConstructMarkerFromStream(stream);
        //Create the point symbol from the marker
        CIMPointSymbol pointSymbolFromStream = SymbolFactory.Instance.ConstructPointSymbol(markerFromStream);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol(ArcGIS.Core.CIM.CIMColor,ArcGIS.Desktop.Mapping.SimpleFillStyle)
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      #region How to construct a polygon symbol of specific color and fill style
      {
        CIMPolygonSymbol polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double, ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol(ArcGIS.Core.CIM.CIMColor,ArcGIS.Desktop.Mapping.SimpleFillStyle,ArcGIS.Core.CIM.CIMStroke)
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      #region How to construct a polygon symbol of specific color, fill style and outline
      {
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);
        CIMPolygonSymbol fillWithOutline = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol(ArcGIS.Core.CIM.CIMColor,ArcGIS.Desktop.Mapping.SimpleFillStyle,ArcGIS.Core.CIM.CIMStroke)
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      #region How to construct a polygon symbol without an outline
      {
        CIMPolygonSymbol fillWithoutOutline = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, null);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructLineSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      #region How to construct a line symbol of specific color, size and line style
      {
        CIMLineSymbol lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructLineSymbol(ArcGIS.Core.CIM.CIMStroke)
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      #region How to construct a line symbol from a stroke
      {
        CIMStroke stroke = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0);
        CIMLineSymbol lineSymbolFromStroke = SymbolFactory.Instance.ConstructLineSymbol(stroke);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(ArcGIS.Core.CIM.CIMColor,System.Double, ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Core.CIM.CIMMarkerPlacementOnVertices
      // cref: ArcGIS.Core.CIM.CIMMarkerStrokePlacement.AngleToLine
      // cref: ArcGIS.Core.CIM.CIMMarkerPlacementOnVertices.PlaceOnEndPoints
      // cref: ArcGIS.Core.CIM.CIMMarkerStrokePlacement.Offset
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer
      #region How to construct a multilayer line symbol with circle markers on the line ends
      {
        //Note: Needs QueuedTask to run
        var lineStrokeRed = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 4.0);
        var markerCircle = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.RedRGB, 12, SimpleMarkerStyle.Circle);
        markerCircle.MarkerPlacement = new CIMMarkerPlacementOnVertices()
        {
          AngleToLine = true,
          PlaceOnEndPoints = true,
          Offset = 0
        };
        var lineSymbolWithCircles = new CIMLineSymbol()
        {
          SymbolLayers = new CIMSymbolLayer[2] { markerCircle, lineStrokeRed }
        };
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(ArcGIS.Core.CIM.CIMColor,System.Double, ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Core.CIM.CIMMarker.Rotation
      // cref: ArcGIS.Core.CIM.CIMMarker.MarkerPlacement
      // cref: ArcGIS.Core.CIM.CIMMarkerPlacementOnLine
      // cref: ArcGIS.Core.CIM.CIMMarkerStrokePlacement.AngleToLine
      // cref: ArcGIS.Core.CIM.CIMMarkerPlacementOnLine.RelativeTo
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer
      #region How to construct a multilayer line symbol with an arrow head on the end
      {
        var markerTriangle = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.RedRGB, 12, SimpleMarkerStyle.Triangle);
        markerTriangle.Rotation = -90; // or -90
        markerTriangle.MarkerPlacement = new CIMMarkerPlacementOnLine() { AngleToLine = true, RelativeTo = PlacementOnLineRelativeTo.LineEnd };

        var lineSymbolWithArrow = new CIMLineSymbol()
        {
          SymbolLayers = new CIMSymbolLayer[2] { markerTriangle,
                    SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 2)
          }
        };
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference(ArcGIS.Core.CIM.CIMSymbol)
      // cref: ArcGIS.Core.CIM.CIMSymbolReference
      #region How to get symbol reference from a symbol
      {
        CIMPolygonSymbol polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB);

        //Get symbol reference from the symbol
        CIMSymbolReference symbolReference = polySymbol.MakeSymbolReference();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(System.Int32, system.String, System.String, System.Int32, ArcGIS.Core.CIM.CIMColor)
      // cref: ArcGIS.Core.CIM.CIMCharacterMarker
      // cref: ArcGIS.Core.CIM.CIMCharacterMarker.Symbol
      // cref: ArcGIS.Core.CIM.CIMMultiLayerSymbol.SymbolLayers
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
      #region Modify a point symbol created from a character marker 
      {
        //create marker from the Font, char index,size,color
        //Note: Needs QueuedTask to run
        var cimMarker = SymbolFactory.Instance.ConstructMarker(125, "Wingdings 3", "Regular", 6, ColorFactory.Instance.BlueRGB) as CIMCharacterMarker;
        var polygonMarker = cimMarker.Symbol;
        //modifying the polygon's outline and fill
        //This is the outline
        polygonMarker.SymbolLayers[0] = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.GreenRGB, 2, SimpleLineStyle.Solid);
        //This is the fill
        polygonMarker.SymbolLayers[1] = SymbolFactory.Instance.ConstructSolidFill(ColorFactory.Instance.BlueRGB);
        //create a symbol from the marker 
        //Note this overload of ConstructPointSymbol does not need to be run within QueuedTask.Run.
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(cimMarker);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.GetAvailableFonts()
      #region Get a List of Available Fonts
      {
        //returns a tuple per font: (string fontName, List<string> fontStyles)
        var fonts = SymbolFactory.Instance.GetAvailableFonts();
        foreach (var font in fonts)
        {
          var styles = string.Join(",", font.fontStyles);
          System.Diagnostics.Debug.WriteLine($"{font.fontName}, styles: {styles}");
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.DefaultFont
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultFont(System.String)
      // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultFont(System.String, System.String)
      #region Get/Set Default Font
      {
        var def_font = SymbolFactory.Instance.DefaultFont;
        System.Diagnostics.Debug.WriteLine($"{def_font.fontName}, styles: {def_font.styleName}");

        //set default font - set through application options
        //Note: Must use QueuedTask
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultFont("tahoma");
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultFont("tahoma", "bold");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor, System.Double)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(System.String, System.STring)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.GetAvailableFonts()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor, System.Double,System.String, System.STring)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMPolygonSymbol, SSystem.Double, System.String)
      #region Construct a Text Symbol With Options
      {
        //Note: Needs QueuedTask to run
        //using the default font
        var textSym1 = SymbolFactory.Instance.ConstructTextSymbol();
        var textSym2 = SymbolFactory.Instance.ConstructTextSymbol(
                           ColorFactory.Instance.BlueRGB, 14);

        //using a specific font
        var textSym3 = SymbolFactory.Instance.ConstructTextSymbol("Arial");
        var textSym4 = SymbolFactory.Instance.ConstructTextSymbol(
                          "Arial", "Narrow Bold");

        //or query available fonts to ensure the font is there
        var all_fonts = SymbolFactory.Instance.GetAvailableFonts();
        var font = all_fonts.FirstOrDefault(f => f.fontName == "Arial");
        if (!string.IsNullOrEmpty(font.fontName))
        {
          var textSym5 = SymbolFactory.Instance.ConstructTextSymbol(font.fontName);
          //or with a font+style
          var textSym6 = SymbolFactory.Instance.ConstructTextSymbol(
                                          font.fontName, font.fontStyles.First());
        }

        //overloads - font + color and size, etc
        var textSym7 = SymbolFactory.Instance.ConstructTextSymbol(
                        ColorFactory.Instance.BlueRGB, 14, "Times New Roman", "Italic");

        //custom symbol - black stroke, red fill
        var poly_symbol = SymbolFactory.Instance.ConstructPolygonSymbol(
          SymbolFactory.Instance.ConstructSolidFill(ColorFactory.Instance.RedRGB),
          SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 1));
        var textSym8 = SymbolFactory.Instance.ConstructTextSymbol(
                poly_symbol, 14, "Georgia", "Bold");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.#ctor
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
      // cref: ArcGIS.Desktop.Mapping.StyleItem.PatchHeight
      // cref: ArcGIS.Desktop.Mapping.StyleItem.PatchWidth
      // cref: ArcGIS.Desktop.Mapping.StyleItem.PreviewImage
      #region Create a Swatch for a given symbol
      {
        //Note: call within QueuedTask.Run()
        CIMSymbol symbolForSwatch = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        //You can generate a swatch for a text symbols also.
        var si = new SymbolStyleItem()
        {
          Symbol = symbolForSwatch,
          PatchHeight = 64,
          PatchWidth = 64
        };
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.GenerateImage(ArcGIS.Core.CIM.CIMPointSymbol,ArcGIS.Desktop.Mapping.OutputImageFormat,System.Double,System.Boolean,System.Double,System.Int64,System.Int64,ArcGIS.Core.CIM.CIMColor)
      // cref: ArcGIS.Desktop.Mapping.OutputImageFormat
      // cref: ArcGIS.Desktop.Mapping.OutputImageFormat.SVG
      // cref: ArcGIS.Desktop.Mapping.OutputImageFormat.PNG
      #region Convert Point Symbol to SVG
      {
        //Note: Needs QueuedTask to run
        //Create a point symbol
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(
          ColorFactory.Instance.RedRGB, 24, SimpleMarkerStyle.RoundedSquare);

        //Generate image returns a stream
        //OutputImageFormat specified the format for the image - in this case
        //we want SVG (an xml-based format)
        //
        //output fmt: SVG, scale factor x2, centerAnchorPoint = true
        //dpi = 300, wd x ht: 100x100px, background: white
        var mem_strm = SymbolFactory.Instance.GenerateImage(
          pointSymbol, OutputImageFormat.SVG, 2.0, true, 300, 100, 100,
          ColorFactory.Instance.WhiteRGB);

        //Set the memory stream position to the beginning
        mem_strm.Seek(0, SeekOrigin.Begin);

        //File path and name for saving the SVG file
        var fileName = "RoundedSquareSymbol.svg";
        string path_svg = System.IO.Path.Combine(System.IO.Path.GetTempPath() + fileName);

        //Write the memory stream to the file
        System.IO.File.WriteAllBytes(path_svg, mem_strm.ToArray());

        //////////////////////////////////////////////
        //Note: to convert SVG to image format, use a 3rd party
        //e.g. Aspose.SVG for .NET, for example convert SVG to PNG
        //using (var svg_doc = new Aspose.Svg.SVGDocument(path_svg))
        //{
        //  string path_png = Path.Combine(Path.GetTempPath() + "RoundedSquareSymbol.png");
        //  using (var img_png = new Aspose.Svg.Rendering.Image.ImageDevice(
        //    new ImageRenderingOptions(ImageFormat.Png), path_png))
        //  {
        //    svg_doc.RenderTo(img_png);
        //  }
        //  //also: https://docs.aspose.com/imaging/net/convert-svg-to-png/
        //}
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.GenerateImage(ArcGIS.Core.CIM.CIMPointSymbol,ArcGIS.Desktop.Mapping.OutputImageFormat,System.Double,System.Boolean,System.Double,System.Int64,System.Int64,ArcGIS.Core.CIM.CIMColor)
      // cref: ArcGIS.Desktop.Mapping.OutputImageFormat
      // cref: ArcGIS.Desktop.Mapping.OutputImageFormat.PNG
      #region Convert Point Symbol to PNG
      {
        //Note: Needs QueuedTask to run
        //Create a point symbol
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(
          ColorFactory.Instance.RedRGB, 24, SimpleMarkerStyle.RoundedSquare);

        //Generate image returns a stream
        //OutputImageFormat specified the format for the image - in this case
        //we want PNG
        //
        //output fmt: PNG, scale factor x2, centerAnchorPoint = true
        //dpi = 300, wd x ht: 100x100px, background: white
        var mem_strm = SymbolFactory.Instance.GenerateImage(
          pointSymbol, OutputImageFormat.PNG, 2.0, true, 300, 100, 100,
          ColorFactory.Instance.WhiteRGB);

        //Set the memory stream position to the beginning
        mem_strm.Seek(0, SeekOrigin.Begin);

        //Write the stream to a bit map
        var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();

        bitmapImage.BeginInit();
        bitmapImage.StreamSource = mem_strm;
        bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        //Write the bit map out to a file
        //File path and name for saving the PNG file
        var fileName = "RoundedSquareSymbol.png";
        string path_png = System.IO.Path.Combine(System.IO.Path.GetTempPath() + fileName);

        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

        using (var fileStream = new System.IO.FileStream(
          path_png, System.IO.FileMode.Create))
        {
          encoder.Save(fileStream);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CanLookupSymbol()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.LookupSymbol(System.Int64, ArcGIS.Desktop.Mapping.MapView)
      #region Lookup Symbol
      {
        //feature layer from the active map
        var featureLayerLookUp = MapView.Active.Map.GetLayersAsFlattenedList()
                                .OfType<FeatureLayer>()
                                .FirstOrDefault();
        //Get the selection
        //Note: Needs QueuedTask to run
        var selection = featureLayerLookUp.GetSelection();
        //Get the first Object ID
        var firstOID = selection.GetObjectIDs().FirstOrDefault();
        //Determine whether the layer's renderer type supports symbol lookup.
        if (featureLayerLookUp.CanLookupSymbol())
        {
          //Looks up the symbol for the corresponding feature identified by the object id.
          var lookupSymbol = featureLayerLookUp.LookupSymbol(firstOID, MapView.Active);
          var jSon = lookupSymbol.ToJson(); //Create a JSON encoding of the symbol
                                            //Do something with symbol
        }
      }
      #endregion

      #region ProSnippet Group: Symbol Search
      #endregion
      //symbol search
      // cref: ArcGIS.Desktop.Mapping.StyleHelper.LookupItem(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      #region How to search for a specific item in a style
      {
        //Search for a specific point symbol in style
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        string key = "Circle 1_Shapes_3";
        SymbolStyleItem item = (SymbolStyleItem)styleProjectItem.LookupItem(StyleItemType.PointSymbol, key);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchSymbols(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      #region How to search for point symbols in a style
      {
        //Search for point symbols
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        //Search for point symbols
        styleProjectItem.SearchSymbols(StyleItemType.PointSymbol, "searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchSymbols(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      #region How to search for line symbols in a style
      {
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        //Search for line symbols
        styleProjectItem.SearchSymbols(StyleItemType.LineSymbol, "searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchSymbols(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
      #region How to search for polygon symbols in a style
      {
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        //Search for polygon symbols
        styleProjectItem.SearchSymbols(StyleItemType.PolygonSymbol, "searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColors(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.ColorStyleItem
      #region How to search for colors in a style
      {
        //Search for colors
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchColors("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColorRamps(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
      #region How to search for color ramps in a style
      {
        //StyleProjectItem can be "ColorBrewer Schemes (RGB)", "ArcGIS 2D"...
        //Search for color ramps
        //Color Ramp searchString can be "Spectral (7 Classes)", "Pastel 1 (3 Classes)", "Red-Gray (10 Classes)"..
        styleProjectItem.SearchColorRamps("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchNorthArrows(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem
      #region How to search for north arrows in a style
      {
        //Search for north arrows
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchNorthArrows("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchScaleBars(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem
      #region How to search for scale bars in a style
      {
        //Search for scale bars
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchScaleBars("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchLabelPlacements(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
      // cref: ArcGIS.Desktop.Mapping.LabelPlacementStyleItem
      #region How to search for label placements in a style
      {
        //Search for label placements
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchLabelPlacements(StyleItemType.StandardLabelPlacement, "searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchLegends(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.LegendStyleItem
      #region How to search for legends in a style
      {
        //Search for legends
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchLegends("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchLegendItems(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.LegendItemStyleItem
      #region How to search for legend items in a style
      {
        //Search for legend items
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchLegendItems("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchGrids(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.GridStyleItem
      #region How to search for grids in a style
      {
        //Search for grids
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchGrids("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchMapSurrounds(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.MapSurroundStyleItem
      #region How to search for map surrounds in a style
      {
        //Search for map surrounds
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchMapSurrounds("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchTableFrames(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.TableFrameStyleItem
      #region How to search for table frames in a style
      {
        //Search for table frames
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchTableFrames("searchString");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchTableFrameFields(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
      // cref: ArcGIS.Desktop.Mapping.TableFrameFieldStyleItem
      #region How to search for table frame fields in a style
      {
        //Search for table frame fields
        //Note: styleProjectItem was created above in the variable initialization section
        //Note: Needs QueuedTask to run
        styleProjectItem.SearchTableFrameFields("searchString");
      }
      #endregion

      #region ProSnippet Group: Feature Layer Symbology
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.UsesRealWorldSymbolSizes
      // cref: ArcGIS.Core.CIM.CIMSimpleRenderer
      #region How to set symbol for a feature layer symbolized with simple renderer
      {
        //Note: Needs QueuedTask to run
        //Get simple renderer from the feature layer
        CIMSimpleRenderer currentRenderer = featureLayer.GetRenderer() as CIMSimpleRenderer;
        if (currentRenderer == null)
          return;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbol.SetRealWorldUnits(featureLayer.UsesRealWorldSymbolSizes);

        //Update the symbol of the current simple renderer
        currentRenderer.Symbol = symbol.MakeSymbolReference();
        //Update the feature layer renderer
        featureLayer.SetRenderer(currentRenderer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.UsesRealWorldSymbolSizes
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
      // cref: ArcGIS.Core.CIM.CIMSimpleRenderer
      // cref: ArcGIS.Core.CIM.CIMSimpleRenderer.Symbol
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.SetRealWorldUnits
      #region How to apply a symbol from style to a feature layer
      {
        //Note: Needs QueuedTask to run
        //Get simple renderer from the feature layer
        CIMSimpleRenderer currentRenderer = featureLayer.GetRenderer() as CIMSimpleRenderer;
        if (currentRenderer == null)
          return;
        //Get symbol from the SymbolStyleItem
        CIMSymbol symbolToApply = symbolStyleItem.Symbol;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbolToApply.SetRealWorldUnits(featureLayer.UsesRealWorldSymbolSizes);

        //Update the symbol of the current simple renderer
        currentRenderer.Symbol = symbolToApply.MakeSymbolReference();
        //Update the feature layer renderer
        featureLayer.SetRenderer(currentRenderer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHSYMBOLS
      // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
      // cref: ArcGIS.Desktop.Mapping.StyleItemType
      // cref: ArcGIS.Core.CIM.CIMSimpleRenderer.Symbol
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.SetRealWorldUnits
      #region How to apply a point symbol from a style to a feature layer
      {
        //Get the ArcGIS 2D System style from the Project
        var arcGIS2DStyle =
  Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 2D");

        //Note: Needs QueuedTask to run
        //Search for the symbolName style items within the ArcGIS 2D style project item.
        var items = arcGIS2DStyle.SearchSymbols(StyleItemType.PointSymbol, "Circle 1");

        //Gets the CIMSymbol
        CIMSymbol symbolToUse = items.FirstOrDefault().Symbol;

        //Get the renderer of the point feature layer
        CIMSimpleRenderer renderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbolToUse.SetRealWorldUnits(featureLayer.UsesRealWorldSymbolSizes);

        //Apply the symbol to the feature layer's current renderer
        renderer.Symbol = symbolToUse.MakeSymbolReference();

        //Apply the renderer to the feature layer
        featureLayer.SetRenderer(renderer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColorRamps(StyleProjectItem, System.String)
      // cref: ArcGIS.Core.CIM.CIMColorRamp
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SearchColorRamps
      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      // cref: ArcGIS.Core.CIM.CIMSimpleRenderer.Symbol
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.SetRealWorldUnits
      #region How to apply a color ramp from a style to a feature layer
      {
        List<string> fields = new List<string>() { "Field1" };
        //Note: Needs QueuedTask to run
        StyleProjectItem styleToUse =
          Project.Current.GetItems<StyleProjectItem>()
              .FirstOrDefault(s => s.Name == "ColorBrewer Schemes (RGB)");
        if (style == null) return;
        //Note: Needs QueuedTask to run
        var colorRampListFound = styleToUse.SearchColorRamps("Red-Gray (10 Classes)");
        if (colorRampListFound == null || colorRampListFound.Count == 0) return;
        CIMColorRamp cimColorRamp = null;
        CIMRenderer renderer = null;

        cimColorRamp = colorRampListFound[0].ColorRamp;
        var rendererDef = new UniqueValueRendererDefinition(fields, null, cimColorRamp);
        renderer = featureLayer?.CreateRenderer(rendererDef);
        featureLayer?.SetRenderer(renderer);
      }
      #endregion

      #region ProSnippet Group: Maps
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      // cref: ArcGIS.Desktop.Mapping.Map
      #region Get the active map
      {
        //This is how you get the active map
        var mapToGet = MapView.Active?.Map;
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMap(System.String,ArcGIS.Core.CIM.MapType,ArcGIS.Core.CIM.MapViewingMode,ArcGIS.Desktop.Mapping.Basemap)
      // cref: ArcGIS.Desktop.Mapping.Map
      #region Create a new map with a default basemap layer
      {
        //Note: Needs QueuedTask to run
        MapFactory.Instance.CreateMap("The Map", ArcGIS.Core.CIM.MapType.Map, ArcGIS.Core.CIM.MapViewingMode.Map, Basemap.ProjectDefault);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapProjectItem
      // cref: ArcGIS.Desktop.Mapping.MapProjectItem.GetMap()
      // cref: ArcGIS.Desktop.Mapping.Map
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.CreateMapPaneAsync(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapViewingMode, ArcGIS.Desktop.Mapping.TimeRange)
      #region Find a map within a project and open it
      {
        //Finding the first project item with name matches with mapName
        MapProjectItem? mpi = Project.Current.GetItems<MapProjectItem>()
          .FirstOrDefault(m => m.Name.Equals("The Map", StringComparison.CurrentCultureIgnoreCase));
        //Note: Needs QueuedTask to run
        var mapFromItem = mpi?.GetMap();
        if (mapFromItem != null)
        {
          //Open the map in a new map pane
          //Must be called from the UI thread
          await ProApp.Panes.CreateMapPaneAsync(map);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapFactory.CanCreateMapFrom(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMapFromItem(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Mapping.Map
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.CreateMapPaneAsync(ArcGIS.Desktop.Framework.PaneCollection, ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapViewingMode, ArcGIS.Desktop.Mapping.TimeRange)
      #region Open a webmap
      {
        //Assume we get the selected webmap from the Project pane's Portal tab
        if (Project.Current.SelectedItems.Count > 0)
        {
          if (MapFactory.Instance.CanCreateMapFrom(Project.Current.SelectedItems[0]))
          {
            map = MapFactory.Instance.CreateMapFromItem(Project.Current.SelectedItems[0]);
            await ProApp.Panes.CreateMapPaneAsync(map);
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.IMapPane
      #region Get Map Panes
      {
        ProApp.Panes.OfType<IMapPane>().OrderBy((mp) => mp.MapView.Map.URI ?? mp.MapView.Map.Name);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.IMapPane
      // cref: ArcGIS.Desktop.Mapping.IMapPane.MapView
      // cref: ArcGIS.Desktop.Mapping.Map.URI
      #region Get the Unique List of Maps From the Map Panes
      {
        //Gets the unique list of Maps from all the MapPanes.
        //Note: The list of maps retrieved from the MapPanes
        //maybe less than the total number of Maps in the project.
        //It depends on what maps the user has actually opened.
        var mapPanes = ProApp.Panes.OfType<IMapPane>()
                    .GroupBy((mp) => mp.MapView.Map.URI).Select(grp => grp.FirstOrDefault());
        List<Map> uniqueMaps = new List<Map>();
        foreach (var pane in mapPanes)
          uniqueMaps.Add(pane.MapView.Map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Caption
      // cref: ArcGIS.Desktop.Mapping.Map.SetName
      #region Change the Map name and caption of the active pane
      {
        //Note: call within QueuedTask.Run()
        MapView.Active.Map.SetName("Test");
        ProApp.Panes.ActivePane.Caption = "Caption";
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapFactory.CanConvertMap(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapConversionType)
      // cref: ArcGIS.Desktop.Mapping.MapConversionType
      // cref: ArcGIS.Desktop.Mapping.MapFactory.ConvertMap(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.MapConversionType, System.Boolean)
      #region Convert Map to Local Scene
      {
        //Note: Run within the context of QueuedTask.Run
        bool canConvertMap = MapFactory.Instance.CanConvertMap(map, MapConversionType.SceneLocal);
        if (canConvertMap)
          MapFactory.Instance.ConvertMap(map, MapConversionType.SceneLocal, true);
      }
      #endregion
      // cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetBasemapsAsync(ArcGIS.Desktop.Core.ArcGISPortal)
      // cref: ArcGIS.Desktop.Core.Portal.PortalItem
      // cref: ArcGIS.Desktop.Mapping.Map.SetBasemapLayers(ArcGIS.Desktop.Mapping.Basemap)
      #region Get Basemaps
      {
        //Basemaps stored locally in the project. This is usually an empty collection
        //Note: Needs QueuedTask to run
        string localBasemapTypeID = "cim_map_basemap";

        var mapContainer = Project.Current.GetProjectItemContainer("Map");
        mapContainer.GetItems().Where(i => i.TypeID == localBasemapTypeID).ToList();

        //portal basemaps. If there is no current active portal, the usual default
        //is arcgis online
        var portal = ArcGISPortalManager.Current.GetActivePortal();
        var portalBaseMaps = await portal.GetBasemapsAsync();

        //use one of them...local or portal...
        //var map = MapView.Active.Map;
        //QueuedTask.Run(() => map?.SetBasemapLayers(portalBaseMaps[0]));  
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.SaveAsFile(System.String, System.Boolean)
      #region Save Map as MapX
      {
        map.SaveAsFile(@"C:\Data\MyMap.mapx", true);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.SaveAsWebMapFile(System.String)
      #region Save 2D Map as WebMap on Disk
      {
        //2D maps only
        //Must be on the QueuedTask.Run(...)
        if (map.DefaultViewingMode == MapViewingMode.Map)
          //Only webmap compatible layers will be saved out to the file
          map.SaveAsWebMapFile(@"C:\Data\MyMap.json");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.SetClipGeometry(ArcGIS.Core.Geometry.Polygon, ArcGIS.Core.CIM.CIMLineSymbol)
      #region Clip Map to the provided clip polygon
      {
        //Note: Run within QueuedTask
        var mapToClip = MapView.Active.Map;
        //A layer to use for the clip extent
        var lyrOfInterest = mapToClip.GetLayersAsFlattenedList().OfType<FeatureLayer>().Where(l => l.Name == "TestPoly").FirstOrDefault();
        //Get the polygon to use to clip the map
        var extent = lyrOfInterest.QueryExtent();
        var polygonForClipping = PolygonBuilderEx.CreatePolygon(extent);
        //Clip the map using the layer's extent
        mapToClip.SetClipGeometry(polygonForClipping,
              SymbolFactory.Instance.ConstructLineSymbol(
              SymbolFactory.Instance.ConstructStroke(
                ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Dash)));
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.ClearClipGeometry()
      #region Clear the current map clip geometry 
      {
        var mapWithClip = MapView.Active.Map;
        //Clear the Map clip.
        //If no clipping is set then this is a no-op.
        mapWithClip.ClearClipGeometry();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetClipGeometry()
      // cref: ArcGIS.Core.CIM.ClippingMode.MapExtent
      // cref: ArcGIS.Core.CIM.ClippingMode.CustomShape
      #region Get the map clipping geometry
      {
        var mapWithClip = MapView.Active.Map;
        //If clipping is set to ArcGIS.Core.CIM.ClippingMode.None or ArcGIS.Core.CIM.ClippingMode.MapSeries null is returned
        //If clipping is set to ArcGIS.Core.CIM.ClippingMode.MapExtent the ArcGIS.Core.CIM.CIMMap.CustomFullExtent is returned.
        //Otherwise, if clipping is set to ArcGIS.Core.CIM.ClippingMode.CustomShape the custom clip polygon is returned.
        //Note: Must be on the QueuedTask.Run()
        var poly = mapWithClip.GetClipGeometry();
        //You can use the polygon returned
        //For example: We make a polygon graphic element and add it to a Graphics Layer.
        var gl = mapWithClip.GetLayersAsFlattenedList().OfType<GraphicsLayer>().FirstOrDefault();
        if (gl == null) return;
        var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(CIMColor.CreateRGBColor(255, 255, 0));
        var cimGraphicElement = new CIMPolygonGraphic
        {
          Polygon = poly,
          Symbol = polygonSymbol.MakeSymbolReference()
        };
        gl.AddElement(cimGraphicElement);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetLocationUnitFormat()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.DisplayName
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.UnitCode
      #region Get the Current Map Location Unit
      {
        //var map = MapView.Active.Map;
        //Note: Must be on the QueuedTask.Run()

        //Get the current location unit
        var loc_unit = map.GetLocationUnitFormat();
        var line = $"{loc_unit.DisplayName}, {loc_unit.UnitCode}";
        System.Diagnostics.Debug.WriteLine(line);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableLocationUnitFormats()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      #region Get the Available List of Map Location Units
      {
        //Linear location unit formats are not included if the map sr
        //is geographic.
        //Note: Must be on the QueuedTask.Run()
        var loc_units = map.GetAvailableLocationUnitFormats();
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetLocationUnitFormat()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.FormatLocation(ArcGIs.Core.Geometry.Coordinate2D, ArcGIS.Core.Geometry.SpatialReference)
      #region Format a Location Using the Current Map Location Unit
      {
        //Get the current view camera location
        var center_pt = new Coordinate2D(mapView.Camera.X, mapView.Camera.Y);
        //Get the current location unit
        //Note: Must be on the QueuedTask.Run()
        var loc_unit = map.GetLocationUnitFormat();

        //Format the camera location
        var str = loc_unit.FormatLocation(center_pt, map.SpatialReference);
        System.Diagnostics.Debug.WriteLine($"Formatted location: {str}");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableLocationUnitFormats()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      // cref: ArcGIS.Desktop.Mapping.Map.SetLocationUnitFormat(ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat)
      #region Set the Location Unit for the Current Map
      {
        //Get the list of available location unit formats
        //for the current map
        //Note: Must be on the QueuedTask.Run()
        var loc_units = map.GetAvailableLocationUnitFormats();

        //arbitrarily use the last unit in the list
        map.SetLocationUnitFormat(loc_units.Last());
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationUnitFormat()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      #region Get the Current Map Elevation Unit
      {
        var elev_unit = map.GetElevationUnitFormat();
        var line = $"{elev_unit.DisplayName}, {elev_unit.UnitCode}";
        System.Diagnostics.Debug.WriteLine(line);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableElevationUnitFormats()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      #region Get the Available List of Map Elevation Units
      {
        //If the map is not a scene, the list of current
        //Project distance units will be returned
        //Note: Must be on the QueuedTask.Run()
        var elev_units = map.GetAvailableElevationUnitFormats();
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationUnitFormat()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat.FormatValue(System.Double)
      #region Format an Elevation Using the Current Map Elevation Unit
      {
        //Get the current elevation unit. If the map is not
        //a scene the default Project distance unit is returned
        //Note: Must be on the QueuedTask.Run()
        var elev_unit = map.GetElevationUnitFormat();

        //Format the view camera elevation
        var str = elev_unit.FormatValue(mapView.Camera.Z);

        System.Diagnostics.Debug.WriteLine($"Formatted elevation: {str}");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetAvailableElevationUnitFormats()
      // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat
      // cref: ArcGIS.Desktop.Mapping.Map.SetElevationUnitFormat( ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormat)
      #region Set the Elevation Unit for the Current Map
      {
        //Trying to set the elevation unit on a map other than
        //a scene will throw an InvalidOperationException
        if (map.IsScene)
        {
          //Get the list of available elevation unit formats
          //for the current map
          var loc_units = map.GetAvailableElevationUnitFormats();
          //arbitrarily use the last unit in the list
          //Note: Must be on the QueuedTask.Run()
          map.SetElevationUnitFormat(loc_units.Last());
        }
      }
      #endregion

      #region ProSnippet Group: Offline Map
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanGenerateReplicas(ArcGIS.Desktop.Mapping.Map)
      #region Check Map Has Sync-Enabled Content
      {
        var hasSyncEnabledContent = GenerateOfflineMap.Instance.GetCanGenerateReplicas(map);
        if (hasSyncEnabledContent)
        {
          //TODO - use status...
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanGenerateReplicas(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GenerateReplicas(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams)
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams.#ctor
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams.Extent
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateReplicaParams.DestinationFolder
      #region Generate Replicas for Sync-Enabled Content
      {
        //namespace ArcGIS.Desktop.Mapping.Offline
        var extent = MapView.Active.Extent;
        //Check map has sync-enabled content that can be taken offline
        //Note: Run within QueuedTask
        var hasSyncEnabledContent = GenerateOfflineMap.Instance.GetCanGenerateReplicas(map);
        if (hasSyncEnabledContent)
        {
          //Generate Replicas and take the content offline
          //sync-enabled content gets copied local into a SQLite DB
          var gen_params = new GenerateReplicaParams()
          {
            Extent = extent, //SR of extent must match map SR

            //DestinationFolder can be left blank, if specified,
            //it must exist. Defaults to project offline maps location
            DestinationFolder = @"C:\Data\Offline"
          };
          //Sync-enabled layer content will be resourced to point to the
          //local replica content.
          GenerateOfflineMap.Instance.GenerateReplicas(map, gen_params);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanSynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)
      #region Check Map Has Local Syncable Content
      {
        //Check map has local syncable content
        //Note: Run within QueuedTask
        var canSyncContent = GenerateOfflineMap.Instance.GetCanSynchronizeReplicas(map);
        if (canSyncContent)
        {
          //TODO - use status
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanSynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.SynchronizeReplicas(ArcGIS.Desktop.Mapping.Map)
      #region Synchronize Replicas for Syncable Content
      {
        //Check map has local syncable content
        //Note: Run within QueuedTask
        var canSyncContent = GenerateOfflineMap.Instance.GetCanSynchronizeReplicas(map);
        if (canSyncContent)
        {
          //Sync Replicas - changes since last sync are pushed to the
          //parent replica. Parent changes are pulled to the client.
          //Unsaved edits are _not_ sync'd. 
          GenerateOfflineMap.Instance.SynchronizeReplicas(map);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanRemoveReplicas(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.RemoveReplicas(ArcGIS.Desktop.Mapping.Map)
      #region Remove Replicas for Syncable Content
      {
        //Check map has local syncable content
        //Either..
        //var canSyncContent = GenerateOfflineMap.Instance.GetCanSynchronizeReplicas(map);
        //Or...both accomplish the same thing...
        //Note: Run within QueuedTask
        var canRemove = GenerateOfflineMap.Instance.GetCanRemoveReplicas(map);
        if (canRemove)
        {
          //Remove Replicas - any unsync'd changes are lost
          //Call sync _first_ to push any outstanding changes if
          //needed. Local syncable content is re-sourced
          //to point to the service
          GenerateOfflineMap.Instance.RemoveReplicas(map);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanExportRasterTileCache(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetExportRasterTileCacheScales(ArcGIS.Desktop.Mapping.Map, ArcGIs.Core.Geometry.Envelope)
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.#ctor
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.Extent
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.MaximumUserDefinedScale
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.DestinationFolder
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.ExportRasterTileCache(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams)
      #region Export Map Raster Tile Cache Content
      {
        //Does the map have any exportable raster content?
        var canExport = GenerateOfflineMap.Instance.GetCanExportRasterTileCache(map);
        var rasterTileExtent = MapView.Active.Extent;
        if (canExport)
        {
          //Check the available LOD scale ranges
          var scales = GenerateOfflineMap.Instance.GetExportRasterTileCacheScales(map, rasterTileExtent);
          //Pick the desired LOD scale
          var max_scale = scales[scales.Count() / 2];

          //Configure the export parameters
          var export_params = new ExportTileCacheParams()
          {
            Extent = rasterTileExtent,//Use same extent as was used to retrieve scales
            MaximumUserDefinedScale = max_scale
            //DestinationFolder = .... (optional)
          };
          //If DestinationFolder is not set, output defaults to project
          //offline maps location set in the project properties. If that is 
          //not set, output defaults to the current project folder location.

          //Do the export. Depending on the MaximumUserDefinedScale and the
          //area of the extent requested, this can take minutes for tile packages
          //over 1 GB or less if your network speed is slow...
          GenerateOfflineMap.Instance.ExportRasterTileCache(map, export_params);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetCanExportVectorTileCache(ArcGIS.Desktop.Mapping.Map)
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.GetExportVectorTileCacheScales(ArcGIS.Desktop.Mapping.Map, ArcGIs.Core.Geometry.Envelope)
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.#ctor
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.Extent
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.MaximumUserDefinedScale
      // cref: ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams.DestinationFolder
      // cref: ArcGIS.Desktop.Mapping.Offline.GenerateOfflineMap.ExportVectorTileCache(ArcGIS.Desktop.Mapping.Map, ArcGIS.Desktop.Mapping.Offline.ExportTileCacheParams)
      #region Export Map Vector Tile Cache Content
      {
        var mapVectorTileExtent = MapView.Active.Extent;
        //Does the map have any exportable vector tile content?
        var canExport = GenerateOfflineMap.Instance.GetCanExportVectorTileCache(map);
        if (canExport)
        {
          //Check the available LOD scale ranges
          var scales = GenerateOfflineMap.Instance.GetExportVectorTileCacheScales(map, mapVectorTileExtent);
          //Pick the desired LOD scale
          var max_scale = scales[scales.Count() / 2];

          //Configure the export parameters
          var export_params = new ExportTileCacheParams()
          {
            Extent = mapVectorTileExtent,//Use same extent as was used to retrieve scales
            MaximumUserDefinedScale = max_scale,
            DestinationFolder = @"C:\Data\Offline"
          };
          //If DestinationFolder is not set, output defaults to project
          //offline maps location set in the project properties. If that is 
          //not set, output defaults to the current project folder location.

          //Do the export. Depending on the MaximumUserDefinedScale and the
          //area of the extent requested, this can take minutes for tile packages
          //over 1 GB or less if your network speed is slow...
          GenerateOfflineMap.Instance.ExportVectorTileCache(map, export_params);
        }
      }
      #endregion
      #region ProSnippet Group: Create Layer
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer(System.Uri, ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
      #region Create and add a layer to the active map
      {
        //* string url = @"c:\data\project.gdb\DEM";  //Raster dataset from a FileGeodatabase
        //* string url = @"c:\connections\mySDEConnection.sde\roads";  //FeatureClass of a SDE
        //* string url = @"c:\connections\mySDEConnection.sde\States\roads";  //FeatureClass within a FeatureDataset from a SDE
        //* string url = @"c:\data\roads.shp";  //Shapefile
        //* string url = @"c:\data\imagery.tif";  //Image from a folder
        //* string url = @"c:\data\mySDEConnection.sde\roads";  //.lyrx or .lpkx file
        //* string url = @"c:\data\CAD\Charlottesville\N1W1.dwg\Polyline";  //FeatureClass in a CAD dwg file
        //* string url = @"C:\data\CAD\UrbanHouse.rvt\Architectural\Windows"; //Features in a Revit file
        //* string url = @"http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer";  //map service
        //* string url = @"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0";  //FeatureLayer off a map service or feature service

        string urlRoads = @"c:\data\project.gdb\roads";  //FeatureClass of a FileGeodatabase
        //Note: Needs QueuedTask to run
        Uri uri = new Uri(urlRoads);
        var newLayer = LayerFactory.Instance.CreateLayer(uri, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create layer with create-params
      {
        var flyrCreatnParam = new FeatureLayerCreationParams(new Uri(@"c:\data\world.gdb\cities"))
        {
          Name = "World Cities",
          IsVisible = false,
          MinimumScale = 1000000,
          MaximumScale = 5000,
          DefinitionQuery = new DefinitionQuery(whereClause: "Population > 100000", name: "More than 100k"),
          RendererDefinition = new SimpleRendererDefinition()
          {
            SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(
           CIMColor.CreateRGBColor(255, 0, 0), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
          }
        };
        //Note: Needs QueuedTask to run
        var featureLayerWithParams = LayerFactory.Instance.CreateLayer<FeatureLayer>(flyrCreatnParam, map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LayerDocument
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(System.String)
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.GetCIMLayerDocument()
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMLayerDocument)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create FeatureLayer and add to Map using LayerCreationParams
      {
        //Get the LayerDocument from a lyrx file
        var layerDoc = new LayerDocument(@"E:\Data\SDK\Default2DPointSymbols.lyrx");
        //Get the CIMLayerDocument from the LayerDocument and use it to create LayerCreationParams
        var createParams = new LayerCreationParams(layerDoc.GetCIMLayerDocument());
        //Create a FeatureLayer using the LayerCreationParams
        //Note: Needs QueuedTask to run
        LayerFactory.Instance.CreateLayer<FeatureLayer>(createParams, MapView.Active.Map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create FeatureLayer and set to not display in Map.
      {
        //The catalog path of the feature layer to add to the map
        var featureClassUriVisibility = new Uri(@"C:\Data\Admin\AdminData.gdb\USA\cities");
        //Define the Feature Layer's parameters.
        var layerParamsVisibility = new FeatureLayerCreationParams(featureClassUriVisibility)
        {
          //Set visibility
          IsVisible = false,
        };
        //Create the layer with the feature layer parameters and add it to the active map
        //Note: Needs QueuedTask to run
        var createdFC = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParamsVisibility, MapView.Active.Map);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
      //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
      //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
      //cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition
      //cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      //cref: ArcGIS.Desktop.Mapping.LayerFactory

      #region Create FeatureLayer with a Renderer
      {
        //Define a simple renderer to draw the Point US Cities feature class.
        var simpleRender = new SimpleRendererDefinition
        {
          SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 4.0, SimpleMarkerStyle.Circle).MakeSymbolReference()

        };
        //The catalog path of the feature layer to add to the map
        var featureClassUri = new Uri(@"C:\Data\Admin\AdminData.gdb\USA\cities");
        //Define the Feature Layer's parameters.
        var layerParams = new FeatureLayerCreationParams(featureClassUri)
        {
          //Set visibility
          IsVisible = true,
          //Set Renderer
          RendererDefinition = simpleRender,
        };
        //Create the layer with the feature layer parameters and add it to the active map
        //Note: Needs QueuedTask to run
        var createdFCWithRenderer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams
      //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(System.Uri)
      //cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.DefinitionQuery
      //cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)

      #region Create FeatureLayer with a Query Definition
      {
        //The catalog path of the feature layer to add to the map
        var featureClassUriDefinition = new Uri(@"C:\Data\Admin\AdminData.gdb\USA\cities");
        //Define the Feature Layer's parameters.
        var layerParamsQueryDefn = new FeatureLayerCreationParams(featureClassUriDefinition)
        {
          IsVisible = true,
          DefinitionQuery = new DefinitionQuery(whereClause: "STATE_NAME = 'California'", name: "CACities")
        };
        //Note: Needs QueuedTask to run
        //Create the layer with the feature layer parameters and add it to the active map
        var createdFCWithQueryDefn = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParamsQueryDefn, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(System.Collections.Generic.IEnumerable{System.Uri},ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32)
      #region Create multiple layers
      {
        //Define a list of dataset URIs for the layers to be created  
        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");
        // Create a list of URIs to be used for creating multiple layers
        var uris = new List<Uri>() { uriShp, uriSde, uri };
        // Create multiple layers using the LayerFactory
        //Note: Needs QueuedTask to run
        var layers = LayerFactory.Instance.CreateLayers(uris, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(System.Collections.Generic.IEnumerable{System.Uri},ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      #region Create multiple layers in bulk - simple
      {
        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");

        var uris = new List<Uri>() { uriShp, uriSde, uri };

        var layers = LayerFactory.Instance.CreateLayers(uris, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{System.Uri})
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.MapMemberPosition
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.MapMemberIndex
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.IsVisible
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      #region Create multiple layers with BulkLayerCreationParams 1
      {
        //Uris to the datasets for the layers to be created
        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");
        // Create a list of URIs to be used for creating multiple layers
        var uris = new List<Uri>() { uriShp, uriSde, uri }; ;

        // set the index and visibility
        var blkParams = new BulkLayerCreationParams(uris);
        blkParams.MapMemberPosition = MapMemberPosition.Index;
        blkParams.MapMemberIndex = 2;
        blkParams.IsVisible = false;
        // Create multiple layers using the BulkLayerCreationParams
        //Note: Needs QueuedTask to run
        var layers = LayerFactory.Instance.CreateLayers(blkParams, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.LayerCreationParams})
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      #region Create multiple layers with BulkLayerCreationParams 2 - invalid
      {
        // get a feature class that is selected in the catalog pane
        Item item = Project.GetCatalogPane().SelectedItems.FirstOrDefault();

        var uriShp = new Uri(@"c:\data\roads.shp");
        var lcpShp = new FeatureLayerCreationParams(uriShp);
        lcpShp.Name = "Roads";
        lcpShp.IsVisible = false;
        lcpShp.DefinitionQuery = new DefinitionQuery("shpQuery", "OBJECTID > 10");

        var lcpItem = new FeatureLayerCreationParams(item);
        lcpItem.Name = "Census Polygons";
        lcpItem.IsVisible = true;

        // list contains a Uri data source and an Item data source
        // LayerFactory.Instance.CreateLayers will throw an ArgumentException
        var lcps = new List<FeatureLayerCreationParams>();
        lcps.Add(lcpShp);
        lcps.Add(lcpItem);

        var blkParams = new BulkLayerCreationParams(lcps);

        // LayerFactory.Instance.CreateLayers will thrown an ArgumentException
        // because the LayerCreationParams are created using different 
        // types of data sources (1 Uri and 1 Item)
        var layers = LayerFactory.Instance.CreateLayers(blkParams, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.LayerCreationParams})
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.MapMemberPosition
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.MapMemberIndex
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      #region Create multiple layers with BulkLayerCreationParams 2 - Valid
      {

        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");

        var lcpShp = new FeatureLayerCreationParams(uriShp);
        lcpShp.Name = "Roads";
        lcpShp.IsVisible = false;
        lcpShp.DefinitionQuery = new DefinitionQuery("shpQuery", "OBJECTID > 10");

        var lcpSde = new FeatureLayerCreationParams(uriSde);
        lcpSde.Name = "Census Polygons";
        lcpSde.IsVisible = true;

        var lcpService = new FeatureLayerCreationParams(uri);
        lcpService.Name = "Shelters";
        lcpService.IsVisible = true;
        // set some renderer here ...
        //lcpService.RendererDefinition = ...

        var lcps = new List<FeatureLayerCreationParams>();
        lcps.Add(lcpShp);
        lcps.Add(lcpSde);
        lcps.Add(lcpService);

        var blkParams = new BulkLayerCreationParams(lcps);
        // set the positioning on the BulkLayerCreationParams
        blkParams.MapMemberPosition = MapMemberPosition.Index;
        blkParams.MapMemberIndex = 0;

        var layers = LayerFactory.Instance.CreateLayers(blkParams, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Mapping.LayerCreationParams})
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.MapMemberPosition
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.MapMemberIndex
      // cref: ArcGIS.Desktop.Mapping.BulkLayerCreationParams.RollbackBehavior
      // cref: ArcGIS.Desktop.Mapping.LayerCreationRollbackBehavior
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      #region Create multiple layers with BulkLayerCreationParams - Using RollbackBehavior
      {

        var uriShp = new Uri(@"c:\data\roads.shp");
        var uriSde = new Uri(@"c:\MyDataConnections\MySDE.sde\Census");
        var uri = new Uri(@"http://sampleserver6.arcgisonline.com/arcgis/rest/services/NapervilleShelters/FeatureServer/0");

        var lcpShp = new FeatureLayerCreationParams(uriShp);
        lcpShp.Name = "Roads";
        lcpShp.IsVisible = false;
        lcpShp.DefinitionQuery = new DefinitionQuery("shpQuery", "OBJECTID > 10");

        var lcpSde = new FeatureLayerCreationParams(uriSde);
        lcpSde.Name = "Census Polygons";
        lcpSde.IsVisible = true;

        var lcpService = new FeatureLayerCreationParams(uri);
        lcpService.Name = "Shelters";
        lcpService.IsVisible = true;
        // set some renderer here ...
        //lcpService.RendererDefinition = ...

        var lcps = new List<FeatureLayerCreationParams>();
        lcps.Add(lcpShp);
        lcps.Add(lcpSde);
        lcps.Add(lcpService);

        var blkParams = new BulkLayerCreationParams(lcps);
        // set the positioning on the BulkLayerCreationParams
        blkParams.MapMemberPosition = MapMemberPosition.Index;
        blkParams.MapMemberIndex = 0;

        // set the rollback behavior
        // - rollback if one or more layers cannot be created due to an invalid data source
        blkParams.RollbackBehavior = LayerCreationRollbackBehavior.RollbackOnMissingLayers;

        var layers = LayerFactory.Instance.CreateLayers(blkParams, MapView.Active.Map);
      }
      #endregion

      //cref:ArcGIS.Desktop.Mapping.BulkLayerCreationParams
      //cref:ArcGIS.Desktop.Mapping.BulkLayerCreationParams.#ctor(System.Collections.Generic.IEnumerable{System.Uri})
      //cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayers(ArcGIS.Desktop.Mapping.BulkLayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      //cref: ArcGIS.Desktop.Core.ItemFactory.Create(System.String,ArcGIS.Desktop.Core.ItemFactory.ItemType)
      #region Add a GeoPackage to the Map
      {
        string pathToGeoPackage = @"C:\Data\Geopackage\flooding.gpkg";
        //Create lists to hold the URIs of the layers and tables in the geopackage
        var layerUris = new List<Uri>();
        var tableUris = new List<Uri>();
        //Create an item from the geopackage
        //Note: Needs QueuedTask to run
        var item = ItemFactory.Instance.Create(pathToGeoPackage, ItemFactory.ItemType.PathItem);
        var children = item.GetItems();
        //Collect the table and spatial data in the geopackage
        foreach (var child in children)
        {
          var childPath = child.Path;

          if (child.TypeID == "sqlite_table")
            tableUris.Add(new Uri(childPath));
          else
            layerUris.Add(new Uri(childPath));
        }
        //Add the spatial data in the geopackage using the BulkLayerCreationParams
        if (layerUris.Count > 0)
        {
          BulkLayerCreationParams bulklcp = new BulkLayerCreationParams(layerUris);
          LayerFactory.Instance.CreateLayers(bulklcp, MapView.Active.Map);
        }
        // add the tables separately
        foreach (var tableUri in tableUris)
        {
          StandaloneTableFactory.Instance.CreateStandaloneTable(tableUri, MapView.Active.Map);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TopologyLayer
      // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.AddAssociatedLayers
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)

      #region Create TopologyLayer with an Uri pointing to a Topology dataset
      {
        var path = @"D:\Data\CommunitySamplesData\Topology\GrandTeton.gdb\BackCountry\Backcountry_Topology";
        var lcp = new TopologyLayerCreationParams(new Uri(path));
        lcp.Name = "GrandTeton_Backcountry";
        lcp.AddAssociatedLayers = true;
        //Note: Needs QueuedTask to run
        var topoLayer = LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.TopologyLayer>(lcp, MapView.Active.Map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.TopologyLayer.GetTopology
      // cref: ArcGIS.Desktop.Mapping.TopologyLayer
      // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      // cref: ArcGIS.Desktop.Mapping.TopologyLayerCreationParams.AddAssociatedLayers
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)

      #region Create Topology Layer using Topology dataset
      {
        //Get the Topology of another Topology layer
        var existingTopology = MapView.Active.Map.GetLayersAsFlattenedList().OfType<TopologyLayer>().FirstOrDefault();
        if (existingTopology != null)
        {
          var topology = existingTopology.GetTopology();
          //Configure the settings for a new Catalog layer using the CatalogDataset of an existing layer
          var topologyLyrParams = new TopologyLayerCreationParams(topology);
          topologyLyrParams.Name = "NewTopologyLayerFromAnotherTopologyLayer";
          topologyLyrParams.AddAssociatedLayers = true;
          //Note: Needs QueuedTask to run
          LayerFactory.Instance.CreateLayer<TopologyLayer>(topologyLyrParams, MapView.Active.Map);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.CatalogLayer
      // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)

      #region Create Catalog Layer using Uri to a Catalog Feature Class
      {
        var createParams = new CatalogLayerCreationParams(new Uri(@"C:\CatalogLayer\CatalogDS.gdb\HurricaneCatalogDS"));
        //Set the definition query
        createParams.DefinitionQuery = new DefinitionQuery("Query1", "cd_itemname = 'PuertoRico'");
        //Set name of the new Catalog Layer
        createParams.Name = "PuertoRico";
        //Create Layer
        //Note: Needs QueuedTask to run
        var catalogLayer = LayerFactory.Instance.CreateLayer<CatalogLayer>(createParams, MapView.Active.Map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.CatalogLayer
      // cref: ArcGIS.Desktop.Mapping.CatalogLayer.GetCatalogDataset
      // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.#ctor(ArcGIS.Core.Data.Mapping.CatalogDataset)
      // cref: ArcGIS.Desktop.Mapping.CatalogLayerCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)

      #region Create Catalog Layer using CatalogDataset
      {
        //Get the CatalogDataset of another Catalog layer
        var existingCatalogLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<CatalogLayer>().FirstOrDefault();
        if (existingCatalogLayer != null)
        {
          var catalogDataset = existingCatalogLayer.GetCatalogDataset();
          //Configure the settings for a new Catalog layer using the CatalogDataset of an existing layer
          var catalogLyrParams = new CatalogLayerCreationParams(catalogDataset);
          catalogLyrParams.Name = "NewCatalogLayerFromAnotherCatalogLayer";
          catalogLyrParams.DefinitionQuery = new DefinitionQuery("Query1", "cd_itemname = 'Asia'");
          //Note: Needs QueuedTask to run
          LayerFactory.Instance.CreateLayer<CatalogLayer>(catalogLyrParams, MapView.Active.Map);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.LayerTemplatePackages
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Add MapNotes to the active map
      {
        //Gets the collection of layer template packages installed with Pro for use with maps
        var items = MapView.Active.Map.LayerTemplatePackages;
        //Iterate through the collection of items to add each Map Note to the active map
        foreach (var item in items)
        {
          //Create a parameter item for the map note
          var layer_params = new LayerCreationParams(item);
          layer_params.IsVisible = false;
          //Note: Needs QueuedTask to run
          //Create a feature layer for the map note
          var layerCreated = LayerFactory.Instance.CreateLayer<Layer>(layer_params, MapView.Active.Map);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition
      #region Apply Symbology from a Layer in the TOC
      {
        //Note: Call within QueuedTask.Run()
        if (MapView.Active.Map == null) return;

        //Get an existing Layer. This layer has a symbol you want to use in a new layer.
        var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>()
              .Where(l => l.ShapeType == esriGeometryType.esriGeometryPoint).FirstOrDefault();
        //This is the renderer to use in the new Layer
        var renderer = lyr.GetRenderer() as CIMSimpleRenderer;
        //Set the DataConnection for the new layer
        Geodatabase geodatabase = new Geodatabase(
          new FileGeodatabaseConnectionPath(new Uri(@"E:\Data\Admin\AdminData.gdb")));
        FeatureClass featureClass = geodatabase.OpenDataset<FeatureClass>("Cities");
        var dataConnection = featureClass.GetDataConnection();
        //Create the definition for the new feature layer
        var featureLayerParams = new FeatureLayerCreationParams(dataConnection)
        {
          RendererDefinition = new SimpleRendererDefinition(renderer.Symbol),
          IsVisible = true,
        };
        //create the new layer
        LayerFactory.Instance.CreateLayer<FeatureLayer>(
          featureLayerParams, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.SubtypeLayers
      // cref: ArcGIS.Desktop.Mapping.SubtypeFeatureLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.SubtypeFeatureLayerCreationParams.#ctor(ArcGIS.Desktop.Mapping.RendererDefinition, System.Int32)
      // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayerCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      // cref: ArcGIS.Desktop.Mapping.SubtypeGroupLayer
      #region Create a new SubTypeGroupLayer
      {
        var subtypeGroupLayerCreateParam = new SubtypeGroupLayerCreationParams
          (new Uri(@"c:\data\SubtypeAndDomain.gdb\Fittings"));

        // Define Subtype layers
        var rendererDefn1 = new UniqueValueRendererDefinition(new List<string> { "type" });
        var renderDefn2 = new SimpleRendererDefinition()
        {
          SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(
                  CIMColor.CreateRGBColor(255, 0, 0), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
        };
        subtypeGroupLayerCreateParam.SubtypeLayers = new List<SubtypeFeatureLayerCreationParams>()
      {
            //define first subtype layer with unique value renderer
            new SubtypeFeatureLayerCreationParams(new UniqueValueRendererDefinition(new List<string> { "type" }), 1),

            //define second subtype layer with simple symbol renderer
            new SubtypeFeatureLayerCreationParams(renderDefn2, 2)
      };

        // Define additional parameters
        subtypeGroupLayerCreateParam.DefinitionQuery = new DefinitionQuery(whereClause: "Enabled = 1", name: "IsActive");
        subtypeGroupLayerCreateParam.IsVisible = true;
        subtypeGroupLayerCreateParam.MinimumScale = 50000;
        //Note: Needs QueuedTask to run
        SubtypeGroupLayer subtypeGroupLayer2 = LayerFactory.Instance.CreateLayer<SubtypeGroupLayer>(
                      subtypeGroupLayerCreateParam, MapView.Active.Map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LayerDocument
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(System.String)
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.GetCIMLayerDocument
      // cref: ArcGIS.Core.CIM.CIMLayerDocument.LayerDefinitions
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.Save(System.String)
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.AsJson()
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.Load(System.String)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create layer from a lyrx file
      {
        //Note: Call within QueuedTask.Run()
        var lyrDocFromLyrxFile = new LayerDocument(@"d:\data\cities.lyrx");
        var cimLyrDoc = lyrDocFromLyrxFile.GetCIMLayerDocument();

        //modifying its renderer symbol to red
        var cimSimpleRenderer = ((CIMFeatureLayer)cimLyrDoc.LayerDefinitions[0]).Renderer as CIMSimpleRenderer;
        cimSimpleRenderer?.Symbol.Symbol.SetColor(new CIMRGBColor() { R = 255 });

        //optionally save the updates out as a file
        lyrDocFromLyrxFile.Save(@"c:\data\cities_red.lyrx");

        //get a json representation of the layer document and you want store away...
        var aJSONString = lyrDocFromLyrxFile.AsJson();

        //... and load it back when needed
        lyrDocFromLyrxFile.Load(aJSONString);
        cimLyrDoc = lyrDocFromLyrxFile.GetCIMLayerDocument();

        //create a layer and add it to a map
        var lcp = new LayerCreationParams(cimLyrDoc);
        var lyr = LayerFactory.Instance.CreateLayer<FeatureLayer>(lcp, map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LayerDocument
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(System.String)
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.GetCIMLayerDocument
      // cref: ArcGIS.Core.CIM.CIMLayerDocument.LayerDefinitions
      // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.Renderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Apply Symbology to a layer from a Layer file
      {
        IEnumerable<FeatureLayer> featureLayers = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>()
          .Where(l => l.ShapeType == esriGeometryType.esriGeometryPoint);
        var layerFile = @"C:\Data\SDK\UniqueValuePointLayer.lyrx";
        foreach (var featureLayerToSymbolize in featureLayers)
        {
          //Get the Layer Document from the lyrx file
          var lyrDocFromLyrxFile = new LayerDocument(layerFile);
          var cimLyrDoc = lyrDocFromLyrxFile.GetCIMLayerDocument();

          //Get the renderer from the layer file
          var rendererFromLayerFile = ((CIMFeatureLayer)cimLyrDoc.LayerDefinitions[0]).Renderer as CIMUniqueValueRenderer;

          //Apply the renderer to the feature layer
          //Note: If working with a raster layer, use the SetColorizer method.
          featureLayerToSymbolize?.SetRenderer(rendererFromLayerFile);
        }
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMInternetServerConnection
      // cref: ArcGIS.Core.CIM.CIMWMSServiceConnection
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Add a WMS service
      {
        // Create a connection to the WMS server
        var serverConnection = new CIMInternetServerConnection { URL = "URL of the WMS service" };
        var connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };

        // Add a new layer to the map
        var layerParams = new LayerCreationParams(connection);
        //Note: Needs QueuedTask to run
        var layerCreated = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
      }
      #endregion

      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection
      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.WorkspaceConnectionString
      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.WorkspaceFactory
      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.Dataset
      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.DatasetType
      // cref: ArcGIS.Core.CIM.esriDatasetType
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Add a WFS Service
      {
        CIMStandardDataConnection cIMStandardDataConnection = new CIMStandardDataConnection()
        {
          WorkspaceConnectionString = @"SWAPXY=TRUE;SWAPXYFILTER=FALSE;URL=http://sampleserver6.arcgisonline.com/arcgis/services/SampleWorldCities/MapServer/WFSServer;VERSION=2.0.0",
          WorkspaceFactory = WorkspaceFactory.WFS,
          Dataset = "Continent",
          DatasetType = esriDatasetType.esriDTFeatureClass
        };

        // Add a new layer to the map
        var layerPamsDC = new LayerCreationParams(cIMStandardDataConnection);
        //Note: Needs QueuedTask to run
        Layer layerCreated = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerPamsDC, MapView.Active.Map);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMProjectServerConnection.URL
      // cref: ArcGIS.Core.CIM.CIMWMTSServiceConnection.ServerConnection
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      #region Add a WMTS service
      {
        // Create a connection to the WMS server
        var serverConnection = new CIMProjectServerConnection
        {
          URL = "URL of the WMTS service.xml",
          ServerType = ServerType.WMTS,
        };
        var service_connection = new CIMWMTSServiceConnection
        {
          ServerConnection = serverConnection,
          LayerName = "AdminBoundaries", // Specify the layer name you want to add
        };

        // Add a new layer to the map
        var layerParams = new LayerCreationParams(service_connection);
        layerParams.MapMemberPosition = MapMemberPosition.AddToBottom;
        //Note: Needs QueuedTask to run
        var layerCreated = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
      // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem.ServerConnection
      // cref: ArcGIS.Core.CIM.CIMAGSSServiceConnection.ServerConnection
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      #region Connect to an AGS Service using a .ags File
      {
        //This workflow would work for 
        var agsFilePath = @"C:\Data\ServerConnectionFiles\AcmeSampleService.ags";

        //ServerConnectionProjectItem supports .ags, .wms, .wmts, .wfs, and .wcs files
        var server_conn_item = ItemFactory.Instance.Create(agsFilePath)
          as ArcGIS.Desktop.Catalog.ServerConnectionProjectItem;

        //Get the server connection - passwords are never returned
        var serverConnection = server_conn_item.ServerConnection as CIMProjectServerConnection;

        //Add to an AGS service connection
        var service_connection = new CIMAGSServiceConnection()
        {
          URL = "URL to the AGS _service_ on the AGS _server_",
          ServerConnection = serverConnection
        };

        // Add a new layer to the map
        var layerParams = new LayerCreationParams(service_connection);
        layerParams.MapMemberPosition = MapMemberPosition.AddToBottom;
        //Note: Needs QueuedTask to run
        var layerCreated = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, map);
      }
      #endregion
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.#ctor(System.Uri)
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.URL
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.User
      // cref: ArcGIS.Core.Data.ServiceConnectionProperties.Password
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(System.Uri)
      #region Connect to an AGS Service using ServiceConnectionProperties
      {
        //Connect to the AGS service. Note: the connection will persist for the
        //duration of the Pro session.
        var serverUrl = "https://sampleserver6.arcgisonline.com/arcgis/rest/services";

        var username = "user1";
        var password = "user1";

        //at any point before creating a layer, first make a connection to the server
        //if one has not been already established for the current session
        var uri = new Uri(serverUrl);
        var props = new ServiceConnectionProperties(uri)
        {
          User = username
        };
        //It is preferred that you use the Windows Credential Manager to store the password

        //However, it can be set as clear text if you so choose
        props.Password = password; //not recommended


        //Establish a connection to the server at any point in time _before_
        //creating a layer from a service on the server
        //Note: Needs QueuedTask to run
        var gdb = new Geodatabase(props);
        gdb.Dispose();//you do not need the geodatabase object after you have connected.

        //later in the session, as needed...create a layer from one of the services
        //on the server
        var serviceUrl = "https://sampleserver6.arcgisonline.com/arcgis/rest/services/Wildfire_secure/MapServer";
        var lc = new LayerCreationParams(new Uri(serviceUrl));
        //Note: Needs QueuedTask to run
        LayerFactory.Instance.CreateLayer<MapImageLayer>(lc, map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapMemberPosition
      // cref: ArcGIS.Desktop.Mapping.WMSLayer
      // cref: ArcGIS.Desktop.Mapping.ServiceCompositeSubLayer
      // cref: ArcGIS.Desktop.Mapping.WMSSubLayer
      // cref: ArcGIS.Desktop.Mapping.WMSSubLayer.GetStyleNames
      // cref: ArcGIS.Desktop.Mapping.WMSSubLayer.SetStyleName(System.String)
      #region Adding and changing styles for WMS Service Layer
      {
        var serverConnection = new CIMInternetServerConnection { URL = "https://spritle.esri.com/arcgis/services/sanfrancisco_sld/MapServer/WMSServer" };
        var connection = new CIMWMSServiceConnection { ServerConnection = serverConnection };
        LayerCreationParams parameters = new LayerCreationParams(connection);
        parameters.MapMemberPosition = MapMemberPosition.AddToBottom;
        //Note: Needs QueuedTask to run
        var compositeLyr = LayerFactory.Instance.CreateLayer<WMSLayer>(parameters, MapView.Active.Map);
        //wms layer in ArcGIS Pro always has a composite layer inside it
        var wmsLayers = compositeLyr.Layers[0] as ServiceCompositeSubLayer;
        //each wms sublayer belongs in that composite layer
        var highwayLayerWMSSub = wmsLayers.Layers[1] as WMSSubLayer;
        //toggling a sublayer's visibility
        if ((highwayLayerWMSSub != null))
        {
          bool visibility = highwayLayerWMSSub.IsVisible;
          highwayLayerWMSSub.SetVisibility(!visibility);
        }
        //applying an existing style to a wms sub layer
        var pizzaLayerWMSSub = wmsLayers.Layers[0] as WMSSubLayer;
        var currentStyles = pizzaLayerWMSSub.GetStyleNames();
        pizzaLayerWMSSub.SetStyleName(currentStyles[1]);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.WorkspaceConnectionString
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.GeometryType
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.OIDFields
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.Srid
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.SqlQuery
      // cref: ArcGIS.Core.CIM.CIMSqlQueryDataConnection.Dataset
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create a query layer
      {
        Geodatabase geodatabase = new Geodatabase(new DatabaseConnectionFile(new Uri(@"C:\Connections\mySDE.sde")));
        CIMSqlQueryDataConnection sqldc = new CIMSqlQueryDataConnection()
        {
          WorkspaceConnectionString = geodatabase.GetConnectionString(),
          GeometryType = esriGeometryType.esriGeometryPolygon,
          OIDFields = "OBJECTID",
          Srid = "102008",
          SqlQuery = "select * from MySDE.dbo.STATES",
          Dataset = "States"
        };
        var lcp = new LayerCreationParams(sqldc)
        {
          Name = "States"
        };
        //Note: Needs QueuedTask to run
        FeatureLayer flyr = LayerFactory.Instance.CreateLayer<FeatureLayer>(lcp, map);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.ClassificationMethod, System.Int32, ArcGIS.Core.CIM.CIMColorRamp, ArcGIS.Core.CIM.CIMSymbolReference)
      #region Create a feature layer with class breaks renderer with defaults
      {
        var featureLayerCreationParams = new FeatureLayerCreationParams(new Uri(@"c:\data\countydata.gdb\counties"))
        {
          Name = "Population Density (sq mi) Year 2010",
          RendererDefinition = new GraduatedColorsRendererDefinition("POP10_SQMI")
        };
        LayerFactory.Instance.CreateLayer<FeatureLayer>(
          featureLayerCreationParams,
          MapView.Active.Map
        );
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition
      // cref: ArcGIS.Core.CIM.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ClassificationField
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ClassificationMethod
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.BreakCount
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ColorRamp
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ExclusionClause
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ExclusionSymbol
      // cref: ArcGIS.Desktop.Mapping.ClassBreaksRendererDefinition.ExclusionLabel
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColorRamps(ArcGIS.Desktop.Mapping.StyleProjectItem, System.String)

      #region Create a feature layer with class breaks renderer
      {
        string colorBrewerSchemesName = "ColorBrewer Schemes (RGB)";
        StyleProjectItem colorBrewerStyle = Project.Current.GetItems<StyleProjectItem>().First(s => s.Name == colorBrewerSchemesName);
        string colorRampName = "Greens (Continuous)";
        //Note: Needs QueuedTask to run
        IList<ColorRampStyleItem> colorRampListFromTheStyle = colorBrewerStyle.SearchColorRamps(colorRampName);

        ColorRampStyleItem colorRampFound = colorRampList[0];
        GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
        {
          ClassificationField = "CROP_ACR07",
          ClassificationMethod = ArcGIS.Core.CIM.ClassificationMethod.NaturalBreaks,
          BreakCount = 6,
          ColorRamp = colorRampFound.ColorRamp,
          SymbolTemplate = SymbolFactory.Instance.ConstructPolygonSymbol(
                                  ColorFactory.Instance.GreenRGB, SimpleFillStyle.Solid, null).MakeSymbolReference(),
          ExclusionClause = "CROP_ACR07 = -99",
          ExclusionSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(
                                  ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, null).MakeSymbolReference(),
          ExclusionLabel = "No yield",
        };
        var featureLayerCreationParams = new FeatureLayerCreationParams((new Uri(@"c:\Data\CountyData.gdb\Counties")))
        {
          Name = "Crop",
          RendererDefinition = gcDef
        };
        //Note: Needs QueuedTask to run
        LayerFactory.Instance.CreateLayer<FeatureLayer>(featureLayerCreationParams, MapView.Active.Map);
      }
      #endregion

      #region ProSnippet Group: Basemap Layers
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.SetBasemapLayers(ArcGIS.Desktop.Mapping.Basemap)        
      #region Update a map's basemap layer
      {
        //Note: Run within QueuedTask
        map.SetBasemapLayers(Basemap.Gray);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.SetBasemapLayers(ArcGIS.Desktop.Mapping.Basemap)
      #region Remove basemap layer from a map
      {
        //Note: Run within QueuedTask
        map.SetBasemapLayers(Basemap.None);
      }
      #endregion

      #region ProSnippet Group: Working with Layers
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
      #region Get a list of layers filtered by layer type from a map
      {
        List<FeatureLayer> featureLayerList = map.GetLayersAsFlattenedList().OfType<FeatureLayer>().ToList();
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.ShapeType
      //cref: ArcGIS.Core.CIM.esriGeometryType
      #region Get a layer of a certain geometry type
      {
        //Get an existing Layer. This layer has a symbol you want to use in a new layer.
        var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>()
                .Where(l => l.ShapeType == esriGeometryType.esriGeometryPoint).FirstOrDefault();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.FindLayer(System.String,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Map.FindLayers(System.String,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
      #region Find a layer
      {
        //Finds layers by name and returns a read only list of Layers
        IReadOnlyList<Layer> layers = map.FindLayers("cities", true);

        //Finds a layer using a URI.
        //The Layer URI you pass in helps you search for a specific layer in a map
        var lyrFindLayer = MapView.Active.Map.FindLayer("CIMPATH=map/u_s__states__generalized_.xml");

        //This returns a collection of layers of the "name" specified. You can use any Linq expression to query the collection.  
        var lyrExists = MapView.Active.Map.GetLayersAsFlattenedList()
                           .OfType<FeatureLayer>().Any(f => f.Name == "U.S. States (Generalized)");
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.FindStandaloneTable(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      #region Find a standalone table
      {
        // these routines find a standalone table whether it is a child of the Map or a GroupLayer
        var tblFind = map.FindStandaloneTable("CIMPATH=map/address_audit.xml");

        IReadOnlyList<StandaloneTable> tables = map.FindStandaloneTables("addresses");

        // this method finds a standalone table as a child of the map only
        var table = map.StandaloneTables.FirstOrDefault(t => t.Name == "Addresses");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList
      #region Find a layer using partial name search
      {
        IEnumerable<Layer> matches = map.GetLayersAsFlattenedList().Where(l => l.Name.IndexOf("partialLayerName", StringComparison.CurrentCultureIgnoreCase) >= 0);
        List<Layer> layers = new List<Layer>();
        foreach (Layer l in matches)
          layers.Add(l);
        System.Diagnostics.Debug.WriteLine($"Found {layers.Count} layers with name containing '{"partialLayerName"}'");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Layer.IsVisible
      // cref: ArcGIS.Desktop.Mapping.Layer.SetVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.IsEditable
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetEditable(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsSnappable
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetSnappable(System.Boolean)
      #region Change layer visibility, editability, snappability
      {
        if (!layer.IsVisible)
          layer.SetVisibility(true);

        if (layer is FeatureLayer featureLayerToChange)
        {
          if (!featureLayer.IsEditable)
            featureLayer.SetEditable(true);

          if (!featureLayer.IsSnappable)
            featureLayer.SetSnappable(true);
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LayerDocument
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.#ctor(ArcGIS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Mapping.LayerDocument.Save(System.String)
      #region Create a Lyrx file
      {
        LayerDocument layerDocument = new LayerDocument(layer);
        //Note: Run within QueuedTask
        layerDocument.Save(@"c:\Data\MyLayerDocument.lyrx");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SelectionCount
      #region Count the features selected on a layer
      {
        var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
        var noFeaturesSelected = lyr.SelectionCount;
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer
      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.FeatureTable
      // cref: ArcGIS.Core.CIM.CIMFeatureTable
      // cref: ArcGIS.Core.CIM.CIMDisplayTable.DisplayField
      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
      #region Access the display field for a layer
      {
        // get the CIM definition from the layer
        // Note: needs to be called on the MCT
        var cimFeatureDefinition = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
        // get the view of the source table underlying the layer
        var cimDisplayTable = cimFeatureDefinition.FeatureTable;
        // this field is used as the 'label' to represent the row
        var displayField = cimDisplayTable.DisplayField;
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsLabelVisible
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetLabelVisibility(System.Boolean)
      #region Enable labeling on a layer
      {
        //Note: needs to be called on the MCT
        // toggle the label visibility
        featureLayer.SetLabelVisibility(!featureLayer.IsLabelVisible);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.ElevationTypeDefinition
      //cref: ArcGIS.Desktop.Mapping.Layer.GetElevationTypeDefinition
      //cref: ArcGIS.Desktop.Mapping.Layer.SetElevationTypeDefinition
      //cref: ArcGIS.Desktop.Mapping.Layer.CanSetElevationTypeDefinition(ArcGIS.Desktop.Mapping.ElevationTypeDefinition)
      //cref: ArcGIS.Desktop.Mapping.Layer.SetElevationTypeDefinition(ArcGIS.Desktop.Mapping.ElevationTypeDefinition)
      //cref: ArcGIS.Desktop.Mapping.LayerElevationType
      //cref: ArcGIS.Desktop.Mapping.ElevationTypeDefinition.CartographicOffset
      //cref: ArcGIS.Desktop.Mapping.ElevationTypeDefinition.VerticalExaggeration

      #region Set Elevation Mode for a layer
      {
        //Note: needs to be called on the MCT
        ElevationTypeDefinition elevationTypeDefinition = featureLayer.GetElevationTypeDefinition();
        elevationTypeDefinition.ElevationType = LayerElevationType.OnGround;
        //elevationTypeDefinition.ElevationType = LayerElevationType.RelativeToGround;
        //elevationTypeDefinition.ElevationType = LayerElevationType.RelativeToScene;
        //elevationTypeDefinition.ElevationType = LayerElevationType.AtAbsoluteHeight;
        //..so on.
        //Optional: Specify the cartographic offset
        elevationTypeDefinition.CartographicOffset = 1000;
        //Optional: Specify the VerticalExaggeration
        elevationTypeDefinition.VerticalExaggeration = 2;
        if (featureLayer.CanSetElevationTypeDefinition(elevationTypeDefinition))
          featureLayer.SetElevationTypeDefinition(elevationTypeDefinition);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.IsFlattened
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition(ArcGIS.Core.CIM.CIMBaseLayer)
      #region Move a layer in the 2D group to the 3D Group in a Local Scene
      {
        //The layer in the 2D group to move to the 3D Group in a Local Scene
        //Get the layer's definition
        //Note: needs to be called on the MCT
        var lyrDefn = featureLayer.GetDefinition() as CIMBasicFeatureLayer;
        //setting this property moves the layer to 3D group in a scene
        lyrDefn.IsFlattened = false;
        //Set the definition back to the layer
        featureLayer.SetDefinition(lyrDefn);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapMember.GetDataConnection()
      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection
      // cref: ArcGIS.Core.CIM.CIMStandardDataConnection.WorkspaceConnectionString
      // cref: ArcGIS.Desktop.Mapping.MapMember.SetDataConnection(ArcGIS.Core.CIM.CIMDataConnection, System.Boolean)
      #region Reset the URL of a feature service layer 
      {
        CIMStandardDataConnection dataConnection = featureLayer.GetDataConnection() as CIMStandardDataConnection;
        dataConnection.WorkspaceConnectionString = @"DATABASE=C:\Data\USNationalParks\USNationalParks.gdb";
        //Note: needs to be called on the MCT
        featureLayer.SetDataConnection(dataConnection);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Layer.FindAndReplaceWorkspacePath(System.String, System.String, System.Boolean)
      // cref: ArcGIS.Core.Data.Datastore.GetConnectionString()
      #region Change the underlying data source of a feature layer - same workspace type
      {
        //This is the existing layer for which we want to switch the underlying datasource
        var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
        //Note: needs to be called on the MCT
        var connectionStringToReplace = lyr.GetFeatureClass().GetDatastore().GetConnectionString();
        string databaseConnectionPath = @"Path to the .sde connection file to replace with";
        //If the new SDE connection did not have a dataset with the same name as in the feature layer,
        //pass false for the validate parameter of the FindAndReplaceWorkspacePath method to achieve this. 
        //If validate is true and the SDE did not have a dataset with the same name, 
        //FindAndReplaceWorkspacePath will return failure
        lyr.FindAndReplaceWorkspacePath(connectionStringToReplace, databaseConnectionPath, true);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.ChangeVersion(ArcGIS.Core.Data.VersionBase,ArcGIS.Core.Data.VersionBase)
      #region Change Geodatabase Version of layers off a specified version in a map
      {
        //Note: needs to be called on the MCT
        //Getting the current version name from the first feature layer of the map
        Datastore dataStore = featureLayer.GetFeatureClass().GetDatastore();  //getting datasource
        Geodatabase geodatabase = dataStore as Geodatabase; //casting to Geodatabase
        if (geodatabase == null)
          return;

        VersionManager versionManager = geodatabase.GetVersionManager();
        ArcGIS.Core.Data.Version currentVersion = versionManager.GetCurrentVersion();
        string currentVersionName = currentVersion.GetName();

        //Getting all available versions except the current one
        List<ArcGIS.Core.Data.Version> versions = [];
        foreach (string versionName in versionManager.GetVersionNames())
        {
          if (versionName != currentVersionName)
            break;
          versions.Add(versionManager.GetVersion(versionName));
        }

        //Assuming there is at least one other version we pick the first one from the list
        ArcGIS.Core.Data.Version toVersion = versions.FirstOrDefault();
        if (toVersion != null)
        {
          //Changing version
          MapView.Active.Map.ChangeVersion(currentVersion, toVersion);
        }
      }
      #endregion
      // cref: ArcGIS.Core.Data.QueryFilter
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.Search(ArcGIS.Core.Data.QueryFilter, ArcGIS.Desktop.Mapping.TimeRange, ArcGIS.Desktop.Mapping.RangeExtent, ArcGIS.Core.CIM.CIMFloorFilterSettings)
      #region Querying a feature layer
      {
        QueryFilter qf = new QueryFilter()
        {
          WhereClause = "Class = 'city'"
        };
        int count = 0;
        using (RowCursor rows = selectedLayer.Search(qf)) //execute
        {
          //Looping through to count
          while (rows.MoveNext()) count++;
        }
        System.Diagnostics.Debug.WriteLine(String.Format(
           "Total features that matched the search criteria: {0}", count));
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.#ctor
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetFeatureOutline(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.FeatureOutlineType)
      // cref: ArcGIS.Desktop.Mapping.FeatureOutlineType
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.InsertDefinitionQuery(ArcGIS.Desktop.Mapping.DefinitionQuery,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetActiveDefinitionQuery(System.String)
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.CanSetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.SetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
      #region Querying a feature layer with a spatial filter
      {
        var layers = map.GetLayersAsFlattenedList().OfType<FeatureLayer>();
        var layerToQuery = layers.FirstOrDefault(f => f.Name == "USNationalParks");
        if (layerToQuery == null) return;

        string whereClause = "RecreationVisitsTotal > 1000000"; //More than million visitors a year

        var spatialDefnLayer = layers.FirstOrDefault(f => f.Name == "AllUSStates");
        if (spatialDefnLayer == null) return;
        try
        {
          if (MapView.Active == null) return;
          //Note: needs to be called on the MCT
          // Set the spatial filter geometry
          //Get the geometry from the selected features in the feature layer
          var spatialClauseGeom = spatialDefnLayer.GetFeatureOutline(MapView.Active, FeatureOutlineType.Selected);
          //Create the definition query
          DefinitionQuery definitionQuery = new DefinitionQuery
          {
            WhereClause = whereClause,
            Name = $"{layerToQuery.Name}"
          };
          //Setting the spatial filter to the Definition Query
          if (definitionQuery.CanSetFilterGeometry(spatialClauseGeom))
          {
            definitionQuery.SetFilterGeometry(spatialClauseGeom);
          }

          layerToQuery.InsertDefinitionQuery(definitionQuery);
          layerToQuery.SetActiveDefinitionQuery(definitionQuery.Name);
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine($"Error querying feature layer: {ex.Message}");
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.#ctor
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.InsertDefinitionQuery(ArcGIS.Desktop.Mapping.DefinitionQuery,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetActiveDefinitionQuery(System.String)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetDefinitionQuery(System.String)
      #region Apply A Definition Query Filter to a Feature Layer
      {
        var us_parks = map.GetLayersAsFlattenedList()
      .OfType<FeatureLayer>().First(l => l.Name == "USNationalParks");
        //Note: needs to be called on the MCT
        var def_query = new DefinitionQuery("CaliforniaParks",
                          "STATE_ABBR = 'CA'");

        us_parks.InsertDefinitionQuery(def_query);
        us_parks.SetDefinitionQuery("STATE_ABBR = 'CA'");

        //Set it active
        us_parks.SetActiveDefinitionQuery(def_query.Name);

        //or....also - set it active when it is inserted
        //us_parks.InsertDefinitionQuery(def_query, true);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.#ctor
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.InsertDefinitionQuery(ArcGIS.Desktop.Mapping.DefinitionQuery,System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetActiveDefinitionQuery(System.String)
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.CanSetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.SetFilterGeometry(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetFeatureOutline(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.FeatureOutlineType)
      // cref: ArcGIS.Desktop.Mapping.FeatureOutlineType
      #region Apply A Definition Query Filter With a Filter Geometry to a Feature Layer
      {
        var greatLakes = map.GetLayersAsFlattenedList()
              .OfType<FeatureLayer>().First(l => l.Name == "Great Lakes");
        var usa_states = map.GetLayersAsFlattenedList()
        .OfType<FeatureLayer>().First(l => l.Name == "US_States");
        //name must be unique
        var def_query = new DefinitionQuery("GreatLakes",
                              "NAME in ('Huron','Michigan','Erie')");

        //create a filter geometry - in this example we will use the outline geometry
        //of all visible features from a us states layer...the filter geometry will be
        //intersected with the layer feature geometry when added to the def query
        var filter_geom = usa_states.GetFeatureOutline(mapView, FeatureOutlineType.Visible);
        //other options...
        //var filter_geom = usa_states.GetFeatureOutline(mapView, FeatureOutlineType.All);
        //var filter_geom = usa_states.GetFeatureOutline(mapView, FeatureOutlineType.Selected);

        //Geometry must have a valid SR and be point, multi-point, line, or poly
        //Note: needs to be called on the MCT
        if (def_query.CanSetFilterGeometry(filter_geom))
        {
          def_query.SetFilterGeometry(filter_geom);
        }

        //Apply the def query
        greatLakes.InsertDefinitionQuery(def_query);
        //Set it active
        greatLakes.SetActiveDefinitionQuery(def_query.Name);

        //or....also - set it active when it is inserted
        //greatLakes.InsertDefinitionQuery(def_query, true);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.SetDefinitionQuery(System.String)
      #region Apply A Definition Query Filter to a Feature Layer Option 2
      {
        var us_parks = map.GetLayersAsFlattenedList()
              .OfType<FeatureLayer>().First(l => l.Name == "USNationalParks");
        //inserts a new definition query and makes it active
        //it will be assigned a unique name
        us_parks.SetDefinitionQuery("STATE_ABBR = 'CA'");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.DefinitionQueries
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.Name
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.WhereClause
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.GeometryUri
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.SpatialReference
      // cref: ArcGIS.Desktop.Mapping.DefinitionQuery.GetFilterGeometry()
      #region Retrieve the Definition Query Filters for a Feature Layer
      {
        var us_parks = map.GetLayersAsFlattenedList()
              .OfType<FeatureLayer>().First(l => l.Name == "USNationalParks");

        //enumerate the layer's definition queries - if any
        var def_queries = us_parks.DefinitionQueries;
        foreach (var def_qry in def_queries)
        {
          var geom_uri = def_qry.GeometryUri ?? "null";
          var sr_wkid = def_qry.SpatialReference?.Wkid.ToString() ?? "null";
          var geom = def_qry.GetFilterGeometry();
          var geom_type = geom?.GeometryType.ToString() ?? "null";

          System.Diagnostics.Debug.WriteLine($" def_qry.Name: {def_qry.Name}");
          System.Diagnostics.Debug.WriteLine($" def_qry.WhereClause: {def_qry.WhereClause}");
          System.Diagnostics.Debug.WriteLine($" def_qry.GeometryUri: {geom_uri}");
          System.Diagnostics.Debug.WriteLine($" def_qry.SpatialReference: {sr_wkid}");
          System.Diagnostics.Debug.WriteLine($" def_qry.FilterGeometry: {geom_type}");
          System.Diagnostics.Debug.WriteLine("");
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetFeatureOutline(ArcGIS.Desktop.Mapping.MapView,ArcGIS.Desktop.Mapping.FeatureOutlineType)
      // cref: ArcGIS.Desktop.Mapping.FeatureOutlineType
      #region Get Feature Outlines from a Feature Layer
      {
        var greatLakes = map.GetLayersAsFlattenedList()
              .OfType<FeatureLayer>().First(l => l.Name == "Great Lakes");
        var michigan = map.GetBookmarks().First(b => b.Name == "Michigan");

        //get all features - multiple feature geometries are always returned as a
        //single multi-part
        var all_features_outline = greatLakes.GetFeatureOutline(mapView, FeatureOutlineType.All);

        //or get just the outline of selected features
        var qry = new QueryFilter()
        {
          SubFields = "*",
          WhereClause = "NAME in ('Huron','Michigan','Erie')"
        };
        greatLakes.Select(qry);
        var sel_features_outline = greatLakes.GetFeatureOutline(
            mapView, FeatureOutlineType.Selected);
        greatLakes.ClearSelection();

        //or just the visible features
        mapView?.ZoomTo(michigan);
        var visible_features_outline = greatLakes.GetFeatureOutline(
            mapView, FeatureOutlineType.Visible);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMRotationVisualVariable
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.VisualVariables
      #region Get the attribute rotation field of a layer
      {
        //Note: needs to be called on the MCT
        var cimRenderer = featureLayer.GetRenderer() as CIMUniqueValueRenderer;
        //Gets the visual variables from the renderer
        var cimRotationVariable = cimRenderer.VisualVariables.OfType<CIMRotationVisualVariable>().FirstOrDefault();
        //Gets the visual variable info for the z dimension
        var rotationInfoZ = cimRotationVariable.VisualVariableInfoZ;
        //The Arcade expression used to evaluate and return the field name for the rotation
        var rotationExpression = rotationInfoZ.ValueExpressionInfo.Expression; // this expression stores the field name  
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMRotationVisualVariable
      // cref: ArcGIS.Core.CIM.CIMSimpleRenderer.VisualVariables
      #region Find connected attribute field for rotation
      {
        // get the CIM renderer from the layer
        // Note: needs to be called on the MCT
        var cimRenderer = featureLayer.GetRenderer() as ArcGIS.Core.CIM.CIMSimpleRenderer;
        // get the collection of connected attributes for rotation
        var cimRotationVariable = cimRenderer.VisualVariables.OfType<ArcGIS.Core.CIM.CIMRotationVisualVariable>().FirstOrDefault();
        // the z direction is describing the heading rotation
        var rotationInfoZ = cimRotationVariable.VisualVariableInfoZ;
        var rotationExpression = rotationInfoZ.Expression; // this expression stores the field name  
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.ScaleSymbols
      #region Toggle "Scale layer symbols when reference scale is set"
      {
        // get the CIM layer definition
        // Note: needs to be called on the MCT
        var cimFeatureLayer = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMFeatureLayer;
        // turn on the option to scale the symbols in this layer based in the map's reference scale
        cimFeatureLayer.ScaleSymbols = true;
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.LayerCacheType
      // cref: ArcGIS.Desktop.Mapping.Layer.SetCacheOptions(ArcGIS.Desktop.Mapping.LayerCacheType)
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDisplayCacheMaxAge(System.TimeSpan)
      #region Set the layer cache
      {
        // change from the default 5 min to 2 min
        //Note: needs to be called on the MCT
        featureLayer.SetDisplayCacheMaxAge(TimeSpan.FromMinutes(2));
      }
      #endregion

      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.UseSelectionSymbol
      // cref: ArcGIS.Core.CIM.CIMBasicFeatureLayer.SelectionColor
      // cref: ArcGIS.Desktop.Mapping.MapView.SelectFeatures(ArcGIS.Core.Geometry.Geometry, ArcGIS.Desktop.Mapping.SelectionCombinationMethod, System.Boolean, System.Boolean)
      #region Change the layer selection color
      {
        // get the CIM definition of the layer
        // Note: needs to be called on the MCT
        var layerDef = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
        // disable the default symbol
        layerDef.UseSelectionSymbol = false;
        // assign a new color
        layerDef.SelectionColor = ColorFactory.Instance.RedRGB;
        // apply the definition to the layer
        featureLayer.SetDefinition(layerDef);

        if (!featureLayer.IsVisible)
          featureLayer.SetVisibility(true);
        //Do a selection
        MapView.Active.SelectFeatures(MapView.Active.Extent);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveLayers(System.Collections.Generic.IEnumerable<ArcGIS.Desktop.Mapping.Layer>)
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveLayer(ArcGIS.Desktop.Mapping.Layer)
      #region Removes all layers that are unchecked
      {
        //Get the group layers first
        IReadOnlyList<GroupLayer> groupLayers = map.Layers.OfType<GroupLayer>().ToList();
        //Iterate and remove the layers within the group layers that are unchecked.
        //Note: Run within a QueuedTask
        foreach (var groupLyr in groupLayers)
        {
          //Get layers that not visible within the group
          var layers = groupLyr.Layers.Where(l => l.IsVisible == false).ToList();
          //Remove all the layers that are not visible within the group
          map.RemoveLayers(layers);
        }

        //Group Layers that are empty and are unchecked
        foreach (var group in groupLayers)
        {
          if (group.Layers.Count == 0 && group.IsVisible == false) //No layers in the group
          {
            //remove the group
            map.RemoveLayer(group);
          }
        }

        //Get Layers that are NOT Group layers and are unchecked
        var notAGroupAndUnCheckedLayers = map.Layers.Where(l => !(l is GroupLayer) && l.IsVisible == false).ToList();
        //Remove all the non group layers that are not visible
        map.RemoveLayers(notAGroupAndUnCheckedLayers);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveLayer(ArcGIS.Desktop.Mapping.Layer)
      // cref: ArcGIS.Desktop.Mapping.GroupLayer
      // cref: ArcGIS.Desktop.Mapping.CompositeLayer.Layers
      #region Remove empty groups
      {
        //Get the group layers
        IReadOnlyList<GroupLayer> groupLayers = map.Layers.OfType<GroupLayer>().ToList();
        //Note: Run within a QueuedTask
        foreach (var group in groupLayers)
        {
          if (group.Layers.Count == 0) //No layers in the group
          {
            //remove the group
            map.RemoveLayer(group);
          }
        }
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMMap.GeneralPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.Abbreviation
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.Text
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.MaplexAbbreviationType
      // cref: ArcGIS.Core.CIM.MaplexAbbreviationType
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionary
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionary.Name
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionary.MaplexDictionary
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.Dictionaries
      #region Create Abbreviation Dictionary in the Map Definition to a layer
      {
        //Get the map's definition
        //Note: needs to be called on the MCT
        var mapDefn = MapView.Active.Map.GetDefinition();
        //Get the Map's Maplex labelling engine properties
        var mapDefnPlacementProps = mapDefn.GeneralPlacementProperties as CIMMaplexGeneralPlacementProperties;

        //Define the abbreviations we need in an array            
        List<CIMMaplexDictionaryEntry> abbreviationDictionary = new List<CIMMaplexDictionaryEntry>
            {
                new CIMMaplexDictionaryEntry {
                Abbreviation = "Hts",
                Text = "Heights",
                MaplexAbbreviationType = MaplexAbbreviationType.Ending

             },
                new CIMMaplexDictionaryEntry
                {
                    Abbreviation = "Ct",
                    Text = "Text",
                    MaplexAbbreviationType = MaplexAbbreviationType.Ending

                }
                //etc
            };
        //The Maplex Dictionary - can hold multiple Abbreviation collections
        var maplexDictionary = new List<CIMMaplexDictionary>
            {
                new CIMMaplexDictionary {
                    Name = "NameEndingsAbbreviations",
                    MaplexDictionary = abbreviationDictionary.ToArray()
                }

            };
        //Set the Maplex Label Engine Dictionary property to the Maplex Dictionary collection created above.
        mapDefnPlacementProps.Dictionaries = maplexDictionary.ToArray();
        //Set the Map definition 
        MapView.Active.Map.SetDefinition(mapDefn);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMMap.GeneralPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.Abbreviation
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.Text
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionaryEntry.MaplexAbbreviationType
      // cref: ArcGIS.Core.CIM.MaplexAbbreviationType
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionary
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionary.Name
      // cref: ArcGIS.Core.CIM.CIMMaplexDictionary.MaplexDictionary
      // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.Dictionaries
      #region Apply Abbreviation Dictionary in the Map Definition to a layer
      {
        //Creates Abbreviation dictionary and adds to Map Definition                                
        //Refer to the " Create Abbreviation Dictionary in the Map Definition to a layer" snippet above
        //Get the layer's definition
        //Note: needs to be called on the MCT
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Modify label Placement props to use abbreviation dictionary 
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        //Get the dictionary from the map definition
        theLabelClass.MaplexLabelPlacementProperties.DictionaryName = "NameEndingsAbbreviations";
        theLabelClass.MaplexLabelPlacementProperties.CanAbbreviateLabel = true;
        theLabelClass.MaplexLabelPlacementProperties.CanStackLabel = false;
        //Set the labelClasses back
        lyrDefn.LabelClasses = listLabelClasses.ToArray();
        //set the layer's definition
        featureLayer.SetDefinition(lyrDefn);
      }
      #endregion
      #region ProSnippet Group: Symbol Layer Drawing (SLD)
      #endregion
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CanAddSymbolLayerDrawing()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.AddSymbolLayerDrawing()
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.CanAddSymbolLayerDrawing()
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.AddSymbolLayerDrawing()
      #region Add SLD
      {
        //check if it can be added to the layer
        //Note: Run within a QueuedTask
        if (featureLayer.CanAddSymbolLayerDrawing())
          featureLayer.AddSymbolLayerDrawing();

        //ditto for a group layer...must have at least
        //one child feature layer that can participate
        if (groupLayer.CanAddSymbolLayerDrawing())
          groupLayer.AddSymbolLayerDrawing();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.HasSymbolLayerDrawingAdded()
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.HasSymbolLayerDrawingAdded()
      #region Determine if a layer has SLD added
      {
        //SLD can be added to feature layers and group layers
        //For a group layer, SLD controls all child feature layers
        //that are participating in the SLD

        //var featLayer = ...;//retrieve the feature layer
        //var groupLayer = ...;//retrieve the group layer
        //Check if the layer has SLD added -returns a tuple
        //Note: Run within a QueuedTask
        var tuple = featureLayer.HasSymbolLayerDrawingAdded();
        if (tuple.addedOnLayer)
        {
          //SLD is added on the layer
        }
        else if (tuple.addedOnParent)
        {
          //SLD is added on the parent (group layer) - 
          //check parent...this can be recursive
          var parentLayer = GetParentLayerWithSLD(featureLayer.Parent as GroupLayer);

          //Recursively get the parent with SLD
          GroupLayer GetParentLayerWithSLD(GroupLayer groupLayer)
          {
            if (groupLayer == null)
              return null;
            //Must be on QueuedTask
            var sld_added = groupLayer.HasSymbolLayerDrawingAdded();
            if (sld_added.addedOnLayer)
              return groupLayer;
            else if (sld_added.addedOnParent)
              return GetParentLayerWithSLD(groupLayer.Parent as GroupLayer);
            return null;
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.HasSymbolLayerDrawingAdded()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetUseSymbolLayerDrawing()
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetUseSymbolLayerDrawing(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.HasSymbolLayerDrawingAdded()
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.GetUseSymbolLayerDrawing()
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.SetUseSymbolLayerDrawing(System.Boolean)
      #region Enable/Disable SLD
      {
        //A layer may have SLD added but is not using it
        //HasSymbolLayerDrawingAdded returns a tuple - to check
        //the layer has SLD (not its parent) check addedOnLayer
        if (featureLayer.HasSymbolLayerDrawingAdded().addedOnLayer)
        {
          //the layer has SLD but is the layer currently using it?
          //GetUseSymbolLayerDrawing returns a tuple - useOnLayer for 
          //the layer (and useOnParent for the parent layer)
          if (!featureLayer.GetUseSymbolLayerDrawing().useOnLayer)
          {
            //enable it
            featureLayer.SetUseSymbolLayerDrawing(true);
          }
        }

        //Enable/Disable SLD on a layer parent
        if (featureLayer.HasSymbolLayerDrawingAdded().addedOnParent)
        {
          //check parent...this can be recursive
          var parent = GetParentLayerWithSLD(featureLayer.Parent as GroupLayer);
          if (parent.GetUseSymbolLayerDrawing().useOnLayer)
            parent.SetUseSymbolLayerDrawing(true);
        }

        //Recursively get the parent with SLD
        GroupLayer GetParentLayerWithSLD(GroupLayer groupLayer)
        {
          if (groupLayer == null)
            return null;
          //Must be on QueuedTask
          var sld_added = groupLayer.HasSymbolLayerDrawingAdded();
          if (sld_added.addedOnLayer)
            return groupLayer;
          else if (sld_added.addedOnParent)
            return GetParentLayerWithSLD(groupLayer.Parent as GroupLayer);
          return null;
        }
      }
      #endregion

      #region ProSnippet Group: Elevation Surface Layers
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateScene(System.String, System.Uri, ArcGIS.Core.CIM.MapViewingMode, ArcGIS.Desktop.Mapping.Basemap)
      #region Create a scene with a ground surface layer
      {
        //Note: needs to be called on the MCT
        var groundSourceUri = new Uri("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer");
        var scene = MapFactory.Instance.CreateScene("My scene", groundSourceUri, MapViewingMode.SceneGlobal);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ElevationSurfaceLayer
      // cref: ArcGIS.Desktop.Mapping.ElevationLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.ElevationLayerCreationParams.#ctor(ArcGIS.Core.CIM.CIMDataConnection)
      // cref: ArcGIS.Desktop.Mapping.Map.GetGroundElevationSurfaceLayer
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams, ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create a New Elevation Surface
      {
        //Define a ServiceConnection to use for the new Elevation surface
        var serverConnection = new CIMInternetServerConnection
        {
          Anonymous = true,
          HideUserProperty = true,
          URL = "https://elevation.arcgis.com/arcgis/services"
        };
        CIMAGSServiceConnection serviceConnection = new CIMAGSServiceConnection
        {
          ObjectName = "WorldElevation/Terrain",
          ObjectType = "ImageServer",
          URL = "https://elevation.arcgis.com/arcgis/services/WorldElevation/Terrain/ImageServer",
          ServerConnection = serverConnection
        };
        //Defines a new elevation source set to the CIMAGSServiceConnection defined above
        //Get the elevation surfaces defined in the map
        var listOfElevationSurfaces = map.GetElevationSurfaceLayers();
        //Add the new elevation surface 
        var elevationLyrCreationParams = new ElevationLayerCreationParams(serviceConnection);
        //Note: needs to be called on the MCT
        var elevationSurface = LayerFactory.Instance.CreateLayer<ElevationSurfaceLayer>(
            elevationLyrCreationParams, map);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMLayerElevationSurface
      // cref: ArcGIS.Core.CIM.CIMBaseLayer.LayerElevation
      #region Set a custom elevation surface to a Z-Aware layer
      {
        //Note: needs to be called on the MCT
        //Define the custom elevation surface to use
        var layerElevationSurface = new CIMLayerElevationSurface
        {
          ElevationSurfaceLayerURI = "https://elevation3d.arcgis.com/arcgis/services/WorldElevation3D/Terrain3D/ImageServer"
        };
        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMBasicFeatureLayer;
        //Set the layer's elevation surface
        lyrDefn.LayerElevation = layerElevationSurface;
        //Set the layer's definition
        featureLayer.SetDefinition(lyrDefn);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ElevationLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.ElevationLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams, ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Add an elevation source to an existing elevation surface layer
      {
        ElevationSurfaceLayer surfaceLayer = null;
        // surfaceLayer could also be the ground layer
        string uri = "https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer";
        var createParams = new ElevationLayerCreationParams(new Uri(uri));
        createParams.Name = "Terrain 3D";
        //Note: needs to be called on the MCT
        var eleSourceLayer = LayerFactory.Instance.CreateLayer<Layer>(createParams, surfaceLayer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ElevationSurfaceLayer
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationSurfaceLayers
      // cref: ArcGIS.Desktop.Mapping.Map.GetGroundElevationSurfaceLayer
      #region Get the elevation surface layers and elevation source layers from a map
      {
        // retrieve the elevation surface layers in the map including the Ground
        var surfaceLayers = map.GetElevationSurfaceLayers();

        // retrieve the single ground elevation surface layer in the map
        var groundSurfaceLayer = map.GetGroundElevationSurfaceLayer();

        // determine the number of elevation sources in the ground elevation surface layer
        int numberGroundSources = groundSurfaceLayer.Layers.Count;
        // get the first elevation source layer from the ground elevation surface layer
        var groundSourceLayer = groundSurfaceLayer.Layers.FirstOrDefault();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ElevationSurfaceLayer
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationSurfaceLayers
      // cref: ArcGIS.Desktop.Mapping.Map.FindElevationSurfaceLayer(System.String)
      #region Find an elevation surface layer
      {
        // retrieve the elevation surface layers in the map 
        var surfaceLayers = map.GetElevationSurfaceLayers();
        // find a specific elevation surface layer by its URI
        var surfaceLayer = surfaceLayers.FirstOrDefault(l => l.Name == "Surface2");
        // or use the FindElevationSurfaceLayer method, passing the layer URI
        surfaceLayer = map.FindElevationSurfaceLayer("layerUri");
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.ClearElevationSurfaceLayers
      #region Remove elevation surface layers
      {
        //Ground will not be removed
        map.ClearElevationSurfaceLayers();
        ElevationSurfaceLayer surfaceLayer = map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "Surface2");
        //Cannot remove ground
        map.RemoveLayer(surfaceLayer);
        //Ground will not be removed
        map.RemoveLayers(map.GetElevationSurfaceLayers());
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetZsFromSurfaceAsync(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult
      #region Get Z values from the default ground surface
      {
        //Note: This method must be called on the MCT
        // Get the center of the map's full extent
        MapPoint mapPoint = MapView.Active.Map.CalculateFullExtent().Center;
        //Pass any Geometry type to GetZsFromSurfaceAsync
        var surfaceZResult = await MapView.Active.Map.GetZsFromSurfaceAsync(mapPoint);
        if (surfaceZResult.Status == SurfaceZsResultStatus.Ok)
        {
          // cast to a mapPoint
          var mapPointZ = surfaceZResult.Geometry as MapPoint;
          var z = mapPointZ.Z;
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetZsFromSurfaceAsync(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult
      #region Get Z values from a specific surface
      {
        var eleLayer = MapView.Active.Map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "TIN");
        //Pass any Geometry type to GetZsFromSurfaceAsync
        //Note: This method must be called on the MCT
        //Ensure the feature layer is a line layer and is Z aware
        var selectedFeatures = featureLayer.GetSelection();
        var rowCursor = selectedFeatures.Search();
        while (rowCursor.MoveNext())
        {
          var feature = rowCursor.Current as Feature;
          var polyline = feature.GetShape() as Polyline;
          var zResult = await MapView.Active.Map.GetZsFromSurfaceAsync(polyline, eleLayer);
          if (zResult.Status == SurfaceZsResultStatus.Ok)
          {
            var polylineZ = zResult.Geometry as Polyline;
            // process the polylineZ
            // For example, you can iterate through the points and access their Z values
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Layer.CanGetZs()
      // cref: ArcGIS.Desktop.Mapping.Layer.GetZs(ArcGIS.Core.Geometry)
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResult
      #region Get Z values from a layer
      {
        TinLayer tinLayer = map.GetLayersAsFlattenedList().OfType<TinLayer>().FirstOrDefault();
        //Note: This method must be called on the MCT
        MapPoint mapPoint = MapView.Active.Map.CalculateFullExtent().Center; //Any Map Point
        Polyline polyline = PolylineBuilderEx.CreatePolyline(mapView.Extent); //Any Polyline

        if (tinLayer.CanGetZs())
        {
          // get z value for a mapPoint
          var zResult = tinLayer.GetZs(mapPoint);
          if (zResult.Status == SurfaceZsResultStatus.Ok)
          {
            // cast to a mapPoint
            var mapPointZ = zResult.Geometry as MapPoint;
            var z = mapPointZ.Z;
          }

          // get z values for a polyline
          zResult = tinLayer.GetZs(polyline);
          if (zResult.Status == SurfaceZsResultStatus.Ok)
          {
            // cast to a mapPoint
            var polylineZ = zResult.Geometry as Polyline;
          }
        }
      }
      #endregion

      #region ProSnippet Group: Raster Layers
      #endregion

      // cref: ArcGIS.Desktop.Mapping.RasterLayer
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer(System.Uri, ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
      #region Create a raster layer
      {
        string urlRatser = @"C:\Images\Italy.tif";
        await QueuedTask.Run(() =>
        {
          // Create a raster layer using a path to an image.
          // Note: You can create a raster layer from a url, project item, or data connection.
          //Note: needs to be called on the MCT
          var rasterLayerToCreate = LayerFactory.Instance.CreateLayer(new Uri(urlRatser), map) as RasterLayer;
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.Brightness
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.Contrast
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.ResamplingType
      // cref: ArcGIS.Core.CIM.RasterResamplingType
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Update the raster colorizer on a raster layer
      {
        // Get the colorizer from the raster layer.
        //Note: needs to be called on the QueuedTask
        CIMRasterColorizer rasterColorizer = rasterLayer.GetColorizer();
        // Update raster colorizer properties.
        rasterColorizer.Brightness = 10;
        rasterColorizer.Contrast = -5;
        rasterColorizer.ResamplingType = RasterResamplingType.NearestNeighbor;
        // Update the raster layer with the changed colorizer.
        rasterLayer.SetColorizer(rasterColorizer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
      // cref: ArcGIS.Core.CIM.CIMRasterRGBColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterRGBColorizer.StretchType
      // cref: ArcGIS.Core.CIM.RasterStretchType
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Update the RGB colorizer on a raster layer
      {
        // Get the colorizer from the raster layer.
        //Note: needs to be called on the QueuedTask
        CIMRasterColorizer rColorizer = rasterLayer.GetColorizer();
        // Check if the colorizer is an RGB colorizer.
        if (rColorizer is CIMRasterRGBColorizer rasterRGBColorizer)
        {
          // Update RGB colorizer properties.
          rasterRGBColorizer.StretchType = RasterStretchType.ESRI;
          // Update the raster layer with the changed colorizer.
          rasterLayer.SetColorizer(rasterRGBColorizer);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      #region Check if a certain colorizer can be applied to a raster layer 
      {
        // Get the list of colorizers that can be applied to the raster layer.
        //Note: needs to be called on the QueuedTask
        IEnumerable<RasterColorizerType> applicableColorizerList = rasterLayer.GetApplicableColorizers();
        // Check if the RGB colorizer is part of the list.
        bool isTrue_ContainTheColorizerType = applicableColorizerList.Contains(RasterColorizerType.RGBColorizer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.CreateColorizerAsync(ArcGIS.Desktop.Mapping.RasterColorizerDefinition)
      // cref: ArcGIS.core.CIM.CIMRasterStretchColorizer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Create a new colorizer based on a default colorizer definition and apply it to the raster layer 
      {
        // Check if the Stretch colorizer can be applied to the raster layer.
        //Note: needs to be called on the QueuedTask
        if (rasterLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition using the default constructor.
          StretchColorizerDefinition stretchColorizerDef_default = new StretchColorizerDefinition();
          // Create a new Stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_default =
          await rasterLayer.CreateColorizerAsync(stretchColorizerDef_default) as CIMRasterStretchColorizer;
          // Set the new colorizer on the raster layer.
          rasterLayer.SetColorizer(newStretchColorizer_default);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor(System.Int32, ArcGIS.Core.CIM.RasterStretchType, System.Double, ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.CreateColorizerAsync(ArcGIS.Desktop.Mapping.RasterColorizerDefinition)
      // cref: ArcGIS.core.CIM.CIMRasterStretchColorizer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Create a new colorizer based on a custom colorizer definition and apply it to the raster layer 
      {
        // Check if the Stretch colorizer can be applied to the raster layer.
        //Get a color ramp from a style
        StyleProjectItem ArcGISColorsStyleItem =
              Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS Colors");
        var colorRamps = ArcGISColorsStyleItem.SearchColorRamps("Heat Map 4 - Semitransparent");
        CIMColorRamp colorRampToUse = colorRampList[0].ColorRamp;

        //Note: needs to be called on the QueuedTask
        if (rasterLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition specifying parameters 
          // for band index, stretch type, gamma and color ramp.
          StretchColorizerDefinition stretchColorizerDef_custom =
      new StretchColorizerDefinition(1, RasterStretchType.ESRI, 2, colorRamp);
          // Create a new stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_custom =
      await rasterLayer.CreateColorizerAsync(stretchColorizerDef_custom) as CIMRasterStretchColorizer;
          // Set the new colorizer on the raster layer.
          rasterLayer.SetColorizer(newStretchColorizer_custom);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.RasterLayer
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams.ColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams, ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create a raster layer with a new colorizer definition
      {
        // Create a new stretch colorizer definition using default constructor.
        StretchColorizerDefinition stretchColorizerDef = new StretchColorizerDefinition();
        // Create a raster layer creation parameters object with the raster file path.
        var rasterLayerCreationParams = new RasterLayerCreationParams(new Uri("rasterPath"))
        {
          ColorizerDefinition = stretchColorizerDef,
          Name = "rasterLayerName",
          MapMemberIndex = 0
        };

        // Create a raster layer using the colorizer definition created above.
        // Note: You can create a raster layer from a url, project item, or data connection.
        //Note: Run within a QueuedTask
        RasterLayer rasterLayerfromURL =
      LayerFactory.Instance.CreateLayer<RasterLayer>(rasterLayerCreationParams, map);

      }
      #endregion


      #region ProSnippet Group: Mosaic Layers
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MosaicLayer
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer(System.Uri, ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
      #region Create a mosaic layer
      {
        //Path to mosaic dataset
        string urlItaly = @"C:\Images\countries.gdb\Italy";
        //Note: Run within a QueuedTask
        // Create a mosaic layer using a path to a mosaic dataset.
        // Note: You can create a mosaic layer from a url, project item, or data connection.
        mosaicLayer = LayerFactory.Instance.CreateLayer(new Uri(urlItaly), map) as MosaicLayer;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageMosaicSubLayer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.Brightness
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.Contrast
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.ResamplingType
      // cref: ArcGIS.Core.CIM.RasterResamplingType
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Update the raster colorizer on a mosaic layer
      {
        // Get the image sub-layer from the mosaic layer.
        //Note: needs to be called on the QueuedTask
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the colorizer from the image sub-layer.
        CIMRasterColorizer rasterColorizer = mosaicImageSubLayer.GetColorizer();
        // Update raster colorizer properties.
        rasterColorizer.Brightness = 10;
        rasterColorizer.Contrast = -5;
        rasterColorizer.ResamplingType = RasterResamplingType.NearestNeighbor;
        // Update the image sub-layer with the changed colorizer.
        mosaicImageSubLayer.SetColorizer(rasterColorizer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageMosaicSubLayer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
      // cref: ArcGIS.Core.CIM.CIMRasterRGBColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterRGBColorizer.StretchType
      // cref: ArcGIS.Core.CIM.RasterStretchType
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Update the RGB colorizer on a mosaic layer
      {
        // Get the image sub-layer from the mosaic layer.
        //Note: needs to be called on the QueuedTask
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the colorizer from the image sub-layer.
        CIMRasterColorizer rColorizer = mosaicImageSubLayer.GetColorizer();
        // Check if the colorizer is an RGB colorizer.
        if (rColorizer is CIMRasterRGBColorizer rasterRGBColorizer)
        {
          // Update RGB colorizer properties.
          rasterRGBColorizer.StretchType = RasterStretchType.ESRI;
          // Update the image sub-layer with the changed colorizer.
          mosaicImageSubLayer.SetColorizer(rasterRGBColorizer);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageMosaicSubLayer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      #region Check if a certain colorizer can be applied to a mosaic layer 
      {
        // Get the image sub-layer from the mosaic layer.
        //Note: needs to be called on the QueuedTask
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the list of colorizers that can be applied to the image sub-layer.
        IEnumerable<RasterColorizerType> applicableColorizerList =
    mosaicImageSubLayer.GetApplicableColorizers();
        // Check if the RGB colorizer is part of the list.
        bool isTrue_ContainTheColorizerType =
    applicableColorizerList.Contains(RasterColorizerType.RGBColorizer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageMosaicSubLayer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.CreateColorizerAsync(ArcGIS.Desktop.Mapping.RasterColorizerDefinition)
      // cref: ArcGIS.core.CIM.CIMRasterStretchColorizer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Create a new colorizer based on a default colorizer definition and apply it to the mosaic layer 
      {
        // Get the image sub-layer from the mosaic layer.
        //Note: needs to be called on the QueuedTask
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Check if the Stretch colorizer can be applied to the image sub-layer.
        if (mosaicImageSubLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition using the default constructor.
          StretchColorizerDefinition stretchColorizerDef_default = new StretchColorizerDefinition();
          // Create a new Stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_default =
      await mosaicImageSubLayer.CreateColorizerAsync(stretchColorizerDef_default) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image sub-layer.
          mosaicImageSubLayer.SetColorizer(newStretchColorizer_default);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageMosaicSubLayer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor(System.Int32, ArcGIS.Core.CIM.RasterStretchType, System.Double, ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.CreateColorizerAsync(ArcGIS.Desktop.Mapping.RasterColorizerDefinition)
      // cref: ArcGIS.core.CIM.CIMRasterStretchColorizer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Create a new colorizer based on a custom colorizer definition and apply it to the mosaic layer 
      {
        // Get the image sub-layer from the mosaic layer.
        //Note: needs to be called on the QueuedTask
        ImageMosaicSubLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Check if the Stretch colorizer can be applied to the image sub-layer.
        if (mosaicImageSubLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch colorizer definition specifying parameters
          // for band index, stretch type, gamma and color ramp.
          StretchColorizerDefinition stretchColorizerDef_custom =
      new StretchColorizerDefinition(1, RasterStretchType.ESRI, 2, colorRamp);
          // Create a new stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_custom =
      await mosaicImageSubLayer.CreateColorizerAsync(stretchColorizerDef_custom) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image sub-layer.
          mosaicImageSubLayer.SetColorizer(newStretchColorizer_custom);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams.ColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams, ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create a mosaic layer with a new colorizer definition
      // Create a new colorizer definition using default constructor.
      {
        StretchColorizerDefinition stretchColorizerDef = new StretchColorizerDefinition();
        var rasterLayerCreationParams = new RasterLayerCreationParams(new Uri("url"))
        {
          Name = "layerRasterName",
          ColorizerDefinition = stretchColorizerDef,
          MapMemberIndex = 0

        };
        // Create a mosaic layer using the colorizer definition created above.
        // Note: You can create a mosaic layer from a url, project item, or data connection.
        //Note: Run within a QueuedTask
        MosaicLayer newMosaicLayer = LayerFactory.Instance.CreateLayer<MosaicLayer>(rasterLayerCreationParams, map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.GetMosaicRule()
      // cref: ArcGIS.Core.CIM.CIMMosaicRule
      // cref: ArcGIS.Core.CIM.CIMMosaicRule.MosaicMethod
      // cref: ArcGIS.Core.CIM.RasterMosaicMethod
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.SetMosaicRule(ArcGIS.Core.CIM.CIMMosaicRule)
      #region Update the sort order - mosaic method on a mosaic layer
      {
        //Note: needs to be called on the QueuedTask
        // Get the image sub-layer from the mosaic layer.
        ImageServiceLayer mosaicImageSubLayer = mosaicLayer.GetImageLayer();
        // Get the mosaic rule.
        CIMMosaicRule mosaicingRule = mosaicImageSubLayer.GetMosaicRule();
        // Set the Mosaic Method to Center.
        mosaicingRule.MosaicMethod = RasterMosaicMethod.Center;
        // Update the mosaic with the changed mosaic rule.
        mosaicImageSubLayer.SetMosaicRule(mosaicingRule);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MosaicLayer.GetImageLayer()
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.GetMosaicRule()
      // cref: ArcGIS.Core.CIM.CIMMosaicRule
      // cref: ArcGIS.Core.CIM.CIMMosaicRule.MosaicOperatorType
      // cref: ArcGIS.Core.CIM.RasterMosaicOperatorType
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.SetMosaicRule(ArcGIS.Core.CIM.CIMMosaicRule)
      #region Update the resolve overlap - mosaic operator on a mosaic layer
      {
        //Note: needs to be called on the QueuedTask
        // Get the image sub-layer from the mosaic layer.
        ImageServiceLayer mosaicImageSublayerToUse = mosaicLayer.GetImageLayer();
        // Get the mosaic rule.
        CIMMosaicRule mosaicRule = mosaicImageSublayerToUse.GetMosaicRule();
        // Set the Mosaic Operator to Mean.
        mosaicRule.MosaicOperatorType = RasterMosaicOperatorType.Mean;
        // Update the mosaic with the changed mosaic rule.
        mosaicImageSublayerToUse.SetMosaicRule(mosaicRule);
      }
      #endregion

      #region ProSnippet Group: Image Service Layers
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer(System.Uri, ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
      #region Create an image service layer
      {
        string urlToUse =
        @"http://imagery.arcgisonline.com/arcgis/services/LandsatGLS/GLS2010_Enhanced/ImageServer";
        // Create an image service layer using the url for an image service.
        //Note: Run within a QueuedTask
        var isLayer = LayerFactory.Instance.CreateLayer(new Uri(urlToUse), map) as ImageServiceLayer;
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.Brightness
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.Contrast
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer.ResamplingType
      // cref: ArcGIS.Core.CIM.RasterResamplingType
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Update the raster colorizer on an image service layer
      {
        // Get the colorizer from the image service layer.

        CIMRasterColorizer rasterColorizer = imageServiceLayer.GetColorizer();
        // Update the colorizer properties.
        rasterColorizer.Brightness = 10;
        rasterColorizer.Contrast = -5;
        rasterColorizer.ResamplingType = RasterResamplingType.NearestNeighbor;
        // Update the image service layer with the changed colorizer.
        imageServiceLayer.SetColorizer(rasterColorizer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetColorizer()
      // cref: ArcGIS.Core.CIM.CIMRasterColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterRGBColorizer
      // cref: ArcGIS.Core.CIM.CIMRasterRGBColorizer.StretchType
      // cref: ArcGIS.Core.CIM.RasterStretchType
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Update the RGB colorizer on an image service layer
      {
        // Get the colorizer from the image service layer.
        //Note: needs to be called on the QueuedTask
        CIMRasterColorizer rColorizer = imageServiceLayer.GetColorizer();
        // Check if the colorizer is an RGB colorizer.
        if (rColorizer is CIMRasterRGBColorizer rasterRGBColorizer)
        {
          // Update RGB colorizer properties.
          rasterRGBColorizer.StretchType = RasterStretchType.ESRI;
          // Update the image service layer with the changed colorizer.
          imageServiceLayer.SetColorizer((CIMRasterColorizer)rasterRGBColorizer);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      #region Check if a certain colorizer can be applied to an image service layer 
      {
        // Get the list of colorizers that can be applied to the imager service layer.
        //Note: needs to be called on the QueuedTask
        IEnumerable<RasterColorizerType> applicableColorizerList = imageServiceLayer.GetApplicableColorizers();
        // Check if the RGB colorizer is part of the list.
        bool isTrue_ContainTheColorizerType = applicableColorizerList.Contains(RasterColorizerType.RGBColorizer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.CreateColorizerAsync(ArcGIS.Desktop.Mapping.RasterColorizerDefinition)
      // cref: ArcGIS.core.CIM.CIMRasterStretchColorizer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Create a new colorizer based on a default colorizer definition and apply it to the image service layer 
      {
        // Check if the Stretch colorizer can be applied to the image service layer.
        if (imageServiceLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition using the default constructor.
          StretchColorizerDefinition stretchColorizerDef_default = new StretchColorizerDefinition();
          // Create a new Stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_default =
      await imageServiceLayer.CreateColorizerAsync(stretchColorizerDef_default) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image service layer.
          imageServiceLayer.SetColorizer(newStretchColorizer_default);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.GetApplicableColorizers()
      // cref: ArcGIS.Desktop.Mapping.RasterColorizerType
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor(System.Int32, ArcGIS.Core.CIM.RasterStretchType, System.Double, ArcGIS.Core.CIM.CIMColorRamp)
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.CreateColorizerAsync(ArcGIS.Desktop.Mapping.RasterColorizerDefinition)
      // cref: ArcGIS.core.CIM.CIMRasterStretchColorizer
      // cref: ArcGIS.Desktop.Mapping.BasicRasterLayer.SetColorizer(ArcGIS.Core.CIM.CIMRasterColorizer)
      #region Create a new colorizer based on a custom colorizer definition and apply it to the image service layer 
      {
        // Check if the Stretch colorizer can be applied to the image service layer.
        //Note: needs to be called on the QueuedTask
        if (imageServiceLayer.GetApplicableColorizers().Contains(RasterColorizerType.StretchColorizer))
        {
          // Create a new Stretch Colorizer Definition specifying parameters
          // for band index, stretch type, gamma and color ramp. 
          StretchColorizerDefinition stretchColorizerDef_custom =
      new StretchColorizerDefinition(1, RasterStretchType.ESRI, 2, colorRamp);
          // Create a new stretch colorizer using the colorizer definition created above.
          CIMRasterStretchColorizer newStretchColorizer_custom =
      await imageServiceLayer.CreateColorizerAsync(stretchColorizerDef_custom) as CIMRasterStretchColorizer;
          // Set the new colorizer on the image service layer.
          imageServiceLayer.SetColorizer(newStretchColorizer_custom);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StretchColorizerDefinition.#ctor
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams.#ctor(System.Uri)
      // cref: ArcGIS.Desktop.Mapping.RasterLayerCreationParams.ColorizerDefinition
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams, ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      #region Create an image service layer with a new colorizer definition
      {
        // Create a new colorizer definition using default constructor.
        StretchColorizerDefinition stretchColorizerDef = new StretchColorizerDefinition();
        var rasterLayerCreationParams = new RasterLayerCreationParams(new Uri(url))
        {
          Name = "RasterLayer",
          ColorizerDefinition = stretchColorizerDef,
          MapMemberIndex = 0
        };
        // Create an image service layer using the colorizer definition created above.
        //Note: Run within a QueuedTask
        ImageServiceLayer imageServiceLayerWithColorizer =
      LayerFactory.Instance.CreateLayer<ImageServiceLayer>(rasterLayerCreationParams, map);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.GetMosaicRule()
      // cref: ArcGIS.Core.CIM.CIMMosaicRule
      // cref: ArcGIS.Core.CIM.CIMMosaicRule.MosaicMethod
      // cref: ArcGIS.Core.CIM.RasterMosaicMethod
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.SetMosaicRule(ArcGIS.Core.CIM.CIMMosaicRule)
      #region Update the sort order - mosaic method on an image service layer
      {
        // Get the mosaic rule of the image service.
        //Note: needs to be called on the QueuedTask
        CIMMosaicRule mosaicRule = imageServiceLayer.GetMosaicRule();
        // Set the Mosaic Method to Center.
        mosaicRule.MosaicMethod = RasterMosaicMethod.Center;
        // Update the image service with the changed mosaic rule.
        imageServiceLayer.SetMosaicRule(mosaicRule);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.GetMosaicRule()
      // cref: ArcGIS.Core.CIM.CIMMosaicRule
      // cref: ArcGIS.Core.CIM.CIMMosaicRule.MosaicOperatorType
      // cref: ArcGIS.Core.CIM.RasterMosaicOperatorType
      // cref: ArcGIS.Desktop.Mapping.ImageServiceLayer.SetMosaicRule(ArcGIS.Core.CIM.CIMMosaicRule)
      #region Update the resolve overlap - mosaic operator on an image service layer
      {
        // Get the mosaic rule of the image service.
        //Note: needs to be called on the QueuedTask
        CIMMosaicRule mosaicingRule = imageServiceLayer.GetMosaicRule();
        // Set the Mosaic Operator to Mean.
        mosaicingRule.MosaicOperatorType = RasterMosaicOperatorType.Mean;
        // Update the image service with the changed mosaic rule.
        imageServiceLayer.SetMosaicRule(mosaicingRule);
      }
      #endregion
      #region ProSnippet Group: Renderers
      #endregion
      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.#ctor(System.Collections.Generic.List<System.String>, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.CIMColorRamp, ArcGIS.Core.CIM.CIMSymbolReference, System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Set unique value renderer to the selected feature layer of the active map
      {
        //field to be used to retrieve unique values
        //Note: Run within a QueuedTask
        var fields = new List<string> { "Type" };
        //constructing a point symbol as a template symbol
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(
            ColorFactory.Instance.GreenRGB, 16.0, SimpleMarkerStyle.Pushpin);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //constructing renderer definition for unique value renderer
        UniqueValueRendererDefinition uniqueValueRendererDef =
      new UniqueValueRendererDefinition(fields, symbolPointTemplate);

        //creating a unique value renderer
        CIMUniqueValueRenderer uniqueValueRenderer = featureLayer.CreateRenderer(uniqueValueRendererDef) as CIMUniqueValueRenderer;

        //setting the renderer to the feature layer
        featureLayer.SetRenderer(uniqueValueRenderer);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMUniqueValue
      // cref: ArcGIS.Core.CIM.CIMUniqueValue.FieldValues
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Editable
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Label
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Patch
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Symbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Visible
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Values
      // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup
      // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup.Classes
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.UseDefaultSymbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultLabel
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultSymbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Groups
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Fields
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Create a UniqueValueRenderer to specify symbols to values 
      {
        //The goal is to construct the CIMUniqueValueRenderer which will be applied to the feature layer.
        // To do this, the following are the objects we need to set the renderer up with the fields and symbols.
        // As a reference, this is the USCities dataset. Snippet will create a unique value renderer that applies 
        // specific symbols to all the cities in California and Alabama.  The rest of the cities will use a default symbol.

        // First create a "CIMUniqueValueClass" for the cities in Alabama.
        List<CIMUniqueValue> listUniqueValuesAlabama = new List<CIMUniqueValue> { new CIMUniqueValue { FieldValues = new string[] { "Alabama" } } };
        CIMUniqueValueClass alabamaUniqueValueClass = new CIMUniqueValueClass
        {
          Editable = true,
          Label = "Alabama",
          Patch = PatchShape.Default,
          Symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB).MakeSymbolReference(),
          Visible = true,
          Values = listUniqueValuesAlabama.ToArray()
        };
        // Create a "CIMUniqueValueClass" for the cities in California.
        List<CIMUniqueValue> listUniqueValuescalifornia = new List<CIMUniqueValue> { new CIMUniqueValue { FieldValues = new string[] { "California" } } };
        CIMUniqueValueClass californiaUniqueValueClass = new CIMUniqueValueClass
        {
          Editable = true,
          Label = "California",
          Patch = PatchShape.Default,
          Symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlueRGB).MakeSymbolReference(),
          Visible = true,
          Values = listUniqueValuescalifornia.ToArray()
        };
        //Create a list of the above two CIMUniqueValueClasses
        List<CIMUniqueValueClass> listUniqueValueClasses = new List<CIMUniqueValueClass>
        {
                      alabamaUniqueValueClass, californiaUniqueValueClass
        };
        //Create a list of CIMUniqueValueGroup
        CIMUniqueValueGroup uvg = new CIMUniqueValueGroup
        {
          Classes = listUniqueValueClasses.ToArray(),
        };
        List<CIMUniqueValueGroup> listUniqueValueGroups = new List<CIMUniqueValueGroup> { uvg };
        //Create the CIMUniqueValueRenderer
        CIMUniqueValueRenderer uvr = new CIMUniqueValueRenderer
        {
          UseDefaultSymbol = true,
          DefaultLabel = "all other values",
          DefaultSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreyRGB).MakeSymbolReference(),
          Groups = listUniqueValueGroups.ToArray(),
          Fields = new string[] { "STATE_NAME" }
        };
        //Set the feature layer's renderer.
        //Note: Run within a QueuedTask
        featureLayer.SetRenderer(uvr);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.Radius
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.WeightField
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.ColorRamp
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.RendereringQuality
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.UpperLabel
      // cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition.LowerLabel
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Core.CIM.CIMHeatMapRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Create a Heatmap Renderer
      {
        string colorBrewerSchemesName = "ArcGIS Colors";
        //Get the style project item that contains the color ramps
        //Refer to the Initialize region for an example of how to get a style item
        //and the color ramp from it.
        //Note: Run within a QueuedTask
        //defining a heatmap renderer that uses values from Population field as the weights
        HeatMapRendererDefinition heatMapDef = new HeatMapRendererDefinition()
        {
          Radius = 20,
          WeightField = "Population",
          ColorRamp = colorRamp,
          RendereringQuality = 8,
          UpperLabel = "High Density",
          LowerLabel = "Low Density"
        };
        CIMHeatMapRenderer heatMapRndr = featureLayer.CreateRenderer(heatMapDef) as CIMHeatMapRenderer;
        featureLayer.SetRenderer(heatMapRndr);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.ArcadeExpression
      #region Use Arcade expression to create Class Breaks
      {
        var usaStatesLayer = map.GetLayersAsFlattenedList().OfType<FeatureLayer>()
                       .FirstOrDefault(fl => fl.Name == "USA States");
        if (usaStatesLayer == null) return;

        //Note: Run within a QueuedTask
        //Create a color ramp from red to blue
        var colorRampForPopulation = ColorFactory.Instance.ConstructColorRamp(
              ColorRampAlgorithm.LinearContinuous,
              ColorFactory.Instance.RedRGB, ColorFactory.Instance.BlueRGB);

        //This Arcade expression categorizes the POP2000 field in the usaStates layer
        //into Low, Medium, and High values
        var arcade = @"if ($feature.POP2000 < 1000000)  
                         {return ""Low"";}" +
                     @"else if ($feature.POP2000 >= 100000 && $feature.POP2000 < 5000000) 
                        {return ""Medium""; }" +
                      @"else if ($feature.POP2000 >= 5000000) 
                        {return ""High"";}" +
                      @"else{return ""NA"";}";
        //Create the Arcade expression info
        CIMExpressionInfo arcadeExp = new CIMExpressionInfo();
        arcadeExp.Expression = arcade;
        arcadeExp.ReturnType = ExpressionReturnType.Default;
        arcadeExp.Title = "State Population Categorized";
        arcadeExp.Name = "USA States Population 2000";

        //Each population category is assigned a color from the color ramp and symbolized on the map
        // using a unique value renderer
        var uvr_desc = new UniqueValueRendererDefinition()
        {
          //ValueFields = new List<string> { "STATION" },
          ColorRamp = colorRampForPopulation,
          UseDefaultSymbol = true,
          ValuesLimit = 100,
          ArcadeExpression = arcadeExp.Expression
        };
        //Create the renderer and assign it to the layer
        var renderer = usaStatesLayer.CreateRenderer(uvr_desc);
        usaStatesLayer.SetRenderer(renderer);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.CIMColorRamp, System.String, System.String, System.Double, System.Double)
      // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition.ShowNullValues
      // cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition.NullValueLabel
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Core.CIM.CIMClassBreaksRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Create an unclassed Renderer
      {
        string colorBrewerSchemesName = "ArcGIS Colors";
        //Get the style project item that contains the color ramps
        //Refer to the Initialize region for an example of how to get a style item
        //Note: Run within a QueuedTask
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 16.0, SimpleMarkerStyle.Diamond);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //defining an unclassed renderer with custom upper and lower stops
        //all features with value >= 5,000,000 will be drawn with the upper color from the color ramp
        //all features with value <= 50,000 will be drawn with the lower color from the color ramp
        UnclassedColorsRendererDefinition unclassRndrDef = new UnclassedColorsRendererDefinition
                              ("Population", symbolPointTemplate, colorRamp, "Highest", "Lowest", 5000000, 50000)
        {

          //drawing features with null values with a different symbol
          ShowNullValues = true,
          NullValueLabel = "Unknown"
        };
        // Create a point symbol for null values
        CIMPointSymbol nullSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 16.0, SimpleMarkerStyle.Circle);
        unclassRndrDef.NullValueSymbol = nullSym.MakeSymbolReference();
        //Create the unclassed renderer using the definition
        CIMClassBreaksRenderer cbRndr = featureLayer.CreateRenderer(unclassRndrDef) as CIMClassBreaksRenderer;
        //Set the renderer to the feature layer
        featureLayer.SetRenderer(cbRndr);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.CIMSymbolReference, System.Double, System.Double, System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.UpperSizeStop
      // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.LowerSizeStop
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Core.CIM.CIMProportionalRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Create a Proportion Renderer with max and min symbol size capped
      {
        string colorBrewerSchemesName = "ArcGIS Colors";
        //Get the style project item that contains the color ramps
        //Refer to the Initialize region for an example of how to get a style item
        //Note: Run within a QueuedTask
        //Creating a point symbol to be used as a template symbol for the proportional renderer
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //minimum symbol size is capped to 4 point while the maximum symbol size is set to 50 point
        //Creating a proportional renderer definition that uses the "Population" field
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition("POPULATION", symbolPointTemplate, 4, 50, true)
        {
          //setting upper and lower size stops to stop symbols growing or shrinking beyond those thresholds
          UpperSizeStop = 5000000,  //features with values >= 5,000,000 will be drawn with maximum symbol size
          LowerSizeStop = 50000    //features with values <= 50,000 will be drawn with minimum symbol size
        };
        // Create a proportional renderer using the definition
        CIMProportionalRenderer propRndr = featureLayer.CreateRenderer(prDef) as CIMProportionalRenderer;
        // Set the renderer to the feature layer
        featureLayer.SetRenderer(propRndr);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition.#ctor(System.String, ArcGIS.Core.CIM.esriUnits, ArcGIS.Core.CIM.CIMSymbolReference, ArcGIS.Core.CIM.SymbolShapes, ArcGIS.Core.CIM.ValueRepresentations)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Core.CIM.CIMProportionalRenderer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
      #region Create a True Proportion Renderer
      {
        string colorBrewerSchemesName = "ArcGIS Colors";
        //Get the style project item that contains the color ramps
        //Refer to the Initialize region for an example of how to get a style item
        //Note: Run within a QueuedTask
        //Creating a point symbol to be used as a template symbol for the proportional renderer
        CIMPointSymbol pointSym = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
        CIMSymbolReference symbolPointTemplate = pointSym.MakeSymbolReference();

        //Defining proportional renderer where size of symbol will be same as its value in field used in the renderer.
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition("POPULATION", esriUnits.esriMeters, symbolPointTemplate, SymbolShapes.Square, ValueRepresentations.Radius);
        //Create a Proportional renderer using the definition
        CIMProportionalRenderer propRndr = featureLayer.CreateRenderer(prDef) as CIMProportionalRenderer;
        // Set the renderer to the feature layer
        featureLayer.SetRenderer(propRndr);
      }
      #endregion
      #region ProSnippet Group: Attribute Table - ITablePane
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePane.ZoomLevel
      // cref: ArcGIS.Desktop.Mapping.ITablePane.SetZoomLevel
      #region Set zoom level for Attribute Table
      {
        //Check if the active pane is an ITablePane
        if (FrameworkApplication.Panes.ActivePane is ITablePane tablePane)
        {
          //Get the current zoom level of the table pane
          var currentZoomLevel = tablePane.ZoomLevel;
          // Set a new zoom level, for example, increase it by 50
          var newZoomLevel = currentZoomLevel + 50;
          // Set the new zoom level to the table pane
          tablePane.SetZoomLevel(newZoomLevel);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePane.MapMember
      // cref: ArcGIS.Desktop.Mapping.ITablePane.ActiveObjectID
      // cref: ArcGIS.Desktop.Mapping.ITablePane.ActiveColumn
      #region Retrieve the values of selected cell in the attribute table
      {
        {
          if (FrameworkApplication.Panes.ActivePane is ITablePane tablePane)
          {
            var mapMember = tablePane.MapMember;
            //Get the active row's object ID from the table pane
            var oid = tablePane.ActiveObjectID;
            if (oid.HasValue && oid.Value != -1 && mapMember != null)
            {
              //Get the field of the active column
              var activeField = tablePane.ActiveColumn;
              QueuedTask.Run(() =>
              {
                // TODO: Use core objects to retrieve record and get value
              });
            }
          }
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ITablePane
      // cref: ArcGIS.Desktop.Mapping.ITablePane.BringIntoView
      #region Move to a particular row
      {
        // Check if the active pane is an ITablePane  
        if (FrameworkApplication.Panes.ActivePane is ITablePane tablePane)
        {
          // move to first row
          tablePane.BringIntoView(0);

          // move to sixth row
          tablePane.BringIntoView(5);
        }
      }
      #endregion
      #region ProSnippet Group: Working with Standalone Tables
      #endregion
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableFactory.CreateStandaloneTable(System.Uri, ArcGIS.Desktop.Mapping.IStandaloneTableContainerEdit, System.Int32, System.String)
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableCreationParams
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableCreationParams.#ctor(ArcGIS.Desktop.Core.Item)
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.StandaloneTableFactory.CreateStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTableCreationParams, ArcGIS.Desktop.Mapping.IStandaloneTableContainerEdit)
      #region Create a StandaloneTable
      {
        //container can be a map or group layer
        var container = MapView.Active.Map;
        //var container =  MapView.Active.Map.GetLayersAsFlattenedList()
        //                                  .OfType<GroupLayer>().First();
        //Note: Run within a QueuedTask
        //use a local path
        var table = StandaloneTableFactory.Instance.CreateStandaloneTable(
            new Uri(@"C:\Temp\Data\SDK.gdb\EarthquakeDamage", UriKind.Absolute), container);
        //use a URI to a feature service table endpoint
        var table2 = StandaloneTableFactory.Instance.CreateStandaloneTable(
                      new Uri(@"https://bexdog.esri.com/server/rest/services/FeatureServer" + "/2", UriKind.Absolute), container);
        //Use an item
        var item = ItemFactory.Instance.Create(@"C:\Temp\Data\SDK.gdb\ParcelOwners");
        var tableCreationParams = new StandaloneTableCreationParams(item);
        var table3 = StandaloneTableFactory.Instance.CreateStandaloneTable(tableCreationParams, container);

        //use table creation params
        var table_params = new StandaloneTableCreationParams(item)
        {
          DefinitionQuery = new DefinitionQuery(whereClause: "LAND_USE = 3", name: "Landuse")
        };
        var table4 = StandaloneTableFactory.Instance.CreateStandaloneTable(table_params, container);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.Map.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.StandaloneTables
      // cref: ArcGIS.Desktop.Core.FrameworkExtender.OpenTablePane(ArcGIS.Desktop.Framework.PaneCollection,ArcGIS.Desktop.Mapping.MapMember, ArcGIS.Desktop.Mapping.TableViewMode)
      #region Retrieve a table from its container
      {
        var container = MapView.Active.Map;

        //the map standalone table collection
        var table = container.GetStandaloneTablesAsFlattenedList()
                                .FirstOrDefault(tbl => tbl.Name == "EarthquakeDamage");

        //or from a group layer
        var grp_layer = MapView.Active.Map.FindLayers("GroupLayer1").First() as GroupLayer;
        var table2 = grp_layer.FindStandaloneTables("EarthquakeDamage").First();
        //or         grp_layer.GetStandaloneTablesAsFlattenedList().First()
        //or         grp_layer.StandaloneTables.Where(...).First(), etc.

        //show the table in a table view 
        //use FrameworkApplication.Current.Dispatcher.BeginInvoke if not on the UI thread
        FrameworkApplication.Panes.OpenTablePane(table2);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.MoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable, System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.Map.MoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable, ArcGIS.Desktop.Mapping.CompositeLayerWithTables, System.Int32)
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.FindStandaloneTables(System.String)
      // cref: ArcGIS.Desktop.Mapping.Map.MoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable, System.Int32)
      #region Move a Standalone table
      {
        //get the first group layer that has at least one table
        var grp_layer = MapView.Active.Map.GetLayersAsFlattenedList()
          .OfType<GroupLayer>().First(g => g.StandaloneTables.Count > 0);
        grp_layer.MoveStandaloneTable(grp_layer.StandaloneTables.First(), -1);

        //move the last table in the map standalone tables to a group
        //layer and place it at position 3. If 3 is invalid, the table
        //will be placed at the bottom of the target container
        //assumes the map has at least one standalone table...
        var table = map.StandaloneTables.Last();
        map.MoveStandaloneTable(table, grp_layer, 3);

        //move a table from a group layer to the map standalone tables
        //collection - assumes a table called 'Earthquakes' exists
        var table2 = grp_layer.FindStandaloneTables("Earthquakes").First();
        //move to the map container
        map.MoveStandaloneTable(table2, 0);//will be placed at the top
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.Map.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable)
      // cref: ArcGIS.Desktop.Mapping.Map.RemoveStandaloneTables(System.IEnumerable<ArcGIS.Desktop.Mapping.StandaloneTable>)
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.GetStandaloneTablesAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.CompositeLayerWithTables.StandaloneTables
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.RemoveStandaloneTable(ArcGIS.Desktop.Mapping.StandaloneTable)
      // cref: ArcGIS.Desktop.Mapping.GroupLayer.RemoveStandaloneTables(System.IEnumerable<ArcGIS.Desktop.Mapping.StandaloneTable>)
      #region Remove a Standalone table
      {
        //get the first group layer that has at least one table
        var grp_layer = MapView.Active.Map.GetLayersAsFlattenedList()
          .OfType<GroupLayer>().First(g => g.StandaloneTables.Count > 0);

        //Note: Run within a QueuedTask
        //get the tables from the map container
        var tables = map.GetStandaloneTablesAsFlattenedList();
        //delete the first...
        if (tables.Count() > 0)
        {
          map.RemoveStandaloneTable(tables.First());
          //or delete all of them
          map.RemoveStandaloneTables(tables);
        }

        //delete a table from a group layer
        //assumes it has at least one table...
        grp_layer.RemoveStandaloneTable(grp_layer.StandaloneTables.First());
      }
      #endregion

      #region ProSnippet Group: Metadata
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetMetadata()
      // cref: ArcGIS.Desktop.Mapping.Map.GetCanEditMetadata()
      // cref: ArcGIS.Desktop.Mapping.Map.SetMetadata(System.String)
      #region Get and Set Map Metadata
      {
        //Note: Run within a QueuedTask
        //Get map's metadata
        var mapMetadata = map.GetMetadata();
        //TODO:Make edits to metadata using the retrieved mapMetadata string.

        //Set the modified metadata back to the map.
        if (map.GetCanEditMetadata())
          map.SetMetadata(mapMetadata);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapMember.GetUseSourceMetadata()
      // cref: ArcGIS.Desktop.Mapping.MapMember.SetUseSourceMetadata(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.MapMember.SupportsMetadata
      // cref: ArcGIS.Desktop.Mapping.MapMember.GetMetadata()
      // cref: ArcGIS.Desktop.Mapping.MapMember.GetCanEditMetadata()
      // cref: ArcGIS.Desktop.Mapping.MapMember.SetMetadata(System.String)
      #region Layer Metadata
      {
        //Search for only layers/tables here if needed.
        MapMember mapMember = map.GetLayersAsFlattenedList().FirstOrDefault();
        if (mapMember == null) return;
        //Note: Run within a QueuedTask
        //Gets whether or not the MapMember stores its own metadata or uses metadata retrieved
        //from its source. This method must be called on the MCT. Use QueuedTask.Run
        bool doesUseSourceMetadata = mapMember.GetUseSourceMetadata();

        //Sets whether or not the MapMember will use its own metadata or the metadata from
        //its underlying source (if it has one). This method must be called on the MCT.
        //Use QueuedTask.Run
        mapMember.SetUseSourceMetadata(true);

        //Does the MapMember supports metadata
        var supportsMetadata = mapMember.SupportsMetadata;

        //Get MapMember metadata
        var metadatstring = mapMember.GetMetadata();
        //TODO:Make edits to metadata using the retrieved mapMetadata string.

        //Set the modified metadata back to the mapmember (layer, table..)
        if (mapMember.GetCanEditMetadata())
          mapMember.SetMetadata(metadatstring);
      }
      #endregion
      #region ProSnippet Group: SelectionSet
      #endregion

      // cref: ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary``1(System.Collections.Generic.Dictionary{``0,System.Collections.Generic.List{System.Int64}})
      #region Translate From Dictionary to SelectionSet
      {
        //Create a selection set from a list of object ids
        //using FromDictionary
        MapMember us_zips_layer = map.GetLayersAsFlattenedList().OfType<FeatureLayer>().First(s => s.Name == "USA zips layer");
        var addToSelection = new Dictionary<MapMember, List<long>>();
        addToSelection.Add(us_zips_layer, new List<long> { 1506, 2696, 2246, 1647, 948 });
        //Create a SelectionSet object
        var selSet = ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary(addToSelection);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1
      #region Translate from SelectionSet to Dictionary
      {
        MapMember us_zips_layer = map.GetLayersAsFlattenedList().OfType<FeatureLayer>().First(s => s.Name == "USA zips layer");

        var addToSelection = new Dictionary<MapMember, List<long>>();
        addToSelection.Add(us_zips_layer, new List<long> { 1506, 2696, 2246, 1647, 948 });
        //Create a SelectionSet object
        var selSet = ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary(addToSelection);
        var selSetDict = selSet.ToDictionary();

        // convert to the dictionary and only include those that are of type FeatureLayer
        var selSetDictFeatureLayer = selSet.ToDictionary<FeatureLayer>();
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.Contains(ArcIGS.Desktop.Mapping.MapMember)
      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.Item(ArcIGS.Desktop.Mapping.MapMember)
      #region Get OIDS from a SelectionSet for a given MapMember
      {
        MapMember us_zips_layer = map.GetLayersAsFlattenedList().OfType<FeatureLayer>().First(s => s.Name == "USA zips layer");

        var addToSelection = new Dictionary<MapMember, List<long>>();
        addToSelection.Add(us_zips_layer, new List<long> { 1506, 2696, 2246, 1647, 948 });
        //Create a SelectionSet object
        var selSet = ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary(addToSelection);

        if (selSet.Contains(us_zips_layer))
        {
          var oids = selSet[us_zips_layer];
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1
      #region Get OIDS from a SelectionSet for a given MapMember by Name
      {
        MapMember us_zips_layer = map.GetLayersAsFlattenedList().OfType<FeatureLayer>().First(s => s.Name == "USA zips layer");

        var addToSelection = new Dictionary<MapMember, List<long>>();
        addToSelection.Add(us_zips_layer, new List<long> { 1506, 2696, 2246, 1647, 948 });
        //Create a SelectionSet object
        var selSet = ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary(addToSelection);

        var kvp = selSet.ToDictionary().Where(kvp => kvp.Key.Name == "LayerName").FirstOrDefault();
        var oidList = kvp.Value;
      }
      #endregion

      #region Check SelectionSet Equality
      {
        // get map selection
        var mapSelSet = MapView.Active.Map.GetSelection();

        // create a selectionSet
        var selDict = new Dictionary<MapMember, List<long>>();
        selDict.Add(featureLayer, new List<long> { 1506, 2696, 2246, 1647, 948 });
        var layerSelSet = ArcGIS.Desktop.Mapping.SelectionSet.FromDictionary(selDict);

        // test for equality - use Equals method
        var isEquals = mapSelSet.Equals(layerSelSet);

        // test for equality - use operators
        var equals = mapSelSet == layerSelSet;
        var notEquals = mapSelSet != layerSelSet;
      }

      #endregion

      #region ProSnippet Group: Selection Options
      #endregion
      // cref: ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions
      // cref: ArcGIS.Desktop.Core.SelectionOptions
      // cref:ArcGIS.Desktop.Mapping.SelectionMethod
      // cref:ArcGIS.Desktop.Mapping.SelectionCombinationMethod
      #region Get/Set Selection Options
      {
        var options = ApplicationOptions.SelectionOptions;
        //Note: Run within a QueuedTask
        var defaultColor = options.DefaultSelectionColor;
        //Set the selection color to red
        var color = options.SelectionColor as CIMRGBColor;
        options.SetSelectionColor(ColorFactory.Instance.CreateRGBColor(255, 0, 0));

        //Set the selection fill color and fill hatch
        var defaultFill = options.DefaultSelectionFillColor;
        var fill = options.SelectionFillColor;
        var isHatched = options.IsSelectionFillHatched;
        options.SetSelectionFillColor(ColorFactory.Instance.CreateRGBColor(100, 100, 0));
        if (!isHatched)
          options.SetSelectionFillIsHatched(true);
        //Toggle the selection Chip and Graphic
        var showSelectionChip = options.ShowSelectionChip;
        options.SetShowSelectionChip(!showSelectionChip);

        var showSelectionGraphic = options.ShowSelectionGraphic;
        options.SetShowSelectionGraphic(!showSelectionGraphic);

        //Get the value indicating whether to save the layer and standalone table selection with the map.
        var saveSelection = options.SaveSelection;
        options.SetSaveSelection(!saveSelection);
        //Get/Set the selection tolerance
        var defaultTol = options.DefaultSelectionTolerance;
        var tol = options.SelectionTolerance;
        options.SetSelectionTolerance(2 * defaultTol);

        // extension methods available for selection methods and combination methods
        var selMethod = options.SelectionMethod;
        options.SetSelectionMethod(SelectionMethod.Contains);

        var combMethod = options.CombinationMethod;
        options.SetCombinationMethod(SelectionCombinationMethod.Add);

        // note that the following SelectionCombinationMethod is not supported
        //options.SetCombinationMethod(SelectionCombinationMethod.XOR);
      }
      #endregion



      #region ProSnippet Group: Elevation Profile
      #endregion
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline})
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint})
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline})
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint})
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
      #region Get Elevation profile from the default ground surface
      {
        // get the elevation profile for a polyline / set of polylines
        Polyline polyline = PolylineBuilderEx.CreatePolyline();
        MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(34, -118, SpatialReferences.WGS84);
        List<MapPoint> mapPoints = new List<MapPoint> { mapPoint };
        var result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync([polyline]);
        if (result.Status == SurfaceZsResultStatus.Ok)
        {
          var polylineZ = result.Polyline;

          // process the polylineZ

          // get the elevation profile for a set of points
          result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(mapPoints);
          if (result.Status == SurfaceZsResultStatus.Ok)
          {
            polylineZ = result.Polyline;
            // process the polylineZ
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
      #region Get Elevation profile from a specific surface
      {
        // find a specific elevation surface layer
        var eleLayer = MapView.Active.Map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "TIN");
        Polyline polyline = PolylineBuilderEx.CreatePolyline();
        MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(34, -118, SpatialReferences.WGS84);
        List<MapPoint> mapPoints = new List<MapPoint> { mapPoint };
        // get the elevation profile for a polyline / set of polylines
        // use the specific elevation surface layer
        var zResult = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync([polyline], eleLayer);
        if (zResult.Status == SurfaceZsResultStatus.Ok)
        {
          var polylineZ = zResult.Polyline;

          // process the polylineZ
        }

        // get the elevation profile for a set of points
        // use the specific elevation surface layer
        zResult = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(mapPoints, eleLayer);
        if (zResult.Status == SurfaceZsResultStatus.Ok)
        {
          var polylineZ = zResult.Polyline;

          // process the polylineZ
        }
      }
      #endregion


      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
      #region Interpolate a line between two points and calculate the elevation profile 
      {
        int numPoints = 20;//Or any number of points you want to interpolate
        // use a specific elevation surface
        var eleLayer = MapView.Active.Map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "TIN");
        // use the default ground elevation surface
        MapPoint startPt = MapPointBuilderEx.CreateMapPoint(-130, 20, SpatialReferences.WGS84);

        MapPoint endPt = MapPointBuilderEx.CreateMapPoint(-100, 50, SpatialReferences.WGS84);
        var result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(startPt, endPt, numPoints);
        if (result.Status == SurfaceZsResultStatus.Ok)
        {
          var polylineZ = result.Polyline;

          // process the polylineZ
        }
        // use a specific elevation surface
        result = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(startPt, endPt, numPoints, eleLayer);
        if (result.Status == SurfaceZsResultStatus.Ok)
        {
          var polylineZ = result.Polyline;

          // process the polylineZ
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
      #region Show Elevation profile graph with the default ground surface
      {
        if (!MapView.Active.CanShowElevationProfileGraph())
          return;

        // show the elevation profile for a polyline 
        // use the default ground surface layer
        Polyline polyline = PolylineBuilderEx.CreatePolyline();
        MapView.Active.ShowElevationProfileGraph([polyline]);

        // show the elevation profile for a set of points
        // use the default ground surface layer
        //Some points
        MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(34, -118, SpatialReferences.WGS84);
        List<MapPoint> mapPoints = new List<MapPoint> { mapPoint };
        MapView.Active.ShowElevationProfileGraph(mapPoints);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.Polyline},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(System.Collections.Generic.IEnumerable{ArcGIS.Core.Geometry.MapPoint},ArcGIS.Desktop.Mapping.ElevationProfileParameters)
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.SurfaceLayer
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.Densify
      #region Show Elevation profile graph with a specific surface
      {
        if (!MapView.Active.CanShowElevationProfileGraph())
          return;
        // use a specific elevation surface
        var eleLayer = MapView.Active.Map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "TIN");
        // set up the parameters
        var profileParams = new ElevationProfileParameters();
        profileParams.SurfaceLayer = eleLayer;
        profileParams.Densify = true;

        // show the elevation profile for a polyline using the params
        //Any line
        Polyline polyline = PolylineBuilderEx.CreatePolyline();
        MapView.Active.ShowElevationProfileGraph([polyline], profileParams);

        // show the elevation profile for a set of points using the params
        //Some points
        MapPoint mapPoint = MapPointBuilderEx.CreateMapPoint(34, -118, SpatialReferences.WGS84);
        List<MapPoint> mapPoints = new List<MapPoint> { mapPoint };
        MapView.Active.ShowElevationProfileGraph(mapPoints, profileParams);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationProfileParameters)
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.SurfaceLayer
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileParameters.Densify
      #region Show Elevation profile graph between two points 
      {
        int numPoints = 20;

        if (!MapView.Active.CanShowElevationProfileGraph())
          return;

        // show the elevation profile 
        // use the default ground elevation surface
        MapPoint startPt = MapPointBuilderEx.CreateMapPoint(-130, 20, SpatialReferences.WGS84);
        MapPoint endPt = MapPointBuilderEx.CreateMapPoint(-100, 50, SpatialReferences.WGS84);
        MapView.Active.ShowElevationProfileGraph(startPt, endPt, numPoints);

        // find a specific elevation surface layer
        var tinLayer = MapView.Active.Map.GetElevationSurfaceLayers().FirstOrDefault(l => l.Name == "TIN");

        // set up the params
        var elevProfileParams = new ElevationProfileParameters();
        elevProfileParams.SurfaceLayer = tinLayer;
        elevProfileParams.Densify = false;

        // show the elevation profile using the params
        MapView.Active.ShowElevationProfileGraph(startPt, endPt, numPoints, elevProfileParams);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurfaceAsync(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32)
      // cref: ArcGIS.Desktop.Mapping.Map.GetElevationProfileFromSurface(ArcGIS.Core.Geometry.MapPoint,ArcGIS.Core.Geometry.MapPoint,System.Int32,ArcGIS.Desktop.Mapping.ElevationSurfaceLayer)
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Status
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileResult.Polyline
      // cref: ArcGIS.Desktop.Mapping.SurfaceZsResultStatus
      // cref: ArcGIS.Desktop.Mapping.MapView.CanShowElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.MapView.ShowElevationProfileGraph(ArcGIS.Desktop.Mapping.ElevationProfileResult)
      #region Show Elevation profile graph using an ElevationProfileResult
      {
        MapPoint startPt = MapPointBuilderEx.CreateMapPoint(-130, 20, SpatialReferences.WGS84);
        MapPoint endPt = MapPointBuilderEx.CreateMapPoint(-100, 50, SpatialReferences.WGS84);
        var elevProfileResult = await MapView.Active.Map.GetElevationProfileFromSurfaceAsync(startPt, endPt, 10);
        if (elevProfileResult.Status != SurfaceZsResultStatus.Ok)
          return;

        if (!MapView.Active.CanShowElevationProfileGraph())
          return;

        // show the elevation profile using the result
        MapView.Active.ShowElevationProfileGraph(elevProfileResult);
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapView.ElevationProfileGraphAdded
      // cref: ArcGIS.Desktop.Mapping.MapView.ElevationProfileGraphRemoved
      // cref: ArcGIS.Desktop.Mapping.MapView.GetElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ContentLoaded
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.Geometry
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ElevationProfileStatistics
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileStatistics
      #region Access the ElevationProfileGraph when added
      {
        // subscribe to the Added, Removed events for the elevation profile graph
        mapView.ElevationProfileGraphAdded += Mv_ElevationProfileGraphAdded;
        mapView.ElevationProfileGraphRemoved += Mv_ElevationProfileGraphRemoved;
        //Handles the event triggered when an elevation profile graph is removed.
        void Mv_ElevationProfileGraphRemoved(object sender, EventArgs e)
        {
          //TODO: handle the removal of the elevation profile graph
        }
        //Handles the event triggered when an elevation profile graph is added to the active map view.
        void Mv_ElevationProfileGraphAdded(object sender, EventArgs e)
        {
          // get the elevation profile graph from the view
          // this will be non-null since we are in a ElevationProfileGraphAdded handler
          var mv = MapView.Active;
          var graph = mv.GetElevationProfileGraph();

          // subscribe to the ContentLoaded event
          graph.ContentLoaded += Graph_ContentLoaded;
        }
        // Handles the event triggered when the elevation profile graph content is loaded.
        void Graph_ContentLoaded(object sender, EventArgs e)
        {
          // get the elevation profile graph
          var graph = sender as ArcGIS.Desktop.Mapping.ElevationProfileGraph;

          // get the elevation profile geometry
          var polyline = graph.Geometry;
          // get the elevation profile statistics
          var stats = graph.ElevationProfileStatistics;
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapView.GetElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.Geometry
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ElevationProfileStatistics
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileStatistics
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.IsReversed
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.IsExpanded
      #region Access the ElevationProfileGraph
      {
        var elevProfileGraph = MapView.Active.GetElevationProfileGraph();
        // Elevation profile graph will be null if no profile graph is displayed
        if (elevProfileGraph == null)
          return;

        // get the elevation profile geometry and stats
        var polyline = elevProfileGraph.Geometry;
        var stats = elevProfileGraph.ElevationProfileStatistics;

        // reverse the graph
        elevProfileGraph.IsReversed = !elevProfileGraph.IsReversed;

        // collapse the graph
        elevProfileGraph.IsExpanded = false;
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.MapView.GetElevationProfileGraph
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.CanExport
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ExportToImage(System.String)
      // cref: ArcGIS.Desktop.Mapping.ElevationProfileGraph.ExportToCSV(System.String)
      #region Export Elevation Profile Graph
      {

        var elevProfileGraph = MapView.Active.GetElevationProfileGraph();
        // Elevation profile graph will be null if no profile graph is displayed
        if (elevProfileGraph == null)
          return;

        if (elevProfileGraph.CanExport)
        {
          elevProfileGraph.ExportToImage("c:\\temp\\myprofileImage.png");
          elevProfileGraph.ExportToCSV("c:\\temp\\myprofile.csv");
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Core.ElevationProfileOptions
      // cref: ArcGIS.Desktop.Core.ExploratoryAnalysisOptions.CanSetElevationProfileOptions(ArcGIS.Desktop.Core.ElevationProfileOptions)
      // cref: ArcGIS.Desktop.Core.ExploratoryAnalysisOptions.SetElevationProfileOptionsAsync(ArcGIS.Desktop.Core.ElevationProfileOptions)
      // cref: ArcGIS.Desktop.Core.ExploratoryAnalysisOptions.GetDefaultElevationProfileOptions()
      #region Customize Elevation Profile Graph Display
      {
        // customize the elevation profile graph options
        var options = new ElevationProfileOptions()
        {
          LineColor = ColorFactory.Instance.CreateRGBColor(0, 0, 100),
          ShowAverageSlope = false,
          ShowMaximumSlope = false
        };

        var eaOptions = ApplicationOptions.ExploratoryAnalysisOptions;
        if (eaOptions.CanSetElevationProfileOptions(options))
          await eaOptions.SetElevationProfileOptionsAsync(options);

        // or reset to default options
        var defaultOptions = eaOptions.GetDefaultElevationProfileOptions();
        if (eaOptions.CanSetElevationProfileOptions(defaultOptions))
          await eaOptions.SetElevationProfileOptionsAsync(defaultOptions);
      }
      #endregion
      #region ProSnippet Group: Device Location API, GPS/GNSS Devices
      #endregion
      // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.#ctor
      // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.ComPort
      // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.BaudRate
      // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.AntennaHeight
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.#ctor
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.AccuracyThreshold
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.Open(ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationSource,ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties)
      #region Connect to a Device Location Source
      {
        var newSrc = new SerialPortDeviceLocationSource();
        //Specify the COM port the device is connected to
        newSrc.ComPort = "Com3";
        newSrc.BaudRate = 4800;
        newSrc.AntennaHeight = 3;  // meters
                                   //fill in other properties as needed

        var props = new DeviceLocationProperties();
        props.AccuracyThreshold = 10;   // meters

        // jump to the background thread
        //Note: Run within a QueuedTask
        //open the device
        DeviceLocationService.Instance.Open(newSrc, props);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetSource()
      #region Get the Current Device Location Source
      {
        var source = DeviceLocationService.Instance.GetSource();
        if (source == null)
        {
          //There is no current source
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.Close()
      #region Close the Current Device Location Source
      {
        //Is there a current device source?
        var src = DeviceLocationService.Instance.GetSource();
        if (src == null)
          return;//no current source

        //Note: Run within a QueuedTask
        DeviceLocationService.Instance.Close();
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.IsDeviceConnected()
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetSource()
      // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource
      // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.GetSpatialReference()
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetProperties()
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.AccuracyThreshold
      #region Get Current Device Location Source and Properties
      {
        // Check if a device is connected
        bool isConnected = DeviceLocationService.Instance.IsDeviceConnected();
        if (!isConnected)
          return; // no device connected
                  // Get the current device location source
        var src = DeviceLocationService.Instance.GetSource();
        // Check if the source is a SerialPortDeviceLocationSource
        //Set values for the SerialPortDeviceLocationSource
        if (src is SerialPortDeviceLocationSource serialPortSrc)
        {
          var port = serialPortSrc.ComPort;
          var antennaHeight = serialPortSrc.AntennaHeight;
          var dataBits = serialPortSrc.DataBits;
          var baudRate = serialPortSrc.BaudRate;
          var parity = serialPortSrc.Parity;
          var stopBits = serialPortSrc.StopBits;

          // retrieving spatial reference needs the MCT
          var sr = await QueuedTask.Run(() =>
          {
            return serialPortSrc.GetSpatialReference();
          });

        }
        //Get current device location properties being used.
        var dlProps = DeviceLocationService.Instance.GetProperties();
        var accuracy = dlProps.AccuracyThreshold;
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetProperties()
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.AccuracyThreshold
      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.UpdateProperties(ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties)
      #region Update Properties on the Current Device Location Source
      {
        //Note: Run within QueuedTask
        // Get the current device location properties
        var dlProps = DeviceLocationService.Instance.GetProperties();
        //Change the accuracy threshold
        dlProps.AccuracyThreshold = 22.5; // meters
        // Update the properties on the device location source
        DeviceLocationService.Instance.UpdateProperties(dlProps);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEvent
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEventArgs
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEventArgs.DeviceLocationProperties
      #region Subscribe to DeviceLocationPropertiesUpdated event
      {
        DeviceLocationPropertiesUpdatedEvent.Subscribe(OnDeviceLocationPropertiesUpdated);
        // Event handler for DeviceLocationPropertiesUpdated event.
        static void OnDeviceLocationPropertiesUpdated(DeviceLocationPropertiesUpdatedEventArgs args)
        {
          if (args == null)
            return;

          var properties = args.DeviceLocationProperties;
          //  TODO - something with the updated properties
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEvent
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEventArgs
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEventArgs.DeviceLocationSource
      #region Subscribe to DeviceLocationSourceChanged event
      {
        DeviceLocationSourceChangedEvent.Subscribe(OnDeviceLocationSourceChanged);
        // Event handler for DeviceLocationSourceChanged event.
        void OnDeviceLocationSourceChanged(DeviceLocationSourceChangedEventArgs args)
        {
          if (args == null)
            return;

          var source = args.DeviceLocationSource;

          //  TODO - something with the updated source properties
        }
      }
      #endregion

      #region ProSnippet Group: Map Device Location Options
      #endregion

      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.IsDeviceLocationEnabled
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.SetDeviceLocationEnabled(System.Boolean)
      #region Enable/Disable Current Device Location Source For the Map
      {
        bool enabled = MapDeviceLocationService.Instance.IsDeviceLocationEnabled;
        await QueuedTask.Run(() =>
        {
          MapDeviceLocationService.Instance.SetDeviceLocationEnabled(!enabled);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.GetDeviceLocationOptions()
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.DeviceLocationVisibility
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.NavigationMode
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.TrackUpNavigation
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.ShowAccuracyBuffer
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MappingDeviceLocationNavigationMode
      #region Get Current Map Device Location Options
      {
        //Gets the current device location options used by the MapDeviceLocationService
        var options = MapDeviceLocationService.Instance.GetDeviceLocationOptions();
        //Device location visibility on the map
        var visibility = options.DeviceLocationVisibility;
        //MappingDeviceLocationNavigationMode
        var navMode = options.NavigationMode;
        //Heading of the location from the device points to the top of the screen
        var trackUp = options.TrackUpNavigation;
        //Show accuracy buffer on the map
        var showBuffer = options.ShowAccuracyBuffer;
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.IsDeviceLocationEnabled
      #region Check if The Current Device Location Is Enabled On The Map
      {
        //Checks if the current device location source is enabled on the map
        if (MapDeviceLocationService.Instance.IsDeviceLocationEnabled)
        {
          //The Device Location Source is Enabled
        }
      }
      #endregion

      #region ProSnippet Group: Map Device Location Options
      #endregion

      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.SetDeviceLocationOptions(ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions)
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.#ctor
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MappingDeviceLocationNavigationMode
      #region Set Current Map Device Location Options
      {
        //Note: Run within a QueuedTask
        //Check there is a source first...
        if (DeviceLocationService.Instance.GetSource() == null)
          //Setting DeviceLocationOptions w/ no Device Location Source
          //Will throw an InvalidOperationException
          return;

        if (!MapDeviceLocationService.Instance.IsDeviceLocationEnabled)
          //Setting DeviceLocationOptions w/ no Device Location Enabled
          //Will throw an InvalidOperationException
          return;

        MapDeviceLocationService.Instance.SetDeviceLocationOptions(
          new MapDeviceLocationOptions()
          {
            DeviceLocationVisibility = true,
            NavigationMode = MappingDeviceLocationNavigationMode.KeepAtCenter,
            TrackUpNavigation = true
          });
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.ZoomOrPanToCurrentLocation(System.Boolean)
      #region Zoom/Pan The Map To The Most Recent Location
      {
        //Note: Run within a QueuedTask
        if (!MapDeviceLocationService.Instance.IsDeviceLocationEnabled)
          //Calling ZoomOrPanToCurrentLocation w/ no Device Location Enabled
          //Will throw an InvalidOperationException
          return;

        // true for zoom, false for pan
        MapDeviceLocationService.Instance.ZoomOrPanToCurrentLocation(true);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetCurrentSnapshot()
      #region Add the Most Recent Location To A Graphics Layer
      {
        //Note: Run within a QueuedTask
        // get the last location
        var pt = DeviceLocationService.Instance.GetCurrentSnapshot()?.GetPositionAsMapPoint();
        if (pt != null)
        {
          //Create a point symbol
          var ptSymbol = SymbolFactory.Instance.ConstructPointSymbol(
                            CIMColor.CreateRGBColor(125, 125, 0), 10, SimpleMarkerStyle.Triangle);
          //Add a graphic to the graphics layer
          graphicsLayer.AddElement(pt, ptSymbol);
          //unselect it
          graphicsLayer.ClearSelection();
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.GetDeviceLocationOptions()
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.DeviceLocationVisibility
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.NavigationMode
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MappingDeviceLocationNavigationMode
      // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.SetDeviceLocationOptions(ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions)
      #region Set map view to always be centered on the device location
      {
        // Get the MapDeviceLocationOptions currently used by the MapDeviceLocationService

        var currentOptions = MapDeviceLocationService.Instance.GetDeviceLocationOptions();
        if (currentOptions == null)
          return;
        // Set the device location visibility on the map to true
        currentOptions.DeviceLocationVisibility = true;
        //Set the navigation mode to keep the device location at the center of the map
        currentOptions.NavigationMode = MappingDeviceLocationNavigationMode.KeepAtCenter;
        //Note: Run within a QueuedTask
        //Sets the MapDeviceLocationOptions to be updates with these values
        MapDeviceLocationService.Instance.SetDeviceLocationOptions(currentOptions);
      }
      #endregion


      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEvent
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEvent.Subscribe(Action<ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEventArgs>, System.Boolean)
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEventArgs
      // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEventArgs.Snapshot
      // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot
      // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.GetPositionAsMapPoint()
      // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.Altitude
      // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.DateTime
      // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.VDOP
      // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.HDOP
      #region Subscribe to Location Snapshot event
      {
        SnapshotChangedEvent.Subscribe(OnSnapshotChanged);
        /// Handles changes to a snapshot by processing the provided snapshot data.
        void OnSnapshotChanged(SnapshotChangedEventArgs args)
        {
          if (args == null)
            return;

          var snapshot = args.Snapshot as NMEASnapshot;
          if (snapshot == null)
            return;
          //Note: Run within a QueuedTask
          var pt = snapshot.GetPositionAsMapPoint();
          if (pt?.IsEmpty ?? true)
            return;

          // access properties
          var alt = snapshot.Altitude;
          var dt = snapshot.DateTime;
          var vdop = snapshot.VDOP;
          var hdop = snapshot.HDOP;
          // etc
          //TODO: use the snapshot
        }
      }
      #endregion

      #region ProSnippet Group: Feature Masking
      #endregion

      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetDrawingOutline(System.Int64, ArcGIs.Desktop.Mapping.MapView, ArcGIS.Desktop.Mapping.DrawingOutlineType)
      // cref: ArcGIS.Desktop.Mapping.DrawingOutlineType
      #region Get the Mask Geometry for a Feature
      {
        if (featureLayer == null)
          return;

        var mv = MapView.Active;
        //Note: Run within a QueuedTask
        using (var table = featureLayer.GetTable())
        {
          using (var rc = table.Search())
          {
            //get the first feature...
            //...assuming at least one feature gets retrieved
            rc.MoveNext();
            var oid = rc.Current.GetObjectID();

            //Use DrawingOutlineType.BoundingEnvelope to retrieve a generalized
            //mask geometry or "Box". The mask will be in the same SpatRef as the map
            var mask_geom = featureLayer.GetDrawingOutline(oid, mv, DrawingOutlineType.Exact);

            //TODO - use the mask geometry...
          }
        }
      }
      #endregion
    }
  }
}

