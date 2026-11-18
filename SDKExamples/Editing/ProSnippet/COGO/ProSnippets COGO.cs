using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.OCR;
using ArcGIS.Core.Parcels;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.COGO;
using ArcGIS.Desktop.Mapping;

namespace ProSnippets.EditingSnippets.COGO
{
  internal static partial class ProSnippetsEditing
  {
    /// <summary>
    /// This class provides code snippets for using the COGO APIs in the ArcGIS Pro SDK for .NET.
    /// </summary>
    /// <remarks>
    /// The following code snippets demonstrate the use of COGO APIs within the ArcGIS Pro SDK.
    /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
    /// The Pro SDK Add-in may require certain code snippets to execute on the ArcGIS Pro Main CIM(MCT) thread or the UI thread.
    /// In such instances, the code should be enclosed within the QueuedTask.Run() block.
    /// <c>Cref</c>s are used for internal purposes only. Please ignore them in the context of this example.
    /// </remarks>
    public static async void ProSnippetsCOGOAsync()
    {
      #region ignore - Variable initialization
      Map map = MapView.Active.Map;
      List<COGOLine> cogoLineList = null;
      COGOLine cogoLine = null;

      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.COGOLineBuilder.CreateCOGOStraightLine(System.Double, System.Double)
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLine
      #region Create a COGOLine defined by direction and distance
      {

        var nAzDirection = ParcelUtilities.Instance.ConvertQuadrantBearingDMSToNorthAzimuthDecDeg("S7-46-51-E");
        var cogoStraightLine = COGOLineBuilder.CreateCOGOStraightLine(nAzDirection, 61.45);

      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.COGOLineBuilder.CreateCOGOStraightLineByClockwiseDeflectionAngle(System.Double,System.Boolean,System.Double)
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLine
      #region Create a COGOLine defined by deflection angle and distance
      {

        // Clockwise deflection angle 90 degrees, 50.00
        var deflectionLine = COGOLineBuilder.CreateCOGOStraightLineByClockwiseDeflectionAngle(90.0, false, 50.00);

        //Clockwise backsight deflection angle 270 degrees, 50.00'
        var backSightDeflectionLine = COGOLineBuilder.CreateCOGOStraightLineByClockwiseDeflectionAngle(270.0, true, 50.00);

      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.CircularArcDefinition
      // cref: ArcGIS.Desktop.Editing.COGO.CircularArcDefinition.ChordDirection
      // cref: ArcGIS.Desktop.Editing.COGO.CircularArcDefinition.TangentDirection
      // cref: ArcGIS.Desktop.Editing.COGO.CircularArcDefinition.RadialDirection
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLineBuilder.CreateCOGOCircularArc(ArcGIS.Desktop.Editing.COGO.CircularArcDefinition)
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLine
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.ArcLength
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.CentralAngle
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.ChordLength
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.Radius
      #region Create a COGOLine defined by a circular arc
      {

        // define with a radius, arcLength and chord direction
        var circularArcDef = new CircularArcDefinition
        {
          Radius = -201.0,
          ArcLength = 45.29,
          ChordDirection = ParcelUtilities.Instance.ConvertQuadrantBearingDMSToNorthAzimuthDecDeg("N21-47-57W")
        };
        var cogoCircArc = COGOLineBuilder.CreateCOGOCircularArc(circularArcDef);


        // define with a radius, chordLength and tangent direction
        var circularArcDef2 = new CircularArcDefinition
        {
          Radius = -201.0,
          ChordLength = 44.0,
          TangentDirection = 340.0
        };
        var cogoCircArc2 = COGOLineBuilder.CreateCOGOCircularArc(circularArcDef2);

        // define with a radius, central angle and radial direction
        var circularArcDef3 = new CircularArcDefinition
        {
          Radius = -201.0,
          CentralAngle = 44.0,
          RadialDirection = 340.0
        };
        var cogoCircArc3 = COGOLineBuilder.CreateCOGOCircularArc(circularArcDef3);

      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.TangentCurveDefinition
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLineBuilder.CreateCOGOCircularArc(ArcGIS.Desktop.Editing.COGO.TangentCurveDefinition)
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLine
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.ArcLength
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.CentralAngle
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.ChordLength
      // cref: ArcGIS.Desktop.Editing.COGO.BaseCurveDefinition.Radius
      #region Create a COGOLine defined by a tangent curve
      {

        var tangentCircArcDef = new TangentCurveDefinition
        {
          Radius = 169.0,
          ArcLength = 52.40
        };
        var cogoTangentArc = COGOLineBuilder.CreateCOGOCircularArc(tangentCircArcDef);

        var tangentCircArcDef2 = new TangentCurveDefinition
        {
          Radius = -201.0,
          ChordLength = 23.24
        };
        var cogoTangentArc2 = COGOLineBuilder.CreateCOGOCircularArc(tangentCircArcDef2);

        var tangentCircArcDef3 = new TangentCurveDefinition
        {
          Radius = 169.0,
          CentralAngle = 44.0
        };
        var cogoTangentArc3 = COGOLineBuilder.CreateCOGOCircularArc(tangentCircArcDef3);

      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.COGOLine.Distance
      // cref: ArcGIS.Desktop.Editing.COGO.COGOLine.Direction
      #region Get COGOLine parameter information

      if (cogoLine.IsStraightLine)
      {
        var dist = cogoLine.Distance;
        var dir = cogoLine.Direction;
      }
      else if (cogoLine.IsCircularArc)
      {
        var circArcDefinition = cogoLine.ToCircularArcDefinition();
        string s = "";

        //show Arc length, either entered or calculated
        if (circArcDefinition.IsDefinedByArcLength)
        {
          if (circArcDefinition.ArcLength.HasValue)
          {
            s += "<entered> Arc Length: " + circArcDefinition.ArcLength.Value.ToString("F3")
              + Environment.NewLine;
          }
        }
        else
        {
          if (circArcDefinition.CanCalculateArcLength())
          {
            s += "<calc> Arc Length: " + circArcDefinition.CalculateArcLength().ToString("F3")
                 + Environment.NewLine;
          }
          else
          {
            s += "<calc> not able to calc arc length." + Environment.NewLine;
          }
        }

        // chordLength
        if (circArcDefinition.IsDefinedByChordLength)
        {
          if (circArcDefinition.ChordLength.HasValue)
          {
            s += "<entered> Chord Length: " + circArcDefinition.ChordLength.Value.ToString("F3")
              + Environment.NewLine;
          }
        }
        else
        {
          if (circArcDefinition.CanCalculateChordLength())
          {
            s += "<calc> Chord Length: " + circArcDefinition.CalculateChordLength().ToString("F3")
                 + Environment.NewLine;
          }
          else
          {
            s += "<calc> not able to calc chord length." + Environment.NewLine;
          }
        }

        // central Angle
        if (circArcDefinition.IsDefinedByCentralAngle)
        {
          if (circArcDefinition.CentralAngle.HasValue)
          {
            s += "<entered> Central Angle: " + circArcDefinition.CentralAngle.Value.ToString("F3")
              + Environment.NewLine;
          }
        }
        else
        {
          if (circArcDefinition.CanCalculateCentralAngle())
          {
            s += "<calc> Central Angle: " + circArcDefinition.CalculateCentralAngle().ToString("F3")
                 + Environment.NewLine;
          }
          else
          {
            s += "<calc> not able to calc Central Angle." + Environment.NewLine;
          }
        }
      }
      #endregion

      #region Create a Traverse

      List<COGOLine> lstCOGOLines = new();
      var direction = ParcelUtilities.Instance.ConvertQuadrantBearingDMSToNorthAzimuthDecDeg("S7-46-51-E");
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOStraightLine(direction, 61.45)); //1
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOStraightLine(166.2625, 37.22));//2
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOStraightLine(255.2833333, 71.4));//3
      var circArcDef = new CircularArcDefinition
      {
        Radius = -201.0,
        ArcLength = 45.29,
        ChordDirection = ParcelUtilities.Instance.ConvertQuadrantBearingDMSToNorthAzimuthDecDeg("N21-47-57W")
      };
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOCircularArc(circArcDef));//4
      var tangentCurveDef = new TangentCurveDefinition
      {
        Radius = 169.0,
        ArcLength = 52.40
      };
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOCircularArc(tangentCurveDef));//5

