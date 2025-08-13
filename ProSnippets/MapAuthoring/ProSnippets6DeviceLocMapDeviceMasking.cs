using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core.DeviceLocation;
using ArcGIS.Desktop.Core.DeviceLocation.Events;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.DeviceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MapAuthoring.ProSnippets
{
  public static class ProSnippets5DeviceLocMapDeviceMasking
  {
    #region ProSnippet Group: Device Location API, GPS/GNSS Devices
    #endregion
    // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.#ctor
    // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.ComPort
    // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.BaudRate
    // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.AntennaHeight
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.#ctor
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.AccuracyThreshold
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.Open(ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationSource,ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties)
    #region Connect to a Device Location Source
    /// <summary>
    /// Establishes a connection to a device location source such as GPS/GNSS device using a COM port.
    /// </summary>
    /// <remarks>This method initializes a <see cref="SerialPortDeviceLocationSource"/> with predefined
    /// settings,  including the COM port, baud rate, and antenna height, and configures location properties such as 
    /// accuracy threshold. It then opens the device location source on a background thread using the  <see
    /// cref="DeviceLocationService"/>. <para> This method is asynchronous and should be awaited to ensure the
    /// connection is established  before proceeding with location-based operations. </para></remarks>
    /// <returns>A task representing the asynchronous operation of connecting to the device location source.</returns>
    public static async Task ConnectToDeviceLocationSource()
    {
      var newSrc = new SerialPortDeviceLocationSource();
      //Specify the COM port the device is connected to
      newSrc.ComPort = "Com3";
      newSrc.BaudRate = 4800;
      newSrc.AntennaHeight = 3;  // meters
                                 //fill in other properties as needed

      var props = new DeviceLocationProperties();
      props.AccuracyThreshold = 10;   // meters

      // jump to the background thread
      await QueuedTask.Run(() =>
      {
        //open the device
        DeviceLocationService.Instance.Open(newSrc, props);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetSource()
    #region Get the Current Device Location Source
    /// <summary>
    /// Retrieves the current device location source used by the application.
    /// </summary>
    /// <remarks>This method accesses the <see cref="DeviceLocationService"/> to determine the active source
    /// for device location data. If no source is currently available, the method will return <c>null</c>.</remarks>
    public static void GetCurrentDeviceLocationSource()
    {
      var source = DeviceLocationService.Instance.GetSource();
      if (source == null)
      {
        //There is no current source
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.Close()
    #region Close the Current Device Location Source

    /// <summary>
    /// Closes the current device location source if one is active.
    /// </summary>
    /// <remarks>This method checks whether a device location source is currently active and, if so, closes it
    /// asynchronously. </remarks>
    /// <returns>A task that represents the asynchronous operation of closing the device location source.</returns>
    public static async Task CloseCurrentDeviceLocationSource()
    {
      //Is there a current device source?
      var src = DeviceLocationService.Instance.GetSource();
      if (src == null)
        return;//no current source

      await QueuedTask.Run(() =>
      {
        DeviceLocationService.Instance.Close();
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.IsDeviceConnected()
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetSource()
    // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource
    // cref: ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource.GetSpatialReference()
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetProperties()
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.AccuracyThreshold
    #region Get Current Device Location Source and Properties
    /// <summary>
    /// Retrieves the current device location source and its associated properties.
    /// </summary>
    /// <remarks>This method checks if a device is connected and retrieves the current location source and
    /// properties. If the source is a <see cref="ArcGIS.Desktop.Core.DeviceLocation.SerialPortDeviceLocationSource"/>, 
    /// additional details such as communication port, antenna height, and spatial reference are obtained. The method
    /// also retrieves the device location properties, including the accuracy threshold.</remarks>
    public async static void GetCurrentDeviceLocationSourceAndProperties()
    {
      // Check if a device is connected
      bool isConnected = DeviceLocationService.Instance.IsDeviceConnected();
      if (!isConnected)
        return; // no device connected
      // Get the current device location source
      var src = DeviceLocationService.Instance.GetSource();
      // Check if the source is a SerialPortDeviceLocationSource
      //Set values for the SerialPortDeviceLocationSource
      if (src is SerialPortDeviceLocationSource serialPortSrc)
      {
        var port = serialPortSrc.ComPort;
        var antennaHeight = serialPortSrc.AntennaHeight;
        var dataBits = serialPortSrc.DataBits;
        var baudRate = serialPortSrc.BaudRate;
        var parity = serialPortSrc.Parity;
        var stopBits = serialPortSrc.StopBits;

        // retrieving spatial reference needs the MCT
        var sr = await QueuedTask.Run(() =>
        {
          return serialPortSrc.GetSpatialReference();
        });

      }
      //Get current device location properties being used.
      var dlProps = DeviceLocationService.Instance.GetProperties();
      var accuracy = dlProps.AccuracyThreshold;
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetProperties()
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties.AccuracyThreshold
    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.UpdateProperties(ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationProperties)
    #region Update Properties on the Current Device Location Source
    /// <summary>
    /// Updates the properties of the current device location source.
    /// </summary>
    /// <remarks>This method retrieves the current device location properties, modifies the accuracy
    /// threshold,  and applies the updated properties to the device location source. The accuracy threshold is set  to
    /// 22.5 meters.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async static Task UpdateDeviceLocationProperties()
    {
      await QueuedTask.Run(() =>
      {
        // Get the current device location properties
        var dlProps = DeviceLocationService.Instance.GetProperties();
        //Change the accuracy threshold
        dlProps.AccuracyThreshold = 22.5; // meters
        // Update the properties on the device location source
        DeviceLocationService.Instance.UpdateProperties(dlProps);
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEvent
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEvent.Subscribe
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEventArgs
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationPropertiesUpdatedEventArgs.DeviceLocationProperties
    #region Subscribe to DeviceLocationPropertiesUpdated event
    /// <summary>
    /// Subscribes to the DeviceLocationPropertiesUpdated event to receive updates on device location properties.
    /// </summary>
    public static void SubscribeToPropertiesEvents()
    {
      DeviceLocationPropertiesUpdatedEvent.Subscribe(OnDeviceLocationPropertiesUpdated);
    }
    /// <summary>
    /// Event handler for DeviceLocationPropertiesUpdated event.
    /// </summary>
    /// <param name="args"></param>
    public static void OnDeviceLocationPropertiesUpdated(DeviceLocationPropertiesUpdatedEventArgs args)
    {
      if (args == null)
        return;

      var properties = args.DeviceLocationProperties;
      //  TODO - something with the updated properties
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEvent
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEvent.Subscribe
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEventArgs
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.DeviceLocationSourceChangedEventArgs.DeviceLocationSource
    #region Subscribe to DeviceLocationSourceChanged event
    /// <summary>
    /// Subscribes to the DeviceLocationSourceChanged event to receive updates on device location source changes.
    /// </summary>
    public static void SubscribeToSourceEvents()
    {
      DeviceLocationSourceChangedEvent.Subscribe(OnDeviceLocationSourceChanged);
    }
    /// <summary>
    /// Event handler for DeviceLocationSourceChanged event.
    /// </summary>
    /// <param name="args"></param>
    public static void OnDeviceLocationSourceChanged(DeviceLocationSourceChangedEventArgs args)
    {
      if (args == null)
        return;

      var source = args.DeviceLocationSource;

      //  TODO - something with the updated source properties
    }
    #endregion

    #region ProSnippet Group: Map Device Location Options
    #endregion

    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.IsDeviceLocationEnabled
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.SetDeviceLocationEnabled(System.Boolean)
    #region Enable/Disable Current Device Location Source For the Map
    /// <summary>
    /// Toggles the current device location source for the map between enabled and disabled states.
    /// </summary>
    /// <remarks>This method checks whether the device location source is currently enabled for the map and
    /// switches its state. If the device location source is enabled, it will be disabled; if it is disabled, it will be
    /// enabled.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task EnableDisableDeviceLocationSourceFromMap()
    {
      bool enabled = MapDeviceLocationService.Instance.IsDeviceLocationEnabled;
      await QueuedTask.Run(() =>
      {
        MapDeviceLocationService.Instance.SetDeviceLocationEnabled(!enabled);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.GetDeviceLocationOptions()
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.DeviceLocationVisibility
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.NavigationMode
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.TrackUpNavigation
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.ShowAccuracyBuffer
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MappingDeviceLocationNavigationMode
    #region Get Current Map Device Location Options
    /// <summary>
    /// Retrieves the current device location options used by the <see
    /// cref="ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService"/>.
    /// </summary>
    /// <remarks>This method provides access to the configuration settings for device location visualization
    /// and navigation on the map. The options include visibility, navigation mode, tracking behavior, and accuracy
    /// buffer display.</remarks>
    public static void GetCurrentMapDeviceLocationOptions()
    {
      //Gets the current device location options used by the MapDeviceLocationService
      var options = MapDeviceLocationService.Instance.GetDeviceLocationOptions();
      //Device location visibility on the map
      var visibility = options.DeviceLocationVisibility;
      //MappingDeviceLocationNavigationMode
      var navMode = options.NavigationMode;
      //Heading of the location from the device points to the top of the screen
      var trackUp = options.TrackUpNavigation;
      //Show accuracy buffer on the map
      var showBuffer = options.ShowAccuracyBuffer;
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.IsDeviceLocationEnabled
    #region Check if The Current Device Location Is Enabled On The Map
    /// <summary>
    /// Checks whether the current device location source is enabled on the map.
    /// </summary>
    /// <remarks>This method determines if the device location functionality is active for the map. It does
    /// not modify any settings or state; it only performs a check.</remarks>
    public static void CheckIfDeviceLocationIsEnabledOnMap()
    {
      //Checks if the current device location source is enabled on the map
      if (MapDeviceLocationService.Instance.IsDeviceLocationEnabled)
      {
        //The Device Location Source is Enabled
      }
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.SetDeviceLocationOptions(ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions)
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.#ctor
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MappingDeviceLocationNavigationMode
    #region Set Current Map Device Location Options
    /// <summary>
    /// Configures the current map to use specific device location options.
    /// </summary>
    /// <remarks>This method sets the device location options for the active map view, enabling device
    /// location visibility,  setting the navigation mode to keep the device location at the center, and enabling
    /// track-up navigation. The method must be called when a valid device location source is available and device
    /// location is enabled.</remarks>
    public static void SetMapDeviceLocationOptions()
    {
      QueuedTask.Run(() =>
      {//Check there is a source first...
        if (DeviceLocationService.Instance.GetSource() == null)
          //Setting DeviceLocationOptions w/ no Device Location Source
          //Will throw an InvalidOperationException
          return;

        var map = MapView.Active.Map;
        if (!MapDeviceLocationService.Instance.IsDeviceLocationEnabled)
          //Setting DeviceLocationOptions w/ no Device Location Enabled
          //Will throw an InvalidOperationException
          return;

        MapDeviceLocationService.Instance.SetDeviceLocationOptions(
          new MapDeviceLocationOptions()
          {
            DeviceLocationVisibility = true,
            NavigationMode = MappingDeviceLocationNavigationMode.KeepAtCenter,
            TrackUpNavigation = true
          });
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.ZoomOrPanToCurrentLocation(System.Boolean)
    #region Zoom/Pan The Map To The Most Recent Location
    /// <summary>
    /// Zooms or pans the map to the most recent device location.
    /// </summary>
    /// <remarks>This method requires that device location services are enabled. If device location services
    /// are not enabled,  calling this method will result in an <see cref="InvalidOperationException"/>.</remarks>
    public static void ZoomOrPanToCurrentLocation()
    {
      QueuedTask.Run(() =>
      {
        if (!MapDeviceLocationService.Instance.IsDeviceLocationEnabled)
          //Calling ZoomOrPanToCurrentLocation w/ no Device Location Enabled
          //Will throw an InvalidOperationException
          return;

        // true for zoom, false for pan
        MapDeviceLocationService.Instance.ZoomOrPanToCurrentLocation(true);
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Core.DeviceLocation.DeviceLocationService.GetCurrentSnapshot()
    #region Add the Most Recent Location To A Graphics Layer
    /// <summary>
    /// Adds the most recent device location to the specified graphics layer as a graphic.
    /// </summary>
    /// <remarks>This method retrieves the latest location snapshot from the device location service and adds
    /// it to the provided graphics layer as a triangle-shaped point graphic. If no location snapshot is available, no
    /// graphic is added.</remarks>
    /// <param name="graphicsLayer">The graphics layer to which the most recent location will be added. This parameter cannot be null.</param>
    public static void AddMostRecentLocationToGraphicsLayer(GraphicsLayer graphicsLayer)
    {
      QueuedTask.Run(() =>
      {
        // get the last location
        var pt = DeviceLocationService.Instance.GetCurrentSnapshot()?.GetPositionAsMapPoint();
        if (pt != null)
        {
          //Create a point symbol
          var ptSymbol = SymbolFactory.Instance.ConstructPointSymbol(
                            CIMColor.CreateRGBColor(125, 125, 0), 10, SimpleMarkerStyle.Triangle);
          //Add a graphic to the graphics layer
          graphicsLayer.AddElement(pt, ptSymbol);
          //unselect it
          graphicsLayer.ClearSelection();
        }
      });
    }
    #endregion
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.GetDeviceLocationOptions()
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.DeviceLocationVisibility
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions.NavigationMode
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MappingDeviceLocationNavigationMode
    // cref: ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationService.SetDeviceLocationOptions(ArcGIS.Desktop.Mapping.DeviceLocation.MapDeviceLocationOptions)
    #region Set map view to always be centered on the device location
    /// <summary>
    /// Configures the map view to always center on the device's current location.
    /// </summary>
    /// <remarks>This method updates the device location visibility and navigation mode settings to ensure
    /// that the  device's location remains visible and centered on the map. It uses the <see
    /// cref="MapDeviceLocationService"/>  to retrieve and modify the current device location options.</remarks>
    public async static void SetMapViewToBeCenteredOnDeviceLocation()
    {
      // Get the MapDeviceLocationOptions currently used by the MapDeviceLocationService

      var currentOptions = MapDeviceLocationService.Instance.GetDeviceLocationOptions();
      if (currentOptions == null)
        return;
      // Set the device location visibility on the map to true
      currentOptions.DeviceLocationVisibility = true;
      //Set the navigation mode to keep the device location at the center of the map
      currentOptions.NavigationMode = MappingDeviceLocationNavigationMode.KeepAtCenter;

      await QueuedTask.Run(() =>
      {
        //Sets the MapDeviceLocationOptions to be updates with these values
        MapDeviceLocationService.Instance.SetDeviceLocationOptions(currentOptions);
      });
    }
    #endregion


    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEvent
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEvent.Subscribe(Action<ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEventArgs>, System.Boolean)
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEventArgs
    // cref: ArcGIS.Desktop.Core.DeviceLocation.Events.SnapshotChangedEventArgs.Snapshot
    // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot
    // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.GetPositionAsMapPoint()
    // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.Altitude
    // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.DateTime
    // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.VDOP
    // cref: ArcGIS.Desktop.Core.DeviceLocation.NMEASnapshot.HDOP
    #region Subscribe to Location Snapshot event
    /// <summary>
    /// Subscribes to the SnapshotChangedEvent to receive notifications when a new location snapshot is available.
    /// </summary>
    /// <remarks>This method registers a callback to handle snapshot change events. The callback processes the
    /// snapshot data,  which includes information such as position, altitude, timestamp, and accuracy metrics. Use this
    /// method to monitor location updates and respond to changes in device location snapshots.</remarks>
    public static void SubscribeToSnapshotEvents()
    {
      SnapshotChangedEvent.Subscribe(OnSnapshotChanged);
    }
    /// <summary>
    /// Handles changes to a snapshot by processing the provided snapshot data.
    /// </summary>
    /// <remarks>This method processes the snapshot asynchronously using <see cref="QueuedTask.Run"/>.  If the
    /// snapshot contains valid position data, additional properties such as altitude, date/time, and DOP values are
    /// accessed for further use.</remarks>
    /// <param name="args">The event arguments containing the snapshot data to process.  If <paramref name="args"/> is <see
    /// langword="null"/> or the snapshot is not of type <see cref="NMEASnapshot"/>, the method exits without performing
    /// any action.</param>
    public static void OnSnapshotChanged(SnapshotChangedEventArgs args)
    {
      if (args == null)
        return;

      var snapshot = args.Snapshot as NMEASnapshot;
      if (snapshot == null)
        return;

      QueuedTask.Run(() =>
      {
        var pt = snapshot.GetPositionAsMapPoint();
        if (pt?.IsEmpty ?? true)
          return;

        // access properties
        var alt = snapshot.Altitude;
        var dt = snapshot.DateTime;
        var vdop = snapshot.VDOP;
        var hdop = snapshot.HDOP;
        // etc

        //TODO: use the snapshot
      });
    }
    #endregion

    #region ProSnippet Group: Feature Masking
    #endregion
    // cref: ArcGIS.Desktop.Mapping.BasicFeatureLayer.GetDrawingOutline(System.Int64, ArcGIs.Desktop.Mapping.MapView, ArcGIS.Desktop.Mapping.DrawingOutlineType)
    // cref: ArcGIS.Desktop.Mapping.DrawingOutlineType
    #region Get the Mask Geometry for a Feature
    /// <summary>
    /// Retrieves the mask geometry for a feature in the active map view.
    /// </summary>
    /// <remarks>This method identifies the first feature in the active map's first available <see
    /// cref="ArcGIS.Desktop.Mapping.BasicFeatureLayer"/>  and calculates its mask geometry using the <see
    /// cref="ArcGIS.Desktop.Mapping.DrawingOutlineType.Exact"/> outline type. The mask geometry is returned in the
    /// spatial reference of the map.</remarks>
    public static void GetMaskGeometryForFeature()
    {
      var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList()
                                 .OfType<BasicFeatureLayer>().FirstOrDefault();
      if (featureLayer == null)
        return;

      var mv = MapView.Active;

      QueuedTask.Run(() =>
      {
        using (var table = featureLayer.GetTable())
        {
          using (var rc = table.Search())
          {
            //get the first feature...
            //...assuming at least one feature gets retrieved
            rc.MoveNext();
            var oid = rc.Current.GetObjectID();

            //Use DrawingOutlineType.BoundingEnvelope to retrieve a generalized
            //mask geometry or "Box". The mask will be in the same SpatRef as the map
            var mask_geom = featureLayer.GetDrawingOutline(oid, mv, DrawingOutlineType.Exact);

            //TODO - use the mask geometry...
          }
        }
      });
}
#endregion
  }
}
