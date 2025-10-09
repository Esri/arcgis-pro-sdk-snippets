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
using ArcGIS.Core.Events;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Workflow.Client;
using ArcGIS.Desktop.Workflow.Client.Events;
using ArcGIS.Desktop.Workflow.Client.Exceptions;
using ArcGIS.Desktop.Workflow.Client.Models;
using ArcGIS.Desktop.Workflow.Client.Steps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProSnippets.WorkFlowSnippets
{
  /// <summary>
  /// Provides utility methods and examples for interacting with the ArcGIS Workflow Manager API.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with creating text symbols in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsWorkFlow
  {
    /// <summary>
    /// Demonstrates various operations using the ArcGIS Workflow Manager API, including checking connection status,
    /// retrieving job information, running and stopping job steps, and handling workflow events.
    /// </summary>
    /// <remarks>This method provides examples of how to interact with the Workflow Manager, such as
    /// determining connection status, retrieving job IDs, searching for jobs, and managing job steps. It also includes
    /// examples of subscribing to and unsubscribing from workflow events. These snippets are intended to guide
    /// developers in using the Workflow Manager API effectively within their applications.
    /// Note: Many of the operations demonstrated in this class require an active connection to the Workflow
    /// Manager server. Ensure that the connection is established before invoking these methods.
    /// </remarks>
    public static void WorkFlowManagerProSnippets()
    {
      #region Variable initialization
      string mapUri = "myMapUri"; // Get a reference to a map using the ArcGIS.Desktop.Mapping API (active view, project item, etc.)
      string jobId = "myJobId"; // Specify a job Id
      string id = "myId"; // Specify an Id
      SubscriptionToken subscriptionToken;
      #endregion Variable initialization

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.IsConnected
      #region How to determine if there is an active Workflow Manager connection
      {
        // determine if there is an active Workflow Manager connection
        var isConnected = WorkflowClientModule.IsConnected;

        // Use the value of isConnected to determine if you can proceed with Workflow Manager operations
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.ItemId
      #region How to get the Workflow Manager item Id
      {
        // Get the Workflow Manager item Id
        var itemId = WorkflowClientModule.ItemId;

        // Use the itemId to identify the Workflow Manager item
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.ServerUrl
      #region How to get the Workflow Manager server url
      {
        // Get the Workflow Manager server url
        var serverUrl = WorkflowClientModule.ServerUrl;

        // Use the serverUrl to identify the Workflow Manager server
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJobId()
      #region How to get the job Id associated with the active map view
      {
        // Get the job Id associated with the active map view
        var jobManager = WorkflowClientModule.JobsManager;
        jobId = jobManager.GetJobId();

        // Use the jobId to identify the job associated with the active map view
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJobId(System.String)
      #region How to get the job Id associated with a map
      {
        // Get the job Id associated with a map
        var jobManager = WorkflowClientModule.JobsManager;
        jobId = jobManager.GetJobId(mapUri);

        // Use the jobId to identify the job associated with the map
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.Steps.OpenProProjectItemsStepCommandArgs
      // cref: ArcGIS.Desktop.Framework.Contracts.Module
      // cref: ArcGIS.Desktop.Framework.FrameworkApplication
      // cref: ArcGIS.Desktop.Framework.IPlugInWrapper
      #region How to get the job Id associated with a running OpenProProjectItems step
      {
        // Get the job Id associated with a running OpenProItems step for a Pro Add-In module

        // In the Add-In Module class, override the ExecuteCommandArgs(string id) method and return a Func<Object[], Task> object like the sample below
        // Refer to the Workflow Manager ProConcepts Sample Code link for an example

        Func<object[], Task> overrideFunction = (args) => QueuedTask.Run(() =>
        {
          try
          {
            // Get the jobId property from the OpenProProjectItemsStep arguments and store it.
            OpenProProjectItemsStepCommandArgs stepArgs = (OpenProProjectItemsStepCommandArgs)args[0];
            var jobId = stepArgs.JobId;
            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"Got job id from ProMappingStep args: {jobId}", "Project Info");

            // Run the command specified by the id passed into ExecuteCommandArgs
            IPlugInWrapper wrapper = FrameworkApplication.GetPlugInWrapper(id);
            if (wrapper is ICommand command && command.CanExecute(null))
              command.Execute(null);
          }
          catch (Exception e)
          {
            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show($"ERROR: {e}", "Error running command");
          }
        });

        // Use overrideFunction to get the jobId from the step args when the command is executed
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.GetJob(System.String,System.Boolean,System.Boolean)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.Job
      #region How to get a job
      {
        // Note: QueuedTask is required to call Workflow Manager API methods
        // GetJob returns an existing job
        try
        {
          var jobManager = WorkflowClientModule.JobsManager;
          var job = jobManager.GetJob(jobId);
          // Do something with the job
        }
        catch (NotConnectedException)
        {
          // Not connected to Workflow Manager server, do some error handling
        }
        catch (Exception)
        {
          // Some other exception occurred, do some error handling
        }
        // Use the job object
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Swagger.Api.JobsApi.SearchJobs(System.String,System.String,System.String,ArcGIS.Desktop.Workflow.Client.Swagger.Model.EsriWorkflowModelsRestJobQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchResult
      #region Search for jobs using a detailed query
      {
        var search = new SearchQuery()
        {
          // Search for all open high priority jobs assigned to users
          Q = "closed=0 AND assignedType='User' AND priority='High'",
          Fields = ["jobId", "jobName", "assignedTo", "dueDate"],
          // Sort by job assignment in ascending order and due date in descending order
          SortFields =
          [
            new SortField() { FieldName = "assignedTo", SortOrder = SortOrder.Asc },
            new SortField() { FieldName = "dueDate", SortOrder = SortOrder.Desc }
          ]
        };
        var jobManager = WorkflowClientModule.JobsManager;
        var searchResults = jobManager.SearchJobs(search);
        var fields = searchResults.Fields;
        var results = searchResults.Results;
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Swagger.Api.JobsApi.SearchJobs(System.String,System.String,System.String,ArcGIS.Desktop.Workflow.Client.Swagger.Model.EsriWorkflowModelsRestJobQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchResult
      #region Search for jobs using a detailed query with an arcade expression
      {
        var search = new SearchQuery()
        {
          // Search for jobs assigned to the current user using the arcade expression '$currentUser'
          Q = "\"assignedType='User' AND closed=0 AND assignedTo='\" + $currentUser + \"' \"",
          Fields = ["jobId", "jobName", "assignedTo", "dueDate"],
          // Sort by job name in ascending order
          SortFields = [new SortField() { FieldName = "jobName", SortOrder = SortOrder.Asc }]
        };
        var jobManager = WorkflowClientModule.JobsManager;
        var searchResults = jobManager.SearchJobs(search);
        var fields = searchResults.Fields;
        var results = searchResults.Results;
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Swagger.Api.JobsApi.SearchJobs(System.String,System.String,System.String,ArcGIS.Desktop.Workflow.Client.Swagger.Model.EsriWorkflowModelsRestJobQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.SearchResult
      #region Search for jobs using a simple string
      {
        var search = new SearchQuery() { Search = "My Search String" };
        var jobManager = WorkflowClientModule.JobsManager;
        var searchResults = jobManager.SearchJobs(search);
        var fields = searchResults.Fields;
        var results = searchResults.Results;

        // Use the fields and results collections
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.CalculateJobStatistics(ArcGIS.Desktop.Workflow.Client.Models.JobStatisticsQuery)
      // cref: ArcGIS.Desktop.Workflow.Client.Models.JobStatisticsQuery
      // cref: ArcGIS.Desktop.Workflow.Client.Models.JobStatistics
      #region Get statistics for jobs
      {
        var query = new JobStatisticsQuery()
        {
          // Search for open jobs assigned to users
          Q = "\"assignedType='User' AND closed=0 \""
        };
        var jobManager = WorkflowClientModule.JobsManager;
        var results = jobManager.CalculateJobStatistics(query);
        var totalJobs = results.Total;
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.RunSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to run steps on a job
      {
        var jobManager = WorkflowClientModule.JobsManager;
        jobManager.RunSteps(jobId);
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.RunSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to run specific steps on a job
      {
        var jobManager = WorkflowClientModule.JobsManager;
        // Specify specific current steps in a job to run
        var stepIds = new List<string> { "step12345", "step67890" };
        jobManager.RunSteps(jobId, stepIds);
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.StopSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to stop running steps on a job
      {
        var jobManager = WorkflowClientModule.JobsManager;
        // Get the job Id associated with the active map view
        jobId = jobManager.GetJobId();
        // Stop the current steps in the job with the given id.
        jobManager.StopSteps(jobId);
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.StopSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to stop specific running steps on a job
      {
        var jobManager = WorkflowClientModule.JobsManager;
        // Get the job Id associated with the active map view
        jobId = jobManager.GetJobId();
        // Specify specific running steps in a job to stop
        List<string> stepIds = ["step12345", "step67890"];
        jobManager.StopSteps(jobId, stepIds);
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.FinishSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to finish steps on a job
      {
        var jobManager = WorkflowClientModule.JobsManager;
        // Finish the current steps in the job with the given id.
        jobManager.FinishSteps(jobId);
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.WorkflowClientModule.JobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager
      // cref: ArcGIS.Desktop.Workflow.Client.Models.IJobsManager.FinishSteps(System.String,System.Collections.Generic.List{System.String})
      #region How to finish specific steps on a job
      {
        var jobManager = WorkflowClientModule.JobsManager;
        List<string> stepIds = ["step12345", "step67890"];
        jobManager.FinishSteps(jobId, stepIds);
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.Events.WorkflowConnectionChangedEvent
      #region How to subscribe to a workflow connection changed event.
      {
        subscriptionToken = WorkflowConnectionChangedEvent.Subscribe(e =>
        {
          // The connection has changed
          // Do something relevant
          return Task.CompletedTask;
        });
      }
      #endregion

      // cref: ArcGIS.Desktop.Workflow.Client.Events.WorkflowConnectionChangedEvent
      #region How to unsubscribe from a workflow connection changed event.
      {
        WorkflowConnectionChangedEvent.Unsubscribe(subscriptionToken);
      }
      #endregion
    }
  }
}