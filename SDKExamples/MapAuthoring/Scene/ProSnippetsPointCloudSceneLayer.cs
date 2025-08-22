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
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace MapAuthoring.ProSnippets
{
  #region ProSnippet Group: PointCloudSceneLayer
  #endregion
  public static class ProSnippetsPointCloudSceneLayer
  {
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
    #region Name of PointCloudSceneLayer 
    /// <summary>
    /// Retrieves the name of the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> instance from which to retrieve the name.  This parameter can be <see
    /// langword="null"/>.</param>
    public static void PointCloudLayerName(PointCloudSceneLayer pointCloudSceneLayer)
    { 
      var scenelayerName = pointCloudSceneLayer?.Name;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetDataSourceType
    // cref: ArcGIS.Desktop.Mapping.SceneLayerDataSourceType
    #region Get Data Source type for PointCloudSceneLayer
    /// <summary>
    /// Determines the data source type of the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> instance whose data source type is to be determined.</param>
    public static void GetDataSourceTyple (PointCloudSceneLayer pointCloudSceneLayer)
    { 
      SceneLayerDataSourceType dataSourceType = pointCloudSceneLayer.GetDataSourceType();
      if (dataSourceType == SceneLayerDataSourceType.Service)
      {
        //TODO...
      }
      else if (dataSourceType == SceneLayerDataSourceType.SLPK)
      {

      }
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailableClassCodesAndLabels
    #region Query all class codes and lables in the PointCloudSceneLayer
    /// <summary>
    /// Queries all class codes and their corresponding labels from the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> from which to retrieve the class codes and labels.</param>
    public async static void QueryClassCodeAndLabels(PointCloudSceneLayer pointCloudSceneLayer)
    {
      await QueuedTask.Run(() =>
      {
        Dictionary<int, string> classCodesAndLabels =
                        pointCloudSceneLayer.GetAvailableClassCodesAndLabels();
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailableClassCodesAndLabels
    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition
    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ClassCodes
    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ReturnValues
    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ToCIM
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetFilters
    // cref: ArcGIS.Core.CIM.PointCloudReturnType
    #region Set a Filter for PointCloudSceneLayer
    /// <summary>
    /// Applies a filter to the specified <see cref="PointCloudSceneLayer"/> to exclude low noise and unclassified
    /// points.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> to which the filter will be applied.</param>
    public static void SetFilter(PointCloudSceneLayer pointCloudSceneLayer)
    {
      QueuedTask.Run(() =>
      {
        //Retrieve the available classification codes
        var dict = pointCloudSceneLayer.GetAvailableClassCodesAndLabels();

        //Filter out low noise and unclassified (7 and 1 respectively)
        //consult https://pro.arcgis.com/en/pro-app/help/data/las-dataset/storing-lidar-data.htm
        var filterDef = new PointCloudFilterDefinition()
        {
          ClassCodes = dict.Keys.Where(c => c != 7 && c != 1).ToList(),
          ReturnValues = new List<PointCloudReturnType> {
                              PointCloudReturnType.FirstOfMany }
        };
        //apply the filter
        pointCloudSceneLayer.SetFilters(filterDef.ToCIM());
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ClassFlags
    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.FromCIM
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetFilters
    // cref: ArcGIS.Desktop.Mapping.ClassFlag
    // cref: ArcGIS.Desktop.Mapping.ClassFlagOption
    #region Update the ClassFlags for PointCloudSceneLayer
    /// <summary>
    /// Updates the class flags for the specified <see cref="PointCloudSceneLayer"/> to exclude points classified as "edge of flight line".
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public static void UpdateClassFlags(PointCloudSceneLayer pointCloudSceneLayer)
    {
      QueuedTask.Run(() => {
        var filters = pointCloudSceneLayer.GetFilters();
        PointCloudFilterDefinition fdef = null;
        if (filters.Count() == 0)
        {
          fdef = new PointCloudFilterDefinition()
          {
            //7 is "edge of flight line" - exclude
            ClassFlags = new List<ClassFlag> {
               new ClassFlag(7, ClassFlagOption.Exclude) }
          };
        }
        else
        {
          fdef = PointCloudFilterDefinition.FromCIM(filters);
          //keep any include or ignore class flags
          var keep = fdef.ClassFlags.Where(
                 cf => cf.ClassFlagOption != ClassFlagOption.Exclude).ToList();
          //7 is "edge of flight line" - exclude
          keep.Add(new ClassFlag(7, ClassFlagOption.Exclude));
          fdef.ClassFlags = keep;
        }
        //apply
        pointCloudSceneLayer.SetFilters(fdef.ToCIM());
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.FromCIM
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetFilters
    // cref: ArcGIS.Desktop.Mapping.PointCloudFilterDefinition.ReturnValues
    // cref: ArcGIS.Core.CIM.PointCloudReturnType
    // cref: ArcGIS.Core.CIM.CIMPointCloudFilter
    // cref: ArcGIS.Core.CIM.CIMPointCloudReturnFilter
    // cref: ArcGIS.Core.CIM.CIMPointCloudValueFilter
    // cref: ArcGIS.Core.CIM.CIMPointCloudBitFieldFilter
    #region Get the filters for PointCloudSceneLayer
    /// <summary>
    /// Retrieves the filters applied to the specified <see cref="PointCloudSceneLayer"/> and processes them.
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public static void GetFilters(PointCloudSceneLayer pointCloudSceneLayer)
  {
      QueuedTask.Run(() => {
        IReadOnlyList<CIMPointCloudFilter> updatedFilter = pointCloudSceneLayer.GetFilters();
        foreach (var filter in updatedFilter)
        {
          //There is either 0 or 1 of each
          if (filter is CIMPointCloudReturnFilter returnFilter)
          {
            PointCloudFilterDefinition pcfl = PointCloudFilterDefinition.FromCIM(updatedFilter);
            List<PointCloudReturnType> updatedReturnValues = pcfl.ReturnValues;

          }
          if (filter is CIMPointCloudValueFilter classCodesFilter)
          {
            // do something
          }

          if (filter is CIMPointCloudBitFieldFilter classFlagsFilter)
          {
            // do something
          }
        }
      });
    }
  #endregion

  // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.ClearFilters
  #region Clear filters in PointCloudSceneLayer
  /// <summary>
  /// Clears all filters applied to the specified <see cref="PointCloudSceneLayer"/>.
  /// </summary>
  /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> instance whose filters will be cleared.  This parameter cannot be <see
  /// langword="null"/>.</param>
  public static void ClearFilters(PointCloudSceneLayer pointCloudSceneLayer)
  {
      QueuedTask.Run( () => pointCloudSceneLayer.ClearFilters()); 
  }
   #endregion

    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetRenderer
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.RendererType
    // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer
    #region Get PointCloudSceneLayer Renderer and RendererType
    /// <summary>
    /// Retrieves the renderer and renderer type for the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> from which to retrieve the renderer and renderer type.</param>
    public static void GetRenderer(PointCloudSceneLayer pointCloudSceneLayer)
    {
      QueuedTask.Run(() =>
      {
        CIMPointCloudRenderer cimGetPCLRenderer = pointCloudSceneLayer.GetRenderer();
        //Can be one of Unknown, Stretch, ClassBreaks, UniqueValue, RGB
        PointCloudRendererType pclRendererType = pointCloudSceneLayer.RendererType;
      });
    }
  #endregion

  // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
  // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
  #region Query PointCloudSceneLayer Renderer fields
    /// <summary>
    /// Retrieves the available point cloud renderer fields for the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> from which to retrieve the available renderer fields.</param>
  public static void GetAvailablePointCloudRendererFields(PointCloudSceneLayer pointCloudSceneLayer)
  {
      QueuedTask.Run(() =>
      {
        IReadOnlyList<string> flds = pointCloudSceneLayer.GetAvailablePointCloudRendererFields(
                                PointCloudRendererType.UniqueValueRenderer);
        var fldCount = flds.Count;
      });
  }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
    // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.CreateRenderer
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetRenderer
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
    // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer
    // cref: ArcGIS.Core.CIM.CIMPointCloudStretchRenderer.ColorRamp
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.ColorModulation
    // cref: ArcGIS.Core.CIM.CIMColorRamp
    // cref: ArcGIS.Core.CIM.CIMColorModulationInfo
    // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MinValue
    // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MaxValue
    // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.SEARCHCOLORRAMPS
    #region Create and Set a Stretch Renderer
    /// <summary>
    /// Creates and sets a stretch renderer for the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public static void CreateSetStretchRenderer(PointCloudSceneLayer pointCloudSceneLayer)
    {
      QueuedTask.Run( () => {
        var fields = pointCloudSceneLayer.GetAvailablePointCloudRendererFields(
                                  PointCloudRendererType.StretchRenderer);
        var stretchDef = new PointCloudRendererDefinition(
                                  PointCloudRendererType.StretchRenderer)
        {
          //Will be either ELEVATION or INTENSITY
          Field = fields[0]
        };
        //Create the CIM Renderer
        var stretchRenderer = pointCloudSceneLayer.CreateRenderer(stretchDef)
                                            as CIMPointCloudStretchRenderer;
        //Apply a color ramp
        var style = Project.Current.GetItems<StyleProjectItem>()
                                        .First(s => s.Name == "ArcGIS Colors");
        var colorRamp = style.SearchColorRamps("").First();
        stretchRenderer.ColorRamp = colorRamp.ColorRamp;
        //Apply modulation
        stretchRenderer.ColorModulation = new CIMColorModulationInfo()
        {
          MinValue = 0,
          MaxValue = 100
        };
        //apply the renderer
        pointCloudSceneLayer.SetRenderer(stretchRenderer);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.GetAvailablePointCloudRendererFields
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.CreateRenderer
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer.SetRenderer
    // cref: ArcGIS.Desktop.Mapping.PointCloudRendererType
    // cref: ArcGIS.Desktop.Mapping.PointCloudRendererDefinition.Field
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem
    // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
    // cref: ARCGIS.DESKTOP.MAPPING.STYLEHELPER.LOOKUPITEM
    // cref: ArcGIS.Desktop.Mapping.ColorFactory.GenerateColorsFromColorRamp
    // cref: ArcGIS.Core.CIM.CIMPointCloudClassBreaksRenderer
    // cref: ArcGIS.Core.CIM.CIMPointCloudClassBreaksRenderer.Breaks
    // cref: ArcGIS.Core.CIM.CIMColorClassBreak
    // cref: ArcGIS.Core.CIM.CIMColorClassBreak.UpperBound
    // cref: ArcGIS.Core.CIM.CIMColorClassBreak.Label
    // cref: ArcGIS.Core.CIM.CIMColorClassBreak.Color
    #region Create and Set a ClassBreaks Renderer
    /// <summary>
    /// Creates and sets a class breaks renderer for the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public async static void CreateSetClassBreaksRenderer(PointCloudSceneLayer pointCloudSceneLayer)
    {
      await QueuedTask.Run(() =>
      {
        var fields = pointCloudSceneLayer.GetAvailablePointCloudRendererFields(
                             PointCloudRendererType.ClassBreaksRenderer);
        var classBreakDef = new PointCloudRendererDefinition(
                                  PointCloudRendererType.ClassBreaksRenderer)
        {
          //ELEVATION or INTENSITY
          Field = fields[0]
        };
        //create the renderer
        var cbr = pointCloudSceneLayer.CreateRenderer(classBreakDef) 
                                  as CIMPointCloudClassBreaksRenderer;
        //Set up a color scheme to use
        var style = Project.Current.GetItems<StyleProjectItem>()
                                   .First(s => s.Name == "ArcGIS Colors");
        var rampStyle = style.LookupItem(
          StyleItemType.ColorRamp, "Spectrum By Wavelength-Full Bright_Multi-hue_2")
                                                                    as ColorRampStyleItem;
        var colorScheme = rampStyle.ColorRamp;
        //Set up 6 manual class breaks
        var breaks = 6;
        var colors = ColorFactory.Instance.GenerateColorsFromColorRamp(
                                                    colorScheme, breaks);
        var classBreaks = new List<CIMColorClassBreak>();
        var min = cbr.Breaks[0].UpperBound;
        var max = cbr.Breaks[cbr.Breaks.Count() - 1].UpperBound;
        var step = (max - min) / (double)breaks;

        //add in the class breaks
        double upper = min;
        for (int b = 1; b <= breaks; b++)
        {
          double lower = upper;
          upper = b == breaks ? max : min + (b * step);
          var cb = new CIMColorClassBreak()
          {
            UpperBound = upper,
            Label = string.Format("{0:#0.0#} - {1:#0.0#}", lower, upper),
            Color = colors[b - 1]
          };
          classBreaks.Add(cb);
        }
        cbr.Breaks = classBreaks.ToArray();
        pointCloudSceneLayer.SetRenderer(cbr);
      });
    }
    #endregion

    #region ProSnippet Group: PointCloudSceneLayer Extended Properties
    #endregion
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
    // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
    // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
    // cref: ArcGIS.Core.CIM.CIMPointCloudLayer
    // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.ColorModulation
    // cref: ArcGIS.Core.CIM.CIMColorModulationInfo
    // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MinValue
    // cref: ArcGIS.Core.CIM.CIMColorModulationInfo.MaxValue
    #region Edit Color Modulation
    /// <summary>
    /// Modifies the color modulation settings of the specified <see cref="PointCloudSceneLayer"/>.
    /// </summary>
    /// <param name="pointCloudSceneLayer">The <see cref="PointCloudSceneLayer"/> whose color modulation settings will be edited.</param>
    public static async void EditColorModulation(PointCloudSceneLayer pointCloudSceneLayer)
    {
      await QueuedTask.Run(() =>
      {
        var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;
        //Get the ColorModulation off the renderer
        var modulation = def.Renderer.ColorModulation;
        if (modulation == null)
          modulation = new CIMColorModulationInfo();
        //Set the minimum and maximum intensity as needed
        modulation.MinValue = 0;
        modulation.MaxValue = 100.0;
        //apply back
        def.Renderer.ColorModulation = modulation;
        //Commit changes back to the CIM
        pointCloudSceneLayer.SetDefinition(def);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
    // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
    // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
    // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointSizeAlgorithm
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointShape
    // cref: ArcGIS.Core.CIM.PointCloudShapeType
    // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm
    // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm.UseRealWorldSymbolSizes
    // cref: ArcGIS.Core.CIM.CIMPointCloudFixedSizeAlgorithm.Size
    #region Edit The Renderer to use Fixed Size
    /// <summary>
    /// Modifies the renderer of the specified <see cref="PointCloudSceneLayer"/> to use a fixed size for point rendering.
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public async static void EditRendererToUseFixedSize(PointCloudSceneLayer pointCloudSceneLayer)
    {
      await QueuedTask.Run(() =>
      {
        var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;

        //Set the point shape and sizing on the renderer
        def.Renderer.PointShape = PointCloudShapeType.DiskShaded;
        var pointSize = new CIMPointCloudFixedSizeAlgorithm()
        {
          UseRealWorldSymbolSizes = false,
          Size = 8
        };
        def.Renderer.PointSizeAlgorithm = pointSize;
        //Commit changes back to the CIM
        pointCloudSceneLayer.SetDefinition(def);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
    // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
    // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
    // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.Renderer
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointSizeAlgorithm
    // cref: ArcGIS.Core.CIM.CIMPointCloudRenderer.PointShape
    // cref: ArcGIS.Core.CIM.PointCloudShapeType
    // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm
    // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm.MinSize
    // cref: ArcGIS.Core.CIM.CIMPointCloudSplatAlgorithm.ScaleFactor
    #region Edit the Renderer to Scale Size
    /// <summary>
    /// Modifies the renderer of the specified <see cref="PointCloudSceneLayer"/> to scale the size of points based on a specified factor.
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public async static void EditRendererToScaleSize(PointCloudSceneLayer pointCloudSceneLayer)
   { 
      await QueuedTask.Run(() =>
      {
        var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;

        //Set the point shape and sizing on the renderer
        def.Renderer.PointShape = PointCloudShapeType.DiskFlat;//default
        var scaleSize = new CIMPointCloudSplatAlgorithm()
        {
          MinSize = 8,
          ScaleFactor = 1.0 //100%
        };
        def.Renderer.PointSizeAlgorithm = scaleSize;
        //Commit changes back to the CIM
        pointCloudSceneLayer.SetDefinition(def);
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Mapping.PointCloudSceneLayer
    // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition
    // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition
    // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.PointsBudget
    // cref: ArcGIS.Core.CIM.CIMPointCloudLayer.PointsPerInch
    #region Edit Density settings
    /// <summary>
    /// Modifies the density settings of the specified <see cref="PointCloudSceneLayer"/> to control the number of points displayed.
    /// </summary>
    /// <param name="pointCloudSceneLayer"></param>
    public static async void EditDensitySettings(PointCloudSceneLayer pointCloudSceneLayer)
  {
      await QueuedTask.Run(() =>
      {
        var def = pointCloudSceneLayer.GetDefinition() as CIMPointCloudLayer;
        //PointsBudget - corresponds to Display Limit on the UI
        // - the absolute maximum # of points to display
        def.PointsBudget = 1000000;

        //PointsPerInch - corresponds to Density Min --- Max on the UI
        // - the max number of points per display inch to renderer
        def.PointsPerInch = 15;
        //Commit changes back to the CIM
        pointCloudSceneLayer.SetDefinition(def);
      });
    }
  #endregion
}
}