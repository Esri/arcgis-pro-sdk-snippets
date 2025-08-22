using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.KnowledgeGraph;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Layouts.ProSnippets
{
  public static class ProSnippetsLayout 
  {
    /// <summary>
    /// This methods has a collection of code snippets related to working with ArcGIS Pro layouts.
    /// </summary>
    /// <remarks>
    /// The code snippets in this class are intended to be used as examples of how to work with layouts in ArcGIS Pro.
    /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
    /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
    /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
    public static void LayoutCodeSnippets()
    {
      {
        var templateFileName = "VisitorsLayout.pagx";
        // cref: ArcGIS.Desktop.Core.IProjectItem
        // cref: ArcGIS.Desktop.Core.ItemFactory
        // cref: ArcGIS.Desktop.Core.ItemFactory.Create
        // cref: ArcGIS.Desktop.Core.Project.AddItem(ArcGIS.Desktop.Core.IProjectItem)
        #region Create an IProjectItem from a layout template pagx file and add it to the project
        // Get layout Template Path from the project's home folder and combine it with a file name
        var projectPath = CoreModule.CurrentProject.HomeFolderPath;
        var layoutTemplateFilePath = System.IO.Path.Combine(projectPath, templateFileName);
        // Create a new layout project item with the layout file path
        // Create an IProjectItem using a layout template pagx file
        IProjectItem pagx = ItemFactory.Instance.Create(layoutTemplateFilePath) as IProjectItem;
        // Add the IProjectItem to the current project
        //Note: Needs QueuedTask to run
        Project.Current.AddItem(pagx);
        #endregion Create an IProjectItem from a layout template pagex file and add it to the project
      }
      {
        var layoutName = "Visitors";
        // cref: ArcGIS.Desktop.Core.Project.GetItems<T>
        // cref: ArcGIS.Desktop.Layouts.LayoutProjectItem
        #region Get a Layout by name from the current project
          LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>()
          .FirstOrDefault((lpi) => lpi.Name == layoutName);
        //Note: Needs QueuedTask to run
        Layout layout = layoutItem.GetLayout();
        #endregion Get a Layout by name from the current project
      }
    }
  }
}
