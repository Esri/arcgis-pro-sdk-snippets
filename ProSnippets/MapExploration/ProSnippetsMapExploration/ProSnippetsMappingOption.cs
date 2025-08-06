using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  public static class ProSnippetsMappingOption
  {
    /// <summary>
    /// Configures and modifies the selection options for the application.
    /// </summary>
    /// <remarks>This method demonstrates how to access and update various selection-related settings  in the
    /// application, such as selection colors, fill styles, tolerance, and methods.  These options control how
    /// selections are visually represented and how selection  operations are performed.  The method uses the <see
    /// cref="ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions"/>  property to retrieve the current selection
    /// options and applies changes using the  provided APIs. Changes include modifying selection colors, enabling or
    /// disabling  selection graphics, and setting selection methods and combination methods.  Note: Some selection
    /// combination methods, such as <see cref="ArcGIS.Desktop.Mapping.SelectionCombinationMethod.XOR"/>,  are not
    /// supported.</remarks>
    #region ProSnippet Group: Mapping Options
    #endregion

    // cref: ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions
    // cref: ArcGIS.Desktop.Core.SelectionOptions
    // cref:ArcGIS.Desktop.Mapping.SelectionMethod
    // cref:ArcGIS.Desktop.Mapping.SelectionCombinationMethod
    #region Get/Set Selection Options
    /// <summary>
    /// Configures and modifies the selection options for the application.
    /// </summary>
    /// <remarks>This method demonstrates how to access and update various selection-related settings in the
    /// application, such as selection colors, fill styles, tolerance, and methods. These options control how
    /// selections are visually represented and how selection operations are performed. The method uses the <see
    /// cref="ArcGIS.Desktop.Core.ApplicationOptions.SelectionOptions"/> property to retrieve the current selection
    /// options and applies changes using the provided APIs. Changes include modifying selection colors, enabling or
    /// disabling selection graphics, and setting selection methods and combination methods. Note: Some selection
    /// combination methods, such as <see cref="ArcGIS.Desktop.Mapping.SelectionCombinationMethod.XOR"/>, are not
    /// supported.</remarks>
    public static void SelectionOptions()
    {
      var options = ApplicationOptions.SelectionOptions;

      QueuedTask.Run(() =>
      {
        var defaultColor = options.DefaultSelectionColor;

        var color = options.SelectionColor as CIMRGBColor;
        options.SetSelectionColor(ColorFactory.Instance.CreateRGBColor(255, 0, 0));

        var defaultFill = options.DefaultSelectionFillColor;
        var fill = options.SelectionFillColor;
        var isHatched = options.IsSelectionFillHatched;
        options.SetSelectionFillColor(ColorFactory.Instance.CreateRGBColor(100, 100, 0));
        if (!isHatched)
          options.SetSelectionFillIsHatched(true);

        var showSelectionChip = options.ShowSelectionChip;
        options.SetShowSelectionChip(!showSelectionChip);

        var showSelectionGraphic = options.ShowSelectionGraphic;
        options.SetShowSelectionGraphic(!showSelectionGraphic);

        var saveSelection = options.SaveSelection;
        options.SetSaveSelection(!saveSelection);

        var defaultTol = options.DefaultSelectionTolerance;
        var tol = options.SelectionTolerance;
        options.SetSelectionTolerance(2 * defaultTol);

        // extension methods available 
        var selMethod = options.SelectionMethod;
        options.SetSelectionMethod(SelectionMethod.Contains);

        var combMethod = options.CombinationMethod;
        options.SetCombinationMethod(SelectionCombinationMethod.Add);

        // note that the following SelectionCombinationMethod is not supported
        //options.SetCombinationMethod(SelectionCombinationMethod.XOR);
      });
    }
    #endregion

    #region Get/Set Table Options
    // cref: ArcGIS.Desktop.Core.ApplicationOptions.TableOptions
    // cref: ArcGIS.Desktop.Core.TableOptions
    // cref:ArcGIS.Core.CIM.TableRowHeightType
    /// <summary>
    /// Configures and modifies table-related application options.
    /// </summary>
    /// <remarks>This method demonstrates how to access and update various table options available in the
    /// application,  such as hiding the "Add New Row" option, toggling selection color overrides, setting font
    /// properties,  and adjusting row and column header height types. It also shows how to change the highlight color 
    /// asynchronously using a queued task.</remarks>
    public static void TableOptions()
    {
      var options = ApplicationOptions.TableOptions;

      var hideAddNewRow = options.HideAddNewRow;
      options.HideAddNewRow = !hideAddNewRow;

      var overrides = options.HonorSelectionColorOverrides;
      options.HonorSelectionColorOverrides = !overrides;

      var activateMapView = options.ActivateMapViewAfterOperations;
      options.ActivateMapViewAfterOperations = !activateMapView;

      var defaultFontTName = options.DefaultFontName;
      var fontName = options.FontName;
      if (options.IsValidFontName("Arial"))
        options.FontName = "Arial";

      var defaultFontSize = options.DefaultFontSize;
      var fontSize = options.FontSize;
      if (options.IsValidFontSize(10))
        options.FontSize = 10;

      var heightType = options.ColumnHeaderHeightType;
      options.ColumnHeaderHeightType = TableRowHeightType.Double;

      var rowHeightType = options.RowHeightType;
      options.RowHeightType = TableRowHeightType.Single;

      var defaultColor = options.DefaultHighlightColor;
      var color = options.HighlightColor;
      QueuedTask.Run(() =>
      {
        options.SetHighlightColor(ColorFactory.Instance.CreateRGBColor(0, 0, 255));
      });
    }
    #endregion
  }
}
