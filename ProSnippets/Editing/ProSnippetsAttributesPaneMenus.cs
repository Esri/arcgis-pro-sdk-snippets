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
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSnippetsEditing
{
  public class ProSnippetsAttributesPaneMenus
  {
    #region ProSnippet Group: Attributes Pane Context MenuItems
    #endregion

    protected async void ContextMenuItem()
    {
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication.ContextMenuDataContextAs<T>
      #region Retrieve SelectionSet from command added to Attribute Pane Context Menu  
      await QueuedTask.Run(async () =>
      {
        var selSet = FrameworkApplication.ContextMenuDataContextAs<SelectionSet>();
        if (selSet == null)
          return;

        int count = selSet.Count;
        if (count == 0)
          return;

        var op = new EditOperation
        {
          Name = "Delete context"
        };
        op.Delete(selSet);
        await op.ExecuteAsync();
      });
      #endregion
    }
  }
}
