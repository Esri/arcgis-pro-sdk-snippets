/*

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

namespace MapAuthoring.ProSnippets
{
  public static class Label 
  {
    // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMap.GeneralPlacementProperties
    #region Get the active map's labeling engine - Maplex or Standard labeling engine

    /// <summary>
    /// Get the active map's labeling engine - Maplex or Standard labeling engine.
    /// </summary>
    public static Task<CIMGeneralPlacementProperties> GetMapLabelEngineAsync()
    {
      return QueuedTask.Run<CIMGeneralPlacementProperties>(() =>
      {
        //Get the active map's definition - CIMMap.
        var cimMap = MapView.Active.Map.GetDefinition();
        //Get the labeling engine from the map definition
        CIMGeneralPlacementProperties labelEngine = cimMap.GeneralPlacementProperties;
        
        return labelEngine;
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMGeneralPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMap.GeneralPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMStandardGeneralPlacementProperties
    #region Change the active map's labeling engine from Standard to Maplex or vice versa
    /// <summary>
    /// Change the active map's labeling engine from Standard to Maplex or vice versa
    /// </summary>
    public static Task ChangeLabelEngineAsync()
    {
      return QueuedTask.Run(() =>
      {
        //Get the active map's definition - CIMMap.
        var cimMap = MapView.Active.Map.GetDefinition();
        //Get the labeling engine from the map definition
        var cimGeneralPlacement = cimMap.GeneralPlacementProperties;

        if (cimGeneralPlacement is CIMMaplexGeneralPlacementProperties)
        {
          //Current labeling engine is Maplex labeling engine
          //Create a new standard label engine properties
          var cimStandardPlacementProperties = new CIMStandardGeneralPlacementProperties();
          //Set the CIMMap's GeneralPlacementProperties to the new label engine
          cimMap.GeneralPlacementProperties = cimStandardPlacementProperties;
        }
        else
        {
          //Current labeling engine is Standard labeling engine
          //Create a new Maplex label engine properties
          var cimMaplexGeneralPlacementProperties = new CIMMaplexGeneralPlacementProperties();
          //Set the CIMMap's GeneralPlacementProperties to the new label engine
          cimMap.GeneralPlacementProperties = cimMaplexGeneralPlacementProperties;
        }
        //Set the map's definition
        MapView.Active.Map.SetDefinition(cimMap);
      });
    }
    #endregion

    // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.LabelClasses
    // cref: ArcGIS.Core.CIM.CIMLabelClass
    // cref: ArcGIS.Core.CIM.CIMLabelClass.TextSymbol
    #region Apply text symbol to a feature layer
    /// <summary>
    /// Apply text symbol to a feature layer. 
    /// </summary>
    public static Task ApplyLabelAsync(FeatureLayer featureLayer, CIMTextSymbol textSymbol)
    {
      return QueuedTask.Run(() =>
      {
        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Set the label classes' symbol to the custom text symbol
        //Refer to the ProSnippets-TextSymbols wiki page for help with creating custom text symbols.
        //Example: var textSymbol = await CreateTextSymbolWithHaloAsync();
        theLabelClass.TextSymbol.Symbol = textSymbol;
        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                                             //set the label's visiblity
        featureLayer.SetLabelVisibility(true);
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.IsLabelVisible
    // cref: ArcGIS.Desktop.Mapping.FeatureLayer.SetLabelVisibility
    #region Enable labeling of a layer
    /// <summary>
    /// Enable labeling of a layer
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task EnableLayerLabelVisibility(FeatureLayer featureLayer)
    {
      return QueuedTask.Run(() =>
      {
        if (!featureLayer.IsLabelVisible)
          //set the label's visiblity
          featureLayer.SetLabelVisibility(true);
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMGeoFeatureLayerBase.LabelClasses
    // cref: ArcGIS.Core.CIM.CIMLabelClass
    // cref: ArcGIS.Core.CIM.CIMLabelClass.StandardLabelPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMLabelClass.MaplexLabelPlacementProperties
    // cref : ArcGIS.Core.CIM.CIMMaplexGeneralPlacementProperties.PointPlacementMethod
    // cref : ArcGIS.Core.CIM.CIMStandardGeneralPlacementProperties.PointPlacementMethod
    // cref : ArcGIS.Core.CIM.StandardPointPlacementMethod
    // cref : ArcGIS.Core.CIM.MaplexPointPlacementMethod
    // cref : ArcGIS.Core.CIM.CIMMap.GeneralPlacementProperties
    #region Modify the Placement/Position of labels - Point geometry

    /// <summary>
    /// Modify the Placement/Position of labels - Point geometry
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task ModifyLabelsPlacementPointAsync(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPoint)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
     {
       //Get the layer's definition
       var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
       //Get the label classes - we need the first one
       var listLabelClasses = lyrDefn.LabelClasses.ToList();
       var theLabelClass = listLabelClasses.FirstOrDefault();

       //Modify label Placement 
       //Check if the label engine is Maplex or standard.
       CIMGeneralPlacementProperties labelEngine = 
          MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
       if (labelEngine is CIMStandardGeneralPlacementProperties) //Current labeling engine is Standard labeling engine               
         theLabelClass.StandardLabelPlacementProperties.PointPlacementMethod = 
                  StandardPointPlacementMethod.OnTopPoint;
       else    //Current labeling engine is Maplex labeling engine            
         theLabelClass.MaplexLabelPlacementProperties.PointPlacementMethod = 
                 MaplexPointPlacementMethod.CenteredOnPoint;

       lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
       featureLayer.SetDefinition(lyrDefn); //set the layer's definition
     });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMStandardLineLabelPosition
    // cref: ArcGIS.Core.CIM.CIMStandardLineLabelPosition.Perpendicular
    // cref: ArcGIS.Core.CIM.CIMStandardLineLabelPosition.Parallel
    // cref: ArcGIS.Core.CIM.CIMStandardLineLabelPosition.ProduceCurvedLabels
    // cref: ArcGIS.Core.CIM.CIMStandardLineLabelPosition.Horizontal
    // cref: ArcGIS.Core.CIM.CIMStandardLineLabelPosition.OnTop
    // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.LineLabelPosition
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.LinePlacementMethod
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.LineFeatureType
    // cref: ArcGIS.Core.CIM.MaplexLinePlacementMethod
    // cref: ArcGIS.Core.CIM.MaplexLineFeatureType
    #region Modify the Placement/Position of labels - Line geometry
    /// <summary>
    /// Modify the Placement/Position of labels - Line geometry
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task ModifyLabelsPlacementLineAsync(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolyline)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
      {
        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Modify label Placement 
        //Check if the label engine is Maplex or standard.
        CIMGeneralPlacementProperties labelEngine = 
            MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        if (labelEngine is CIMStandardGeneralPlacementProperties)
        {
          //Current labeling engine is Standard labeling engine
          var lineLablePosition = new CIMStandardLineLabelPosition
          {
            Perpendicular = true,
            Parallel = false,
            ProduceCurvedLabels = false,
            Horizontal = false,
            OnTop = true
          };
          theLabelClass.StandardLabelPlacementProperties.LineLabelPosition = 
              lineLablePosition;
        }
        else //Current labeling engine is Maplex labeling engine
        {
          theLabelClass.MaplexLabelPlacementProperties.LinePlacementMethod = 
                        MaplexLinePlacementMethod.CenteredPerpendicularOnLine;
          theLabelClass.MaplexLabelPlacementProperties.LineFeatureType = 
                        MaplexLineFeatureType.General;
        }
        //theLabelClass.MaplexLabelPlacementProperties.LinePlacementMethod = MaplexLinePlacementMethod.CenteredPerpendicularOnLine;
        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.PolygonPlacementMethod
    // cref: ArcGIS.Core.CIM.CIMStandardLabelPlacementProperties.PlaceOnlyInsidePolygon
    // cref: ArcGIS.Core.CIM.StandardPolygonPlacementMethod
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.PolygonFeatureType
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AvoidPolygonHoles
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.PolygonPlacementMethod
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.CanPlaceLabelOutsidePolygon
    // cref: ArcGIS.Core.CIM.MaplexPolygonFeatureType
    // cref: ArcGIS.Core.CIM.MaplexPolygonPlacementMethod
    #region Modify the Placement/Position of labels - Polygon geometry
    /// <summary>
    /// Modify the Placement/Position of labels - Polygon geometry
    /// </summary>
    /// <param name="featureLayer"></param>

    public static Task ModifyLabelsPlacementPolygonAsync(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolygon)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
      {
        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Modify label Placement 
        //Check if the label engine is Maplex or standard.
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        if (labelEngine is CIMStandardGeneralPlacementProperties)
        {
          //Current labeling engine is Standard Labeling engine
          theLabelClass.StandardLabelPlacementProperties.PolygonPlacementMethod = 
                   StandardPolygonPlacementMethod.AlwaysHorizontal;
          theLabelClass.StandardLabelPlacementProperties.PlaceOnlyInsidePolygon = true;
        }
        else
        {
          //Current labeling engine is Maplex labeling engine
          theLabelClass.MaplexLabelPlacementProperties.PolygonFeatureType = 
                                   MaplexPolygonFeatureType.LandParcel;
          theLabelClass.MaplexLabelPlacementProperties.AvoidPolygonHoles = true;
          theLabelClass.MaplexLabelPlacementProperties.PolygonPlacementMethod = 
                              MaplexPolygonPlacementMethod.HorizontalInPolygon;
          theLabelClass.MaplexLabelPlacementProperties.CanPlaceLabelOutsidePolygon = true;
        }

        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
                                             //set the label's visiblity
        featureLayer.SetLabelVisibility(true);
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.GraticuleAlignment
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.GraticuleAlignmentType
    // cref: ArcGIS.Core.CIM.MaplexGraticuleAlignmentType
    #region Modify Orientation of a label using the MaplexEngine - Points and Polygon geometry
    /// <summary>
    /// Modify Orientation of a label using the MaplexEngine - Points and Polygon geometry
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task ModifyLabelsOrientationPointPolygonAsync(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType == esriGeometryType.esriGeometryLine)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
     {
       //Check if the label engine is Maplex or standard.
       CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
       if (labelEngine is CIMStandardGeneralPlacementProperties)
         return;
       //Note: call within QueuedTask.Run()

       //Get the layer's definition
       var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
       //Get the label classes - we need the first one
       var listLabelClasses = lyrDefn.LabelClasses.ToList();
       var theLabelClass = listLabelClasses.FirstOrDefault();
       //Modify label Orientation                 
       theLabelClass.MaplexLabelPlacementProperties.GraticuleAlignment = true;
       theLabelClass.MaplexLabelPlacementProperties.GraticuleAlignmentType = MaplexGraticuleAlignmentType.Curved;

       lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
       featureLayer.SetDefinition(lyrDefn); //set the layer's definition       
     });
    }
    #endregion

    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.AlignLabelToLineDirection
    #region Modify Orientation of a label using the MaplexEngine - Line geometry
    /// <summary>
    /// Modify Orientation of a label using the MaplexEngine - Line geometry
    /// </summary>
    /// <param name="featureLayer"></param>

    public static Task ModifyLabelsOrientationLineAsync(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolyline)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
      {
        //Check if the label engine is Maplex or standard.
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        if (labelEngine is CIMStandardGeneralPlacementProperties)
          return;

        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Modify label Orientation
        theLabelClass.MaplexLabelPlacementProperties.AlignLabelToLineDirection = true;

        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMLabelClass.MaplexLabelPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties.Enable
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties.RotationField
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties.AdditionalAngle
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties.RotationType
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties.AlignmentType
    // cref: ArcGIS.Core.CIM.CIMMaplexRotationProperties.AlignLabelToAngle
    // cref: ArcGIS.Core.CIM.MaplexLabelRotationType
    // cref ArcGIS.Core.CIM.MaplexRotationAlignmentType
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.RotationProperties
    #region Modify label Rotation - Point geometry
    /// <summary>
    /// Modify label Rotation (Point geomerty)
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task ModifyLabelsRotationPointsAsync(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPoint)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
      {
        //Check if the label engine is Maplex or standard.
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        if (labelEngine is CIMStandardGeneralPlacementProperties)
          return;

        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Modify label Rotation
        CIMMaplexRotationProperties rotationProperties = new CIMMaplexRotationProperties
        {
          Enable = true, //Enable rotation
          RotationField = "ELEVATION", //Field that is used to define rotation angle
          AdditionalAngle = 15, //Additional rotation 
          RotationType = MaplexLabelRotationType.Arithmetic,
          AlignmentType = MaplexRotationAlignmentType.Perpendicular,
          AlignLabelToAngle = true
        };
        theLabelClass.MaplexLabelPlacementProperties.RotationProperties = rotationProperties;
        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
        
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMLabelClass.MaplexLabelPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.SpreadWords
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.SpreadCharacters
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.MaximumCharacterSpacing
    #region Spread labels across Polygon geometry
    /// <summary>
    /// Spread labels across Polygon geometry
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task ModifyLabelsSpreadLabelPolygon(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolygon)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
      {
        //Check if the label engine is Maplex or standard.
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        if (labelEngine is CIMStandardGeneralPlacementProperties)
          return;

        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //Spread Labels (words and characters to fill feature)
        // Spread words to fill feature
        theLabelClass.MaplexLabelPlacementProperties.SpreadWords = true;
        //Spread Characters to a fixed limit of 50%
        theLabelClass.MaplexLabelPlacementProperties.SpreadCharacters = true;
        theLabelClass.MaplexLabelPlacementProperties.MaximumCharacterSpacing = 50.0;
        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
      });
    }
    #endregion
    // cref: ArcGIS.Core.CIM.CIMLabelClass.MaplexLabelPlacementProperties
    // cref: ArcGIS.Core.CIM.CIMMaplexLabelPlacementProperties.PolygonAnchorPointType
    // cref: ArcGIS.Core.CIM.MaplexAnchorPointType
    #region Modify label's Leader Line Anchor point properties - Polygon geometry
    /// <summary>
    /// Modify label's Leader Line Anchor point properties - Polygon geometry
    /// </summary>
    /// <param name="featureLayer"></param>
    public static Task ModifyLabelsLeaderLineAnchorPolygon(FeatureLayer featureLayer)
    {
      if (featureLayer.ShapeType != esriGeometryType.esriGeometryPolygon)
        return Task.FromResult(0);
      return QueuedTask.Run(() =>
      {
        //Check if the label engine is Maplex or standard.
        CIMGeneralPlacementProperties labelEngine = MapView.Active.Map.GetDefinition().GeneralPlacementProperties;
        if (labelEngine is CIMStandardGeneralPlacementProperties)
          return;

        //Get the layer's definition
        var lyrDefn = featureLayer.GetDefinition() as CIMFeatureLayer;
        //Get the label classes - we need the first one
        var listLabelClasses = lyrDefn.LabelClasses.ToList();
        var theLabelClass = listLabelClasses.FirstOrDefault();
        //If TextSymbol is a callout the leader line anachor point can be modified
        theLabelClass.MaplexLabelPlacementProperties.PolygonAnchorPointType = MaplexAnchorPointType.Perimeter;
        lyrDefn.LabelClasses = listLabelClasses.ToArray(); //Set the labelClasses back
        featureLayer.SetDefinition(lyrDefn); //set the layer's definition
      });
    }
    #endregion
  }
}
