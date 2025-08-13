﻿/*

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Core.Data;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippetsCharts
  {
    #region ProSnippet Group: Charts
    #endregion
    // cref: ArcGIS.Core.CIM.CIMChart
    // cref: ArcGIS.Core.CIM.CIMChartGeneralProperties
    // cref: ArcGIS.Core.CIM.CIMChartSeries
    // cref: ArcGIS.Core.CIM.CIMChartScatterSeries
    #region Create a simple scatter plot    
    /// <summary>
    /// Creates a scatter plot chart for the specified feature layer using predefined fields.
    /// </summary>
    /// <remarks>This method generates a scatter plot chart based on the fields "minimum_nights" and "price" in
    /// the provided <see cref="FeatureLayer"/>. The chart includes a trend line and is added to the layer's existing list
    /// of charts, if any exist.  The chart's title is automatically set to "<c>minimum_nights vs. price</c>", and the X
    /// and Y axes correspond to the "minimum_nights" and "price" fields, respectively.</remarks>
    /// <param name="featureLayer">The feature layer to which the scatter plot chart will be added. This parameter cannot be null.</param>
    public static void CreateScatterPlotChart(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      { // For more information on the chart CIM specification:
        // https://github.com/Esri/cim-spec/blob/main/docs/v3/CIMCharts.md

        // Define fields names used in chart parameters.
        const string xField = "minimum_nights";
        const string yField = "price";

        var lyrDefScatter = featureLayer.GetDefinition();

        // Define scatter plot CIM properties
        var scatterPlot = new CIMChart
        {
          Name = "scatterPlot",
          GeneralProperties = new CIMChartGeneralProperties
          {
            Title = $"{xField} vs. {yField}",
            UseAutomaticTitle = false
          },
          Series = new CIMChartSeries[]
            {
                new CIMChartScatterSeries {
                    UniqueName = "scatterPlotSeries",
                    Name = "scatterPlotSeries",
                    // Specify the X and Y field names
                    Fields = new string[] { xField , yField },
                    // Turn on trend line
                    ShowTrendLine = true
                }
            }
        };

        // Add new chart to layer's existing list of charts (if any exist)
        var newChartsScatter = new CIMChart[] { scatterPlot };
        var allChartsScatter = (lyrDefScatter == null) ? newChartsScatter : lyrDefScatter.Charts.Concat(newChartsScatter);
        // Add CIM chart to layer defintion 
        lyrDefScatter.Charts = allChartsScatter.ToArray();
        featureLayer.SetDefinition(lyrDefScatter);
      });

    }
    #endregion

    // cref: ArcGIS.Core.CIM.CIMChart
    // cref: ArcGIS.Core.CIM.CIMChartGeneralProperties
    // cref: ArcGIS.Core.CIM.CIMChartSeries
    // cref: ArcGIS.Core.CIM.CIMChartLineSeries
    // cref: ArcGIS.Core.CIM.CIMChartLineSymbolProperties
    // cref: ArcGIS.Core.CIM.CIMChartMarkerSymbolProperties
    #region Create a line chart with custom time binning and style
    public static void CreateLineChartWithCustomTimeBinningAndStyle(FeatureLayer featureLayer)
    {
      // For more information on the chart CIM specification:
      // https://github.com/Esri/cim-spec/blob/main/docs/v3/CIMCharts.md
      QueuedTask.Run(() =>
      {
        // Define fields names used in chart parameters.
        const string dateField = "last_review";
        const string numericField = "price";


        var lyrDefLine = featureLayer.GetDefinition();

        // Define line chart CIM properties
        var lineChart = new CIMChart
        {
          Name = "lineChart",
          GeneralProperties = new CIMChartGeneralProperties
          {
            Title = $"Line chart for {dateField} summarized by {numericField}",
            UseAutomaticTitle = false
          },
          Series = new CIMChartSeries[]
            {
                new CIMChartLineSeries {
                    UniqueName = "lineChartSeries",
                    Name = $"Sum({numericField})",
                    // Specify date field and numeric field
                    Fields = new string[] { dateField, numericField },
                    // Specify aggregation type
                    FieldAggregation = new string[] { string.Empty, "SUM" },
                    // Specify custom time bin of 6 months
                    TimeAggregationType = ChartTimeAggregationType.EqualIntervalsFromStartTime,
                    TimeIntervalSize = 6.0,
                    TimeIntervalUnits = esriTimeUnits.esriTimeUnitsMonths,
                    // NOTE: When setting custom time binning, be sure to set CalculateAutomaticTimeInterval = false
                    CalculateAutomaticTimeInterval = false,
                    // Define custom line color
                    ColorType = ChartColorType.CustomColor,
                    LineSymbolProperties = new CIMChartLineSymbolProperties {
                        Style = ChartLineDashStyle.DashDot,
                        Color = new CIMRGBColor { R = 0, G = 150, B = 20 },
                    },
                    MarkerSymbolProperties = new CIMChartMarkerSymbolProperties
                    {
                        Color = new CIMRGBColor { R = 0, G = 150, B = 20 }
                    }
                },
            }
        };

        // Add new chart to layer's existing list of charts (if any exist)
        var newChartsLine = new CIMChart[] { lineChart };
        var allChartsLine = (lyrDefLine == null) ? newChartsLine : lyrDefLine.Charts.Concat(newChartsLine);
        // Add CIM chart to layer defintion 
        lyrDefLine.Charts = allChartsLine.ToArray();
        featureLayer.SetDefinition(lyrDefLine);
      });

    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMChart
    // cref: ArcGIS.Core.CIM.CIMChartGeneralProperties
    // cref: ArcGIS.Core.CIM.CIMChartSeries
    // cref: ArcGIS.Core.CIM.CIMChartBarSeries

    #region Create a bar chart
    /// <summary>
    /// Creates a bar chart for the specified feature layer using predefined fields for the X-axis and Y-axis.
    /// </summary>
    /// <remarks>This method generates a bar chart with a custom title and green fill symbols for the bars. 
    /// The chart is added to the feature layer's existing list of charts, if any exist.</remarks>
    /// <param name="featureLayer">The feature layer to which the bar chart will be added. This parameter cannot be null.</param>
    public static void CreateBarChart(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      {
        var layerDefinition = featureLayer.GetDefinition();
        string fieldXAxis = "textField";
        string fieldYAxis = "NumericField";

        var myBarChart = new CIMChart
        {
          Name = "Bar chart",
          GeneralProperties = new CIMChartGeneralProperties
          {
            Title = $"{fieldXAxis} vs. {fieldYAxis}",
            UseAutomaticTitle = false
          },
          Series =
          [
              new CIMChartBarSeries
            {
              Name = "Bar chart",
              UniqueName = "Bar chart",
              Fields = new string[] { fieldXAxis, fieldYAxis },
              //Create green fill symbols
              FillSymbolProperties = new CIMChartFillSymbolProperties
                {
                  Color = CIMColor.CreateRGBColor(38, 115, 0, 70)
                }
            }
          ]
        };
        // Add new chart to layer's existing list of charts (if any exist)
        var newBarCharts = new CIMChart[] { myBarChart };
        var allChartsBar = (layerDefinition.Charts == null) ? newBarCharts : layerDefinition.Charts.Concat(newBarCharts);
        // Add CIM chart to layer definition 
        layerDefinition.Charts = allChartsBar.ToArray();
        featureLayer.SetDefinition(layerDefinition);
      });

    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMChart
    // cref: ArcGIS.Core.CIM.CIMChartGeneralProperties
    // cref: ArcGIS.Core.CIM.CIMChartSeries
    // cref: ArcGIS.Core.CIM.CIMChartHistogramSeries
    #region Create a histogram for every field of type Double
    /// <summary>
    /// Creates histogram charts for all fields of type <see cref="FieldType.Double"/> in the specified feature layer.
    /// </summary>
    /// <remarks>This method generates a histogram chart for each field of type <see cref="FieldType.Double"/>
    /// in the provided  <paramref name="featureLayer"/>. Each histogram is configured with a default bin count of 15
    /// and is added to the  layer's chart definition. If the layer already contains charts, the new histograms are
    /// appended to the existing list.</remarks>
    /// <param name="featureLayer">The feature layer for which histograms will be created. Must not be <see langword="null"/>.</param>
    public static void CreateHistogramForDoubleFields(FeatureLayer featureLayer)
    {// For more information on the chart CIM specification:
     // https://github.com/Esri/cim-spec/blob/main/docs/v3/CIMCharts.md


      var lyrDefHistogram = featureLayer.GetDefinition();

      // Get list names for fields of type Double
      var doubleFields = featureLayer.GetFieldDescriptions().Where(f => f.Type == FieldType.Double).Select(f => f.Name);

      // Create list that will contain all histograms
      var histograms = new List<CIMChart>();

      // Create histogram for each Double field
      foreach (var field in doubleFields)
      {
        // Define histogram CIM properties
        var histogram = new CIMChart
        {
          Name = $"histogram_{field}",
          GeneralProperties = new CIMChartGeneralProperties
          {
            Title = $"Histogram for {field}",
            UseAutomaticTitle = false
          },
          Series = new CIMChartSeries[]
            {
                    new CIMChartHistogramSeries {
                        UniqueName = "histogramSeries",
                        Name = $"histogram_{field}",
                        BinCount = 15,
                        // Specify the number field
                        Fields = new string[] { field },
                    }
            }
        };


        histograms.Add(histogram);
      }
      ;

      // Add new chart to layer's existing list of charts (if any exist)
      var allChartsHistogram = (lyrDefHistogram == null) ? histograms : lyrDefHistogram.Charts.Concat(histograms);
      // Add CIM chart to layer defintion 
      lyrDefHistogram.Charts = allChartsHistogram.ToArray();
      featureLayer.SetDefinition(lyrDefHistogram);
    }
    #endregion

    // cref: ArcGIS.Core.CIM.CIMChart
    // cref: ArcGIS.Core.CIM.CIMChartGeneralProperties
    // cref: ArcGIS.Core.CIM.CIMChartSeries
    // cref: ArcGIS.Core.CIM.CIMChartBarSeries
    #region Create a multiseries bar chart
    /// <summary>
    /// Creates a multi-series bar chart for the specified feature layer.
    /// </summary>
    /// <remarks>This method generates a bar chart where each series represents a unique value from a
    /// specified field. The chart groups data by a category field and aggregates the count of features for each group.
    /// The chart is added to the feature layer's definition.</remarks>
    /// <param name="featureLayer">The feature layer to which the bar chart will be added. This layer must contain the fields used for
    /// categorization and grouping.</param>
    public static void CreateMultiSeriesBarChart(FeatureLayer featureLayer)
    {
      // For more information on the chart CIM specification:
      // https://github.com/Esri/cim-spec/blob/main/docs/v3/CIMCharts.md

      // Define fields names used in chart parameters.
      const string categoryField = "neighbourhood_group";
      const string splitByField = "room_type";


      var lyrDefBar = featureLayer.GetDefinition();

      // Get unique values for `splitByField`
      var values = new List<string>();
      using (RowCursor cursor = featureLayer.Search())
      {
        while (cursor.MoveNext())
        {
          var value = Convert.ToString(cursor.Current[splitByField]);
          values.Add(value);
        }
      }
      ;
      var uniqueValues = values.Distinct().ToList();

      // Define bar chart CIM properties
      var barChart = new CIMChart
      {
        Name = "barChart",
        GeneralProperties = new CIMChartGeneralProperties
        {
          Title = $"{categoryField} grouped by {splitByField}",
          UseAutomaticTitle = false
        },
      };


      // Create list to store the info for each chart series
      var allSeries = new List<CIMChartSeries>();

      // Create a series for each unique category
      foreach (var value in uniqueValues)
      {
        var series = new CIMChartBarSeries
        {
          UniqueName = value,
          Name = value,
          // Specify the category field
          Fields = new string[] { categoryField, string.Empty },
          // Specify the WhereClause to filter a series by unique value
          WhereClause = $"{splitByField} = '{value}'",
          GroupFields = new[] { categoryField },
          // Specify aggregation type
          FieldAggregation = new string[] { string.Empty, "COUNT" }
        };

        allSeries.Add(series);

      }

      barChart.Series = allSeries.ToArray();

      // Add new chart to layer's existing list of charts (if any exist)
      var newChartsBar = new CIMChart[] { barChart };
      var allBarCharts = (lyrDefBar == null) ? newChartsBar : lyrDefBar.Charts.Concat(newChartsBar);
      // Add CIM chart to layer defintion 
      lyrDefBar.Charts = allBarCharts.ToArray();
      featureLayer.SetDefinition(lyrDefBar);
    }
    #endregion

    // cref: ArcGIS.Core.CIM.CIMChart
    // cref: ArcGIS.Core.CIM.CIMChartGeneralProperties
    // cref: ArcGIS.Core.CIM.CIMChartSeries
    // cref: ArcGIS.Core.CIM.CIMChartPieSeries
    #region Create a pie chart with custom legend formatting
    /// <summary>
    /// Creates a pie chart with custom legend formatting and adds it to the specified feature layer.
    /// </summary>
    /// <remarks>The pie chart is configured to use the "neighbourhood_group" field as the category field. 
    /// The legend text properties are customized to include a specific font color and size. This method modifies the
    /// feature layer's chart definitions by appending the new pie chart.</remarks>
    /// <param name="featureLayer">The feature layer to which the pie chart will be added. This layer must support chart definitions.</param>
    public static void CreatePieChartWithCustomLegendFormatting(FeatureLayer featureLayer)
    { // For more information on the chart CIM specification:
      // https://github.com/Esri/cim-spec/blob/main/docs/v3/CIMCharts.md

      // Define fields names used in chart parameters.
      const string fieldCategory = "neighbourhood_group";


      var lyrDef = featureLayer.GetDefinition();

      // Define pie chart CIM properties
      var pieChart = new CIMChart
      {
        Name = "pieChart",
        GeneralProperties = new CIMChartGeneralProperties
        {
          Title = "Pie chart with custom legend formatting",
          UseAutomaticTitle = true
        },
        Legend = new CIMChartLegend
        {
          LegendText = new CIMChartTextProperties
          {
            FontFillColor = new CIMRGBColor { R = 0, G = 250, B = 20 }, // Specify new font color
            FontSize = 6.0, // Specify new font size
          }
        },
        Series = new CIMChartSeries[] {
                  new CIMChartPieSeries
                  {
                     UniqueName = "pieSeries",
                     Name = "pieSeries",
                     Fields = new string[] { fieldCategory, string.Empty }
                  }
               }
      };

      // Add new chart to layer's existing list of charts (if any exist)
      var newCharts = new CIMChart[] { pieChart };
      var allCharts = (lyrDef?.Charts == null) ? newCharts : lyrDef.Charts.Concat(newCharts);
      // Add CIM chart to layer defintion 
      lyrDef.Charts = allCharts.ToArray();
      featureLayer.SetDefinition(lyrDef);
    }
    #endregion
  }
}




