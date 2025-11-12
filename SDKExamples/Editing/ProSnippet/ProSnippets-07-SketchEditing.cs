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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProSnippets.EditingSnippets
{   
  #region ProSnippet Group: Working with the Sketch
  #endregion

  /// <summary>
  /// Provides functionality for handling sketch editing events and toggling sketch selection modes within a map tool.
  /// </summary>
  public class ProSnippetsSketchEditing : MapTool
  {
    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ACTIVATESELECTASYNC
    #region Toggle sketch selection mode

    /// <summary>
    /// Handles key down events for the tool, enabling or toggling sketch selection mode when specific keys are pressed.
    /// </summary>
    protected override async void OnToolKeyDown(MapViewKeyEventArgs k)
    {
      //toggle sketch selection mode with a custom key
      if (k.Key == System.Windows.Input.Key.W)
      {
        if (!_inSelMode)
        {
          k.Handled = true;

          // Toggle the tool to select mode.
          //  The sketch is saved if UseSelection = true;
          if (await ActivateSelectAsync(true))
            _inSelMode = true;
        }
      }
      else if (!_inSelMode)
      {
        //disable effect of Shift in the base class.
        //Mark the key event as handled to prevent further processing
        k.Handled = IsShiftKey(k);
      }
    }
    /// <summary>
    /// Handles the key up events for the tool, toggling sketch selection mode off when the W key is released.
    /// </summary>
    protected override void OnToolKeyUp(MapViewKeyEventArgs k)
    {
      if (k.Key == System.Windows.Input.Key.W)
      {
        if (_inSelMode)
        {
          _inSelMode = false;
          k.Handled = true;//process this one

          // Toggle back to sketch mode. If UseSelection = true
          //   the sketch will be restored
          ActivateSelectAsync(false);
        }
      }
      else if (_inSelMode)
      {
        //disable effect of Shift in the base class.
        //Mark the key event as handled to prevent further processing
        k.Handled = IsShiftKey(k);
      }
    }//UseSelection = true; (UseSelection must be set to true in the tool constructor or tool activate)
    private bool _inSelMode = false;

    private bool IsShiftKey(MapViewKeyEventArgs k)
    {
      return k.Key == System.Windows.Input.Key.LeftShift ||
             k.Key == System.Windows.Input.Key.RightShift;
    }
    #endregion
  }

  #region ProSnippet Group: Sketch Events
  #endregion

  public static partial class ProSnippetsEditing
  {
    public static void ProSnippetsSketchEventSamples()
    {
      // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEvent.Subscribe(ArcGIS.Desktop.Mapping.Events.SketchModifiedEventHandler)
      // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs
      // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.CurrentSketch
      // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.PreviousSketch
      #region Listen to the sketch modified event
      // Occurs when a sketch is modified. SketchModified event is fired by 
      //  - COTS construction tools (except annotation, dimension geometry types), 
      //  - Edit Vertices, Reshape, Align Features
      //  - 3rd party tools with FireSketchEvents = true
      // Handles the SketchModified event, providing access to the current and previous sketch geometries when a sketch
      // is modified.
      // Register for the SketchModified event
      SketchModifiedEvent.Subscribe(sketchModifiedEventArgs
        =>
      {
        // if not an undo operation
        if (!sketchModifiedEventArgs.IsUndo)
        {
          // what was the sketch before the change?
          var prevSketch = sketchModifiedEventArgs.PreviousSketch;
          // what is the current sketch?
          var currentSketch = sketchModifiedEventArgs.CurrentSketch;
          if (currentSketch is Polyline polyline)
          {
            // Examine the current (last) vertex in the line sketch
            var lastSketchPoint = polyline.Points.Last();
            // do something with the last point
          }
        }
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEvent.Subscribe(ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventHandler)
      // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs
      // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs.Sketch
      // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs.SetSketchGeometry
      #region Listen to the before sketch completed event and modify the sketch
      // Occurs before a sketch is completed. BeforeSketchCompleted event is fired by 
      //  - COTS construction tools (except annotation, dimension geometry types), 
      //  - Edit Vertices, Reshape, Align Features
      //  - 3rd party tools with FireSketchEvents = true
      // Register for the BeforeSketchCompleted event
      BeforeSketchCompletedEvent.Subscribe(beforeSketchCompletedEventArgs
        =>
      {
        //assign sketch Z values from default surface and set the sketch geometry
        var modifiedSketch = beforeSketchCompletedEventArgs.MapView.Map.GetZsFromSurfaceAsync(beforeSketchCompletedEventArgs.Sketch).Result;
        beforeSketchCompletedEventArgs.SetSketchGeometry(modifiedSketch.Geometry);
        return Task.CompletedTask;
      });
      #endregion

      // cref: ArcGIS.Desktop.Mapping.Events.SketchCompletedEventArgs.Sketch
      #region Listen to the sketch completed event

      // Occurs when a sketch is completed. SketchCompleted event is fired by 
      //  - COTS construction tools (except annotation, dimension geometry types), 
      //  - Edit Vertices, Reshape, Align Features
      //  - 3rd party tools with FireSketchEvents = true
      // Register for the SketchCompleted event
      SketchCompletedEvent.Subscribe(sketchCompletedEventArgs
        =>
      {
        // get the sketch
        var finalSketch = sketchCompletedEventArgs.Sketch;

        // do something with the sketch - audit trail perhaps
      });
      #endregion
    }
  }

    // cref: ArcGIS.Desktop.Mapping.MapTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.FireSketchEvents
    #region Custom construction tool that fires sketch events
    /// <summary>
    /// Represents a custom map construction tool that enables sketching of all types of geometries (points, lines, polygons) with snapping and template support in
    /// ArcGIS Pro.  ProSnippetConstructionToolLine is a specialized MapTool class configured for creating line geometries
    /// </summary>
    public class ProSnippetConstructionToolLine : MapTool
    {
      public ProSnippetConstructionToolLine()
      {
        IsSketchTool = true;
        UseSnapping = true;
        // Select the type of construction tool you wish to implement.  
        // Make sure that the tool is correctly registered with the correct component category type in the daml 
        SketchType = SketchGeometryType.Line;
        //Gets or sets whether the sketch is for creating a feature and should use the CurrentTemplate.
        UsesCurrentTemplate = true;

        // set FireSketchEvents property to true
        FireSketchEvents = true;
      }
    //  ...
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.GetSketchSegmentSymbolOptions()
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
    // cref: ArcGIS.Desktop.Mapping.MapTool.SetSketchSegmentSymbolOptions
    // cref: ArcGIS.Desktop.Mapping.MapTool.SetSketchVertexSymbolOptions
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
    #region Customizing the Sketch Symbol of a Custom Sketch Tool
    /// <summary>
    /// Within the specialization of a MapTool, this method handles activation and deactivation of the tool, allowing customization of sketch segment and vertex symbology when
    /// the tool becomes active.  When the tool is activated, this method customizes the appearance of sketch segments and vertices by updating their symbol options. This includes setting colors, widths, marker types, and other visual properties for both segments and regular unselected vertices.
    /// </summary>
    protected override Task OnToolActivateAsync(bool active)
    {
      QueuedTask.Run(() =>
      {
        //Getting the current symbology options of the segment
        var segmentOptions = GetSketchSegmentSymbolOptions();
        //Modifying the primary and secondary color and the width of the segment symbology options
        var deepPurple = new CIMRGBColor() { R = 75, G = 0, B = 110 };
        segmentOptions.PrimaryColor = deepPurple;
        segmentOptions.Width = 4;
        segmentOptions.HasSecondaryColor = true;
        var pink = new CIMRGBColor() { R = 219, G = 48, B = 130 };
        segmentOptions.SecondaryColor = pink;
        //Creating a new vertex symbol options instance with the values you want
        var vertexOptions = new VertexSymbolOptions(VertexSymbolType.RegularUnselected);
        var yellow = new CIMRGBColor() { R = 255, G = 215, B = 0 };
        var purple = new CIMRGBColor() { R = 148, G = 0, B = 211 };
        vertexOptions.AngleRotation = 45;
        vertexOptions.Color = yellow;
        vertexOptions.MarkerType = VertexMarkerType.Star;
        vertexOptions.OutlineColor = purple;
        vertexOptions.OutlineWidth = 3;
        vertexOptions.Size = 5;

        //Setting the value of the segment symbol options
        SetSketchSegmentSymbolOptions(segmentOptions);
        //Setting the value of the vertex symbol options of the regular unselected vertices using the vertexOptions instance created above.
        SetSketchVertexSymbolOptions(VertexSymbolType.RegularUnselected, vertexOptions);
      });

      return base.OnToolActivateAsync(active);
    }
    #endregion
  }

  #region ProSnippet Group: SketchTool
  #endregion

  // cref: ArcGIS.Desktop.Mapping.MapTool.ContextMenuID
  // cref: ArcGIS.Desktop.Mapping.MapTool.ContextToolbarID
  #region Set a MiniToolbar, ContextMenuID

  // daml entries
  // 
  // <menus>
  //  <menu id="MyMenu" caption="Nav">
  //    <button refID="esri_mapping_prevExtentButton"/>
  //    <button refID="esri_mapping_fixedZoomInButton"/>
  //  </menu>
  // </menus>
  // <miniToolbars>
  //   <miniToolbar id="MyMiniToolbar">
  //    <row>
  //      <button refID="esri_mapping_fixedZoomInButton"/>
  //      <button refID="esri_mapping_prevExtentButton"/>
  //    </row>
  //   </miniToolbar>
  // </miniToolbars>
  /// <summary>
  /// Represents a map sketch tool that provides a custom context menu and mini toolbar for user interactions.
  /// </summary>
  /// <remarks>This tool enables line sketching on the map and displays a context menu and mini toolbar, as
  /// specified by their respective DAML IDs, when activated. The context menu and toolbar can be customized to include
  /// commands relevant to the sketch workflow. The tool is configured to output line geometry directly to the map.
  /// <para> The <see cref="MapTool.ContextMenuID"/> and <see cref="MapTool.ContextToolbarID"/> properties are set to
  /// reference custom DAML menu and mini toolbar definitions. Ensure that the specified IDs correspond to valid DAML
  /// entries in your add-in configuration. </para></remarks>
  public class SketchToolWithToolbar : MapTool
  {
    public SketchToolWithToolbar()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Line;
      SketchOutputMode = SketchOutputMode.Map;
      ContextMenuID = "MyMenu";
      ContextToolbarID = "MyMiniToolbar";
    }
  }
  #endregion

  // cref: ArcGIS.Desktop.Mapping.MapTool.SketchTip
  #region Set a simple sketch tip
  /// <summary>
  /// Represents a map tool that enables line sketching with a predefined sketch tip.
  /// </summary>
  /// <remarks>This tool is configured to allow users to sketch lines on the map. The sketch tip provides
  /// guidance to the user during the sketching process.</remarks>
  public class SketchToolWithSketchTip : MapTool
  {
    public SketchToolWithSketchTip()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Line;
      SketchOutputMode = SketchOutputMode.Map;
      SketchTip = "hello World";
    }
  }
  #endregion

  // cref: ArcGIS.Desktop.Mapping.MapTool.SketchTipID
  // cref: ArcGIS.Desktop.Mapping.MapTool.SketchTipEmbeddableControl
  #region Set a custom UI Sketch Tip

  // 1. Add an embeddable control using VS template.  This is the daml entry

  //<categories>
  //  <updateCategory refID = "esri_embeddableControls">
  //    <insertComponent id="SketchTip_EmbeddableControl1" className="EmbeddableControl1ViewModel">
  //      <content className = "EmbeddableControl1View"/>
  //    </insertComponent>
  //  </updateCategory>
  // </categories>

  // 2. Define UI controls on the EmbeddableControl1View
  // 3. Define properties on the EmbeddableControl1ViewModel which
  //    bind to the UI controls on the EmbeddableControl1View
  /// <summary>
  /// Represents a map sketch tool that displays a custom UI sketch tip using an embeddable control.
  /// </summary>
  public class SketchToolWithUISketchTip : MapTool
  {
    public SketchToolWithUISketchTip()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Line;
      SketchOutputMode = SketchOutputMode.Map;
      SketchTipID = "SketchTip_EmbeddableControl1";
    }

    protected override Task<bool> OnSketchModifiedAsync()
    {
      var sketchTipVM = SketchTipEmbeddableControl as EmbeddableControl1ViewModel;
      if (sketchTipVM != null)
      {
        // modify properties on the sketchTipVM
        QueuedTask.Run(async () =>
        {
          var sketch = await GetCurrentSketchAsync();
          var line = sketch as Polyline;
          var count = line.PointCount;

          sketchTipVM.Text = "Vertex Count " + count.ToString();
        });
      }

      return base.OnSketchModifiedAsync();
    }

  }

  /// <summary>
  /// Represents the view model for an embeddable control, providing data and configuration options for the control's
  /// behavior and appearance.
  /// </summary>
  /// <remarks>This class is typically used to supply options and state to an embeddable control within the
  /// ArcGIS Pro framework. It is constructed with configuration data and a flag indicating whether options can be
  /// changed at runtime.</remarks>
  public class EmbeddableControl1ViewModel : ArcGIS.Desktop.Framework.Controls.EmbeddableControl
  {
    public EmbeddableControl1ViewModel(XElement options, bool canChangeOptions) : base(options, canChangeOptions)
    { }

    public string Text;
  }
  #endregion

}
