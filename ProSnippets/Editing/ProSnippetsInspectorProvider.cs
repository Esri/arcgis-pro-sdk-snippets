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
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsEditing
{
  public static class ProSnippetsInspectorProvider
  {    
    #region ProSnippet Group: Inspector Provider Class
    #endregion

    // cref: ArcGIS.Desktop.Editing.InspectorProvider
    #region How to create a custom Feature inspector provider class

    public class MyProvider : InspectorProvider
    {
      private System.Guid guid = System.Guid.NewGuid();
      internal MyProvider()
      {
      }
      public override System.Guid SharedFieldColumnSizeID()
      {
        return guid;
      }

      public override string CustomName(Attribute attr)
      {
        //Giving a custom name to be displayed for the field FeatureID
        if (attr.FieldName == "FeatureID")
          return "Feature Identification";

        return attr.FieldName;
      }
      public override bool? IsVisible(Attribute attr)
      {
        //The field FontStyle will not be visible
        if (attr.FieldName == "FontStyle")
          return false;

        return true;
      }
      public override bool? IsEditable(Attribute attr)
      {
        //The field DateField will not be editable
        if (attr.FieldName == "DateField")
          return false;

        return true;
      }
      public override bool? IsHighlighted(Attribute attr)
      {
        //ZOrder field will be highlighted in the feature inspector grid
        if (attr.FieldName == "ZOrder")
          return true;

        return false;
      }

      public override IEnumerable<Attribute> AttributesOrder(IEnumerable<Attribute> attrs)
      {
        //Reverse the order of display
        var newList = new List<Attribute>();
        foreach (var attr in attrs)
        {
          newList.Insert(0, attr);
        }
        return newList;
      }

      public override bool? IsDirty(Attribute attr)
      {
        //The field will not be marked dirty for FeatureID if you enter the value -1
        if ((attr.FieldName == "FeatureID") && (attr.CurrentValue.ToString() == "-1"))
          return false;

        return base.IsDirty(attr);
      }

      public override IEnumerable<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError> Validate(Attribute attr)
      {
        var errors = new List<ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError>();

        if ((attr.FieldName == "FeatureID") && (attr.CurrentValue.ToString() == "2"))
          errors.Add(ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Value not allowed", ArcGIS.Desktop.Editing.Attributes.Severity.Low));

        if ((attr.FieldName == "FeatureID") && (attr.CurrentValue.ToString() == "-1"))
          errors.Add(ArcGIS.Desktop.Editing.Attributes.Attribute.ValidationError.Create("Invalid value", ArcGIS.Desktop.Editing.Attributes.Severity.High));

        return errors;
      }
    }
    #endregion

    public static async void InspectorProviderExample()
    {
      int objectId = 1;
      // cref: ArcGIS.Desktop.Editing.InspectorProvider
      // cref: ArcGIS.Desktop.Editing.InspectorProvider.Create()
      #region Using the custom inspector provider class
      var layer = ArcGIS.Desktop.Mapping.MapView.Active.Map.GetLayersAsFlattenedList().OfType<ArcGIS.Desktop.Mapping.FeatureLayer>().FirstOrDefault();

      var provider = new MyProvider();
      Inspector _featureInspector = provider.Create();
      //Create an embeddable control from the inspector class to display on the pane
      var icontrol = _featureInspector.CreateEmbeddableControl();

      await _featureInspector.LoadAsync(layer, objectId);
      var attribute = _featureInspector.Where(a => a.FieldName == "FontStyle").FirstOrDefault();
      var visibility = attribute.IsVisible; //Will return false

      attribute = _featureInspector.Where(a => a.FieldName == "ZOrder").FirstOrDefault();
      var highlighted = attribute.IsHighlighted; //Will return true
      #endregion
    }
  }
}
