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
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippets.MapAuthoringSnippets.Symbology
{
  /// <summary>
  /// This methods has a collection of code snippets related to working with ArcGIS Pro Symbol creation.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with creating symbols in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
  public static partial class ProSnippetsMapAuthoring
  {
    public static void ProSnippetsSymbology()
    {
      #region ProSnippet Group: Line Symbology
      #endregion
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(System.Int32,System.String,System.String,System.Int32)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructLineSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Core.CIM.CIMMarkerPlacementAlongLineSameSize

      #region Markers placed at a 45 degree angle
      /// <summary>
      /// Create a line symbol with the markers placed at a 45 degree angle. <br/>  
      /// ![LineSymbolAngleMarker](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/line-marker-angle.png)
      /// </summary>
      /// <returns></returns>
      {
        //Create a line symbol with the markers placed at a 45 degree angle.
        //Create a marker from the "|" character.  This is the marker that will be used to render the line layer.
        //Note: Run withing QueuedTask
        var lineMarker = SymbolFactory.Instance.ConstructMarker(124, "Agency FB", "Regular", 12);

        //Default line symbol which will be modified 
        CIMLineSymbol lineSymbolWithMarkersAtAngle = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.BlackRGB, 2, SimpleLineStyle.Solid);

        //Modifying the marker to align with line
        //First define "markerplacement"
        CIMMarkerPlacementAlongLineSameSize markerPlacement = new CIMMarkerPlacementAlongLineSameSize()
        {
          AngleToLine = true,
          PlacementTemplate = new double[] { 5 }
        };
        //assign the markerplacement to the marker
        lineMarker.MarkerPlacement = markerPlacement;
        //angle the marker if needed
        lineMarker.Rotation = 45;

        //assign the marker as a layer to the line symbol
        lineSymbolWithMarkersAtAngle.SymbolLayers[0] = lineMarker;
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ArcGIS.Core.CIM.CIMSolidStroke
      // cref: ArcGIS.Core.CIM.CIMGeometricEffect
      // cref: ArcGIS.Core.CIM.CIMGeometricEffectDashes
      // cref: ArcGIS.Core.CIM.CIMGeometricEffectOffset
      // cref: ArcGIS.Core.CIM.CIMMarkerPlacementAlongLineSameSize
      #region Dash line with two markers - Method I
      /// <summary>
      /// Create a line symbol with a dash and two markers.<br/>          
      /// </summary>
      /// <remarks>
      /// This line symbol comprises three symbol layers listed below: 
      /// 1. A solid stroke that has dashes.
      /// 1. A circle marker.
      /// 1. A square marker.
      /// ![LineSymbolTwoMarkers](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/line-dash-two-markers.png)
      /// </remarks>
      /// <returns></returns>
      {
        //Create a line symbol with a dash and two markers.
        //Note: Run withing QueuedTask
        CIMLineSymbol dash2MarkersLine = new CIMLineSymbol();
        var mySymbolLyrs = new CIMSymbolLayer[]
              {
                    new CIMSolidStroke()
                    {
                        Color = ColorFactory.Instance.BlackRGB,
                        Enable = true,
                        ColorLocked = true,
                        CapStyle = LineCapStyle.Round,
                        JoinStyle = LineJoinStyle.Round,
                        LineStyle3D = Simple3DLineStyle.Strip,
                        MiterLimit = 10,
                        Width = 1,
                        CloseCaps3D = false,
                        Effects = new CIMGeometricEffect[]
                        {
                            new CIMGeometricEffectDashes()
                            {
                                CustomEndingOffset = 0,
                                DashTemplate = new double[] {20, 10, 20, 10},
                                LineDashEnding = LineDashEnding.HalfPattern,
                                OffsetAlongLine = 0,
                                ControlPointEnding = LineDashEnding.NoConstraint
                            },
                            new CIMGeometricEffectOffset()
                            {
                                Method = GeometricEffectOffsetMethod.Bevelled,
                                Offset = 0,
                                Option = GeometricEffectOffsetOption.Fast
                            }
                        },
                    },
                    CreateCircleMarkerPerSpecs(),
                    CreateSquareMarkerPerSpecs()
          };
        dash2MarkersLine.SymbolLayers = mySymbolLyrs;

        static CIMMarker CreateCircleMarkerPerSpecs()
        {
          var circleMarker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.BlackRGB, 5, SimpleMarkerStyle.Circle) as CIMVectorMarker;
          //Modifying the marker to align with line
          //First define "markerplacement"
          CIMMarkerPlacementAlongLineSameSize markerPlacement = new CIMMarkerPlacementAlongLineSameSize()
          {
            AngleToLine = true,
            Offset = 0,
            Endings = PlacementEndings.Custom,
            OffsetAlongLine = 15,
            PlacementTemplate = new double[] { 60 }
          };
          //assign the markerplacement to the marker
          circleMarker.MarkerPlacement = markerPlacement;
          return circleMarker;
        }
        static CIMMarker CreateSquareMarkerPerSpecs()
        {
          var squareMarker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.BlueRGB, 5, SimpleMarkerStyle.Square) as CIMVectorMarker;
          CIMMarkerPlacementAlongLineSameSize markerPlacement2 = new CIMMarkerPlacementAlongLineSameSize()
          {
            AngleToLine = true,
            Endings = PlacementEndings.Custom,
            OffsetAlongLine = 45,
            PlacementTemplate = new double[] { 60 },
          };
          squareMarker.MarkerPlacement = markerPlacement2;
          return squareMarker;
        }
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(System.Int32,System.String,System.String,System.Int32)
      // cref: ArcGIS.Core.CIM.CIMGeometricEffect
      // cref: ArcGIS.Core.CIM.CIMGeometricEffectDashes
      // cref: ArcGIS.Core.CIM.CIMSolidStroke
      // cref: ArcGIS.Core.CIM.CIMVectorMarker
      #region Dash line with two markers - Method II
      /// <summary>
      /// Create a line symbol with a dash and two markers. <br/>
      /// In this pattern of creating this symbol, a [CIMVectorMarker](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic6176.html) object is created as a new [CIMSymbolLayer](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic5503.html).
      /// The circle and square markers created by [ContructMarker](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic12350.html) method is then assigned to the [MarkerGraphics](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic6188.html) property of the CIMVectorMarker. 
      /// When using this method, the CIMVectorMarker's [Frame](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic6187.html) property needs to be set to the [CIMMarker](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic3264.html) object's Frame. 
      /// Similarly, the CIMVectorMarker's [Size](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic3284.html) property needs to be set to the CIMMarker object's size.
      /// </summary>
      /// <remarks>
      /// This line symbol comprises three symbol layers listed below: 
      /// 1. A solid stroke that has dashes.
      /// 1. A circle marker.
      /// 1. A square marker.
      /// ![LineSymbolTwoMarkers](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/line-dash-two-markers.png)
      /// </remarks>
      /// <returns></returns>
      {
        // Create a line symbol with a dash and two markers.
        //default line symbol that will get modified.
        //Note: Run withing QueuedTask
        CIMLineSymbol dash2MarkersLine = new CIMLineSymbol();
        //circle marker to be used in our line symbol as a layer
        var circleMarker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.BlackRGB, 5, SimpleMarkerStyle.Circle) as CIMVectorMarker;
        //circle marker to be used in our line symbol as a layer
        var squareMarker = SymbolFactory.Instance.ConstructMarker(ColorFactory.Instance.BlueRGB, 5, SimpleMarkerStyle.Square) as CIMVectorMarker;
        //Create the array of layers that make the new line symbol
        CIMSymbolLayer[] mySymbolLyrs =
            {
                    new CIMSolidStroke() //dash line
                    {
                        Color = ColorFactory.Instance.BlackRGB,
                        Enable = true,
                        ColorLocked = true,
                        CapStyle = LineCapStyle.Round,
                        JoinStyle = LineJoinStyle.Round,
                        LineStyle3D = Simple3DLineStyle.Strip,
                        MiterLimit = 10,
                        Width = 1,
                        CloseCaps3D = false,
                        Effects = new CIMGeometricEffect[]
                        {
                            new CIMGeometricEffectDashes()
                            {
                                CustomEndingOffset = 0,
                                DashTemplate = new double[] {20, 10, 20, 10},
                                LineDashEnding = LineDashEnding.HalfPattern,
                                OffsetAlongLine = 0,
                                ControlPointEnding = LineDashEnding.NoConstraint
                            },
                            new CIMGeometricEffectOffset()
                            {
                                Method = GeometricEffectOffsetMethod.Bevelled,
                                Offset = 0,
                                Option = GeometricEffectOffsetOption.Fast
                            }
                        }
                    },
                    new CIMVectorMarker() //circle marker
                    {
                        MarkerGraphics = circleMarker.MarkerGraphics,
                        Frame = circleMarker.Frame, //need to match the CIMVector marker's frame to the circleMarker's frame.
                        Size = circleMarker.Size,    //need to match the CIMVector marker's size to the circleMarker's size.                    
                       MarkerPlacement = new CIMMarkerPlacementAlongLineSameSize()
                       {
                           AngleToLine = true,
                           Offset = 0,
                           Endings = PlacementEndings.Custom,
                           OffsetAlongLine = 15,
                           PlacementTemplate = new double[] {60},
                       }

                    },
                    new CIMVectorMarker() //square marker
                    {
                       MarkerGraphics = squareMarker.MarkerGraphics,
                       Frame = squareMarker.Frame, //need to match the CIMVector marker's frame to the squareMarker frame.
                       Size = squareMarker.Size, //need to match the CIMVector marker's size to the squareMarker size.
                       MarkerPlacement = new CIMMarkerPlacementAlongLineSameSize()
                       {
                           AngleToLine = true,
                           Endings = PlacementEndings.Custom,
                           OffsetAlongLine = 45,
                           PlacementTemplate = new double[] {60},
                       }
                    }
          };
        dash2MarkersLine.SymbolLayers = mySymbolLyrs;
      }
      #endregion
      #region ProSnippet Group: Mesh Symbology
      #endregion

      // cref: ArcGIS.Core.CIM.CIMMeshSymbol
      // cref: ArcGIS.Core.CIM.CIMMaterialSymbolLayer
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.CreateRGBColor(System.Double,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory
      #region Mesh material fill symbol
      /// <summary>
      /// Create a mesh symbol that can be applied to a multi-patch feature layer.
      /// </summary>
      /// <remarks>
      /// A mesh symbol is a CIMMeshSymbol object.  Define an array of CIMSymbolLayers which contains a CIMMaterialSymbol layer with the specified properties such as Color, etc.
      /// Assign this array of CIMSymbolLayers to the CIMMeshSymbol.
      /// ![MeshSymbolOrange](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/mesh-material-orange.png)
      /// </remarks>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        CIMSymbolLayer[] materialSymbolLayer =
              {
                    new CIMMaterialSymbolLayer()
                    {
                        Color = ColorFactory.Instance.CreateRGBColor(230,152,0),
                        MaterialMode = MaterialMode.Multiply
                    }
               };
        var meshMaterialFillSymbol = new CIMMeshSymbol()
        {
          SymbolLayers = materialSymbolLayer
        };
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMMeshSymbol
      // cref: ArcGIS.Core.CIM.CIMProceduralSymbolLayer
      // cref: ArcGIS.Core.CIM.CIMProceduralSymbolLayer.RulePackage
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer
      #region Mesh procedural texture symbol
      /// <summary>
      /// Creates Mesh procedural symbol with various textures.
      /// ![MeshProceduralTexture](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/mesh-procedural-texture.png)
      /// </summary>
      /// <remarks>Note: The rule package used in this method can be obtained from the Sample Data included in the arcgis-pro-sdk-community-samples repository.</remarks>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        string _rulePkgPath = @"C:\Data\RulePackages\MultipatchTextures.rpk";
        CIMSymbolLayer[] proceduralSymbolLyr =
                {
                    new CIMProceduralSymbolLayer()
                    {
                        PrimitiveName = "Textures",
                        RulePackage = _rulePkgPath,
                        RulePackageName = "Textures",
                    }
                };
        var meshProceduralTextureSymbol = new CIMMeshSymbol()
        {
          SymbolLayers = proceduralSymbolLyr
        };
      }
      #endregion
      #region ProSnippet Group: Point Symbology
      #endregion
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      // cref: ArcGIS.Core.CIM.CIMVectorMarker
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructSolidFill(ArcGIS.Core.CIM.CIMColor)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleMarkerStyle)
      #region Custom fill and outline
      /// <summary>
      /// Creates a point symbol with custom fill and outline          
      /// ![PointSymbolMarker](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/point-fill-outline.png)
      /// </summary>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        var circlePtCustomFileOutlineSymbol = SymbolFactory.Instance.ConstructPointSymbol(ColorFactory.Instance.BlueRGB, 6, SimpleMarkerStyle.Circle);
        //Modifying this point symbol with the attributes we want.
        //getting the marker that is used to render the symbol
        var marker = circlePtCustomFileOutlineSymbol.SymbolLayers[0] as CIMVectorMarker;
        //Getting the polygon symbol layers components in the marker
        var polySymbol = marker.MarkerGraphics[0].Symbol as CIMPolygonSymbol;
        //modifying the polygon's outline and width per requirements
        polySymbol.SymbolLayers[0] = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.BlackRGB, 2, SimpleLineStyle.Solid); //This is the outline
        polySymbol.SymbolLayers[1] = SymbolFactory.Instance.ConstructSolidFill(ColorFactory.Instance.GreenRGB); //This is the fill
        //To apply the symbol to a point feature layer
        //var renderer = pointLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = circlePtCustomFileOutlineSymbol.MakeSymbolReference();
        //pointLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMPointSymbol
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructMarker(System.Int32, System.String, System.String, System.Int32)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPointSymbol(ArcGIS.Core.CIM.CIMMarker)
      #region Point Symbol from a font
      /// <summary>
      /// Create a point symbol from a character in a font file
      /// ![PointSymbolFont](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/point-marker.png)
      /// </summary>
      /// <returns></returns>
      {
        //creating the marker from the Font selected
        //Note: Run withing QueuedTask
        var cimMarker = SymbolFactory.Instance.ConstructMarker(47, "Wingdings 3", "Regular", 12);
        CIMPointSymbol fontPointSymbol = SymbolFactory.Instance.ConstructPointSymbol(cimMarker);
        //To apply the symbol to a point feature layer
        //var renderer = pointLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = fontPointSymbol.MakeSymbolReference();
        //pointLayer.SetRenderer(renderer);
      }
      #endregion
      #region ProSnippet Group: Polygon Symbology
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Core.CIM.CIMFill
      // cref: ArcGIS.Core.CIM.CIMHatchFill
      // cref: ArcGIS.Core.CIM.CIMLineSymbol
      #region Diagonal cross hatch fill
      /// <summary>
      /// Create a polygon symbol with a diagonal cross hatch fill. <br/>
      /// ![PolygonSymbolDiagonalCrossHatch](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygon-diagonal-crosshatch.png)
      /// </summary>
      /// <returns></returns>
      {
        var trans = 50.0;//semi transparent
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(0, 0, 0, trans), 2.0, SimpleLineStyle.Solid);

        //Stroke for the fill
        var solid = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(255, 0, 0, trans), 1.0, SimpleLineStyle.Solid);

        //Mimic cross hatch
        CIMFill[] diagonalCross =
                  {
                    new CIMHatchFill() {
                        Enable = true,
                        Rotation = 45.0,
                        Separation = 5.0,
                        LineSymbol = new CIMLineSymbol() { SymbolLayers = new CIMSymbolLayer[1] { solid } }
                    },
                    new CIMHatchFill() {
                        Enable = true,
                        Rotation = -45.0,
                        Separation = 5.0,
                        LineSymbol = new CIMLineSymbol() { SymbolLayers = new CIMSymbolLayer[1] { solid } }
                    }
          };
        List<CIMSymbolLayer> symbolLayers =
          [
            outline, .. diagonalCross
          ];
        //This is the polygon symbol with a diagonal cross hatch fill
        CIMPolygonSymbol diagonalCrossHatchFillSymbol = new()
        {
          SymbolLayers = [.. symbolLayers]
        };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = diagonalCrossHatchFillSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructHatchFill(ArcGIS.Core.CIM.CIMStroke,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      #region Cross hatch
      /// <summary>
      /// Create a polygon symbol using the ConstructHatchFill method . <br/>
      /// ![PolygonSymbolDiagonalCrossHatch](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/ConstructHatchFill.png)
      /// </summary>
      /// <returns></returns>
      {
        CIMStroke lineStroke = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(51, 51, 51, 60), 4, SimpleLineStyle.Solid);
        //gradient
        var hatchFill = SymbolFactory.Instance.ConstructHatchFill(lineStroke, 45, 6, 0);

        List<CIMSymbolLayer> symbolLayers = new()
        {
          hatchFill
        };
        //This is the polygon symbol with a diagonal cross hatch fill
        CIMPolygonSymbol crossHatchPolygonSymbol = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = crossHatchPolygonSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructHatchFill(ArcGIS.Core.CIM.CIMStroke,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Core.CIM.CIMPolygonSymbol
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.CreateRGBColor(System.Double,System.Double,System.Double,System.Double)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory
      // cref: ArcGIS.Core.CIM.CIMHatchFill
      // cref: ArcGIS.Core.CIM.CIMHatchFill.LineSymbol
      // cref: ArcGIS.Core.CIM.CIMSolidFill
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer

      #region Dash dot fill
      /// <summary>
      /// Create a polygon symbol with a dash dot fill. <br/>
      /// ![PolygonSymbolDashDot](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygon-dash-dot.png)
      /// </summary>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        var trans = 50.0;//semi transparent
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(0, 0, 0, trans), 2.0, SimpleLineStyle.Solid);

        //Stroke for the fill            
        var dashDot = SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 1.0, SimpleLineStyle.DashDotDot);
        //Mimic cross hatch
        CIMFill[] solidColorHatch =
              {

                 new CIMHatchFill()
                {
                    Enable = true,
                    Rotation = 0.0,
                    Separation = 2.5,
                    LineSymbol = new CIMLineSymbol(){SymbolLayers = new CIMSymbolLayer[1] {dashDot } }
                },
                 new CIMSolidFill()
                {
                    Enable = true,
                    Color = ColorFactory.Instance.CreateRGBColor(255, 255, 0)
                },
      };
        List<CIMSymbolLayer> symbolLayers = [outline, .. solidColorHatch];
        //This is the polygon symbol with a dash dot fill
        CIMPolygonSymbol dashDotFillPolygon = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = dashDotFillPolygon.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Desktop.Mapping.ColorFactory.ConstructColorRamp(ArcGIS.Desktop.Mapping.ColorRampAlgorithm,ArcGIS.Core.CIM.CIMColor,ArcGIS.Core.CIM.CIMColor)
      // cref: ArcGIS.Core.CIM.CIMGradientFill
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer
      // cref: ArcGIS.Core.CIM.CIMFill
      // cref: ArcGIS.Core.CIM.CIMStroke
      // cref: ArcGIS.Core.CIM.CIMSymbolLayer

      #region Gradient color fill using CIMGradientFill
      /// <summary>
      /// Create a polygon symbol with a gradient color fill. <br/>
      /// ![PolygonSymbolGradientColor](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygon-gradient-color.png)
      /// </summary>
      /// <remarks>
      /// 1. Create a solid colored stroke with 50% transparency
      /// 1. Create a fill using gradient colors red through green
      /// 1. Apply both the stroke and fill as a symbol layer array to the new PolygonSymbol
      /// </remarks>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        var trans = 50.0;//semi transparent
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(0, 0, 0, trans), 2.0, SimpleLineStyle.Solid);
        //Mimic cross hatch
        CIMFill solidColorHatch =
               new CIMGradientFill()
               {
                 ColorRamp = ColorFactory.Instance.ConstructColorRamp(ColorRampAlgorithm.LinearContinuous,
                                      ColorFactory.Instance.RedRGB, ColorFactory.Instance.GreenRGB)
               };
        List<CIMSymbolLayer> symbolLayers = new List<CIMSymbolLayer>
          {
                    outline,
                    solidColorHatch
          };

        CIMPolygonSymbol gradientColorFillPolygon = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = gradientColorFillPolygon.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructGradientFill(ArcGIS.Core.CIM.CIMColorRamp,ArcGIS.Core.CIM.GradientFillMethod,System.Int32)
      #region Gradient fill between two colors
      /// <summary>
      /// Create a polygon symbol using the ConstructGradientFill method. Constructs a gradient fill between two colors passed to the method. <br/>
      /// ![PolygonSymbolTwoColors](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/PolygonSymbolTwoColors.png)
      /// </summary>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        //gradient fill between 2 colors
        var gradientFill = SymbolFactory.Instance.ConstructGradientFill(CIMColor.CreateRGBColor(235, 64, 52), CIMColor.NoColor(), GradientFillMethod.Linear);
        List<CIMSymbolLayer> symbolLayers = new List<CIMSymbolLayer>
          {
                    gradientFill
          };
        CIMPolygonSymbol gradientFillTwoColors = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = gradientFillTwoColors.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructGradientFill(ArcGIS.Core.CIM.CIMColorRamp,ArcGIS.Core.CIM.GradientFillMethod,System.Int32)

      #region Gradient fill using Color ramp
      /// <summary>
      /// Create a polygon symbol using the ConstructGradientFill method. Constructs a gradient fill using the specified color ramp. <br/>
      /// ![PolygonSymbolColorRamp](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/PolygonSymbolColorRamp.png)
      /// </summary>
      /// <returns></returns>
      /// 
      {
        //Note: Run withing QueuedTask
        //outine
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(49, 49, 49), 2.0, SimpleLineStyle.Solid);

        //gradient fill using a color ramp
        var gradientFill = SymbolFactory.Instance.ConstructGradientFill(GetColorRamp(), GradientFillMethod.Linear);

        List<CIMSymbolLayer> symbolLayers = new List<CIMSymbolLayer>
          {
                    outline,
                    gradientFill
          };
        CIMPolygonSymbol gradientColorRampSymbol = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = gradientColorRampSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);

        //Helper method to get a color ramp from the "ArcGIS Colors" style.
        static CIMColorRamp GetColorRamp()
        {
          //Get a ColorRamp
          StyleProjectItem style =
                  Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS Colors");
          var colorRampList = style.SearchColorRamps("Heat Map 4 - Semitransparent");

          CIMColorRamp colorRamp = colorRampList[0].ColorRamp;
          return colorRamp;
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPictureFill(System.String,System.Double)
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)

      #region Picture fill
      /// <summary>
      /// Constructs a picture fill with the specified parameters.
      /// ![ConstructPictureFill](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/ConstructPictureFill.png)
      /// </summary>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(110, 110, 110), 2.0, SimpleLineStyle.Solid);
        //picture
        var imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Images\CaliforniaEmblem.png");
        var pictureFill = SymbolFactory.Instance.ConstructPictureFill(imgPath, 64);

        List<CIMSymbolLayer> symbolLayers = new()
        {
          outline,
          pictureFill
        };
        CIMPolygonSymbol pictureFillPolygonSymbol = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = pictureFillPolygonSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.ISymbolFactory.ConstructWaterFill(ArcGIS.Core.CIM.CIMColor,ArcGIS.Core.CIM.WaterbodySize,ArcGIS.Core.CIM.WaveStrength) 
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructStroke(ArcGIS.Core.CIM.CIMColor,System.Double,ArcGIS.Desktop.Mapping.SimpleLineStyle)

      #region Animation water
      /// <summary>
      /// Constructs a water fill of specific color, waterbody size and wave strength. This fill can be used on polygon feature classes in a Scene view only.
      /// ![ConstructWaterFill](https://github.com/ArcGIS/arcgis-pro-sdk/blob/master/Images/waterAnimation.gif)
      /// </summary>
      /// <returns></returns>
      /// 
      {
        //Note: Run withing QueuedTask

        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(49, 49, 49, 50.0), 2.0, SimpleLineStyle.Solid);
        var waterFill = SymbolFactory.Instance.ConstructWaterFill(CIMColor.CreateRGBColor(3, 223, 252), WaterbodySize.Large, WaveStrength.Rippled);
        List<CIMSymbolLayer> symbolLayers = new List<CIMSymbolLayer>
          {
                    outline,
                    waterFill
          };
        CIMPolygonSymbol waterFillPolygonSymbol = new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = waterFillPolygonSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbolWithPenInkRipple(ArcGIS.Core.CIM.CIMColor) 

      #region Pen and Ink: Ripple
      /// <summary>
      /// Constructs a polygon symbol in the specified color representing a pen and ink ripple water fill. See https://www.esri.com/arcgis-blog/products/arcgis-pro/mapping/please-steal-this-pen-and-ink-style/
      /// ![polygonRipple.png](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygonRipple.png)
      /// </summary>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        //Ripple pen and ink
        var penInkRipple = SymbolFactory.Instance.ConstructPolygonSymbolWithPenInkRipple(CIMColor.CreateRGBColor(13, 24, 54));
        CIMPolygonSymbol penInkRipplePolygonSymbol = penInkRipple;
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = penInkRipplePolygonSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbolWithPenInkStipple(ArcGIS.Core.CIM.CIMColor,System.Boolean) 
      #region Pen and Ink: Stipple
      /// <summary>
      /// Constructs a polygon symbol in the specified color representing a pen and ink stipple effect. See https://www.esri.com/arcgis-blog/products/arcgis-pro/mapping/please-steal-this-pen-and-ink-style/
      /// ![polygonStipple.png](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygonStipple.png)
      /// </summary>
      /// <returns></returns>

      {
        //Note: Run withing QueuedTask
        //Stipple pen and ink
        var penInkRipple = SymbolFactory.Instance.ConstructPolygonSymbolWithPenInkStipple(CIMColor.CreateRGBColor(78, 133, 105), true);
        CIMPolygonSymbol penInkStipple = penInkRipple;
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = penInkStipple.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbolWithPenInkCrossHatch(ArcGIS.Core.CIM.CIMColor,System.Boolean) 

      #region Pen and Ink: Cross Hatch 
      /// <summary>
      /// Constructs a polygon symbol in the specified color representing a pen and ink cross hatch effect. See https://www.esri.com/arcgis-blog/products/arcgis-pro/mapping/please-steal-this-pen-and-ink-style/
      /// ![polygonPNHatch.png](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygonPNHatch.png)
      /// </summary>
      /// <returns></returns>
      {
        //Note: Run withing QueuedTask
        //Cross Hatch pen and ink
        var penkInkCrossHatch = SymbolFactory.Instance.ConstructPolygonSymbolWithPenInkCrossHatch(CIMColor.CreateRGBColor(168, 49, 22), true);
        CIMPolygonSymbol penInkCrossHatch = penkInkCrossHatch;
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = penInkCrossHatch.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
      // cref: ArcGIS.Core.CIM.CIMProceduralSymbolLayer
      // cref: ArcGIS.Desktop.Mapping.SymbolFactory.ConstructPolygonSymbol

      #region Procedural Symbol
      /// <summary>
      /// Create a procedural symbol that can be applied to a polygon building footprint layer
      /// ![ProceduralSymbol](http://Esri.github.io/arcgis-pro-sdk/images/Symbology/polygon-procedural.png)        
      /// </summary>    
      /// <remarks>Note: The rule package used in this method can be obtained from the Sample Data included in the arcgis-pro-sdk-community-samples repository.</remarks>
      {
        //Note: Run withing QueuedTask
        string _rulePkgPath = @"C:\Data\RulePackages\Venice_2014.rpk";
        //Polygon symbol to hold the procedural layer
        var proceduralSymbol = SymbolFactory.Instance.ConstructPolygonSymbol();

        //Array of layers to hold a procedural symbol layer
        CIMSymbolLayer[] proceduralSymbolLyr =
              {
                  new CIMProceduralSymbolLayer()
                  {
                      PrimitiveName = "Venice Rule package 2014",
                      RulePackage = _rulePkgPath,
                      RulePackageName = "Venice_2014",
                  }
        };
        proceduralSymbol.SymbolLayers = proceduralSymbolLyr;
        //To apply the symbol to a polygon feature layer
        //var renderer = theLayer.GetRenderer() as CIMSimpleRenderer;
        //renderer.Symbol = proceduralSymbol.MakeSymbolReference();
        //theLayer.SetRenderer(renderer);
      }
      #endregion
    }
  }
}
