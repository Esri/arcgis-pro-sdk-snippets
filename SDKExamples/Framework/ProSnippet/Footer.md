## Miscellaneous

### Start ArcGIS Pro from the command line

```c#
C:\>"C:\Program Files\ArcGIS Pro\bin\ArcGISPro.exe"
```

### Get Command Line Arguments

If your Add-in requires the use of custom command line arguments your arguments must be of the form "/argument" - note the forward slash "/". There must be no white space. If the command line contains a project to be opened (see Open project from command line) then custom arguments or switches must be placed before the project filename argument.

```c#
   string[] args = System.Environment.GetCommandLineArgs();
   foreach (var arg in args)
   {
      // look for your command line switches
   }
```

### Application Accelerators (Shortcut Keys)

Application accelerators can be added to your Add-in config.daml using an accelerators/insertAccelerator DAML element with the refID of the element to which you are associating the accelerator (i.e. short cut).

```xml
<accelerators>
    <insertAccelerator refID="esri_core_openProjectButton" flags="Ctrl" key="O" />
    <insertAccelerator refID="esri_core_redoButton" flags="Ctrl" key="Y" />
    <insertAccelerator refID="esri_core_undoButton" flags="Ctrl" key="Z" />
</accelerators>
```
Note: Use the deleteAccelerator and updateAccelerator DAML elements within an updateModule element to remove or alter application accelerators respectively.
Flags can be one of: Shift, Ctrl, Alt, Ctrl+Shift, Alt+Shift, Ctrl+Alt, Ctrl+Alt+Shift


### Defining controls in DAML with Pro Styles

There are many ArcGIS Pro styles defined which can be applied to buttons, labels and other controls on your panes and dockpanes to make your add-ins look and feel seamless with ArcGIS Pro.  Some of the most common styles are listed below.  For more styles and colors see the Styling-With-ArcGIS-Pro sample in the Community Samples repo.

Button styles
```xml
<Button Content="Button" Style="{StaticResource Esri_SimpleButton}" ToolTip="Button">
<Button Content="Button" Style="{StaticResource Esri_BackButton}" ToolTip="Button">
<Button Content="Button" Style="{StaticResource Esri_BackButtonSmall}" ToolTip="Button">
<Button Content="Button" Style="{StaticResource Esri_CloseButton}" ToolTip="Button">
```

Dockpane heading style
```xml
<TextBlock Text="MyDockPane" Style="{StaticResource DockPaneHeading}" 
                   VerticalAlignment="Center" HorizontalAlignment="Center"/>
```
