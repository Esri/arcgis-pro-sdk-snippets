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
using System.Linq;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Reports;

namespace ProSnippets.LayoutsSnippets.Reports
{
  /// <summary>
  /// This methods has a collection of code snippets related to working with ArcGIS Pro Reports.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with Reports in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// Crefs are used for internal purposes only. Please ignore them in the context of this example.
  public static partial class ProSnippetsLayouts
  {
    public static async Task ProSnippetsReportsAsync()
    {
      #region ProSnippet Group: Report Project Items
      #endregion
      #region ignore - Variable initialization
      //Different ways to reference or create a report
      ReportView reportView = ReportView.Active;
      Report report = reportView?.Report; //Reference to the active report
      string reportName = "MyReport"; //Name of a report in the project
      ReportProjectItem reportProjectItem = Project.Current.GetItems<ReportProjectItem>()
                                       .FirstOrDefault(); //Reference to a report project item
      report = await QueuedTask.Run(() => reportProjectItem?.GetReport()); //Get the report from a layout project item

      //Reference a feature layer in the map to use as a data source for a report
      var featureLayer = MapView.Active?.Map.GetLayersAsFlattenedList()
                                .OfType<FeatureLayer>()
                                .FirstOrDefault(l => l.Name == "U.S. Cities");

      #endregion
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      #region Gets all the reports in the current project
      {
        var reportsInProject = Project.Current.GetItems<ReportProjectItem>();
        foreach (var reportItem in reportsInProject)
        {
          //Do Something with the report
        }
      }
      #endregion
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport()
      #region Get a specific report
      {
        ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals("Report Name"));
        Report reportInProject = await QueuedTask.Run(() => reportProjItem?.GetReport());
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport()
      // cref: ArcGIS.Desktop.Core.ReportFrameworkExtender.CreateReportPaneAsync(ArcGIS.Desktop.Reports.Report)
      #region Open a Report project item in a new view
      {
        //Open a report project item in a new view.
        //A report project item may exist but it may not be open in a view.

        //Reference a report project item by name
        ReportProjectItem reportPrjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals("MyReport"));

        //Get the report associated with the report project item
        Report reportToOpen = await QueuedTask.Run(() => reportPrjItem.GetReport());

        //Create the new pane
        IReportPane iNewReportPane = await ProApp.Panes.CreateReportPaneAsync(reportToOpen); //GUI thread
      }
      #endregion
      // cref: ArcGIS.Desktop.Reports.IReportPane
      // cref: ArcGIS.Desktop.Reports.IReportPane.ReportView
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Core.ReportFrameworkExtender.FindReportPanes(ArcGIS.Desktop.Reports.Report)
      // cref: ArcGIS.Desktop.Framework.Contracts.Pane.Activate()
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport()
      #region Activate an already open report view
      {
        Report reportToActivate = await QueuedTask.Run(() =>
          Project.Current.GetItems<ReportProjectItem>().FirstOrDefault().GetReport());
        var reportPane = FrameworkApplication.Panes.FindReportPanes(report).Last();
        if (reportPane == null)
        {
          return;
        }
        //Activate the pane
        (reportPane as ArcGIS.Desktop.Framework.Contracts.Pane).Activate();
        //Get the "ReportView" associated with the Report Pane.
        ReportView reportViewFromPane = reportPane.ReportView;
      }
      #endregion

