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
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editing.ProSnippets
{
  public static class ProSnippetsInspector
  {
    #region ProSnippet Group: Inspector
    #endregion

    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.#ctor
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadAsync(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    #region Load a feature from a layer into the inspector
    /// <summary>
    /// Loads a feature from the specified feature layer into an <see
    /// cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/> instance.
    /// </summary>
    /// <remarks>This method creates a new instance of the <see
    /// cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/> class  and asynchronously loads the feature identified by
    /// the specified <paramref name="objectId"/> from the given  <paramref name="featureLayer"/>. The loaded feature
    /// can then be inspected or edited using the inspector.</remarks>
    /// <param name="featureLayer">The <see cref="FeatureLayer"/> containing the feature to load.</param>
    /// <param name="objectId">The ObjectID of the feature to load into the inspector.</param>
    public static async void LoadFeatureIntoInspector(FeatureLayer featureLayer, long objectId)
    {
      // create an instance of the inspector class
      var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      // load the feature with ObjectID 'objectId' into the inspector
      await inspector.LoadAsync(featureLayer, objectId);
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadAsync(MAPMEMBER,INT64)
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1()
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary()
    // cref: ArcGIS.Desktop.Mapping.Map.GetSelection
    #region Load map selection into Inspector
    /// <summary>
    /// Loads the selected features from the specified <see cref="SelectionSet"/> into an <see
    /// cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/>.
    /// </summary>
    /// <remarks>This method initializes an <see cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/> instance
    /// and loads the selected features  from the first layer in the provided <paramref name="selectedFeatures"/>.  The
    /// method uses the object IDs of the selected features for the loading process.</remarks>
    /// <param name="selectedFeatures">A <see cref="SelectionSet"/> containing the selected features to be loaded.  The first layer and its
    /// corresponding selected feature object IDs will be used.</param>
    public static async void LoadSelectionSetIntoInspector(SelectionSet selectedFeatures)
    {
      // get the first layer and its corresponding selected feature OIDs
      var firstSelectionSet = selectedFeatures.ToDictionary().First();

      // create an instance of the inspector class
      var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      // load the selected features into the inspector using a list of object IDs
      await inspector.LoadAsync(firstSelectionSet.Key, firstSelectionSet.Value);
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Load(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Shape
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary``1()
    // cref: ArcGIS.Desktop.Mapping.MapMemberIDSet.ToDictionary()
    // cref: ArcGIS.Desktop.Mapping.Map.GetSelection
    #region Get selected feature's attribute value
    /// <summary>
    /// Retrieves the value of a specific attribute and the geometry of the first selected feature from the provided
    /// selection set.
    /// </summary>
    /// <remarks>This method uses the <see cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/> class to load
    /// the attributes and geometry of the first selected feature in the provided selection set. The attribute value for
    /// the field "STATE_NAME" and the geometry of the feature are retrieved.</remarks>
    /// <param name="selectedFeatures">A <see cref="SelectionSet"/> containing the selected features from which the attribute value and geometry will
    /// be retrieved. Must contain at least one selected feature.</param>
    public static void InspectorGetAttributeValue(SelectionSet selectedFeatures)
    {
      QueuedTask.Run(() =>
      {
        // get the first layer and its corresponding selected feature OIDs
        var firstSelectionSet = selectedFeatures.ToDictionary().First();

        // create an instance of the inspector class
        var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();

        // load the selected features into the inspector using a list of object IDs
        inspector.Load(firstSelectionSet.Key, firstSelectionSet.Value);

        //get the value of
        var pscode = inspector["STATE_NAME"];
        var myGeometry = inspector.Shape;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Item(System.String)
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.ApplyAsync()
    #region Load map selection into Inspector and Change Attributes
    /// <summary>
    /// Updates the "Description" attribute of the selected features in the specified selection set.
    /// </summary>
    /// <remarks>This method uses the <see cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/> class to load
    /// the selected  features, update their "Description" attribute to a new value, and apply the changes as an edit
    /// operation.  If multiple features are loaded, the change is applied to all of them. <para> Note that this method
    /// is asynchronous but returns void, so any exceptions or errors during execution  will not be propagated to the
    /// caller. Use with caution in production scenarios. </para></remarks>
    /// <param name="selectedFeatures">A <see cref="SelectionSet"/> containing the selected features to be updated. The first layer and its 
    /// corresponding selected feature object IDs will be used for the operation.</param>
    public static async void InspectorChangeAttributes(SelectionSet selectedFeatures)
    {
      // get the first layer and its corresponding selected feature OIDs
      var firstSelectionSet = selectedFeatures.ToDictionary().First();

      // create an instance of the inspector class
      var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();
      // load the selected features into the inspector using a list of object IDs
      await inspector.LoadAsync(firstSelectionSet.Key, firstSelectionSet.Value);

      // assign the new attribute value to the field "Description"
      // if more than one features are loaded, the change applies to all features
      inspector["Description"] = "The new value.";
      // apply the changes as an edit operation 
      await inspector.ApplyAsync();
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadSchema(ArcGIS.Desktop.Mapping.MapMember)
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.GetEnumerator()
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.FieldName
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.FieldAlias
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.FieldType
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsNullable
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsEditable
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsVisible
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsSystemField
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.IsGeometryField
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.GetField()
    #region Get a layers schema using Inspector
    /// <summary>
    /// Loads the schema of the specified feature layer and iterates through its attributes,  providing access to their
    /// properties such as field name, alias, type, and other metadata.
    /// </summary>
    /// <remarks>This method uses the <see cref="ArcGIS.Desktop.Editing.Attributes.Inspector"/> class to load
    /// the schema of the  provided feature layer. It allows access to attribute properties such as field name, alias,
    /// type, index,  and metadata flags (e.g., whether the field is nullable, editable, visible, or a system/geometry
    /// field).  The operation is performed on a background thread using <see cref="QueuedTask.Run(Action)"/> to ensure
    /// thread safety  when interacting with ArcGIS Pro's API.</remarks>
    /// <param name="featureLayer">The <see cref="FeatureLayer"/> whose schema will be loaded and inspected.</param>
    public static void SchemaAttributes(FeatureLayer featureLayer)
    {
      QueuedTask.Run(() =>
      {
        // create an instance of the inspector class
        var inspector = new ArcGIS.Desktop.Editing.Attributes.Inspector();

        // load the layer
        inspector.LoadSchema(featureLayer);

        // iterate through the attributes, looking at properties
        foreach (var attribute in inspector)
        {
          var fldName = attribute.FieldName;
          var fldAlias = attribute.FieldAlias;
          var fldType = attribute.FieldType;
          int idxFld = attribute.FieldIndex;
          var fld = attribute.GetField();
          var isNullable = attribute.IsNullable;
          var isEditable = attribute.IsEditable;
          var isVisible = attribute.IsVisible;
          var isSystemField = attribute.IsSystemField;
          var isGeometryField = attribute.IsGeometryField;
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create(System.String,ArcGIS.Desktop.Editing.Attributes.Severity)
    // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.AddValidate
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.#ctor
    // cref: ArcGIS.Desktop.Editing.Attributes.Inspector
    #region Inspector.AddValidate
    /// <summary>
    /// Adds a custom validation rule to the "Mineral" field of the specified feature layer.
    /// </summary>
    /// <remarks>This method attaches a validation rule to the "Mineral" field so that only the value "Salt"
    /// passes validation; all other values result in a low-severity validation error. The validation is added using the
    /// attribute inspector for the provided feature layer.</remarks>
    /// <param name="featureLayer">The <see cref="FeatureLayer"/> to which the field validation will be applied. Must not be <c>null</c>.</param>
    public static void AddFieldValidation(FeatureLayer featureLayer)
    {
      var insp = new Inspector();
      insp.LoadSchema(featureLayer);
      var attrib = insp.Where(a => a.FieldName == "Mineral").First();

      attrib.AddValidate(() =>
      {
        if (attrib.CurrentValue.ToString() == "Salt")
          return Enumerable.Empty<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError>();
        else return new[] { ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Error", ArcGIS.Desktop.Editing.Attributes.Severity.Low) };
      });
    }
    #endregion
  }
}
