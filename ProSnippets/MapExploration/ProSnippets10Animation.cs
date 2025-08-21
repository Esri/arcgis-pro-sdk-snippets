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

// Ignore Spelling: Keyframes Keyframe

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
    /// Sets the total duration of the animation for the active map view.
    /// </summary>
    /// <param name="length">The desired total duration of the animation as a <see cref="TimeSpan"/>. Must be greater than zero.</param>
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
    /// Scales the duration of the animation on the active map view after a specified time.
    /// </summary>
    /// <param name="afterTime">The time after which the animation duration should be scaled.</param>
    /// <param name="length">The desired length of the animation after the specified time.</param>
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
    /// Retrieves the list of camera keyframes from the active map view's animation.
    /// </summary>
    /// <returns>A list of <see cref="CameraKeyframe"/> objects representing the camera keyframes in the active map view's
    /// animation. Returns <see langword="null"/> if there is no active map view.</returns>
    public static List<CameraKeyframe> GetCameraKeyframes()
    {
      var mapView = MapView.Active;
      if (mapView != null)
        return null;

      var animation = mapView.Map.Animation;
      var cameraTrack = animation.Tracks.OfType<CameraTrack>().First(); //There will always be only 1 CameraTrack in the animation.
      return [.. cameraTrack.Keyframes.OfType<CameraKeyframe>()];
    }
    #endregion

    // cref: ArcGIS.Desktop.Mapping.Animation.NumberOfFrames
    // cref: ArcGIS.Desktop.Mapping.ViewAnimation.GetCameraAtTime(System.TimeSpan)
    #region Interpolate Camera
    /// <summary>
    /// Retrieves a collection of <see cref="Camera"/> objects representing the camera position for each frame in the
    /// active map view's animation.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is a list of <see cref="Camera"/> objects, 
    /// where each object corresponds to a frame in the animation. Returns <see langword="null"/> if there is no active
    /// map view or if the map view does not have an animation.</returns>
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
    /// Asynchronously retrieves a collection of time ranges representing the map time for each frame in the animation.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="ArcGIS.Desktop.Mapping.TimeRange"/> objects,  where each time range corresponds to a frame in the
    /// animation. Returns <see langword="null"/> if no active map view or animation is available.</returns>
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
    /// Asynchronously retrieves a collection of map ranges interpolated for each frame in the active map's animation.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result is a list of <see
    /// cref="ArcGIS.Desktop.Mapping.Range"/> objects, where each range corresponds to a frame in the animation. Returns
    /// <see langword="null"/> if there is no active map view or if the active map does not have an animation.</returns>
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
    /// Creates a keyframe for the camera in the active map view's animation at the specified time.
    /// </summary>
    /// <param name="atTime">The time within the animation at which the keyframe should be created.</param>
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
    /// Creates a time keyframe in the animation at the specified time.
    /// </summary>
    /// <param name="atTime">The time within the animation's duration at which the keyframe will be created.</param>
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
    /// Creates a keyframe for the specified range at the given time in the animation timeline.
    /// </summary>
    /// <param name="range">The range to associate with the keyframe. This defines the range values to be applied at the keyframe.</param>
    /// <param name="atTime">The time within the animation timeline at which the keyframe will be created.</param>
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
    /// Creates a keyframe for the specified layer in the animation timeline with the given transparency and time.
    /// </summary>
    /// <param name="layer">The <see cref="Layer"/> for which the keyframe will be created. This cannot be <see langword="null"/>.</param>
    /// <param name="transparency">The transparency level to apply to the layer at the keyframe, specified as a value between 0 (fully opaque) and
    /// 1 (fully transparent).</param>
    /// <param name="atTime">The <see cref="TimeSpan"/> indicating the time in the animation timeline at which the keyframe will be created.</param>
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
