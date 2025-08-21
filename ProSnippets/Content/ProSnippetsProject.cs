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
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.UnitFormats;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.GeoProcessing;
using ArcGIS.Desktop.Internal.Framework;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Content.ProSnippets
{
  #region ProSnippet Group: Project
  #endregion

  public static class ProSnippetsProject
  {
    // cref: ArcGIS.Desktop.Core.Project.CreateAsync()
    // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
    // cref: ArcGIS.Desktop.Core.CreateProjectSettings
    // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
    #region Create a new project
    /// <summary>
    /// Creates a new ArcGIS Pro project using the default project settings.
    /// </summary>
    public static async Task<Project> CreateNewProjectAsync()
    {
      //Create a new project using Pro's default settings
      var defaultProjectSettings = Project.GetDefaultProjectSettings();
      return await Project.CreateAsync(defaultProjectSettings);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.CreateAsync
    #region Create an empty project
    /// <summary>
    /// Creates an empty ArcGIS Pro project in the default folder with an automatically generated name.
    /// </summary>
    public static async Task<Project> CreateEmptyProjectAsync()
    {
      //Create an empty project. The project will be created in the default folder
      //It will be named MyProject1, MyProject2, or similar...
      return await Project.CreateAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
    // cref: ArcGIS.Desktop.Core.CreateProjectSettings
    #region Create a new project with specified name
    /// <summary>
    /// Creates a new ArcGIS Pro project with the specified name and default settings.
    /// </summary>
    public static async Task<Project> CreateProjectWithNameAsync(string projectName)
    {
      //Settings used to create a new project
      CreateProjectSettings projectSettings = new()
      {
        //Sets the name of the project that will be created
        // example: projectName =  @"C:\Data\MyProject1\MyProject1.aprx"
        Name = projectName
      };
      //Create the new project
      return await Project.CreateAsync(projectSettings);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
    // cref: ArcGIS.Desktop.Core.CreateProjectSettings
    // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
    #region Create new project using Pro's default settings
    /// <summary>
    /// Creates a new ArcGIS Pro project with the specified name and location.
    /// </summary>
    public static async Task<Project> CreateProjectUsingDefaultSettings()
    {
      //Get Pro's default project settings.
      var defaultProjectSettings = Project.GetDefaultProjectSettings();
      //Create a new project using the default project settings
      return await Project.CreateAsync(defaultProjectSettings);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
    // cref: ArcGIS.Desktop.Core.CreateProjectSettings
    #region New project using a custom template file
    /// <summary>
    /// Creates a new ArcGIS Pro project using a custom template, specifying the project name, location, and template path.
    /// </summary>
    public static async Task<Project> CreateProjectWithCustomTemplateAsync()
    {
      //Settings used to create a new project
      CreateProjectSettings projectSettings = new()
      {
        //Sets the project's name
        Name = "New Project",
        //Path where new project will be stored in
        LocationPath = @"C:\Data\NewProject",
        //Sets the project template that will be used to create the new project
        TemplatePath = @"C:\Data\MyProject1\CustomTemplate.aptx"
      };
      //Create the new project
      return await Project.CreateAsync(projectSettings);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
    // cref: ArcGIS.Desktop.Core.CreateProjectSettings
    // cref: ArcGIS.Desktop.Core.TemplateType
    #region Create a project using template available with ArcGIS Pro
    /// <summary>
    /// Creates a new ArcGIS Pro project using a predefined template type, specifying the project name and location.
    /// </summary>
    public static async Task<Project> CreateProjectWithCustomTemplateTypeAsync()
    {
      //Settings used to create a new project
      CreateProjectSettings proTemplateSettings = new()
      {
        //Sets the project's name
        Name = "New Project",
        //Path where new project will be stored in
        LocationPath = @"C:\Data\NewProject",
        //Select which Pro template you like to use
        TemplateType = TemplateType.Catalog
        //TemplateType = TemplateType.LocalScene
        //TemplateType = TemplateType.GlobalScene
        //TemplateType = TemplateType.Map
      };
      //Create the new project
      return await Project.CreateAsync(proTemplateSettings);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
    #region Open an existing project
    /// <summary>
    /// Opens an existing ArcGIS Pro project or project package from the specified file path.
    /// </summary>
    public static async Task<Project> OpenExistingProjectAsync(string projectPath)
    {
      //Opens an existing project or project package
      // example: @"C:\Data\MyProject1\MyProject1.aprx"
      return await Project.OpenAsync(projectPath);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.Current 
    #region Get the Current project
    /// <summary>
    /// Retrieves the current ArcGIS Pro project instance.
    /// </summary>
    public static Project GetCurrentProject()
    {
      //Gets the current project
      var project = Project.Current;
      return project;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.Current
    // cref: ArcGIS.Desktop.Core.Project.URI
    #region Get location of current project
    /// <summary>
    /// Retrieves the file path of the current ArcGIS Pro project file (*.aprx).
    /// </summary>
    public static string GetCurrentProjectPath()
    {
      //Gets the location of the current project; that is, the path to the current project file (*.aprx)  
      string projectPath = Project.Current.URI;
      return projectPath;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.Current
    // cref: ArcGIS.Desktop.Core.Project.DefaultGeodatabasePath
    #region Get the project's default gdb path
    /// <summary>
    /// Retrieves the file path of the default geodatabase for the current ArcGIS Pro project.
    /// </summary>
    public static string GetDefaultGeodatabasePath()
    {
      var projGDBPath = Project.Current.DefaultGeodatabasePath;
      return projGDBPath;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.SetDefaultGeoDatabasePath(System.String)
    // cref: ArcGIS.Desktop.Core.Project.RemoveItem(ArcGIS.Desktop.Core.IProjectItem)
    // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
    #region Change the Project's default gdb path
    /// <summary>
    /// Changes the default geodatabase for the current ArcGIS Pro project by adding a new geodatabase and removing the old one.
    /// </summary>
    public static async Task<bool> ChangeDefaultGeodatabasePathAsync(string oldGDBItemPath, string newGDDItemPath)
    {
      // example: newGDDItemPath = @"C:\path\ArcGIS\Project\NewLocation.gdb";
      // example: string oldGDBItemPath = @"C:\Path\Project\OldLocation.gdb";

      //Create a new GDB item and add it to the project
      if (ItemFactory.Instance.Create(newGDDItemPath) is not IProjectItem newGDBItem)
        return false;
      var success = Project.Current.AddItem(newGDBItem);
      //make the newly added GDB item the default
      if (success)
        Project.Current.SetDefaultGeoDatabasePath(newGDDItemPath);
      //Now remove the old item
      if (Project.Current.GetItems<Item>().FirstOrDefault(i => i.Path == oldGDBItemPath) is not IProjectItem oldGDBItem)
        return false;
      var removeSuccess = Project.Current.RemoveItem(oldGDBItem);
      #endregion

      // cref: ArcGIS.Desktop.Core.Project.SaveAsync
      #region Save project
      //Saves the project
      return await Project.Current.SaveAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.Current
    // cref: ArcGIS.Desktop.Core.Project.IsDirty
    #region Check if project needs to be saved
    /// <summary>
    /// Determines whether the current ArcGIS Pro project has unsaved changes.
    /// </summary>
    public static bool IsProjectDirty()
    {
      //The project's dirty state indicates changes made to the project have not yet been saved. 
      bool isProjectDirty = Project.Current.IsDirty;
      return isProjectDirty;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.SaveAsAsync
    #region SaveAs project
    /// <summary>
    /// Saves a copy of the current ArcGIS Pro project to the specified file path and opens the new project file.
    /// </summary>
    public static async Task<bool> SaveProjectAsAsync(string newProjectPath)
    {
      //Saves a copy of the current project file (*.aprx) to the specified location with the specified file name, 
      //then opens the new project file
      // example: newProjectPath = @"C:\Data\MyProject1\MyProject1_New.aprx";
      return await Project.Current.SaveAsAsync(newProjectPath);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.OpenAsync(System.String)
    #region Close a project
    //A project cannot be closed using the ArcGIS Pro API. 
    //A project is only closed when another project is opened, a new one is created, or the application is shutdown.
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapFactory.CreateMap(System.String,ArcGIS.Core.CIM.MapType,ArcGIS.Core.CIM.MapViewingMode,ArcGIS.Desktop.Mapping.Basemap)
    #region How to add a new map to a project
    /// <summary>
    /// Adds a new map to the current ArcGIS Pro project and opens it in a map pane.
    /// </summary>
    public static async Task AddNewMapToProject()
    {
      await QueuedTask.Run(() =>
      {
        //Note: see also MapFactory in ArcGIS.Desktop.Mapping
        var map = MapFactory.Instance.CreateMap("New Map", MapType.Map, MapViewingMode.Map, Basemap.Oceans);
        ProApp.Panes.CreateMapPaneAsync(map);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.GetRecentProjects()
    #region Get Recent Projects
    /// <summary>
    /// Retrieves a list of file paths for recently opened ArcGIS Pro projects.
    /// </summary>
    public static IReadOnlyList<string> GetRecentProjects()
    {
      var recentProjects = ArcGIS.Desktop.Core.Project.GetRecentProjects();
      return recentProjects;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.ClearRecentProjects()
    #region Clear Recent Projects
    /// <summary>
    /// Clears the list of recently opened ArcGIS Pro projects.
    /// </summary>
    public static void ClearRecentProjects()
    {
      ArcGIS.Desktop.Core.Project.ClearRecentProjects();
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.RemoveRecentProject(string)
    #region Remove a Recent Project
    /// <summary>
    /// Removes the specified project from the list of recently opened ArcGIS Pro projects.
    /// </summary>
    public static void RemoveRecentProject(string projectPath)
    {
      ArcGIS.Desktop.Core.Project.RemoveRecentProject(projectPath);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.GetPinnedProjects()
    #region Get Pinned Projects
    /// <summary>
    /// Retrieves a list of file paths for pinned ArcGIS Pro projects.
    /// </summary>
    public static IReadOnlyList<string> GetPinnedProjects()
    {
      var pinnedProjects = ArcGIS.Desktop.Core.Project.GetPinnedProjects();
      return pinnedProjects;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.ClearPinnedProjects()
    #region Clear Pinned Projects
    /// <summary>
    /// Clears the list of pinned ArcGIS Pro projects.
    /// </summary>
    public static void ClearPinnedProjects()
    {
      ArcGIS.Desktop.Core.Project.ClearPinnedProjects();
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.PinProject(string)
    // cref: ArcGIS.Desktop.Core.Project.UnpinProject(string)
    #region Pin / UnPin Projects
    /// <summary>
    /// Pins the specified project to the list of pinned projects and unpins the specified project from the list.
    /// </summary>
    public static void PinUnpinProjects(string pinThisProjectPath, string unpinThisProjectPath)
    {
      ArcGIS.Desktop.Core.Project.PinProject(pinThisProjectPath);
      ArcGIS.Desktop.Core.Project.UnpinProject(unpinThisProjectPath);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.GetRecentProjectTemplates()
    #region Get Recent Project Templates
    /// <summary>
    /// Retrieves a list of file paths for recently used ArcGIS Pro project templates.
    /// </summary>
    public static IReadOnlyList<string> GetRecentProjectTemplates()
    {
      var recentTemplates = ArcGIS.Desktop.Core.Project.GetRecentProjectTemplates();
      return recentTemplates;
    }

    // cref: ArcGIS.Desktop.Core.Project.ClearRecentProjectTemplates()
    #region Clear Recent Project Templates
    /// <summary>
    /// Clears the list of recently used ArcGIS Pro project templates.
    /// </summary>
    public static void ClearRecentProjectTemplates()
    {
      ArcGIS.Desktop.Core.Project.ClearRecentProjectTemplates();
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.RemoveRecentProjectTemplate(string)
    #region Remove a Recent Project Template
    /// <summary>
    /// Removes the specified project template from the list of recently used ArcGIS Pro project templates.
    /// </summary>
    public static void RemoveRecentProjectTemplate(string templatePath)
    {
      // example: templatePath = @"C:\Data\MyProject1\CustomTemplate.aptx";
      ArcGIS.Desktop.Core.Project.RemoveRecentProjectTemplate(templatePath);
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.GetPinnedProjectTemplates()
    #region Get Pinned Project Templates
    /// <summary>
    /// Retrieves a list of file paths for pinned ArcGIS Pro project templates.
    /// </summary>
    public static IReadOnlyList<string> GetPinnedProjectTemplates()
    {
      var pinnedTemplates = ArcGIS.Desktop.Core.Project.GetPinnedProjectTemplates();
      return pinnedTemplates;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.ClearPinnedProjectTemplates()
    #region Clear Pinned Project Templates
    /// <summary>
    /// Clears the list of pinned ArcGIS Pro project templates.
    /// </summary>
    public static void ClearPinnedProjectTemplates()
    {
      ArcGIS.Desktop.Core.Project.ClearPinnedProjectTemplates();
      #endregion
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.PinProjectTemplate(string)
    // cref: ArcGIS.Desktop.Core.Project.UnpinTemplateProject(string)
    #region Pin / UnPin Project Templates
    /// <summary>
    /// Pins the specified project template to the list of pinned templates and unpins the specified template from the list.
    /// </summary>
    public static void PinUnpinProjectTemplates(string templatePath, string templatePath2)
    {
      // example: templatePath = @"C:\Data\MyProject1\CustomTemplate.aptx";
      // example: templatePath2 = @"C:\Data\MyProject1\CustomTemplate2.aptx";
      ArcGIS.Desktop.Core.Project.PinProjectTemplate(templatePath);
      ArcGIS.Desktop.Core.Project.UnpinTemplateProject(templatePath2);
    }
    #endregion

    #region ProSnippet Group: Project Items
    #endregion

    // cref: ArcGIS.Desktop.Core.ItemFactory
    // cref: ArcGIS.Desktop.Core.IItemFactory.Create
    // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
    #region Add a folder connection item to the current project
    /// <summary>
    /// Adds a folder connection to the current ArcGIS Pro project using the specified folder path.
    /// </summary>
    public static async Task AddFolderConnectionToCurrentProject(string folderPath)
    {
      //Adding a folder connection
      // example: folderPath = "@C:\\myDataFolder";
      var folder = await QueuedTask.Run(() =>
      {
        //Create the folder connection project item
        var item = ItemFactory.Instance.Create(folderPath) as IProjectItem;
        //If it is successfully added to the project, return it otherwise null
        return Project.Current.AddItem(item) ? item as FolderConnectionProjectItem : null;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ItemFactory
    // cref: ArcGIS.Desktop.Core.IItemFactory.Create
    // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
    #region Add a geodatabase item to the current project
    /// <summary>
    /// Adds a geodatabase to the current ArcGIS Pro project using the specified geodatabase path.
    /// </summary>
    public static async Task AddGeodatabaseToCurrentProject(string gdbPath)
    {
      //Adding a Geodatabase:
      // example: gdbPath = "@C:\\myDataFolder\\myData.gdb";
      var newlyAddedGDB = await QueuedTask.Run(() =>
      {
        //Create the File GDB project item
        var item = ItemFactory.Instance.Create(gdbPath) as IProjectItem;
        //If it is successfully added to the project, return it otherwise null
        return Project.Current.AddItem(item) ? item as GDBProjectItem : null;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all project items
    /// <summary>
    /// Retrieves all project items from the current ArcGIS Pro project.
    /// </summary>
    public static void GetAllProjectItemsAsync()
    {
      IEnumerable<Item> allProjectItems = Project.Current.GetItems<Item>();
      foreach (var pi in allProjectItems)
      {
        //Do Something 
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all "MapProjectItems" for a project
    /// <summary>
    /// Retrieves all items from the current ArcGIS Pro project.
    /// </summary>
    public static async Task GetAllMapProjectItemsAsync(Project project)
    {
      IEnumerable<MapProjectItem> newMapItemsContainer = project.GetItems<MapProjectItem>();
      await QueuedTask.Run(() =>
      {
        foreach (var mp in newMapItemsContainer)
        {
          //Do Something with the map. For Example:
          Map myMap = mp.GetMap();
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.MapProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get a specific "MapProjectItem"
    /// <summary>
    /// Retrieves a specific map project item from the current ArcGIS Pro project by its name.
    /// </summary>
    public static MapProjectItem GetSpecificMapProjectItemAsync()
    {
      MapProjectItem mapProjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(item => item.Name.Equals("EuropeMap"));
      return mapProjItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all "StyleProjectItems"
    /// <summary>
    /// Retrieves all style project items from the current ArcGIS Pro project.
    /// </summary>
    public static void GetAllStyleProjectItemsAsync()
    {
      IEnumerable<StyleProjectItem> newStyleItemsContainer = null;
      newStyleItemsContainer = Project.Current.GetItems<StyleProjectItem>();
      foreach (var styleItem in newStyleItemsContainer)
      {
        //Do Something with the style.
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get a specific "StyleProjectItem"
    /// <summary>
    /// Retrieves a specific style project item and its associated style item from the current ArcGIS Pro project 
    /// based on the provided style name and symbol name.
    /// </summary>
    public static (StyleProjectItem, StyleItem) GetSpecificStyleProjectItemAsync(string projectStyleName, string symbolName)
    {
      var container = Project.Current.GetItems<StyleProjectItem>();
      // example: projectStyleName = "ArcGIS 3D";
      StyleProjectItem testStyle = container.FirstOrDefault(style => (style.Name == projectStyleName));
      StyleItem cone = null;
      if (testStyle != null)
      {
        // example: symbolName = "Cone_Volume_3";
        cone = testStyle.LookupItem(StyleItemType.PointSymbol, symbolName);
      }
      return (testStyle, cone);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get the "Favorite" StyleProjectItem
    /// <summary>
    /// Retrieves the favorite style project item from the current ArcGIS Pro project.
    /// </summary>
    public static async Task<StyleProjectItem> GetFavoriteStyleProjectItemAsync()
    {
      var fav_style_item = await QueuedTask.Run(() =>
      {
        var containerStyle = Project.Current.GetProjectItemContainer("Style");
        return containerStyle.GetItems().OfType<StyleProjectItem>().First(item => item.TypeID == "personal_style");
      });
      return fav_style_item;
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all "GDBProjectItems"
    /// <summary>
    /// Retrieves all geodatabase project items from the current ArcGIS Pro project.
    /// </summary>
    public static void GetAllGDBProjectItemsAsync()
    {
      IEnumerable<GDBProjectItem> newGDBItemsContainer = null;
      newGDBItemsContainer = Project.Current.GetItems<GDBProjectItem>();
      foreach (var GDBItem in newGDBItemsContainer)
      {
        //Do Something with the GDB.
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.GDBProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get a specific "GDBProjectItem"
    /// <summary>
    /// Retrieves a specific geodatabase project item from the current ArcGIS Pro project by its name.
    /// </summary>
    public static GDBProjectItem GetSpecificGDBProjectItemAsync()
    {
      // example: "myGDB" is the name of the GDBProjectItem
      GDBProjectItem gdbProjItem = Project.Current.GetItems<GDBProjectItem>().FirstOrDefault(item => item.Name.Equals("myGDB"));
      return gdbProjItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all "ServerConnectionProjectItems"
    /// <summary>
    /// Retrieves all server connection project items from the specified ArcGIS Pro project.
    /// </summary>
    public static void GetAllServerConnectionProjectItemsAsync(Project project)
    {
      IEnumerable<ServerConnectionProjectItem> newServerConnections = null;
      newServerConnections = project.GetItems<ServerConnectionProjectItem>();
      foreach (var serverItem in newServerConnections)
      {
        //Do Something with the server connection.
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.ServerConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get a specific "ServerConnectionProjectItem"
    /// <summary>
    /// Retrieves a specific server connection project item from the current ArcGIS Pro project by its name.
    /// </summary>
    public static ServerConnectionProjectItem GetSpecificServerConnectionProjectItemAsync()
    {
      // example: "myServer" is the name of the ServerConnectionProjectItem
      ServerConnectionProjectItem serverProjItem = Project.Current.GetItems<ServerConnectionProjectItem>().FirstOrDefault(item => item.Name.Equals("myServer"));
      return serverProjItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all folder connections in a project
    /// <summary>
    /// Retrieves all folder connection project items from the current ArcGIS Pro project.
    /// </summary>
    public static void GetAllFolderConnectionsAsync()
    {
      //Gets all the folder connections in the current project
      var projectFolders = Project.Current.GetItems<FolderConnectionProjectItem>();
      foreach (var FolderItem in projectFolders)
      {
        //Do Something with the Folder connection.
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get a specific folder connection
    /// <summary>
    /// Retrieves a specific folder connection project item from the current ArcGIS Pro project by its name.
    /// </summary>
    public static FolderConnectionProjectItem GetSpecificFolderConnectionAsync()
    {
      // example: "myDataFolder" is the name of the FolderConnectionProjectItem
      FolderConnectionProjectItem folderProjItem = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(item => item.Name.Equals("myDataFolder"));
      return folderProjItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.RemoveItem
    #region Remove a specific folder connection
    /// <summary>
    /// Removes a specific folder connection from the current ArcGIS Pro project without deleting the folder from the disk or network.
    /// </summary>
    public static void RemoveFolderConnectionAsync()
    {
      // Remove a folder connection from a project; the folder stored on the local disk or the network is not deleted
      FolderConnectionProjectItem folderToRemove = Project.Current.GetItems<FolderConnectionProjectItem>().FirstOrDefault(myfolder => myfolder.Name.Equals("PlantSpecies"));
      if (folderToRemove != null)
        Project.Current.RemoveItem(folderToRemove as IProjectItem);
    }
    #endregion

    // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Gets a specific "LayoutProjectItem"
    /// <summary>
    /// Retrieves a specific layout project item from the current ArcGIS Pro project by its name.
    /// </summary>
    public static LayoutProjectItem GetSpecificLayoutProjectItemAsync()
    {
      LayoutProjectItem layoutProjItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals("myLayout"));
      return layoutProjItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all layouts in a project
    /// <summary>
    /// Retrieves all layout project items from the current ArcGIS Pro project.
    /// </summary>
    public static void GetAllLayoutProjectItemsAsync()
    {
      //Gets all the layouts in the current project
      IEnumerable<LayoutProjectItem> projectLayouts = Project.Current.GetItems<LayoutProjectItem>();
      foreach (var layoutItem in projectLayouts)
      {
        //Do Something with the layout
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.GeoProcessing.GeoprocessingProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get a specific "GeoprocessingProjectItem"
    /// <summary>
    /// Retrieves a specific geoprocessing project item from the current ArcGIS Pro project by its name.
    /// </summary>
    public static GeoprocessingProjectItem GetSpecificGeoprocessingProjectItemAsync()
    {
      //Gets a specific GeoprocessingProjectItem in the current project
      // example: "myToolbox" is the name of the GeoprocessingProjectItem
      GeoprocessingProjectItem gpProjItem = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(item => item.Name.Equals("myToolbox"));
      return gpProjItem;
    }
    #endregion

    // cref: ArcGIS.Desktop.GeoProcessing.GeoprocessingProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Get all GeoprocessingProjectItems in a project
    /// <summary>
    /// Retrieves all geoprocessing project items from the current ArcGIS Pro project.
    /// </summary>
    public static void GetAllGeoprocessingProjectItemsAsync()
    {
      //Gets all the GeoprocessingProjectItem in the current project
      var GPItems = Project.Current.GetItems<GeoprocessingProjectItem>();
      foreach (var tbx in GPItems)
      {
        //Do Something with the toolbox
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    #region Search project for a specific item
    /// <summary>
    /// Searches the current ArcGIS Pro project for items in folder connections and retrieves matching items.
    /// </summary>
    public static void SearchProjectForItemAsync()
    {
      List<Item> _mxd = [];
      //Gets all the folder connections in the current project
      var allFoldersItem = Project.Current.GetItems<FolderConnectionProjectItem>();
      if (allFoldersItem != null)
      {
        //iterate through all the FolderConnectionProjectItems found
        foreach (var folderItem in allFoldersItem)
        {
          //Search for mxd files in that folder connection and add it to the List<T>
          //Note:ArcGIS Pro automatically creates and dynamically updates a searchable index as you build and work with projects. 
          //Items are indexed when they are added to a project.
          //The first time a folder or database is indexed, indexing may take a while if it contains a large number of items. 
          //While the index is being created, searches will not return any results.
          _mxd.AddRange(folderItem.GetItems());
        }
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.CreateProjectSettings.LocationPath
    // cref: ArcGIS.Desktop.Core.Project.GetDefaultProjectSettings
    #region Get the Default Project Folder
    /// <summary>
    /// Retrieves the default folder path for saving ArcGIS Pro projects.
    /// </summary>
    public static string GetDefaultProjectFolder()
    {
      //Get Pro's default project settings.
      var defaultSettings = Project.GetDefaultProjectSettings();
      var defaultProjectPath = defaultSettings.LocationPath;
      // If not set, projects are saved in the user's My Documents\ArcGIS\Projects folder;
      // this folder is created if it doesn't already exist.
      defaultProjectPath ??= System.IO.Path.Combine(
                  System.Environment.GetFolderPath(
                       Environment.SpecialFolder.MyDocuments),
                  @"ArcGIS\Projects");
      return defaultProjectPath;
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Core.Item.Refresh
    #region Refresh the child item for a folder connection Item
    /// <summary>
    /// Refreshes the child items of a folder connection in the current ArcGIS Pro project.
    /// </summary>
    public static void RefreshFolderConnectionItem()
    {
      var contentItem = Project.Current.GetItems<FolderConnectionProjectItem>().First();
      //var contentItem = ...
      //Check if the MCT is required for Refresh()
      if (contentItem.IsMainThreadRequired)
      {
        //QueuedTask.Run must be used if item.IsMainThreadRequired
        //returns true
        QueuedTask.Run(() => contentItem.Refresh());
      }
      else
      {
        //if item.IsMainThreadRequired returns false, any
        //thread can be used to invoke Refresh(), though
        //BackgroundTask is preferred.
        contentItem.Refresh();

        //Or, via BackgroundTask
        ArcGIS.Core.Threading.Tasks.BackgroundTask.Run(() =>
          contentItem.Refresh(), ArcGIS.Core.Threading.Tasks.BackgroundProgressor.None);
      }
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.Item.ItemCategories
    #region Get Item Categories
    /// <summary>
    /// Retrieves the item categories associated with a geodatabase and filters its contents using those categories.
    /// </summary>
    public static void GetItemCategories()
    {
      // Get the ItemCategories with which an item is associated
      Item gdb = ItemFactory.Instance.Create(@"E:\CurrentProject\RegionalPolling\polldata.gdb");
      List<ItemCategory> gdbItemCategories = gdb.ItemCategories;
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemCategory.Items(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Core.Item})
      #region Using Item Categories
      // Browse items using an ItemCategory as a filter
      IEnumerable<Item> gdbContents = gdb.GetItems();
      IEnumerable<Item> filteredGDBContents1 = gdbContents.Where(item => item.ItemCategories.OfType<ItemCategoryDataSet>().Any());
      IEnumerable<Item> filteredGDBContents2 = new ItemCategoryDataSet().Items(gdbContents);
      #endregion

    }

    // cref: ArcGIS.Desktop.Core.Project.CreateAsync(ArcGIS.Desktop.Core.CreateProjectSettings)
    // cref: ArcGIS.Desktop.Core.CreateProjectSettings
    #region Create Project with Template
    /// <summary>
    /// Creates a new ArcGIS Pro project using a specified template, project name, and location.
    /// </summary>
    public static async Task<Project> CreateProjectWithTemplate()
    {

      var projectFolder = System.IO.Path.Combine(
          System.Environment.GetFolderPath(
              Environment.SpecialFolder.MyDocuments),
          @"ArcGIS\Projects");
      CreateProjectSettings ps = new()
      {
        Name = "MyProject",
        LocationPath = projectFolder,
        TemplatePath = @"C:\data\my_templates\custom_template.aptx"
      };
      var project = await Project.CreateAsync(ps);
      return project;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.Project.ProjectItemContainers
    // cref: ArcGIS.Desktop.Core.Project.GetProjectItemContainer(System.String)
    #region Select project containers - for use with SelectItemAsync
    /// <summary>
    /// Retrieves various project item containers from the current ArcGIS Pro project, such as folder connections, geodatabases, maps, layouts, and toolboxes.
    /// </summary>
    public static void ItemFindAndSelection()
    {
      //Use Project.Current.ProjectItemContainers
      var folderContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "FolderConnection");
      var gdbContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "GDB");
      var mapContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "Map");
      var layoutContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "Layout");
      var toolboxContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "GP");
      //etc.

      //or...use Project.Current.GetProjectItemContainer

      folderContainer = Project.Current.GetProjectItemContainer("FolderConnection");
      gdbContainer = Project.Current.GetProjectItemContainer("GDB");
      mapContainer = Project.Current.GetProjectItemContainer("Map");
      layoutContainer = Project.Current.GetProjectItemContainer("Layout");
      toolboxContainer = Project.Current.GetProjectItemContainer("GP");
      //etc.
    }
    #endregion

    // cref: ArcGIS.Desktop.Catalog.FolderConnectionProjectItem
    // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
    // cref: ArcGIS.Desktop.Mapping.MapProjectItem
    // cref: ArcGIS.Desktop.Mapping.StyleProjectItem
    // cref: ArcGIS.Desktop.Core.Project.FindItem(System.String)
    #region ProjectItem: Get an Item or Find an Item
    /// <summary>
    /// Retrieves various project items from the current ArcGIS Pro project, such as maps, layouts, folder connections, styles, and specific items by catalog path.
    /// </summary>
    public static void ProjectItemGet()
    {
      //GetItems searches project content
      var map = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name == "Map1");
      var layout = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(m => m.Name == "Layout1");
      var folders = Project.Current.GetItems<FolderConnectionProjectItem>();
      var style = Project.Current.GetItems<StyleProjectItem>().FirstOrDefault(s => s.Name == "ArcGIS 3D");

      //Find item uses a catalog path. The path can be to a file or dataset
      var fcPath = @"C:\Pro\CommunitySampleData\Interacting with Maps\Interacting with Maps.gdb\Crimes";
      var pdfPath = @"C:\Temp\Layout1.pdf";
      var imgPath = @"C:\Temp\AddinDesktop16.png";

      var fc = Project.Current.FindItem(fcPath);
      var pdf = Project.Current.FindItem(pdfPath);
      var img = Project.Current.FindItem(imgPath);
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
    // cref: ArcGIS.Desktop.Core.Project.GetCatalogPane
    // cref: ArcGIS.Desktop.Core.IProjectWindow
    // cref: ArcGIS.Desktop.Core.Project.ProjectItemContainers
    // cref: ArcGIS.Desktop.Core.IProjectWindow.SelectItemAsync
    #region Select an item in the Catalog pane
    /// <summary>
    /// Selects specific items, such as toolboxes or maps, in the Catalog pane of the current ArcGIS Pro project.
    /// </summary>
    public static void SelectItemInCatalogPane()
    {
      //Get the catalog pane
      ArcGIS.Desktop.Core.IProjectWindow projectWindow = Project.GetCatalogPane();
      //or get the active catalog view...
      //ArcGIS.Desktop.Core.IProjectWindow projectWindow = Project.GetActiveCatalogWindow();

      //eg Find a toolbox in the project
      string gpName = "Interacting with Maps.tbx";
      var toolbox = Project.Current.GetItems<GeoprocessingProjectItem>().FirstOrDefault(tbx => tbx.Name == gpName);
      //Select it under Toolboxes
      projectWindow.SelectItemAsync(toolbox, true, true, null);//null selects it in the first container - optionally await
                                                               //Note: Project.Current.GetProjectItemContainer("GP") would get toolbox container...

      //assume toolbox is also under Folders container. Select it under Folders instead of Toolboxes
      var foldersContainer = Project.Current.ProjectItemContainers.First(c => c.Path == "FolderConnection");
      //We must specify the container because Folders comes second (after Toolboxes)
      projectWindow.SelectItemAsync(toolbox, true, true, foldersContainer);//optionally await

      //Find a map and select it
      var mapItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(m => m.Name == "Map");
      //Map only occurs under "Maps" so the container need not be specified
      projectWindow.SelectItemAsync(mapItem, true, false, null);
    }
    #endregion


    #region ProSnippet Group: Project Units
    #endregion

    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetPredefinedProjectUnitFormats
    #region Get The Full List of All Available Unit Formats
    /// <summary>
    /// Retrieves and logs the full list of all available unit formats in the current ArcGIS Pro project.
    /// </summary>
    public static async Task GetProjectUnitsFormats()
    {
      await QueuedTask.Run(() =>
      {
        //Must be on the QueuedTask.Run()
        var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                        .OfType<UnitFormatType>().ToList();
        System.Diagnostics.Debug.WriteLine("All available units\r\n");

        foreach (var unit_format in unit_formats)
        {
          var units = DisplayUnitFormats.Instance.GetPredefinedProjectUnitFormats(unit_format);
          System.Diagnostics.Debug.WriteLine(unit_format.ToString());

          foreach (var display_unit_format in units)
          {
            var line = $"{display_unit_format.DisplayName}, {display_unit_format.UnitCode}";
            System.Diagnostics.Debug.WriteLine(line);
          }
          System.Diagnostics.Debug.WriteLine("");
        }
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
    #region Get The List of Unit Formats for the Current Project
    /// <summary>
    /// Retrieves and logs the list of unit formats configured for the current ArcGIS Pro project.
    /// </summary>
    public static async Task GetCurrentProjectUnitsFormatsAsync()
    {
      await QueuedTask.Run(() =>
      {
        //Must be on the QueuedTask.Run()
        var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                          .OfType<UnitFormatType>().ToList();
        System.Diagnostics.Debug.WriteLine("Project units\r\n");
        foreach (var unit_format in unit_formats)
        {
          var units = DisplayUnitFormats.Instance.GetProjectUnitFormats(unit_format);
          System.Diagnostics.Debug.WriteLine(unit_format.ToString());

          foreach (var display_unit_format in units)
          {
            var line = $"{display_unit_format.DisplayName}, {display_unit_format.UnitCode}";
            System.Diagnostics.Debug.WriteLine(line);
          }
          System.Diagnostics.Debug.WriteLine("");
        }
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
    #region Get A Specific List of Unit Formats for the Current Project
    /// <summary>
    /// Retrieves a list of distance unit formats configured for the current ArcGIS Pro project.
    /// </summary>
    public static IList<DisplayUnitFormat> GetListofUnitFormatsFromCurrentProject()
    {
      //Must be on the QueuedTask.Run()

      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location, UnitFormatType.Page
      //UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
      var units = DisplayUnitFormats.Instance.GetProjectUnitFormats(UnitFormatType.Distance);
      return units;
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
    #region Get The List of Default Formats for the Current Project
    /// <summary>
    /// Retrieves and logs the default unit formats for all unit types in the current ArcGIS Pro project.
    /// </summary>
    public static async Task GetListofDefaultFormats()
    {
      //Must be on the QueuedTask.Run()
      await QueuedTask.Run(() =>
      {
        var unit_formats = Enum.GetValues(typeof(UnitFormatType))
                              .OfType<UnitFormatType>().ToList();
        System.Diagnostics.Debug.WriteLine("Default project units\r\n");

        foreach (var unit_format in unit_formats)
        {
          var default_unit = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(unit_format);
          var line = $"{unit_format}: {default_unit.DisplayName}, {default_unit.UnitCode}";
          System.Diagnostics.Debug.WriteLine(line);
        }
        System.Diagnostics.Debug.WriteLine("");
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
    #region Get A Specific Default Unit Format for the Current Project
    /// <summary>
    /// Retrieves the default distance unit format for the current ArcGIS Pro project.
    /// </summary>
    public static DisplayUnitFormat GetProjectDefaultUnitFormat()
    {
      //Must be on the QueuedTask.Run()

      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location, UnitFormatType.Page
      //UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
      var default_unit = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(
                                                           UnitFormatType.Distance);
      return default_unit;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetPredefinedProjectUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetProjectUnitFormats
    #region Set a Specific List of Unit Formats for the Current Project
    /// <summary>
    /// Sets a specific list of location unit formats for the current ArcGIS Pro project, optionally specifying a new default unit.
    /// </summary>
    public static async Task SetListofUnitFormatsForCurrentProject()
    {
      await QueuedTask.Run(() =>
      {
        //Must be on the QueuedTask.Run()

        //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
        //UnitFormatType.Direction, UnitFormatType.Location

        //Get the full list of all available location units
        var all_units = DisplayUnitFormats.Instance.GetPredefinedProjectUnitFormats(
                                                              UnitFormatType.Location);
        //keep units with an even factory code
        var list_units = all_units.Where(du => du.UnitCode % 2 == 0).ToList();

        //set them as the new location unit collection. A new default is not being specified...
        DisplayUnitFormats.Instance.SetProjectUnitFormats(list_units);

        //set them as the new location unit collection along with a new default
        DisplayUnitFormats.Instance.SetProjectUnitFormats(
                                                list_units, list_units.First());

        //Note: UnitFormatType.Page, UnitFormatType.Symbol2D, UnitFormatType.Symbol3D
        //cannot be set.
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetDefaultProjectUnitFormat
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetDefaultProjectUnitFormat

    #region Set the Defaults for the Project Unit Formats
    /// <summary>
    /// Sets the default unit formats for all unit types in the current ArcGIS Pro project.
    /// </summary>
    public static async Task SetDefaultsforProjectUnitFormats()
    {
      //Must be on the QueuedTask.Run()
      await QueuedTask.Run(() =>
      {
        var unit_formats = Enum.GetValues(typeof(UnitFormatType)).OfType<UnitFormatType>().ToList();
        foreach (var unit_type in unit_formats)
        {
          var current_default = DisplayUnitFormats.Instance.GetDefaultProjectUnitFormat(unit_type);
          //Arbitrarily pick the last unit in each unit format list
          var replacement = DisplayUnitFormats.Instance.GetProjectUnitFormats(unit_type).Last();
          DisplayUnitFormats.Instance.SetDefaultProjectUnitFormat(replacement);

          var line = $"{current_default.DisplayName}, {current_default.UnitName}, {current_default.UnitCode}";
          var line2 = $"{replacement.DisplayName}, {replacement.UnitName}, {replacement.UnitCode}";

          System.Diagnostics.Debug.WriteLine($"Format: {unit_type}");
          System.Diagnostics.Debug.WriteLine($" Current default: {line}");
          System.Diagnostics.Debug.WriteLine($" Replacement default: {line2}");
        }
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.UnitFormats.UnitFormatType
    // cref: ArcGIS.Desktop.Core.UnitFormats.DisplayUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.GetProjectUnitFormats
    // cref: ArcGIS.Desktop.Core.UnitFormats.IDisplayUnitFormats.SetProjectUnitFormats
    #region Update Unit Formats for the Project
    /// <summary>
    /// Updates the display names of angular unit formats in the current ArcGIS Pro project by appending their abbreviations and sets the first unit as the default.
    /// </summary>
    public static void UpdateProjectUnitFormats()
    {
      //UnitFormatType.Angular, UnitFormatType.Area, UnitFormatType.Distance, 
      //UnitFormatType.Direction, UnitFormatType.Location
      var angle_units = DisplayUnitFormats.Instance.GetProjectUnitFormats(UnitFormatType.Angular);

      //Edit the display name of each unit - append the abbreviation
      foreach (var unit in angle_units)
      {
        unit.DisplayName = $"{unit.DisplayName} ({unit.Abbreviation})";
      }
      //apply the changes to the units and set the default to be the first entry
      DisplayUnitFormats.Instance.SetProjectUnitFormats(angle_units, angle_units.First());
      //The project must be saved to persist the changes...
    }
    #endregion
  }
}
