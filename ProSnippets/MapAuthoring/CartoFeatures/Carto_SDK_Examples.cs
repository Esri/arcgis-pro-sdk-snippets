/*

   Copyright 2018 Esri

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
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Carto_SDK_Examples
{
  public static class Carto_SDK_Examples
  {
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
    // cref: ArcGIS.Desktop.Mapping.SymbolStyleItem.Symbol
    #region Get symbol from SymbolStyleItem
    /// <summary>
    /// Retrieves the symbol associated with the specified <see cref="SymbolStyleItem"/>.
    /// </summary>
    /// <param name="symbolItem">The <see cref="SymbolStyleItem"/> from which to retrieve the symbol. Cannot be null.</param>
    public static async void GetSymbolFromSymbolStyleItem(SymbolStyleItem symbolItem)
    {
      CIMSymbol symbol = await QueuedTask.Run<CIMSymbol>(() =>
      {
        return symbolItem.Symbol;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.ColorStyleItem.Color
    // cref: ArcGIS.Desktop.Mapping.ColorStyleItem.Color
    #region Get color from ColorStyleItem
    /// <summary>
    /// Retrieves the color associated with the specified <see cref="ColorStyleItem"/>.
    /// </summary>
    /// <param name="colorItem">The <see cref="ColorStyleItem"/> from which to retrieve the color.  This parameter cannot be <see
    /// langword="null"/>.</param>
    public static async void GetColorFromColorStyleItem(ColorStyleItem colorItem)
    {
      CIMColor color = await QueuedTask.Run<CIMColor>(() =>
      {
        return colorItem.Color;
      });
    }
    #endregion

      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      // cref: ArcGIS.Desktop.Mapping.ColorRampStyleItem.ColorRamp
      #region Get color ramp from ColorRampStyleItem
    /// <summary>
    /// Retrieves the <see cref="CIMColorRamp"/> from the specified <see cref="ColorRampStyleItem"/>.
    /// </summary>
    /// <param name="colorRampItem">The <see cref="ColorRampStyleItem"/> from which to extract the <see cref="CIMColorRamp"/>. Must not be <see
    /// langword="null"/>.</param>
    public static async void GetColorRampFromColorRampStyleItem(ColorRampStyleItem colorRampItem)
    { 
      CIMColorRamp colorRamp = await QueuedTask.Run<CIMColorRamp>(() =>
      {
        return colorRampItem.ColorRamp;
      });
    }
    #endregion

      // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem.NorthArrow
      // cref: ArcGIS.Desktop.Mapping.NorthArrowStyleItem.NorthArrow
      #region Get north arrow from NorthArrowStyleItem
    /// <summary>
    /// Retrieves the <see cref="ArcGIS.Desktop.Mapping.CIMNorthArrow"/> object associated with the specified  <see
    /// cref="ArcGIS.Desktop.Mapping.NorthArrowStyleItem"/>.
    /// </summary>  
    /// <param name="northArrowItem">The <see cref="ArcGIS.Desktop.Mapping.NorthArrowStyleItem"/> from which to retrieve the north arrow. Must not be
    /// <see langword="null"/>.</param>
    public static async void GetNorthArrowFromNorthArrowStyleItem(NorthArrowStyleItem northArrowItem)
    { 
      CIMNorthArrow northArrow = await QueuedTask.Run<CIMNorthArrow>(() =>
      {
        return northArrowItem.NorthArrow;
      });
    }
    #endregion

      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem.ScaleBar
      // cref: ArcGIS.Desktop.Mapping.ScaleBarStyleItem.ScaleBar
      #region Get scale bar from ScaleBarStyleItem
/// <summary>
/// Retrieves the scale bar configuration from the specified <see cref="ScaleBarStyleItem"/>.
/// </summary>
/// <param name="scaleBarItem">The <see cref="ScaleBarStyleItem"/> from which to retrieve the scale bar configuration. Must not be <see
/// langword="null"/>.</param>
    public static async void GetScaleBarFromScaleBarStyleItem(ScaleBarStyleItem scaleBarItem)
    { 
      
      CIMScaleBar scaleBar = await QueuedTask.Run<CIMScaleBar>(() =>
      {
        return scaleBarItem.ScaleBar;
      });
    }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LabelPlacementStyleItem.LabelPlacement
      // cref: ArcGIS.Desktop.Mapping.LabelPlacementStyleItem.LabelPlacement

      #region Get label placement from LabelPlacementStyleItem
    /// <summary>
    /// Retrieves the label placement properties from the specified <see cref="LabelPlacementStyleItem"/>.
    /// </summary>
    /// <param name="labelPlacementItem">The <see cref="LabelPlacementStyleItem"/> from which to obtain the label placement properties. This parameter
    /// cannot be null.</param>
    public static async void GetLabelPlacementFromLabelPlacementStyleItem(LabelPlacementStyleItem labelPlacementItem)
    {
      CIMLabelPlacementProperties labelPlacement = await QueuedTask.Run<CIMLabelPlacementProperties>(() =>
      {
        return labelPlacementItem.LabelPlacement;
      });
    }
     #endregion

      // cref: ArcGIS.Desktop.Mapping.GridStyleItem.Grid
      // cref: ArcGIS.Desktop.Mapping.GridStyleItem.Grid
      #region Get grid from GridStyleItem
    /// <summary>
    /// Retrieves the grid configuration from the specified <see cref="GridStyleItem"/>.
    /// </summary>
    /// <param name="gridItem">The <see cref="GridStyleItem"/> from which to retrieve the grid configuration. Must not be <see
    /// langword="null"/>.</param>
    public static async void GetGridFromGridStyleItem(GridStyleItem gridItem)
    { 
      CIMMapGrid grid = await QueuedTask.Run<CIMMapGrid>(() =>
      {
        return gridItem.Grid;
      });
    }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LegendStyleItem.Legend
      // cref: ArcGIS.Desktop.Mapping.LegendStyleItem.Legend
      #region Get legend from LegendStyleItem
/// <summary>
/// Retrieves the legend associated with the specified <see cref="LegendStyleItem"/>.
/// </summary>
/// <param name="legendStyleItem">The <see cref="LegendStyleItem"/> from which to retrieve the legend.  This parameter cannot be <see
/// langword="null"/>.</param>
    public static async void GetLegendFromLegendStyleItem(LegendStyleItem legendStyleItem)
    { 

      CIMLegend legend = await QueuedTask.Run<CIMLegend>(() =>
      {
        return legendStyleItem.Legend;
      });
    }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LegendItemStyleItem.LegendItem
      // cref: ArcGIS.Desktop.Mapping.LegendItemStyleItem.LegendItem
      #region Get legend item from LegendItemStyleItem
    /// <summary>
    /// Retrieves the <see cref="ArcGIS.Desktop.Mapping.CIMLegendItem"/> associated with the specified  <see
    /// cref="ArcGIS.Desktop.Mapping.LegendItemStyleItem"/>.
    /// </summary>
    /// <param name="legendItemStyleItem">The <see cref="ArcGIS.Desktop.Mapping.LegendItemStyleItem"/> from which to retrieve the legend item.</param>
    public static async void GetLegendItemFromLegendItemStyleItem(LegendItemStyleItem legendItemStyleItem)
    {
      CIMLegendItem legendItem = await QueuedTask.Run<CIMLegendItem>(() =>
      {
        return legendItemStyleItem.LegendItem;
      });
    }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableFrameStyleItem.TableFrame
      // cref: ArcGIS.Desktop.Mapping.TableFrameStyleItem.TableFrame
      #region Get table frame from TableFrameStyleItem
    /// <summary>
    /// Retrieves the table frame associated with the specified <see cref="TableFrameStyleItem"/>.
    /// </summary>
    /// <param name="tableFrameItem">The <see cref="TableFrameStyleItem"/> from which to retrieve the table frame.  This parameter cannot be null.</param>
    public static async void GetTableFrameFromTableFrameStyleItem(TableFrameStyleItem tableFrameItem)
    {

      CIMTableFrame tableFrame = await QueuedTask.Run<CIMTableFrame>(() =>
      {
        return tableFrameItem.TableFrame;
      });
    }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.TableFrameFieldStyleItem.TableFrameField
      // cref: ArcGIS.Desktop.Mapping.TableFrameFieldStyleItem.TableFrameField
      #region Get table frame field from TableFrameFieldStyleItem
    public static async void GetTableFrameFieldFromTableFrameFieldStyleItem(TableFrameFieldStyleItem tableFrameFieldItem)
    {
      CIMTableFrameField tableFrameField = await QueuedTask.Run<CIMTableFrameField>(() =>
      {
        return tableFrameFieldItem.TableFrameField;
      });
    }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.MapSurroundStyleItem.MapSurround
      // cref: ArcGIS.Desktop.Mapping.MapSurroundStyleItem.MapSurround
      #region Get map surround from MapSurroundStyleItem
    public static async void GetMapSurroundFromMapSurroundStyleItem(MapSurroundStyleItem mapSurroundItem)
    { 
      CIMMapSurround mapSurround = await QueuedTask.Run<CIMMapSurround>(() =>
      {
        return mapSurroundItem.MapSurround;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.DimensionStyleStyleItem.DimensionStyle
    // cref: ArcGIS.Desktop.Mapping.DimensionStyleStyleItem.DimensionStyle
    #region Get dimension style from DimensionStyleStyleItem
    public static async void GetDimensionStyleFromDimensionStyleStyleItem(DimensionStyleStyleItem dimensionStyleStyleItem)
    { 
      CIMDimensionStyle dimensionStyle = await QueuedTask.Run<CIMDimensionStyle>(() =>
      {
        return dimensionStyleStyleItem.DimensionStyle;
      });
    }
      #endregion



    // cref: ArcGIS.Desktop.Mapping.StyleItem.SetObject(System.Object)
    // cref: ArcGIS.Desktop.Mapping.StyleItem.SetObject(System.Object)
    #region Creates a new style item and sets its properties from an existing style item
    /// <summary>
    /// Creates a new <see cref="StyleItem"/> and initializes its properties based on an existing <see
    /// cref="StyleItem"/>.
    /// </summary>
    /// <remarks>The new <see cref="StyleItem"/> will have its object set using the <see
    /// cref="StyleItem.GetObject"/> method of the  <paramref name="existingItem"/>. However, the <c>Key</c>,
    /// <c>Name</c>, <c>Tags</c>, and <c>Category</c> properties  of the new item must be explicitly set, as they are
    /// not populated from the object passed to <see cref="StyleItem.SetObject"/>.</remarks>
    /// <param name="existingItem">The existing <see cref="StyleItem"/> from which the new item's properties will be derived.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see
    /// cref="StyleItem"/>.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="existingItem"/> is <see langword="null"/>.</exception>
    public static Task<StyleItem> CreateNewStyleItemAsync(StyleItem existingItem)
    {
      if (existingItem == null)
        throw new System.ArgumentNullException();

      return QueuedTask.Run(() =>
      {
        StyleItem item = new StyleItem();

        //Get object from existing style item
        object obj = existingItem.GetObject();

        //New style item's properties are set from the existing item
        item.SetObject(obj);

        //Key, Name, Tags and Category for the new style item have to be set
        //These values are NOT populated from the object passed in to SetObject
        item.Key = "KeyOfTheNewItem";
        item.Name = "NameOfTheNewItem";
        item.Tags = "TagsForTheNewItem";
        item.Category = "CategoryOfTheNewItem";

        return item;
      });
    }
    #endregion
  }
}
