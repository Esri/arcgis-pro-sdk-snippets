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
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Content.ProSnippets
{
  public static class ProSnippetsCatalogWindow
  {
    #region ProSnippet Group: Catalog Window
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.GetCatalogPane(System.Boolean)
    //cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Activate
    #region Set the catalog dockpane as the active window
    /// <summary>
    /// Sets the catalog dockpane as the active window in the current ArcGIS Pro session.
    /// </summary>
    public static void SetCatalogWindowActive()
    {
      //cast ICatalogWindow to ArcGIS.Desktop.Framework.Contracts.DockPane
      var catalogWindow = Project.GetCatalogPane() as DockPane;
      //Activate it
      catalogWindow.Activate();
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.ICatalogWindow.IsActiveWindow
    #region Check if the Catalog Window is the active window
    /// <summary>
    /// Checks if the Catalog Window is the active window in the current ArcGIS Pro session.
    /// </summary>
    public static void ContentSnippetCatalog2()
    {
      var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
      if (catalogWindow.IsActiveWindow)
      {
        //TODO - query/change catalog window content
      }
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.GetCatalogPane(System.Boolean)
    //cref: ArcGIS.Desktop.Core.ICatalogWindow
    //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentContentType
    //cref: ArcGIS.Desktop.Core.CatalogContentType
    #region Get the catalog content type currently being shown
    /// <summary>
    /// Checks if the Catalog Window is currently the active window in the ArcGIS Pro session.
    /// </summary>
    public static CatalogContentType GetCurrentCatalogContentType()
    {
      //Gets the Catalog pane
      var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
      var catContentType = catalogWindow.GetCurrentContentType();
      return catContentType;
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentContentType
    //cref: ArcGIS.Desktop.Core.CatalogContentType
    //cref: ArcGIS.Desktop.Core.ICatalogWindow.SetContentTypeAsync(ArcGIS.Desktop.Core.CatalogContentType)
    #region Set the catalog content type
    /// <summary>
    /// Changes the content type displayed in the Catalog Window to the next available tab.
    /// </summary>
    public static void ChangeCatalogContentType()
    {
      //Gets the Catalog pane
      var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
      if (!catalogWindow.IsActiveWindow)
        return; //catalog dockpane must be the active window

      //Change the content to whatever is the next tab
      var catContentType = (int)catalogWindow.GetCurrentContentType();
      catContentType++;

      if (catContentType > (int)CatalogContentType.Favorites)
        catContentType = (int)CatalogContentType.Project;

      //Must be on the UI - no QueuedTask!
      catalogWindow.SetContentTypeAsync((CatalogContentType)catContentType);
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentSecondaryPortalContentType
    //cref: ArcGIS.Desktop.Core.CatalogContentType
    //cref: ArcGIS.Desktop.Core.CatalogSecondaryPortalContentType
    #region Get the secondary portal catalog content
    /// <summary>
    /// Retrieves the current secondary portal content type displayed in the Catalog Window when the portal is the active content type.
    /// </summary>
    public static void GetCurrentSecondaryPortalContentType()
    {
      //Gets the Catalog pane
      var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
      var catContentType = catalogWindow.GetCurrentContentType();
      //Is Portal the content type?
      if (catContentType == CatalogContentType.Portal)
      {
        //check what is the portal content type being shown...
        var secondaryContentType =
          catalogWindow.GetCurrentSecondaryPortalContentType();
        //TODO use secondary portal content type...
      }
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentContentType
    //cref: ArcGIS.Desktop.Core.ICatalogWindow.GetCurrentSecondaryPortalContentType
    //cref: ArcGIS.Desktop.Core.CatalogContentType
    //cref: ArcGIS.Desktop.Core.CatalogSecondaryPortalContentType
    //cref: ArcGIS.Desktop.Core.ICatalogWindow.SetSecondaryPortalContentTypeAsync(ArcGIS.Desktop.Core.CatalogSecondaryPortalContentType)
    #region Set the secondary portal catalog content
    /// <summary>
    /// Changes the secondary portal content type displayed in the Catalog Window to the next available option.
    /// </summary>
    public static void ChangeSecondaryPortalContentType()
    {
      //Gets the Catalog pane
      var catalogWindow = Project.GetCatalogPane() as ICatalogWindow;
      if (!catalogWindow.IsActiveWindow)
        return; //catalog dockpane must be the active window

      var catContentType = catalogWindow.GetCurrentContentType();
      //Is portal content being shown?
      if (catContentType == CatalogContentType.Portal)
      {
        //check what is the portal content type being shown...
        var portalContentType = (int)catalogWindow.GetCurrentSecondaryPortalContentType();
        //advance to the next tab
        portalContentType++;
        if (portalContentType > (int)CatalogSecondaryPortalContentType.LivingAtlas)
          portalContentType = (int)CatalogSecondaryPortalContentType.UserContent;

        //set the secondary portal content type
        //Must be on the UI - no QueuedTask!
        catalogWindow.SetSecondaryPortalContentTypeAsync(
          (CatalogSecondaryPortalContentType)portalContentType);
      }
    }
    #endregion
  }
}
