using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.KnowledgeGraph;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Layouts.Helper
{
  /// <summary>
  /// This class contains a collection of utility methods that can be used for working with ArcGIS Pro layouts.
  /// </summary>
  /// <remarks>
  /// These methods are ready to be used in your ArcGIS Pro add-in projects as helpers for creating and manipulating layout elements.
  /// Note that crefs...
  public static class LayoutHelper 
  {   
    // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleGraphic
    // cref: ArcGIS.Core.Geometry.EllipticArcBuilderEx.CreateCircle
    // cref: ArcGIS.Core.Geometry.ArcOrientation
    /// <summary>
    /// Creates a circular graphic element with predefined geometry and symbology.
    /// </summary>
    /// <remarks>The method creates a circle with a fixed center at (2, 4) and a radius of 0.5 units. The
    /// circle is styled with a solid red fill and a dashed black outline. The graphic is created on a background thread
    /// using the ArcGIS Pro QueuedTask framework.</remarks>
    /// <param name="container">The container to which the graphic element will be added. This parameter is not used in the current
    /// implementation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CIMGraphic"/>
    /// representing the circular graphic.</returns>
    public async static Task<CIMGraphic> CreateCircleGraphicAsync(IElementContainer container)
    {
      return await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D center = new Coordinate2D(2, 4);
        EllipticArcSegment circle_seg = EllipticArcBuilderEx.CreateCircle(
          new Coordinate2D(2, 4), 0.5, ArcOrientation.ArcClockwise, null);
        var circle_poly = PolygonBuilderEx.CreatePolygon(PolylineBuilderEx.CreatePolyline(circle_seg));

        //PolylineBuilderEx.CreatePolyline(cir, AttributeFlags.AllAttributes));
        //Set symbology, create and add element to layout
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
          ColorFactory.Instance.BlackRGB, 2.0, SimpleLineStyle.Dash);

        CIMPolygonSymbol circleSym = SymbolFactory.Instance.ConstructPolygonSymbol(
          ColorFactory.Instance.RedRGB, SimpleFillStyle.Solid, outline);
        SymbolFactory.Instance.ConstructPolygonSymbol(null,
          SymbolFactory.Instance.ConstructStroke(ColorFactory.Instance.RedRGB, 2));

         return GraphicFactory.Instance.CreateSimpleGraphic(circle_poly, circleSym);
      });
    }
    // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleTextGraphic
    // cref: ArcGIS.Desktop.Layouts.TextType
    /// <summary>
    /// Creates a circular text graphic and adds it to the specified container.
    /// </summary>
    /// <remarks>The circular text graphic is created with a predefined geometry, symbology, and text content.
    /// The text is styled as a circular paragraph.</remarks>
    /// <param name="container">The container to which the circular text graphic will be added. This is typically a layout or map frame.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="CIMGraphic"/>
    /// representing the circular text graphic.</returns>
    public async static Task<CIMGraphic> CreateCircleTextGraphic(IElementContainer container)
    {
      return await QueuedTask.Run( () => {  //Build geometry
        Coordinate2D center = new Coordinate2D(4.5, 4);
        var eabCir = new EllipticArcBuilderEx(center, 0.5, ArcOrientation.ArcClockwise);
        var cir = eabCir.ToSegment();

        var poly = PolygonBuilderEx.CreatePolygon(
          PolylineBuilderEx.CreatePolyline(cir, AttributeFlags.AllAttributes));

        //Set symbology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
          ColorFactory.Instance.GreenRGB, 10, "Arial", "Regular");
        string text = "Circle, circle, circle";

        return GraphicFactory.Instance.CreateSimpleTextGraphic(
          TextType.CircleParagraph, poly, sym, text);
      });
    }
    /// <summary>
    /// Creates a bezier text graphic and adds it to the specified container.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    // cref: ArcGIS.Desktop.Layouts.GraphicFactory.CreateSimpleTextGraphic
    // cref: ArcGIS.Desktop.Layouts.TextType
    public async static Task<CIMGraphic> CreateBezierTextGraphic(IElementContainer container)
    {
      return await QueuedTask.Run(() =>
      {
        //Build geometry
        Coordinate2D pt1 = new Coordinate2D(3.5, 7.5);
        Coordinate2D pt2 = new Coordinate2D(4.16, 8);
        Coordinate2D pt3 = new Coordinate2D(4.83, 7.1);
        Coordinate2D pt4 = new Coordinate2D(5.5, 7.5);
        var bez = new CubicBezierBuilderEx(pt1, pt2, pt3, pt4);
        var bezSeg = bez.ToSegment();
        Polyline bezPl = PolylineBuilderEx.CreatePolyline(bezSeg, AttributeFlags.AllAttributes);

        //Set symbology, create and add element to layout
        CIMTextSymbol sym = SymbolFactory.Instance.ConstructTextSymbol(
              ColorFactory.Instance.BlackRGB, 24, "Comic Sans MS", "Regular");

        return GraphicFactory.Instance.CreateSimpleTextGraphic(
          TextType.SplinedText, bezPl, sym, "Splined text");
      });
    }
  }
}
