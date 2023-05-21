﻿using dymaptic.GeoBlazor.Core.Components.Geometries;
using dymaptic.GeoBlazor.Core.Exceptions;
using dymaptic.GeoBlazor.Core.Model;
using dymaptic.GeoBlazor.Core.Objects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;


namespace dymaptic.GeoBlazor.Core.Test.Blazor.Components;

public class GeometryEngineTests: TestRunnerBase
{
    [Inject]
    public GeometryEngine GeometryEngine { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    [TestMethod]
    public async Task TestBufferWithProjectedPoint()
    {
        Point point = new Point(0, 0, spatialReference: new SpatialReference(103002));
        Polygon buffer = await GeometryEngine.Buffer(point, 10.0, LinearUnit.Feet);
        Assert.IsNotNull(buffer);
    }

    [TestMethod]
    public async Task TestBufferWithWgs84PointThrowsJavaScriptError()
    {
        Point point = new Point(0, 0, spatialReference: new SpatialReference(4326));
        await Assert.ThrowsExceptionAsync<JSException>(() => GeometryEngine.Buffer(point, 10.0, LinearUnit.Feet));
    }
    
    [TestMethod]
    public async Task TestBufferWithMultipleProjectedPoints()
    {
        Point point1 = new Point(0, 0, spatialReference: new SpatialReference(103002));
        Point point2 = new Point(10, 10, spatialReference: new SpatialReference(103002));
        Polygon[] buffers = await GeometryEngine.Buffer(new[] {point1, point2}, new[] {10.0, 20.0}, LinearUnit.Feet);
        Assert.IsNotNull(buffers);
        Assert.AreEqual(2, buffers.Length);
    }
    
    [TestMethod]
    public async Task TestBufferWithMultipleProjectedPointsUnioned()
    {
        Point point1 = new Point(0, 0, spatialReference: new SpatialReference(103002));
        Point point2 = new Point(10, 10, spatialReference: new SpatialReference(103002));
        Polygon[] buffers = await GeometryEngine.Buffer(new[] {point1, point2}, new[] {10.0, 20.0}, LinearUnit.Feet, true);
        Assert.IsNotNull(buffers);
        Assert.AreEqual(1, buffers.Length);
    }

    [TestMethod]
    public async Task TestClip()
    {
        Polygon boundaryPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Extent envelope = new Extent(-5, 5, -15, 15, spatialReference: new SpatialReference(103002));
        
        Polygon clippedPolygon = (Polygon)(await GeometryEngine.Clip(boundaryPolygon, envelope))!;
        Assert.IsNotNull(clippedPolygon);
    }
    
    [TestMethod]
    public async Task TestClipNoOverlapReturnsNull()
    {
        Polygon boundaryPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Extent envelope = new Extent(5, 5, 15, 15, spatialReference: new SpatialReference(103002));
        
        Polygon? clippedPolygon = (await GeometryEngine.Clip(boundaryPolygon, envelope)) as Polygon;
        Assert.IsNull(clippedPolygon);
    }

    [TestMethod]
    public async Task TestContainsTrue()
    {
        Polygon boundaryPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Point point = new Point(5, 5, spatialReference: new SpatialReference(103002));
        
        bool contains = await GeometryEngine.Contains(boundaryPolygon, point);
        Assert.IsTrue(contains);
    }

    [TestMethod]
    public async Task TestContainsFalse()
    {
        Polygon boundaryPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Point point = new Point(15, 15, spatialReference: new SpatialReference(103002));
        
        bool contains = await GeometryEngine.Contains(boundaryPolygon, point);
        Assert.IsFalse(contains);
    }

    [TestMethod]
    public async Task TestConvexHull()
    {
        Point point = new Point(0, 0, spatialReference: new SpatialReference(103002));
        
        Geometry convexHull = await GeometryEngine.ConvexHull(point);
        Assert.IsInstanceOfType<Point>(convexHull);
    }
    
    [TestMethod]
    public async Task TestConvexHullMultiplePoints()
    {
        List<Point> points = new();
        for (int i = 0; i < 10; i++)
        {
            points.Add(new Point(i, i, spatialReference: new SpatialReference(103002)));
        }
        
        Geometry[] convexHull = await GeometryEngine.ConvexHull(points);
        Assert.IsInstanceOfType<Point>(convexHull[0]);
        Assert.AreEqual(10, convexHull.Length);
    }

    [TestMethod]
    public async Task TestConvexHullMerged()
    {
        List<Point> points = new();
        for (int i = 0; i < 10; i++)
        {
            points.Add(new Point(i, i, spatialReference: new SpatialReference(103002)));
        }
        
        Geometry[] convexHull = await GeometryEngine.ConvexHull(points, true);
        Assert.IsInstanceOfType<Polygon>(convexHull[0]);
        Assert.AreEqual(1, convexHull.Length);
    }

    [TestMethod]
    public async Task TestCrossesTrue()
    {
        PolyLine polyline1 = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(10, 10)
            }
        }, spatialReference: new SpatialReference(103002));
        
        PolyLine polyline2 = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 10),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        bool crosses = await GeometryEngine.Crosses(polyline1, polyline2);
        
        Assert.IsTrue(crosses);
    }

    [TestMethod]
    public async Task TestCrossesFalse()
    {
        PolyLine polyline1 = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(10, 10)
            }
        }, spatialReference: new SpatialReference(103002));
        
        PolyLine polyline2 = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(10, 0),
                new MapPoint(20, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        bool crosses = await GeometryEngine.Crosses(polyline1, polyline2);
        
        Assert.IsFalse(crosses);
    }

    [TestMethod]
    public async Task TestCut()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(10, 0),
                new MapPoint(10, 10),
                new MapPoint(0, 10),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        PolyLine cutter = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, -5),
                new MapPoint(5, 15)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Geometry[] cut = await GeometryEngine.Cut(polygon, cutter);
        
        Assert.AreEqual(2, cut.Length);
    }

    [TestMethod]
    public async Task TestCutNotIntersectedReturnsEmpty()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(10, 0),
                new MapPoint(10, 10),
                new MapPoint(0, 10),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        PolyLine cutter = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(-5, -5),
                new MapPoint(-5, -15)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Geometry[] cut = await GeometryEngine.Cut(polygon, cutter);
        
        Assert.AreEqual(0, cut.Length);
    }

    [TestMethod]
    public async Task TestDensify()
    {
        Polygon boundaryPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Geometry densifiedPolygon = await GeometryEngine.Densify(boundaryPolygon, 1, LinearUnit.Feet);
        
        Assert.AreNotEqual(boundaryPolygon, densifiedPolygon);
    }

    [TestMethod]
    public async Task TestDifference()
    {
        Polygon boundaryPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Polygon subtractor = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5),
                new MapPoint(5, 5)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Geometry difference = await GeometryEngine.Difference(boundaryPolygon, subtractor);
        Assert.AreNotEqual(boundaryPolygon, difference);
    }
    
    [TestMethod]
    public async Task TestDifferenceMultipleInputs()
    {
        Polygon boundaryPolygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
		Polygon boundaryPolygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(2, 2),
                new MapPoint(2, 12),
                new MapPoint(12, 12),
                new MapPoint(12, 2),
                new MapPoint(2, 2)
            }
        }, spatialReference: new SpatialReference(103002));
        Polygon subtractor = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5),
                new MapPoint(5, 5)
            }
        }, spatialReference: new SpatialReference(103002));
        
        Geometry[] differences = await GeometryEngine.Difference(new[] {boundaryPolygon1, boundaryPolygon2}, subtractor);
        Assert.AreEqual(2, differences.Length);
    }

	[TestMethod]
	public async Task TestDisjointTrue()
	{
		Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(20, 20),
                new MapPoint(20, 30),
                new MapPoint(30, 30),
                new MapPoint(30, 20),
                new MapPoint(20, 20)
            }
        }, spatialReference: new SpatialReference(103002));
        
        bool disjoint = await GeometryEngine.Disjoint(polygon1, polygon2);
        
        Assert.IsTrue(disjoint);
	}

    [TestMethod]
	public async Task TestDisjointFalse()
	{
		Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5),
                new MapPoint(5, 5)
            }
        }, spatialReference: new SpatialReference(103002));
        
        bool disjoint = await GeometryEngine.Disjoint(polygon1, polygon2);
        
        Assert.IsFalse(disjoint);
	}

	[TestMethod]
	public async Task TestDistance()
    {
		Point point1 = new Point(0, 0, spatialReference: new SpatialReference(103002));
		Point point2 = new Point(10, 10, spatialReference: new SpatialReference(103002));
		
		double distance = await GeometryEngine.Distance(point1, point2, LinearUnit.Feet);
        
        Assert.AreNotEqual(0, distance);
    }

    [TestMethod]
    public async Task TestAreEqualTrue()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        bool areEqual = await GeometryEngine.AreEqual(polygon1, polygon2);
        
        Assert.IsTrue(areEqual);
    }

    [TestMethod]
    public async Task TestAreEqualFalse()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 0),
                new MapPoint(5, 10),
                new MapPoint(15, 10),
                new MapPoint(15, 0),
                new MapPoint(5, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        
        bool areEqual = await GeometryEngine.AreEqual(polygon1, polygon2);
        
        Assert.IsFalse(areEqual);
    }

    [TestMethod]
    public async Task TestExtendedSpatialReferenceInfo()
    {
        SpatialReference spatialReference = new SpatialReference(103002);
        
        SpatialReferenceInfo spatialReferenceInfo = await GeometryEngine.ExtendedSpatialReferenceInfo(spatialReference);
        
        Assert.IsNotNull(spatialReferenceInfo);
    }

    [TestMethod]
    public async Task TestFlipHorizontal()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Point flipPoint = new Point(5, 5, spatialReference: new SpatialReference(103002));
        Polygon? flippedPolygon = await GeometryEngine.FlipHorizontal(polygon, flipPoint) as Polygon;
        Assert.IsNotNull(flippedPolygon);
        Assert.AreNotEqual(polygon, flippedPolygon);
    }

    [TestMethod]
    public async Task TestFlipVertical()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(103002));
        Point flipPoint = new Point(5, 5, spatialReference: new SpatialReference(103002));
        Polygon? flippedPolygon = await GeometryEngine.FlipVertical(polygon, flipPoint) as Polygon;
        Assert.IsNotNull(flippedPolygon);
        Assert.AreNotEqual(polygon, flippedPolygon);
    }

    [TestMethod]
    public async Task TestGeneralize()
    {
        Polygon complexPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(0, 0)
            },
            new MapPath
            {
                new MapPoint(2, 2),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(2, 2)
            },
            new MapPath
            {
                new MapPoint(4, 4),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(_random.NextDouble(), _random.NextDouble()),
                new MapPoint(4, 4)
            }
        }, spatialReference: new SpatialReference(103002));

        Polygon? generalizedPolygon =
            await GeometryEngine.Generalize(complexPolygon, 1, true, 
                LinearUnit.Feet) as Polygon;
        Assert.IsNotNull(generalizedPolygon);
        Assert.AreNotEqual(complexPolygon, generalizedPolygon);
        Assert.IsTrue(complexPolygon.Rings.Select(r => r.Count).Sum() > 
            generalizedPolygon.Rings.Select(r => r.Count).Sum());
    }

    [TestMethod]
    public async Task TestGeodesicArea()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        });
        
        double area = await GeometryEngine.GeodesicArea(polygon, ArealUnit.SquareFeet);
        
        Assert.AreNotEqual(0, area);
    }

    [TestMethod]
    public async Task TestGeodesicBuffer()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        });
        
        Polygon bufferedPolygon = await GeometryEngine.GeodesicBuffer(polygon, 10, LinearUnit.Feet);
        
        Assert.IsNotNull(bufferedPolygon);
        
        Assert.AreNotEqual(polygon, bufferedPolygon);
    }

    [TestMethod]
    public async Task TestGeodesicBufferMultiplePolygons()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        });
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 0),
                new MapPoint(5, 10),
                new MapPoint(15, 10),
                new MapPoint(15, 0),
                new MapPoint(5, 0)
            }
        });

        Geometry[] bufferedGeometries =
            await GeometryEngine.GeodesicBuffer(new[] { polygon1, polygon2 }, new double[] { 10, 15 }, 
                LinearUnit.Feet);
        
        Assert.IsNotNull(bufferedGeometries);
        Assert.AreEqual(2, bufferedGeometries.Length);
        Assert.AreNotEqual(bufferedGeometries[0], bufferedGeometries[1]);
    }

    [TestMethod]
    public async Task TestGeodesicDensify()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon? densifiedPolygon = await GeometryEngine.GeodesicDensify(polygon, 100, LinearUnit.Feet) as Polygon;
        
        Assert.IsNotNull(densifiedPolygon);
        Assert.AreNotEqual(densifiedPolygon, polygon);
    }

    [TestMethod]
    public async Task TestGeodesicLength()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        });
        
        double length = await GeometryEngine.GeodesicLength(polygon, LinearUnit.Feet);
        
        Assert.IsTrue(length > 0);
    }

    [TestMethod]
    public async Task TestIntersectTrue()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Polygon intersectingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5)
            }
        });
        
        Polygon? intersect = await GeometryEngine.Intersect(polygon, intersectingPolygon) as Polygon;

        Assert.IsNotNull(intersect);
    }

    [TestMethod]
    public async Task TestIntersectFalse()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Polygon intersectingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(15, 15),
                new MapPoint(15, 25),
                new MapPoint(25, 25),
                new MapPoint(25, 15)
            }
        });
        
        Polygon? intersect = await GeometryEngine.Intersect(polygon, intersectingPolygon) as Polygon;

        Assert.IsNull(intersect);
    }

    [TestMethod]
    public async Task TestIntersectMultipleGeometries()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        PolyLine polyline = new PolyLine(new MapPath[] 
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5)
            }
        });
        
        Polygon intersectingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5)
            }
        });
        
        Geometry[] intersect = await GeometryEngine.Intersect(new Geometry[] {polygon, polyline}, intersectingPolygon);

        Assert.IsNotNull(intersect);
        Assert.AreEqual(2, intersect.Length);
        Assert.IsInstanceOfType<Polygon>(intersect[0]);
        Assert.IsInstanceOfType<PolyLine>(intersect[1]);
    }

    [TestMethod]
    public async Task TestIntersectsTrue()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Polygon intersectingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5)
            }
        });
        
        bool intersects = await GeometryEngine.Intersects(polygon, intersectingPolygon);
        
        Assert.IsTrue(intersects);
    }

    [TestMethod]
    public async Task TestIntersectsFalse()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Polygon intersectingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(15, 15),
                new MapPoint(15, 25),
                new MapPoint(25, 25),
                new MapPoint(25, 15)
            }
        });
        
        bool intersects = await GeometryEngine.Intersects(polygon, intersectingPolygon);
        
        Assert.IsFalse(intersects);
    }

    [TestMethod]
    public async Task TestIsSimpleTrue()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        bool isSimple = await GeometryEngine.IsSimple(polygon);
        
        Assert.IsTrue(isSimple);
    }

    [TestMethod]
    public async Task TestIsSimpleFalse()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        });
        
        bool isSimple = await GeometryEngine.IsSimple(polygon);
        
        Assert.IsFalse(isSimple);
    }

    [TestMethod]
    public async Task TestNearestCoordinate()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Point point = new Point(15, 15);
        
        NearestPointResult result = await GeometryEngine.NearestCoordinate(polygon, point);
        
        Assert.AreEqual(10, result.Coordinate.X);
        Assert.AreEqual(10,result.Coordinate.Y);
        Assert.AreNotEqual(4, result.VertexIndex);
    }

    [TestMethod]
    public async Task TestNearestVertex()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Point point = new Point(15, 5);
        
        NearestPointResult result = await GeometryEngine.NearestVertex(polygon, point);
        
        Assert.AreEqual(10, result.Coordinate.X);
        Assert.AreEqual(10,result.Coordinate.Y);
        Assert.AreEqual(2, result.VertexIndex);
    }

    [TestMethod]
    public async Task TestNearestVertices()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        });
        
        Point point = new Point(15, 5);
        
        NearestPointResult[] result = await GeometryEngine.NearestVertices(polygon, point, 200, 100);

        Assert.AreEqual(6, result.Length);
    }
    
    [TestMethod]
    public async Task TestOffset()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Geometry offset = await GeometryEngine.Offset(polygon, 10, LinearUnit.Feet, JoinType.Bevel);
        
        Assert.IsNotNull(offset);
        Assert.AreNotEqual(polygon, offset);
    }

    [TestMethod]
    public async Task TestOffsetMultipleGeometries()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Geometry[] geometries = new Geometry[] { polygon1, polygon2 };
        
        Geometry[] offset = await GeometryEngine.Offset(geometries, 10, LinearUnit.Feet, JoinType.Bevel);
        
        Assert.IsNotNull(offset);
        
        foreach (Geometry geometry in offset)
        {
            Assert.AreNotEqual(polygon1, geometry);
            Assert.AreNotEqual(polygon2, geometry);
        }
    }

    [TestMethod]
    public async Task TestOverlapsTrue()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Polygon overlappingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(5, 5),
                new MapPoint(5, 15),
                new MapPoint(15, 15),
                new MapPoint(15, 5)
            }
        });
        
        bool overlaps = await GeometryEngine.Overlaps(polygon, overlappingPolygon);
        
        Assert.IsTrue(overlaps);
    }

    [TestMethod]
    public async Task TestOverlapsFalse()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        });
        
        Polygon overlappingPolygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(15, 15),
                new MapPoint(15, 25),
                new MapPoint(25, 25),
                new MapPoint(25, 15)
            }
        });
        
        bool overlaps = await GeometryEngine.Overlaps(polygon, overlappingPolygon);
        
        Assert.IsFalse(overlaps);
    }
    
    [TestMethod]
    public async Task TestPlanarArea(){
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        double area = await GeometryEngine.PlanarArea(polygon, ArealUnit.SquareKilometers);
        
        Assert.IsTrue(area > 0);
    }

    [TestMethod]
    public async Task TestPlanarLength()
    {
        PolyLine polyline = new PolyLine(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        double length = await GeometryEngine.PlanarLength(polyline, LinearUnit.Kilometers);
        
        Assert.IsTrue(length > 0);
    }
    
    [TestMethod]
    public async Task TestRelateTrue()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        bool relate = await GeometryEngine.Relate(polygon1, polygon2, "T*F**F***");
        
        Assert.IsTrue(relate);
    }
    
    [TestMethod]
    public async Task TestRelateFalse()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(100, 100),
                new MapPoint(200, 100),
                new MapPoint(200, 200),
                new MapPoint(100, 100)
            }
        }, spatialReference: new SpatialReference(102100));
        
        bool relate = await GeometryEngine.Relate(polygon1, polygon2, "T*F**F***");
        
        Assert.IsFalse(relate);
    }
    
    [TestMethod]
    public async Task TestRotate()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Point origin = new Point(0, 0, spatialReference: new SpatialReference(102100));
        
        Polygon? rotatedPolygon = await GeometryEngine.Rotate(polygon, 45, origin) as Polygon;
        
        Assert.IsNotNull(rotatedPolygon);
        Assert.AreNotEqual(polygon, rotatedPolygon);
    }
    
    [TestMethod]
    public async Task TestSimplify()
    {
        Polygon polygon = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Geometry? simplifiedGeometry = await GeometryEngine.Simplify(polygon);
        
        Assert.IsNotNull(simplifiedGeometry);
        Assert.AreNotEqual(polygon, simplifiedGeometry);
    }
    
    [TestMethod]
    public async Task TestSymmetricDifference()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(100, 100),
                new MapPoint(200, 100),
                new MapPoint(200, 200),
                new MapPoint(100, 100)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Geometry? symmetricDifference = await GeometryEngine.SymmetricDifference(polygon1, polygon2);
        
        Assert.IsNotNull(symmetricDifference);
        Assert.AreNotEqual(polygon1, symmetricDifference);
        Assert.AreNotEqual(polygon2, symmetricDifference);
    }

    [TestMethod]
    public async Task TestSymmetricDifferences()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(-10, 10),
                new MapPoint(10, 10),
                new MapPoint(5, 5),
                new MapPoint(-10, 5),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(100, 100),
                new MapPoint(200, 100),
                new MapPoint(200, 200),
                new MapPoint(100, 100)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon3 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(100, 100),
                new MapPoint(200, 100),
                new MapPoint(200, 200),
                new MapPoint(100, 100)
            }
        }, spatialReference: new SpatialReference(102100));

        Geometry[]? symmetricDifferences =
            await GeometryEngine.SymmetricDifference(new Geometry[] { polygon1, polygon2 }, polygon3);
        
        Assert.IsNotNull(symmetricDifferences);
        
        foreach (Geometry symmetricDifference in symmetricDifferences)
        {
            Assert.AreNotEqual(polygon1, symmetricDifference);
            Assert.AreNotEqual(polygon2, symmetricDifference);
            Assert.AreNotEqual(polygon3, symmetricDifference);
        }
    }
    
    [TestMethod]
    public async Task TestTouchesTrue()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(10, 0),
                new MapPoint(20, 0),
                new MapPoint(20, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        bool touches = await GeometryEngine.Touches(polygon1, polygon2);
        
        Assert.IsTrue(touches);
    }
    
    [TestMethod]
    public async Task TestTouchesFalse()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(20, 20),
                new MapPoint(30, 20),
                new MapPoint(30, 30),
                new MapPoint(20, 30),
                new MapPoint(20, 20)
            }
        }, spatialReference: new SpatialReference(102100));
        
        bool touches = await GeometryEngine.Touches(polygon2, polygon1);
        
        Assert.IsFalse(touches);
    }
    
    [TestMethod]
    public async Task TestUnion()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(10, 0),
                new MapPoint(20, 0),
                new MapPoint(20, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Geometry? union = await GeometryEngine.Union(polygon1, polygon2);
        
        Assert.IsNotNull(union);
        Assert.AreNotEqual(polygon1, union);
        Assert.AreNotEqual(polygon2, union);
    }
    
    [TestMethod]
    public async Task TestWithinTrue()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(1, 1),
                new MapPoint(1, 9),
                new MapPoint(9, 9),
                new MapPoint(9, 1),
                new MapPoint(1, 1)
            }
        }, spatialReference: new SpatialReference(102100));
        
        bool within = await GeometryEngine.Within(polygon2, polygon1);
        
        Assert.IsTrue(within);
    }
    
    [TestMethod]
    public async Task TestWithinFalse()
    {
        Polygon polygon1 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(0, 0),
                new MapPoint(0, 10),
                new MapPoint(10, 10),
                new MapPoint(10, 0),
                new MapPoint(0, 0)
            }
        }, spatialReference: new SpatialReference(102100));
        
        Polygon polygon2 = new Polygon(new MapPath[]
        {
            new MapPath
            {
                new MapPoint(1, 1),
                new MapPoint(1, 9),
                new MapPoint(9, 9),
                new MapPoint(9, 1),
                new MapPoint(1, 1)
            }
        }, spatialReference: new SpatialReference(102100));
        
        bool within = await GeometryEngine.Within(polygon1, polygon2);
        
        Assert.IsFalse(within);
    }

    private readonly Random _random = new();
}