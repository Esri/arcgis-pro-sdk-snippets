using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.DDL;
using ArcGIS.Core.Data.LinearReferencing;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.KnowledgeGraph;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldDescription = ArcGIS.Core.Data.DDL.FieldDescription;

namespace ProSnippets.GeoDatabaseSnippets.LinearReferencing
{
  public static partial class ProSnippetsGeoDatabase
  {
    /// <summary>
    /// This class provides code snippets for using the Linear Referencing APIs in the ArcGIS Pro SDK for .NET.
    /// </summary>
    /// <remarks>
    /// The following code snippets demonstrate the use of Linear Referencing APIs within the ArcGIS Pro SDK.
    /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
    /// The Pro SDK Add-in may require certain code snippets to execute on the ArcGIS Pro Main CIM(MCT) thread or the UI thread.
    /// In such instances, the code should be enclosed within the QueuedTask.Run() block.
    /// <c>Cref</c>s are used for internal purposes only. Please ignore them in the context of this example.
    /// </remarks>
    public static void ProSnippetsLinearReferencingAsync()
    {
      // cref: ArcGIS.Core.Data.LinearReferencing.RouteInfo.#ctor(ArcGIS.Core.Data.FeatureClass,System.String)
      #region Create a routes feature class using the DDL
      {
        static void CreateRoutes(SchemaBuilder schemaBuilder)
        {
          FieldDescription routeIdDescription = FieldDescription.CreateIntegerField("RouteID");
          FieldDescription routeNameDescription = FieldDescription.CreateStringField("RouteName", 100);
          ShapeDescription shapeDescription = new ShapeDescription(GeometryType.Polyline, SpatialReferences.WGS84)
          { HasM = true, HasZ = false };

          FeatureClassDescription routeFeatureClassDescription = new FeatureClassDescription("Routes",
            new List<FieldDescription>() { routeIdDescription, routeNameDescription }, shapeDescription);

          //Create an M-enabled poly-line feature class 
          schemaBuilder.Create(routeFeatureClassDescription);
          schemaBuilder.Build();
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.LinearReferencing.LineEventInfo.#ctor(ArcGIS.Core.Data.Table,System.String,System.String,System.String)
      #region Create an events table using the DDL
      {
        static void CreateEvents(SchemaBuilder schemaBuilder)
        {
          FieldDescription routeIdDescription = FieldDescription.CreateIntegerField("RID");
          FieldDescription measureFieldDescription = new FieldDescription("Measure", FieldType.Double);
          FieldDescription offsetFieldDescription = FieldDescription.CreateIntegerField("OffsetValue");

          TableDescription eventTableDescription = new TableDescription("LR_EventTable",
            new List<FieldDescription>() { routeIdDescription, offsetFieldDescription, measureFieldDescription });

          schemaBuilder.Create(eventTableDescription);
          schemaBuilder.Build();
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.LinearReferencing.RouteInfo.#ctor(ArcGIS.Core.Data.FeatureClass,System.String)
      #region Get route information from a polyline feature class with M-values
      {
        static void CreateRouteInfo(Geodatabase geodatabase, string routeFeatureClassName = "Roads", string routeIdFieldName = "RouteID")
        {
          using (FeatureClass routeFeatureClass = geodatabase.OpenDataset<FeatureClass>(routeFeatureClassName))
          using (FeatureClassDefinition routeFeatureClassDefinition = routeFeatureClass.GetDefinition())
          {
            if (routeFeatureClassDefinition.HasM())
            {
              RouteInfo routeInfo = new RouteInfo(routeFeatureClass, routeIdFieldName);

              // Process the routeInfo as needed ...
            }
          }
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.LinearReferencing.PointEventInfo.#ctor(ArcGIS.Core.Data.Table,System.String,System.String)
      #region Get event information
      {
        static void CreateEventInfo(Geodatabase geodatabase, string eventTableName = "Accidents", string routeIdFieldName = "RID", string measureFieldName = "Measure", string offsetFieldName = "Offset")
        {
          using (Table eventTable = geodatabase.OpenDataset<Table>(eventTableName))
          {
            PointEventInfo eventInfo = new PointEventInfo(eventTable, routeIdFieldName, measureFieldName, offsetFieldName);

            // Get event type: Point or Line type
            EventType eventTableType = eventInfo.EventType;
          }
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.LinearReferencing.PointEventInfo.#ctor(ArcGIS.Core.Data.Table,System.String,System.String,System.String)
      // cref: ArcGIS.Core.Data.LinearReferencing.PointEventInfo.#ctor(ArcGIS.Core.Data.Table,System.String,System.String)
      // cref: ArcGIS.Core.Data.LinearReferencing.PointEventSourceOptions.#ctor
      // cerf: ArcGIS.Core.Data.LinearReferencing.RouteEventSource.GetErrors
      // cref: ArcGIS.Core.Data.LinearReferencing.RouteEventSource.#ctor(ArcGIS.Core.Data.LinearReferencing.RouteInfo,ArcGIS.Core.Data.LinearReferencing.EventInfo,ArcGIS.Core.Data.LinearReferencing.RouteEventSourceOptions)
      #region Create a RouteEventSource via dynamic segmentation process for point events
      {
        static void CreatePointEventSource(Geodatabase geodatabase, string routeFeatureClassName = "Roads", string eventTableName = "Accidents", string routeIdFieldName = "RID", string measureFieldName = "Measure")
        {
          using (FeatureClass routesFeatureClass = geodatabase.OpenDataset<FeatureClass>(routeFeatureClassName))
          using (Table eventsTable = geodatabase.OpenDataset<Table>(eventTableName))
          {
            RouteInfo routeInfo = new RouteInfo(routesFeatureClass, routeIdFieldName);
            EventInfo eventInfo = new PointEventInfo(eventsTable, routeIdFieldName, measureFieldName);
            RouteEventSourceOptions routeEventSourceOptions = new PointEventSourceOptions(AngleType.Tangent)
            { ComplementAngle = true };

            using (RouteEventSource routeEventSource = new RouteEventSource(routeInfo, eventInfo, routeEventSourceOptions))
            using (RouteEventSourceDefinition routeEventSourceDefinition = routeEventSource.GetDefinition())
            {
              // Locating errors 
              IReadOnlyList<RouteEventSourceError> errors = routeEventSource.GetErrors();

              // Route event source fields 
              IReadOnlyList<Field> routeEventSourceFields = routeEventSourceDefinition.GetFields();

              // Add RouteEventSource to the ArcGIS Pro map
              FeatureLayerCreationParams layerParams = new FeatureLayerCreationParams(routeEventSource)
              {
                Name = "RoadAccidents"
              };

              LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
            }
          }
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.LinearReferencing.LineEventInfo.#ctor(ArcGIS.Core.Data.Table,System.String,System.String,System.String,System.String)
      // cref: ArcGIS.Core.Data.LinearReferencing.LineEventInfo.#ctor(ArcGIS.Core.Data.Table,System.String,System.String,System.String)
      // cref: ArcGIS.Core.Data.LinearReferencing.LineEventSourceOptions.#ctor
      // cref: ArcGIS.Core.Data.LinearReferencing.RouteEventSource.GetErrors
      // cref: ArcGIS.Core.Data.LinearReferencing.RouteEventSource.#ctor(ArcGIS.Core.Data.LinearReferencing.RouteInfo,ArcGIS.Core.Data.LinearReferencing.EventInfo,ArcGIS.Core.Data.LinearReferencing.RouteEventSourceOptions)
      #region Create a RouteEventSource via dynamic segmentation process for line events
      {
        static void CreateLineEventSource(Geodatabase geodatabase, string routeFeatureClassName = "Roads", string eventTableName = "Accidents", string routeIdFieldName = "RID", string toMeasureFieldName = "toMeasure", string fromMeasureFieldName = "fromMeasure", string offsetFieldName = "Offset")
        {
          using (FeatureClass routesFeatureClass = geodatabase.OpenDataset<FeatureClass>(routeFeatureClassName))
          using (Table eventsTable = geodatabase.OpenDataset<Table>(eventTableName))
          {
            RouteInfo routeInfo = new RouteInfo(routesFeatureClass, routeIdFieldName);
            EventInfo eventInfo = new LineEventInfo(eventsTable, routeIdFieldName, fromMeasureFieldName, toMeasureFieldName, offsetFieldName);
            RouteEventSourceOptions routeEventSourceOptions = new LineEventSourceOptions() { IsPositiveOffsetOnRight = true };

            using (RouteEventSource routeEventSource = new RouteEventSource(routeInfo, eventInfo, routeEventSourceOptions))
            using (RouteEventSourceDefinition routeEventSourceDefinition = routeEventSource.GetDefinition())
            {
              // Locating errors 
              IReadOnlyList<RouteEventSourceError> errors = routeEventSource.GetErrors();

              // Route event source fields 
              IReadOnlyList<Field> routeEventSourceFields = routeEventSourceDefinition.GetFields();

              // Add RouteEventSource to the ArcGIS Pro map
              FeatureLayerCreationParams layerParams = new FeatureLayerCreationParams(routeEventSource)
              {
                Name = "HighCrashAreas"
              };

              LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
            }
          }
        }
      }
      #endregion


      // cref: ArcGIS.Core.Data.LinearReferencing.RouteInfo.LocateFeatures(ArcGIS.Core.Data.FeatureClass,System.Double,ArcGIS.Core.Data.LinearReferencing.EventTableConfiguration)
      // cref: ArcGIS.Core.Data.LinearReferencing.RouteInfo.LocateFeatures(ArcGIS.Core.Data.Selection,System.Double,ArcGIS.Core.Data.LinearReferencing.EventTableConfiguration)
      // cref: ArcGIS.Core.Data.LinearReferencing.RouteEventSource.#ctor(ArcGIS.Core.Data.LinearReferencing.RouteInfo,ArcGIS.Core.Data.LinearReferencing.EventInfo,ArcGIS.Core.Data.LinearReferencing.RouteEventSourceOptions)
      #region Locate features along routes
      {
        static void LocateLineFeaturesAlongRoutes(Geodatabase geodatabase, string routeFeatureClassName = "Roads", string eventTableName = "Accidents", string routeIdFieldName = "RID", string toMeasureFieldName = "toMeasure", string fromMeasureFieldName = "fromMeasure")
        {
          // Configure events table
          EventTableConfiguration lineEventTableConfiguration = new LineEventTableConfiguration(eventTableName, routeIdFieldName, fromMeasureFieldName, toMeasureFieldName) { KeepAllFields = true, MDirectionOffset = true };

          using (FeatureClass routeFeatureClass = geodatabase.OpenDataset<FeatureClass>(routeFeatureClassName))
          using (FeatureClass highCrashAreaFeatureClass = geodatabase.OpenDataset<FeatureClass>("HighCrashRegion"))
          {
            RouteInfo routeInfo = new RouteInfo(routeFeatureClass, routeIdFieldName);

            // Creates an event table inside the geodatabase
            routeInfo.LocateFeatures(highCrashAreaFeatureClass, 0.5, lineEventTableConfiguration);

            // Open newly created event table
            using (Table eventTable = geodatabase.OpenDataset<Table>(eventTableName))
            {
              EventInfo eventInfo = new LineEventInfo(eventTable, routeIdFieldName, fromMeasureFieldName, toMeasureFieldName);

              // Create RouteEventSource using new event table
              using (RouteEventSource routeEventSource = new RouteEventSource(routeInfo, eventInfo, new LineEventSourceOptions()))
              {
                // Process the routeEventSource as needed ...
              }
            }
          }
        }
      }
      #endregion


    }
  }
}
