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
using ArcGIS.Desktop.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsGroundToGrid
  {
    #region ProSnippet Group: Ground to Grid
    #endregion


    // cref: ArcGIS.Core.CIM.CIMGroundToGridCorrection
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.IsCorrecting(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingDirectionOffset(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.GetDirectionOffset(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingDistanceFactor(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingElevationMode(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.UsingConstantScaleFactor(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    // cref: ArcGIS.Desktop.Editing.GroundToGridCorrection.GetConstantScaleFactor(ArcGIS.Core.CIM.CIMGroundToGridCorrection)
    #region G2G Settings
    /// <summary>
    /// Configures and evaluates the settings of a ground-to-grid correction.
    /// </summary>
    /// <remarks>This method evaluates various properties of the provided ground-to-grid correction, such as
    /// whether corrections are enabled, whether direction offsets or distance factors are being used, and retrieves
    /// associated values like direction offsets and scale factors. It is intended to help analyze and work with
    /// ground-to-grid correction configurations in editing workflows.</remarks>
    /// <param name="correction">The <see cref="CIMGroundToGridCorrection"/> object representing the ground-to-grid correction settings. This
    /// parameter must not be <see langword="null"/> and should be properly initialized.</param>
     public static void GroundToGridSettings(CIMGroundToGridCorrection correction)
    {
      bool isCorecting = correction.IsCorrecting();   // equivalent to correction != null && correction.Enabled;
      bool UsingOffset = correction.UsingDirectionOffset();   // equivalent to correction.IsCorrecting() && correction.UseDirection;
      double dOffset = correction.GetDirectionOffset(); // equivalent to correction.UsingDirectionOffset() ? correction.Direction : DefaultDirectionOffset;
      bool usingDistanceFactor = correction.UsingDistanceFactor();  // equivalent to correction.IsCorrecting() && correction.UseScale;
      bool usingElevation = correction.UsingElevationMode(); // equivalent to correction.UsingDistanceFactor() && c.ScaleType == GroundToGridScaleType.ComputeUsingElevation;
      bool usingSFactor = correction.UsingConstantScaleFactor();  //; equivalent to correction.UsingDistanceFactor() && correction.ScaleType == GroundToGridScaleType.ConstantFactor;
      double dSFactor = correction.GetConstantScaleFactor(); // equivalent to correctionc.UsingDistanceFactor() ? correction.ConstantScaleFactor : DefaultConstantScaleFactor;
    }
    #endregion

  }
}
