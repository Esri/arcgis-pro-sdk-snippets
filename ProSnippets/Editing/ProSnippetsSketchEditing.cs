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
using ArcGIS.Desktop.Internal.Editing;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Editing.ProSnippets
{
  public class ProSnippetsSketchEditing : MapTool
  {
    #region ProSnippet Group: Working with the Sketch
    #endregion

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ACTIVATESELECTASYNC
    #region Toggle sketch selection mode
    
    /// <summary>
    /// Handles key down events for the tool, enabling or toggling sketch selection mode when specific keys are pressed.
    /// </summary>
    /// <remarks>Pressing the <see cref="System.Windows.Input.Key.W"/> key activates the sketch selection mode
    /// if it is not already enabled. When selection mode is activated, the sketch is saved if selection is used. If
    /// selection mode is not active, pressing the Shift key is suppressed to prevent default behavior.</remarks>
    /// <param name="k">The <see cref="MapViewKeyEventArgs"/> instance containing event data for the key press. The <see
    /// cref="MapViewKeyEventArgs.Key"/> property indicates which key was pressed, and <see
    /// cref="MapViewKeyEventArgs.Handled"/> can be set to prevent further processing.</param>
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
      return (k.Key == System.Windows.Input.Key.LeftShift ||
             k.Key == System.Windows.Input.Key.RightShift);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.CurrentSketch
    // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.PreviousSketch
    #region Listen to the sketch modified event

    // SketchModified event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true

    /// <summary>
    /// Handles the SketchModified event, providing access to the current and previous sketch geometries when a sketch
    /// is modified.
    /// </summary>
    /// <remarks>This method is intended to be used as an event handler for the <see
    /// cref="ArcGIS.Desktop.Mapping.Events.SketchModifiedEvent"/>. The event is triggered by various editing tools,
    /// including construction tools (excluding annotation and dimension geometry types), Edit Vertices, Reshape, Align
    /// Features, and third-party tools with <c>FireSketchEvents</c> set to <see langword="true"/>. The <paramref
    /// name="args"/> parameter provides access to both the current and previous sketch geometries, allowing inspection
    /// or processing of changes made during the sketch operation.</remarks>
    /// <param name="args">The event arguments containing information about the sketch modification, including the current and previous
    /// sketch geometries.</param>
    public static void OnSketchModified(ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs args)
    {
      // if not an undo operation
      if (!args.IsUndo)
      {
        // what was the sketch before the change?
        var prevSketch = args.PreviousSketch;
        // what is the current sketch?
        var currentSketch = args.CurrentSketch;
        if (currentSketch is Polyline polyline)
        {
          // Examine the current (last) vertex in the line sketch
          var lastSketchPoint = polyline.Points.Last();

          // do something with the last point
        }
      }
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs.Sketch
    // cref: ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEventArgs.SetSketchGeometry
    #region Listen to the before sketch completed event and modify the sketch

    // BeforeSketchCompleted event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true


    //Subscribe to the before sketch completed event
    //ArcGIS.Desktop.Mapping.Events.BeforeSketchCompletedEvent.Subscribe(OnBeforeSketchCompleted);
    /// <summary>
    /// Handles the event that occurs before a sketch is completed, allowing modification of the sketch geometry prior
    /// to finalization.
    /// </summary>
    /// <remarks>This method is typically invoked by subscribing to the <c>BeforeSketchCompleted</c> event in
    /// ArcGIS Pro.  It enables customization of the sketch geometry before it is committed, such as updating Z values
    /// or applying other transformations.</remarks>
    /// <param name="args">The event arguments containing the current sketch geometry and methods to update it.  Must not be <c>null</c>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task is completed when the event handling
    /// is finished.</returns>
    public Task OnBeforeSketchCompleted(BeforeSketchCompletedEventArgs args)
    {
      //assign sketch Z values from default surface and set the sketch geometry
      var modifiedSketch = args.MapView.Map.GetZsFromSurfaceAsync(args.Sketch).Result;
      args.SetSketchGeometry(modifiedSketch.Geometry);
      return Task.CompletedTask;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.SketchCompletedEventArgs.Sketch
    #region Listen to the sketch completed event

    // SketchCompleted event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true
    /// <summary>
    /// Handles the completion of a sketch operation by processing the provided sketch data.
    /// </summary>
    /// <remarks>This method is typically invoked in response to the <c>SketchCompleted</c> event, which is
    /// fired by standard construction tools (excluding annotation and dimension geometry types), Edit Vertices,
    /// Reshape, Align Features, and third-party tools with <c>FireSketchEvents</c> set to <see langword="true"/>. Use
    /// the <see cref="ArcGIS.Desktop.Mapping.Events.SketchCompletedEventArgs.Sketch"/> property of <paramref
    /// name="args"/> to access the completed sketch geometry for further processing.</remarks>
    /// <param name="args">The event arguments containing information about the completed sketch, including the resulting geometry.</param>
    public void OnSketchCompleted(SketchCompletedEventArgs args)
    {
      // get the sketch
      var finalSketch = args.Sketch;

      // do something with the sketch - audit trail perhaps
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.FireSketchEvents
    #region Custom construction tool that fires sketch events
    /// <summary>
    /// Represents a custom map construction tool that enables line sketching with snapping and template support in
    /// ArcGIS Pro.
    /// </summary>
    /// <remarks><para> <b>ConstructionTool1</b> is a specialized <see cref="ArcGIS.Desktop.Mapping.MapTool"/>
    /// configured for creating line geometries. It is designed to fire sketch events, utilize snapping, and apply the
    /// current feature template during construction. </para> <para> To use this tool, ensure it is properly registered
    /// in your DAML with the appropriate component category. The tool is intended for workflows that require
    /// interactive line creation and integration with ArcGIS Pro's editing environment. </para> <para> <b>Thread
    /// Safety:</b> This tool is intended for use within the ArcGIS Pro UI thread and is not thread-safe.
    /// </para></remarks>
    public class ConstructionTool1 : MapTool
    {
      public ConstructionTool1()
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
    }

    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool.GetSketchSegmentSymbolOptions()
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
    // cref: ArcGIS.Desktop.Mapping.MapTool.SetSketchSegmentSymbolOptions
    // cref: ArcGIS.Desktop.Mapping.MapTool.SetSketchVertexSymbolOptions
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
    #region Customizing the Sketch Symbol of a Custom Sketch Tool

/// <summary>
/// Handles activation and deactivation of the tool, allowing customization of sketch segment and vertex symbology when
/// the tool becomes active.
/// </summary>
/// <remarks>When the tool is activated (<paramref name="active"/> is <see langword="true"/>), this method
/// customizes the appearance of sketch segments and vertices by updating their symbol options. This includes setting
/// colors, widths, marker types, and other visual properties for both segments and regular unselected vertices. These
/// changes affect how the sketch is displayed to the user while the tool is active. <para> When deactivated, no
/// symbology changes are applied. </para></remarks>
/// <param name="active"><see langword="true"/> to activate the tool and apply custom sketch symbology; <see langword="false"/> to deactivate
/// the tool.</param>
/// <returns>A <see cref="Task"/> representing the asynchronous operation. The task completes when the activation or deactivation
/// process is finished.</returns>
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
  /// <remarks>This tool enables interactive sketching of line geometries on the map and provides a custom UI
  /// sketch tip by associating an embeddable control with the sketch tool. The sketch tip UI can display dynamic
  /// information, such as the current vertex count, and can be customized by implementing properties and controls in
  /// the associated view model and view. <para> To use a custom UI sketch tip, define an embeddable control in your
  /// DAML configuration and set its ID to the <see cref="ArcGIS.Desktop.Mapping.MapTool.SketchTipID"/> property. The
  /// embeddable control's view model can be accessed via <see
  /// cref="ArcGIS.Desktop.Mapping.MapTool.SketchTipEmbeddableControl"/> for updating UI elements in response to sketch
  /// events. </para> <para> This tool is configured to sketch line geometries and outputs the sketch in map
  /// coordinates. </para></remarks>
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
  #endregion

}