      tangentCurveDef = new TangentCurveDefinition
      {
        Radius = 13.0,
        ArcLength = 22.98
      };
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOCircularArc(tangentCurveDef));//6

      tangentCurveDef = new TangentCurveDefinition
      {
        Radius = -281.0,
        ArcLength = 73.68
      };
      lstCOGOLines.Add(COGOLineBuilder.CreateCOGOCircularArc(tangentCurveDef));//7

      var spatialReference = map?.SpatialReference;
      var travParcel = new Traverse(spatialReference);
      await travParcel.AddCoursesAsync(lstCOGOLines);
      if (travParcel.IsValid())
      {
        ; //insert code to use the traverse
      }

      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.Course.COGOLine
      // cref: ArcGIS.Desktop.Editing.COGO.Course.EntranceTangentDirection
      // cref: ArcGIS.Desktop.Editing.COGO.Course.ExitTangentDirection
      // cref: ArcGIS.Desktop.Editing.COGO.Course.IsDefinedAsTangentToPriorCourse
      // cref: ArcGIS.Desktop.Editing.COGO.Course.IsTangent
      #region Iterate Traverse Courses

      // iterate thru courses
      foreach (var course in travParcel.Courses)
      {
        var courseCOGOLine = course.COGOLine;

        var entranceTangentDir = course.EntranceTangentDirection;
        var exitTangentDir = course.ExitTangentDirection;

        // tangent to prior course if one of the following
        //     - defined with a ArcGIS.Desktop.Editing.COGO.TangentCurveDefinition
        //     - defined as a straight line defined with a 0 deflection angle.
        var isTangentToPriorCourse = course.IsDefinedAsTangentToPriorCourse;
        // IsTangent if one of the following
        //    - IsTangentToPriorCourse
        //    - is implied tangent - ie difference between entranceTangentDirection and previous courses ExitTangentDirection < tolerance
        var isTangent = course.IsTangent;
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.GetClosure()
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure.MiscloseDistance
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure.MiscloseDirection
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure.MiscloseX
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure.MiscloseY
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure.CalculatedArea
      #region Traverse Closure

      if (travParcel.IsValid())
      {
        string sReport = "";
        TraverseClosure travClosure = travParcel.GetClosure();

        var miscDistance = travClosure.MiscloseDistance;
        var miscDirection = travClosure.MiscloseDirection;
        var miscX = travClosure.MiscloseX;
        var miscY = travClosure.MiscloseY;
        var calcArea = travClosure.CalculatedArea;

        sReport = "Misclose distance: " + miscDistance.ToString("F2");
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.GetClosure()
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseClosure.MiscloseDistance
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.CanAdjust(ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentMethod)
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.Adjust(ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentMethod)
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentMethod
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentResults
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentResults.AdjustedCoordinates
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentResults.AdjustedCourses
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentResults.DistanceResiduals
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseAdjustmentResults.DirectionalResiduals
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.AdjustmentResults
      #region Traverse Adjustment

      if (travParcel.IsValid())
      {
        TraverseClosure travClosure = travParcel.GetClosure();

        if (travClosure.MiscloseDistance < 0.1)
        {
          if (travParcel.CanAdjust(TraverseAdjustmentMethod.Compass))
          {
            travParcel.Adjust(TraverseAdjustmentMethod.Compass);

            TraverseAdjustmentResults adjResults = travParcel.AdjustmentResults;

            var adjCoords = adjResults.AdjustedCoordinates;
            var adjustedCourses = adjResults.AdjustedCourses;
            var distanceResiduals = adjResults.DistanceResiduals;
            var directionResiduals = adjResults.DirectionalResiduals;
          }
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.UseGroundToGridCorrections
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.GroundToGridCorrection
      #region Calculating Coordinates

      var startPoint = MapPointBuilderEx.CreateMapPoint(7500500.0, 445900.0, spatialReference);
      travParcel.StartPoint = startPoint;

      // The geometry of the features created is affected by the ground to grid correction.
      // This correction is also assigned to the traverse and in the following code the
      // active map's ground to grid correction is used.
      var g2gCorrection = map?.GetDefinition()?.GroundToGridCorrection;
      if (g2gCorrection != null)
      {
        travParcel.UseGroundToGridCorrections = true;
        travParcel.GroundToGridCorrection = g2gCorrection;
      }
      else
      {
        travParcel.UseGroundToGridCorrections = false;
      }

      // calculate coordinates
      if (travParcel.CanCalculateCoordinates())
      {
        var coords = travParcel.CalculateCoordinates();
        var sb = new StringBuilder();
        sb.AppendLine("Calculated Coordinates:");
        foreach (var coord in coords)
        {
          sb.AppendLine($"{coord.X:F2}, {coord.Y:F2}");
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.GetCourseGeometries()
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.GetPolygonGeometry()
      #region Traverse Geometries
      {
        // get polyline and polygon geometries
        IReadOnlyList<ArcGIS.Core.Geometry.Polyline> polyLines = travParcel.GetCourseGeometries();
        ArcGIS.Core.Geometry.Polygon polygon = travParcel.GetPolygonGeometry();

        // build some symbols to add the geometries to the overlay
        var lineSymbol = SymbolFactory.Instance.ConstructLineSymbol(ColorFactory.Instance.RedRGB);
        var polygonSymbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlueRGB, SimpleFillStyle.Horizontal);

        var lineSymbolRef = lineSymbol.MakeSymbolReference();
        var polySymbolRef = polygonSymbol.MakeSymbolReference();

        // add to overlay
        foreach (var polyline in polyLines)
        {
          var disp = MapView.Active.AddOverlay(polyline, lineSymbolRef);
        }

        if (polygon != null)
        {
          var polyDisp = MapView.Active.AddOverlay(polygon, polySymbolRef);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.EditOperation.CreateTraverse(ArcGIS.Desktop.Editing.COGO.Traverse,ArcGIS.Desktop.Editing.Templates.EditingTemplate,System.Collections.Generic.Dictionary{System.String,System.Object})
      #region Create Features

      var layerName = "Lot_Lines"; //Name of the COGO enabled line layer in the map
      var fLayer = MapView.Active?.Map?.GetLayersAsFlattenedList().OfType<FeatureLayer>().Where(l => l.
                Name.Equals(layerName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
      var templateLotLines = fLayer.GetTemplates().FirstOrDefault(); //get the feature template

      var editOper = new EditOperation
      {
        Name = "Create Traverse",
        SelectNewFeatures = false
      };

      var dirOffset = g2gCorrection.GetDirectionOffset();
      var scaleFactor = g2gCorrection.GetConstantScaleFactor();
      var dictAttributes = new Dictionary<string, object>();
      dictAttributes["Rotation"] = dirOffset;
      dictAttributes["Scale"] = scaleFactor;
      if (editOper.CanCreateTraverse(travParcel, templateLotLines))
      {
        await editOper.CreateTraverse(travParcel, templateLotLines, dictAttributes);
      }

      if (!editOper.IsEmpty)
      {
        editOper.Execute();

        // access the mapMember, OID for each created feature
        foreach (var course in travParcel.Courses)
        {
          var mapMember = course.MapMember;
          var oid = course.ObjectID;

          var insp = new ArcGIS.Desktop.Editing.Attributes.Inspector();
          insp.Load(mapMember, oid);
          var shape = insp.Shape;
        }
      }
      #endregion

      #region Loading a traverse into the Traverse UI

      var isValidTrav = travParcel.IsValid(); //confirm it's a valid traverse
      if (!isValidTrav)
      {
        return;
      }

      //Load the traverse into the traverse grid
      await TraverseManager.Current.LoadTraverse(travParcel, "Load my custom traverse");
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.TraverseManager.GetTraverseAsync()
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse
      // cref: ArcGIS.Desktop.Editing.COGO.TraverseManager.Current
      #region Get a traverse from the Traverse UI
      {
        var traverseFromUI = await TraverseManager.Current.GetTraverseAsync();
      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.ImportAsync(System.String, ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse
      #region Import a traverse file
      {

        var pathAndNameOfTraverseFile = "C:\\MyTraverseFileFolder\\MyTraverseFile.txt";
        var traverse = await Traverse.ImportAsync(pathAndNameOfTraverseFile, spatialReference);

      }
      #endregion

      // cref: ArcGIS.Desktop.Editing.COGO.TraverseExportOptions
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.Export(ArcGIS.Desktop.Editing.COGO.TraverseExportOptions)
      #region Export a traverse to a file
      {

        var directionDecimalPlaces = 0;
        var distanceDecimalPlaces = 3;
        var exportOptions = new TraverseExportOptions("C:\\MyTraverseFileFolder\\MyTraverseFile.txt",
                                      ArcGIS.Core.CIM.DirectionType.QuadrantBearing,
                                      ArcGIS.Core.CIM.DirectionUnits.DegreesMinutesSeconds,
                                      directionDecimalPlaces, distanceDecimalPlaces);
        travParcel.Export(exportOptions);

      }
      #endregion


      // cref: ArcGIS.Core.OCR.OCRExtensions.ExtractTraverseFromDeedAsync(System.String,ArcGIS.Core.Geometry.SpatialReference)
      // cref: ArcGIS.Desktop.Editing.COGO.Traverse.Courses
      #region Extract a traverse from a deed
      {
        string deedFileName = "c:\\MyTraverseFileFolder\\MyTraverseDeed.pdf";

        var traverseList = await OCRManager.Instance.ExtractTraverseFromDeedAsync(deedFileName, spatialReference);

        // or extract from a specific page
        //int pageIdx = 1;
        //var traverseList = await OCRManager.Instance.ExtractTraverseFromDeedAsync(deedFileName, pageIdx, spatialReference);

        foreach (var trav in traverseList)
        {
          // iterate thorugh the courses
          foreach (var course in trav.Courses)
          {
            // ...
          }
        }
      }
      #endregion
    }
  }
}
