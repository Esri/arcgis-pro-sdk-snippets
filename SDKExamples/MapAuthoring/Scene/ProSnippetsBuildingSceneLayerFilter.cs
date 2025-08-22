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
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Data;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippetsBuildingSceneLayerFilter
  {
    #region ProSnippet Group: Building Scene Layer
    #endregion
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer
    // cref: ArcGIS.Desktop.Mapping.MapMember.Name
    #region Name of BuildingSceneLayer 
    public static void GetBuildingSceneLayerName()
    {
      var bsl = MapView.Active.Map.GetLayersAsFlattenedList()
                        .OfType<BuildingSceneLayer>().FirstOrDefault();
      var scenelayerName = bsl.Name;
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetAvailableFieldsAndValues
    #region Query Building Scene Layer for available Types and Values
    /// <summary>
    /// Queries the specified <see cref="BuildingSceneLayer"/> for available types and values,  such as disciplines,
    /// categories, and building levels.
    /// </summary>
    /// <param name="bsl">The <see cref="BuildingSceneLayer"/> to query. Must not be <see langword="null"/>.</param>
    public static async void QueryBuildingSceneLayerForAvailableTypesAndValues(BuildingSceneLayer bsl)
    {
      await QueuedTask.Run(() =>
        {
          var dict = bsl.GetAvailableFieldsAndValues();

          //get a list of existing disciplines
          var disciplines = dict.SingleOrDefault(
                   kvp => kvp.Key == "Discipline").Value ?? new List<string>();

          //get a list of existing categories
          var categories = dict.SingleOrDefault(
                   kvp => kvp.Key == "Category").Value ?? new List<string>();

          //get a list of existing floors or "levels"
          var floors = dict.SingleOrDefault(
                   kvp => kvp.Key == "BldgLevel").Value ?? new List<string>();
        });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.CreateDefaultFilter
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilters
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
    #region Create a Default Filter and Get Filter Count
    public static void CreateDefaultFilterAndGetFilterCount(BuildingSceneLayer bsl)
    {
      QueuedTask.Run(() => {
        var filter1 = bsl.CreateDefaultFilter();
        var values = filter1.FilterBlockDefinitions[0].SelectedValues;
        //values will be a single value for the type
        //"CreatedPhase", value "New Construction"

        //There will be at least one filter after "CreateDefaultFilter()" call
        var filtersCount = bsl.GetFilters().Count;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilters
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
    // cref: ArcGIS.Core.CIM.Object3DRenderingMode
    #region Get all the Filters that Contain WireFrame Blocks
    public static void GetFiltersWithWireFrameBlocks(BuildingSceneLayer bsl)
    {
      QueuedTask.Run( () => {
        //Note: wire_frame_filters can be null in this example
        var wire_frame_filters = bsl.GetFilters().Where(
          f => f.FilterBlockDefinitions.Any(
            fb => fb.FilterBlockMode == Object3DRenderingMode.Wireframe));
        //substitute Object3DRenderingMode.None to get blocks with a solid mode (default)
        //and...
        //fb.FilterBlockMode == Object3DRenderingMode.Wireframe &&
        //fb.FilterBlockMode == Object3DRenderingMode.None
        //for blocks with both
        });
      }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.HasFilter
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.SetActiveFilter
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetActiveFilter
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.ClearActiveFilter
    #region Set and Clear Active Filter for BuildingSceneLayer
    public static void SetAndClearActiveFilter(BuildingSceneLayer bsl, FilterDefinition filter)
    {
      QueuedTask.Run(() => {
        //Note: Use HasFilter to check if a given filter id exists in the layer
        if (bsl.HasFilter(filter.ID))
          bsl.SetActiveFilter(filter.ID);
        var activeFilter = bsl.GetActiveFilter();

        //Clear the active filter
        bsl.ClearActiveFilter();
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetFilter
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
    #region Get BuildingSceneLayer Filter ID and Filter    
    public static void GetBuildingSceneLayerFilter(BuildingSceneLayer bsl, string filterID)
    {
      var filterDefinition = bsl.GetFilter(filterID);
      //or via Linq
      //var filter = bsl.GetFilters().FirstOrDefault(f => f.ID == filterID1);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Name
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Description
    #region Modify BuildingSceneLayer Filter Name and Description
    /// <summary>
    /// Updates the name and description of a filter in a <see cref="BuildingSceneLayer"/>.
    /// </summary>  
    /// <param name="bsl">The <see cref="BuildingSceneLayer"/> containing the filter to be updated.</param>
    /// <param name="filter">The <see cref="FilterDefinition"/> whose name and description will be modified.</param>
    public static void ModifyBuildingSceneLayerFilterNameAndDescription(BuildingSceneLayer bsl, FilterDefinition filter)
    {
      QueuedTask.Run(() => { 

      filter.Name = "Updated Filter Name";
        filter.Description = "Updated Filter description";
        bsl.UpdateFilter(filter);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.GetAvailableFieldsAndValues
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.SetActiveFilter
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Name
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.Description
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.Title
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
    #region Create a Filter using Building Level and Category
    /// <summary>
    /// Creates and applies a filter to the specified <see cref="BuildingSceneLayer"/> based on building levels and
    /// categories.
    /// </summary>
    /// <param name="bsl">The <see cref="BuildingSceneLayer"/> to which the filter will be applied. Cannot be null.</param>
    public static void CreateFilterUsingBuildingLevelAndCategory(BuildingSceneLayer bsl)
    {
      QueuedTask.Run(() =>
      {
        //refer to "Query Building Scene Layer for available Types and Values
        var dict = bsl.GetAvailableFieldsAndValues();
        var categories = dict.SingleOrDefault(kvp => kvp.Key == "Category").Value;
        //get a list of existing floors or "levels"
        var floors = dict.SingleOrDefault(kvp => kvp.Key == "BldgLevel").Value;

        //Make a new filter definition
        var fd = new FilterDefinition()
        {
          Name = "Floor and Category Filter",
          Description = "Example filter",
        };
        //Set up the values for the filter
        var filtervals = new Dictionary<string, List<string>>();
        filtervals.Add("BldgLevel", new List<string>() { floors[0] });
        var category_vals = categories.Where(v => v == "Walls" || v == "Stairs").ToList() ?? new List<string>();
        if (category_vals.Count() > 0)
        {
          filtervals.Add("Category", category_vals);
        }
        //Create a solid block (other option is "Wireframe")
        var fdef = new FilterBlockDefinition()
        {
          FilterBlockMode = Object3DRenderingMode.None,
          Title = "Solid Filter",
          SelectedValues = filtervals//Floor and Category
        };
        //Apply the block
        fd.FilterBlockDefinitions = new List<FilterBlockDefinition>() { fdef };
        //Add the filter definition to the layer
        bsl.UpdateFilter(fd);
        //Set it active. The ID is auto-generated
        bsl.SetActiveFilter(fd.ID);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.UpdateFilter
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.FilterBlockDefinitions
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.FilterBlockMode
    // cref: ArcGIS.Desktop.Mapping.FilterBlockDefinition.SelectedValues
    // cref: ArcGIS.Core.CIM.Object3DRenderingMode
    #region Modify BuildingSceneLayer Filter Block
    /// <summary>
    /// Modifies the filter block of a specified <see cref="BuildingSceneLayer"/> using the provided <see
    /// cref="FilterDefinition"/>.
    /// </summary>
    /// <param name="bsl">The <see cref="BuildingSceneLayer"/> to modify. This layer must already exist and be valid.</param>
    /// <param name="filter">The <see cref="FilterDefinition"/> to update with the new filter block configuration.</param>
    public static void ModifyBuildingSceneLayerFilterBlock(BuildingSceneLayer bsl, FilterDefinition filter)
    {
      QueuedTask.Run(() =>
      {
        var filterBlock = new FilterBlockDefinition();
        filterBlock.FilterBlockMode = Object3DRenderingMode.Wireframe;

        var selectedValues = new Dictionary<string, List<string>>();
        //We assume QueryAvailableFieldsAndValues() contains "Walls" and "Doors"
        //For 'Category'
        selectedValues["Category"] = new List<string>() { "Walls", "Doors" };
        filterBlock.SelectedValues = selectedValues;

        //Overwrite
        filter.FilterBlockDefinitions = new List<FilterBlockDefinition>() { filterBlock };
        bsl.UpdateFilter(filter);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.HasFilter
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.RemoveFilter
    // cref: ArcGIS.Desktop.Mapping.BuildingSceneLayer.RemoveAllFilters
    // cref: ArcGIS.Desktop.Mapping.FilterDefinition.ID
    #region Remove BuildingSceneLayer Filter
    /// <summary>
    /// Removes a specific filter from the specified <see cref="BuildingSceneLayer"/> or removes all filters if the
    /// specified filter does not exist.
    /// </summary>
    public static void RemoveBuildingSceneLayerFilter(BuildingSceneLayer bsl, FilterDefinition filter)
    {
      QueuedTask.Run(() =>
      {
        //Note: Use HasFilter to check if a given filter id exists in the layer
        if (bsl.HasFilter(filter.ID))
          bsl.RemoveFilter(filter.ID);
        //Or remove all filters
        bsl.RemoveAllFilters();
      });
    }
    #endregion
  }
}