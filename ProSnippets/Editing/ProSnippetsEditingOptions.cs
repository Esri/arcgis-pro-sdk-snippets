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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsEditingOptions
  {
    #region ProSnippet Group: EditingOptions
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions
    // cref: ArcGIS.Desktop.Core.UncommitedEditMode
    // cref: ArcGIS.Desktop.Core.ToolbarPosition
    // cref: ArcGIS.Desktop.Core.ToolbarSize
    #region Get/Set Editing Options

    public static void EditingOptionsToggleAndSwitch()
    {
      //toggle, switch option values
      var options = ApplicationOptions.EditingOptions;

      options.EnforceAttributeValidation = !options.EnforceAttributeValidation;
      options.WarnOnSubtypeChange = !options.WarnOnSubtypeChange;
      options.InitializeDefaultValuesOnSubtypeChange = !options.InitializeDefaultValuesOnSubtypeChange;
      options.UncommitedAttributeEdits = (options.UncommitedAttributeEdits ==
        UncommitedEditMode.AlwaysPrompt) ? UncommitedEditMode.Apply : UncommitedEditMode.AlwaysPrompt;

      options.StretchGeometry = !options.StretchGeometry;
      options.StretchTopology = !options.StretchTopology;
      options.UncommitedGeometryEdits = (options.UncommitedGeometryEdits ==
        UncommitedEditMode.AlwaysPrompt) ? UncommitedEditMode.Apply : UncommitedEditMode.AlwaysPrompt;

      options.ActivateMoveAfterPaste = !options.ActivateMoveAfterPaste;
      options.ShowFeatureSketchSymbology = !options.ShowFeatureSketchSymbology;
      options.FinishSketchOnDoubleClick = !options.FinishSketchOnDoubleClick;
      options.AllowVertexEditingWhileSketching = !options.AllowVertexEditingWhileSketching;
      options.ShowDeleteDialog = !options.ShowDeleteDialog;
      options.EnableStereoEscape = !options.EnableStereoEscape;
      options.DragSketch = !options.DragSketch;
      options.ShowDynamicConstraints = !options.ShowDynamicConstraints;
      options.IsDeflectionDefaultDirectionConstraint =
        !options.IsDeflectionDefaultDirectionConstraint;
      options.IsDirectionDefaultInputConstraint = !options.IsDirectionDefaultInputConstraint;
      options.ShowEditingToolbar = !options.ShowEditingToolbar;
      options.ToolbarPosition = (options.ToolbarPosition == ToolbarPosition.Bottom) ?
                ToolbarPosition.Right : ToolbarPosition.Bottom;
      options.ToolbarSize = (options.ToolbarSize == ToolbarSize.Medium) ?
                ToolbarSize.Small : ToolbarSize.Medium;
      options.MagnifyToolbar = !options.MagnifyToolbar;

      options.EnableEditingFromEditTab = !options.EnableEditingFromEditTab;
      options.AutomaticallySaveEdits = !options.AutomaticallySaveEdits;
      options.AutoSaveByTime = !options.AutoSaveByTime;
      options.SaveEditsInterval = (options.AutomaticallySaveEdits) ? 20 : 10;
      options.SaveEditsOperations = (options.AutomaticallySaveEdits) ? 60 : 30;
      options.SaveEditsOnProjectSave = !options.SaveEditsOnProjectSave;
      options.ShowSaveEditsDialog = !options.ShowSaveEditsDialog;
      options.ShowDiscardEditsDialog = !options.ShowDiscardEditsDialog;
      options.DeactivateToolOnSaveOrDiscard = !options.DeactivateToolOnSaveOrDiscard;
      options.NewLayersEditable = !options.NewLayersEditable;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions
    // cref: ArcGIS.Desktop.Core.AnnotationFollowMode
    // cref: ArcGIS.Desktop.Core.AnnotationPlacementMode
    #region Get/Set Editing Annotation Options
    public static void EditingAnnotationOptions()
    {
      var eOptions = ApplicationOptions.EditingOptions;

      var followLinkedLines = eOptions.AutomaticallyFollowLinkedLineFeatures;
      var followLinedPolygons = eOptions.AutomaticallyFollowLinkedPolygonFeatures;
      var usePlacementProps = eOptions.UseAnnotationPlacementProperties;
      var followMode = eOptions.AnnotationFollowMode;
      var placementMode = eOptions.AnnotationPlacementMode;


      eOptions.AnnotationFollowMode = AnnotationFollowMode.Parallel;
      eOptions.AnnotationPlacementMode = AnnotationPlacementMode.Left;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions.GetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType)
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.GetPointSymbol()
    // cref: ArcGIS.Desktop.Core.VertexSymbolType
    #region Get Sketch Vertex Symbology Options
    public static void GetSketchVertexSymbologyOptions()
    {
      var options = ApplicationOptions.EditingOptions;

      //Must use QueuedTask
      QueuedTask.Run(() =>
      {
        //There are 4 vertex symbol settings - selected, unselected and the
        //current vertex selected and unselected.
        var reg_select = options.GetVertexSymbolOptions(VertexSymbolType.RegularSelected);
        var reg_unsel = options.GetVertexSymbolOptions(VertexSymbolType.RegularUnselected);
        var curr_sel = options.GetVertexSymbolOptions(VertexSymbolType.CurrentSelected);
        var curr_unsel = options.GetVertexSymbolOptions(VertexSymbolType.CurrentUnselected);

        //to convert the options to a symbol use
        //GetPointSymbol
        var reg_sel_pt_symbol = reg_select.GetPointSymbol();
        //ditto for reg_unsel, curr_sel, curr_unsel
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.EditingOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions.GetSegmentSymbolOptions()
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.PrimaryColor
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.Width
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.HasSecondaryColor
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions.SecondaryColor
    #region Get Sketch Segment Symbology Options
    public static void GetSketchSegmentSymbologyOptions()
    {
      var options = ApplicationOptions.EditingOptions;

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        var seg_options = options.GetSegmentSymbolOptions();
        //to convert the options to a symbol use
        //SymbolFactory. Note: this is approximate....sketch isn't using the
        //CIM directly for segments
        var layers = new List<CIMSymbolLayer>();
        var stroke0 = SymbolFactory.Instance.ConstructStroke(seg_options.PrimaryColor,
          seg_options.Width, SimpleLineStyle.Dash);
        layers.Add(stroke0);
        if (seg_options.HasSecondaryColor)
        {
          var stroke1 = SymbolFactory.Instance.ConstructStroke(
            seg_options.SecondaryColor, seg_options.Width, SimpleLineStyle.Solid);
          layers.Add(stroke1);
        }
        //segment symbology only
        var sketch_line = new CIMLineSymbol()
        {
          SymbolLayers = layers.ToArray()
        };
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
    // cref: ArcGIS.Desktop.Core.VertexSymbolType
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.#ctor(ArcGIS.Desktop.Core.VertexSymbolType)
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.OutlineColor
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.MarkerType
    // cref: ArcGIS.Desktop.Core.VertexMarkerType
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions.Size
    // cref: ArcGIS.Desktop.Core.EditingOptions.CanSetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType, ArcGIS.Desktop.Core.VertexSymbolOptions)
    // cref: ArcGIS.Desktop.Core.EditingOptions.SetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType, ArcGIS.Desktop.Core.VertexSymbolOptions)
    #region Set Sketch Vertex Symbol Options
    public static void SetSketchVertexSymbolOptions()
    {
      var options = ApplicationOptions.EditingOptions;

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        //change the regular unselected vertex symbol
        //default is a green, hollow, square, 5pts. Change to
        //Blue outline diamond, 10 pts
        var vertexSymbol = new VertexSymbolOptions(VertexSymbolType.RegularUnselected);
        vertexSymbol.OutlineColor = ColorFactory.Instance.BlueRGB;
        vertexSymbol.MarkerType = VertexMarkerType.Diamond;
        vertexSymbol.Size = 10;

        //Are these valid?
        if (options.CanSetVertexSymbolOptions(
             VertexSymbolType.RegularUnselected, vertexSymbol))
        {
          //apply them
          options.SetVertexSymbolOptions(VertexSymbolType.RegularUnselected, vertexSymbol);
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.EditingOptions.GetSegmentSymbolOptions()
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions.CanSetSegmentSymbolOptions(ArcGIS.Desktop.Core.SegmentSymbolOptions)
    // cref: ArcGIS.desktop.Core.EditingOptions.SetSegmentSymbolOptions(ArcGIS.Desktop.Core.SegmentSymbolOptions)
    #region Set Sketch Segment Symbol Options
    public static void SetSketchSegmentSymbolOptions()
    {
      var options = ApplicationOptions.EditingOptions;

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        //change the segment symbol primary color to green and
        //width to 1 pt
        var segSymbol = options.GetSegmentSymbolOptions();
        segSymbol.PrimaryColor = ColorFactory.Instance.GreenRGB;
        segSymbol.Width = 1;

        //Are these valid?
        if (options.CanSetSegmentSymbolOptions(segSymbol))
        {
          //apply them
          options.SetSegmentSymbolOptions(segSymbol);
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.VertexSymbolType
    // cref: ArcGIS.Desktop.Core.EditingOptions.GetDefaultVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType)
    // cref: ArcGIS.Desktop.Core.VertexSymbolOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions.SetVertexSymbolOptions(ArcGIS.Desktop.Core.VertexSymbolType, ArcGIS.Desktop.Core.VertexSymbolOptions)
    #region Set Sketch Vertex Symbol Back to Default
    public static void SetSketchVertexSymbolBackToDefault()
    {
      var options = ApplicationOptions.EditingOptions;

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        //ditto for reg selected and current selected, unselected
        var def_reg_unsel =
          options.GetDefaultVertexSymbolOptions(VertexSymbolType.RegularUnselected);
        //apply default
        options.SetVertexSymbolOptions(
          VertexSymbolType.RegularUnselected, def_reg_unsel);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.EditingOptions.GetDefaultSegmentSymbolOptions
    // cref: ArcGIS.Desktop.Core.SegmentSymbolOptions
    // cref: ArcGIS.Desktop.Core.EditingOptions.SetSegmentSymbolOptions
    #region Set Sketch Segment Symbol Back to Default
    public static void SetSketchSegmentSymbolBackToDefault()
    {
      var options = ApplicationOptions.EditingOptions;

      //var options = ApplicationOptions.EditingOptions;
      QueuedTask.Run(() =>
      {
        var def_seg = options.GetDefaultSegmentSymbolOptions();
        options.SetSegmentSymbolOptions(def_seg);
      });
    }
    #endregion

    #region ProSnippet Group: VersioningOptions
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.VersioningOptions
    // cref: ArcGIS.Desktop.Core.VersioningOptions
    // cref: ArcGIS.Desktop.Core.VersioningOptions.DefineConflicts
    // cref: ArcGIS.Desktop.Core.VersioningOptions.ConflictResolution
    // cref: ArcGIS.Desktop.Core.VersioningOptions.ShowConflictsDialog
    // cref: ArcGIS.Desktop.Core.VersioningOptions.ShowReconcileDialog
    // cref: ArcGIS.Core.Data.ConflictDetectionType
    // cref: ArcGIS.Core.Data.ConflictResolutionType
    #region Get and Set Versioning Options
    public static void VersioningOptionsToggleAndSwitch()
    {
      var vOptions = ApplicationOptions.VersioningOptions;

      vOptions.DefineConflicts = (vOptions.DefineConflicts == ConflictDetectionType.ByRow) ?
        ConflictDetectionType.ByColumn : ConflictDetectionType.ByRow;
      vOptions.ConflictResolution = (
        vOptions.ConflictResolution == ConflictResolutionType.FavorEditVersion) ?
          ConflictResolutionType.FavorTargetVersion : ConflictResolutionType.FavorEditVersion;
      vOptions.ShowConflictsDialog = !vOptions.ShowConflictsDialog;
      vOptions.ShowReconcileDialog = !vOptions.ShowReconcileDialog;

      #endregion
    }
  }
}
