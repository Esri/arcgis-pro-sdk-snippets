using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides functionality for working with advanced Pro Snippets features in ArcGIS Pro.
  /// </summary>
  /// <remarks>This class contains methods and utilities for demonstrating and implementing advanced GIS-related
  /// operations, such as layer masking. It is designed to be used in the context of ArcGIS Pro SDK development and
  /// scripting.</remarks>
  public static class ProSnippetsFeature
  {
    #region ProSnippet Group: Features
    #endregion

    // cref: ArcGIS.Core.CIM.CIMBaselayer.LayerMasks
    #region Mask feature
    /// <summary>
    /// This method demonstrates how to apply masking to a feature layer in the active map view using a polygon layer as the mask.
    /// </summary>
    public static void Masking()
    {
      QueuedTask.Run(() =>
      {
        //Get the layer to be masked
        var lineLyrToBeMasked = MapView.Active.Map.Layers.FirstOrDefault(lyr => lyr.Name == "TestLine") as FeatureLayer;
        //Get the layer's definition
        var lyrDefn = lineLyrToBeMasked.GetDefinition();
        //Create an array of Masking layers (polygon only)
        //Set the LayerMasks property of the Masked layer
        lyrDefn.LayerMasks = new string[] { "CIMPATH=map3/testpoly.xml" };
        //Re-set the Masked layer's definition
        lineLyrToBeMasked.SetDefinition(lyrDefn);
      });
    }
    #endregion
  }
}
