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
// Ignore Spelling: Renderers

using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace ProSnippets.MapAuthoringSnippets.Renderers
{
  /// <summary>
  /// This methods has a collection of code snippets related to working with labeling in ArcGIS Pro.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with labeling in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
  public static partial class ProSnippetsMapAuthoring
  {
    public static void ProSnippetsRenderers()
    {
      #region ignore - Variable initialization
      //Get a ColorRamp
      StyleProjectItem style =
              Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS Colors");
      var colorRampList = style.SearchColorRamps("Heat Map 4 - Semitransparent");

      CIMColorRamp colorRamp = colorRampList[0].ColorRamp;
      #endregion

      #region ProSnippet Group: Chart Renderers
      #endregion
      //cref: ArcGIS.Desktop.Mapping.PieChartRendererDefinition
      //cref: ArcGIS.Desktop.Mapping.PieChartRendererDefinition.#ctor
      #region Pie chart renderer for a feature layer
      /// <summary>
      /// Renders a feature layer using Pie chart symbols to represent data
      /// ![Pie chart renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/pie-chart.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Note: Run within a QueuedTask
        //Fields to use for the pie chart slices
        var chartFields = new List<string>
        {
                "WHITE10",
                 "BLACK10",
                 "AMERIND10",
                 "ASIAN10",
                 "HISPPOP10",
                 "HAMERIND10",
                 "HWHITE10",
                 "HASIAN10",
                 "HPACIFIC10",
                 "HBLACK10",
                  "HOTHRACE10"
        };

        PieChartRendererDefinition pieChartRendererDefn = new PieChartRendererDefinition()
        {
          ChartFields = chartFields,
          ColorRamp = colorRamp,
          SizeOption = PieChartSizeOptions.Field,
          FieldName = "BLACK10",
          FixedSize = 36.89,
          DisplayIn3D = true,
          ShowOutline = true,
          Orientation = PieChartOrientation.CounterClockwise,
        };
        //Creates a "Renderer"
        var pieChartRenderer = featureLayer.CreateRenderer(pieChartRendererDefn);

        //Sets the renderer to the feature layer
        featureLayer.SetRenderer(pieChartRenderer);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.BarChartRendererDefinition
      #region Bar Chart Value renderer for a feature layer
      /// <summary>
      /// Renders a feature layer using Bar chart symbols to represent data
      /// ![bar chart renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/bar-chart.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        var chartFields = new List<string>
          {
              "WHITE10",
                "BLACK10",
                "AMERIND10",
                "ASIAN10",
                "HISPPOP10",
                "HAMERIND10",
                "HWHITE10",
                "HASIAN10",
                "HPACIFIC10",
                "HBLACK10",
                "HOTHRACE10"
          };
        //Note: Run within QueuedTask
        BarChartRendererDefinition barChartRendererDefn = new BarChartRendererDefinition()
        {
          ChartFields = chartFields,
          BarWidth = 12,
          BarSpacing = 1,
          MaximumBarLength = 65,
          Orientation = ChartOrientation.Vertical,
          DisplayIn3D = true,
          ShowAxes = true,
          ColorRamp = colorRamp

        };
        //Creates a "Renderer"
        var barChartChartRenderer = featureLayer.CreateRenderer(barChartRendererDefn);

        //Sets the renderer to the feature layer
        featureLayer.SetRenderer(barChartChartRenderer);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.StackedChartRendererDefinition
      #region Stacked bar chart renderer for a feature layer
      /// <summary>
      /// Renders a feature layer using stacked bar chart symbols to represent data
      /// ![stacked bar chart renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/stacked-bar-chart.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        var chartFields = new List<string>
            {
                "WHITE10",
                 "BLACK10",
                 "AMERIND10",
                 "ASIAN10",
                 "HISPPOP10",
                 "HAMERIND10",
                 "HWHITE10",
                 "HASIAN10",
                 "HPACIFIC10",
                 "HBLACK10",
                  "HOTHRACE10"
            };

        StackedChartRendererDefinition barChartpieChartRendererDefn = new StackedChartRendererDefinition()
        {
          ChartFields = chartFields,
          SizeOption = StackChartSizeOptions.SumSelectedFields,
          Orientation = ChartOrientation.Horizontal,
          ShowOutline = true,
          DisplayIn3D = true,
          StackWidth = 8,
          StackLength = 25.87,
          ColorRamp = colorRamp

        };
        //Note: Run within QueuedTask
        //Creates a "Renderer"
        var stackedBarChartChartRenderer = featureLayer.CreateRenderer(barChartpieChartRendererDefn);

        //Sets the renderer to the feature layer
        featureLayer.SetRenderer(stackedBarChartChartRenderer);
      }
      #endregion
      #region ProSnippet Group: ClassBreak Renderers
      #endregion
      //cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition
      #region Class Breaks renderer with graduated colors.
      /// <summary>
      /// Renders a feature layer using graduated colors to draw quantities.
      /// ![cb-colors.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-colors.png "Graduated colors with natural breaks renderer.")
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }

        GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
        {
          ClassificationField = "NumericFieldInFeatureLayer",
          ClassificationMethod = ClassificationMethod.NaturalBreaks,
          BreakCount = 5,
          ColorRamp = colorRamp,
        };
        //Note: Run within QueuedTask
        CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(gcDef);
        featureLayer?.SetRenderer(renderer);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.GraduatedColorsRendererDefinition
      //cref: ArcGIS.Core.CIM.CIMVisualVariable
      #region Class Breaks renderer with graduated colors and outline
      /// <summary>
      /// Renders a feature layer using graduated colors to draw quantities. The outline width is varied based on attributes.
      /// ![graduatedColorOutline.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/graduatedColorOutline.png "Graduated colors with natural breaks renderer.") 
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Gets the first numeric field of the feature layer
        var firstNumericFieldOfFeatureLayer = "NumericFieldInFeatureLayer";
        //Gets the min and max value of the field
        var minMax = GetFieldMinMax(featureLayer, firstNumericFieldOfFeatureLayer);
        GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
        {
          ClassificationField = "NumericFieldInFeatureLayer",
          ClassificationMethod = ClassificationMethod.NaturalBreaks,
          BreakCount = 5,
          ColorRamp = colorRamp
        };
        CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(gcDef);
        //Create array of CIMVisualVariables to hold the outline information.
        var visualVariables = new CIMVisualVariable[] {
                    new CIMSizeVisualVariable
                    {
                        ValueExpressionInfo = new CIMExpressionInfo
                        {
                           Title = "Custom",
                           Expression = "$feature.AREA",
                           ReturnType = ExpressionReturnType.Default
                        },
                        AuthoringInfo = new CIMVisualVariableAuthoringInfo
                        {
                            MinSliderValue = Convert.ToDouble(minMax.Item1),
                            MaxSliderValue = Convert.ToDouble(minMax.Item2),
                            ShowLegend = false,
                            Heading = firstNumericFieldOfFeatureLayer
                        },
                        VariableType = SizeVisualVariableType.Graduated,
                        Target = "outline",
                        MinSize = 1,
                        MaxSize = 13,
                        MinValue = Convert.ToDouble(minMax.Item1),
                        MaxValue = Convert.ToDouble(minMax.Item2)
                    },

                };
        renderer.VisualVariables = visualVariables;
        //Note: Run within QueuedTask
        featureLayer?.SetRenderer(renderer);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.GraduatedSymbolsRendererDefinition
      #region Class Breaks renderer with graduated symbols.
      /// <summary>
      /// Renders a feature layer using graduated symbols and natural breaks to draw quantities.
      /// ![cb-symbols.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-symbols.png "Graduated symbols with natural breaks renderer.")
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        GraduatedSymbolsRendererDefinition gsDef = new GraduatedSymbolsRendererDefinition()
        {
          ClassificationField = "NumericFieldInFeatureLayer", //getting the first numeric field
          ClassificationMethod = ClassificationMethod.NaturalBreaks,
          SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(CIMColor.CreateRGBColor(76, 230, 0)).MakeSymbolReference(),
          MinimumSymbolSize = 4,
          MaximumSymbolSize = 50,
          BreakCount = 5,
          ColorRamp = colorRamp, //getting a color ramp
        };
        //Note: Run within QueuedTask
        CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(gsDef);
        featureLayer?.SetRenderer(renderer);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.UnclassedColorsRendererDefinition
      #region UnClassed graduated color renderer.
      /// <summary>
      /// Renders a feature layer using an unclassed color gradient.
      /// ![cb-unclassed.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-unclassed.png "Class breaks unclassed renderer.")
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "U.S. National Transportation Atlas Interstate Highways");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the U.S. National Transportation Atlas Interstate Highways feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Gets the first numeric field of the feature layer
        var firstNumericFieldOfFeatureLayer = "NumericFieldInFeatureLayer";
        //Gets the min and max value of the field
        //Refer to the GetFieldMinMax function below
        var labels = GetFieldMinMax(featureLayer, firstNumericFieldOfFeatureLayer);
        UnclassedColorsRendererDefinition ucDef = new UnclassedColorsRendererDefinition()
        {
          Field = firstNumericFieldOfFeatureLayer,
          ColorRamp = colorRamp,
          LowerColorStop = Convert.ToDouble(labels.Item1),
          UpperColorStop = Convert.ToDouble(labels.Item2),
          UpperLabel = labels.Item2,
          LowerLabel = labels.Item1,
        };
        //Note: Run within QueuedTask
        CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(ucDef);
        featureLayer?.SetRenderer(renderer);
      }
      #endregion
      #region Class Breaks renderer with Manual Intervals
      /// <summary>
      /// Renders a feature layer using graduated colors and manual intervals to draw quantities.
      /// ![cb-colors-manual-breaks.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-colors-manual-breaks.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Change these class breaks to be appropriate for your data. These class breaks defined below apply to the US States feature class
        List<CIMClassBreak> listClassBreaks = new List<CIMClassBreak>
            {
                new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB).MakeSymbolReference(),
                    UpperBound = 24228
                },
                new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreenRGB).MakeSymbolReference(),
                    UpperBound = 67290
                },
                new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlueRGB).MakeSymbolReference(),
                    UpperBound = 121757
                },
                 new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreyRGB).MakeSymbolReference(),
                    UpperBound = 264435
                },
                  new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.WhiteRGB).MakeSymbolReference(),
                    UpperBound = 576594
                }
            };
        CIMClassBreaksRenderer cimClassBreakRenderer = new CIMClassBreaksRenderer
        {
          ClassBreakType = ClassBreakType.GraduatedColor,
          ClassificationMethod = ClassificationMethod.Manual,
          Field = "NumericFieldInLayer",
          //Important to add the Minimum break for your data to be classified correctly.
          //This is vital especially if you have data with negative values.
          //MinimumBreak = 
          Breaks = listClassBreaks.ToArray()
        };
        //Note: Run within QueuedTask
        featureLayer?.SetRenderer(cimClassBreakRenderer);
      }
      #endregion
      #region ProSnippet Group: DotDensity Renderer
      #endregion
      //cref: ArcGIS.Desktop.Mapping.DotDensityRendererDefinition
      #region Dot Density renderer.
      /// <summary>
      /// Renders a polygon feature layer with Dot Density symbols to represent quantities.
      /// ![Dot Density renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/dotDensity-renderer.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Define size of the dot to use for the renderer
        int dotSize = 3;
        //Check if the TOTPOP10 field exists
        int idxField = -1;
        using (var table = featureLayer.GetTable())
        {
          var def = table.GetDefinition();
          idxField = def.FindField("TOTPOP10");
        }
        // "TOTPOP10" field was not found
        if (idxField == -1)
          return;

        //array of fields to be represented in the renderer
        var valueFields = new List<string> { "TOTPOP10" };
        //Note: Run within QueuedTask
        //Create the DotDensityRendererDefinition object
        var dotDensityDef = new DotDensityRendererDefinition(valueFields, colorRamp,
                                                                          dotSize, 30000, "Dot", "people");
        //Create the renderer using the DotDensityRendererDefinition
        CIMDotDensityRenderer dotDensityRndr = (CIMDotDensityRenderer)featureLayer.CreateRenderer(dotDensityDef);

        //if you want to customize the dot symbol for the renderer, create a "DotDensitySymbol" which is an
        //Amalgamation of 3 symbol layers: CIMVectorMarker, CIMSolidFill and CIMSolidStroke
        //Define CIMVectorMarker layer          
        var cimMarker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.RedRGB, dotSize);
        var dotDensityMarker = cimMarker as CIMVectorMarker;
        //Define the placement
        CIMMarkerPlacementInsidePolygon markerPlacement = new CIMMarkerPlacementInsidePolygon { Randomness = 100, GridType = PlacementGridType.RandomFixedQuantity, Clipping = PlacementClip.RemoveIfCenterOutsideBoundary };
        dotDensityMarker.MarkerPlacement = markerPlacement;

        //Define CIMSolidFill layer
        CIMSolidFill solidFill = new CIMSolidFill { Color = new CIMRGBColor { R = 249, G = 232, B = 189, Alpha = 50 } };

        //Define CIMSolidStroke
        CIMSolidStroke solidStroke = new CIMSolidStroke { Color = ColorFactory.Instance.GreyRGB, Width = .5 };

        //Create the amalgamated CIMPolygonSymbol that includes the 3 layers
        var dotDensitySymbol = new CIMPolygonSymbol
        {
          SymbolLayers = new CIMSymbolLayer[] { dotDensityMarker, solidFill, solidStroke }
        };

        //Apply the dotDensitySymbol to the CIMDotDenstityRenderer's DotDensitySymbol property.
        dotDensityRndr.DotDensitySymbol = dotDensitySymbol.MakeSymbolReference();

        //Apply the renderer to the polygon Feature Layer.
        featureLayer.SetRenderer(dotDensityRndr);
      }
      #endregion
      #region ProSnippet Group: Heat Map Renderers
      #endregion
      //cref: ArcGIS.Desktop.Mapping.HeatMapRendererDefinition
      #region Heat map renderer
      /// <summary>
      /// Renders a point feature layer using a continuous color gradient to represent density of points.
      /// ![Heat map renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/heat-map.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the U.S. Cities feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "U.S. Cities");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the U.S. Cities feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //defining a heatmap renderer that uses values from Population field as the weights
        HeatMapRendererDefinition heatMapDef = new HeatMapRendererDefinition()
        {
          Radius = 20,
          WeightField = "NumericField",
          ColorRamp = colorRamp,
          RendereringQuality = 8,
          UpperLabel = "High Density",
          LowerLabel = "Low Density"
        };
        //Note: Run within QueuedTask
        CIMHeatMapRenderer heatMapRndr = (CIMHeatMapRenderer)featureLayer.CreateRenderer(heatMapDef);
        featureLayer.SetRenderer(heatMapRndr);
      }
      #endregion
      #region ProSnippet Group: Proportional Renderers
      #endregion
      //cref: ArcGIS.Desktop.Mapping.ProportionalRendererDefinition
      #region Proportional symbols renderer.
      /// <summary>
      /// Renders a feature layer using proportional symbols to draw quantities.
      /// ![Proportional Symbols renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/proportional-renderer.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Gets the first numeric field of the feature layer
        var firstNumericFieldOfFeatureLayer = "NumericFieldInLayer";
        //Gets the min and max value of the field
        var sizes = GetFieldMinMax(featureLayer, firstNumericFieldOfFeatureLayer);
        ProportionalRendererDefinition prDef = new ProportionalRendererDefinition()
        {
          Field = firstNumericFieldOfFeatureLayer,
          MinimumSymbolSize = 4,
          MaximumSymbolSize = 50,
          LowerSizeStop = Convert.ToDouble(sizes.Item1),
          UpperSizeStop = Convert.ToDouble(sizes.Item2)
        };
        //Note: Run within QueuedTask
        CIMProportionalRenderer propRndr = (CIMProportionalRenderer)featureLayer.CreateRenderer(prDef);
        featureLayer.SetRenderer(propRndr);
        }
      #endregion

      #region ProSnippet Group: Simple Renderers
      #endregion

      //cref: ArcGIS.Desktop.Mapping.CIMSimpleRenderer
      #region Simple Renderer for a Polygon feature layer.
      /// <summary>
      /// Renders a Polygon feature layer using a single symbol.
      /// ![Simple Renderer for Polygon features](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/simple-polygon.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Creating a polygon with a red fill and blue outline.
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
              ColorFactory.Instance.BlueRGB, 2.0, SimpleLineStyle.Solid);
        CIMPolygonSymbol fillWithOutline = SymbolFactory.Instance.ConstructPolygonSymbol(
              ColorFactory.Instance.CreateRGBColor(255, 190, 190), SimpleFillStyle.Solid, outline);
        //Get the layer's current renderer
        //Note: Run within QueuedTask
        CIMSimpleRenderer renderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

        //Update the symbol of the current simple renderer
        renderer.Symbol = fillWithOutline.MakeSymbolReference();

        //Update the feature layer renderer
        featureLayer.SetRenderer(renderer);
        }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.CIMSimpleRenderer
      #region Simple Renderer for a Point feature layer.
      /// <summary>
      /// Renders a Point feature layer using a single symbol.
      /// ![Simple Renderer for Point features](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/simple-point.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.ShapeType == esriGeometryType.esriGeometryPoint);
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with a point feature layer", "Data missing");
        }
        //Create a circle marker
        var pointSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.RedRGB, 8, SimpleMarkerStyle.Circle);

        //Get the layer's current renderer
        //Note: Run within QueuedTask
        CIMSimpleRenderer renderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

        //Update the symbol of the current simple renderer
        renderer.Symbol = pointSymbol.MakeSymbolReference();

        //Update the feature layer renderer
        featureLayer.SetRenderer(renderer);
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.CIMSimpleRenderer
      #region  Simple Renderer for a Line feature layer.
      /// <summary>
      /// Renders a Line feature layer using a single symbol.
      /// ![Simple Renderer for Line features](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/simple-line.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "U.S. National Transportation Atlas Interstate Highways");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the U.S. National Transportation Atlas Interstate Highways feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Create a circle marker
        var lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB, 2, SimpleLineStyle.DashDotDot);

        //Get the layer's current renderer
        //Note: Run within QueuedTask
        CIMSimpleRenderer renderer = featureLayer.GetRenderer() as CIMSimpleRenderer;

        //Update the symbol of the current simple renderer
        renderer.Symbol = lineSymbol.MakeSymbolReference();

        //Update the feature layer renderer
        featureLayer.SetRenderer(renderer);
        }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.CIMSimpleRenderer
      #region Simple Renderer for a Line feature layer using a style from a StyleProjectItem.
      /// <summary>
      /// Renders a Line feature layer using a style from a StyleProjectItem.
      /// ![Simple Renderer Style item](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/simple-line-style-item.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "U.S. National Transportation Atlas Interstate Highways");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the U.S. National Transportation Atlas Interstate Highways feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
        //Get all styles in the project
        var styleProjectItem2D = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 2D");

        //Get a specific style in the project by name
        var arrowLineSymbol = styleProjectItem2D.SearchSymbols(StyleItemType.LineSymbol, "Arrow Line 2 (Mid)")[0];
        if (arrowLineSymbol == null) return;
        //Note: Run within QueuedTask
        //Get the layer's current renderer
        var renderer = featureLayer?.GetRenderer() as CIMSimpleRenderer;

        //Update the symbol of the current simple renderer
        renderer.Symbol = arrowLineSymbol.Symbol.MakeSymbolReference();

        //Update the feature layer renderer
        featureLayer.SetRenderer(renderer);
        }
      #endregion


      #region ProSnippet Group: Unique Value Renderers
      #endregion

      #region Unique Value Renderer for a feature layer
      /// <summary>
      /// Renders a feature layer using unique values from one or multiple fields
      /// ![Unique Value renderer](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/unique-value.png)
      /// </summary>
      {
        //Check feature layer name
        //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
        var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
        if (featureLayer == null)
        {
          MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
        }
          //construct unique value renderer definition                
          UniqueValueRendererDefinition uvr = new
                   UniqueValueRendererDefinition()
          {
            //Refer to the function below to get a display field of the feature layer
            ValueFields = new List<string> { GetDisplayField(featureLayer) }, //multiple fields in the array if needed.
            ColorRamp = colorRamp, //Specify color ramp
          };

          //Creates a "Renderer"
          var cimRenderer = featureLayer.CreateRenderer(uvr);

          //Sets the renderer to the feature layer
          featureLayer.SetRenderer(cimRenderer);

        //Function to get the display field of a feature layer
        static string GetDisplayField(FeatureLayer featureLayer)
        {
          // get the CIM definition from the layer
          var cimFeatureDefinition = featureLayer.GetDefinition() as ArcGIS.Core.CIM.CIMBasicFeatureLayer;
          // get the view of the source table underlying the layer
          var cimDisplayTable = cimFeatureDefinition.FeatureTable;
          // this field is used as the 'label' to represent the row
          return cimDisplayTable.DisplayField;
        }
      }
      #endregion
      #region get the min and max value of a field in a feature layer
      //Function to get the min and max value of a field in a feature layer
      static Tuple<string, string> GetFieldMinMax(FeatureLayer featureLayer, string fieldName)
      {
        //Get the file gdb from the feature layer
        var tableDef = featureLayer.GetTable().GetDefinition();
        var fields = tableDef.GetFields();
        //var fieldName = fields[2].Name;
        //var field = tableDef.GetFields().First(f => f.Name == "FID_1");

        QueryFilter queryFilter = new QueryFilter()
        {
          WhereClause = "1 = 1",
        };
        using (var rowCursor = featureLayer.Search(queryFilter))
        {
          long iMin = -1;
          long iMax = -1;
          while (rowCursor.MoveNext())
          {
            using (Row currentRow = rowCursor.Current)
            {
              var iVal = Convert.ToInt64(currentRow[fieldName]);
              if ((iMin > iVal) || (iMin == -1))
                iMin = iVal;
              if ((iMax < iVal) || (iMax == -1))
                iMax = iVal;
            }
          }
          return new Tuple<string, string>(iMin.ToString(), iMax.ToString());
        }
      }
      #endregion
  }
 }
} 