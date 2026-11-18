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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Realtime;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace ProSnippets.MapAuthoringSnippets.Stream
{
  /// <summary>
  /// Provides methods for authoring and managing stream layers in ArcGIS Pro.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with Stream Layers in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Thread-affine operations are implemented with awaited <see cref="QueuedTask.Run"/> calls.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsMapAuthoring
  {
    /// <summary>
    /// Demonstrates various operations and configurations for working with stream layers in ArcGIS Pro.
    /// </summary>
    /// <remarks>This method includes examples of creating stream layers, setting renderers, managing
    /// streaming connections, and subscribing to real-time data. It showcases how to configure stream layers with
    /// different renderers, handle track-aware layers, and manage data expiration settings. The method also
    /// demonstrates how to subscribe to streaming data and handle real-time feature updates.</remarks>
    public static async Task ProSnippetsStreamAsync()
    {
      #region ignore - Variable initialization
      Map map = MapView.Active.Map;
      StreamLayer streamLayer = map.GetLayersAsFlattenedList()
                                   .OfType<StreamLayer>().FirstOrDefault();

      QueryFilter qfilter = new QueryFilter()
      {
        WhereClause = "1=1", //all features
        //Optionally set an extent to limit the features returned
        //Geometry = MapView.Active.Extent,
        //SpatialRelationship = SpatialRelationship.Intersects
      };

      FeatureLayer featureLayer = map.GetLayersAsFlattenedList()
                                   .OfType<FeatureLayer>().FirstOrDefault();
      #endregion

      #region ProSnippet Group: Create Stream Layer
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer
      // cref: ArcGIS.Desktop.Mapping.Layer.SetVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer(System.Uri,ArcGIS.Desktop.Mapping.ILayerContainerEdit,System.Int32,System.String)
      // cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.Instance
      #region Create Stream Layer with URI
      {
        await QueuedTask.Run(() =>
        {
          var url = "https://geoeventsample1.esri.com:6443/arcgis/rest/services/AirportTraffics/StreamServer";
          var createParam = new FeatureLayerCreationParams(new Uri(url))
          {
            IsVisible = false //turned off by default
          };
          streamLayer = LayerFactory.Instance.CreateLayer<StreamLayer>(createParam, map);

          //or use "original" create layer (will be visible by default)
          Uri uri = new Uri(url);
          streamLayer = LayerFactory.Instance.CreateLayer(uri, map) as StreamLayer;
          streamLayer.SetVisibility(false);//turn off
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer
      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.DefinitionQuery
      // cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.Instance
      #region Create a stream layer with a definition query
      {
        await QueuedTask.Run(() =>
        {
          var url = "https://geoeventsample1.esri.com:6443/arcgis/rest/services/AirportTraffics/StreamServer";
          var lyrCreateParam = new FeatureLayerCreationParams(new Uri(url))
          {
            IsVisible = true,
            DefinitionQuery = new DefinitionQuery(whereClause: "RWY = '29L'", name: "Runway")
          };

          streamLayer = LayerFactory.Instance.CreateLayer<StreamLayer>(lyrCreateParam, map);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayerCreationParams.RendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.BlueRGB
      // cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Create a stream layer with a simple renderer
      {
        await QueuedTask.Run(() =>
        {
          var url = @"https://geoeventsample1.esri.com:6443/arcgis/rest/services/LABus/StreamServer";
          var uri = new Uri(url, UriKind.Absolute);
          var createParams = new FeatureLayerCreationParams(uri)
          {
            RendererDefinition = new SimpleRendererDefinition()
            {
              SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(
                                  ColorFactory.Instance.BlueRGB,
                                  12,
                           SimpleMarkerStyle.Pushpin).MakeSymbolReference()
            }
          };
          streamLayer = LayerFactory.Instance.CreateLayer<StreamLayer>(
                              createParams, map);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.LayerCreationParams.IsVisible
      // cref: ArcGIS.Desktop.Mapping.ILayerFactory.CreateLayer``1(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Fields
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.UseDefaultSymbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultLabel
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultSymbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Groups
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Values
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Visible
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Label
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Symbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValue
      // cref: ArcGIS.Core.CIM.CIMUniqueValue.FieldValues
      // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup
      // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup.Classes
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(CIMRenderer)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.GreenRGB
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.RedRGB
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.Layer.SetVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Setting a unique value renderer for latest observations
      {
        await QueuedTask.Run(() =>
        {
          var url = @"https://geoeventsample1.esri.com:6443/arcgis/rest/services/AirportTraffics/StreamServer";
          var uri = new Uri(url, UriKind.Absolute);

          var createParams = new FeatureLayerCreationParams(uri)
          {
            IsVisible = false
          };
          streamLayer = LayerFactory.Instance.CreateLayer<StreamLayer>(
                              createParams, map);
          //Define the unique values by hand
          var uvr = new CIMUniqueValueRenderer()
          {
            Fields = new string[] { "ACTYPE" },
            UseDefaultSymbol = true,
            DefaultLabel = "Others",
            DefaultSymbol = SymbolFactory.Instance.ConstructPointSymbol(
                        CIMColor.CreateRGBColor(185, 185, 185), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
          };

          var classes = new List<CIMUniqueValueClass>
          {
            //add in classes - one for ACTYPE of 727, one for DC 9
            new CIMUniqueValueClass()
            {
              Values = new CIMUniqueValue[] {
                      new CIMUniqueValue() { FieldValues = new string[] { "B727" } } },
              Visible = true,
              Label = "Boeing 727",
              Symbol = SymbolFactory.Instance.ConstructPointSymbol(
                        ColorFactory.Instance.RedRGB, 10, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
            },
            new CIMUniqueValueClass()
            {
              Values = new CIMUniqueValue[] {
                      new CIMUniqueValue() { FieldValues = new string[] { "DC9" } } },
              Visible = true,
              Label = "DC 9",
              Symbol = SymbolFactory.Instance.ConstructPointSymbol(
                        ColorFactory.Instance.GreenRGB, 10, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
            }
          };
          //add the classes to a group
          var groups = new List<CIMUniqueValueGroup>()
        {
          new CIMUniqueValueGroup() {
             Classes = classes.ToArray()
          }
        };
          //add the groups to the renderer
          uvr.Groups = groups.ToArray();
          //Apply the renderer (for current observations)
          streamLayer.SetRenderer(uvr);
          streamLayer.SetVisibility(true);//turn on the layer
        });
      }
      #endregion

      #region ProSnippet Group: Stream Layer settings and properties
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsTrackAware
      // cref: ArcGIS.Desktop.Mapping.Map.GetLayersAsFlattenedList()
      // cref: ArcGIS.Desktop.Mapping.MapView.Active
      // cref: ArcGIS.Desktop.Mapping.MapView.Map
      #region Find all Stream Layers that are Track Aware
      {
        var trackAwareLayers = MapView.Active.Map.GetLayersAsFlattenedList()
                                   .OfType<StreamLayer>().Where(sl => sl.IsTrackAware)?.ToList();
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      // cref: ArcGIS.Core.Data.TrackType
      #region Determine the Stream Layer type
      {
        //spatial or non-spatial?
        if (streamLayer.TrackType == TrackType.AttributeOnly)
        {
          //this is a non-spatial stream layer
        }
        else
        {
          //this must be a spatial stream layer
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.IsStreamingConnectionOpen
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.StartStreaming()
      #region Check the Stream Layer connection state
      {
        if (!streamLayer.IsStreamingConnectionOpen)
          await QueuedTask.Run(() =>
          {
            streamLayer.StartStreaming();
          });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.StartStreaming()
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.StopStreaming()
      #region Start and stop streaming
      {
        await QueuedTask.Run(() =>
        {
          //Start...
          streamLayer.StartStreaming();
          //Stop...
          streamLayer.StopStreaming();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass.Truncate()
      #region Delete all current and previous observations
      {
        await QueuedTask.Run(() =>
        {
          //Must be called on the feature class
          using var rfc = streamLayer.GetFeatureClass();
          rfc.Truncate();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsTrackAware
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackIdFieldName
      #region Get the Track Id Field
      {
        if (streamLayer.IsTrackAware)
        {
          var trackField = streamLayer.TrackIdFieldName;
          //TODO use the field name
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      // cref: ArcGIS.Core.Data.TrackType
      #region Get The Track Type
      {
        var trackType = streamLayer.TrackType;
        switch (trackType)
        {
          //TODO deal with tracktype
          case TrackType.None:
          case TrackType.AttributeOnly:
          case TrackType.Spatial:
            break;
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsTrackAware
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetExpirationMethod()
      // cref: ArcGIS.Core.CIM.FeatureExpirationMethod
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.SetExpirationMethod(ArcGIS.Core.CIM.FeatureExpirationMethod)
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.SetExpirationMaxCount(System.UInt64)
      #region Set the Maximum Count of Previous Observations to be Stored in Memory
      {
        await QueuedTask.Run(() =>
        {
          //Set Expiration Method and Max Expiration Count
          if (streamLayer.GetExpirationMethod() != FeatureExpirationMethod.MaximumFeatureCount)
          {
            streamLayer.SetExpirationMethod(FeatureExpirationMethod.MaximumFeatureCount);
          }

          streamLayer.SetExpirationMaxCount(15);
          //FYI
          if (streamLayer.IsTrackAware)
          {
            //MaxCount is per track! otherwise for the entire layer
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetExpirationMethod()
      // cref: ArcGIS.Core.CIM.FeatureExpirationMethod
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.SetExpirationMethod(ArcGIS.Core.CIM.FeatureExpirationMethod)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsTrackAware
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.SetExpirationMaxAge(System.TimeSpan)
      #region Set the Maximum Age of Previous Observations to be Stored in Memory
      {
        await QueuedTask.Run(() =>
        {
          //Set Expiration Method and Max Expiration Age
          if (streamLayer.GetExpirationMethod() != FeatureExpirationMethod.MaximumFeatureAge)
          {
            streamLayer.SetExpirationMethod(FeatureExpirationMethod.MaximumFeatureAge);
          }
          //set to 12 hours (max is 24 hours)
          streamLayer.SetExpirationMaxAge(new TimeSpan(12, 0, 0));

          //FYI
          if (streamLayer.IsTrackAware)
          {
            //MaxAge is per track! otherwise for the entire layer
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      // cref: ArcGIS.Core.Data.TrackType
      // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.PreviousObservationsCount
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetExpirationMaxCount()
      // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.ShowPreviousObservations
      // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.ShowTracks
      // cref: ArcGIS.Desktop.Mapping.Layer.GetDefinition()
      // cref: ArcGIS.Desktop.Mapping.Layer.SetDefinition(ArcGIS.Core.CIM.CIMBaseLayer)
      #region Set Stream Layer properties via the CIM
      {
        //The layer must be track aware and spatial
        if (streamLayer.TrackType != TrackType.Spatial)
        {
          // not track aware and spatial
        }
        await QueuedTask.Run(() =>
        {
          //get the CIM Definition
          var def = streamLayer.GetDefinition() as CIMFeatureLayer;
          //set the number of previous observations,
          def.PreviousObservationsCount = (int)streamLayer.GetExpirationMaxCount() - 1;
          //set show previous observations and track lines to true
          def.ShowPreviousObservations = true;
          def.ShowTracks = true;
          //commit the changes
          streamLayer.SetDefinition(def);
        });
      }
      #endregion

      #region ProSnippet Group: Rendering
      #endregion

      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition
      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.ValueFields
      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.UniqueValueRendererDefinition.ValuesLimit
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(CIMRenderer)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.RedRGB
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Defining a unique value renderer definition
      {
        await QueuedTask.Run(() =>
        {
          streamLayer = null;
          //https://geoeventsample1.esri.com:6443/arcgis/rest/services/AirportTraffics/StreamServer

          var uvrDef = new UniqueValueRendererDefinition()
          {
            ValueFields = new List<string> { "ACTYPE" },
            SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(
              ColorFactory.Instance.RedRGB, 10, SimpleMarkerStyle.Hexagon)
                .MakeSymbolReference(),
            ValuesLimit = 5
          };
          //Note: CreateRenderer can only create value classes based on
          //the current events it has received
          streamLayer.SetRenderer(streamLayer.CreateRenderer(uvrDef));
        });
      }
      #endregion

      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Fields
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.UseDefaultSymbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultLabel
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.DefaultSymbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.Groups
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Values
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Visible
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Label
      // cref: ArcGIS.Core.CIM.CIMUniqueValueClass.Symbol
      // cref: ArcGIS.Core.CIM.CIMUniqueValue
      // cref: ArcGIS.Core.CIM.CIMUniqueValue.FieldValues
      // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup
      // cref: ArcGIS.Core.CIM.CIMUniqueValueGroup.Classes
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(CIMRenderer)
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Setting a unique value renderer for latest observations 2
      {
        await QueuedTask.Run(() =>
        {
          //Define the classes by hand to avoid using CreateRenderer(...)
          CIMUniqueValueClass uvcB727 = new CIMUniqueValueClass()
          {
            Values = new CIMUniqueValue[] { new CIMUniqueValue() { FieldValues = new string[] { "B727" } } },
            Visible = true,
            Label = "Boeing 727",
            Symbol = SymbolFactory.Instance.ConstructPointSymbol(CIMColor.CreateRGBColor(255, 0, 0), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
          };

          CIMUniqueValueClass uvcD9 = new CIMUniqueValueClass()
          {
            Values = new CIMUniqueValue[] { new CIMUniqueValue() { FieldValues = new string[] { "DC9" } } },
            Visible = true,
            Label = "DC 9",
            Symbol = SymbolFactory.Instance.ConstructPointSymbol(CIMColor.CreateRGBColor(0, 255, 0), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
          };
          //Assign the classes to a group
          CIMUniqueValueGroup uvGrp = new CIMUniqueValueGroup()
          {
            Classes = new CIMUniqueValueClass[] { uvcB727, uvcD9 }
          };
          //assign the group to the renderer
          var UVrndr = new CIMUniqueValueRenderer()
          {
            Fields = new string[] { "ACTYPE" },
            Groups = new CIMUniqueValueGroup[] { uvGrp },
            UseDefaultSymbol = true,
            DefaultLabel = "Others",
            DefaultSymbol = SymbolFactory.Instance.ConstructPointSymbol(
              CIMColor.CreateRGBColor(185, 185, 185), 8, SimpleMarkerStyle.Hexagon).MakeSymbolReference()
          };
          //set the renderer. Depending on the current events received, the
          //layer may or may not have events for each of the specified
          //unique value classes
          streamLayer.SetRenderer(UVrndr);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(CIMRenderer,FeatureRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.FeatureRendererTarget
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Setting a unique value renderer for previous observations
      {
        //The layer must be track aware and spatial
        if (streamLayer.TrackType != TrackType.Spatial)
        {
          // not track aware and spatial
        }
        await QueuedTask.Run(() =>
        {
          //Define unique value classes same as we do for current observations
          //or use "CreateRenderer(...)" to assign them automatically
          CIMUniqueValueClass uvcB727Prev = new CIMUniqueValueClass()
          {
            Values = new CIMUniqueValue[] { new CIMUniqueValue() {
            FieldValues = new string[] { "B727" } } },
            Visible = true,
            Label = "Boeing 727",
            Symbol = SymbolFactory.Instance.ConstructPointSymbol(
            CIMColor.CreateRGBColor(255, 0, 0), 4, SimpleMarkerStyle.Hexagon)
            .MakeSymbolReference()
          };

          CIMUniqueValueClass uvcD9Prev = new CIMUniqueValueClass()
          {
            Values = new CIMUniqueValue[] { new CIMUniqueValue() {
            FieldValues = new string[] { "DC9" } } },
            Visible = true,
            Label = "DC 9",
            Symbol = SymbolFactory.Instance.ConstructPointSymbol(
              CIMColor.CreateRGBColor(0, 255, 0), 4, SimpleMarkerStyle.Hexagon)
              .MakeSymbolReference()
          };

          CIMUniqueValueGroup uvGrpPrev = new CIMUniqueValueGroup()
          {
            Classes = new CIMUniqueValueClass[] { uvcB727Prev, uvcD9Prev }
          };

          var UVrndr = new CIMUniqueValueRenderer()
          {
            Fields = new string[] { "ACTYPE" },
            Groups = new CIMUniqueValueGroup[] { uvGrpPrev },
            UseDefaultSymbol = true,
            DefaultLabel = "Others",
            DefaultSymbol = SymbolFactory.Instance.ConstructPointSymbol(
              CIMColor.CreateRGBColor(185, 185, 185), 4, SimpleMarkerStyle.Hexagon)
              .MakeSymbolReference()
          };

          streamLayer.SetRenderer(UVrndr, FeatureRendererTarget.PreviousObservations);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureRendererTarget
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer(ArcGIS.Core.CIM.CIMRenderer, ArcGIS.Desktop.Mapping.FeatureRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.Instance
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.CreateRenderer(ArcGIS.Desktop.Mapping.RendererDefinition)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      // cref: ArcGIS.Desktop.Mapping.IColorFactory.BlueRGB
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructLineSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Desktop.Mapping.SimpleRendererDefinition.SymbolTemplate
      // cref: ArcGIS.Desktop.Mapping.SymbolExtensionMethods.MakeSymbolReference()
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.Instance
      #region Setting a simple renderer to draw track lines
      {
        await QueuedTask.Run(() =>
        {
          //The layer must be track aware and spatial
          if (streamLayer.TrackType != TrackType.Spatial)
          {
            // not track aware and spatial
          }

          //Note: only a simple renderer with solid line symbol is supported for track
          //line renderer
          var trackRenderer = new SimpleRendererDefinition()
          {
            SymbolTemplate = SymbolFactory.Instance.ConstructLineSymbol(
                ColorFactory.Instance.BlueRGB, 2, SimpleLineStyle.Solid)
                  .MakeSymbolReference()
          };
          streamLayer.SetRenderer(
               streamLayer.CreateRenderer(trackRenderer),
                 FeatureRendererTarget.TrackLines);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.AreTrackLinesVisible
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetTrackLinesVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.ArePreviousObservationsVisible
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetPreviousObservationsVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      #region Check Previous Observation and Track Line Visibility
      {
        //The layer must be track aware and spatial for these settings
        //to have an effect
        if (streamLayer.TrackType != TrackType.Spatial)
        {
          // not track aware and spatial
        }
        await QueuedTask.Run(() =>
        {
          if (!streamLayer.AreTrackLinesVisible)
          {
            streamLayer.SetTrackLinesVisibility(true);
          }

          if (!streamLayer.ArePreviousObservationsVisible)
          {
            streamLayer.SetPreviousObservationsVisibility(true);
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetPreviousObservationsCount(System.Int32)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.AreTrackLinesVisible
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetTrackLinesVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.ArePreviousObservationsVisible
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetPreviousObservationsVisibility(System.Boolean)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      #region Make Track Lines and Previous Observations Visible
      {
        //The layer must be track aware and spatial for these settings
        //to have an effect

        if (streamLayer.TrackType != TrackType.Spatial)
        {
          // not track aware and spatial
        }

        await QueuedTask.Run(() =>
        {
          //Note: Setting PreviousObservationsCount larger than the
          //"SetExpirationMaxCount()" has no effect
          streamLayer.SetPreviousObservationsCount(6);
          if (!streamLayer.AreTrackLinesVisible)
          {
            streamLayer.SetTrackLinesVisibility(true);
          }

          if (!streamLayer.ArePreviousObservationsVisible)
          {
            streamLayer.SetPreviousObservationsVisibility(true);
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer()
      #region Retrieve the current observation renderer
      {
        await QueuedTask.Run(() =>
        {
          var renderer = streamLayer.GetRenderer();
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureRendererTarget
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer(ArcGIS.Desktop.Mapping.FeatureRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      #region Retrieve the previous observation renderer
      {
        //The layer must be track aware and spatial
        if (streamLayer.TrackType != TrackType.Spatial)
        {
          // not track aware and spatial
        }
        await QueuedTask.Run(() =>
        {
          var prev_renderer = streamLayer.GetRenderer(
          FeatureRendererTarget.PreviousObservations);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.FeatureRendererTarget
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer(ArcGIS.Desktop.Mapping.FeatureRendererTarget)
      // cref: ArcGIS.Desktop.Mapping.FeatureLayer.TrackType
      #region Retrieve the track lines renderer
      {
        //The layer must be track aware and spatial
        if (streamLayer.TrackType != TrackType.Spatial)
        {

        }

        await QueuedTask.Run(() =>
        {
          var track_renderer = streamLayer.GetRenderer(
            FeatureRendererTarget.TrackLines);
        });
      }
      #endregion

      #region ProSnippet Group: Subscribe and SearchAndSubscribe
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.SearchAndSubscribe(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.Current
      // cref: ArcGIS.Core.Data.Realtime.RealtimeRow.GetRowSource()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeRowSource
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass.SearchAndSubscribe(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Search And Subscribe for Streaming Data
      {
        //query filter can be null to search and retrieve all rows
        //true means recycling cursor
        using (var rc = await QueuedTask.Run(() => streamLayer.SearchAndSubscribe(qfilter, true)))
        {
          //waiting for new features to be streamed
          //default is no cancellation
          while (await rc.WaitForRowsAsync())
          {
            while (rc.MoveNext())
            {
              using (var row = rc.Current)
              {
                //determine the origin of the row event
                switch (row.GetRowSource())
                {
                  case RealtimeRowSource.PreExisting:
                    //pre-existing row at the time of subscribe
                    continue;
                  case RealtimeRowSource.EventInsert:
                    //row was inserted after subscribe
                    continue;
                  case RealtimeRowSource.EventDelete:
                    //row was deleted after subscribe
                    continue;
                }
              }
            }
          }
        }//row cursor is disposed. row cursor is unsubscribed

        //....or....
        //Use the feature class instead of the layer
        RealtimeFeatureClass rfc = null;
        RealtimeCursor realtimeCursor = null;
        await QueuedTask.Run(() =>
        {
          rfc = streamLayer.GetFeatureClass();
          //non-recycling cursor - 2nd param "false"
          realtimeCursor = rfc.SearchAndSubscribe(qfilter, false);
        });
        using (rfc)
        using (realtimeCursor)
        {
          //waiting for new features to be streamed
          //default is no cancellation
          while (await realtimeCursor.WaitForRowsAsync())
          {
            //etc
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.SearchAndSubscribe(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.Current
      #region Search And Subscribe With Cancellation
      {
        //Recycling cursor - 2nd param "true"
        //or streamLayer.Subscribe(qfilter, true) to just subscribe
        using (var rc = await QueuedTask.Run(() => streamLayer.SearchAndSubscribe(qfilter, true)))
        {
          //auto-cancel after 20 seconds
          var cancel = new CancellationTokenSource(new TimeSpan(0, 0, 20));
          //catch TaskCanceledException
          try
          {
            while (await rc.WaitForRowsAsync(cancel.Token))
            {
              //check for row events
              while (rc.MoveNext())
              {
                using var row = rc.Current;
                //etc
              }
            }
          }
          catch (TaskCanceledException)
          {
            //Handle cancellation as needed
          }
          cancel.Dispose();
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.Current
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.Subscribe(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      #region Explicitly Cancel WaitForRowsAsync
      {
        bool SomeConditionForCancel = false;

        //somewhere in our code we create a CancellationTokenSource
        var cancel = new CancellationTokenSource();
        //...

        //call cancel on the CancellationTokenSource anywhere in
        //the add-in, assuming the CancellationTokenSource is in scope
        if (SomeConditionForCancel)
        {
          cancel.Cancel();//<-- will cancel the token
        }

        using RealtimeCursor rc = await QueuedTask.Run(() => streamLayer.Subscribe(qfilter, true));
        try
        {
          //TaskCanceledException will be thrown when the token is cancelled
          while (await rc.WaitForRowsAsync(cancel.Token))
          {
            //check for row events
            while (rc.MoveNext())
            {
              using var row = rc.Current;
              //etc
            }
          }
        }
        catch (TaskCanceledException)
        {
          //Handle cancellation as needed
        }
        cancel.Dispose();
      }
      #endregion

      #region ProSnippet Group: Realtime FeatureClass
      #endregion

      // cref: ArcGIS.Core.Data.Realtime.RealtimeServiceConnectionProperties
      // cref: ArcGIS.Core.Data.Realtime.RealtimeDatastoreType
      // cref: ArcGIS.Core.Data.Realtime.RealtimeDatastore.GetTableNames()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeDatastore.OpenTable(System.String)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass.StartStreaming()
      #region Connect to a real-time feature class from a real-time datastore
      {
        var url = "https://geoeventsample1.esri.com:6443/arcgis/rest/services/AirportTraffics/StreamServer";

        await QueuedTask.Run(() =>
        {
          var realtimeServiceConProp = new RealtimeServiceConnectionProperties(
                                         new Uri(url),
                                         RealtimeDatastoreType.StreamService
                                      );
          using var realtimeDatastore = new RealtimeDatastore(realtimeServiceConProp);
          //A Realtime data store only contains **one** Realtime feature class (or table)
          var name = realtimeDatastore.GetTableNames().First();
          using (RealtimeFeatureClass realtimeFeatureClass = realtimeDatastore.OpenTable(name) as RealtimeFeatureClass)
          {
            //feature class, by default, is not streaming (opposite of the stream layer)
            realtimeFeatureClass.StartStreaming();
            //TODO use the feature class
            //...
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClassDefinition
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClassDefinition.HasTrackIDField()
      #region Check the Realtime Feature Class is Track Aware
      {
        await QueuedTask.Run(() =>
        {
          using var rfc = streamLayer.GetFeatureClass();
          using var rfc_def = rfc.GetDefinition();
          if (rfc_def.HasTrackIDField())
          {
            //Track aware
          }
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass.GetDefinition()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClassDefinition
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClassDefinition.HasTrackIDField()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClassDefinition.GetTrackIDField()
      #region Get the Track Id Field from the Realtime Feature class
      {
        await QueuedTask.Run(() =>
        {
          using (var rfc = streamLayer.GetFeatureClass())
          using (var rfc_def = rfc.GetDefinition())
          {
            if (rfc_def.HasTrackIDField())
            {
              var fld_name = rfc_def.GetTrackIDField();

            }
          }
        });
      }
      #endregion

      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeatureClass.Subscribe(ArcGIS.Core.Data.QueryFilter,System.Boolean)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.Current
      // cref: ArcGIS.Core.Data.Realtime.RealtimeRow.GetRowSource()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeRowSource
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeature
      // cref: ArcGIS.Core.Data.Realtime.RealtimeFeature.GetShape()
      // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.Select(ArcGIS.Core.Data.QueryFilter,ArcGIS.Desktop.Mapping.SelectionCombinationMethod,ArcGIS.Desktop.Mapping.TimeRange,ArcGIS.Desktop.Mapping.RangeExtent,ArcGIS.Core.CIM.CIMFloorFilterSettings)
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      #region Subscribe to Streaming Data
      {
        //Note: with feature class we can also use a System Task to subscribe and
        //process rows
        RealtimeFeatureClass rfc = null;
        RealtimeCursor rc = null;
        await QueuedTask.Run(() =>
        {
          // or var rfc = realtimeDatastore.OpenTable(name) as RealtimeFeatureClass
          rfc = streamLayer.GetFeatureClass();
          //non-recycling cursor - 2nd param "false"
          //subscribe, pre-existing rows are not searched
          rc = rfc.Subscribe(qfilter, false);
        });
        using (rfc)
        using (rc)
        {
          SpatialQueryFilter spatialFilter = new SpatialQueryFilter();
          //waiting for new features to be streamed
          //default is no cancellation
          while (await rc.WaitForRowsAsync())
          {
            while (rc.MoveNext())
            {
              using (var row = rc.Current)
              {
                switch (row.GetRowSource())
                {
                  case RealtimeRowSource.EventInsert:
                    //getting geometry from new events as they arrive
                    Polygon poly = ((RealtimeFeature)row).GetShape() as Polygon;

                    //using the geometry to select features from another feature layer
                    spatialFilter.FilterGeometry = poly;//project poly if needed...
                    await QueuedTask.Run(() => featureLayer.Select(spatialFilter));
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
          //row cursor is disposed. row cursor is unsubscribed
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.Current
      // cref: ArcGIS.Core.Data.Realtime.RealtimeRow.GetRowSource()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeRowSource
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      #region Search Existing Data and Subscribe for Streaming Data
      {
        //Note we can use System Task with the Realtime feature class
        //for subscribe

        RealtimeFeatureClass rfc = null;
        RealtimeCursor rc = null;
        await QueuedTask.Run(() =>
        {
          rfc = streamLayer.GetFeatureClass();
          //non-recycling cursor - 2nd param "false"
          rc = rfc.SearchAndSubscribe(qfilter, false);
        });
        using (rfc)
        using (rc)
        {

          //waiting for new features to be streamed
          //default is no cancellation - use await in async method
          while (await rc.WaitForRowsAsync())
          {
            //pre-existing rows will be retrieved that were searched
            while (rc.MoveNext())
            {
              using RealtimeRow row = rc.Current;
              var row_source = row.GetRowSource();
              switch (row_source)
              {
                case RealtimeRowSource.EventDelete:
                  //TODO - handle deletes
                  break;
                case RealtimeRowSource.EventInsert:
                  //TODO handle inserts
                  break;
                case RealtimeRowSource.PreExisting:
                  //TODO handle pre-existing rows
                  break;
              }
            }
          }
          //row cursor is disposed. row cursor is unsubscribed
        }
      }
      #endregion

      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursorBase.WaitForRowsAsync(System.Threading.CancellationToken)
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.MoveNext()
      // cref: ArcGIS.Core.Data.Realtime.RealtimeCursor.Current
      // cref: ArcGIS.Desktop.Mapping.StreamLayer.GetFeatureClass()
      #region Search And Subscribe With Cancellation 2
      {
        RealtimeFeatureClass rfc = null;
        RealtimeCursor rc = null;
        await QueuedTask.Run(() =>
        {
          rfc = streamLayer.GetFeatureClass();
          //Recycling cursor - 2nd param "true"
          rc = rfc.SearchAndSubscribe(qfilter, true);
        });
        using (rfc)
        using (rc)
        {
          //auto-cancel after 20 seconds
          var cancel = new CancellationTokenSource(new TimeSpan(0, 0, 20));
          //catch TaskCanceledException
          try
          {
            // Use await in async method
            while (await rc.WaitForRowsAsync(cancel.Token))
            {
              //check for row events
              while (rc.MoveNext())
              {
                using RealtimeRow record = rc.Current;
                // Process the record
              }
            }
          }
          catch (TaskCanceledException)
          {
            //Handle cancellation as needed
          }
          cancel.Dispose();
        }
      }
      #endregion

    }
  }
}
