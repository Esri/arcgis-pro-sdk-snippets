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
using ArcGIS.Core.Arcade;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcade.ProSnippets
{
  public static class ProSnippetsArcade
  {
    #region ProSnippet Group: Basic Queries
    #endregion

    //cref: ArcGIS.Core.CIM.CIMExpressionInfo
    //cref: ArcGIS.Core.CIM.CIMExpressionInfo.Expression
    //cref: ArcGIS.Core.CIM.CIMExpressionInfo.ReturnType
    //cref: ArcGIS.Core.Arcade.ArcadeScriptEngine.CreateEvaluator(ArcGIS.Core.CIM.CIMExpressionInfo,ArcGIS.Core.Arcade.ArcadeProfile)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Basic Query
    /// <summary>
    /// Executes a basic Arcade query on a given <see cref="FeatureLayer"/> to count the number of features in the layer.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> on which the Arcade query will be executed.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to construct and evaluate an Arcade expression using the ArcGIS Pro SDK.
    /// It uses the <see cref="ArcGIS.Core.Arcade.ArcadeScriptEngine"/> to create an evaluator and evaluate the expression.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    public static void BasicQuery(FeatureLayer featureLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      _ = QueuedTask.Run(() =>
      {
        //construct an expression
        var query = @"Count($layer)";//count of features in "layer"

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups);
        //Provision  values for any profile variables referenced...
        //in our case '$layer'
        var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", featureLayer)
            };
        //evaluate the expression
        try
        {
          var result = arcade.Evaluate(variables).GetResult();
          System.Diagnostics.Debug.WriteLine($"Result: {result}");
        }
        //handle any exceptions
        catch (InvalidProfileVariableException ipe)
        {
          //something wrong with the profile variable specified
          //TODO...
        }
        catch (EvaluationException ee)
        {
          //something wrong with the query evaluation
          //TODO...
        }
      });
    }
    #endregion

    //cref: ArcGIS.Core.CIM.CIMExpressionInfo
    //cref: ArcGIS.Core.CIM.CIMExpressionInfo.Expression
    //cref: ArcGIS.Core.CIM.CIMExpressionInfo.ReturnType
    //cref: ArcGIS.Core.Arcade.ArcadeScriptEngine.CreateEvaluator(ArcGIS.Core.CIM.CIMExpressionInfo,ArcGIS.Core.Arcade.ArcadeProfile)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Basic Query using Features
    /// <summary>
    /// Executes an Arcade query on a given <see cref="FeatureLayer"/> to calculate the area of each feature in square feet.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> on which the Arcade query will be executed.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to construct and evaluate an Arcade expression using the ArcGIS Pro SDK.
    /// It evaluates the expression for each feature in the layer and outputs the calculated area in square feet.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void BasicQueryUsingFeatures(FeatureLayer featureLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      _ = QueuedTask.Run(() =>
      {
        //construct an expression
        var query = @"$feature.AreaInAcres * 43560.0";//convert acres to ft 2

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups);
        //we are evaluating the expression against all features
        using var rc = featureLayer.Search();

        while (rc.MoveNext())
        {
          //Provision  values for any profile variables referenced...
          //in our case '$feature'
          var variables = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("$feature", rc.Current)
              };
          //evaluate the expression (per feature in this case)
          try
          {
            var result = arcade.Evaluate(variables).GetResult();
            var val = ((double)result).ToString("0.0#");
            System.Diagnostics.Debug.WriteLine(
              $"{rc.Current.GetObjectID()} area: {val} ft2");
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }
        }
      });
    }
    #endregion

    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Retrieve features using FeatureSetByName
    /// <summary>
    /// Executes an Arcade query to retrieve features from a specified layer in the map and counts the number of features.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> used as a reference for the operation. This parameter is not directly used in the query but is included for consistency.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to construct and evaluate an Arcade expression using the ArcGIS Pro SDK.
    /// It uses the <see cref="ArcGIS.Core.Arcade.ArcadeScriptEngine"/> to create an evaluator and evaluate the expression.
    /// The query retrieves a feature set by the layer name and counts the number of features in the set.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void RetrieveFeaturesUsingFeatureSetByName(FeatureLayer featureLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var map = MapView.Active.Map;
      _ = QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        var layer_name = "USA Current Wildfires - Current Incidents";
        //https://developers.arcgis.com/arcade/function-reference/featureset_functions/
        query.AppendLine(
          $"var features = FeatureSetByName($map,'{layer_name}', ['*'], false);");
        query.AppendLine("return Count(features);");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups);
        //Provision  values for any profile variables referenced...
        //in our case '$map'
        var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$map", map)
            };
        //evaluate the expression
        try
        {
          var result = arcade.Evaluate(variables).GetResult();
          System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
        }
        //handle any exceptions
        catch (InvalidProfileVariableException ipe)
        {
          //something wrong with the profile variable specified
          //TODO...
        }
        catch (EvaluationException ee)
        {
          //something wrong with the query evaluation
          //TODO...
        }
      });
    }

    #endregion

    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Retrieve features using Filter
    /// <summary>
    /// Executes an Arcade query to filter features in a given <see cref="FeatureLayer"/> based on a condition and counts the number of matching features.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> on which the Arcade query will be executed.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to construct and evaluate an Arcade expression using the ArcGIS Pro SDK.
    /// It uses the <see cref="ArcGIS.Core.Arcade.ArcadeScriptEngine"/> to create an evaluator and evaluate the expression.
    /// The query filters features in the layer where the "DailyAcres" field is not null and counts the number of matching features.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void BasicQueryRetrieveFeaturesUsingFilter(FeatureLayer featureLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      _ = QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        //https://developers.arcgis.com/arcade/function-reference/featureset_functions/
        query.AppendLine(
          "var features = Filter($layer, 'DailyAcres is not NULL');");
        query.AppendLine("return Count(features);");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups);
        //Provision  values for any profile variables referenced...
        //in our case '$layer'
        var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", featureLayer)
            };
        //evaluate the expression
        try
        {
          var result = arcade.Evaluate(variables).GetResult();
          System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
        }
        //handle any exceptions
        catch (InvalidProfileVariableException ipe)
        {
          //something wrong with the profile variable specified
          //TODO...
        }
        catch (EvaluationException ee)
        {
          //something wrong with the query evaluation
          //TODO...
        }
      });
    }
    #endregion

    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Calculate basic statistics with Math functions
    /// <summary>
    /// Executes an Arcade query to calculate basic statistics (count, sum, max, min, and average) for features in a given <see cref="FeatureLayer"/>.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> on which the Arcade query will be executed.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to construct and evaluate an Arcade expression using the ArcGIS Pro SDK.
    /// It filters features in the layer where the "DailyAcres" field is not null and calculates the following statistics:
    /// <list type="bullet">
    /// <item>Count of features</item>
    /// <item>Sum of the "DailyAcres" field</item>
    /// <item>Maximum value of the "DailyAcres" field</item>
    /// <item>Minimum value of the "DailyAcres" field</item>
    /// <item>Average value of the "DailyAcres" field</item>
    /// </list>
    /// The results are concatenated into a single string and logged to the debug output.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void CalculateBasicStatisticsWithMathFunctions(FeatureLayer featureLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        //https://developers.arcgis.com/arcade/function-reference/math_functions

        query.AppendLine("var features = Filter($layer, 'DailyAcres is not NULL');");

        query.AppendLine("var count_feat = Count(features);");
        query.AppendLine("var sum_feat = Sum(features, 'DailyAcres');");
        query.AppendLine("var max_feat = Max(features, 'DailyAcres');");
        query.AppendLine("var min_feat = Min(features, 'DailyAcres');");
        query.AppendLine("var avg_feat = Average(features, 'DailyAcres');");

        query.AppendLine("var answer = [count_feat, sum_feat, max_feat, min_feat, avg_feat]");
        query.AppendLine("return Concatenate(answer,'|');");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups);
        //Provision  values for any profile variables referenced...
        //in our case '$layer'
        var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", featureLayer)
            };
        //evaluate the expression
        try
        {
          var result = arcade.Evaluate(variables).GetResult();
          System.Diagnostics.Debug.WriteLine($"Result: {result.ToString()}");
        }
        //handle any exceptions
        catch (InvalidProfileVariableException ipe)
        {
          //something wrong with the profile variable specified
          //TODO...
        }
        catch (EvaluationException ee)
        {
          //something wrong with the query evaluation
          //TODO...
        }
      });
    }
    #endregion

    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Using FeatureSet functions Filter and Intersects
    public static void UsingFeatureSetFunctionsFilterAndIntersects(FeatureLayer crimesLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var map = MapView.Active.Map;
      _ = QueuedTask.Run(() =>
      {
        //construct a query
        var query = new StringBuilder();
        //https://developers.arcgis.com/arcade/function-reference/featureset_functions/

        //Assume we have two layers - Oregon Counties (poly) and Crimes (points). Crimes
        //is from the Pro SDK community sample data.
        //Select all crime points within the relevant county boundaries and sum the count
        query.AppendLine("var results = [];");

        query.AppendLine("var counties = FeatureSetByName($map, 'Oregon_Counties', ['*'], true);");

        //'Clackamas','Multnomah','Washington'
        query.AppendLine("var sel_counties = Filter(counties, 'DHS_Districts IN (2, 15, 16)');");

        query.AppendLine("for(var county in sel_counties) {");
        query.AppendLine("   var name = county.County_Name;");
        query.AppendLine("   var cnt_crime = Count(Intersects($layer, Geometry(county)));");
        query.AppendLine("   Insert(results, 0, cnt_crime);");
        query.AppendLine("   Insert(results, 0, name);");
        query.AppendLine("}");

        query.AppendLine("return Concatenate(results,'|');");

        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = query.ToString(),
          //Return type can be string, numeric, or default
          //When set to default, addin is responsible for determining
          //the return type
          ReturnType = ExpressionReturnType.Default
        };

        //Construct an evaluator
        //select the relevant profile - it must support Pro and it must
        //contain any profile variables you are using in your expression.
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.Popups);
        //Provision  values for any profile variables referenced...
        //in our case '$layer' and '$map'
        var variables = new List<KeyValuePair<string, object>>() {
              new KeyValuePair<string, object>("$layer", crimesLayer),
              new KeyValuePair<string, object>("$map", map)
            };
        //evaluate the expression
        try
        {
          var result = arcade.Evaluate(variables).GetResult();

          var results = result.ToString().Split('|', StringSplitOptions.None);
          var entries = results.Length / 2;
          int i = 0;
          for (var e = 0; e < entries; e++)
          {
            var name = results[i++];
            var count = results[i++];
            System.Diagnostics.Debug.WriteLine($"'{name}' crime count: {count}");
          }
        }
        //handle any exceptions
        catch (InvalidProfileVariableException ipe)
        {
          //something wrong with the profile variable specified
          //TODO...
        }
        catch (EvaluationException ee)
        {
          //something wrong with the query evaluation
          //TODO...
        }
      });
    }
    #endregion

    #region ProSnippet Group: Evaluating Expressions
    #endregion

    //cref: ArcGIS.Core.CIM.CIMLabelClass
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Evaluating an Arcade Labeling Expression
    /// <summary>
    /// Evaluates an Arcade labeling expression for a given <see cref="FeatureLayer"/> and outputs the results for each feature.
    /// </summary>
    /// <param name="oregonCnts">
    /// The <see cref="FeatureLayer"/> representing the Oregon Counties layer, which contains the Arcade labeling expression to be evaluated.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to evaluate an Arcade labeling expression interactively using the ArcGIS Pro SDK.
    /// It retrieves the labeling expression from the specified layer, evaluates it for each feature, and logs the results to the debug output.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void EvaluateArcadeLabelingExpression(FeatureLayer oregonCnts)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var map = MapView.Active.Map;
      QueuedTask.Run(() =>
      {
        //Assume we a layer - Oregon County (poly) that has an arcade labeling
        //expression and we want to evaluate that interactively...
        var def = oregonCnts.GetDefinition() as CIMFeatureLayer;
        //Get the label class
        var label_class = def.LabelClasses
                           .FirstOrDefault(lc =>
                           {
                             return lc.Name == "Arcade_Example_1" &&
                                    lc.ExpressionEngine == LabelExpressionEngine.Arcade;
                           });
        if (label_class == null)
          return;

        //evaluate the label expression against the features
        var expr_info = new CIMExpressionInfo()
        {
          Expression = label_class.Expression,
          ReturnType = ExpressionReturnType.String
        };

        //https://developers.arcgis.com/arcade/profiles/labeling/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                                  expr_info, ArcadeProfile.Labeling);
        //loop through the features
        using var rc = oregonCnts.Search();
        while (rc.MoveNext())
        {
          var variables = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("$feature", rc.Current)
              };
          var result = arcade.Evaluate(variables).GetResult();
          //output
          System.Diagnostics.Debug.WriteLine(
             $"[{rc.Current.GetObjectID()}]: {result}");
        }
      });

    }
    #endregion


    //cref: ArcGIS.Core.CIM.CIMVisualVariable
    //cref: ArcGIS.Core.CIM.CIMColorVisualVariable
    //cref: ArcGIS.Core.CIM.CIMTransparencyVisualVariable
    //cref: ArcGIS.Core.CIM.CIMSizeVisualVariable
    //cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.ValueExpressionInfo
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluator.Evaluate(IEnumerable<KeyValuePair<string, object>>)
    //cref: ArcGIS.Core.Arcade.ArcadeEvaluationResult.GetResult()
    #region Evaluating Arcade Visual Variable Expressions on a Renderer
    /// <summary>
    /// Evaluates Arcade visual variable expressions for a given <see cref="FeatureLayer"/> and outputs the results for each feature.
    /// </summary>
    /// <param name="oregonCnts">
    /// The <see cref="FeatureLayer"/> representing the Oregon Counties layer, which contains the visual variable expressions to be evaluated.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to evaluate Arcade visual variable expressions interactively using the ArcGIS Pro SDK.
    /// It retrieves the visual variable expressions from the renderer of the specified layer, evaluates them for each feature, and logs the results to the debug output.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void EvaluatingArcadeVisualVariableExpressionsOnRenderer(FeatureLayer oregonCnts)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/ for more examples
      //and arcade reference

      var mv = MapView.Active;
      var map = mv.Map;

      QueuedTask.Run(() =>
      {
        //Assume we a layer - Oregon County (poly) that is using Visual Variable
        //expressions that we want to evaluate interactively...
        var def = oregonCnts.GetDefinition() as CIMFeatureLayer;

        //Most all feature renderers have a VisualVariable collection
        var renderer = def.Renderer as CIMUniqueValueRenderer;
        var vis_variables = renderer.VisualVariables?.ToList() ??
                              new List<CIMVisualVariable>();
        if (vis_variables.Count == 0)
          return;//there are none
        var vis_var_with_expr = new Dictionary<string, string>();
        //see if any are using expressions
        foreach (var vv in vis_variables)
        {
          if (vv is CIMColorVisualVariable cvv)
          {
            if (!string.IsNullOrEmpty(cvv.ValueExpressionInfo?.Expression))
              vis_var_with_expr.Add("Color", cvv.ValueExpressionInfo?.Expression);
          }
          else if (vv is CIMTransparencyVisualVariable tvv)
          {
            if (!string.IsNullOrEmpty(tvv.ValueExpressionInfo?.Expression))
              vis_var_with_expr.Add("Transparency", tvv.ValueExpressionInfo?.Expression);
          }
          else if (vv is CIMSizeVisualVariable svv)
          {
            if (!string.IsNullOrEmpty(svv.ValueExpressionInfo?.Expression))
              vis_var_with_expr.Add("Outline", svv.ValueExpressionInfo?.Expression);
          }
        }
        if (vis_var_with_expr.Count == 0)
          return;//there arent any with expressions

        //loop through the features (outer)
        //per feature evaluate each visual variable.... (inner)
        //....
        //the converse is to loop through the expressions (outer)
        //then per feature evaluate the expression (inner)
        using (var rc = oregonCnts.Search())
        {
          while (rc.MoveNext())
          {
            foreach (var kvp in vis_var_with_expr)
            {
              var expr_info = new CIMExpressionInfo()
              {
                Expression = kvp.Value,
                ReturnType = ExpressionReturnType.Default
              };
              //per feature eval each expression...
              using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                          expr_info, ArcadeProfile.Visualization))
              {

                var variables = new List<KeyValuePair<string, object>>() {
                  new KeyValuePair<string, object>("$feature", rc.Current)
                };
                //note 2D maps can also have view scale...
                //...if necessary...
                if (mv.ViewingMode == MapViewingMode.Map)
                {
                  variables.Add(new KeyValuePair<string, object>(
                    "$view.scale", mv.Camera.Scale));
                }
                var result = arcade.Evaluate(variables).GetResult().ToString();
                //output
                System.Diagnostics.Debug.WriteLine(
                   $"[{rc.Current.GetObjectID()}] '{kvp.Key}': {result}");
              }
            }
          }
        }

        ////foreach (var kvp in vis_var_with_expr)
        ////{
        ////  var expr_info = new CIMExpressionInfo()
        ////  {
        ////    Expression = kvp.Value,
        ////    ReturnType = ExpressionReturnType.Default
        ////  };

        ////  using (var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
        ////                                  expr_info, ArcadeProfile.Visualization))
        ////  {
        ////    //loop through the features
        ////    using (var rc = oregon_cnts.Search())
        ////    {
        ////      while (rc.MoveNext())
        ////      {
        ////        var variables = new List<KeyValuePair<string, object>>() {
        ////          new KeyValuePair<string, object>("$feature", rc.Current)
        ////        };

        ////        var result = arcade.Evaluate(variables).GetResult();
        ////        //output
        ////        //...
        ////      }
        ////    }
        ////  }
        ////}
      });

      #endregion
    }
    // cref: ArcGIS.Core.CIM.CIMExpressionInfo
    // cref: ArcGIS.Core.CIM.CIMExpressionInfo.Expression
    // cref: ArcGIS.Core.CIM.CIMExpressionInfo.Title
    // cref: ArcGIS.Core.CIM.CIMUniqueValueRenderer.ValueExpressionInfo
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer
    #region Modify renderer using Arcade
    /// <summary>
    /// Modifies the renderer of a given <see cref="FeatureLayer"/> by applying a custom Arcade expression.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> whose renderer will be modified.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to update the renderer of a feature layer using an Arcade expression.
    /// It assumes the layer uses a unique value renderer and modifies its value expression to display different values based on the map's scale.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the renderer of the feature layer is not a unique value renderer.
    /// </exception>
    public static void ModifyRendererUsingArcade(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      {
        // GetRenderer from Layer (assumes it is a unique value renderer)
        var uvRenderer = featureLayer.GetRenderer() as CIMUniqueValueRenderer;
        if (uvRenderer == null) return;
        //layer has STATE_NAME field
        //community sample Data\Admin\AdminSample.aprx
        string expression = "if ($view.scale > 21000000) { return $feature.STATE_NAME } else { return 'All' }";
        CIMExpressionInfo updatedExpressionInfo = new CIMExpressionInfo
        {
          Expression = expression,
          Title = "Custom" // can be any string used for UI purpose.
        };
        //set the renderer's expression
        uvRenderer.ValueExpressionInfo = updatedExpressionInfo;

        //SetRenderer on Layer
        featureLayer.SetRenderer(uvRenderer);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.GetRenderer
    // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.LabelClasses
    // cref: ArcGIS.Core.CIM.CIMLabelClass
    // cref: ArcGIS.Core.CIM.CIMLabelClass.Expression
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetRenderer
    #region Modify label expression using Arcade
    /// <summary>
    /// Modifies the label expression of a given <see cref="FeatureLayer"/> by applying a custom Arcade expression.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> whose label expression will be modified.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to update the label expression of a feature layer using an Arcade expression.
    /// It retrieves the first label class of the layer, sets a new Arcade expression to display the state name and population, and updates the layer's definition.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/">Arcade Developer Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the feature layer does not have a valid label definition.
    /// </exception>
    public static void ModifyLabelExpressionWithArcade(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      {
        //Get the layer's definition
        //community sample Data\Admin\AdminSample.aprx
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        if (lyrDefn == null) return;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //set the label class Expression to use the Arcade expression
        theLabelClass.Expression = "return $feature.STATE_NAME + TextFormatting.NewLine + $feature.POP2000;";
        //Set the label definition back to the layer.
        featureLayer.SetDefinition(lyrDefn);
      });

      #endregion
    }
    //cref: ArcGIS.Core.Data.TableDefinition.GetAttributeRules(AttributeRuleType)
    //cref: ArcGIS.Core.Data.AttributeRuleDefinition.GetScriptExpression()
    #region Evaluate AttributeRule Expression
    /// <summary>
    /// Evaluates an Arcade attribute rule expression for a given <see cref="FeatureLayer"/> and validates each feature against the rule.
    /// </summary>
    /// <param name="featureLayer">
    /// The <see cref="FeatureLayer"/> whose attribute rule expression will be evaluated.
    /// </param>
    /// <remarks>
    /// This method demonstrates how to evaluate an Arcade attribute rule expression using the ArcGIS Pro SDK.
    /// It retrieves the first validation attribute rule from the feature layer, evaluates the rule for each feature, and logs whether each feature is valid.
    /// For more examples and references, consult:
    /// <list type="bullet">
    /// <item><see href="https://github.com/Esri/arcade-expressions/">Arcade Expressions GitHub</see></item>
    /// <item><see href="https://developers.arcgis.com/arcade/profiles/attribute-rules/">Arcade Attribute Rules Documentation</see></item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidProfileVariableException">
    /// Thrown when there is an issue with the profile variable specified in the Arcade expression.
    /// </exception>
    /// <exception cref="EvaluationException">
    /// Thrown when there is an error during the evaluation of the Arcade expression.
    /// </exception>
    public static void EvaluateAttributeRuleExpression(FeatureLayer featureLayer)
    {
      //Consult https://github.com/Esri/arcade-expressions/ and
      //https://developers.arcgis.com/arcade/profiles/attribute-rules/ for
      //more examples and arcade reference

      QueuedTask.Run(() =>
      {
        //Retrieve the desired feature class/table
        var def = featureLayer.GetFeatureClass().GetDefinition();

        //get the desired attribute rule whose expression is to be
        //evaluated.
        //AttributeRuleType.All, Calculation, Constraint, Validation
        var validation_rule = def.GetAttributeRules(
                                    AttributeRuleType.Validation).FirstOrDefault();
        if (validation_rule == null)
          return;

        //Get the expression
        var expr = validation_rule.GetScriptExpression();
        //construct a CIMExpressionInfo
        var arcade_expr = new CIMExpressionInfo()
        {
          Expression = expr,
          //Return type can be string, numeric, or default
          ReturnType = ExpressionReturnType.Default
        };

        System.Diagnostics.Debug.WriteLine($"Evaluating {expr}:");
        //Construct an evaluator
        //we are using ArcadeProfile.AttributeRules profile...
        //Consult: https://developers.arcgis.com/arcade/profiles/
        using var arcade = ArcadeScriptEngine.Instance.CreateEvaluator(
                                arcade_expr, ArcadeProfile.AttributeRuleValidation);
        //we are evaluating the expression against all features
        using var rc = featureLayer.Search();

        while (rc.MoveNext())
        {
          //Provision  values for any profile variables referenced...
          //in our case we assume '$feature'
          //...use arcade.ProfileVariablesUsed() if necessary...
          var variables = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("$feature", rc.Current)
              };

          //evaluate the expression per feature
          try
          {
            var result = arcade.Evaluate(variables).GetResult();
            //'Validation' attribute rules return true or false...
            var valid = System.Boolean.Parse(result.ToString());
            System.Diagnostics.Debug.WriteLine(
              $"{rc.Current.GetObjectID()} valid: {valid}");
          }
          //handle any exceptions
          catch (InvalidProfileVariableException ipe)
          {
            //something wrong with the profile variable specified
            //TODO...
          }
          catch (EvaluationException ee)
          {
            //something wrong with the query evaluation
            //TODO...
          }
        }
      });

    }
    #endregion
  }
}
