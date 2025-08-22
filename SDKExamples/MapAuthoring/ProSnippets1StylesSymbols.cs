/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;


namespace MapAuthoring.ProSnippets
{
  public static class ProSnippets1StylesSymbols
  {
    //style management
    #region ProSnippet Group: Style Management
    #endregion
    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
    #region How to get a style in project by name
    /// <summary>
    /// Retrieves a specific style from the current project by its name.
    /// </summary>
    public static void GetStyleInProjectByName()
    {
      //Get all styles in the project
      var ProjectStyles = Project.Current.GetItems<StyleProjectItem>();

      //Get a specific style in the project by name
      StyleProjectItem style = ProjectStyles.First(x => x.Name == "NameOfTheStyle");

    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.StyleHelper.CreateStyle(ArcGIS.Desktop.Core.Project,System.String)
    #region How to create a new style
    /// <summary>
    /// Creates a new style file (.stylx) at the specified location.
    /// </summary>
    public static async void CreateNewStyle()
    {
      //Full path for the new style file (.stylx) to be created
      string styleToCreate = @"C:\Temp\NewStyle.stylx";
      await QueuedTask.Run(() => StyleHelper.CreateStyle(Project.Current, styleToCreate));
    }
    #endregion
    // cref:ArcGIS.Desktop.Mapping.StyleHelper.AddStyle(ArcGIS.Desktop.Core.Project,System.String)
    #region How to add a style to project
    /// <summary>
    /// Adds ArcGIS Pro system styles and custom styles to the current ArcGIS Pro project.
    /// </summary>

    public static async void AddStyleToProject()
    {
      //For ArcGIS Pro system styles, just pass in the name of the style to add to the project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, "3D Vehicles"));

      //For custom styles, pass in the full path to the style file on disk
      string customStyleToAdd = @"C:\Temp\CustomStyle.stylx";
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, customStyleToAdd));
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.StyleHelper.RemoveStyle(ArcGIS.Desktop.Core.Project,System.String)
    #region How to remove a style from project
    /// <summary>
    /// Removes a style from the current ArcGIS Pro project.
    /// </summary>

