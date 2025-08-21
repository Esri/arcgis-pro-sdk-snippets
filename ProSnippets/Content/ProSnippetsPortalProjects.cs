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
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Portal;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.ProSnippets
{
  public static class ProSnippetsPortalProjects
  {
    #region ProSnippet Group: Portal Projects
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.CanOpen(System.String,System.String@)
    //cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
    //cref: ArcGIS.Desktop.Core.Project
    //cref: ArcGIS.Desktop.Core.ArcGISPortalManager
    //cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortal(System.Uri)
    //cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername
    //cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetUserContentAsync
    #region Workflow to open an ArcGIS Pro project
    /// <summary>
    /// Opens an ArcGIS Pro project from a portal or local path. If the project is a portal project, ensures the active portal matches the project's portal.
    /// </summary>
    public static async void OpenPortalProject()
    {
      var projectPath = @"https://<userName>.<domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
      string docVer = string.Empty;

      // A portal project path looks like this:
      //@"https://<ServerName>.<Domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
      //A local project path looks like this:
      //@"C:\Users\<UserName>\Documents\ArcGIS\Projects\MyProject\MyProject.aprx";

      //Check if the project can be opened
      if (Project.CanOpen(projectPath, out docVer))
      {
        //Open the project
        await Project.OpenAsync(projectPath);
      }
      else //The project cannot be opened
      {
        //One possible reason: If the project is a portal project, the active portal must match the portal of the project
        //Check if this is a portal project
        bool isPortalProject = Uri.TryCreate(projectPath, UriKind.Absolute, out Uri uriResult)
             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (isPortalProject)
        {
          //Parse the project path to get the portal
          var uri = new Uri(projectPath);
          var portalUrlOfProjectToOpen = $"{uri.Scheme}://{uri.Host}/portal/";

          //Get the current active portal
          var activePortal = ArcGIS.Desktop.Core.ArcGISPortalManager.Current.GetActivePortal();
          //Compare to see if the active Portal is the same as the portal of the project
          bool isSamePortal = (activePortal != null && activePortal.PortalUri.ToString() == portalUrlOfProjectToOpen);
          if (!isSamePortal) //not the same. 
          {
            //Set new active portal to be the portal of the project
            //Find the portal to sign in with using its Uri...
            var projectPortal = ArcGISPortalManager.Current.GetPortal(new Uri(portalUrlOfProjectToOpen, UriKind.Absolute));
            await QueuedTask.Run(() =>
            {
              if (!projectPortal.IsSignedOn())
              {
                //Calling "SignIn" will trigger the OAuth popup if your credentials are
                //not cached (eg from a previous sign in in the session)
                if (projectPortal.SignIn().success)
                {
                  //Set this portal as my active portal
                  ArcGISPortalManager.Current.SetActivePortal(projectPortal);
                  return;
                }
              }
              //Set this portal as my active portal
              ArcGISPortalManager.Current.SetActivePortal(projectPortal);
            });
            //Now try opening the project again
            if (Project.CanOpen(projectPath, out docVer))
            {
              await Project.OpenAsync(projectPath);
            }
            else
            {
              System.Diagnostics.Debug.WriteLine("The project cannot be opened.");
            }
          }
          else //The portals are the same. So the problem could be something else - permissions, portal is down?
          {
            System.Diagnostics.Debug.WriteLine("The project cannot be opened.");
          }
        }
        else //Project is on disk and cannot be opened. 
        {
          System.Diagnostics.Debug.WriteLine("The project cannot be opened.");
        }
      }
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.IsPortalProject
    //cref: ArcGIS.Desktop.Core.Project.Current
    //cref: ArcGIS.Desktop.Core.Project
    //#region Determine if the project is a portal project from a project's path
    #region Determine if the project is a portal project from a project object
    /// <summary>
    /// Determines if the current ArcGIS Pro project is a portal project by analyzing its path.
    /// </summary>
    public static bool ProjectIsAPortalProjectUsingProjectPath()
    {
      string projectPath = Project.Current.Url;
      // A portal project path looks like this:
      //@"https://<ServerName>.<Domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
      //A local project path looks like this:
      //@"C:\Users\<UserName>\Documents\ArcGIS\Projects\MyProject\MyProject.aprx";
      bool isPortalProject = Uri.TryCreate(projectPath, UriKind.Absolute, out Uri uriResult)
     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
      if (isPortalProject)
      {
        System.Diagnostics.Debug.WriteLine("This is a portal project");
      }
      else
      {
        System.Diagnostics.Debug.WriteLine("This is not a portal project");
      }
      return isPortalProject;
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.IsPortalProject
    //cref: ArcGIS.Desktop.Core.Project.Current
    //cref: ArcGIS.Desktop.Core.Project
    #region Determine if the project is a portal project from a project object
    /// <summary>
    /// Determines if the current ArcGIS Pro project is a portal project by using the project object.
    /// </summary>
    public static bool ProjectIsAPortalProjectUsingProjectObject()
    {
      var isPortalProject = Project.Current.IsPortalProject;
      return isPortalProject;
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.CanOpen(System.String,System.String@)
    //cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
    //cref: ArcGIS.Desktop.Core.Project
    #region Get the portal from a portal project's path
    /// <summary>
    /// Retrieves the portal URL from the current ArcGIS Pro project's path and obtains the corresponding ArcGISPortal object.
    /// </summary>
    public static void GetThePortalFromAPortalProjectsPath()
    {
      string projectPath = Project.Current.Url;
      // A portal project path looks like this:
      //@"https://<ServerName>.<Domain>.com/portal/sharing/rest/content/items/1a434faebbe7424d9982f57d00223baa";
      //A local project path looks like this:
      //@"C:\Users\<UserName>\Documents\ArcGIS\Projects\MyProject\MyProject.aprx";

      //Check if the project is a portal project
      bool isPortalProject = Uri.TryCreate(projectPath, UriKind.Absolute, out Uri uriResult)
     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
      if (isPortalProject)
      {
        //Parse the project path to get the portal
        var uri = new Uri(projectPath);
        var fullUri = $"{uri.Scheme}://{uri.Host}/portal/";
        System.Diagnostics.Debug.WriteLine($"The Url of the project is: {fullUri}");
        //Now get the ArcGISPortal object from the portal Uri
        var arcgisPortal = ArcGISPortalManager.Current.GetPortal(new Uri(fullUri, UriKind.Absolute));
        System.Diagnostics.Debug.WriteLine($"The portal of the project is: {arcgisPortal.PortalUri}");
        //Note: You can set the active portal to be the portal of the project. Refer to this snippet: [ArcGISPortalManager: Get a portal and Sign In, Set it Active](ProSnippets-sharing#arcgisportalmanager-get-a-portal-and-sign-in-set-it-active)
      }
    }
    #endregion

    //cref: ArcGIS.Desktop.Catalog.OpenItemDialog
    //cref: ArcGIS.Desktop.Catalog.OpenItemDialog.ShowDialog
    //cref: ArcGIS.Desktop.Core.BrowseProjectFilter
    //cref: ArcGIS.Desktop.Core.BrowseProjectFilter.AddFilter(ArcGIS.Desktop.Core.BrowseProjectFilter)
    #region Workflow to open an ArcGIS Pro project using the OpenItemDialog
    /// <summary>
    /// Opens an ArcGIS Pro project using the OpenItemDialog, allowing selection from both portal and local projects.
    /// </summary>
    public static void OpenProjectsUsingOpenItemDlg()
    {
      BrowseProjectFilter portalAndLocalProjectsFilter = new BrowseProjectFilter();
      //A filter to pick projects from the portal
      //This filter will allow selection of ppkx and portal project items on the portal
      portalAndLocalProjectsFilter.AddFilter(BrowseProjectFilter.GetFilter("esri_browseDialogFilters_projects_online_proprojects"));
      //A filter to pick projects from the local machine
      portalAndLocalProjectsFilter.AddFilter(BrowseProjectFilter.GetFilter("esri_browseDialogFilters_projects"));
      //Create the OpenItemDialog and set the filter to the one we just created
      var openDlg = new OpenItemDialog()
      {
        Title = "Select a Project",
        MultiSelect = false,
        BrowseFilter = portalAndLocalProjectsFilter
      };
      //Show the dialog
      var result = openDlg.ShowDialog();
      //Check if the user clicked OK and selected an item
      bool? ok = openDlg.ShowDialog();
      if (!ok.HasValue || openDlg.Items.Count() == 0)
        return; //nothing selected
      var selectedItem = openDlg.Items.FirstOrDefault();
      //Open the project use the OpenAsync method.
    }
    #endregion


    //cref: ArcGIS.Desktop.Core.Project.CanOpen(System.String,System.String@)
    //cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
    //cref: ArcGIS.Desktop.Core.Project
    //cref: ArcGIS.Desktop.Core.ArcGISPortalManager
    //cref: ArcGIS.Desktop.Core.ArcGISPortalManager.GetPortal(System.Uri)
    //cref: ArcGIS.Desktop.Core.ArcGISPortal.GetSignOnUsername
    //cref: ArcGIS.Desktop.Core.ArcGISPortalExtensions.GetUserContentAsync
    #region Retrieve a project item from a portal and open it
    /// <summary>
    /// Retrieves a project item from a portal, checks if it can be opened, and opens it if possible.
    /// </summary>
    public static async void RetrieveProjectItemFromPortal()
    {
      var projectPortal = ArcGISPortalManager.Current.GetPortal(new Uri(@"https://<serverName>.<domain>.com/portal/", UriKind.Absolute));
      string owner = string.Empty;
      await QueuedTask.Run(() =>
      {
        //Get the signed on user name
        owner = projectPortal.GetSignOnUsername();
      });
      //Get the user content from the portal
      var userContent = await projectPortal.GetUserContentAsync(owner);
      //Get the first portal project item
      var firstPortalProject = userContent.PortalItems.FirstOrDefault(pi => pi.PortalItemType == PortalItemType.ProProject);
      var portalProjectUri = firstPortalProject.ItemUri.ToString();
      //Check if project can be opened
      string docVer = string.Empty;
      if (Project.CanOpen(portalProjectUri, out docVer))
      {
        await Project.OpenAsync(portalProjectUri);
      }
      //Note: If Project.CanOpen returns false, the project cannot be opened. One reason could be 
      // the active portal is not the same as the portal of the project. Refer to the snippet: [Workflow to open an ArcGIS Pro project](ProSnippets-sharing#workflow-to-open-an-arcgis-pro-project)
    }
    #endregion

    //cref: ArcGIS.Desktop.Core.Project.GetRecentProjectsEx
    #region Retrieve the list of recently opened projects
    /// <summary>
    /// Retrieves the list of recently opened ArcGIS Pro projects, including both local and portal projects.
    /// </summary>
    public static void GetRecentProject()
    {
      IReadOnlyList<Tuple<string, string>> result = [];
      //A list of Tuple instances containing two strings.
      //The first string: full path to the .aprx. In case of Portal projects, 
      //this is the cached location of the project on the local machine.
      //Second string:  url for portal projects
      result = Project.GetRecentProjectsEx();
      foreach (var project in result)
      {
        string projectPath;
        string projectName;
        string projectUrl;
        if (!string.IsNullOrEmpty(project.Item2))
        {
          //this is a portal project
          //Url
          projectUrl = project.Item2;
          //local cached location of the portal project
          projectPath = project.Item1;
        }
        else
        {
          //this is a local project
          //path to local project
          projectPath = project.Item1;
        }
        projectName = new FileInfo(project.Item1).Name;
      }
    }
    #endregion
  }
}
