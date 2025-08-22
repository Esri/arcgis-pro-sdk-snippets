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

using ArcGIS.Core.Data.UtilityNetwork.Trace;
using ArcGIS.Core.Events;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Events;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Controls;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Framework.Metro;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Framework.Snippets
{
  //dummy classes to allow snippet to compile
  public class ProWindow1 : Window
  {
    public ProWindow1(double x, double y)
    {
    }
    public bool SaveWindowPosition { get; set; }
  }

  public class DockpaneViewModel : DockPane
  {
  }

  public class ProSnippetsFramework
  {
    public static async void FrameworkProSnippets()
    {
      SubscriptionToken _eventToken = null;
      bool isVisible = true;
      {
        // Override the OnShow method of a dockpane to subscribe and unsubscribe to events
        // cref: ArcGIS.Desktop.Framework.Contracts.DockPane.OnShow
        #region  How to subscribe and unsubscribe to events when the dockpane is visible or hidden
        {
          if (isVisible && _eventToken == null) // Subscribe to event when dockpane is visible
          {
            _eventToken = MapSelectionChangedEvent.Subscribe(OnMapSelectionChangedEvent);
          }

          if (!isVisible && _eventToken != null) // Unsubscribe as the dockpane closes.
          {
            MapSelectionChangedEvent.Unsubscribe(_eventToken);
            _eventToken = null;
          }
          #endregion

          void OnMapSelectionChangedEvent(MapSelectionChangedEventArgs obj)
          {
            MessageBox.Show("Selection has changed");
          }
        }
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetPlugInWrapper
        // cref: ArcGIS.Desktop.Framework.IPlugInWrapper
        #region Execute a command
        IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("esri_editing_ShowAttributes");
        var command = wrapper as ICommand; // tool and command(Button) supports this

        if ((command != null) && command.CanExecute(null))
          command.Execute(null);
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.SetCurrentToolAsync
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetPlugInWrapper
        // cref: ArcGIS.Desktop.Framework.IPlugInWrapper
        #region Set the current tool
        // use SetCurrentToolAsync
        FrameworkApplication.SetCurrentToolAsync("esri_mapping_selectByRectangleTool");

        // or use ICommand.Execute
        ICommand cmd = FrameworkApplication.GetPlugInWrapper("esri_mapping_selectByRectangleTool") as ICommand;
        if ((cmd != null) && cmd.CanExecute(null))
          cmd.Execute(null);
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.ActivateTab
        #region Activate a tab
        FrameworkApplication.ActivateTab("esri_mapping_insertTab");
        #endregion
      }
      {
        bool activate = true;
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.State.Activate
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.State.Deactivate
        #region Activate/Deactivate a state - to modify a condition

        // Define the condition in the DAML file based on the state 
        if (activate)
          FrameworkApplication.State.Activate("someState");
        else
          FrameworkApplication.State.Deactivate("someState");
        #endregion
      }
      {
        // The application is considered busy if a task is currently running on the main worker thread or any 
        // pane or dock pane reports that it is busy or initializing.   

        // Many Pro styles (such as Esri_SimpleButton) ensure that a button is disabled when FrameworkApplication.IsBusy is true
        // You would use this property to bind to the IsEnabled property of a control (such as a listbox) on a dockpane or pane in order
        // to disable it from user interaction while the application is busy. 
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.IsBusy
        #region Determine if the application is busy
        bool isbusy = FrameworkApplication.IsBusy;
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication
        #region Get the Application main window
        System.Windows.Window window = FrameworkApplication.Current.MainWindow;

        // center it
        Rect rect = System.Windows.SystemParameters.WorkArea;
        FrameworkApplication.Current.MainWindow.Left = rect.Left + (rect.Width - FrameworkApplication.Current.MainWindow.ActualWidth) / 2;
        FrameworkApplication.Current.MainWindow.Top = rect.Top + (rect.Height - FrameworkApplication.Current.MainWindow.ActualHeight) / 2;
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Close
        #region Close ArcGIS Pro
        FrameworkApplication.Close();
        #endregion
      }
      {
        #region Get ArcGIS Pro version
        string version = System.Reflection.Assembly.GetEntryAssembly()
                                               .GetName().Version.ToString();
        #endregion
      }
      {
        string paneID = "MySample_Pane"; // example pane contentID
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
        // cref: ArcGIS.Desktop.Framework.PaneCollection.ClosePane
        #region Close a specific pane
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
        #endregion
      }
      {
        string paneID = "Map"; // example pane caption
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.Panes
        // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate
        #region Activate a pane

        var mapPanes = ProApp.Panes.OfType<IMapPane>();
        foreach (Pane pane in mapPanes)
        {
          if (pane.Caption == paneID)
          {
            pane.Activate();
            break;
          }
        }
        #endregion
      }
      {
        #region ProWindow Position on Screen 
        void ShowProWindow1(double left, double top)
        {
          ProWindow1 _prowindow1 = new ProWindow1(left, top); //create window
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
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetAddInInfos
        // cref: ArcGIS.Desktop.Framework.AddInInfo
        #region Get Information on the Currently Installed Add-ins
        var addin_infos = FrameworkApplication.GetAddInInfos();
        StringBuilder sb = new StringBuilder();

        foreach (var info in addin_infos)
        {
          if (info == null)
            break;//no add-ins probed

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
        #endregion
      }
      {
        string dockPaneID = "MySample_Dockpane"; // example dockpane DAML id
        // cref: ArcGIS.Desktop.Framework.DockPaneManager
        // cref: ArcGIS.Desktop.Framework.DockPaneManager.Find
        #region Find a dockpane
        // in order to find a dockpane you need to know its DAML id
        var pane = FrameworkApplication.DockPaneManager.Find(dockPaneID);
        #endregion
      }
      {
        string dockPaneID = "MySample_Dockpane"; // example dockpane DAML id
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
      }
      {
        string dockPaneID = "MySample_Dockpane"; // example dockpane DAML id
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
        #endregion
      }
      {
        string dockPaneID = "MySample_Dockpane"; // example dockpane DAML id
                                                 // in order to find a dockpane you need to know it's DAML id.  

        // Here is a DAML example with a dockpane defined. Once you have found the dockpane you can cast it
        // to the dockpane viewModel which is defined by the className attribute. 
        // 
        //<dockPanes>
        //  <dockPane id="MySample_Dockpane" caption="Dockpane 1" className="Dockpane1ViewModel" dock="bottom" height="5">
        //    <content className="Dockpane1View" />
        //  </dockPane>
        //</dockPanes>

        // cref: ArcGIS.Desktop.Framework.DockPaneManager.Find
        #region Find a dockpane and obtain its ViewModel
        DockpaneViewModel vm = FrameworkApplication.DockPaneManager.Find(dockPaneID) as DockpaneViewModel;
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.OpenBackstage
        #region Open the Backstage tab
        //Opens the Backstage to the "About ArcGIS Pro" tab.
        FrameworkApplication.OpenBackstage("esri_core_aboutTab");
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.ApplicationTheme
        #region Access the current theme
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
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.Notification
        // cref: ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(Window,String,String,MessageBoxButton,MessageBoxImage,MessageBoxResult,String,String,String)
        // cref: ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(Boolean@,String,Window,String,String,MessageBoxButton,MessageBoxImage,MessageBoxResult,String,String,String)
        #region Display a Pro MessageBox
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Some Message", "Some title", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.Notification
        // cref: ArcGIS.Desktop.Framework.Notification.Title
        // cref: ArcGIS.Desktop.Framework.Notification.Message
        // cref: ArcGIS.Desktop.Framework.Notification.ImageSource
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.AddNotification
        #region Add a toast notification
        Notification notification = new Notification();
        notification.Title = FrameworkApplication.Title;
        notification.Message = "Notification 1";
        notification.ImageSource = System.Windows.Application.Current.Resources["ToastLicensing32"] as ImageSource;

        ArcGIS.Desktop.Framework.FrameworkApplication.AddNotification(notification);
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.FrameworkApplication.GetPlugInWrapper
        // cref: ArcGIS.Desktop.Framework.IPlugInWrapper.SmallImage
        // cref: ArcGIS.Desktop.Framework.IPlugInWrapper.LargeImage
        #region Change a buttons caption or image
        IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper("MyAddin_MyCustomButton");
        if (wrapper != null)
        {
          wrapper.Caption = "new caption";

          // ensure that T-Rex16 and T-Rex32 are included in your add-in under the images folder and have 
          // BuildAction = Resource and Copy to OutputDirectory = Do not copy
          wrapper.SmallImage = BuildImage("T-Rex16.png");
          wrapper.LargeImage = BuildImage("T-Rex32.png");
        }
        #endregion
        ImageSource BuildImage(string imageName)
        {
          return new BitmapImage(PackUriForResource(imageName));
        }

        Uri PackUriForResource(string resourceName)
        {
          string asm = System.IO.Path.GetFileNameWithoutExtension(
              System.Reflection.Assembly.GetExecutingAssembly().Location);
          return new Uri(string.Format("pack://application:,,,/{0};component/Images/{1}", asm, resourceName), UriKind.Absolute);
        }
      }
      {
        string proButtonID = "MyAddin_MyCustomButton"; // example button DAML id
                                                       // cref: ArcGIS.Desktop.Framework.IPlugInWrapper.TooltipHeading
        #region Get a button's tooltip heading

        //Pass in the id of your button. Or pass in any Pro button ID.
        IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper(proButtonID);
        var buttonTooltipHeading = wrapper.TooltipHeading;
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent
        // cref: ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Subscribe
        // cref: ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Unsubscribe
        // cref: ArcGIS.Desktop.Framework.Events.ToolEventArgs.PreviousID
        // cref: ArcGIS.Desktop.Framework.Events.ToolEventArgs.CurrentID
        #region Subscribe to Active Tool Changed Event

        void SubscribeEvent()
        {
          ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Subscribe(OnActiveToolChanged);
        }
        void UnSubscribeEvent()
        {
          ArcGIS.Desktop.Framework.Events.ActiveToolChangedEvent.Unsubscribe(OnActiveToolChanged);
        }
        void OnActiveToolChanged(ArcGIS.Desktop.Framework.Events.ToolEventArgs args)
        {
          string prevTool = args.PreviousID;
          string newTool = args.CurrentID;
        }
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(FUNC{TASK},Progressor,TASKCREATIONOPTIONS)
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(ACTION,Progressor,TASKCREATIONOPTIONS)
        #region Progressor - Simple and non-cancelable
        ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource ps = new ArcGIS.Desktop.Framework.Threading.Tasks.ProgressorSource("Doing my thing...", false);

        int numSecondsDelay = 5;
        //If you run this in the DEBUGGER you will NOT see the dialog
        await ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(() => Task.Delay(numSecondsDelay * 1000).Wait(), ps.Progressor);
        #endregion
      }
      {
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Value
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Status
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Message
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.Progressor.Max
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressor
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.CancelableProgressorSource
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(FUNC{TASK},Progressor,TASKCREATIONOPTIONS)
        // cref: ArcGIS.Desktop.Framework.Threading.Tasks.QueuedTask.Run(ACTION,Progressor,TASKCREATIONOPTIONS)
        #region Cancelable Progressor
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
        #endregion

        // cref: ArcGIS.Desktop.Framework.Contracts.Plugin
        // cref: ArcGIS.Desktop.Framework.Contracts.Plugin.Enabled
        // cref: ArcGIS.Desktop.Framework.Contracts.Plugin.DisabledTooltip
        // cref: ArcGIS.Desktop.Framework.Contracts.Plugin.OnUpdate
        bool Enabled = true;
        string DisabledTooltip = "";
        #region customize the disabledText property of a button or tool
        //Set the tool's loadOnClick attribute to "false" in the config.daml. 
        //This will allow the tool to be created when Pro launches, so that the disabledText property can display customized text at startup.
        //Remove the "condition" attribute from the tool. Use the OnUpdate method(below) to set the enable\disable state of the tool.
        //Add the OnUpdate method to the tool.
        //Note: since OnUpdate is called very frequently, you should avoid lengthy operations in this method 
        //as this would reduce the responsiveness of the application user interface.
        {
          {
            bool enableState = true; //TODO: Code your enabled state  
            bool criteria = true;  //TODO: Evaluate criteria for disabledText  

            if (enableState)
            {
              Enabled = true;  //tool is enabled  
            }
            else
            {
              Enabled = false;  //tool is disabled  
                                     //customize your disabledText here  
              if (criteria)
                DisabledTooltip = "Missing criteria 1";
            }
          }
        }
        #endregion

        #region Get an Image Resource from the Current Assembly
        {
          //Image 'Dino32.png' is added as Build Action: Resource, 'Do not copy'
          var img = ForImage("Dino32.png");

          //Use the image...
          static BitmapImage ForImage(string imageName)
          {
            return new BitmapImage(PackUriForResource(imageName));
          }
          static Uri PackUriForResource(string resourceName, string folderName = "Images")
          {
            string asm = System.IO.Path.GetFileNameWithoutExtension(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            string uriString = folderName.Length > 0
                ? string.Format("pack://application:,,,/{0};component/{1}/{2}", asm, folderName, resourceName)
                : string.Format("pack://application:,,,/{0};component/{1}", asm, resourceName);
            return new Uri(uriString, UriKind.Absolute);
          }
        }
        #endregion
      }
      {
        Func<System.ComponentModel.CancelEventArgs, Task> OnApplicationClosing = null;

        // There are two ways to prevent ArcGIS Pro from closing
        // 1. Override the CanUnload method on your add-in's module and return false.
        // 2. Subscribe to the ApplicationClosing event and cancel the event when you receive it

        // cref: ArcGIS.Desktop.Framework.Contracts.Module.CanUnload
        // cref: ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent
        // cref: ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Subscribe
        // cref: ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Unsubscribe

        #region Prevent ArcGIS Pro from Closing
        // 1. Override the CanUnload method on your add-in's module and return false.
        // Called by Framework when ArcGIS Pro is closing
        {
          //return false to ~cancel~ Application close
        }
        {
          // 2. Subscribe to the ApplicationClosing event and cancel the event when you receive it
          ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Subscribe(OnApplicationClosing);

          // in the Application Closing event handler set the Cancel property of the event args to true to prevent Pro from closing

        }
        // Called by Framework when ArcGIS Pro is closing
        {
          ArcGIS.Desktop.Framework.Events.ApplicationClosingEvent.Unsubscribe(OnApplicationClosing);
        }
        #endregion

        {
          // override the Initialize and Uninitialize methods of your add-in's module to subscribe and unsubscribe to the ProjectOpenedEvent
          // cref: ArcGIS.Desktop.Core.Events.ProjectOpenedEvent
          // cref: ArcGIS.Desktop.Core.Events.ProjectOpenedEvent.Subscribe
          // cref: ArcGIS.Desktop.Core.Events.ProjectOpenedEvent.Unsubscribe
          // cref: ArcGIS.Desktop.Framework.Contracts.Module.Initialize
          // cref: ArcGIS.Desktop.Framework.Contracts.Module.Uninitialize
          #region How to determine when a project is opened
          ProjectOpenedEvent.Subscribe(OnProjectOpened); //subscribe to Project opened event

          ProjectOpenedEvent.Unsubscribe(OnProjectOpened); //unsubscribe from the event as the module is unloaded
          #endregion

          void OnProjectOpened(ProjectEventArgs obj) //Project Opened event handler
          {
            MessageBox.Show($"{Project.Current} has opened"); //show your message box
          }
        }
      }
      {
        string activeTabID = "Some_Tab_DAML_ID"; // example tab DAML id
        string[] result = null;
        // cref: ArcGIS.Desktop.Core.CoreModule
        // cref: ArcGIS.Desktop.Core.CoreModule.GetSuggestedCMDIDs
        #region Suggested command options in CommandSearch when a tab is activated.
        //In the module class..

        //Return the static list of daml ids you want to be the (suggested) 
        //defaults relevant to the given tab. It can be none, some, or all of the
        //commands associated with the activeTabID.
        //In this example, there are two tabs. This example arbitrarily
        //identifies just one command on each tab to be a default to show in the
        //command search list (when _that_ particular tab is active)
        switch (activeTabID)
        {
          case "CommandSearch_Example_Tab1":
            result = new string[] { "CommandSearch_Example_Button2" };
            break;
          case "CommandSearch_Example_Tab2":
            result = new string[] { "CommandSearch_Example_Button4" };
            break;
        }
        result = new string[] { "" };
      }
      #endregion
    }
  }
}

internal class ProSnippetMapTool
{
  private string OverlayControlID = null;
  // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.OVERLAYCONTROLID
  // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.ONTOOLMOUSEDOWN
  // cref: ARCGIS.DESKTOP.MAPPING.MAPTOOL.HANDLEMOUSEDOWNASYNC
  // cref: ArcGIS.Desktop.Mapping.MapTool.OverlayControlPositionRatio
  #region How to position an embeddable control inside a MapView

  public ProSnippetMapTool()
  {
    //Set the MapTool base class' OverlayControlID to the DAML id of your embeddable control in the constructor
    OverlayControlID = "ProAppModule1_EmbeddableControl1";
  }

  // Override the MapTool base class' OverlayControlPositionRatio property to set the position of the embeddable control
  protected void OnToolMouseDown(MapViewMouseButtonEventArgs e)
  {
    if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
      e.Handled = true;
  }

  // Override the MapTool base class' HandleMouseDownAsync method to set the position of the embeddable control
  protected Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
  {
    return QueuedTask.Run(() =>
    {
      Point
            //assign the screen coordinate clicked point to the MapTool base class' OverlayControlLocation property.
            OverlayControlPositionRatio = e.ClientPoint;
    });
  }

  #endregion

}