    public static async void RemoveStyleFromProject()
    {
      //For ArcGIS Pro system styles, just pass in the name of the style to remove from the project
      await QueuedTask.Run(() => StyleHelper.RemoveStyle(Project.Current, "3D Vehicles"));

      //For custom styles, pass in the full path to the style file on disk
      string customStyleToAdd = @"C:\Temp\CustomStyle.stylx";
      await QueuedTask.Run(() => StyleHelper.RemoveStyle(Project.Current, customStyleToAdd));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.AddItem(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItem)
    #region How to add a style item to a style
    /// <summary>
    /// Adds a style item to the specified style asynchronously.
    /// </summary>
    /// <param name="style">The style project item to which the style item will be added. Cannot be <see langword="null"/>.</param>
    /// <param name="itemToAdd">The style item to add to the style. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> or <paramref name="itemToAdd"/> is <see langword="null"/>.</exception>
    public static Task AddStyleItemAsync(StyleProjectItem style, StyleItem itemToAdd)
    {
      return QueuedTask.Run(() =>
      {
        if (style == null || itemToAdd == null)
          throw new System.ArgumentNullException();

        //Add the item to style
        style.AddItem(itemToAdd);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.RemoveItem(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItem)
    #region How to remove a style item from a style
    /// <summary>
    /// Removes a specified style item from a given style asynchronously.
    /// </summary>
    /// <param name="style">The style project item from which the style item will be removed. Cannot be <see langword="null"/>.</param>
    /// <param name="itemToRemove">The style item to remove from the specified style. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> or <paramref name="itemToRemove"/> is <see langword="null"/>.</exception>
    public static Task RemoveStyleItemAsync(StyleProjectItem style, StyleItem itemToRemove)
    {
      return QueuedTask.Run(() =>
      {
        if (style == null || itemToRemove == null)
          throw new System.ArgumentNullException();

        //Remove the item from style
        style.RemoveItem(itemToRemove);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.CanUpgrade
    #region How to determine if a style can be upgraded

    /// <summary>
    /// Determines whether the specified style file can be upgraded.
    /// </summary>
    /// <param name="stylePath">The full path to the style file on disk.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the style can be
    /// upgraded;  otherwise, <see langword="false"/>.</returns>
    public static async Task<bool> CanUpgradeStyleAsync(string stylePath)
    {
      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //returns true if style can be upgraded
      return style.CanUpgrade;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.IsReadOnly
    #region How to determine if a style is read-only
    //Pass in the full path to the style file on disk
    /// <summary>
    /// Determines whether the specified style file is read-only.
    /// </summary>
    /// <param name="stylePath">The full path to the style file on disk.</param>
    /// <returns>A <see langword="true"/> value if the style file is read-only; otherwise, <see langword="false"/>.</returns>
    public static async Task<bool> IsReadOnly(string stylePath)
    {
      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //returns true if style is read-only
      return style.IsReadOnly;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.IsCurrent
    #region How to determine if a style is current
    //Pass in the full path to the style file on disk
    /// <summary>
    /// Determines whether the specified style file is compatible with the current version of ArcGIS Pro.
    /// </summary>
    /// <param name="stylePath">The full path to the style file on disk.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the style file
    /// matches the current version of ArcGIS Pro; otherwise, <see langword="false"/>.</returns>
    public static async Task<bool> IsCurrent(string stylePath)
    {
      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //returns true if style matches the current Pro version
      return style.IsCurrent;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem.CanUpgrade
    // cref: ArcGIS.Desktop.Mapping.StyleHelper.UpgradeStyle(ArcGIS.Desktop.Mapping.StyleProjectItem)
    #region How to upgrade a style
    //Pass in the full path to the style file on disk
    /// <summary>
    /// Upgrades a style file to the latest format if it is eligible for an upgrade.
    /// </summary>
    /// <param name="stylePath">The full path to the style file on disk.</param>
    /// <returns><see langword="true"/> if the style file was successfully upgraded; otherwise, <see langword="false"/>. </returns>
    public static async Task<bool> UpgradeStyleAsync(string stylePath)
    {
      bool success = false;

      //Add the style to the current project
      await QueuedTask.Run(() => StyleHelper.AddStyle(Project.Current, stylePath));
      StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == stylePath);

      //Verify that style can be upgraded
      if (style.CanUpgrade)
      {
        success = await QueuedTask.Run(() => StyleHelper.UpgradeStyle(style));
      }
      //return true if style was upgraded
      return success;
    }
    #endregion

    //construct point symbol
    #region ProSnippet Group: Symbols
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double)
    // cref: ArcGIS.Core.CIM.CIMPointSymbol
    #region How to construct a point symbol of a specific color and size
    /// <summary>
    /// Constructs a point symbol with a specified color and size.
    /// </summary>
    /// <returns></returns>
    public static async Task ConstructPointSymbolColorSize()
    {
      await QueuedTask.Run(() =>
      {
        CIMPointSymbol pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 10.0);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
    // cref: ArcGIS.Core.CIM.CIMPointSymbol
    #region How to construct a point symbol of a specific color, size and shape
    /// <summary>
    /// Constructs a point symbol with a specified color, size, and shape.
    /// </summary>
    /// <returns></returns>
    public static async Task ConstructPointSymbolColorSizeShape()
    {
      await QueuedTask.Run(() =>
      {
        CIMPointSymbol starPointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 10.0, SimpleMarkerStyle.Star);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
    // cref: ArcGIS.Core.CIM.CIMMarker
    // cref: ArcGIS.Core.CIM.CIMPointSymbol
    #region How to construct a point symbol from a marker
    /// <summary>
    /// Constructs a point symbol using a marker with predefined attributes.
    /// </summary>
    /// <returns></returns>
    public static async Task ConstructPointSymbolMarker()
    {
      await QueuedTask.Run(() =>
      {
        CIMMarker marker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.GreenRGB, 8.0, SimpleMarkerStyle.Pushpin);
        CIMPointSymbol pointSymbolFromMarker = SymbolFactory.Instance.ConstructPointSymbol(marker);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarkerFromFile(System.String)
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
    // cref: ArcGIS.Core.CIM.CIMMarker
    // cref: ArcGIS.Core.CIM.CIMPointSymbol
    #region How to construct a point symbol from a file on disk
    /// <summary>
    /// How to construct a point symbol from a file on disk.
    /// </summary>
    /// <returns></returns>
    public static async Task ConstructPointSymbolFileFromDesk()
    {
      //The following file formats can be used to create the marker: DAE, 3DS, FLT, EMF, JPG, PNG, BMP, GIF
      CIMMarker markerFromFile = await QueuedTask.Run(() => SymbolFactory.Instance.ConstructMarkerFromFile(@"C:\Temp\fileName.dae"));

      CIMPointSymbol pointSymbolFromFile = SymbolFactory.Instance.ConstructPointSymbol(markerFromFile);

    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarkerFromStream(System.IO.Stream)
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
    // cref: ArcGIS.Core.CIM.CIMMarker
    // cref: ArcGIS.Core.CIM.CIMPointSymbol
    #region How to construct a point symbol from a in memory graphic
    /// <summary>
    /// How to construct a point symbol from an in-memory graphic.
    /// </summary>
    public static void ConstructPointSymbolFromMarkerStream()
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
    /// <summary>
    /// Constructs a polygon symbol with a specified color and fill style.
    /// </summary>
    public static void ConstructPolygonSymbolColorFill()
    {
      CIMPolygonSymbol polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid);
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double, ArcGIS.Desktop.Mapping.SimpleLineStyle)
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol(ArcGIS.Core.CIM.CIMColor,ArcGIS.Desktop.Mapping.SimpleFillStyle,ArcGIS.Core.CIM.CIMStroke)
    // cref: ArcGIS.Core.CIM.CIMStroke
    // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
    #region How to construct a polygon symbol of specific color, fill style and outline
    /// <summary>
    /// Constructs a polygon symbol with a specified fill color, fill style, and outline.
    /// </summary>
    public static void ConstructPolygonSymbolColorFillOutline()
    {
      CIMStroke outline = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);
      CIMPolygonSymbol fillWithOutline = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol(ArcGIS.Core.CIM.CIMColor,ArcGIS.Desktop.Mapping.SimpleFillStyle,ArcGIS.Core.CIM.CIMStroke)
    // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
    #region How to construct a polygon symbol without an outline
    /// <summary>
    /// Constructs a polygon symbol with a solid fill and no outline.
    /// </summary>

    public static void ConstructPolygonSymbolNoOutline()
    {
      CIMPolygonSymbol fillWithoutOutline = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, null);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructLineSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
    // cref: ArcGIS.Core.CIM.CIMLineSymbol
    #region How to construct a line symbol of specific color, size and line style
    /// <summary>
    /// Demonstrates how to construct a line symbol with a specific color, size, and style.
    /// </summary>
    public static void ConstructLineSymbolColorSizeStyle()
    {
      CIMLineSymbol lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlueRGB, 4.0, SimpleLineStyle.Solid);
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double)
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructLineSymbol(ArcGIS.Core.CIM.CIMStroke)
    // cref: ArcGIS.Core.CIM.CIMStroke
    // cref: ArcGIS.Core.CIM.CIMLineSymbol
    #region How to construct a line symbol from a stroke
    /// <summary>
    /// Demonstrates how to construct a line symbol from a stroke using the ArcGIS Pro SDK.
    /// </summary>
    public static void ConstructLineSymbolFromStroke()
    {
      CIMStroke stroke = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2.0);
      CIMLineSymbol lineSymbolFromStroke = SymbolFactory.Instance.ConstructLineSymbol(stroke);
    }
    #endregion
    //multilayer symbols
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double)
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(ArcGIS.Core.CIM.CIMColor,System.Double, ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
    // cref: ArcGIS.Core.CIM.CIMMarkerPlacementOnVertices
    // cref: ArcGIS.Core.CIM.CIMMarkerStrokePlacement.AngleToLine
    // cref: ArcGIS.Core.CIM.CIMMarkerPlacementOnVertices.PlaceOnEndPoints
    // cref: ArcGIS.Core.CIM.CIMMarkerStrokePlacement.Offset
    // cref: ArcGIS.Core.CIM.CIMLineSymbol
    // cref: ArcGIS.Core.CIM.CIMSymbolLayer
    #region How to construct a multilayer line symbol with circle markers on the line ends
    /// <summary>
    /// How to construct a multilayer line symbol with circle markers on the line ends.
    /// </summary>
    public static void ConstructMultilayerLineSymbolCircleMarkersLineEnds()
    {
      QueuedTask.Run(() => {
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
      });
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
    /// <summary>
    /// Constructs a multilayer line symbol with an arrowhead at the end of the line.
    /// </summary>

    public static void ConstructMultilayerLineSymbolArrowHeadEnds()
    {
      QueuedTask.Run(() => {
        var markerTriangle = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.RedRGB, 12, SimpleMarkerStyle.Triangle);
        markerTriangle.Rotation = -90; // or -90
        markerTriangle.MarkerPlacement = new CIMMarkerPlacementOnLine() { AngleToLine = true, RelativeTo = PlacementOnLineRelativeTo.LineEnd };

        var lineSymbolWithArrow = new CIMLineSymbol()
        {
          SymbolLayers = new CIMSymbolLayer[2] { markerTriangle,
                    SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 2)
                }
        };
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference(ArcGIS.Core.CIM.CIMSymbol)
    // cref: ArcGIS.Core.CIM.CIMSymbolReference
    #region How to get symbol reference from a symbol
    /// <summary>
    /// Demonstrates how to create a symbol reference from a polygon symbol.
    /// </summary>
    public static void GetSymbolReference()
    {
      CIMPolygonSymbol symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB);

      //Get symbol reference from the symbol
      CIMSymbolReference symbolReference = symbol.MakeSymbolReference();
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(System.Int32, system.String, System.String, System.Int32, ArcGIS.Core.CIM.CIMColor)
    // cref: ArcGIS.Core.CIM.CIMCharacterMarker
    // cref: ArcGIS.Core.CIM.CIMCharacterMarker.Symbol
    // cref: ArcGIS.Core.CIM.CIMMultiLayerSymbol.SymbolLayers
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
    #region Modify a point symbol created from a character marker 
    /// <summary>
    /// Creates and modifies a point symbol based on a character marker.
    /// </summary>
    public static void GetSymbol()
    {
      //create marker from the Font, char index,size,color
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
    /// <summary>
    /// Retrieves a list of available fonts and their associated styles.
    /// </summary>

    public static void GetListOfAvailableFonts()
    {
      QueuedTask.Run(() => {
        //returns a tuple per font: (string fontName, List<string> fontStyles)
        var fonts = SymbolFactory.Instance.GetAvailableFonts();
        foreach (var font in fonts)
        {
          var styles = string.Join(",", font.fontStyles);
          System.Diagnostics.Debug.WriteLine($"{font.fontName}, styles: {styles}");
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.DefaultFont
    // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultFont(System.String)
    // cref: ArcGIS.Desktop.Core.TextAndGraphicsElementsOptions.SetDefaultFont(System.String, System.String)
    #region Get/Set Default Font
    /// <summary>
    /// Retrieves the current default font used by the application and sets a new default font.
    /// </summary>
    public static void GetSetDefaultFont()
    { 
      QueuedTask.Run(() => {
  var def_font = SymbolFactory.Instance.DefaultFont;
      System.Diagnostics.Debug.WriteLine($"{def_font.fontName}, styles: {def_font.styleName}");

        //set default font - set through application options
        //Must use QueuedTask
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultFont("tahoma");
        ApplicationOptions.TextAndGraphicsElementsOptions.SetDefaultFont("tahoma","bold");
        });
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
    /// <summary>
    /// Demonstrates various ways to construct text symbols using the <see cref="ArcGIS.Desktop.Mapping.SymbolFactory"/>
    /// class.
    /// </summary>

    public static void ContructTextSymbolWithOptions()
    {
      QueuedTask.Run(() =>
      {
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

      });

    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.#ctor
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
    // cref: ArcGIS.Desktop.Mapping.StyleItem.PatchHeight
    // cref: ArcGIS.Desktop.Mapping.StyleItem.PatchWidth
    // cref: ArcGIS.Desktop.Mapping.StyleItem.PreviewImage
    #region Create a Swatch for a given symbol
    /// <summary>
    /// Creates a swatch image for a given symbol.
    /// </summary>

    public static Task CreateSymbolSwatch()
    {
        return QueuedTask.Run(() => {
          //Note: call within QueuedTask.Run()
          CIMSymbol symbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.GreenRGB, 1.0, SimpleMarkerStyle.Circle);
            //You can generate a swatch for a text symbols also.
            var si = new SymbolStyleItem()
            {
                Symbol = symbol,
                PatchHeight = 64,
                PatchWidth = 64
            };
            return si.PreviewImage;
            #endregion
        });
    }
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.GenerateImage(ArcGIS.Core.CIM.CIMPointSymbol,ArcGIS.Desktop.Mapping.OutputImageFormat,System.Double,System.Boolean,System.Double,System.Int64,System.Int64,ArcGIS.Core.CIM.CIMColor)
    // cref: ArcGIS.Desktop.Mapping.OutputImageFormat
    // cref: ArcGIS.Desktop.Mapping.OutputImageFormat.SVG
    // cref: ArcGIS.Desktop.Mapping.OutputImageFormat.PNG
    #region Convert Point Symbol to SVG
    /// <summary>
    /// Creates an SVG image representation of a point symbol and saves it to a temporary file.
    /// </summary>

    public static void CreateImageOfPointSymbol()
    {
      QueuedTask.Run( () => {
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
        string path_svg = Path.Combine(Path.GetTempPath() + fileName);

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
      });     
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.SymbolFactory.GenerateImage(ArcGIS.Core.CIM.CIMPointSymbol,ArcGIS.Desktop.Mapping.OutputImageFormat,System.Double,System.Boolean,System.Double,System.Int64,System.Int64,ArcGIS.Core.CIM.CIMColor)
    // cref: ArcGIS.Desktop.Mapping.OutputImageFormat
    // cref: ArcGIS.Desktop.Mapping.OutputImageFormat.PNG
    #region Convert Point Symbol to PNG
    /// <summary>
    /// Converts a point symbol to a PNG image and saves it to a file.
    /// </summary>
    public static void ConvertPointSymbolToPNG()
    {
      QueuedTask.Run(() => {  //Create a point symbol
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
        string path_png = Path.Combine(Path.GetTempPath() + fileName);

        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

        using (var fileStream = new System.IO.FileStream(
          path_png, System.IO.FileMode.Create))
        {
          encoder.Save(fileStream);
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CanLookupSymbol()
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.LookupSymbol(System.Int64, ArcGIS.Desktop.Mapping.MapView)
    #region Lookup Symbol
    /// <summary>
    /// Performs a symbol lookup for the first selected feature in the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer from which the selection and symbol lookup will be performed. Cannot be null.</param>
    public static void SymbolLookup(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() => {//Get the selection
        var selection = featureLayer.GetSelection();
        //Get the first Object ID
        var firstOID = selection.GetObjectIDs().FirstOrDefault();
        //Determine whether the layer's renderer type supports symbol lookup.
        if (featureLayer.CanLookupSymbol())
        {
          //Looks up the symbol for the corresponding feature identified by the object id.
          var symbol = featureLayer.LookupSymbol(firstOID, MapView.Active);
          var jSon = symbol.ToJson(); //Create a JSON encoding of the symbol
                                      //Do something with symbol
        }
      });
    }
    #endregion

    #region ProSnippet Group: Symbol Search
    #endregion
    //symbol search
    // cref: ArcGIS.Desktop.Mapping.StyleHelper.LookupItem(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
    #region How to search for a specific item in a style
    /// <summary>
    ///  How to search for a specific point symbol in a style. 
    /// </summary>
    /// <param name="style"></param>
    /// <param name="key"></param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static Task<SymbolStyleItem> GetSymbolFromStyleAsync(StyleProjectItem style, string key)
    {
      return QueuedTask.Run(() =>
      {
        if (style == null)
            throw new System.ArgumentNullException();

        //Search for a specific point symbol in style
        SymbolStyleItem item = (SymbolStyleItem)style.LookupItem(StyleItemType.PointSymbol, key);
        return item;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchSymbols(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
    #region How to search for point symbols in a style
    /// <summary>
    /// Searches for point symbols in the specified style based on the provided search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter point symbols. If <see langword="null"/> or empty, all point symbols are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="ArcGIS.Desktop.Mapping.SymbolStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<SymbolStyleItem>> GetPointSymbolsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for point symbols
      return QueuedTask.Run(() => style.SearchSymbols(StyleItemType.PointSymbol, searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchSymbols(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
    #region How to search for line symbols in a style
    public static Task<IList<SymbolStyleItem>> GetLineSymbolsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for line symbols
      return QueuedTask.Run(() => style.SearchSymbols(StyleItemType.LineSymbol, searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchSymbols(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem
    #region How to search for polygon symbols in a style
    /// <summary>
    /// Asynchronously retrieves a list of polygon symbols from the specified style based on a search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter polygon symbols. If <see langword="null"/> or empty, all polygon symbols are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="SymbolStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static async Task<IList<SymbolStyleItem>> GetPolygonSymbolsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for polygon symbols
      return await QueuedTask.Run(() => style.SearchSymbols(StyleItemType.PolygonSymbol, searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColors(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.ColorStyleItem
    #region How to search for colors in a style
    /// <summary>
    /// Asynchronously retrieves a list of color style items from the specified style project item that match the given
    /// search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The string used to filter color style items. If <paramref name="searchString"/> is empty or <see
    /// langword="null"/>,  all color style items in the style project item are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
    /// cref="ArcGIS.Desktop.Mapping.ColorStyleItem"/> objects that match the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static async Task<IList<ColorStyleItem>> GetColorsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for colors
      return await QueuedTask.Run(() => style.SearchColors(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchColorRamps(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
    #region How to search for color ramps in a style
    /// <summary>
    /// Asynchronously retrieves a list of color ramp style items from the specified style project item that match the
    /// given search string.
    /// </summary>
    /// <param name="style">The style project item to search within. This can represent predefined styles such as  "ColorBrewer Schemes
    /// (RGB)" or "ArcGIS 2D".</param>
    /// <param name="searchString">The search string used to filter color ramp style items. Examples include  "Spectral (7 Classes)", "Pastel 1 (3
    /// Classes)", or "Red-Gray (10 Classes)".</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
    /// cref="ArcGIS.Desktop.Mapping.ColorRampStyleItem"/> objects that match the search criteria. If no matching items
    /// are found, the list will be empty.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static async Task<IList<ColorRampStyleItem>> GetColorRampsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      //StyleProjectItem can be "ColorBrewer Schemes (RGB)", "ArcGIS 2D"...
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for color ramps
      //Color Ramp searchString can be "Spectral (7 Classes)", "Pastel 1 (3 Classes)", "Red-Gray (10 Classes)"..
      return await QueuedTask.Run(() => style.SearchColorRamps(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchNorthArrows(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem
    #region How to search for north arrows in a style
    /// <summary>
    /// Retrieves a list of north arrow style items from the specified style based on a search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter north arrow style items. If <see langword="null"/> or empty, all north arrow
    /// items are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="NorthArrowStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<NorthArrowStyleItem>> GetNorthArrowsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for north arrows
      return QueuedTask.Run(() => style.SearchNorthArrows(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchScaleBars(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem
    #region How to search for scale bars in a style
    /// <summary>
    /// Retrieves a list of scale bar style items from the specified style based on the provided search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter scale bar style items. If <see langword="null"/> or empty, all scale bar items
    /// are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="ScaleBarStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<ScaleBarStyleItem>> GetScaleBarsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for scale bars
      return QueuedTask.Run(() => style.SearchScaleBars(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchLabelPlacements(ArcGIS.Desktop.Mapping.StyleProjectItem,ArcGIS.Desktop.Mapping.StyleItemType,System.String)
    // cref: ArcGIS.Desktop.Mapping.LabelPlacementStyleItem
    #region How to search for label placements in a style
    /// <summary>
    /// Searches for label placement style items in the specified style project item that match the given search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter label placement style items. If <see langword="null"/> or empty, all label
    /// placements are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="LabelPlacementStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<LabelPlacementStyleItem>> GetLabelPlacementsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      //Search for standard label placement
      return QueuedTask.Run(() => style.SearchLabelPlacements(StyleItemType.StandardLabelPlacement, searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchLegends(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.LegendStyleItem
    #region How to search for legends in a style
    /// <summary>
    /// Searches for legend style items in the specified style project item that match the given search string.
    /// </summary>
    /// <param name="style">The style project item to search within. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The string used to filter legend style items. If <see langword="null"/> or empty, all legend style items are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="LegendStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<LegendStyleItem>> GetLegendFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() => style.SearchLegends(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchLegendItems(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.LegendItemStyleItem
    #region How to search for legend items in a style
    /// <summary>
    /// Asynchronously retrieves a list of legend items from the specified style based on a search string.
    /// </summary>
    /// <param name="style">The style project item to search for legend items. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter legend items. If <see langword="null"/> or empty, all legend items are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="ArcGIS.Desktop.Mapping.LegendItemStyleItem"/> objects matching the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<LegendItemStyleItem>> GetLegendItemsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() => style.SearchLegendItems(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchGrids(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.GridStyleItem
    #region How to search for grids in a style
    /// <summary>
    /// Asynchronously retrieves a list of grid style items from the specified style project item that match the given
    /// search string.
    /// </summary>
    /// <param name="style">The style project item to search for grid style items. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter grid style items. If <see langword="null"/> or empty, all grid style items are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="GridStyleItem"/> objects that match the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<GridStyleItem>> GetGridsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() => style.SearchGrids(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchMapSurrounds(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.MapSurroundStyleItem
    #region How to search for map surrounds in a style
    /// <summary>
    /// Searches for map surround style items in the specified style project item that match the given search string.
    /// </summary>
    /// <param name="style">The style project item to search. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter map surround style items. If <see langword="null"/> or empty, all items are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="MapSurroundStyleItem"/> objects that match the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<MapSurroundStyleItem>> GetMapSurroundsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() => style.SearchMapSurrounds(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchTableFrames(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.TableFrameStyleItem
    #region How to search for table frames in a style
    /// <summary>
    /// Searches for table frame style items within the specified style project item that match the given search string.
    /// </summary>
    /// <param name="style">The style project item to search. Cannot be <see langword="null"/>.</param>
    /// <param name="searchString">The search string used to filter table frame style items. If <see langword="null"/> or empty, all table frame
    /// style items are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="TableFrameStyleItem"/> objects that match the search criteria.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="style"/> is <see langword="null"/>.</exception>
    public static Task<IList<TableFrameStyleItem>> GetTableFramesFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() => style.SearchTableFrames(searchString));
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleHelper.SearchTableFrameFields(ArcGIS.Desktop.Mapping.StyleProjectItem,System.String)
    // cref: ArcGIS.Desktop.Mapping.TableFrameFieldStyleItem
    #region How to search for table frame fields in a style
    public static Task<IList<TableFrameFieldStyleItem>> GetTableFrameFieldsFromStyleAsync(StyleProjectItem style, string searchString)
    {
      if (style == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() => style.SearchTableFrameFields(searchString));
    }
    #endregion


    #region ProSnippet Group: Feature Layer Symbology
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer()
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer)
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.UsesRealWorldSymbolSizes
    // cref: ArcGIS.Core.CIM.CIMSimpleRenderer
    #region How to set symbol for a feature layer symbolized with simple renderer
    /// <summary>
    /// Updates the symbology of a feature layer by applying a specified symbol to its simple renderer.
    /// </summary>
    /// <param name="ftrLayer">The feature layer whose symbology will be updated. Cannot be <see langword="null"/>.</param>
    /// <param name="symbolToApply">The symbol to apply to the feature layer's simple renderer. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="ftrLayer"/> or <paramref name="symbolToApply"/> is <see langword="null"/>.</exception>
    public static Task SetFeatureLayerSymbolAsync(FeatureLayer ftrLayer, CIMSymbol symbolToApply)
    {
      if (ftrLayer == null || symbolToApply == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() =>
      {

        //Get simple renderer from the feature layer
        CIMSimpleRenderer currentRenderer = ftrLayer.GetRenderer() as CIMSimpleRenderer;
        if (currentRenderer == null)
          return;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbolToApply.SetRealWorldUnits(ftrLayer.UsesRealWorldSymbolSizes);

        //Update the symbol of the current simple renderer
        currentRenderer.Symbol = symbolToApply.MakeSymbolReference();
        //Update the feature layer renderer
        ftrLayer.SetRenderer(currentRenderer);
      });
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
    /// <summary>
    /// Updates the symbol of a feature layer using a specified symbol from a style item.
    /// </summary>
    /// <param name="ftrLayer">The feature layer whose symbol will be updated. Cannot be <see langword="null"/>.</param>
    /// <param name="symbolItem">The symbol style item containing the symbol to apply. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="ftrLayer"/> or <paramref name="symbolItem"/> is <see langword="null"/>.</exception>
    public static Task SetFeatureLayerSymbolFromStyleItemAsync(
               FeatureLayer ftrLayer, SymbolStyleItem symbolItem)
    {
      if (ftrLayer == null || symbolItem == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() =>
      {
        //Get simple renderer from the feature layer
        CIMSimpleRenderer currentRenderer = ftrLayer.GetRenderer() as CIMSimpleRenderer;
        if (currentRenderer == null)
          return;
        //Get symbol from the SymbolStyleItem
        CIMSymbol symbol = symbolItem.Symbol;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbol.SetRealWorldUnits(ftrLayer.UsesRealWorldSymbolSizes);

        //Update the symbol of the current simple renderer
        currentRenderer.Symbol = symbol.MakeSymbolReference();
        //Update the feature layer renderer
        ftrLayer.SetRenderer(currentRenderer);
      });
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
    /// <summary>
    /// Applies a point symbol from a style to the specified feature layer.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the symbol will be applied. Must be a point feature layer.</param>
    /// <param name="symbolName">The name of the symbol to search for and apply. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task ApplySymbolToFeatureLayerAsync(FeatureLayer featureLayer, string symbolName)
    {
      return QueuedTask.Run(async () =>
      {
        //Get the ArcGIS 2D System style from the Project
        var arcGIS2DStyle =
  Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 2D");

        //Search for the symbolName style items within the ArcGIS 2D style project item.
        var items = await QueuedTask.Run(() =>
        arcGIS2DStyle.SearchSymbols(StyleItemType.PointSymbol, symbolName));

        //Gets the CIMSymbol
        CIMSymbol symbol = items.FirstOrDefault().Symbol;

        //Get the renderer of the point feature layer
        CIMSimpleRenderer renderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

        //Set symbol's real world setting to be the same as that of the feature layer
        symbol.SetRealWorldUnits(featureLayer.UsesRealWorldSymbolSizes);

        //Apply the symbol to the feature layer's current renderer
        renderer.Symbol = symbol.MakeSymbolReference();

        //Apply the renderer to the feature layer
        featureLayer.SetRenderer(renderer);
      });
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
    /// <summary>
    /// Applies a specified color ramp from a style to a feature layer using the provided fields.
    /// </summary>
    /// <param name="featureLayer">The feature layer to which the color ramp will be applied. Must not be null.</param>
    /// <param name="fields">A list of field names used to define unique values for the renderer. Must not be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the color ramp is applied to the
    /// feature layer.</returns>
    public static async Task ApplyColorRampAsync(FeatureLayer featureLayer, List<string> fields)
    {

        StyleProjectItem style =
            Project.Current.GetItems<StyleProjectItem>()
                .FirstOrDefault(s => s.Name == "ColorBrewer Schemes (RGB)");
        if (style == null) return;
        var colorRampList = await QueuedTask.Run(() => 
                    style.SearchColorRamps("Red-Gray (10 Classes)"));
        if (colorRampList == null || colorRampList.Count == 0) return;
        CIMColorRamp cimColorRamp = null;
        CIMRenderer renderer = null;
        await QueuedTask.Run(() =>
        {
            cimColorRamp = colorRampList[0].ColorRamp;
            var rendererDef = new UniqueValueRendererDefinition(fields, null, cimColorRamp);
            renderer = featureLayer?.CreateRenderer(rendererDef);
            featureLayer?.SetRenderer(renderer);
        });
    }
    #endregion
    }
}
