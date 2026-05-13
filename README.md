## ArcGIS Pro 3.7 SDK for .NET

```
ArcGIS Pro Version: 3.7.0.1901
```

This repository provides a comprehensive set of ready-to-use code snippets for ArcGIS Pro SDK addin development. These snippets provide pre-written examples for common tasks and functionalities within the ArcGIS Pro SDK for .NET, allowing developers to quickly integrate specific features into their addins. Addin developers are also welcome to use these examples as a learning resource. These snippets are provided "as-is" and are not supported. 

<a href="https://doc.esri.com/en/arcgis-pro/3.7/sdk/" target="_blank">View it live</a>

--------------------------

## Features

The Visual Studio solution in this repo is organized into folders, each corresponding to a specific functional area, or category, in ArcGIS Pro, such as Editing, Map Exploration, Map Authoring, Geodatabase, Layouts, and more. Within each folder, C# class files contain the relevant code snippets demonstrating common tasks and workflows for each particular category. Each snippet is enclosed within a `#region...#endregion` describing its purpose.

## Instructions

1.	Clone the Repository <br/>
* Use Git to clone the repository to your local machine:
```cmd
 git clone https://github.com/arcgis/arcgis-pro-sdk-snippets.git
```
2.	Explore the Snippet Folders
* The solution is organized into folders by functional area (e.g., Editing, Map Authoring, Map Exploration, Geodatabase, Layouts).
* Each folder contains class files with code snippets relevant to that area/category.
3.	Find Relevant Snippets
* Browse the folders and open the class files.
* Each code snippet is enclosed within a `#region...#endregion` describing its purpose.
4.	Copy and Use Snippets
* Select the snippet(s) you need and copy it/them into your own ArcGIS Pro add-in or application.
* Review any comments for context, prerequisites, or threading requirements (e.g., some snippets require particular layers, to be run on the QueuedTask, etc.).
5.	Reference for GitHub Copilot
* This repository can be used as a reference for GitHub Copilot and other AI coding assistants. Prompt Copilot with questions about ArcGIS Pro SDK tasks and use these snippets as examples.
6.	Learn and Experiment
* Use the snippets to learn ArcGIS Pro SDK patterns and best practices.
* Experiment by modifying snippets to fit your project needs.
7.	Stay Up to Date
* Periodically pull updates from the repository to get new or improved snippets:
```cmd
 git pull origin main
```

**Tip:**<br/>
If you use Visual Studio’s “Find in Files” or GitHub Copilot’s search features, you can quickly locate snippets for specific tasks or API calls. <br/>
**Note:**<br/>
Some snippets may require specific data or context (e.g., a particular layer name or project item). Review the comments in each snippet for details.

## Requirements

#### ArcGIS Pro

* ArcGIS Pro 3.7

#### Supported platforms

* Windows 11 Home, Pro, Enterprise (64 bit)

#### Supported .NET

* Microsoft .NET Runtime 10.0.0 or better. [Download .NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

#### Supported IDEs

* Visual Studio 2026 (v18.4.1 or higher)  
   * Community Edition  
   * Professional Edition  
   * Enterprise Edition  

## Resources

* [API Reference online](https://doc.esri.com/en/arcgis-pro/3.7/sdk/api-reference)
* <a href="https://doc.esri.com/en/arcgis-pro/3.7/sdk/" target="_blank">ArcGIS Pro SDK for .NET (doc.esri.com)</a>
* [arcgis-pro-sdk-community-samples](http://github.com/Esri/arcgis-pro-sdk-community-samples)
* [ArcGIS Pro DAML ID Reference](https://doc.esri.com/en/arcgis-pro/3.7/sdk/api-reference/conceptdocs/docs/ArcGIS-Pro-DAML-ID-Reference.html)
* [ArcGIS Pro SDK icons](https://github.com/Esri/arcgis-pro-sdk/releases/tag/3.7.0.1901)

## Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

## Issues

Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2026 Esri

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

A copy of the license is available in the repository's [LICENSE.txt](LICENSE.txt?raw=true) file.

<p align = center><img src="https://esri.github.io/arcgis-pro-sdk/images/ArcGISPro.png"  alt="pre-req" align = "top" height = "20" width = "20" ><b> ArcGIS Pro 3.7 SDK for Microsoft .NET Framework</b></p>

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="https://doc.esri.com/en/arcgis-pro-app/latest/sdk/api-reference/" target="_blank">API Reference</a> | [Requirements](#requirements) | <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>



