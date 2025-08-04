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

namespace ProSnippetsEditing
{
  public class ProSnippetsSketchEditing : MapTool
  {
    #region ProSnippet Group: Working with the Sketch
    #endregion

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ACTIVATESELECTASYNC
    #region Toggle sketch selection mode
    //UseSelection = true; (UseSelection must be set to true in the tool constructor or tool activate)
    private bool _inSelMode = false;

    public bool IsShiftKey(MapViewKeyEventArgs k)
    {
      return (k.Key == System.Windows.Input.Key.LeftShift ||
             k.Key == System.Windows.Input.Key.RightShift);
    }

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
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.CurrentSketch
    // cref: ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs.PreviousSketch
    #region Listen to the sketch modified event

    // SketchModified event is fired by 
    //  - COTS construction tools (except annotation, dimension geometry types), 
    //  - Edit Vertices, Reshape, Align Features
    //  - 3rd party tools with FireSketchEvents = true


    //Subscribe the sketch modified event
    //ArcGIS.Desktop.Mapping.Events.SketchModifiedEvent.Subscribe(OnSketchModified);

    private void OnSketchModified(ArcGIS.Desktop.Mapping.Events.SketchModifiedEventArgs args)
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

    private Task OnBeforeSketchCompleted(BeforeSketchCompletedEventArgs args)
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


    //Subscribe to the sketch completed event
    //ArcGIS.Desktop.Mapping.Events.SketchCompletedEvent.Subscribe(OnSketchCompleted);

    private void OnSketchCompleted(SketchCompletedEventArgs args)
    {
      // get the sketch
      var finalSketch = args.Sketch;

      // do something with the sketch - audit trail perhaps
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapTool
    // cref: ArcGIS.Desktop.Mapping.MapTool.FireSketchEvents
    #region Custom construction tool that fires sketch events

    internal class ConstructionTool1 : MapTool
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

    //Custom tools have the ability to change the symbology used when sketching a new feature. 
    //Both the Sketch Segment Symbol and the Vertex Symbol can be modified using the correct set method. 
    //This is set in the activate method for the tool.
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