      #region Reference the active report view
      //Confirm if the current, active view is a report view.  If it is, do something.
      ReportView activeReportView = ReportView.Active;
      if (activeReportView != null)
      {
        // do something
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView.Refresh()
      #region Refresh the report view
      {
        if (reportView == null)
        {
          return;
        }

        reportView.Refresh();
      }
      #endregion
      // cref: ArcGIS.Desktop.Reports.ReportView.ZoomToWholePage()
      #region Zoom to whole page
      {
        await QueuedTask.Run(() => reportView.ZoomToWholePage());
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Reports.ReportSection
      // cref: ArcGIS.Desktop.Layouts.Element.GetBounds(System.Boolean)
      // cref: ArcGIS.Desktop.Reports.ReportView.ZoomTo(ArcGIS.Core.Geometry.Geometry)
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Desktop.Reports.Report.Elements
      // cref: ArcGIS.Desktop.Reports.ReportView.Active
      #region Zoom to specific location on Report view
      {
        await QueuedTask.Run(() =>
        {
          var detailsSection = report.Elements.OfType<ReportSection>().FirstOrDefault().Elements.OfType<ReportDetails>().FirstOrDefault();
          var bounds = detailsSection.GetBounds();
          ReportView.Active.ZoomTo(bounds);
        });
      }
      #endregion
      // cref: ArcGIS.Desktop.Reports.ReportView.ZoomToPageWidth()
      #region Zoom to page width
      {
        await QueuedTask.Run(() => reportView.ZoomToPageWidth());
      }
      #endregion
      #region ProSnippet Group: Create Report
      #endregion

      // cref: ArcGIS.Core.CIM.CIMReportField
      // cref: ArcGIS.Core.CIM.CIMTableField.Name
      // cref: ArcGIS.Core.CIM.CIMTableField.FieldOrder
      // cref: ArcGIS.Core.CIM.CIMTableField.Group
      // cref: ArcGIS.Core.CIM.CIMTableField.SortInfo
      // cref: ArcGIS.Core.CIM.FieldSortInfo
      // cref: ArcGIS.Desktop.Reports.ReportDataSource
      // cref: ArcGIS.Core.CIM.CIMPage
      // cref: ArcGIS.Core.CIM.CIMPage.Height
      // cref: ArcGIS.Core.CIM.CIMPage.StretchElements
      // cref: ArcGIS.Core.CIM.CIMPage.Width
      // cref: ArcGIS.Core.CIM.CIMPage.ShowRulers
      // cref: ArcGIS.Core.CIM.CIMPage.ShowGuides
      // cref: ArcGIS.Core.CIM.CIMPage.Margin
      // cref: ArcGIS.Core.CIM.CIMPage.Units
      // cref: ArcGIS.Core.CIM.CIMMargin
      // cref: ArcGIS.Core.CIM.CIMMargin.Bottom
      // cref: ArcGIS.Core.CIM.CIMMargin.Left
      // cref: ArcGIS.Core.CIM.CIMMargin.Right
      // cref: ArcGIS.Core.CIM.CIMMargin.Top
      // cref: ArcGIS.Core.Geometry.LinearUnit
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager.GetTemplatesAsync()
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager.GetStylingsAsync()
      // cref: ArcGIS.Desktop.Reports.ReportTemplate
      // cref: ArcGIS.Desktop.Reports.ReportTemplate.Name
      // cref: ArcGIS.Desktop.Reports.ReportFieldStatistic
      // cref: ArcGIS.Desktop.Reports.ReportFieldStatistic.Field
      // cref: ArcGIS.Desktop.Reports.ReportFieldStatistic.Statistic
      // cref: ArcGIS.Core.CIM.FieldStatisticsFlag
      // cref: ArcGIS.Desktop.Reports.ReportFactory
      // cref: ArcGIS.Desktop.Reports.IReportFactory.CreateReport(System.String,ArcGIS.Desktop.Reports.ReportDataSource,ArcGIS.Core.CIM.CIMPage,System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Reports.ReportFieldStatistic},ArcGIS.Desktop.Reports.ReportTemplate,System.String)
      // cref: ArcGIS.Desktop.Reports.ReportFactory.Instance
      #region Create report
      {
        await QueuedTask.Run(async () =>
        {
          //The fields in the datasource used for the report
          //This uses a US Cities dataset
          var listFields = new List<CIMReportField> {
                //Grouping should be the first field
                new CIMReportField{Name = "STATE_NAME", FieldOrder = 0, Group = true, SortInfo = FieldSortInfo.Desc}, //Group cities using STATES
                new CIMReportField{Name = "CITY_NAME", FieldOrder = 1},
                new CIMReportField{Name = "POP1990", FieldOrder = 2, },
            };
          //Definition query to use for the data source
          var defQuery = "STATE_NAME LIKE 'C%'";
          //Define the Datasource
          var reportDataSource = new ReportDataSource(featureLayer, defQuery, listFields);
          //The CIMPage defintion - page size, units, etc
          var cimReportPage = new CIMPage
          {
            Height = 11,
            StretchElements = false,
            Width = 6.5,
            ShowRulers = true,
            ShowGuides = true,
            Margin = new CIMMargin { Bottom = 1, Left = 1, Right = 1, Top = 1 },
            Units = LinearUnit.Inches
          };

          //Report template
          var reportTemplates = await ReportTemplateManager.GetTemplatesAsync();
          var reportTemplate = reportTemplates.Where(r => r.Name == "Attribute List with Grouping").First();

          //Report Styling
          var reportStyles = await ReportStylingManager.GetStylingsAsync();
          var reportStyle = reportStyles.Where(s => s == "Cool Tones").First();

          //Field Statistics
          var fieldStatisticsList = new List<ReportFieldStatistic> {
                new ReportFieldStatistic{ Field = "POP1990", Statistic = FieldStatisticsFlag.Sum}
                //Note: NoStatistics option for FieldStatisticsFlag is not supported.
            };
          var newReport = ReportFactory.Instance.CreateReport("USAReport", reportDataSource, cimReportPage, fieldStatisticsList, reportTemplate, reportStyle);
        });
      }
      #endregion

      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDefinition
      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDefinition.ReportTemplateDataSourceItems
      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDataSourceInfo
      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDataSourceInfo.SetReportDataSource(ArcGIS.Desktop.Mapping.MapMember)
      //cref: ArcGIS.Desktop.Reports.TemplateTokenFieldInfoValues
      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDefinition.CanCreateReport()
      //cref: ArcGIS.Desktop.Reports.IReportFactory.CreateReport(System.String,ArcGIS.Desktop.Reports.ReportCustomTemplateDefinition)
      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDataSourceInfo.DefinitionQuery
      //cref: ArcGIS.Desktop.Reports.ReportCustomTemplateDataSourceInfo.TokenInfoValues
      //cref: ArcGIS.Desktop.Reports.ReportFactory.Instance
      //cref: ArcGIS.Desktop.Reports.ReportTemplateManager.GetCustomTemplateDefinition(System.String)
      #region Create a report from a custom report template
      {
        await QueuedTask.Run(() =>
        {
          // get the first map in the project
          MapProjectItem item = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(n => n.Name == "Cities");
          if (item == null)
            return;

          Map map = item.GetMap();
          if (map == null)
            return;

          Layer cityLayer = map.FindLayers("U.S. Cities")[0];
          if (cityLayer == null)
            return;

          // load template information
          var templatePath = System.IO.Path.Combine($@"{Project.Current.HomeFolderPath}\TemplateSimpleReportWithTokens.rptt");
          ReportCustomTemplateDefinition templateDef = ReportTemplateManager.GetCustomTemplateDefinition(templatePath);
          if (templateDef == null || templateDef.ReportTemplateDataSourceItems?.Count() < 1)
            return;

          // get template info of first data source (parent) and set its data source
          ReportCustomTemplateDataSourceInfo parentDataSourceInfo = templateDef.ReportTemplateDataSourceItems.ElementAt(0);

          parentDataSourceInfo.SetReportDataSource(cityLayer);

          TemplateTokenFieldInfoValues templateTokenFieldInfoValues = parentDataSourceInfo.TokenInfoValues;

          parentDataSourceInfo.DefinitionQuery = "STATE_NAME = 'Massachusetts'";
          templateTokenFieldInfoValues["[text-field-value-0]"] = "CITY_NAME";
          templateTokenFieldInfoValues["[text-field-name-0]"] = "City";
          templateTokenFieldInfoValues["[numeric-field-value-0]"] = "POP1990";
          templateTokenFieldInfoValues["[numeric-field-name-0]"] = "Population";

          if (templateDef.CanCreateReport())
          {
            // create the report
            report = ReportFactory.Instance.CreateReport("RelateSiblingsFromCode", templateDef);
          }
        });
        //UI thread
        if (report != null)
        {
          //open report
          await FrameworkApplication.Panes.CreateReportPaneAsync(report);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportExportOptions
      // cref: ArcGIS.Desktop.Reports.ReportExportOptions.ExportPageOption
      // cref: ArcGIS.Desktop.Reports.ReportExportOptions.TotalPageNumberOverride
      // cref: ArcGIS.Desktop.Mapping.ExportPageOptions
      // cref: ArcGIS.Desktop.Mapping.PDFFormat
      // cref: ArcGIS.Desktop.Mapping.ExportFormat.Resolution
      // cref: ArcGIS.Desktop.Mapping.ExportFormat.OutputFileName
      // cref: ArcGIS.Desktop.Reports.Report.ExportToPDF(System.String,ArcGIS.Desktop.Mapping.PDFFormat,ArcGIS.Desktop.Reports.ReportExportOptions,System.Boolean)
      // cref: ArcGIS.Desktop.Reports.Report.Name
      #region Export report to pdf
      {
        await QueuedTask.Run(() =>
        {
          //Define Export Options
          var exportOptions = new ReportExportOptions
          {
            ExportPageOption = ExportPageOptions.ExportAllPages,
            TotalPageNumberOverride = 0

          };
          //Create PDF format with appropriate settings
          string path = $@"{Project.Current.HomeFolderPath}\ReportExport.pdf";
          PDFFormat pdfFormat = new PDFFormat();
          pdfFormat.Resolution = 300;
          pdfFormat.OutputFileName = path;
          report.ExportToPDF($"{report.Name}", pdfFormat, exportOptions, true);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Core.ItemFactory
      // cref: ArcGIS.Desktop.Core.IItemFactory.Create(System.String,ArcGIS.Desktop.Core.ItemFactory.ItemType)
      // cref: ArcGIS.Desktop.Core.IProjectItem
      #region Import a report file
      {
        var reportFilePath = @"c:\temp\Report.rptx";
        await QueuedTask.Run(() =>
        {
          Item reportToImport = ItemFactory.Instance.Create(reportFilePath);
          Project.Current.AddItem(reportToImport as IProjectItem);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportProjectItem
      // cref: ArcGIS.Desktop.Core.Project.GetItems``1()
      // cref: ArcGIS.Desktop.Core.Project.RemoveItem(ArcGIS.Desktop.Core.IProjectItem)
      #region Delete a report
      {
        await QueuedTask.Run(() =>
        {
          //Reference a ReportProjectItem in a project by name
          ReportProjectItem reportItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals(reportName));
          if (reportItem != null)
          {
            //Delete the report from the project
            Project.Current.RemoveItem(reportItem);
          }
        });
      }
      #endregion

      #region ProSnippet Group: Modify Reports
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.SetName(System.String)
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport()
      #region Rename Report
      {
        await QueuedTask.Run(() =>
        {
          ReportProjectItem reportProjItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(item => item.Name.Equals("Report Name"));
          reportProjItem.GetReport().SetName("RenamedReport");
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.AddGroup(System.String,System.Boolean,System.Boolean,System.String)
      // cref: ArcGIS.Desktop.Reports.Report.SetDefinitionQuery(System.String)
      // cref: ArcGIS.Desktop.Reports.Report.RemoveGroups(System.Collections.Generic.IEnumerable{System.String})
      #region Modify the Report DefinitionQuery
      {
        await QueuedTask.Run(() =>
        {
          //Remove Groups
          // The fields in the datasource used for the report
          var listFields = new List<string> {
             "STATE_NAME"
            };
          report.RemoveGroups(listFields);

          //Add Group
          report.AddGroup("STATE_NAME", true, true, "");

          //Modify the Definition Query
          var defQuery = "STATE_NAME LIKE 'C%'";
          report.SetDefinitionQuery(defQuery);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.SetPage(ArcGIS.Core.CIM.CIMPage)
      // cref: ArcGIS.Desktop.Reports.Report.SetPageHeight(System.Double)
      // cref: ArcGIS.Core.CIM.CIMPage
      // cref: ArcGIS.Core.CIM.CIMPage.Height
      // cref: ArcGIS.Core.CIM.CIMPage.StretchElements
      // cref: ArcGIS.Core.CIM.CIMPage.Width
      // cref: ArcGIS.Core.CIM.CIMPage.ShowRulers
      // cref: ArcGIS.Core.CIM.CIMPage.ShowGuides
      // cref: ArcGIS.Core.CIM.CIMPage.Margin
      // cref: ArcGIS.Core.CIM.CIMPage.Units
      // cref: ArcGIS.Core.CIM.CIMMargin
      // cref: ArcGIS.Core.CIM.CIMMargin.Bottom
      // cref: ArcGIS.Core.CIM.CIMMargin.Left
      // cref: ArcGIS.Core.CIM.CIMMargin.Right
      // cref: ArcGIS.Core.CIM.CIMMargin.Top
      #region Modify the report Page
      {
        await QueuedTask.Run(() =>
        {
          var cimReportPage = new CIMPage
          {
            Height = 12,
            StretchElements = false,
            Width = 6.5,
            ShowRulers = true,
            ShowGuides = true,
            Margin = new CIMMargin { Bottom = 1, Left = 1, Right = 1, Top = 1 },
            Units = LinearUnit.Inches
          };
          report.SetPage(cimReportPage);
          //Change only the report's page height
          report.SetPageHeight(12);
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.AddSubReport(ArcGIS.Desktop.Core.Item,System.Int32,System.Boolean)
      // cref: ArcGIS.Desktop.Reports.ReportProjectItem.GetReport()
      #region Add SubReport
      {
        await QueuedTask.Run(() =>
        {
          var mainReport = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(r => r.Name == "USAReports")?.GetReport();

          if (mainReport == null)
            return;

          //Add sub report
          var vermontReportItem = Project.Current.GetItems<ReportProjectItem>().FirstOrDefault(r => r.Name == "Vermont");
          if (vermontReportItem == null)
            return;

          Report vermontReport = vermontReportItem.GetReport();
          mainReport.AddSubReport(vermontReportItem, -1, true); //  If -1, the subreport is added to the end of the report.
        });
      }
      #endregion

      #region ProSnippet Group: Report Design
      #endregion


      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager
      // cref: ArcGIS.Desktop.Reports.ReportTemplateManager.GetTemplatesAsync()
      // cref: ArcGIS.Desktop.Reports.ReportTemplate.Name
      #region Get a report template
      {
        //Report Template Styles:
        //Attribute List
        //Attribute List with Grouping
        //Basic Summary
        //Basic Summary with Grouping
        //Page Per Feature
        var reportTemplates = await ReportTemplateManager.GetTemplatesAsync();
        var reportTemplate = reportTemplates.Where(r => r.Name == "Report Template Name").First();
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportStylingManager
      // cref: ArcGIS.Desktop.Reports.ReportStylingManager.GetStylingsAsync()
      #region Get a report styling
      {
        //Report Styling:
        //Black and White
        //Cool Tones
        //Warm Tones
        var reportStyles = await ReportStylingManager.GetStylingsAsync();
        var reportStyle = reportStyles.Where(s => s == "Report style name").First();
      }
      #endregion

      #region ProSnippet Group: Report Elements
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportSection
      // cref: ArcGIS.Desktop.Reports.ReportHeader
      // cref: ArcGIS.Desktop.Reports.ReportPageHeader
      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Reports.ReportPageFooter
      // cref: ArcGIS.Desktop.Reports.ReportFooter
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Desktop.Reports.Report.Elements
      #region Get various Report sections
      {
        //Get the "ReportSection element"
        //ReportSectionElement contains the ReportHeader, ReportPageHeader, ReportDetails. ReportPageFooter, ReportFooter sections.
        var mainReportSection = report.Elements.OfType<ReportSection>().FirstOrDefault();

        //Get the ReportHeader
        var reportHeader = mainReportSection?.Elements.OfType<ReportHeader>().FirstOrDefault();

        //Get the ReportHeader
        var reportPageHeader = mainReportSection?.Elements.OfType<ReportPageHeader>().FirstOrDefault();

        //Get the "ReportDetails" within the ReportSectionElement. ReportDetails is where "fields" are.
        var reportDetailsSection = mainReportSection?.Elements.OfType<ReportDetails>().FirstOrDefault();

        //Get the ReportPageFooter
        var reportPageFooter = mainReportSection?.Elements.OfType<ReportPageFooter>().FirstOrDefault();

        //Get the ReportFooter
        var reportFooter = mainReportSection?.Elements.OfType<ReportFooter>().FirstOrDefault();
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Desktop.Reports.ReportSectionElement.SelectElements(System.Collections.Generic.IReadOnlyList{ArcGIS.Desktop.Layouts.Element})
      #region Select elements
      {
        //Get the "ReportSection element"
        //ReportSectionElement contains the ReportHeader, ReportPageHeader, ReportDetails. ReportPageFooter, ReportFooter sections.
        var mainReportSection = report.Elements.OfType<ReportSection>().FirstOrDefault();

        //ReportDetailsSection contains the "Fields"
        //Get the "ReportDetails" within the ReportSectionElement. ReportDetails is where "fields" are.
        var reportDetailsSection = mainReportSection?.Elements.OfType<ReportDetails>().FirstOrDefault();

        var elements = reportDetailsSection.Elements;
        reportDetailsSection.SelectElements(elements);
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportPageFooter
      // cref: ArcGIS.Desktop.Reports.ReportSectionElement.SelectAllElements()
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Desktop.Reports.Report.Elements
      #region Select all elements
      {
        //Select all elements in the Report Footer.
        ReportPageFooter pageFooterSection = report.Elements.OfType<ReportSection>().FirstOrDefault().Elements.OfType<ReportPageFooter>().FirstOrDefault();
        pageFooterSection.SelectAllElements();
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView.GetSelectedElements()
      // cref: ArcGIS.Desktop.Reports.Report.GetSelectedElements()
      // cref: ArcGIS.Desktop.Reports.ReportView.Active
      #region Get selected elements
      {
        IReadOnlyList<Element> selectedElements = report.GetSelectedElements();
        //Can also use the active ReportView
        IReadOnlyList<Element> selectedElementsFromView = ReportView.Active.GetSelectedElements();
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.ReportView.ZoomToSelectedElements()
      #region Zoom to selected elements
      {
        await QueuedTask.Run(() => reportView.ZoomToSelectedElements());
      }
      #endregion Zoom to selected elements

      // cref: ArcGIS.Desktop.Reports.ReportView.ClearElementSelection()
      #region Clear element selection
      {
        reportView.ClearElementSelection();
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.FindElements(System.Collections.Generic.IEnumerable{System.String})
      #region Find specific elements in the report based on their Name.
      {
        var reportElementsToFind = new List<string> { "ReportText1", "ReportText2" };
        var textReportElements = report.FindElements(reportElementsToFind);
      }
      #endregion

      // cref: ArcGIS.Desktop.Reports.Report.DeleteElements(System.Collections.Generic.IEnumerable{ArcGIS.Desktop.Layouts.Element})
      // cref: ArcGIS.Desktop.Reports.Report.FindElements(System.Collections.Generic.IEnumerable{System.String})
      #region Delete Elements
      {
        await QueuedTask.Run(() =>
        {
          var reportElementsToFind = new List<string> { "ReportText1", "ReportText2" };
          var textReportElements = report.FindElements(reportElementsToFind);
          report.DeleteElements(textReportElements);
        });
      }
      #endregion
      // cref: ArcGIS.Desktop.Reports.ReportSection
      // cref: ArcGIS.Desktop.Reports.ReportDetails
      // cref: ArcGIS.Desktop.Layouts.GroupElement.Elements
      // cref: ArcGIS.Core.CIM.CIMReportField
      // cref: ArcGIS.Core.CIM.CIMTableField.Name
      // cref: ArcGIS.Core.CIM.CIMTableField.FieldOrder
      // cref: ArcGIS.Desktop.Layouts.GraphicElement.GetGraphic()
      // cref: ArcGIS.Core.CIM.CIMParagraphTextGraphic
      // cref: ArcGIS.Desktop.Layouts.Element.GetBounds(System.Boolean)
      // cref: ArcGIS.Desktop.Reports.IReportElementFactory.CreateFieldValueTextElement(ArcGIS.Desktop.Mapping.IElementContainer,ArcGIS.Core.Geometry.Envelope,ArcGIS.Core.CIM.CIMReportField,ArcGIS.Core.CIM.CIMTextSymbol)
      // cref: ArcGIS.Desktop.Reports.ReportElementFactory.Instance
      #region Create a new field in the report
      {
        //This is the gap between two fields
        double fieldIncrement = 0.9388875113593206276389;
        await QueuedTask.Run(() =>
        {
          //New field to add.
          var newReportField = new CIMReportField
          {
            Name = "POP1990",
            FieldOrder = 2,
          };
          //Get the "ReportSection element"
          var mainReportSection = report.Elements.OfType<ReportSection>().FirstOrDefault();
          if (mainReportSection == null)
            return;

          //Get the "ReportDetails" within the ReportSectionElement. ReportDetails is where "fields" are.
          var reportDetailsSection = mainReportSection?.Elements.OfType<ReportDetails>().FirstOrDefault();
          if (reportDetailsSection == null)
            return;

          //Within ReportDetails find the envelope that encloses a field.
          //We get the first CIMParagraphTextGraphic in the collection so that we can add the new field next to it.
          var lastFieldGraphic = reportDetailsSection.Elements.FirstOrDefault((r) =>
          {
            var gr = r as GraphicElement;
            if (gr == null)
            {
              return false;
            }

            return (gr.GetGraphic() is CIMParagraphTextGraphic ? true : false);
          });
          //Get the Envelope of the last field
          var graphicBounds = lastFieldGraphic.GetBounds();

          //Min and Max values of the envelope
          var xMinOfFieldEnvelope = graphicBounds.XMin;
          var yMinOfFieldEnvelope = graphicBounds.YMin;

          var xMaxOfFieldEnvelope = graphicBounds.XMax;
          var YMaxOfFieldEnvelope = graphicBounds.YMax;
          //create the new Envelope to be offset from the existing field

          MapPoint newMinPoint = MapPointBuilderEx.CreateMapPoint(xMinOfFieldEnvelope + fieldIncrement, yMinOfFieldEnvelope);
          MapPoint newMaxPoint = MapPointBuilderEx.CreateMapPoint(xMaxOfFieldEnvelope + fieldIncrement, YMaxOfFieldEnvelope);
          Envelope newFieldEnvelope = EnvelopeBuilderEx.CreateEnvelope(newMinPoint, newMaxPoint);

          //Create field
          GraphicElement fieldGraphic = ReportElementFactory.Instance.CreateFieldValueTextElement(reportDetailsSection, newFieldEnvelope, newReportField);
        });
      }
      #endregion
      #region ProSnippet Group: Raster and Imagery Options
      #endregion

      // cref: ArcGIS.Desktop.Core.ApplicationOptions.ReportOptions
      // cref: ArcGIS.Desktop.Core.ReportOptions
      #region Get/Set Report Options
      {
        //toggle/switch option values
        var options = ApplicationOptions.ReportOptions;

        options.PreviewAllPages = false;
        options.NumberOfPreviewPages = 25;

        options.ReportCustomTemplatePath = @"c:\temp";
      }
      #endregion
    }
  }
}
