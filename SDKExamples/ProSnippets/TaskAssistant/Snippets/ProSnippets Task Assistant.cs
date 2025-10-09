/*

   Copyright 2018 Esri

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
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.TaskAssistant;
using ArcGIS.Desktop.TaskAssistant.Events;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.TaskAssistant.Exceptions;

namespace ProSnippets.TaskAssistantSnippets
{
	/// <summary>
	/// Provides code snippets demonstrating common operations with the ArcGIS Pro Task Assistant API,
	/// including retrieving, opening, closing, exporting, and querying task items, as well as handling task events and exceptions.
	/// </summary>
	public static partial class ProSnippetsTaskAssistant
  {
		/// <summary>
		/// Demonstrates common operations with ArcGIS Pro Task Assistant, including retrieving, opening, closing, exporting, 
		/// and querying task items, as well as handling task events and exceptions.
		/// </summary>
		public static async Task ProSnippetsTaskAssistantAsync()
    {
      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem
      #region Retrieve all the Task Items in a Project
      IEnumerable<TaskProjectItem> taskItems = Project.Current.GetItems<TaskProjectItem>();
      foreach (var item in taskItems)
      {
        // do something
      }

      #endregion

      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.CanOpenTaskFile(System.String)
      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.OpenTaskFileAsync(System.String)
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.OpenTaskException
      #region Open a Task File - .esriTasks file
      // Open a task file
      try
      {
        // TODO - substitute your own .esriTasks file to be opened
        string taskFile = @"c:\Tasks\Get Started.esriTasks";
        //At 2.x -
        //System.Guid guid = await TaskAssistantModule.OpenTaskAsync(taskFile);
        System.Guid guid;
        if (TaskAssistantFactory.Instance.CanOpenTaskFile(taskFile))
          guid = await TaskAssistantFactory.Instance.OpenTaskFileAsync(taskFile);

        // TODO - retain the guid returned for use with CloseTaskItemAsync
      }
      catch (OpenTaskException e)
      {
        // exception thrown if task file doesn't exist or has incorrect format
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
      }

      #endregion

      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem
      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem.TaskItemGuid
      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.OpenTaskItemAsync(System.Guid)
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.OpenTaskException
      #region Open a Project Task Item
      {
        // get the first project task item
        var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault();
        // if there isn't a project task item, return
        if (taskItem == null)
          return;

        try
        {
          // Open it
          //At 2.x -
          //System.Guid guid = await TaskAssistantModule.OpenTaskItemAsync(taskItem.TaskItemGuid);
          var guid = await TaskAssistantFactory.Instance.OpenTaskItemAsync(taskItem.TaskItemGuid);

          // TODO - retain the guid returned for use with CloseTaskItemAsync
        }
        catch (OpenTaskException e)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.CloseTaskItemAsync(System.Guid)
      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem
      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem.IsOpen
      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem.TaskItemGuid
      #region Close a Task Item
      {
        // find the first project task item which is open
        var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault(t => t.IsOpen == true);
        // if there isn't a project task item, return
        if (taskItem == null)
          return;

        if (taskItem.IsOpen)
        {
          // close it
          // NOTE : The task item will also be removed from the project
          //At 2.x -
          //TaskAssistantModule.CloseTaskAsync(taskItem.TaskItemGuid);
          TaskAssistantFactory.Instance?.CloseTaskItemAsync(taskItem.TaskItemGuid);
        }
      }
      #endregion

      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem
      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.ExportTaskItemAsync(System.Guid,System.String)
      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem.TaskItemGuid
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.ExportTaskException
      #region Export a Task Item
      {
        // get the first project task item
        var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault();
        // if there isn't a project task item, return
        if (taskItem == null)
          return;

        try
        {
          // export the task item to the c:\Temp folder
          string exportFolder = @"c:\temp";
          //At 2.x -
          //string fileName = await TaskAssistantModule.ExportTaskAsync(taskItem.TaskItemGuid, exportFolder);
          string fileName = await TaskAssistantFactory.Instance.ExportTaskItemAsync(taskItem.TaskItemGuid, exportFolder);
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Task saved to " + fileName);
        }
        catch (ExportTaskException e)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Error saving task " + e.Message);
        }
      }
      #endregion Export a Task Item

      // cref: ArcGIS.Desktop.TaskAssistant.TaskProjectItem.GetTaskItemInfoAsync
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.Name
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.Description
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.Guid
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.GetTasks
      // cref: ArcGIS.Desktop.TaskAssistant.TaskInfo.Name
      // cref: ArcGIS.Desktop.TaskAssistant.TaskInfo.Guid
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.OpenTaskException
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.TaskFileVersionException
      #region Get Task Information - from a TaskProjectItem
      {
        var taskItem = Project.Current.GetItems<TaskProjectItem>().FirstOrDefault();
        // if there isn't a project task item, return
        if (taskItem == null)
          return;

        string message = await QueuedTask.Run(async () =>
        {
          bool isOpen = taskItem.IsOpen;
          Guid taskGuid = taskItem.TaskItemGuid;

          string msg = "";
          try
          {
            TaskItemInfo taskItemInfo = await taskItem.GetTaskItemInfoAsync();

            msg = "Name : " + taskItemInfo.Name;
            msg += "\r\n" + "Description : " + taskItemInfo.Description;
            msg += "\r\n" + "Guid : " + taskItemInfo.Guid.ToString("B");
            msg += "\r\n" + "Task Count : " + taskItemInfo.GetTasks().Count();

            // iterate the tasks in the task item
            IEnumerable<TaskInfo> taskInfos = taskItemInfo.GetTasks();
            foreach (TaskInfo taskInfo in taskInfos)
            {
              string name = taskInfo.Name;
              Guid guid = taskInfo.Guid;

              // do something 
            }
          }
          catch (OpenTaskException e)
          {
            // exception thrown if task file doesn't exist or has incorrect format
            msg = e.Message;
          }
          catch (TaskFileVersionException e)
          {
            // exception thrown if task file does not support returning task information
            msg = e.Message;
          }
          return msg;
        });
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(message, "Task Information");
      }
      #endregion

      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.GetTaskItemInfoAsync(System.String)
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.Name
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.Description
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.Guid
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.GetTasks()
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.OpenTaskException
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.TaskFileVersionException
      #region Get Task Information - from an .esriTasks file
      {
        // TODO - substitute your own .esriTasks file
        string taskFile = @"c:\Tasks\Get Started.esriTasks";

        string message = await QueuedTask.Run(async () =>
        {
          string msg = "";
          try
          {
            // retrieve the task item information
            //At 2.x -
            //TaskItemInfo taskItemInfo = await TaskAssistantModule.GetTaskItemInfoAsync(taskFile);
            TaskItemInfo taskItemInfo = await TaskAssistantFactory.Instance.GetTaskItemInfoAsync(taskFile);

            msg = "Name : " + taskItemInfo.Name;
            msg += "\r\n" + "Description : " + taskItemInfo.Description;
            msg += "\r\n" + "Guid : " + taskItemInfo.Guid.ToString("B");
            msg += "\r\n" + "Task Count : " + taskItemInfo.GetTasks().Count();

          }
          catch (OpenTaskException e)
          {
            // exception thrown if task file doesn't exist or has incorrect format
            msg = e.Message;
          }
          catch (TaskFileVersionException e)
          {
            // exception thrown if task file does not support returning task information
            msg = e.Message;
          }
          return msg;
        });
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(message, "Task Information");
      }
      #endregion Get Task item information

      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.GetTaskItemInfoAsync(System.String)
      // cref: ArcGIS.Desktop.TaskAssistant.TaskItemInfo.GetTasks()
      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.OpenTaskFileAsync(System.String,System.Guid)
      // cref: ArcGIS.Desktop.TaskAssistant.TaskAssistantFactory.OpenTaskFileAsync(System.String)
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.OpenTaskException
      // cref: ArcGIS.Desktop.TaskAssistant.Exceptions.TaskFileVersionException
      #region Open a specific Task in a Task File - .esriTasks file
      {
        // TODO - substitute your own .esriTasks file to be opened
        string taskFile = @"c:\Tasks\Get Started.esriTasks";

        await QueuedTask.Run(async () =>
        {
          try
          {
            // retrieve the task item information
            //At 2.x -
            //TaskItemInfo taskItemInfo = await TaskAssistantModule.GetTaskItemInfoAsync(taskFile);
            var taskItemInfo = await TaskAssistantFactory.Instance.GetTaskItemInfoAsync(taskFile);

            // find the first task
            TaskInfo taskInfo = taskItemInfo.GetTasks().FirstOrDefault();

            Guid guid = Guid.Empty;
            if (taskInfo != null)
            {
              // if a task exists, open it
              //At 2.x -
              //guid = await TaskAssistantModule.OpenTaskAsync(taskFile, taskInfo.Guid);
              guid = await TaskAssistantFactory.Instance.OpenTaskFileAsync(taskFile, taskInfo.Guid);
            }
            else
            {
              // else just open the task item
              //At 2.x -
              //guid = await TaskAssistantModule.OpenTaskAsync(taskFile);
              guid = await TaskAssistantFactory.Instance.OpenTaskFileAsync(taskFile);
            }

            // TODO - retain the guid returned for use with CloseTaskItemAsync 
          }
          catch (OpenTaskException e)
          {
            // exception thrown if task file doesn't exist or has incorrect format
            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
          }
          catch (TaskFileVersionException e)
          {
            // exception thrown if task file does not support returning task information
            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(e.Message);
          }

        });
			}
			#endregion

			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEvent
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.UserID
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.ProjectName
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.TaskItemGuid
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.TaskItemName
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.TaskItemVersion
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.TaskGuid
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.TaskName
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskStartedEventArgs.StartTime
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEvent
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.UserID
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.ProjectName
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.TaskItemGuid
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.TaskItemName
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.TaskItemVersion
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.TaskGuid
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.TaskName
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.StartTime
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.EndTime
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.Duration
			// cref: ArcGIS.Desktop.TaskAssistant.Events.TaskEndedEventArgs.Completed
			#region Subscribe to Task Events
			{
				TaskStartedEvent.Subscribe(taskStartedEventArgs =>
				{
					string userName = taskStartedEventArgs.UserID;    // ArcGIS Online signed in userName.  If not signed in to ArcGIS Online then returns the name of the user logged in to the Windows OS.
					string projectName = taskStartedEventArgs.ProjectName;

					Guid taskItemGuid = taskStartedEventArgs.TaskItemGuid;
					string taskItemName = taskStartedEventArgs.TaskItemName;
					string taskItemVersion = taskStartedEventArgs.TaskItemVersion;

					Guid taskGuid = taskStartedEventArgs.TaskGuid;
					string taskName = taskStartedEventArgs.TaskName;

					DateTime startTime = taskStartedEventArgs.StartTime;
				});
				TaskEndedEvent.Subscribe(taskEndedEventArgs =>
							{
								string userName = taskEndedEventArgs.UserID;    // ArcGIS Online signed in userName.  If not signed in to ArcGIS Online then returns the name of the user logged in to the Windows OS.
								string projectName = taskEndedEventArgs.ProjectName;

								Guid taskItemGuid = taskEndedEventArgs.TaskItemGuid;
								string taskItemName = taskEndedEventArgs.TaskItemName;
								string taskItemVersion = taskEndedEventArgs.TaskItemVersion;

								Guid taskGuid = taskEndedEventArgs.TaskGuid;
								string taskName = taskEndedEventArgs.TaskName;

								DateTime startTime = taskEndedEventArgs.StartTime;
								DateTime endTime = taskEndedEventArgs.EndTime;
								double duration = taskEndedEventArgs.Duration;

								bool completed = taskEndedEventArgs.Completed;    // completed or cancelled
							});
			}
			#endregion
		}
	}
}
