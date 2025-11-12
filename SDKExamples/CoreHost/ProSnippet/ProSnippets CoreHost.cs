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
using System;
using System.Collections.Generic;
using ArcGIS.Core.Data;
//There must be a reference to ArcGIS.CoreHost.dll
using ArcGIS.Core.Hosting;

namespace ProSnippets.CoreHostSnippets
{
  /// <summary>
  /// Provides the entry point for initializing the ArcGIS Core host environment and accessing geodatabase definitions.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with creating text symbols in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsCoreHost
  {
    // cref: ArcGIS.Core.Hosting.Host.Initialize()
    #region Initializing Core Host
    /// <summary>
    /// The main entry point for the application, responsible for initializing the ArcGIS Core Host.
    /// </summary>
    /// <remarks>This method must be marked with the <see cref="STAThreadAttribute"/> to ensure proper
    /// threading model. It initializes the ArcGIS Core Host, which is required before constructing any objects from the
    /// ArcGIS.Core namespace. If initialization fails, an error message is displayed and the application
    /// exits.</remarks>
    /// <param name="args">Command-line arguments passed to the application.</param>
    //[STAThread] must be present on the Application entry point
    [STAThread]
    static void Main(string[] args)
    {
      //Call Host.Initialize before constructing any objects from ArcGIS.Core
      try
      {
        Host.Initialize();
      }
      catch (Exception e)
      {
        // Error (missing installation, no license, 64 bit mismatch, etc.)
        Console.WriteLine(string.Format("Initialization failed: {0}", e.Message));
        return;
      }

      //if we are here, ArcGIS.Core is successfully initialized
      Geodatabase gdb = new(new FileGeodatabaseConnectionPath(new Uri(@"C:\Data\SDK\GDB\MySampleData.gdb")));
      IReadOnlyList<TableDefinition> definitions = gdb.GetDefinitions<FeatureClassDefinition>();

      foreach (var fdsDef in definitions)
      {
        Console.WriteLine(TableString(fdsDef as TableDefinition));
      }
      Console.Read();
    }

    private static string TableString(TableDefinition table)
    {
      string alias = table.GetAliasName();
      string name = table.GetName();
      return string.Format("{0} ({1})", alias.Length > 0 ? alias : name, name);
    }
  }
  #endregion
}
