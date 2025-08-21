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
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.ProSnippets
{
  public static class ProSnippetsMetadata
  {
    #region ProSnippet Group: Metadata
    #endregion

    // cref: Item: Get its IMetadata interface;ArcGIS.Desktop.Core.IMetadata
    #region Item: Get its IMetadata interface
    /// <summary>
    /// Retrieves the metadata interface for a geodatabase item, allowing access to its metadata properties and methods.
    /// </summary>
    public static void GetMetadataInterface()
    {
      Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");
      IMetadata gdbMetadataItem = gdbItem as IMetadata;
    }
    #endregion

    // cref: Item: Get an item's metadata: GetXML;ArcGIS.Desktop.Core.IMetadata.GetXml
    // cref: Item: Get an item's metadata: GetXML;ArcGIS.Desktop.Core.Item.GetXml
    #region Item: Get an item's metadata: GetXML
    /// <summary>
    /// Retrieves the metadata of a geodatabase item as an XML string asynchronously.
    /// </summary>
    public static async Task GetMetadataXml(IMetadata gdbMetadataItem)
    {
      string gdbXMLMetadataXmlAsString = string.Empty;
      gdbXMLMetadataXmlAsString = await QueuedTask.Run(() => gdbMetadataItem.GetXml());
      //check metadata was returned
      if (!string.IsNullOrEmpty(gdbXMLMetadataXmlAsString))
      {
        //use the metadata
      }
    }
    #endregion

    // cref: Item: Set the metadata of an item: SetXML;ArcGIS.Desktop.Core.IMetadata.SetXml(System.String)
    // cref: Item: Set the metadata of an item: SetXML;ArcGIS.Desktop.Core.Item.SetXml(System.String)
    // cref: Item: Set the metadata of an item: SetXML;ArcGIS.Desktop.Core.Project.SetXml(System.String)
    // cref: Item: Set the metadata of an item: SetXML;ARCGIS.DESKTOP.CORE.ITEM.CANEDIT
    #region Item: Set the metadata of an item: SetXML
    /// <summary>
    /// Sets the metadata of a geodatabase item using an XML string, after verifying that the metadata can be edited.
    /// </summary>
    public static async Task SetMetadataXML(IMetadata featureClassMetadataItem)
    {
      await QueuedTask.Run(() =>
      {
        var xmlMetadata = System.IO.File.ReadAllText(@"E:\Data\Metadata\MetadataForFeatClass.xml");
        //Will throw InvalidOperationException if the metadata cannot be changed
        //so check "CanEdit" first
        if (featureClassMetadataItem.CanEdit())
          featureClassMetadataItem.SetXml(xmlMetadata);
      });
    }

    #endregion

    // cref: Item: Check the metadata can be edited: CanEdit;ArcGIS.Desktop.Core.IMetadata.CanEdit
    // cref: Item: Check the metadata can be edited: CanEdit;ArcGIS.Desktop.Core.Item.CanEdit
    // cref: Item: Check the metadata can be edited: CanEdit;ArcGIS.Desktop.Core.Project.CanEdit
    #region Item: Check the metadata can be edited: CanEdit
    /// <summary>
    /// Checks if the metadata of a geodatabase item can be edited.
    /// </summary>
    public static async Task CheckMetadataCanBeEdited(IMetadata metadataItemToCheck)
    {
      bool canEdit1;
      //Call CanEdit before calling SetXml
      await QueuedTask.Run(() => canEdit1 = metadataItemToCheck.CanEdit());

    }
    #endregion

    // cref: Item: Updates metadata with the current properties of the item: Synchronize;ArcGIS.Desktop.Core.IMetadata.Synchronize
    // cref: Item: Updates metadata with the current properties of the item: Synchronize;ArcGIS.Desktop.Core.Item.Synchronize
    // cref: Item: Updates metadata with the current properties of the item: Synchronize;ArcGIS.Desktop.Core.Project.Synchronize
    #region Item: Updates metadata with the current properties of the item: Synchronize
    /// <summary>
    /// Synchronizes the metadata of a geodatabase item with its current properties and retrieves the updated metadata as an XML string.
    /// </summary>
    public static async Task SyncMetadateXml(IMetadata metadataItemToSync)
    {
      string syncedMetadataXml = string.Empty;
      await QueuedTask.Run(() => syncedMetadataXml = metadataItemToSync.Synchronize());
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.IMetadata.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
    // cref: ArcGIS.Desktop.Core.Item.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
    // cref: ArcGIS.Desktop.Core.Project.CopyMetadataFromItem(ArcGIS.Desktop.Core.Item)
    // cref: ARCGIS.DESKTOP.CORE.ITEMFACTORY.CREATE
    #region Item: Copy metadata from the source item's metadata: CopyMetadataFromItem
    /// <summary>
    /// Copies metadata from a source item to the specified metadata item.
    /// </summary>
    public static async Task CopyMetadataFromItem(IMetadata metadataCopyFrom)
    {
      Item featureClassItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb\SourceFeatureClass");
      await QueuedTask.Run(() => metadataCopyFrom.CopyMetadataFromItem(featureClassItem));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDDeleteContentOption
    // cref: ArcGIS.Desktop.Core.IMetadata.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
    // cref: ArcGIS.Desktop.Core.Item.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
    // cref: ArcGIS.Desktop.Core.Project.DeleteMetadataContent(ArcGIS.Desktop.Core.MDDeleteContentOption)
    // cref: ARCGIS.DESKTOP.CORE.ITEMFACTORY.CREATE
    #region Item: Delete certain content from the metadata of the current item: DeleteMetadataContent
    /// <summary>
    /// Deletes specific content, such as thumbnails, from the metadata of a geodatabase item.
    /// </summary>
    public static async Task DeleteMetadataContent()
    {
      Item featureClassWithMetadataItem = ItemFactory.Instance.Create(@"C:\projectBeta\GDBs\regionFive.gdb\SourceFeatureClass");
      //Delete thumbnail content from item's metadata
      await QueuedTask.Run(() => featureClassWithMetadataItem.DeleteMetadataContent(MDDeleteContentOption.esriMDDeleteThumbnail));
    }
    #endregion

    //Item gdbItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\regionFive.gdb");

    // cref: ArcGIS.Desktop.Core.MDImportExportOption
    // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
    // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
    // cref: ArcGIS.Desktop.Core.Item.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
    // cref: ArcGIS.Desktop.Core.Item.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
    // cref: ArcGIS.Desktop.Core.Project.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption)
    // cref: ArcGIS.Desktop.Core.Project.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
    #region Item: Updates metadata with the imported metadata - the input path can be the path to an item with metadata, or a URI to a XML file: ImportMetadata
    /// <summary>
    /// Imports metadata into a geodatabase item from a specified path or URI, using the current metadata style.
    /// </summary>
    public static async Task ImportMetadata(IMetadata metadataItemImport)
    {
      // the input path can be the path to an item with metadata, or a URI to an XML file
      await QueuedTask.Run(() => metadataItemImport.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCurrentMetadataStyle));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDImportExportOption
    // cref: ArcGIS.Desktop.Core.IMetadata.ImportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,System.String)
    #region Item: Updates metadata with the imported metadata: ImportMetadata
    /// <summary>
    /// Imports metadata into a geodatabase item from a specified path or URI, using a custom XSLT stylesheet.
    /// </summary>
    public static async Task ImportMetadataWithCustomStyleSheet(IMetadata metadataItemImport)
    {
      // the input path can be the path to an item with metadata, or a URI to an XML file
      await QueuedTask.Run(() => metadataItemImport.ImportMetadata(@"E:\YellowStone.gdb\MyDataset\MyFeatureClass", MDImportExportOption.esriCustomizedStyleSheet, @"E:\StyleSheets\Import\MyImportStyleSheet.xslt"));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDImportExportOption
    // cref: ArcGIS.Desktop.Core.MDExportRemovalOption
    // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
    // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
    // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
    // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
    // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
    // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
    #region Item: export the metadata of the currently selected item: ExportMetadata
    /// <summary>
    /// Exports the metadata of a geodatabase item to a specified file path, using the current metadata style and exact copy options.
    /// </summary>
    public static async Task ExportMetadata(IMetadata metadataItemExport)
    {
      await QueuedTask.Run(() => metadataItemExport.ExportMetadata(@"E:\Temp\OutputXML.xml", MDImportExportOption.esriCurrentMetadataStyle, MDExportRemovalOption.esriExportExactCopy));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDImportExportOption
    // cref: ArcGIS.Desktop.Core.MDExportRemovalOption
    // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
    // cref: ArcGIS.Desktop.Core.IMetadata.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
    // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
    // cref: ArcGIS.Desktop.Core.Item.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
    // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption)
    // cref: ArcGIS.Desktop.Core.Project.ExportMetadata(System.String,ArcGIS.Desktop.Core.MDImportExportOption,ArcGIS.Desktop.Core.MDExportRemovalOption,System.String)
    #region Item: export the metadata of the currently selected item: ExportMetadata
    /// <summary>
    /// Exports the metadata of a geodatabase item to a specified file path, using a custom XSLT stylesheet and exact copy options.
    /// </summary>
    public static async Task ExportMetadataWithCustomStyleSheet(IMetadata metadataItemExport)
    {
      await QueuedTask.Run(() => metadataItemExport.ExportMetadata(@"E:\Temp\OutputXML.xml", MDImportExportOption.esriCustomizedStyleSheet, MDExportRemovalOption.esriExportExactCopy, @"E:\StyleSheets\Export\MyExportStyleSheet.xslt"));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDSaveAsXMLOption
    // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
    // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
    // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsXML(System.String,ArcGIS.Desktop.Core.MDSaveAsXMLOption)
    #region Item: Save the metadata of the current item as XML: SaveMetadataAsXML
    /// <summary>
    /// Saves the metadata of a geodatabase item as an XML file at the specified file path, using the exact copy option.
    /// </summary>
    public static async Task SaveMetadataAsXML(IMetadata metadataItemToSaveAsXML)
    {
      await QueuedTask.Run(() => metadataItemToSaveAsXML.SaveMetadataAsXML(@"E:\Temp\OutputXML.xml", MDSaveAsXMLOption.esriExactCopy));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDSaveAsHTMLOption
    // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
    // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
    // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsHTML(System.String,ArcGIS.Desktop.Core.MDSaveAsHTMLOption)
    #region Item: Save the metadata of the current item as HTML: SaveMetadataAsHTML
    /// <summary>
    /// Saves the metadata of a geodatabase item as an HTML file at the specified file path, using the current metadata style.
    /// </summary>
    public static async Task SaveMetadataAsHtml(IMetadata metadataItemToSaveAsHTML)
    {
      await QueuedTask.Run(() => metadataItemToSaveAsHTML.SaveMetadataAsHTML(@"E:\Temp\OutputHTML.htm", MDSaveAsHTMLOption.esriCurrentMetadataStyle));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.IMetadata.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
    // cref: ArcGIS.Desktop.Core.Item.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
    // cref: ArcGIS.Desktop.Core.Project.SaveMetadataAsUsingCustomXSLT(System.String,System.String)
    #region Item: Save the metadata of the current item using customized XSLT: SaveMetadataAsUsingCustomXSLT
    /// <summary>
    /// Saves the metadata of a geodatabase item as an XML file at the specified file path, using a custom XSLT stylesheet.
    /// </summary>
    public static async Task SaveMetadataUsingCustomStyleSheet(IMetadata metadataItemToSaveAsUsingCustomXSLT)
    {
      await QueuedTask.Run(() => metadataItemToSaveAsUsingCustomXSLT.SaveMetadataAsUsingCustomXSLT(@"E:\Data\Metadata\CustomXSLT.xsl", @"E:\Temp\OutputXMLCustom.xml"));
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.MDUpgradeOption
    // cref: ArcGIS.Desktop.Core.IMetadata.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
    // cref: ArcGIS.Desktop.Core.Item.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
    // cref: ArcGIS.Desktop.Core.Project.UpgradeMetadata(ArcGIS.Desktop.Core.MDUpgradeOption)
    #region Item: Upgrade the metadata of the current item: UpgradeMetadata
    /// <summary>
    /// Upgrades the metadata of a geodatabase item to a specified metadata standard, such as FGDC CSDGM.
    /// </summary>
    public static async Task UpgradeMetadata(IMetadata metadataItemToSaveAsUsingCustomXSLT)
    {
      var fgdcItem = ItemFactory.Instance.Create(@"C:\projectAlpha\GDBs\testData.gdb");
      await QueuedTask.Run(() => fgdcItem.UpgradeMetadata(MDUpgradeOption.esriUpgradeFgdcCsdgm));
    }
    #endregion

  }
}
