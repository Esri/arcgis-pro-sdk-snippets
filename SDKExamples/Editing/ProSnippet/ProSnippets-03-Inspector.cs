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
using System.Linq;
using System.Threading.Tasks;

namespace ProSnippets.EditingSnippets
{
  public static partial class ProSnippetsEditing
  {
    #region ProSnippet Group: Inspector
    #endregion

    /// <summary>
    /// Demonstrates various operations using the <see cref="Inspector"/> class,
    /// including loading features, retrieving and updating attributes, and schema inspection.
    /// </summary>
    public static async Task InspectorEditing()
    {

      #region ignore - Variable initialization

      var activeMap = MapView.Active.Map;
      var featureLayer = activeMap.Layers.OfType<FeatureLayer>().FirstOrDefault();
      var objectId = 1; // example ObjectID of the feature to load
      var selectedFeatures = activeMap.GetSelection();
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.#ctor
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.LoadAsync(ArcGIS.Desktop.Mapping.MapMember, System.Int64)
      #region Load a feature from a layer into the inspector
      {
        // Loads a feature from the specified feature layer into an Inspector instance.
        // create an instance of the inspector class
        var inspector = new Inspector();
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
      {
        // Loads the selected features into an Inspector instance.
        // get the first layer and its corresponding selected feature OIDs
        var firstSelectionSet = selectedFeatures.ToDictionary().First();
        // create an instance of the inspector class
        var inspector = new Inspector();
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
      // Retrieves the value of a specific attribute and the geometry of the first selected feature from the provided selection set.
      await QueuedTask.Run(() =>
      {
        // get the first layer and its corresponding selected feature OIDs
        var firstSelectionSet = selectedFeatures.ToDictionary().First();

        // create an instance of the inspector class
        var inspector = new Inspector();

        // load the selected features into the inspector using a list of object IDs
        inspector.Load(firstSelectionSet.Key, firstSelectionSet.Value);

        //get the value of
        var pscode = inspector["STATE_NAME"];
        var myGeometry = inspector.Shape;
      });
      #endregion

      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.Item(System.String)
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.ApplyAsync()
      #region Load map selection into Inspector and Change Attributes
      // Updates the "Description" attribute of the selected features in the specified selection set.
      {
        // get the first layer and its corresponding selected feature OIDs
        var firstSelectionSet = selectedFeatures.ToDictionary().First();
        // create an instance of the inspector class
        var inspector = new Inspector();
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
      // Loads the schema of the specified feature layer and iterates through its attributes,  providing access to their properties such as field name, alias, type, and other metadata.
      await QueuedTask.Run(() =>
      {
        // create an instance of the inspector class
        var inspector = new Inspector();
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
      #endregion
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create(System.String,ArcGIS.Desktop.Editing.Attributes.Severity)
      // cref: ArcGIS.Desktop.Editing.Attributes.Attribute.AddValidate
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector.#ctor
      // cref: ArcGIS.Desktop.Editing.Attributes.Inspector
      #region Inspector.AddValidate
      // Adds a custom validation rule to the "Mineral" field of the specified feature layer.
      await QueuedTask.Run(() =>
      {
        var insp = new Inspector();
        insp.LoadSchema(featureLayer);
        var attrib = insp.Where(a => a.FieldName == "Mineral").First();
        attrib.AddValidate(() =>
        {
          if (attrib.CurrentValue.ToString() == "Salt")
            return [];
          else return [ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Error", Severity.Low)];
        });
      });
      #endregion
    }
  }
}
