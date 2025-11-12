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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProSnippets.MapAuthoringSnippets.TextSymbols
{
  /// <summary>
  /// This methods has a collection of code snippets related to working with ArcGIS Pro Text Symbol creation.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with creating text symbols in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
  public static partial class ProSnippetsMapAuthoring
  {
    public static void ProSnippetsTextSymbols()
    {
      #region ignore - Variable Initialization
      //Get the first polygon feature layer in the map
      var theLayerToLabel = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(fl => fl.ShapeType == esriGeometryType.esriGeometryPolygon);
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String)
      #region Creates a simple text symbol 
      /// <summary>
      ///  Creates a simple black text symbol with a size of 8.5, Font Family "Corbel" and Font Style of "Regular".
      /// ![lineCallOut](http://Esri.github.io/arcgis-pro-sdk/images/Labeling/SimpleText.png)
      /// </summary>
      {
        //Create a simple text symbol
        //Note: Run this code inside a QueuedTask
        CIMTextSymbol simpleTextSymbol = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 8.5, "Corbel", "Regular");
        //Refer to the snippet "How to apply a Text Symbol to labels of a layer" below to see how to apply this text symbol to a label class
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String)
      #region Snippet Creates a text symbol with a halo
      /// <summary>
      /// Creates a text symbol with a red halo
      /// ![halo](http://Esri.github.io/arcgis-pro-sdk/images/Labeling/halo.png)
      /// </summary>
      {
        //create a polygon symbol for the halo
        //Note: Run this code inside a QueuedTask
        var haloPoly = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid);
        //create text symbol using the halo polygon
        CIMTextSymbol textSymbolWithHalo = SymbolFactory.Instance.ConstructTextSymbol(haloPoly, 10, "Arial", "Bold");
        //Refer to the snippet "How to apply a Text Symbol to labels of a layer" below to see how to apply this text symbol to a label class

      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String)
      //cref: ArcGIS.Core.CIM.CIMSimpleLineCallout

      #region Snippet Creates a simple line callout text symbol
      /// <summary>
      /// Creates a simple line callout text symbol.  The [CIMSimpleLineCallout](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic2760.html) created is a dash-dot-dash line symbol with an offset of 10 from the geometry being labeled.
      /// ![lineCallOut](http://Esri.github.io/arcgis-pro-sdk/images/Labeling/line-callout.png)
      /// </summary>
      {
        //create a text symbol
        //Note: Run this code inside a QueuedTask
        var textSymbolCallOut = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 10, "Verdana", "Regular");
        //Create a line call out
        var lineCalloutSymbol = new CIMSimpleLineCallout();
        //Get a line symbol
        var lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlackRGB, 1, SimpleLineStyle.DashDotDot);
        //assign the line symbol to the callout
        lineCalloutSymbol.LineSymbol = lineSymbol;
        //Offset for the text
        textSymbolCallOut.OffsetX = 10;
        textSymbolCallOut.OffsetY = 10;
        //Assign the callout to the text symbol
        textSymbolCallOut.Callout = lineCalloutSymbol;

        //Refer to the snippet "How to apply a Text Symbol to labels of a layer" below to see how to apply this text symbol to a label class
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String)
      //cref: ArcGIS.Core.CIM.CIMBalloonCallout
      //cref: ArcGIS.Core.CIM.BalloonCalloutStyle
      //cref: ArcGIS.Core.CIM.BalloonCalloutStyle.RoundedRectangle
      #region Snippet Creates a balloon callout text symbol 
      /// <summary>
      /// Creates a black banner balloon callout text symbol. The [CIMBalloonCallout](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic487.html) created is a rectangular polygon with rounded corners.
      /// ![lineCallOut](http://Esri.github.io/arcgis-pro-sdk/images/Labeling/banner-callout.png)
      /// </summary>
      {
        //create a text symbol
        //Note: Run this code inside a QueuedTask
        var textSymbolBalloonCallout = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.WhiteRGB, 11, "Corbel", "Regular");
        //A balloon callout
        var balloonCallout = new CIMBalloonCallout();
        //set the callout's style
        balloonCallout.BalloonStyle = BalloonCalloutStyle.RoundedRectangle;
        //Create a solid fill polygon symbol for the callout.
        var polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlackRGB, SimpleFillStyle.Solid);
        //Set the callout's background to be the black polygon symbol
        balloonCallout.BackgroundSymbol = polySymbol;
        //margin inside the callout to place the text
        balloonCallout.Margin = new CIMTextMargin
        {
          Left = 5,
          Right = 5,
          Bottom = 5,
          Top = 5
        };
        //assign the callout to the text symbol's callout property
        textSymbolBalloonCallout.Callout = balloonCallout;
        //Refer to the snippet "How to apply a Text Symbol to labels of a layer" below to see how to apply this text symbol to a label class
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String)
      //cref: ArcGIS.Core.CIM.CIMPointSymbolCallout
      #region Snippet Creates a point callout text symbol 
      /// <summary>
      /// Creates a highway shield callout text symbol. The [CIMPointSymbolCallout](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic4116.html) created is a highway shield point symbol from the ArcGIS 2D style.
      /// ![lineCallOut](http://Esri.github.io/arcgis-pro-sdk/images/Labeling/highway-callout.png)
      /// </summary>
      {
        //create a text symbol
        //Note: Run this code inside a QueuedTask
        var textSymbolPointCallout = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.WhiteRGB, 6, "Tahoma", "Bold");
        //Create a call out
        var shieldCalloutSymbol = new CIMPointSymbolCallout();
        //Get a Shield symbolStyleItem from ArcGIS 2D StyleProjectitem
        var symbolStyleItem = GetPointSymbol("ArcGIS 2D", "Shield 1");
        //assign the point symbol (Highway shield) to the callout
        shieldCalloutSymbol.PointSymbol = symbolStyleItem.Symbol as CIMPointSymbol;
        shieldCalloutSymbol.PointSymbol.SetSize(18.0); //set symbol size
                                                       //Assign the callout to the text symbol
        textSymbolPointCallout.Callout = shieldCalloutSymbol;
        //Refer to the snippet "How to apply a Text Symbol to labels of a layer" below to see how to apply this text symbol to a label class

        //Gets a Highway point symbol from a style
        static SymbolStyleItem GetPointSymbol(string styleProjectItemName, string symbolStyleName)
        {
          var style2DProjectItem = Project.Current.GetItems<StyleProjectItem>().Where(p => p.Name == styleProjectItemName).FirstOrDefault();
          var symbolStyle = style2DProjectItem.SearchSymbols(StyleItemType.PointSymbol, symbolStyleName).FirstOrDefault();
          return symbolStyle;
        }
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String)
      //cref: ArcGIS.Core.CIM.CIMBackgroundCallout
      #region Snippet Creates a background callout text symbol
      /// <summary>
      /// Creates a solid fill background text symbol with an Accent bar and leader line.  The [CIMBackgroundCallout](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic474.html) created has a solid fill aqua polygon, with a black dash-dot-dash leader line and a solid accent bar.
      /// ![lineCallOut](http://Esri.github.io/arcgis-pro-sdk/images/Labeling//background-callout.png)
      /// </summary>
      {
        //Note: Run this code inside a QueuedTask
        var textSymbolBackgroundCallout = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.BlackRGB, 8, "Tahoma", "Bold");
        //Create a call out
        var backgroundCalloutSymbol = new CIMBackgroundCallout();
        //Leader line
        //Get a line symbol
        var lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlackRGB, 1, SimpleLineStyle.DashDotDot);
        //Create a solid fill polygon symbol for the callout.
        var aquaBackground = ColorFactory.Instance.CreateRGBColor(190, 255, 232, 100);
        var polySymbol = SymbolFactory.Instance.ConstructPolygonSymbol(aquaBackground, SimpleFillStyle.Solid);
        //assign the line to the callout
        backgroundCalloutSymbol.LeaderLineSymbol = lineSymbol;
        //Offset for the text
        textSymbolBackgroundCallout.OffsetX = 10;
        textSymbolBackgroundCallout.OffsetY = 10;
        //Assign the polygon to the background callout
        backgroundCalloutSymbol.BackgroundSymbol = polySymbol;
        //Accent bar
        var accentSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlackRGB, 2, SimpleLineStyle.Solid);
        backgroundCalloutSymbol.AccentBarSymbol = accentSymbol;
        //Set margins for the callout
        backgroundCalloutSymbol.Margin = new CIMTextMargin
        {
          Left = 5,
          Right = 5,
          Top = 5,
          Bottom = 5
        };

        //assign the callout to the textSymbol
        textSymbolBackgroundCallout.Callout = backgroundCalloutSymbol;
        //Refer to the snippet "How to apply a Text Symbol to labels of a layer" below to see how to apply this text symbol to a label class
      }
      #endregion
      //cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructTextSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,System.String,System.String)
      //cref: ArcGIS.Core.CIM.CIMLabelClass
      //cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetLabelVisibility(System.Boolean)
      #region How to apply a Text Symbol to label of a layer
      {
        var cimMap = MapView.Active.Map.GetDefinition();

        CIMTextSymbol simpleTextSymbol = SymbolFactory.Instance.ConstructTextSymbol(ColorFactory.Instance.GreenRGB, 8.5, "Corbel", "Regular");
        var lyrDefn = theLayerToLabel.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Place all labels horizontally
        theLabelClass.StandardLabelPlacementProperties.LineLabelPosition.Horizontal = true;
        //theLabelClass.MaplexLabelPlacementProperties.AlignLabelToLineDirection = true;
        //Set the label classes' symbol to the custom text symbol
        theLabelClass.TextSymbol.Symbol = simpleTextSymbol;
        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        theLayerToLabel.SetDefinition(lyrDefn);
        (theLayerToLabel as FeatureLayer).SetLabelVisibility(true);
      }
      #endregion
    }
  }
}