using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExploration.ProSnippets
{
  /// <summary>
  /// Provides utility methods for working with animations in ArcGIS Pro, including setting animation lengths, scaling
  /// animations, retrieving interpolated values, and creating keyframes for various animation tracks.
  /// </summary>
  /// <remarks>This class contains static methods that interact with the active map view's animation. It assumes
  /// that an active map view is available and that the map contains an animation. If no active map view or animation is
  /// present, the methods will return without performing any operations.</remarks>
  public static class ProSnippetsAnimation
  {
    #region ProSnippet Group: Animations
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Duration
    // cref: ArcGIS.Desktop.Mapping.Animation.ScaleDuration(System.Double)
    #region Set Animation Length
    /// <summary>
    /// This method sets the total length of the animation to the specified length by scaling the current duration.
    /// </summary>
    /// <param name="length"></param>
    public static void SetAnimationLength(TimeSpan length)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var duration = animation.Duration;
      if (duration == TimeSpan.Zero)
        return;

      var factor = length.TotalSeconds / duration.TotalSeconds;
      animation.ScaleDuration(factor);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Duration
    // cref: ArcGIS.Desktop.Mapping.Animation.ScaleDuration(System.TimeSpan,System.TimeSpan,System.Double)
    #region Scale Animation
    /// <summary>
    /// This method scales the animation duration after a specified time by a given length.
    /// </summary>
    /// <param name="afterTime"></param>
    /// <param name="length"></param>
    public static void ScaleAnimationAfterTime(TimeSpan afterTime, TimeSpan length)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var duration = animation.Duration;
      if (duration == TimeSpan.Zero || duration <= afterTime)
        return;

      var factor = length.TotalSeconds / (duration.TotalSeconds - afterTime.TotalSeconds);
      animation.ScaleDuration(afterTime, duration, factor);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Map.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.CameraTrack
    // cref: ArcGIS.Desktop.Mapping.Track.Keyframes
    // cref: ArcGIS.Desktop.Mapping.CameraKeyframe
    #region Camera Keyframes
    /// <summary>
    /// This method retrieves all camera keyframes from the active map view's animation.
    /// </summary>
    /// <returns></returns>
    public static List<CameraKeyframe> GetCameraKeyframes()
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return null;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      return cameraTrack.Keyframes.OfType<CameraKeyframe>().ToList();
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCameraAtTime(System.TimeSpan)
    #region Interpolate Camera
    /// <summary>
    /// This method retrieves a list of interpolated cameras for each frame in the animation.
    /// </summary>
    /// <returns></returns>
    public static Task<List<Camera>> GetInterpolatedCameras()
    {
      //Return the collection representing the camera for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var cameras = new List<Camera>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          cameras.Add(mapView.Animation.GetCameraAtTime(time));
        }
        return cameras;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.TimeRange
    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCurrentTimeAtTime(System.TimeSpan)
    #region Interpolate Time
    /// <summary>
    /// This method retrieves a list of interpolated time ranges for each frame in the animation.
    /// </summary>
    /// <returns></returns>
    public static Task<List<TimeRange>> GetInterpolatedMapTimes()
    {
      //Return the collection representing the map time for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var timeRanges = new List<TimeRange>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greating than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          timeRanges.Add(mapView.Animation.GetCurrentTimeAtTime(time));
        }
        return timeRanges;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Range
    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCurrentRangeAtTime(System.TimeSpan)
    #region Interpolate Range
    /// <summary>
    /// This method retrieves a list of interpolated ranges for each frame in the animation.
    /// </summary>
    /// <returns></returns>
    public static Task<List<ArcGIS.Desktop.Mapping.Range>> GetInterpolatedMapRanges()
    {
      //Return the collection representing the map time for each frame in animation.
      return QueuedTask.Run(() =>
      {
        var mapView = MapView.Active;
        if (mapView != null || mapView.Animation == null)
          return null;

        var animation = mapView.Map.Animation;

        var ranges = new List<ArcGIS.Desktop.Mapping.Range>();
        //We will use ticks here rather than milliseconds to get the highest precision possible.
        var ticksPerFrame = Convert.ToInt64(animation.Duration.Ticks / (animation.NumberOfFrames - 1));
        for (int i = 0; i < animation.NumberOfFrames; i++)
        {
          var time = TimeSpan.FromTicks(i * ticksPerFrame);
          //Because of rounding for ticks the last calculated time may be greeting than the duration.
          if (time > animation.Duration)
            time = animation.Duration;
          ranges.Add(mapView.Animation.GetCurrentRangeAtTime(time));
        }
        return ranges;
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.CameraTrack
    // cref: ArcGIS.Desktop.Mapping.CameraTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Camera,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
    // cref: ArcGIS.Core.CIM.AnimationTransition
    #region Create Camera Keyframe
    /// <summary>
    /// This method creates a camera keyframe at the specified time using the current camera of the active map view.
    /// </summary>
    /// <param name="atTime"></param>
    public static void CreateCameraKeyframe(TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      cameraTrack.CreateKeyframe(mapView.Camera, atTime, ArcGIS.Core.CIM.AnimationTransition.FixedArc);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.TimeTrack
    // cref: ArcGIS.Core.CIM.AnimationTransition
    // cref: ArcGIS.Desktop.Mapping.TimeTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.TimeRange,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
    #region Create Time Keyframe
    /// <summary>
    /// This method creates a time keyframe at the specified time using the current map time of the active map view.
    /// </summary>
    /// <param name="atTime"></param>
    public static void CreateTimeKeyframe(TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var timeTrack = animation.Tracks.OfType<TimeTrack>().First(); //There will always be only 1 TimeTrack in the animation.
      timeTrack.CreateKeyframe(mapView.Time, atTime, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.RangeTrack
    // cref: ArcGIS.Core.CIM.AnimationTransition
    // cref: ArcGIS.Desktop.Mapping.RangeTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Range,System.TimeSpan,ArcGIS.Core.CIM.AnimationTransition)
    #region Create Range Keyframe
    /// <summary>
    /// This method creates a range keyframe at the specified time using the provided range.
    /// </summary>
    /// <param name="range"></param>
    /// <param name="atTime"></param>
    public static void CreateRangeKeyframe(ArcGIS.Desktop.Mapping.Range range, TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var rangeTrack = animation.Tracks.OfType<RangeTrack>().First(); //There will always be only 1 RangeTrack in the animation.
      rangeTrack.CreateKeyframe(range, atTime, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation
    // cref: ArcGIS.Desktop.Mapping.Animation.Tracks
    // cref: ArcGIS.Desktop.Mapping.LayerTrack
    // cref: ArcGIS.Core.CIM.AnimationTransition
    // cref: ArcGIS.Desktop.Mapping.LayerTrack.CreateKeyframe(ArcGIS.Desktop.Mapping.Layer,System.TimeSpan,System.Boolean,System.Double,ArcGIS.Core.CIM.AnimationTransition)
    #region Create Layer Keyframe
    /// <summary>
    /// This method creates a layer keyframe at the specified time for the given layer, setting its transparency.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="transparency"></param>
    /// <param name="atTime"></param>
    public static void CreateLayerKeyframe(Layer layer, double transparency, TimeSpan atTime)
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return;

      var animation = mapView.Map.Animation;
      var layerTrack = animation.Tracks.OfType<LayerTrack>().First(); //There will always be only 1 LayerTrack in the animation.
      layerTrack.CreateKeyframe(layer, atTime, true, transparency, ArcGIS.Core.CIM.AnimationTransition.Linear);
    }
    #endregion
  }
}
