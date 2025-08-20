/*

   Copyright 2025 Esri

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

// Ignore Spelling: Dockpane Addin Cancelable Progressor Uninitialize

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System.Collections.Generic;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Core.Events;
using ArcGIS.Desktop.Mapping.Events;
using System.Linq;
using ArcGIS.Desktop.Framework.Controls;
using ArcGIS.Desktop.Internal.Framework.Metro;

namespace Framework.Snippets
{
  public class Dockpane1ViewModel : ArcGIS.Desktop.Framework.Contracts.DockPane
  {
    private SubscriptionToken _eventToken = null;

    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.OnShow
    #region  How to subscribe and unsubscribe to events when the dockpane is visible or hidden
    /// <summary>
    /// Handles changes to the visibility state of the dock pane.
    /// </summary>
    /// <remarks>Override this method to perform actions when the dock pane is shown or hidden, such as
    /// subscribing to or unsubscribing from events.  This method is called automatically by the framework when the dock
    /// pane's visibility changes.</remarks>
    /// <param name="isVisible"><see langword="true"/> if the dock pane is being shown; otherwise, <see langword="false"/>.</param>
    protected override void OnShow(bool isVisible)
    {
      if (isVisible && _eventToken == null) //Subscribe to event when dockpane is visible
      {
        _eventToken = MapSelectionChangedEvent.Subscribe(OnMapSelectionChangedEvent);
      }

      if (!isVisible && _eventToken != null) //Unsubscribe as the dockpane closes.
      {
        MapSelectionChangedEvent.Unsubscribe(_eventToken);
        _eventToken = null;
      }
    }
    //Event handler when the MapSelection event is triggered.
    private void OnMapSelectionChangedEvent(MapSelectionChangedEventArgs obj)
    {
      MessageBox.Show("Selection has changed");
    }
    #endregion
  }

  public static class ProSnippets2
  {
    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetPlugInWrapper
    // cref: ArcGIS.Desktop.Framework.IPlugInWrapper
    #region Execute a command
    /// <summary>
    /// Executes a specific command within the ArcGIS Pro framework.
    /// </summary>
    /// <remarks>This method retrieves a plugin wrapper for the command identified by the  string
    /// "esri_editing_ShowAttributes" and attempts to execute it if the  command is available and can be executed. The
    /// command must implement  <see cref="System.Windows.Input.ICommand"/> and support execution without
    /// parameters.</remarks>
    public static void ExecuteCommand()
    {
      IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("esri_editing_ShowAttributes");
      var command = wrapper as ICommand; // tool and command(Button) supports this

      if ((command != null) && command.CanExecute(null))
        command.Execute(null);
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.SetCurrentToolAsync
    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetPlugInWrapper
    // cref: ArcGIS.Desktop.Framework.IPlugInWrapper
    #region Set the current tool
    /// <summary>
    /// Sets the current tool to the "Select By Rectangle" tool in the ArcGIS Pro application.
    /// </summary>
    /// <remarks>This method sets the active tool to the "Select By Rectangle" tool, which allows users to
    /// select features by drawing a rectangle on the map. It internally uses either <see
    /// cref="FrameworkApplication.SetCurrentToolAsync(string)"/> or the <see cref="ICommand.Execute(object)"/> method
    /// to activate the tool.</remarks>
    public static void SetCurrentTool()
    {
      // use SetCurrentToolAsync
      FrameworkApplication.SetCurrentToolAsync("esri_mapping_selectByRectangleTool");

      // or use ICommand.Execute
      ICommand cmd = FrameworkApplication.GetPlugInWrapper("esri_mapping_selectByRectangleTool") as ICommand;
      if ((cmd != null) && cmd.CanExecute(null))
        cmd.Execute(null);
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.ActivateTab
    #region Activate a tab
    /// <summary>
    /// Activates the "Insert" tab in the ArcGIS Pro application.
    /// </summary>
    /// <remarks>This method switches the active tab in the ArcGIS Pro ribbon to the "Insert" tab,  allowing
    /// users to access tools and functionality related to inserting elements  such as layouts, maps, and data into the
    /// project.</remarks>
    public static void ActivateTab()
    {
      FrameworkApplication.ActivateTab("esri_mapping_insertTab");
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.State.Activate
    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.State.Deactivate
    #region Activate/Deactivate a state - to modify a condition
    public static void Activate(bool activate)
    {
      // Define the condition in the DAML file based on the state 
      if (activate)
        FrameworkApplication.State.Activate("someState");
      else
        FrameworkApplication.State.Deactivate("someState");
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.IsBusy
    #region Determine if the application is busy
    /// <summary>
    /// Determines whether the application is currently busy.
    /// </summary>
    /// <remarks>The application is considered busy if a task is running on the main worker thread or if any
    /// pane or dock pane  reports that it is busy or initializing. This property is commonly used to disable user
    /// interaction with UI  elements, such as buttons or list boxes, while the application is busy. <para> For example,
    /// many Pro styles (e.g., <c>Esri_SimpleButton</c>) automatically disable buttons when  <see
    /// cref="FrameworkApplication.IsBusy"/> is <see langword="true"/>. You can bind this property to the 
    /// <c>IsEnabled</c> property of a control to achieve similar behavior. </para></remarks>
    public static void IsBusy()
    {
      // The application is considered busy if a task is currently running on the main worker thread or any 
      // pane or dock pane reports that it is busy or initializing.   

      // Many Pro styles (such as Esri_SimpleButton) ensure that a button is disabled when FrameworkApplication.IsBusy is true
      // You would use this property to bind to the IsEnabled property of a control (such as a listbox) on a dockpane or pane in order
      // to disable it from user interaction while the application is busy. 
      bool isbusy = FrameworkApplication.IsBusy;
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication
    #region Get the Application main window
    /// <summary>
    /// Centers the main application window on the screen.
    /// </summary>
    /// <remarks>This method retrieves the main window of the application and adjusts its position so that it
    /// is centered within the available screen work area.</remarks>
    public static void GetMainWindow()
    {
      System.Windows.Window window = FrameworkApplication.Current.MainWindow;

      // center it
      Rect rect = System.Windows.SystemParameters.WorkArea;
      FrameworkApplication.Current.MainWindow.Left = rect.Left + (rect.Width - FrameworkApplication.Current.MainWindow.ActualWidth) / 2;
      FrameworkApplication.Current.MainWindow.Top = rect.Top + (rect.Height - FrameworkApplication.Current.MainWindow.ActualHeight) / 2;
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Close
    #region Close ArcGIS Pro
    /// <summary>
    /// Closes the ArcGIS Pro application.
    /// </summary>
    /// <remarks>This method invokes the framework's close operation to terminate the ArcGIS Pro application. 
    /// Ensure that any unsaved work is saved before calling this method, as it will close the application.</remarks>
    public static void ClosePro()
    {
      FrameworkApplication.Close();
    }
    #endregion

    #region Get ArcGIS Pro version
    /// <summary>
    /// Retrieves and processes the version of the currently executing ArcGIS Pro application.
    /// </summary>
    /// <remarks>This method obtains the version information from the entry assembly of the application. The
    /// version is represented as a string in the format defined by the assembly's versioning.</remarks>
    public static void GetProVersion()
    {
      string version = System.Reflection.Assembly.GetEntryAssembly()
                                             .GetName().Version.ToString();
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
    // cref: ArcGIS.Desktop.Framework.PaneCollection.ClosePane
    #region Close a specific pane
    /// <summary>
    /// Closes all instances of a pane with the specified pane ID.
    /// </summary>
    /// <remarks>This method iterates through all open panes in the application to find those matching the specified
    /// <paramref name="paneID"/>. If multiple instances of the pane exist, all of them will be closed.</remarks>
    /// <param name="paneID">The unique identifier (DAML ID) of the pane to close. This ID is used to locate and close all instances of the
    /// specified pane.</param>
    public static void ClosePane(string paneID)
    {
      IList<uint> myPaneInstanceIDs = new List<uint>();
      foreach (Pane pane in FrameworkApplication.Panes)
      {
        if (pane.ContentID == paneID)
        {
          myPaneInstanceIDs.Add(pane.InstanceID); //InstanceID of your pane, could be multiple, so build the collection                    
        }
      }
      foreach (var instanceID in myPaneInstanceIDs) //close each of "your" panes.
      {
        FrameworkApplication.Panes.ClosePane(instanceID);
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
    // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate
    #region Activate a pane
    /// <summary>
    /// Activates the pane with the specified identifier.
    /// </summary>
    /// <remarks>If a pane with the specified identifier is found, it is activated. If no matching pane is
    /// found,  the method performs no action. This method is case-sensitive when matching the pane
    /// identifier.</remarks>
    /// <param name="paneID">The identifier of the pane to activate. This typically corresponds to the pane's caption.</param>
    public static void ActivatePane(string paneID)
    {
      var mapPanes = ProApp.Panes.OfType<IMapPane>();
      foreach (Pane pane in mapPanes)
      {
        if (pane.Caption == paneID)
        {
          pane.Activate();
          break;
        }
      }
    }
    #endregion
  }

  //dummy class to allow snippet to compile
  public class ProWindow1 : Window
  {
    public ProWindow1(double x, double y)
    {
    }
    public bool SaveWindowPosition { get; set; }
  }


  public class ShowProWindow()
  {
    #region ProWindow Position on Screen 
    /// <summary>
    /// Displays the ProWindow1 window at a specified position on the screen.
    /// </summary>
    /// <param name="left">The left position (in pixels) where the window should be displayed on the screen.</param>
    /// <param name="top">The top position (in pixels) where the window should be displayed on the screen.</param>
    /// <remarks>This method creates and displays an instance of the <see cref="ProWindow1"/> class at a fixed
    /// position on the screen. The window's position is not saved between sessions, as <see cref="ProWindow1.SaveWindowPosition"/> is set to <see langword="false"/>.</remarks>
    public static void ShowProWindow1(double left, double top)
    {
      var _prowindow1 = new ProWindow1(left, top); //create window
      _prowindow1.Owner = FrameworkApplication.Current.MainWindow;
      _prowindow1.Closed += (o, e) => { _prowindow1 = null; };

      //MetroWindows remember their last location unless SaveWindowPosition is set to
      //false.
      _prowindow1.SaveWindowPosition = false; //set to false to override the last position

      _prowindow1.Show();
      //uncomment for modal
      //_prowindow1.ShowDialog();
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetAddInInfos
    // cref: ArcGIS.Desktop.Framework.AddInInfo
    #region Get Information on the Currently Installed Add-ins
    /// <summary>
    /// Displays information about the currently installed add-ins in the application.
    /// </summary>
    /// <remarks>This method retrieves metadata for all installed add-ins using the <see
    /// cref="ArcGIS.Desktop.Framework.FrameworkApplication.GetAddInInfos"/> method. The information includes details such
    /// as the add-in's name, description, author, version, and compatibility status. The details are displayed in a
    /// message box and written to the debug output.</remarks>
    public static void GetAddinInfos()
    {
      var addin_infos = FrameworkApplication.GetAddInInfos();
      StringBuilder sb = new StringBuilder();

      foreach (var info in addin_infos)
      {
        if (info == null)
          break;//no addins probed

        sb.AppendLine($"Addin: {info.Name}");
        sb.AppendLine($"Description {info.Description}");
        sb.AppendLine($"ImagePath {info.ImagePath}");
        sb.AppendLine($"Author {info.Author}");
        sb.AppendLine($"Company {info.Company}");
        sb.AppendLine($"Date {info.Date}");
        sb.AppendLine($"Version {info.Version}");
        sb.AppendLine($"FullPath {info.FullPath}");
        sb.AppendLine($"DigitalSignature {info.DigitalSignature}");
        sb.AppendLine($"IsCompatible {info.IsCompatible}");
        sb.AppendLine($"IsDeleted {info.IsDeleted}");
        sb.AppendLine($"TargetVersion {info.TargetVersion}");
        sb.AppendLine($"ErrorMsg {info.ErrorMsg}");
        sb.AppendLine($"ID {info.ID}");
        sb.AppendLine("");
      }
      System.Diagnostics.Debug.WriteLine(sb.ToString());
      MessageBox.Show(sb.ToString(), "Addin Infos");
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.DockPaneManager
    // cref: ArcGIS.Desktop.Framework.DockPaneManager.Find
    #region Find a dockpane
    /// <summary>
    /// Finds a dockpane by its unique DAML ID.
    /// </summary>
    /// <remarks>Use this method to retrieve a dockpane instance when you know its DAML ID.  The DAML ID is
    /// typically defined in the add-in's configuration file.</remarks>
    /// <param name="dockPaneID">The unique identifier of the dockpane, as defined in the DAML configuration.  This value cannot be <see
    /// langword="null"/> or empty.</param>
    public static void FindDockpane(string dockPaneID)
    {
      // in order to find a dockpane you need to know its DAML id
      var pane = FrameworkApplication.DockPaneManager.Find(dockPaneID);
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.DockPaneManager
    // cref: ArcGIS.Desktop.Framework.DockPaneManager.Find
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPaneState
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Activate
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.DockState
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Activate()
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Activate(Bool)
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Pin
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.Hide
    #region Dockpane operations
    /// <summary>
    /// Performs a series of operations on a dockpane identified by its DAML ID.
    /// </summary>
    /// <remarks>This method demonstrates common operations that can be performed on a dockpane, including: <list
    /// type="bullet"> <item><description>Finding the dockpane using its DAML ID.</description></item>
    /// <item><description>Checking its visibility state.</description></item> <item><description>Activating the
    /// dockpane.</description></item> <item><description>Retrieving its current docking state.</description></item>
    /// <item><description>Pinning the dockpane.</description></item> <item><description>Hiding the
    /// dockpane.</description></item> </list> Ensure that the provided <paramref name="dockPaneID"/> corresponds to a
    /// valid dockpane; otherwise, the method may throw an exception.</remarks>
    /// <param name="dockPaneID">The unique DAML ID of the dockpane to operate on. This ID must correspond to a valid dockpane.</param>
    public void DockpaneOperations(string dockPaneID)
    {
      // in order to find a dockpane you need to know its DAML id
      var pane = FrameworkApplication.DockPaneManager.Find(dockPaneID);

      // determine visibility
      bool visible = pane.IsVisible;

      // activate it
      pane.Activate();

      // determine dockpane state
      DockPaneState state = pane.DockState;

      // pin it
      pane.Pin();

      // hide it
      pane.Hide();
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.DockPaneManager
    // cref: ArcGIS.Desktop.Framework.DockPaneManager.Find
    // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.OperationManager
    // cref: ArcGIS.Desktop.Framework.OperationManager.CanRedo
    // cref: ArcGIS.Desktop.Framework.OperationManager.CanUndo
    // cref: ArcGIS.Desktop.Framework.OperationManager.UndoAsync
    // cref: ArcGIS.Desktop.Framework.OperationManager.RedoAsync
    // cref: ArcGIS.Desktop.Framework.OperationManager.ClearUndoCategory
    // cref: ArcGIS.Desktop.Framework.OperationManager.ClearRedoCategory
    #region Dockpane undo / redo
    /// <summary>
    /// Performs undo and redo operations for a specified dockpane, and clears undo and redo stacks for a specific
    /// category.
    /// </summary>
    /// <remarks>This method locates the dockpane using its DAML ID and accesses its <see
    /// cref="ArcGIS.Desktop.Framework.OperationManager"/> to perform undo and redo operations if available. Additionally,
    /// it clears the undo and redo stacks for operations categorized under "Some category". <para> Ensure that the
    /// specified dockpane has an associated <see cref="ArcGIS.Desktop.Framework.OperationManager"/> before calling this
    /// method. If no operations are available to undo or redo, the respective actions will be skipped. </para> <para>
    /// Note: This method is asynchronous but returns void, so exceptions thrown during the asynchronous operations will
    /// not be propagated to the caller. Use with caution in production code. </para></remarks>
    /// <param name="dockPaneID">The unique DAML ID of the dockpane for which undo and redo operations will be performed.</param>
    public static async void DockpaneUndoRedo(string dockPaneID)
    {
      // in order to find a dockpane you need to know its DAML id
      var pane = FrameworkApplication.DockPaneManager.Find(dockPaneID);

      // get the undo stack
      OperationManager manager = pane.OperationManager;
      if (manager != null)
      {
        // undo an operation
        if (manager.CanUndo)
          await manager.UndoAsync();

        // redo an operation
        if (manager.CanRedo)
          await manager.RedoAsync();

        // clear the undo and redo stack of operations of a particular category
        manager.ClearUndoCategory("Some category");
        manager.ClearRedoCategory("Some category");
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.DockPaneManager.Find
    #region Find a dockpane and obtain its ViewModel
    /// <summary>
    /// Finds a dockpane by its DAML ID and retrieves its associated ViewModel.
    /// </summary>
    /// <remarks>The method uses the specified <paramref name="dockPaneID"/> to locate the dockpane and casts it
    /// to its associated ViewModel type. Ensure that the <paramref name="dockPaneID"/> corresponds to a valid dockpane
    /// defined in the DAML file.</remarks>
    /// <param name="dockPaneID">The unique DAML ID of the dockpane to locate. This ID is defined in the application's DAML configuration.</param>
    public static void FindDockpaneViewModel(string dockPaneID)
    {
      // in order to find a dockpane you need to know it's DAML id.  

      // Here is a DAML example with a dockpane defined. Once you have found the dockpane you can cast it
      // to the dockpane viewModel which is defined by the className attribute. 
      // 
      //<dockPanes>
      //  <dockPane id="MySample_Dockpane" caption="Dockpane 1" className="Dockpane1ViewModel" dock="bottom" height="5">
      //    <content className="Dockpane1View" />
      //  </dockPane>
      //</dockPanes>

      Dockpane1ViewModel vm = FrameworkApplication.DockPaneManager.Find(dockPaneID) as Dockpane1ViewModel;
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.OpenBackstage
    #region Open the Backstage tab
    /// <summary>
    /// Opens the Backstage to the "About ArcGIS Pro" tab.
    /// </summary>
    /// <remarks>This method activates the Backstage view and navigates directly to the "About ArcGIS Pro"
    /// tab. Use this method to provide users with access to application information, version details, and other related
    /// content available in the "About" section.</remarks>
    public static void OpenBackstage()
    {
      //Opens the Backstage to the "About ArcGIS Pro" tab.
      FrameworkApplication.OpenBackstage("esri_core_aboutTab");
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.ApplicationTheme
    #region Access the current theme
    /// <summary>
    /// Retrieves the current application theme.
    /// </summary>
    /// <remarks>The application theme is determined by the <see
    /// cref="ArcGIS.Desktop.Framework.FrameworkApplication.ApplicationTheme"/> property. This method can be used to
    /// check whether the application is using the Dark, High Contrast, or Default (Light) theme.</remarks>
    public static void GetTheme()
    {
      //Gets the application's theme
      var theme = FrameworkApplication.ApplicationTheme;
      //ApplicationTheme enumeration
      if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Dark)
      {
        //Dark theme
      }

      if (FrameworkApplication.ApplicationTheme == ApplicationTheme.HighContrast)
      {
        //High Contrast
      }
      if (FrameworkApplication.ApplicationTheme == ApplicationTheme.Default)
      {
        //Light/Default theme
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.Notification
    // cref: ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(Window,String,String,MessageBoxButton,MessageBoxImage,MessageBoxResult,String,String,String)
    // cref: ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(Boolean@,String,Window,String,String,MessageBoxButton,MessageBoxImage,MessageBoxResult,String,String,String)
    #region Display a Pro MessageBox
    /// <summary>
    /// Displays a message box with a specified message, title, buttons, icon, and default result.
    /// </summary>
    /// <remarks>This method shows a message box using the ArcGIS Pro framework. The message box includes a
    /// message,  a title, a set of buttons for user interaction, an icon to indicate the type of message, and a default
    /// button selection.</remarks>
    public static void ShowMessageBox()
    {
      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Some Message", "Some title", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.Notification
    // cref: ArcGIS.Desktop.Framework.Notification.Title
    // cref: ArcGIS.Desktop.Framework.Notification.Message
    // cref: ArcGIS.Desktop.Framework.Notification.ImageSource
    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.AddNotification
    #region Add a toast notification
    public static void AddNotification()
    {
      Notification notification = new Notification();
      notification.Title = FrameworkApplication.Title;
      notification.Message = "Notification 1";
      notification.ImageSource = System.Windows.Application.Current.Resources["ToastLicensing32"] as ImageSource;

      ArcGIS.Desktop.Framework.FrameworkApplication.AddNotification(notification);
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetPlugInWrapper
    // cref: ArcGIS.Desktop.Framework.IPlugInWrapper.SmallImage
    // cref: ArcGIS.Desktop.Framework.IPlugInWrapper.LargeImage
    #region Change a buttons caption or image
    /// <summary>
    /// Updates the caption and images of a custom button in the application.
    /// </summary>
    /// <remarks>This method retrieves a custom button by its unique identifier and modifies its caption, 
    /// small image, and large image. Ensure that the specified image files are included in the  add-in's images folder
    /// with their <c>BuildAction</c> set to <c>Resource</c> and  <c>Copy to Output Directory</c> set to <c>Do not
    /// copy</c>.</remarks>
    public void ChangeCaptionImage()
    {
      IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("MyAddin_MyCustomButton");
      if (wrapper != null)
      {
        wrapper.Caption = "new caption";

        // ensure that T-Rex16 and T-Rex32 are included in your add-in under the images folder and have 
        // BuildAction = Resource and Copy to OutputDirectory = Do not copy
        wrapper.SmallImage = BuildImage("T-Rex16.png");
        wrapper.LargeImage = BuildImage("T-Rex32.png");
      }
    }

    private ImageSource BuildImage(string imageName)
    {
      return new BitmapImage(PackUriForResource(imageName));
    }

    private Uri PackUriForResource(string resourceName)
    {
      string asm = System.IO.Path.GetFileNameWithoutExtension(
          System.Reflection.Assembly.GetExecutingAssembly().Location);
      return new Uri(string.Format("pack://application:,,,/{0};component/Images/{1}", asm, resourceName), UriKind.Absolute);
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.IPlugInWrapper.TooltipHeading
    #region Get a button's tooltip heading
    /// <summary>
    /// Retrieves the tooltip heading for a specified button in the application.
    /// </summary>
    /// <remarks>Use this method to access the tooltip heading associated with a specific button.  The button
    /// ID can correspond to any button in the application.</remarks>
    /// <param name="proButtonID">The unique identifier of the button whose tooltip heading is to be retrieved.  This must be a valid button ID
    /// within the application.</param>
    private void GetButtonTooltipHeading(string proButtonID)
    {
      //Pass in the id of your button. Or pass in any Pro button ID.
      IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper(proButtonID);
      var buttonTooltipHeading = wrapper.TooltipHeading;
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent
    // cref: ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Subscribe
    // cref: ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Unsubscribe
    // cref: ArcGIS.Desktop.Framework.Events.ToolEventArgs.PreviousID
    // cref: ArcGIS.Desktop.Framework.Events.ToolEventArgs.CurrentID
    #region Subscribe to Active Tool Changed Event
    private void SubscribeEvent()
    {
      ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Subscribe(OnActiveToolChanged);
    }
    private void UnSubscribeEvent()
    {
      ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Unsubscribe(OnActiveToolChanged);
    }
    private void OnActiveToolChanged(ArcGIS.Desktop.Framework.Events.ToolEventArgs args)
    {
      string prevTool = args.PreviousID;
      string newTool = args.CurrentID;
    }
    #endregion

    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(FUNC{TASK},Progressor,TASKCREATIONOPTIONS)
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(ACTION,Progressor,TASKCREATIONOPTIONS)
    #region Progressor - Simple and non-cancelable
    /// <summary>
    /// Executes a non-cancelable task with a progressor to display progress feedback.
    /// </summary>
    /// <remarks>This method demonstrates the use of a non-cancelable <see
    /// cref="ArcGIS.Desktop.Framework.Threading.Tasks.Progressor"/> to provide progress feedback during the execution
    /// of a task. The progressor displays a dialog with a message while the task runs. Note that the progress dialog
    /// will not appear if the method is executed in a debugger.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task NonCancelableProgressor()
    {
      ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource ps = new ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource("Doing my thing...", false);

      int numSecondsDelay = 5;
      //If you run this in the DEBUGGER you will NOT see the dialog
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => Task.Delay(numSecondsDelay * 1000).Wait(), ps.Progressor);
    }

    #endregion

    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Value
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Status
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Message
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Max
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressor
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(FUNC{TASK},Progressor,TASKCREATIONOPTIONS)
    // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(ACTION,Progressor,TASKCREATIONOPTIONS)
    #region Cancelable Progressor
    /// <summary>
    /// Executes a task with a cancelable progressor, allowing the operation to be monitored and canceled by the user.
    /// </summary>
    /// <remarks>This method demonstrates the use of a <see
    /// cref="ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource"/>  to perform a long-running
    /// operation that can be canceled. The progressor updates its status, message, and value  during the operation, and
    /// the task can be canceled via the associated cancellation token.  Note: If this method is run in a debugger, the
    /// progress dialog will not be displayed.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task completes when the operation finishes or is
    /// canceled.</returns>
    public async Task CancelableProgressor()
    {
      ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource cps =
        new ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource("Doing my thing - cancelable", "Canceled");

      int numSecondsDelay = 5;
      //If you run this in the DEBUGGER you will NOT see the dialog

      //simulate doing some work which can be canceled
      await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() =>
      {
        cps.Progressor.Max = (uint)numSecondsDelay;
        //check every second
        while (!cps.Progressor.CancellationToken.IsCancellationRequested)
        {
          cps.Progressor.Value += 1;
          cps.Progressor.Status = "Status " + cps.Progressor.Value;
          cps.Progressor.Message = "Message " + cps.Progressor.Value;

          if (System.Diagnostics.Debugger.IsAttached)
          {
            System.Diagnostics.Debug.WriteLine(string.Format("RunCancelableProgress Loop{0}", cps.Progressor.Value));
          }
          //are we done?
          if (cps.Progressor.Value == cps.Progressor.Max) break;
          //block the CIM for a second
          Task.Delay(1000).Wait();

        }
        System.Diagnostics.Debug.WriteLine(string.Format("RunCancelableProgress: Canceled {0}",
                                            cps.Progressor.CancellationToken.IsCancellationRequested));

      }, cps.Progressor);
    }
    #endregion
  }

  // cref: ArcGIS.Desktop.Framework.Contracts.Plugin
  // cref: ArcGIS.Desktop.Framework.Contracts.Plugin.Enabled
  // cref: ArcGIS.Desktop.Framework.Contracts.Plugin.DisabledTooltip
  // cref: ArcGIS.Desktop.Framework.Contracts.Plugin.OnUpdate
  #region customize the disabledText property of a button or tool
  //Set the tool's loadOnClick attribute to "false" in the config.daml. 
  //This will allow the tool to be created when Pro launches, so that the disabledText property can display customized text at startup.
  //Remove the "condition" attribute from the tool. Use the OnUpdate method(below) to set the enable\disable state of the tool.
  //Add the OnUpdate method to the tool.
  //Note: since OnUpdate is called very frequently, you should avoid lengthy operations in this method 
  //as this would reduce the responsiveness of the application user interface.
  internal class SnippetButton : ArcGIS.Desktop.Framework.Contracts.Button
  {
    /// <summary>
    /// Updates the state of the tool, enabling or disabling it based on specific conditions.
    /// </summary>
    /// <remarks>This method determines whether the tool should be enabled or disabled and updates the  <see
    /// cref="Enabled"/> property accordingly. If the tool is disabled, a custom tooltip  message may be set to provide
    /// additional context for the disabled state.</remarks>
    protected override void OnUpdate()
    {
      bool enableSate = true; //TODO: Code your enabled state  
      bool criteria = true;  //TODO: Evaluate criteria for disabledText  

      if (enableSate)
      {
        this.Enabled = true;  //tool is enabled  
      }
      else
      {
        this.Enabled = false;  //tool is disabled  
                               //customize your disabledText here  
        if (criteria)
          this.DisabledTooltip = "Missing criteria 1";
      }
    }
  }
  #endregion

  internal class ProSnippets1
  {
    #region Get an Image Resource from the Current Assembly
    /// <summary>
    /// Demonstrates how to retrieve an image resource embedded in the current assembly.
    /// </summary>
    /// <remarks>This example assumes that the image file is added to the project as a resource with the 
    /// "Build Action" set to "Resource" and "Copy to Output Directory" set to "Do not copy."</remarks>
    public static void ExampleUsage()
    {
      //Image 'Dino32.png' is added as Build Action: Resource, 'Do not copy'
      var img = ForImage("Dino32.png");
      //Use the image...
    }

    /// <summary>
    /// Creates a <see cref="BitmapImage"/> for the specified image resource.
    /// </summary>
    /// <param name="imageName">The name of the image resource to load. This must be a valid resource name.</param>
    /// <returns>A <see cref="BitmapImage"/> representing the specified image resource.</returns>
    public static BitmapImage ForImage(string imageName)
    {
      return new BitmapImage(PackUriForResource(imageName));
    }
    /// <summary>
    /// Creates a pack URI for a resource embedded in the application.
    /// </summary>
    /// <remarks>This method generates a pack URI that can be used to reference resources embedded in the
    /// application,  such as images or other files. The URI follows the "pack://application:,,," scheme, which is used 
    /// in WPF applications to access resources.</remarks>
    /// <param name="resourceName">The name of the resource file, including its extension (e.g., "icon.png").</param>
    /// <param name="folderName">The name of the folder containing the resource within the application's project structure.  Defaults to
    /// "Images". Pass an empty string if the resource is located at the root level.</param>
    /// <returns>A <see cref="Uri"/> representing the absolute pack URI for the specified resource.</returns>
    public static Uri PackUriForResource(string resourceName, string folderName = "Images")
    {
      string asm = System.IO.Path.GetFileNameWithoutExtension(
          System.Reflection.Assembly.GetExecutingAssembly().Location);
      string uriString = folderName.Length > 0
          ? string.Format("pack://application:,,,/{0};component/{1}/{2}", asm, folderName, resourceName)
          : string.Format("pack://application:,,,/{0};component/{1}", asm, resourceName);
      return new Uri(uriString, UriKind.Absolute);
    }

    #endregion
  }


  // There are two ways to prevent ArcGIS Pro from closing
  // 1. Override the CanUnload method on your add-in's module and return false.
  // 2. Subscribe to the ApplicationClosing event and cancel the event when you receive it

  internal class Module1 : Module
  {

    // cref: ArcGIS.Desktop.Framework.Contracts.Module.CanUnload
    // cref: ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent
    // cref: ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Subscribe
    // cref: ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Unsubscribe

    #region Prevent ArcGIS Pro from Closing

    // Called by Framework when ArcGIS Pro is closing
    /// <summary>
    /// Determines whether the application can safely unload.
    /// </summary>
    /// <remarks>This method is invoked by the framework when ArcGIS Pro is closing.  Returning <see
    /// langword="false"/> prevents the application from closing. Override this method to implement custom logic for
    /// determining whether  the application should be allowed to close.</remarks>
    /// <returns><see langword="true"/> if the application can proceed with unloading;  otherwise, <see langword="false"/> to
    /// cancel the unload operation.</returns>
    protected override bool CanUnload()
    {
      //return false to ~cancel~ Application close
      return false;
    }
      public Module1()
      {
        ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Subscribe(OnApplicationClosing);
      }
      ~Module1()
      {
        ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Unsubscribe(OnApplicationClosing);
      }

      private Task OnApplicationClosing(System.ComponentModel.CancelEventArgs args)
      {
        args.Cancel = true;
        return Task.FromResult(0);
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.Events.ProjectOpenedEvent
      // cref: ArcGIS.Desktop.Core.Events.ProjectOpenedEvent.Subscribe
      // cref: ArcGIS.Desktop.Core.Events.ProjectOpenedEvent.Unsubscribe
      // cref: ArcGIS.Desktop.Framework.Contracts.Module.Initialize
      // cref: ArcGIS.Desktop.Framework.Contracts.Module.Uninitialize
      #region How to determine when a project is opened
      protected override bool Initialize() //Called when the Module is initialized.
      {
        ProjectOpenedEvent.Subscribe(OnProjectOpened); //subscribe to Project opened event
        return base.Initialize();
      }

      private void OnProjectOpened(ProjectEventArgs obj) //Project Opened event handler
      {
        MessageBox.Show($"{Project.Current} has opened"); //show your message box
      }

      protected override void Uninitialize() //unsubscribe to the project opened event
      {
        ProjectOpenedEvent.Unsubscribe(OnProjectOpened); //unsubscribe
        return;
      }
      #endregion
    }
  }

  internal class ProSnippetMapTool : MapTool
  {

    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.OVERLAYCONTROLID
    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ONTOOLMOUSEDOWN
    // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.HANDLEMOUSEDOWNASYNC
    // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayControlPositionRatio
    #region How to position an embeddable control inside a MapView

    public ProSnippetMapTool()
    {
      //Set the MapTool base class' OverlayControlID to the DAML id of your embeddable control in the constructor
      this.OverlayControlID = "ProAppModule1_EmbeddableControl1";
    }

    protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
    {
      if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
        e.Handled = true;
    }

    protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
    {
      return QueuedTask.Run(() =>
      {
        //assign the screen coordinate clicked point to the MapTool base class' OverlayControlLocation property.
        this.OverlayControlPositionRatio = e.ClientPoint;

      });
    }

    #endregion

  }

  internal class Module1 : Module
  {
    // cref: ArcGIS.Desktop.Core.CoreModule.GetSuggestedCMDIDs
    #region Suggested command options in CommandSearch when a tab is activated.
    //In the module class..
    public override string[] GetSuggestedCMDIDs(string activeTabID)
    {
      //Return the static list of daml ids you want to be the (suggested) 
      //defaults relevant to the given tab. It can be none, some, or all of the
      //commands associated with the activeTabID.
      //In this example, there are two tabs. This example arbitrarily
      //identifies just one command on each tab to be a default to show in the
      //command search list (when _that_ particular tab is active)
      switch (activeTabID)
      {
        case "CommandSearch_Example_Tab1":
          return new string[] { "CommandSearch_Example_Button2" };
        case "CommandSearch_Example_Tab2":
          return new string[] { "CommandSearch_Example_Button4" };
      }
      return new string[] { "" };
    }
    #endregion
  }



