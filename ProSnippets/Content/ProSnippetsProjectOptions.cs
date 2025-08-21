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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsContent
{
  public static class ProSnippetsProjectOptions
  {
    #region ProSnippet Group: Application Options 
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
    // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
    // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
    // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
    // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
    #region Get GeneralOptions
    /// <summary>
    /// Retrieves the current general application options, including startup settings, home folder, default geodatabase, default toolbox, and project creation folder settings.
    /// </summary>
    public static void GeneralOptions()
    {
      var startMode = ApplicationOptions.GeneralOptions.StartupOption;
      var aprx_path = ApplicationOptions.GeneralOptions.StartupProjectPath;

      var hf_option = ApplicationOptions.GeneralOptions.HomeFolderOption;
      var folder = ApplicationOptions.GeneralOptions.CustomHomeFolder;

      var gdb_option = ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption;
      var def_gdb = ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase;

      var tbx_option = ApplicationOptions.GeneralOptions.DefaultToolboxOption;
      var def_tbx = ApplicationOptions.GeneralOptions.CustomDefaultToolbox;

      var create_in_folder = ApplicationOptions.GeneralOptions.ProjectCreateInFolder;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
    // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
    // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
    // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
    // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
    #region Set GeneralOptions to Use Custom Settings
    /// <summary>
    /// Configures the application to use custom settings for the startup project, home folder, default geodatabase, and default toolbox.
    /// </summary>
    public static void SetGeneralOptions()
    {
      //Set the application to use a custom project, home folder, gdb, and toolbox
      //In each case, the custom _path_ must be set _first_ before 
      //setting the "option". This ensures the application remains 
      //in a consistent state. This is the same behavior as on the Pro UI.
      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.StartupProjectPath))
        ApplicationOptions.GeneralOptions.StartupProjectPath = @"D:\data\usa.aprx";//custom project path first
      ApplicationOptions.GeneralOptions.StartupOption = StartProjectMode.WithDefaultProject;//option to use it second

      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomHomeFolder))
        ApplicationOptions.GeneralOptions.CustomHomeFolder = @"D:\home_folder";//custom home folder first
      ApplicationOptions.GeneralOptions.HomeFolderOption = OptionSetting.UseCustom;//option to use it second

      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase))
        ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase = @"D:\data\usa.gdb";//custom gdb path first
      ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption = OptionSetting.UseCustom;//option to use it second

      if (string.IsNullOrEmpty(ApplicationOptions.GeneralOptions.CustomDefaultToolbox))
        ApplicationOptions.GeneralOptions.CustomDefaultToolbox = @"D:\data\usa.tbx";//custom toolbox path first
      ApplicationOptions.GeneralOptions.DefaultToolboxOption = OptionSetting.UseCustom;//option to use it second
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.StartupProjectPath
    // cref: ArcGIS.Desktop.Core.GeneralOptions.HomeFolderOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomHomeFolder
    // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultGeodatabaseOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultGeodatabase
    // cref: ArcGIS.Desktop.Core.GeneralOptions.DefaultToolboxOption
    // cref: ArcGIS.Desktop.Core.GeneralOptions.CustomDefaultToolbox
    // cref: ArcGIS.Desktop.Core.GeneralOptions.ProjectCreateInFolder
    // cref: ArcGIS.Desktop.Core.StartProjectMode
    // cref: ArcGIS.Desktop.Core.OptionSetting
    #region Set GeneralOptions to Use Defaults
    /// <summary>
    /// Configures the application to reset all general options, such as startup project, home folder, default geodatabase, and default toolbox, to their default settings.
    /// </summary>
    public static void SetGeneralOptionsToDefaults()
    {
      //Default options can be set regardless of the value of the "companion"
      //path (to a project, folder, gdb, toolbox, etc.). The path value is ignored if
      //the option setting does not use it. This is the same behavior as on the Pro UI.
      ApplicationOptions.GeneralOptions.StartupOption = StartProjectMode.ShowStartPage;
      ApplicationOptions.GeneralOptions.HomeFolderOption = OptionSetting.UseDefault;
      ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption = OptionSetting.UseDefault;
      ApplicationOptions.GeneralOptions.DefaultToolboxOption = OptionSetting.UseDefault;//set default option first

      //path values can (optionally) be set (back) to null if their 
      //"companion" option setting is the default option.
      if (ApplicationOptions.GeneralOptions.StartupOption != StartProjectMode.WithDefaultProject)
        ApplicationOptions.GeneralOptions.StartupProjectPath = null;
      if (ApplicationOptions.GeneralOptions.HomeFolderOption == OptionSetting.UseDefault)
        ApplicationOptions.GeneralOptions.CustomHomeFolder = null;
      if (ApplicationOptions.GeneralOptions.DefaultGeodatabaseOption == OptionSetting.UseDefault)
        ApplicationOptions.GeneralOptions.CustomDefaultGeodatabase = null;
      if (ApplicationOptions.GeneralOptions.DefaultToolboxOption == OptionSetting.UseDefault)
        ApplicationOptions.GeneralOptions.CustomDefaultToolbox = null;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.DownloadOptions
    // cref: ArcGIS.Desktop.Core.DownloadOptions
    #region Get DownloadOptions
    /// <summary>
    /// Retrieves the current download options, including staging location, unpack locations for PPKX and other files, and offline maps settings.
    /// </summary>
    public static void GetDownloadOptions()
    {
      var staging = ApplicationOptions.DownloadOptions.StagingLocation;

      var ppkx_loc = ApplicationOptions.DownloadOptions.UnpackPPKXLocation;
      var ask_ppkx_loc = ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation;

      var other_loc = ApplicationOptions.DownloadOptions.UnpackOtherLocation;
      var ask_other_loc = ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation;
      var use_proj_folder = ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation;

      var offline_loc = ApplicationOptions.DownloadOptions.OfflineMapsLocation;
      var ask_offline_loc = ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation;
      var use_proj_folder_offline = ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DownloadOptions
    // cref: ArcGIS.Desktop.Core.DownloadOptions.StagingLocation
    #region Set Staging Location for Sharing and Publishing
    /// <summary>
    /// Sets the staging location for sharing and publishing operations in the application.
    /// </summary>
    public static void SetStagingLocation()
    {
      ApplicationOptions.DownloadOptions.StagingLocation = @"D:\data\staging";
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DownloadOptions
    // cref: ArcGIS.Desktop.Core.DownloadOptions.AskForUnpackPPKXLocation
    #region Set DownloadOptions for PPKX
    /// <summary>
    /// Configures the download options for PPKX files, including whether to prompt the user for a location or use a specified default unpack location.
    /// </summary>
    public static void SetDownloadOptionsForPPKX()
    {
      //Options are mutually exclusive.

      //Setting ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = true
      //supersedes any value in ApplicationOptions.DownloadOptions.UnpackPPKXLocation
      //and will prompt the user on an unpack. The value of 
      //ApplicationOptions.DownloadOptions.UnpackPPKXLocation will be unaffected
      //and is ignored. This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = true;//override location

      //The default location is typically <My Documents>\ArcGIS\Packages
      //Setting ApplicationOptions.DownloadOptions.UnpackPPKXLocation to any
      //location overrides ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation
      //and sets it to false. This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.UnpackPPKXLocation = @"D:\data\for_ppkx";

      //Or, if ApplicationOptions.DownloadOptions.UnpackPPKXLocation already
      //contains a valid path, set ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation
      //explicitly to false to use the UnpackPPKXLocation
      if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.UnpackPPKXLocation))
        ApplicationOptions.DownloadOptions.AskForUnpackPPKXLocation = false;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DownloadOptions
    // cref: ArcGIS.Desktop.Core.DownloadOptions.UnpackOtherLocation
    #region Set DownloadOptions for UnpackOther
    /// <summary>
    /// Configures the download options for unpacking files other than PPKX or APTX, including whether to prompt the user for a location or use a specified default unpack location.
    /// </summary>
    public static void SetDownloadOptionsForUnpackOther()
    {
      //UnpackOther settings control unpacking of anything _other than_
      //a ppkx or aptx. Options are mutually exclusive.

      //Set ApplicationOptions.DownloadOptions.UnpackOtherLocation explicitly to
      //toggle ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation and
      //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false
      //Note: default is typically <My Documents>\ArcGIS\Packages, _not_ null.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.UnpackOtherLocation = @"D:\data\for_other";

      //or...to use a location already stored in UnpackOtherLocation as the
      //default without changing it, 
      //set ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation and
      //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false
      //explicitly. This is the same behavior as on the Pro UI.
      if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.UnpackOtherLocation))
      {
        ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation = false;
        ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation = false;
      }

      //Setting ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation to
      //true overrides any UnpackOtherLocation value and sets 
      //ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation = true;

      //Setting ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation to
      //true overrides any UnpackOtherLocation value and sets 
      //ApplicationOptions.DownloadOptions.AskForUnpackOtherLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.UnpackOtherToProjectLocation = false;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DownloadOptions
    // cref: ArcGIS.Desktop.Core.DownloadOptions.OfflineMapsLocation
    #region Set DownloadOptions for OfflineMaps
    /// <summary>
    /// Configures the download options for offline maps, including whether to prompt the user for a location or use a specified default location.
    /// </summary>
    public static void SetDownloadOptionsForOfflineMaps()
    {
      //OfflineMaps settings control where map content that is taken
      //offline is copied to on the local machine. Options are mutually exclusive.

      //Set ApplicationOptions.DownloadOptions.OfflineMapsLocation explicitly to
      //toggle ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation and
      //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false
      //Note: default is typically <My Documents>\ArcGIS\OfflineMaps, _not_ null.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.OfflineMapsLocation = @"D:\data\for_offline";

      //or...to use a location already stored in OfflineMapsLocation as the
      //default without changing it, 
      //set ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation and
      //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false
      //explicitly.
      if (!string.IsNullOrEmpty(ApplicationOptions.DownloadOptions.OfflineMapsLocation))
      {
        ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation = false;
        ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation = false;
      }

      //Setting ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation to
      //true overrides any OfflineMapsLocation value and sets 
      //ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation = true;

      //Setting ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation to
      //true overrides any OfflineMapsLocation value and sets 
      //ApplicationOptions.DownloadOptions.AskForOfflineMapsLocation to false.
      //This is the same behavior as on the Pro UI.
      ApplicationOptions.DownloadOptions.OfflineMapsToProjectLocation = true;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions
    // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomHomeFolder
    // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomDefaultGeodatabase
    // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectCustomDefaultToolbox
    // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectDeleteLocalCopyOnClose
    // cref: ArcGIS.Desktop.Core.GeneralOptions.PortalProjectDownloadLocation
    #region Get/Set Portal Project Options
    /// <summary>
    /// Retrieves and sets portal project options, including custom home folder, default geodatabase, default toolbox, download location, and delete-on-close settings.
    /// </summary>
    public static void GetSetPortalProjectOptions()
    {
      // access the current options
      var def_home = ApplicationOptions.GeneralOptions.PortalProjectCustomHomeFolder;
      var def_gdb = ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultGeodatabase;
      var def_tbx = ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultToolbox;
      var deleteOnClose = ApplicationOptions.GeneralOptions.PortalProjectDeleteLocalCopyOnClose;
      var def_location = ApplicationOptions.GeneralOptions.PortalProjectDownloadLocation;


      // set the options
      ApplicationOptions.GeneralOptions.PortalProjectCustomHomeFolder = @"E:\data";
      ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultGeodatabase = @"E:\data\usa.gdb";
      ApplicationOptions.GeneralOptions.PortalProjectCustomDefaultToolbox = @"E:\data\usa.tbx";
      ApplicationOptions.GeneralOptions.PortalProjectDeleteLocalCopyOnClose = false;
      ApplicationOptions.GeneralOptions.PortalProjectDownloadLocation = @"E:\data";
    }
    #endregion
  }
}
