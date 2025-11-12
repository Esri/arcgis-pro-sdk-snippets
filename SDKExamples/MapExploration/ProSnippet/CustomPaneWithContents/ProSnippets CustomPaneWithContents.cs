using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProSnippets.MapExplorationSnippets.CustomPaneWithContents
{
  public partial class ProSnippetsMapExploration : ViewStatePane, IContentsProvider, IContentsControl
  {
    DispatcherTimer timer;
    private string _currentTime = "";
    private static OperationManager _operationsManager = null;
    /// <summary>
    /// Consume the passed in CIMView. Call the base constructor to wire up the CIMView.
    /// </summary>
    public ProSnippetsMapExploration(CIMView view)
      : base(view)
    {

      timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background,
          timer_Tick, Dispatcher.CurrentDispatcher);
      timer.IsEnabled = true;
    }
    private void timer_Tick(object sender, EventArgs e)
    {
      _currentTime = DateTime.Now.ToString("HH:mm:ss tt");
      NotifyPropertyChanged("CurrentTime");
    }
    #region Snippet Create a Contents Control for a Custom View
    /// <summary>
    /// Create a contents control to be shown as the TOC when the View is activated. The contents will be shown in the Contents Dock Pane when the IContentsProvider (i.e. your Pane) is the active pane
    /// Refer to the community sample for a complete example: https://github.com/Esri/arcgis-pro-sdk-community-samples/tree/master/Map-Exploration/CustomPaneWithContents
    /// </summary>
    private Contents _contents;

    public Contents Contents
    {
      get
      {
        if (_contents == null)
        {
          //This is your user control to be used as contents
          FrameworkElement contentsControl = new CustomPaneContentsControl()
          {
            //Vanilla MVVM here
            DataContext = this//This can be any custom view model
          };

          //This is the Pro Framework contents control wrapper
          _contents = new Contents()
          {
            //The view is your user control
            ContentsView = contentsControl,
            ContentsViewModel = this,//This is your pane view model
            OperationManager = this.OperationManager//optional
          };
        }
        return _contents;
      }
    }
    #endregion
    /// <summary>
    /// Note: Provide your own operations manager to maintain your own associated
    /// undo/redo stack on the UI
    /// </summary>
    public override OperationManager OperationManager
    {
      get
      {
        if (_operationsManager == null)
          _operationsManager = new OperationManager();
        return _operationsManager;
      }
    }

    public override CIMView ViewState => throw new NotImplementedException();

    bool IContentsProvider.ContentsReady => throw new NotImplementedException();

    Contents IContentsProvider.Contents => throw new NotImplementedException();

    bool IContentsControl.ReadOnly { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    string IContentsControl.CaptionOverride => throw new NotImplementedException();
  }
}
