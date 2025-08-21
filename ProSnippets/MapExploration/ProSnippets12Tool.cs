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
using ArcGIS.Desktop.Framework.Controls;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Editing;
using ArcGIS.Desktop.Internal.Mapping.Controls;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapExploration.ProSnippets
{
  public static class ProSnippetsTool
  {
    #region ProSnippet Group: Tools
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.SketchSymbol
    #region Change symbol for a sketch tool
    /// <summary>
    /// Represents a map tool that allows users to sketch a rectangle geometry on the map with a customizable symbol.
    /// </summary>
    public class SketchTool_WithSymbol : MapTool
    {
      public SketchTool_WithSymbol()
      {
        IsSketchTool = true;
        SketchOutputMode = SketchOutputMode.Map; //Changing the Sketch Symbol is only supported with map sketches.
        SketchType = SketchGeometryType.Rectangle;
      }

      protected override Task OnToolActivateAsync(bool hasMapViewChanged)
      {
        return QueuedTask.Run(() =>
        {
          //Set the Sketch Symbol if it hasn't already been set.
          if (SketchSymbol != null)
            return;
          var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.CreateRGBColor(24, 69, 59),
                              SimpleFillStyle.Solid,
                            SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 1.0, SimpleLineStyle.Dash));
          SketchSymbol = polygonSymbol.MakeSymbolReference();
        });
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.OnToolMouseDown(ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs)
    // cref: ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs
    // cref: ArcGIS.Desktop.Mapping.MapTool.HandleMouseDownAsync(ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs)
    // cref: ArcGIS.Desktop.Mapping.MapViewMouseButtonEventArgs.ClientPoint
    // cref: ArcGIS.Desktop.Mapping.MapView.ClientToMap(System.Windows.Point)
    #region Create a tool to the return coordinates of the point clicked in the map
    /// <summary>
    /// A map tool that retrieves the map coordinates of a point clicked on the map.
    /// </summary>
    public class GetMapCoordinates : MapTool
    {
      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
          e.Handled = true; //Handle the event args to get the call to the corresponding async method
      }

      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        return QueuedTask.Run(() =>
        {
          //Convert the clicked point in client coordinates to the corresponding map coordinates.
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(string.Format("X: {0} Y: {1} Z: {2}",
                      mapPoint.X, mapPoint.Y, mapPoint.Z), "Map Coordinates");
        });
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapView.GetFeatures(ArcGIS.Core.Geometry.Geometry, bool, bool)
    // cref: ArcGIS.Desktop.Mapping.MapView.FlashFeature(ArcGIS.Desktop.Mapping.SelectionSet)
    #region Create a tool to identify the features that intersect the sketch geometry
    /// <summary>
    /// Represents a custom map tool that identifies and highlights features intersecting a user-defined sketch
    /// geometry.
    /// </summary>
    public class CustomIdentify : MapTool
    {
      public CustomIdentify()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Rectangle;

        //To perform a interactive selection or identify in 3D or 2D, sketch must be created in screen coordinates.
        SketchOutputMode = SketchOutputMode.Screen;
      }

      protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
      {
        return QueuedTask.Run(() =>
        {
          var mapView = MapView.Active;
          if (mapView == null)
            return true;

          //Get all the features that intersect the sketch geometry and flash them in the view. 
          var results = mapView.GetFeatures(geometry);
          mapView.FlashFeature(results);

          var debug = String.Join("\n", results.ToDictionary()
                        .Select(kvp => String.Format("{0}: {1}", kvp.Key.Name, kvp.Value.Count())));
          System.Diagnostics.Debug.WriteLine(debug);
          return true;
        });
      }
    }
    #endregion

    internal class Resource1
    {
      public static byte[] red_cursor = new byte[256];
    }

    // cref: ArcGIS.Desktop.Framework.Contracts.Tool.Cursor
    #region Change the cursor of a Tool
    /// <summary>
    /// Represents a custom map tool that allows users to sketch a rectangle on the map and provides the ability to
    /// customize the tool's cursor.
    /// </summary>
    public class CustomMapTool : MapTool
    {
      public CustomMapTool()
      {
        IsSketchTool = true;
        SketchType = SketchGeometryType.Rectangle;
        SketchOutputMode = SketchOutputMode.Map;
        //A custom cursor file as an embedded resource
        var cursorEmbeddedResource = new Cursor(new MemoryStream(Resource1.red_cursor));
        //A built in system cursor
        var systemCursor = System.Windows.Input.Cursors.ArrowCD;
        //Set the "CustomMapTool's" Cursor property to either one of the cursors defined above
        Cursor = cursorEmbeddedResource;
        //or
        Cursor = systemCursor;
      }
      protected override Task OnToolActivateAsync(bool active)
      {
        return base.OnToolActivateAsync(active);
      }

      protected override Task<bool> OnSketchCompleteAsync(Geometry geometry)
      {
        return base.OnSketchCompleteAsync(geometry);
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.ControlID
    // cref: ArcGIS.Desktop.Mapping.MapTool.EmbeddableControl
    #region Tool with an Embeddable Control
    /// <summary>
    /// Represents a map tool with an embeddable control that can handle mouse interactions on a map view.
    /// </summary>
    public class MapTool_WithControl : MapTool
    {
      // Using the Visual Studio SDK templates, add a MapTool and an EmbeddableControl
      // The EmbeddableControl is registered in the "esri_embeddableControls" category in the config.daml file
      // 
      //  <categories>
      //    <updateCategory refID = "esri_embeddableControls" >
      //      <insertComponent id="mapTool_EmbeddableControl" className="EmbeddableControl1ViewModel">
      //        <content className = "EmbeddableControl1View" />
      //      </insertComponent>
      //    <updateCategory>
      //  </categories>

      public MapTool_WithControl()
      {
        // substitute this string with the daml ID of the embeddable control you added
        ControlID = "mapTool_EmbeddableControl";
      }

      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        e.Handled = true;
      }
      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        //Get the instance of the ViewModel
        var vm = EmbeddableControl;
        if (vm == null)
          return Task.FromResult(0);

        // cast vm to your viewModel in order to access your properties

        //Get the map coordinates from the click point and set the property on the ViewMode.
        return QueuedTask.Run(() =>
        {
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          string clickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayControlID
    // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayEmbeddableControl
    #region Tool with an Overlay Embeddable Control
    /// <summary>
    /// Represents a map tool that integrates an overlay embeddable control for enhanced user interaction.
    /// </summary>
    public class MapTool_WithOverlayControl : MapTool
    {
      // Using the Visual Studio SDK templates, add a MapTool and an EmbeddableControl
      // The EmbeddableControl is registered in the "esri_embeddableControls" category in the config.daml file
      // 
      //  <categories>
      //    <updateCategory refID = "esri_embeddableControls" >
      //      <insertComponent id="mapTool_EmbeddableControl" className="EmbeddableControl1ViewModel">
      //        <content className = "EmbeddableControl1View" />
      //      </insertComponent>
      //    <updateCategory>
      //  </categories>

      public MapTool_WithOverlayControl()
      {
        // substitute this string with the daml ID of the embeddable control you added
        OverlayControlID = "mapTool_EmbeddableControl";
      }

      protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
      {
        e.Handled = true;
      }
      protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
      {
        //Get the instance of the ViewModel
        var vm = OverlayEmbeddableControl;
        if (vm == null)
          return Task.FromResult(0);

        // cast vm to your viewModel in order to access your properties

        //Get the map coordinates from the click point and set the property on the ViewMode.
        return QueuedTask.Run(() =>
        {
          var mapPoint = MapView.Active.ClientToMap(e.ClientPoint);
          string clickText = string.Format("X: {0}, Y: {1}, Z: {2}", mapPoint.X, mapPoint.Y, mapPoint.Z);
        });
      }
    }
    #endregion
  }
}
