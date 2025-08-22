
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

// Ignore Spelling: voxel

using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Realtime;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Layouts.Utilities;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using ArcGIS.Desktop.Mapping.Voxel;
using ArcGIS.Desktop.Mapping.Voxel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
	public static class ProSnippetsVoxelLayers
	{
		#region ProSnippet Group: Create Voxel Layer
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer
		// cref: ArcGIS.Desktop.Mapping.MapView.ViewingMode
		// cref: ArcGIS.Core.CIM.MapViewingMode
		#region Check if a Voxel Layer can be created
		/// <summary>
		/// Determines whether a voxel layer can be created in the specified map.
		/// </summary>
		/// <param name="map">The map to check. The map must be a local scene for a voxel layer to be created.</param>
		public static async void CheckIfVoxelLayerCanBeCreated(Map map)
		{
			//Map must be a local scene
			bool canCreateVoxel = (MapView.Active.ViewingMode == MapViewingMode.SceneLocal);

			if (canCreateVoxel)
			{
				//TODO - use the voxel api methods
			}
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer
		// cref: ArcGIS.Core.CIM.CIMVoxelDataConnection
		// cref: ArcGIS.Core.CIM.CIMVoxelDataConnection.URI
		// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams
		// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams.Create(ArcGIS.Core.CIM.CIMVoxelDataConnection)
		// cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
		// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams.Variables
		// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams
		// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.Variable
		// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.DataType
		// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.Description
		// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.IsDefault
		// cref: ArcGIS.Desktop.Mapping.VoxelVariableCreationParams.IsSelected
		// cref: ArcGIS.Desktop.Mapping.VoxelLayerCreationParams.SetDefaultVariable(ArcGIS.Desktop.Mapping.VoxelVariableCreationParams)
		// cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer
		// cref: ArcGIS.Desktop.Mapping.LayerFactory
		#region Create Voxel Layer
		/// <summary>
		/// Creates a voxel layer from a NetCDF file and adds it to the specified map.
		/// </summary>
		/// <param name="map">The map to which the voxel layer will be added. Must be a local scene.</param>
		public static async void CreateVoxelLayer(Map map)
		{
			await QueuedTask.Run(() =>
			{
				//Must be a .NetCDF file for voxels
				var url = @"C:\MyData\AirQuality_Redlands.nc";
				var cim_connection = new CIMVoxelDataConnection()
				{
					URI = url
				};
				//Create a VoxelLayerCreationParams
				var createParams = VoxelLayerCreationParams.Create(cim_connection);
				createParams.IsVisible = true;

				//Can also just use the path directly...
				//var createParams = VoxelLayerCreationParams.Create(url);

				//Use VoxelLayerCreationParams to enumerate the variables within
				//the voxel
				var variables = createParams.Variables;
				foreach (var variable in variables)
				{
					var line = $"{variable.Variable}: {variable.DataType}, " +
						 $"{variable.Description}, {variable.IsDefault}, {variable.IsSelected}";
					System.Diagnostics.Debug.WriteLine(line);
				}
				//Optional: set the default variable
				createParams.SetDefaultVariable(variables.Last());

				//Create the layer - map must be a local scene
				VoxelLayer voxelLayer = LayerFactory.Instance.CreateLayer<VoxelLayer>(createParams, map);
			});
		}
		#endregion


		#region ProSnippet Group: Voxel Layer settings and properties
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer
		#region Get a Voxel Layer from the TOC
		/// <summary>
		/// Retrieves a voxel layer from the Table of Contents (TOC) of the specified map.
		/// </summary>
		/// <param name="map">The map from which to retrieve the voxel layer. This parameter is required to access the TOC.</param>
		public static void GetVoxelLayerFromTOC(Map map)
		{
			//Get selected layer if a voxel layer is selected
			var voxelLayer = MapView.Active.GetSelectedLayers().OfType<VoxelLayer>().FirstOrDefault();
			if (voxelLayer == null)
			{
				//just get the first voxel layer in the TOC
				voxelLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<VoxelLayer>().FirstOrDefault();
				if (voxelLayer == null)
					return;
			}
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer
		// cref: ArcGIS.Desktop.Mapping.Layer.SetExpanded
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetIsosurfaceContainerExpanded(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetIsosurfaceContainerVisibility(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerExpanded(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSectionContainerExpanded(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSectionContainerVisibility(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetLockedSectionContainerExpanded(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetLockedSectionContainerVisibility(System.Boolean)
		#region Manipulate the Voxel Layer TOC Group

		/// <summary>
		/// Sets the visibility, toggles containers and expansion state of the voxel layer and its containers in the Table of Contents (TOC).
		/// </summary>
		/// <param name="voxelLayer"></param>

		public static void ManipulateVoxelLayerTOCGroup(VoxelLayer voxelLayer)
		{
			//Toggle containers and visibility in the TOC
			QueuedTask.Run(() =>
			{
				voxelLayer.SetExpanded(!voxelLayer.IsExpanded);
				voxelLayer.SetVisibility(!voxelLayer.IsVisible);
				voxelLayer.SetIsosurfaceContainerExpanded(!voxelLayer.IsIsosurfaceContainerExpanded);
				voxelLayer.SetIsosurfaceContainerVisibility(!voxelLayer.IsIsosurfaceContainerVisible);
				voxelLayer.SetSliceContainerExpanded(voxelLayer.IsSliceContainerExpanded);
				voxelLayer.SetSliceContainerVisibility(!voxelLayer.IsSliceContainerVisible);
				voxelLayer.SetSectionContainerExpanded(!voxelLayer.IsSectionContainerExpanded);
				voxelLayer.SetSectionContainerVisibility(!voxelLayer.IsSectionContainerVisible);
				voxelLayer.SetLockedSectionContainerExpanded(!voxelLayer.IsLockedSectionContainerExpanded);
				voxelLayer.SetLockedSectionContainerVisibility(!voxelLayer.IsLockedSectionContainerVisible);
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedIsosurfaces()
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition
		// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSlices()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
		// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
		// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelSection( ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
		// cref: ArcGIS.Desktop.Mapping.MapView.SelectVoxelLockedSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
		#region Get/Set Selected Voxel Assets from the TOC
		/// <summary>
		/// Gets and sets the selected voxel assets (isosurfaces, slices, sections, locked sections) from the Table of Contents (TOC).	
		/// </summary>
		public static void GetSetSelectedVoxelAssetsFromTOC()
		{

			var surfaces = MapView.Active.GetSelectedIsosurfaces();
			//set selected w/ MapView.Active.SelectVoxelIsosurface(isoSurface)
			var slices = MapView.Active.GetSelectedSlices();
			//set selected w/ MapView.Active.SelectVoxelSlice(slice)
			var sections = MapView.Active.GetSelectedSections();
			//set selected w/ MapView.Active.SelectVoxelSection(section)
			var locked_sections = MapView.Active.GetSelectedLockedSections();
			//set selected w/ MapView.Active.SelectVoxelLockedSection(locked_section)
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVisualization(ArcGIS.Core.CIM.VoxelVisualization)
		// cref: ArcGIS.Core.CIM.VoxelVisualization
		#region Change the Voxel Visualization
		/// <summary>
		/// Changes the visualization mode of the specified voxel layer.
		/// </summary>		
		/// <param name="voxelLayer">The voxel layer whose visualization mode will be changed.  This parameter cannot be null.</param>
		public static void ChangeVoxelVisualization(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				//Change the visualization to Volume
				//e.g. for creating slices
				voxelLayer.SetVisualization(VoxelVisualization.Volume);

				//Change the visualization to Surface
				//e.g. to create isosurfaces and sections
				voxelLayer.SetVisualization(VoxelVisualization.Surface);
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.CartographicOffset
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetCartographicOffset(System.Double)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.VerticalExaggeration
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVerticalExaggeration(System.Double)
		// cref: ArcGIS.Core.CIM.CIM3DLayerProperties.ExaggerationMode
		// cref: ArcGIS.Core.CIM.CIM3DLayerProperties.VerticalExaggeration
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.IsDiffuseLightingEnabled
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetDiffuseLightingEnabled(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.DiffuseLighting
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetDiffuseLighting(System.Double)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.IsSpecularLightingEnabled
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSpecularLightingEnabled(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SpecularLighting
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSpecularLighting(System.Double)
		#region Lighting Properties, Offset, Vertical Exaggeration
		/// <summary>
		/// Modifies the lighting properties, cartographic offset, and vertical exaggeration of the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> to modify. This parameter cannot be null.</param>

		public static void ChangeLightingProperties(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				//Offset
				var offset = voxelLayer.CartographicOffset;
				//apply an offset
				voxelLayer.SetCartographicOffset(offset + 100.0);

				//VerticalExaggeration
				var exaggeration = voxelLayer.VerticalExaggeration;
				//apply an exaggeration
				voxelLayer.SetVerticalExaggeration(exaggeration + 100.0);

				//Change the exaggeration mode to "ScaleZ" - corresponds to 'Z-coordinates' 
				//on the Layer properties UI - must use the CIM
				var def = voxelLayer.GetDefinition() as CIMVoxelLayer;
				def.Layer3DProperties.ExaggerationMode = ExaggerationMode.ScaleZ;
				//can set vertical exaggeration via the CIM also
				//def.Layer3DProperties.VerticalExaggeration = exaggeration + 100.0;

				//apply the change
				voxelLayer.SetDefinition(def);

				//Diffuse Lighting
				if (!voxelLayer.IsDiffuseLightingEnabled)
					voxelLayer.SetDiffuseLightingEnabled(true);
				var diffuse = voxelLayer.DiffuseLighting;
				//set Diffuse lighting to a value between 0 and 1
				voxelLayer.SetDiffuseLighting(0.5); //50%

				//Specular Lighting
				if (!voxelLayer.IsSpecularLightingEnabled)
					voxelLayer.SetSpecularLightingEnabled(true);
				var specular = voxelLayer.SpecularLighting;
				//set Diffuse lighting to a value between 0 and 1
				voxelLayer.SetSpecularLighting(0.5); //50%
			});

		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVolumes
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
		#region Get the Voxel Volume Dimensions
		/// <summary>
		/// Gets the dimensions of the voxel volumes in the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void GetVoxelVolumeDimensions(VoxelLayer voxelLayer)
		{
			var x_max = voxelLayer.GetVolumes().Max(v => v.GetVolumeSize().X);
			var y_max = voxelLayer.GetVolumes().Max(v => v.GetVolumeSize().Y);
			var z_max = voxelLayer.GetVolumes().Max(v => v.GetVolumeSize().Z);

			//Get the dimensions of just one volume
			var dimensions = voxelLayer.GetVolumes().FirstOrDefault();
			//Get the dimensions of the volume associated with the selected variable
			var dimensions2 = voxelLayer.SelectedVariableProfile.Volume.GetVolumeSize();
		}
		#endregion


		#region ProSnippet Group: Events
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Variable
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Renderer
		// cref: ArcGIS.Desktop.Mapping.Events.MapMemberEventHint
		// cref: ArcGIS.Desktop.Mapping.Events.MapMemberPropertiesChangedEventArgs.EventHints
		// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetChangedEvent.Subscribe
		// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs
		// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.ChangeType
		// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.AssetType
		// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.VoxelAssetChangeType
		// cref: ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetEventArgs.VoxelAssetType
		#region Subscribe for Changes to a Voxel Layer
		/// <summary>
		/// Subscribes to events that notify changes to a voxel layer, including variable profile selection, renderer updates,
		/// and asset modifications such as isosurfaces, slices, and sections.
		/// </summary>
		public static void SubscribeForChangesToVoxelLayer()
		{
			ArcGIS.Desktop.Mapping.Events.MapMemberPropertiesChangedEvent.Subscribe((args) =>
			{
				var voxel = args.MapMembers.OfType<VoxelLayer>().FirstOrDefault();
				if (voxel == null)
					return;
				//Anything changed on a voxel layer?
				if (args.EventHints.Any(hint => hint == MapMemberEventHint.VoxelSelectedVariable))
				{
					//Voxel variable profile selection changed
					var changed_variable_name = voxel.SelectedVariableProfile.Variable;
					//TODO respond to change, use QueuedTask if needed

				}
				else if (args.EventHints.Any(hint => hint == MapMemberEventHint.Renderer))
				{
					//This can fire when a renderer becomes ready on a new layer; the selected variable profile
					//is changed; visualization is changed, etc.
					var renderer = voxel.SelectedVariableProfile.Renderer;
					//TODO respond to change, use QueuedTask if needed

				}
			});

			ArcGIS.Desktop.Mapping.Voxel.Events.VoxelAssetChangedEvent.Subscribe((args) =>
			{
				//An asset changed on a voxel layer
				System.Diagnostics.Debug.WriteLine("");
				System.Diagnostics.Debug.WriteLine("VoxelAssetChangedEvent");
				System.Diagnostics.Debug.WriteLine($" AssetType: {args.AssetType}, ChangeType: {args.ChangeType}");

				if (args.ChangeType == VoxelAssetEventArgs.VoxelAssetChangeType.Remove)
					return;
				//Get "what"changed - add or update
				//eg IsoSurface
				VoxelLayer voxelLayer = null;
				if (args.AssetType == VoxelAssetEventArgs.VoxelAssetType.Isosurface)
				{
					var surface = MapView.Active.GetSelectedIsosurfaces().FirstOrDefault();
					//there will only be one selected...
					if (surface != null)
					{
						voxelLayer = surface.Layer;
						//TODO respond to change, use QueuedTask if needed
					}
				}
				//Repeat for Slices, Sections, LockedSections...
				//GetSelectedSlices(), GetSelectedSections(), GetSelectedLockedSections();
			});
		}
		#endregion


		#region ProSnippet Group: Variable Profiles + Renderer
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Variable
		#region Get the Selected Variable Profile
		/// <summary>
		/// Retrieves the selected variable profile from the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer from which to retrieve the selected variable profile. Cannot be <see langword="null"/>.</param>
		public static void GetSelectedVariableProfile(VoxelLayer voxelLayer)
		{
			var sel_profile = voxelLayer.SelectedVariableProfile;

			//Get the variable profile name
			var profile_name = sel_profile.Variable;
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Variable
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSelectedVariableProfile(ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile)
		#region Change the Selected Variable Profile
		/// <summary>
		/// Changes the selected variable profile of the specified <see cref="VoxelLayer"/> to a different profile.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> whose selected variable profile is to be changed. Cannot be <see langword="null"/>.</param>
		public static void ChangeSelectedVariableProfile(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				var profiles = voxelLayer.GetVariableProfiles();
				var sel_profile = voxelLayer.SelectedVariableProfile;
				//Select any profile as long as it is not the current selected variable
				var not_selected = profiles.Where(p => p.Variable != sel_profile.Variable).ToList();
				if (not_selected.Count() > 0)
				{
					voxelLayer.SetSelectedVariableProfile(not_selected.First());
				}
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile
		#region Get the Variable Profiles
		/// <summary>
		/// Retrieves the variable profiles associated with the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer from which to retrieve the variable profiles.  This parameter cannot be <see langword="null"/>.</param>
		public static void GetVariableProfiles(VoxelLayer voxelLayer)
		{

			var variable_profiles = voxelLayer.GetVariableProfiles();
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Renderer
		// cref: ArcGIS.Core.CIM.CIMVoxelRenderer
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DataType
		// cref: ArcGIS.Core.CIM.VoxelVariableDataType
		// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer
		// cref: ArcGIS.Core.CIM.CIMVoxelUniqueValueRenderer
		#region Get the Variable Renderer
		/// <summary>
		/// Gets the renderer for the first variable profile in the specified voxel layer.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="voxelLayer"></param>
		public static void GetVariableRenderer(Map map, VoxelLayer voxelLayer)
		{
			var variable = voxelLayer.GetVariableProfiles().First();
			var renderer = variable.Renderer;
			if (variable.DataType == VoxelVariableDataType.Continuous)
			{
				//Renderer will be stretch
				var stretchRenderer = renderer as CIMVoxelStretchRenderer;
				//access the renderer

			}
			else //VoxelVariableDataType.Discrete
			{
				//Renderer will be unique value
				var uvr = renderer as CIMVoxelUniqueValueRenderer;
				//access the renderer
			}
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SelectedVariableProfile
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Statistics
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DataType
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Renderer
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MinimumValue
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MaximumValue
		// cref: ArcGIS.Core.CIM.VoxelVariableDataType
		// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMax
		// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMin
		#region Access Stats and Color Range for a Stretch Renderer
		/// <summary>
		/// Accesses the statistics and color range of the selected variable profile in the specified voxel layer.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="voxelLayer"></param>
		public static void AccessStatsColorRangeStretchRenderer(Map map, VoxelLayer voxelLayer)
		{
			//Get the variable profile on which to access the data
			var variable = voxelLayer.SelectedVariableProfile;
			//or use ...voxelLayer.GetVariableProfiles()

			//Data range
			var min = variable.Statistics.MinimumValue;
			var max = variable.Statistics.MaximumValue;

			//Color range (Continuous only)
			double color_min, color_max;
			if (variable.DataType == VoxelVariableDataType.Continuous)
			{
				var renderer = variable.Renderer as CIMVoxelStretchRenderer;
				color_min = renderer.ColorRangeMin;
				color_max = renderer.ColorRangeMax;
			}
		}
		#endregion

		// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMax
		// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRangeMin
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MinimumValue
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableStatistics.MAximumValue
		#region Change Stretch Renderer Color Range
		/// <summary>
		/// Adjusts the color range of the stretch renderer for the selected variable profile in the specified voxel layer.
		/// </summary>
		/// <param name="map">The map containing the voxel layer. This parameter is required to ensure the operation is performed within the
		/// context of the map.</param>
		/// <param name="voxelLayer">The voxel layer whose selected variable profile's stretch renderer will be modified. The layer must have a
		/// selected variable profile with a continuous data type.</param>
		public static void ChangeStretchRendererColorRange(Map map, VoxelLayer voxelLayer)
		{
			//Typically, the default color range covers the most
			//commonly occuring voxel values. Usually, the data
			//range is much broader than the color range
			QueuedTask.Run(() => {
				//Get the variable profile whose renderer will be changed
				var variable = voxelLayer.SelectedVariableProfile;

				//Check DataType
				if (variable.DataType != VoxelVariableDataType.Continuous)
					return;//must be continuous to have a Stretch Renderer...

				var renderer = variable.Renderer as CIMVoxelStretchRenderer;
				var color_min = renderer.ColorRangeMin;
				var color_max = renderer.ColorRangeMax;
				//increase range by 10% of the current difference
				var dif = (color_max - color_min) * 0.05;
				color_min -= dif;
				color_max += dif;

				//make sure we do not exceed data range

				if (color_min < variable.Statistics.MinimumValue)
					color_min = variable.Statistics.MinimumValue;
				if (color_max > variable.Statistics.MaximumValue)
					color_max = variable.Statistics.MaximumValue;

				//variable.Statistics.MinimumValue
				renderer.ColorRangeMin = color_min;
				renderer.ColorRangeMax = color_max;

				//apply changes
				variable.SetRenderer(renderer);
			});
		}
		#endregion

		// cref: ArcGIS.Core.CIM.CIMVoxelUniqueValueRenderer
		// cref: ArcGIS.Core.CIM.CIMVoxelUniqueValueRenderer.Classes
		// cref: ArcGIS.Core.CIM.CIMVoxelColorUniqueValue
		// cref: ArcGIS.Core.CIM.CIMVoxelColorUniqueValue.Visible
		#region Change The Visibility on a CIMVoxelColorUniqueValue class
		/// <summary>
		/// Changes the visibility of the first <see cref="ArcGIS.Core.CIM.CIMVoxelColorUniqueValue"/> class  in the renderer
		/// of the selected variable profile of the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer whose selected variable profile's renderer will be modified.</param>
		public static void ChangeVisibilityCIMVoxelColorUniqueValueClass(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				//Get the variable profile whose renderer will be changed
				var variable = voxelLayer.SelectedVariableProfile;

				//Check DataType
				if (variable.DataType != VoxelVariableDataType.Discrete)
					return;//must be Discrete to have a UV Renderer...

				var renderer = variable.Renderer as CIMVoxelUniqueValueRenderer;

				//A CIMVoxelUniqueValueRenderer consists of a collection of
				//CIMVoxelColorUniqueValue classes - one per discrete value
				//in the associated variable profile value array

				//Get the first class
				var classes = renderer.Classes.ToList();
				var unique_value_class = classes.First();
				//Set its visibility off
				unique_value_class.Visible = false;

				//Apply the change to the renderer
				renderer.Classes = classes.ToArray();
				//apply the changes
				variable.SetRenderer(renderer);
			});
		}
		#endregion



		#region ProSnippet Group: IsoSurfaces
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.MaxNumberOfIsosurfaces
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaces()
		#region Check the MaxNumberofIsoSurfaces for a Variable
		/// <summary>
		/// Checks whether the maximum number of isosurfaces for the first variable in the specified voxel layer has been
		/// reached.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer containing the variable to check. Cannot be null.</param>
		public static void CheckMaxNumberOfIsoSurfaces(VoxelLayer voxelLayer)
		{
			var variable = voxelLayer.GetVariableProfiles().First();
			var max = variable.MaxNumberOfIsosurfaces;
			if (max >= variable.GetIsosurfaces().Count)
			{
				//no more surfaces can be created on this variable
			}
		}
		#endregion


		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfiles
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DataType
		#region Check a Variable's Datatype
		/// <summary>
		/// Checks the data type of the first variable in the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer containing the variable profiles to check. Cannot be null.</param>
		public static void CheckVariablesDataType(VoxelLayer voxelLayer)
		{
			var variable = voxelLayer.GetVariableProfiles().First();
			if (variable.DataType != VoxelVariableDataType.Continuous)
			{
				//No iso surfaces
				//Iso surface can only be created for VoxelVariableDataType.Continuous
			}

			#endregion
		}
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.Visualization
		// cref: ArcGIS.Core.CIM.VoxelVisualization
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CanCreateIsosurface
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVisualization(ArcGIS.Core.CIM.VoxelVisualization)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CanCreateIsosurface
		#region Check CanCreateIsoSurface
		public static void CheckCanreateIsoSurface(VoxelLayer voxelLayer)
		{
			//Visualization must be surface or CanCreateIsosurface will return
			//false
			if (voxelLayer.Visualization != VoxelVisualization.Surface)
				voxelLayer.SetVisualization(VoxelVisualization.Surface);

			//Get the variable profile on which to create the iso surface
			var variable = voxelLayer.SelectedVariableProfile;
			//or use ...voxelLayer.GetVariableProfiles().First(....

			// o Visualization must be Surface
			// o Variable profile must be continuous
			// o Variable MaxNumberofIsoSurfaces must not have been reached...
			if (variable.CanCreateIsosurface)
			{
				//Do the create
			}
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.Visualization
		// cref: ArcGIS.Core.CIM.VoxelVisualization
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetVisualization(ArcGIS.Core.CIM.VoxelVisualization)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CanCreateIsosurface
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.CreateIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Value
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.IsVisible
		#region Create Isosurface
		public static void CreateIsoSurface(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{

				//Visualization must be surface
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);

				//Get the variable profile on which to create the iso surface
				var variable = voxelLayer.SelectedVariableProfile;

				// o Visualization must be Surface
				// o Variable profile must be continuous
				// o Variable MaxNumberofIsoSurfaces must not have been reached...
				if (variable.CanCreateIsosurface)
				{
					//Note: calling create if variable.CanCreateIsosurface == false
					//will trigger an InvalidOperationException

					//Specify a voxel value for the iso surface

					var min = variable.Statistics.MinimumValue;
					var max = variable.Statistics.MaximumValue;
					var mid = (max + min) / 2;

					//color range (i.e. values that are being rendered)
					var renderer = variable.Renderer as CIMVoxelStretchRenderer;
					var color_min = renderer.ColorRangeMin;
					var color_max = renderer.ColorRangeMax;

					//keep the surface within the current color range (or it
					//won't render)
					if (mid < color_min)
					{
						mid = renderer.ColorRangeMin;
					}
					else if (mid > color_max)
					{
						mid = renderer.ColorRangeMax;
					}

					//Create the iso surface
					var suffix = Math.Truncate(mid * 100) / 100;
					variable.CreateIsosurface(new IsosurfaceDefinition()
					{
						Name = $"Surface {suffix}",
						Value = mid,
						IsVisible = true
					});
				}
			});
		}

		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaces()
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Value
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.Color
		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.IsCustomColor
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.UpdateIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
		// cref: ArcGIS.Core.CIM.CIMVoxelStretchRenderer.ColorRamp
		// cref: ArcGIS.Desktop.Mapping.ColorFactory.GenerateColorsFromColorRamp
		#region How to Change Value and Color on an Isosurface
		/// <summary>
		/// Modifies the value and color of the first isosurface in the selected variable profile of the specified voxel
		/// layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer containing the selected variable profile and isosurfaces to modify.</param>
		public static void ChangeValueColorIsosurface(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				var variable = voxelLayer.SelectedVariableProfile;

				//Change the color of the first surface for the given profile
				var surface = variable.GetIsosurfaces().FirstOrDefault();
				if (surface != null)
				{
					if (voxelLayer.Visualization != VoxelVisualization.Surface)
						voxelLayer.SetVisualization(VoxelVisualization.Surface);

					//Change the iso surface voxel value
					surface.Value = surface.Value * 0.9;

					//get a random color
					var count = new Random().Next(0, 100);
					var colors = ColorFactory.Instance.GenerateColorsFromColorRamp(
						((CIMVoxelStretchRenderer)variable.Renderer).ColorRamp, count);

					var idx = new Random().Next(0, count - 1);
					surface.Color = colors[idx];
					//set the custom color flag true to lock the color
					//locking the color prevents it from being changed if the
					//renderer color range or color theme is updated
					surface.IsCustomColor = true;

					//update the surface
					variable.UpdateIsosurface(surface);
				}
			});
		}

		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition.IsCustomColor
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaceColor(System.Double)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.UpdateIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
		#region Change Isourface Color Back to Default
		/// <summary>
		/// Resets the color of the first isosurface in the selected variable profile of the specified voxel layer to its
		/// default value.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer containing the selected variable profile whose isosurface color will be reset.</param>
		public static void ChangeIsosurfaceColorBackToDefault(VoxelLayer voxelLayer)
		{
			var variable = voxelLayer.SelectedVariableProfile;
			//Change the color of the first surface for the given profile
			var surface = variable.GetIsosurfaces().FirstOrDefault();
			QueuedTask.Run(() =>
			{
				if (surface.IsCustomColor)
				{
					surface.Color = variable.GetIsosurfaceColor((double)surface.Value);
					surface.IsCustomColor = false;
					//update the surface
					variable.UpdateIsosurface(surface);
				}
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.GetIsosurfaces()
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.DeleteIsosurface(ArcGIS.Desktop.Mapping.Voxel.IsosurfaceDefinition)
		#region Delete Isosurfaces
		/// <summary>
		/// Deletes all isosurfaces from the selected variable profile of the specified voxel layer.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="voxelLayer"></param>
		public static void DeleteIsosurfaces(Map map, VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				var variable = voxelLayer.SelectedVariableProfile;

				//delete the last surface
				var last_surface = variable.GetIsosurfaces().LastOrDefault();

				if (last_surface != null)
				{
					variable.DeleteIsosurface(last_surface);
				}

				//delete all the surfaces
				foreach (var surface in variable.GetIsosurfaces())
					variable.DeleteIsosurface(surface);

				//Optional - set visualization back to Volume
				if (variable.GetIsosurfaces().Count() == 0)
				{
					voxelLayer.SetVisualization(VoxelVisualization.Volume);
				}
			});
		}
		#endregion

		#region ProSnippet Group: Slices
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Volume
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSlices()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.IsVisible
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerExpanded(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility(System.Boolean)
		#region Get the Collection of Slices
		/// <summary>
		/// Gets the collection of slices from the selected variable profile's volume in the specified voxel layer,
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void GetCollectionOfSlices(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				//Use the SelectedVariableProfile to get the slices currently in the TOC
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var slices = volume.GetSlices();

				//Do something... e.g. make them visible
				foreach (var slice in slices)
				{
					slice.IsVisible = true;
					volume.UpdateSlice(slice);
				}

				//expand the slice container and make sure container visibility is true
				voxelLayer.SetSliceContainerExpanded(true);
				voxelLayer.SetSliceContainerVisibility(true);
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile.Volume
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSlices()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
		#region Get a Slice
		/// <summary>
		/// Retrieves a slice from the specified voxel layer based on the provided slice identifier.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer from which the slice will be retrieved. This must not be <see langword="null"/>.</param>
		/// <param name="my_slice_id">The identifier of the slice to retrieve. This must match the ID of an existing slice in the voxel layer.</param>
		public static void GetASlice(VoxelLayer voxelLayer, string my_slice_id)
		{
			//Use the SelectedVariableProfile to get the slices currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;
			var slice = volume.GetSlices().FirstOrDefault();
			var slice2 = volume.GetSlices().First(s => s.Id == my_slice_id);
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSlices()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
		#region Get Selected Slice in TOC
		/// <summary>
		/// Get Selected Slice in the Table of Contents (TOC).
		/// </summary>
		public static void GetSelectedSliceInTOC()
		{
			QueuedTask.Run(() => {
				var slice = MapView.Active?.GetSelectedSlices()?.FirstOrDefault();
				if (slice != null)
				{
					//Do something with the slice
				}
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSlices()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Layer
		#region Get Voxel Layer for the Selected Slice in TOC
		/// <summary>
		/// Retrieves the voxel layer associated with the first selected slice in the Table of Contents (TOC).
		/// </summary>
		public static void Example8i()
		{
			QueuedTask.Run(() => {
				VoxelLayer voxelLayer = null;
				var slice = MapView.Active?.GetSelectedSlices()?.FirstOrDefault();
				if (slice != null)
				{
					voxelLayer = slice.Layer;
					//TODO - use the layer
				}
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerExpanded(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility(System.Boolean)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.AutoShowExploreDockPane
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetNormal(System.Double, System.Double)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.#ctor
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.VoxelPosition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.IsVisible
		#region Create a Slice
		/// <summary>
		/// Creates a slice in the specified voxel layer at the midpoint of the volume, using a normal derived from the specified orientation and tilt.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void CreateASlice(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Volume)
					voxelLayer.SetVisualization(VoxelVisualization.Volume);
				voxelLayer.SetSliceContainerExpanded(true);
				voxelLayer.SetSliceContainerVisibility(true);

				//To stop the Voxel Exploration Dockpane activating use:
				voxelLayer.AutoShowExploreDockPane = false;
				//This is useful if u have your own dockpane currently activated...

				//Use the SelectedVariableProfile to get the slices currently in the TOC
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var volumeSize = volume.GetVolumeSize();

				//Orientation 90 degrees (West), Tilt 0.0 (vertical)
				//Convert to a normal
				var normal = voxelLayer.GetNormal(90, 0.0);

				//Create the slice at the voxel mid-point. VoxelPosition
				//is specified in voxel-space coordinates

				//Create the slice on the respective volume
				volume.CreateSlice(new SliceDefinition()
				{
					Name = "Middle Slice",
					VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
					Normal = normal,
					IsVisible = true
				});

				//reset if needed...
				voxelLayer.AutoShowExploreDockPane = true;
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSliceContainerVisibility
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetOrientationAndTilt(ArcGIS.Desktop.Mapping.Voxel.Coordinate3D)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetNormal(System.Double, System.Double)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SliceDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
		#region Change Tilt on a Slice
		public static void ChangeTiltOnASlice(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				//To stop the Voxel Exploration Dockpane activating use:
				voxelLayer.AutoShowExploreDockPane = false;
				//This is useful if u have your own dockpane currently activated...
				//Normally, it would be set in your dockpane

				if (voxelLayer.Visualization != VoxelVisualization.Volume)
					voxelLayer.SetVisualization(VoxelVisualization.Volume);
				voxelLayer.SetSliceContainerVisibility(true);

				//Use the SelectedVariableProfile to get the slices currently in the TOC
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var slice = volume.GetSlices().First(s => s.Name == "Change Tilt Slice");

				(double orientation, double tilt) = voxelLayer.GetOrientationAndTilt(slice.Normal);

				//Convert orientation and tilt to a normal
				slice.Normal = voxelLayer.GetNormal(orientation, 45.0);
				volume.UpdateSlice(slice);

				//reset if needed...Normally this might be when your dockpane
				//was de-activated (ie "closed")
				voxelLayer.AutoShowExploreDockPane = true;
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.DeleteSlice(ArcGIS.Desktop.Mapping.Voxel.SliceDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSlices()
		#region Delete Slice
		/// <summary>
		/// Deletes the last slice from the specified voxel layer's volume, and then deletes all slices from that volume.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void DeleteSlice(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				//Use the SelectedVariableProfile to get the slices currently in the TOC
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;

				var last_slice = volume.GetSlices().LastOrDefault();
				if (last_slice != null)
					volume.DeleteSlice(last_slice);

				//Delete all slices
				var slices = volume.GetSlices();
				foreach (var slice in slices)
					volume.DeleteSlice(slice);
			});
		}
		#endregion

		#region ProSnippet Group: Sections
		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
		#region Get a Section
		/// <summary>
		/// Retrieves a section from the specified voxel layer based on the provided section ID.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer from which the section is retrieved. Cannot be <see langword="null"/>.</param>
		/// <param name="my_section_id">The unique identifier of the section to retrieve. Must match an existing section ID.</param>
		public static void GetASection(VoxelLayer voxelLayer, string my_section_id)
		{
			//Use the SelectedVariableProfile to get the sections currently in the TOC
			//via its associated volume
			var volume = voxelLayer.SelectedVariableProfile.Volume;

			var section = volume.GetSections().FirstOrDefault();
			var section2 = volume.GetSections().First(sec => sec.ID == my_section_id);
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
		#region Get the Current Collection of Sections
		/// <summary>
		/// Configures the specified <see cref="VoxelLayer"/> to display its current collection of sections.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> to configure and retrieve the sections from. Must not be <see langword="null"/>.</param>
		public static void GetCurrentCollectionOfSections(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections currently in the TOC
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var sections = volume.GetSections();
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
		#region Get the Selected Section in TOC
		/// <summary>
		/// Retrieves the first selected section in the Table of Contents (TOC) for the active map view.
		/// </summary>
		public static void GetSelectionSectionInTOC()
		{
			var section = MapView.Active?.GetSelectedSections()?.FirstOrDefault();
			if (section != null)
			{
				//Do something with the section
			}
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Layer
		#region Get Voxel Layer for the Selected Section in TOC
		/// <summary>
		/// Gets the voxel layer associated with the first selected section in the Table of Contents (TOC).
		/// </summary>
		public static void GetVoxelLayerForSelectedSectionInTOC()
		{
			VoxelLayer voxelLayer = null;
			var section = MapView.Active?.GetSelectedSections()?.FirstOrDefault();
			if (section != null)
			{
				voxelLayer = section.Layer;
				//TODO - use the layer
			}
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.AutoShowExploreDockPane
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetNormal(System.Double, System.Double)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
		#region Create a Section at the Voxel MidPoint
		/// <summary>
		/// Creates a vertical section at the midpoint of the voxel volume within the specified <see cref="VoxelLayer"/>.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> on which the section will be created. Cannot be null.</param>
		public static void CreateSectionAtVoxelMidPoint(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//To stop the Voxel Exploration Dockpane activating use:
				voxelLayer.AutoShowExploreDockPane = false;
				//This is useful if u have your own dockpane currently activated...
				//Normally, it would be set in your dockpane

				//Create a section that cuts the volume in two on the vertical plane

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var volumeSize = volume.GetVolumeSize();

				//Orientation 90 degrees (due West), Tilt 0 degrees
				var normal = voxelLayer.GetNormal(90, 0.0);

				//Position must be specified in voxel space

				volume.CreateSection(new SectionDefinition()
				{
					Name = "Middle Section",
					VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
					Normal = normal,
					IsVisible = true
				});

				//reset if needed...Normally this might be when your dockpane
				//was de-activated (ie "closed")
				voxelLayer.AutoShowExploreDockPane = true;
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.CreateHorizontalSectionDefinition()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
		#region Create a Horizontal Section
		/// <summary>
		/// Creates a horizontal section at the midpoint of the voxel volume within the specified <see cref="VoxelLayer"/>.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void CreateHorizontalSection(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Create a section that cuts the volume in two on the horizontal plane

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var volumeSize = volume.GetVolumeSize();

				//Or use normal (0, 0, 1) or (0, 0, -1)...
				var horz_section = SectionDefinition.CreateHorizontalSectionDefinition();

				horz_section.Name = "Horizontal Section";
				horz_section.IsVisible = true;
				horz_section.VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2);

				volume.CreateSection(horz_section);
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
		#region Create Sections in a Circle Pattern
		/// <summary>
		/// Creates a series of sections in a circular pattern around the midpoint of the voxel volume within the specified <see cref="VoxelLayer"/>.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void CreateSectionInACirclePattern(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var volumeSize = volume.GetVolumeSize();

				//180 degrees orientation is due South. 90 degrees orientation is due west.
				var south = 180.0;
				var num_sections = 12;
				var spacing = 1 / (double)num_sections;

				//Create a section every nth degree of orientation. Each section
				//bisects the middle of the voxel
				for (int s = 0; s < num_sections; s++)
				{
					var orientation = south * (s * spacing);
					volume.CreateSection(new SectionDefinition()
					{
						Name = $"Circle {s + 1}",
						VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
						Normal = voxelLayer.GetNormal(orientation, 0.0),
						IsVisible = true
					});
				}
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
		#region Create Sections that Bisect the Voxel
		/// <summary>
		/// Creates three sections that bisect the voxel volume along its X, Y, and Z planes.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> instance containing the voxel volume to be bisected.</param>
		public static void CreateSectionsThatBisectTheVoxel(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var volumeSize = volume.GetVolumeSize();

				//Make three Normals - each is a Unit Vector (x, y, z)
				var north_south = new Coordinate3D(1, 0, 0);
				var east_west = new Coordinate3D(0, 1, 0);
				var horizontal = new Coordinate3D(0, 0, 1);

				int n = 0;
				//The two verticals bisect the x,y plane. The horizontal normal bisects
				//the Z plane.
				foreach (var normal in new List<Coordinate3D> { north_south, east_west, horizontal })
				{
					volume.CreateSection(new SectionDefinition()
					{
						Name = $"Cross {++n}",
						VoxelPosition = new Coordinate3D(volumeSize.X / 2, volumeSize.Y / 2, volumeSize.Z / 2),
						Normal = normal,
						IsVisible = true
					});
				}
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetVolumeSize()
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CreateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.#ctor
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Name
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.VoxelPosition
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
		#region Create Sections Diagonally across the Voxel
		/// <summary>
		/// Creates a series of sections diagonally across the voxel volume within the specified <see cref="VoxelLayer"/>.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void CreateSectionsDiagonally(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var volumeSize = volume.GetVolumeSize();

				//make a diagonal across the voxel
				var voxel_pos = new Coordinate3D(0, 0, volumeSize.Z);
				var voxel_pos_ur = new Coordinate3D(volumeSize.X, volumeSize.Y, volumeSize.Z);

				var lineBuilder = new LineBuilderEx(voxel_pos, voxel_pos_ur, null);
				var diagonal = PolylineBuilderEx.CreatePolyline(lineBuilder.ToSegment());

				var num_sections = 12;
				var spacing = 1 / (double)num_sections;

				//change as needed
				var orientation = 20.0; //(approx NNW)
				var tilt = -15.0;

				var normal = voxelLayer.GetNormal(orientation, tilt);

				for (int s = 0; s < num_sections; s++)
				{
					Coordinate2D end_pt = new Coordinate2D(0, 0);
					if (s > 0)
					{
						//position each section evenly spaced along the diagonal
						var segments = new List<Segment>() as ICollection<Segment>;
						var part = GeometryEngine.Instance.GetSubCurve3D(
								diagonal, 0.0, s * spacing, AsRatioOrLength.AsRatio);
						part.GetAllSegments(ref segments);
						end_pt = segments.First().EndCoordinate;
					}

					volume.CreateSection(new SectionDefinition()
					{
						Name = $"Diagonal {s + 1}",
						VoxelPosition = new Coordinate3D(end_pt.X, end_pt.Y, volumeSize.Z),
						Normal = normal,
						IsVisible = true
					});
				}
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.Normal
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		#region Update Section Orientation and Tilt
		/// <summary>
		/// Updates the orientation and tilt of all sections in the specified voxel layer to a 45-degree angle.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer whose sections will be updated. Must not be <see langword="null"/>.</param>
		public static void UpdateSectionOrientationAndTilt(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;

				foreach (var section in volume.GetSections())
				{
					//set each normal to 45.0 orientation and tilt
					section.Normal = voxelLayer.GetNormal(45.0, 45.0);
					//apply the change
					volume.UpdateSection(section);
				}
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.GetSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.SectionDefinition.isVisible
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.UpdateSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		#region Update Section Visibility
		/// <summary>
		/// Updates the visibility of all sections in the specified voxel layer, ensuring they are visible.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> whose sections' visibility will be updated.  This parameter cannot be null.</param>
		public static void UpdateSectionVisibility(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;
				var sections = volume.GetSections().Where(s => !s.IsVisible);

				//Make them all visible
				foreach (var section in sections)
				{
					section.IsVisible = true;
					//apply the change
					volume.UpdateSection(section);
				}
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.DeleteSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		#region Delete Sections
		/// <summary>
		/// Deletes all sections from the specified voxel layer's volume.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void DeleteSections(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;

				foreach (var section in volume.GetSections())
					volume.DeleteSection(section);

				//optional...
				if (voxelLayer.Visualization != VoxelVisualization.Volume)
					voxelLayer.SetVisualization(VoxelVisualization.Volume);
			});
		}
		#endregion

		#region ProSnippet Group: Locked Sections
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
		#region Get the Current Collection of Locked Sections
		/// <summary>
		/// Retrieves the current collection of locked sections for the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer from which to retrieve the locked sections. Must not be <see langword="null"/>.</param>
		public static void GetCurrentCollectionOfLockedSections(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetLockedSectionContainerExpanded(true);
				voxelLayer.SetLockedSectionContainerVisibility(true);

				var locked_sections = voxelLayer.GetLockedSections();
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
		#region Get a Locked Section
		/// <summary>
		/// Retrieves a locked section from the specified <see cref="VoxelLayer"/>.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> instance from which locked sections are retrieved.</param>
		public static void GetALockedSection(VoxelLayer voxelLayer)
		{
			var my_locked_section_id = -1;

			var locked_section = voxelLayer.GetLockedSections().FirstOrDefault();
			var locked_section2 = voxelLayer.GetLockedSections()
											.First(lsec => lsec.ID == my_locked_section_id);
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition
		#region Get Selected Locked Section in TOC
		/// <summary>
		/// Retrieves the first selected locked section in the Table of Contents (TOC) for the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer for which the selected locked section is retrieved. This parameter cannot be null.</param>
		public static void GetSelectedLockedSectionInTOC(VoxelLayer voxelLayer)
		{
			var locked_section = MapView.Active?.GetSelectedLockedSections()?.FirstOrDefault();
			if (locked_section != null)
			{
				//Do something with the locked section
			}
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.Layer
		#region Get Voxel Layer for the Selected Locked Section in TOC
		/// <summary>
		/// Retrieves the voxel layer associated with the first selected locked section in the Table of Contents (TOC).
		/// </summary>
		/// <param name="voxelLayer">An output parameter that will be set to the voxel layer corresponding to the selected locked section. If no locked
		/// section is selected, the parameter remains unchanged.</param>
		public static void GetVoxelLayerForSelectedLockedSectionInTOC(VoxelLayer voxelLayer)
		{
			var locked_section = MapView.Active?.GetSelectedLockedSections()?.FirstOrDefault();
			if (locked_section != null)
			{
				voxelLayer = locked_section.Layer;
				//TODO - use the layer
			}
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.Layer
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.VariableName
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetVariableProfile(System.String)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.SetSelectedVariableProfile(ArcGIS.Desktop.Mapping.Voxel.VoxelVariableProfile)
		#region Set the Variable Profile Active for Selected Locked Section
		/// <summary>
		/// Activates the variable profile associated with the selected locked section in the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer containing the locked section and variable profile to activate.</param>
		public static void SetVariableProfileActiveForSelectedLockedSection(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				var locked_section = MapView.Active?.GetSelectedLockedSections()?.FirstOrDefault();
				if (locked_section != null)
				{
					var variable = locked_section.Layer.GetVariableProfile(locked_section.VariableName);
					locked_section.Layer.SetSelectedVariableProfile(variable);
				}
			});
		}
		#endregion

		// cref: ArcGIS.Desktop.Mapping.MapView.GetSelectedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.CanLockSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.Voxel.VoxelVolume.LockSection(ArcGIS.Desktop.Mapping.Voxel.SectionDefinition)
		#region Lock a Section/Create a Locked Section
		/// <summary>
		/// Locks the currently selected section in the Table of Contents (TOC) for the specified voxel layer, creating a locked section.
		/// </summary>
		/// <param name="voxelLayer"></param>
		public static void LockCreateSection(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetLockedSectionContainerExpanded(true);
				voxelLayer.SetLockedSectionContainerVisibility(true);

				//Use the SelectedVariableProfile to get the sections
				//via its associated volume
				var volume = voxelLayer.SelectedVariableProfile.Volume;

				//get the selected section
				var section = MapView.Active.GetSelectedSections().FirstOrDefault();
				if (section == null)
				{
					section = volume.GetSections().FirstOrDefault();
				}

				if (section == null)
					return;

				//Lock the section (Creates a locked section, deletes
				//the section)
				//if (voxelLayer.CanLockSection(section))
				//	voxelLayer.LockSection(section);
				if (volume.CanLockSection(section))
					volume.LockSection(section);
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.GetLockedSections()
		// cref: ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition.IsVisible
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.UpdateSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
		#region Update Locked Section Visibility
		/// <summary>
		/// Updates the visibility of all locked sections in the specified voxel layer.
		/// </summary>
		/// <param name="voxelLayer">The voxel layer whose locked sections will be updated. Cannot be <see langword="null"/>.</param>
		public static void UpdateLockedSectionVisibility(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() => {
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetLockedSectionContainerExpanded(true);
				voxelLayer.SetLockedSectionContainerVisibility(true);

				var locked_sections = voxelLayer.GetLockedSections().Where(ls => !ls.IsVisible);

				//Make them visible
				foreach (var locked_section in locked_sections)
				{
					locked_section.IsVisible = true;
					//apply change
					voxelLayer.UpdateSection(locked_section);
				}
			});
		}
		#endregion
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.CanUnlockSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
		// cref: ArcGIS.Desktop.Mapping.VoxelLayer.UnlockSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
		#region Unlock a Locked Section
		/// <summary>
		/// Unlocks a locked section in the specified voxel layer, if possible.
		/// </summary>
		/// <param name="voxelLayer">The <see cref="VoxelLayer"/> containing the locked section to unlock.  This parameter cannot be null.</param>
		public static void UnlockSection(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				if (voxelLayer.Visualization != VoxelVisualization.Surface)
					voxelLayer.SetVisualization(VoxelVisualization.Surface);
				voxelLayer.SetSectionContainerExpanded(true);
				voxelLayer.SetSectionContainerVisibility(true);
				voxelLayer.SetLockedSectionContainerExpanded(true);

				//get the selected locked section
				var locked_section = MapView.Active.GetSelectedLockedSections().FirstOrDefault();
				if (locked_section == null)
					locked_section = voxelLayer.GetLockedSections().FirstOrDefault();
				if (locked_section == null)
					return;

				//Unlock the locked section (Deletes the locked section, creates
				//a section)
				if (voxelLayer.CanUnlockSection(locked_section))
					voxelLayer.UnlockSection(locked_section);
			});
		}
    #endregion

    // cref: ArcGIS.Desktop.Mapping.VoxelLayer.DeleteSection(ArcGIS.Desktop.Mapping.Voxel.LockedSectionDefinition)
    #region Delete a Locked Section
    /// <summary>
    /// Deletes the last locked section from the specified voxel layer's collection of locked sections.
    /// </summary>
    /// <param name="voxelLayer"></param>
    public static void DeleteALockedSection(VoxelLayer voxelLayer)
		{
			QueuedTask.Run(() =>
			{
				if (voxelLayer.GetLockedSections().Count() == 0)
					return;

				//Delete the last locked section from the collection of
				//locked sections
				voxelLayer.DeleteSection(voxelLayer.GetLockedSections().Last());
			});
		}
		#endregion
		}
	}

