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
		/// Masks a feature layer using a polygon layer.
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
        lyrDefn.LayerMasks = ["CIMPATH=map3/testpoly.xml"];
        //Re-set the Masked layer's definition
        lineLyrToBeMasked.SetDefinition(lyrDefn);
      });
    }
    #endregion
  }
}
