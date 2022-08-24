// noinspection JSUnresolvedFunction
const esriConfig = require("esri/config");
const geometryEngine = require("esri/geometry/geometryEngine");
const projection = require("esri/geometry/projection");
const Basemap = require("esri/Basemap");
const Map = require("esri/Map");
const SceneView = require("esri/views/SceneView");
const MapView = require("esri/views/MapView");
const WebMap = require("esri/WebMap");
const WebScene = require("esri/WebScene");
const route = require("esri/rest/route");
const RouteParameters = require("esri/rest/support/RouteParameters");
const FeatureSet = require("esri/rest/support/FeatureSet");
const ServiceAreaParameters = require("esri/rest/support/ServiceAreaParameters");
const serviceArea = require("esri/rest/serviceArea");
const Graphic = require("esri/Graphic");
const Point = require("esri/geometry/Point");
const SpatialReference = require("esri/geometry/SpatialReference");
const locator = require("esri/rest/locator");
const BasemapGallery = require("esri/widgets/BasemapGallery");
const ScaleBar = require("esri/widgets/ScaleBar");
const Legend = require("esri/widgets/Legend");
const PortalBasemapsSource = require("esri/widgets/BasemapGallery/support/PortalBasemapsSource");
const Portal = require("esri/portal/Portal");
const BasemapToggle = require("esri/widgets/BasemapToggle");
const Search = require("esri/widgets/Search");
const Locate = require("esri/widgets/Locate");
const GraphicsLayer = require("esri/layers/GraphicsLayer");
const FeatureLayer = require("esri/layers/FeatureLayer");
const VectorTileLayer = require("esri/layers/VectorTileLayer");
const TileLayer = require("esri/layers/TileLayer");
const GeoJSONLayer = require("esri/layers/GeoJSONLayer");
const PopupTemplate = require("esri/PopupTemplate");
const Query = require("esri/rest/support/Query");
export let arcGisObjectRefs = {};
export let dotNetRefs = {};
export let queryLayer;
export async function buildMapView(id, dotNetReference, long, lat, rotation, mapObject, zoom, scale, apiKey, mapType, widgets, graphics, spatialReference, zIndex, tilt) {
    console.log("render map");
    try {
        setWaitCursor(id);
        let dotNetRef = dotNetReference;
        dotNetRefs[id] = dotNetRef;
        esriConfig.apiKey = apiKey;
        disposeView(id);
        let view;
        let basemap;
        let basemapLayers = [];
        if (!mapType.startsWith('web')) {
            if (mapObject.arcGISDefaultBasemap !== undefined) {
                basemap = mapObject.arcGISDefaultBasemap;
            }
            else if (mapObject.basemap?.portalItem?.id !== undefined &&
                mapObject.basemap?.portalItem?.id !== null) {
                basemap = new Basemap({
                    portalItem: {
                        id: mapObject.basemap.portalItem.id
                    }
                });
            }
            else {
                if (mapObject.basemap?.layers.length > 0) {
                    for (let i = 0; i < mapObject.basemap.layers.length; i++) {
                        const layerObject = mapObject.basemap.layers[i];
                        basemapLayers.push(layerObject);
                    }
                }
                basemap = new Basemap({
                    baseLayers: []
                });
            }
        }
        switch (mapType) {
            case 'webmap':
                const webMap = new WebMap({
                    portalItem: {
                        id: mapObject.portalItem.id
                    }
                });
                view = new MapView({
                    container: `map-container-${id}`,
                    map: webMap
                });
                break;
            case 'webscene':
                const webScene = new WebScene({
                    portalItem: {
                        id: mapObject.portalItem.id
                    }
                });
                view = new SceneView({
                    container: `map-container-${id}`,
                    map: webScene
                });
                break;
            case 'scene':
                const scene = new Map({
                    basemap: basemap,
                    ground: mapObject.ground
                });
                view = new SceneView({
                    container: `map-container-${id}`,
                    map: scene,
                    camera: {
                        position: {
                            x: long,
                            y: lat,
                            z: zIndex //Meters
                        },
                        tilt: tilt
                    }
                });
                break;
            default:
                const map = new Map({
                    basemap: basemap,
                    ground: mapObject.ground
                });
                let center;
                let spatialRef;
                if (spatialReference !== undefined && spatialReference !== null) {
                    spatialRef = new SpatialReference({
                        wkid: spatialReference.wkid
                    });
                    center = new Point({
                        latitude: lat,
                        longitude: long,
                        spatialReference: spatialRef
                    });
                    await resetCenterToSpatialReference(center, spatialRef, id);
                }
                else {
                    center = [long, lat];
                }
                view = new MapView({
                    map: map,
                    center: center,
                    container: `map-container-${id}`,
                    rotation: rotation
                });
                if (scale !== undefined && scale !== null) {
                    view.scale = scale;
                }
                else {
                    view.zoom = zoom;
                }
                if (spatialRef !== undefined && spatialRef !== null) {
                    view.spatialReference = spatialRef;
                }
                break;
        }
        arcGisObjectRefs[id] = view;
        if (mapObject.layers !== undefined && mapObject.layers !== null) {
            mapObject.layers.forEach(layerObject => {
                addLayer(layerObject, id);
            });
        }
        basemapLayers.forEach(l => addLayer(l, id, true));
        widgets.forEach(widget => {
            addWidget(widget, id);
        });
        graphics.forEach(graphicObject => {
            addGraphic(graphicObject, id);
        });
        view.on('click', (evt) => {
            dotNetRef.invokeMethodAsync('OnJavascriptClick', buildDotNetPoint(evt.mapPoint));
        });
        view.on('pointer-move', (evt) => {
            let point = view.toMap({
                x: evt.x,
                y: evt.y
            });
            dotNetRef.invokeMethodAsync('OnJavascriptPointerMove', buildDotNetPoint(point));
        });
        view.watch('spatialReference', () => {
            dotNetRef.invokeMethodAsync('OnSpatialReferenceChanged', view.spatialReference);
        });
        dotNetReference.invokeMethodAsync('OnViewRendered');
        unsetWaitCursor(id);
    }
    catch (error) {
        logError(error, id);
    }
}
export function disposeView(viewId) {
    try {
        Object.values(arcGisObjectRefs).forEach(o => o?.destroy());
        arcGisObjectRefs = {};
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function disposeMapComponent(componentId, viewId) {
    try {
        let component = arcGisObjectRefs[componentId];
        switch (component?.declaredClass) {
            case 'esri.Graphic':
                let graphic = component;
                graphic.layer.graphics.remove(graphic);
                break;
        }
        component?.destroy();
        delete arcGisObjectRefs[componentId];
        let view = arcGisObjectRefs[viewId];
        view?.ui?.remove(component);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function updateView(property, value, viewId) {
    try {
        setWaitCursor(viewId);
        let view = arcGisObjectRefs[viewId];
        switch (property) {
            case 'Longitude':
                view.center = new Point({ longitude: value, latitude: view.center.latitude });
                break;
            case 'Latitude':
                view.center = new Point({ longitude: view.center.longitude, latitude: value });
                break;
            case 'Zoom':
                view.zoom = value;
                break;
            case 'Rotation':
                view.rotation = value;
        }
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function queryFeatureLayer(queryObject, layerObject, symbol, popupTemplateObject, viewId) {
    try {
        setWaitCursor(viewId);
        let query = new Query({
            where: queryObject.where,
            outFields: queryObject.outFields,
            returnGeometry: queryObject.returnGeometry,
            spatialRelationship: queryObject.spatialRelationship,
        });
        if (queryObject.useViewExtent) {
            let view = arcGisObjectRefs[viewId];
            query.geometry = view.extent;
        }
        else if (queryObject.geometry !== undefined && queryObject.geometry !== null) {
            query.geometry = queryObject.geometry;
        }
        let popupTemplate = buildPopupTemplate(popupTemplateObject);
        addLayer(layerObject, viewId, false, true, () => {
            displayQueryResults(query, symbol, popupTemplate, viewId);
        });
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function updateGraphicsLayer(layerObject, layerId, viewId) {
    try {
        setWaitCursor(viewId);
        console.log('update graphics layer');
        removeGraphicsLayer(viewId, layerId);
        addLayer(layerObject, viewId);
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function removeGraphicsLayer(viewId, layerId) {
    try {
        setWaitCursor(viewId);
        console.log('remove graphics layer');
        let view = arcGisObjectRefs[viewId];
        let layer = arcGisObjectRefs[layerId];
        view?.map?.remove(layer);
        layer?.destroy();
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function updateGraphic(graphicObject, layerIndex, viewId) {
    try {
        setWaitCursor(viewId);
        let oldGraphic = arcGisObjectRefs[graphicObject.id];
        let gLayer = null;
        let view = arcGisObjectRefs[viewId];
        if (layerIndex === undefined || layerIndex === null) {
            if (oldGraphic !== undefined && oldGraphic !== null) {
                view.graphics.remove(oldGraphic);
            }
            else {
                view.graphics.removeAt(graphicObject.graphicIndex);
            }
        }
        else {
            gLayer = view.map.layers.filter(l => l.type === "graphics")[layerIndex];
            if (gLayer !== null) {
                if (oldGraphic !== null) {
                    gLayer.graphics.remove(oldGraphic);
                }
                else {
                    gLayer.graphics.removeAt(graphicObject.graphicIndex);
                }
            }
        }
        addGraphic(graphicObject, viewId, gLayer);
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function removeGraphicAtIndex(index, layerIndex, viewId) {
    try {
        setWaitCursor(viewId);
        let view = arcGisObjectRefs[viewId];
        if (layerIndex === undefined || layerIndex === null) {
            view.graphics.removeAt(index);
        }
        else {
            let gLayer = view?.map?.layers.filter(l => l.type === "graphics")[layerIndex];
            gLayer?.graphics?.removeAt(index);
        }
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function updateFeatureLayer(layerObject, viewId) {
    try {
        setWaitCursor(viewId);
        removeFeatureLayer(layerObject, viewId);
        addLayer(layerObject, viewId);
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function removeFeatureLayer(layerObject, viewId) {
    try {
        setWaitCursor(viewId);
        let featureLayer = arcGisObjectRefs[layerObject.id];
        let view = arcGisObjectRefs[viewId];
        view?.map?.remove(featureLayer);
        featureLayer?.destroy();
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function findPlaces(addressQueryParams, symbol, popupTemplateObject, viewId) {
    try {
        setWaitCursor(viewId);
        let view = arcGisObjectRefs[viewId];
        locator.addressToLocations(addressQueryParams.locatorUrl, {
            location: view.center,
            categories: addressQueryParams.categories,
            maxLocations: addressQueryParams.maxLocations,
            outFields: addressQueryParams.outFields,
            address: null
        })
            .then(function (results) {
            view.popup.close();
            view.graphics.removeAll();
            let popupTemplate = buildPopupTemplate(popupTemplateObject);
            results.forEach(function (result) {
                view.graphics.add(new Graphic({
                    attributes: result.attributes,
                    geometry: result.location,
                    symbol: symbol,
                    popupTemplate: popupTemplate
                }));
            });
            unsetWaitCursor(viewId);
        }).catch((error) => {
            logError(error, viewId);
        });
    }
    catch (error) {
        logError(error, viewId);
    }
}
export async function showPopup(popupTemplateObject, location, viewId) {
    try {
        setWaitCursor(viewId);
        let popupTemplate = buildPopupTemplate(popupTemplateObject);
        arcGisObjectRefs[viewId].popup.open({
            title: popupTemplate.title,
            content: popupTemplate.content,
            location: location
        });
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export async function showPopupWithGraphic(graphicObject, options, viewId) {
    try {
        setWaitCursor(viewId);
        addGraphic(graphicObject, viewId);
        let view = arcGisObjectRefs[viewId];
        let graphic = arcGisObjectRefs[graphicObject.id];
        view.popup.dockOptions = options.dockOptions;
        view.popup.visibleElements = options.visibleElements;
        view.popup.open({
            features: [graphic]
        });
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function addGraphic(graphicObject, viewId, graphicsLayer) {
    try {
        setWaitCursor(viewId);
        let graphic = createGraphic(graphicObject);
        let view = arcGisObjectRefs[viewId];
        if (graphicsLayer === undefined || graphicsLayer === null) {
            view.graphics.add(graphic);
        }
        else if (typeof (graphicsLayer) === 'object') {
            graphicsLayer.add(graphic);
        }
        else {
            view.map.layers.filter(l => l.type === "graphics")[graphicsLayer].add(graphic);
        }
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function clearViewGraphics(viewId) {
    try {
        setWaitCursor(viewId);
        arcGisObjectRefs[viewId].graphics.removeAll();
        unsetWaitCursor(viewId);
    }
    catch (error) {
        logError(error, viewId);
    }
}
export async function drawRouteAndGetDirections(routeUrl, routeSymbol, viewId) {
    try {
        setWaitCursor(viewId);
        let view = arcGisObjectRefs[viewId];
        const routeParams = new RouteParameters({
            stops: new FeatureSet({
                features: view.graphics.toArray()
            }),
            returnDirections: true
        });
        let data = await route.solve(routeUrl, routeParams);
        data.routeResults.forEach(function (result) {
            result.route.symbol = routeSymbol;
            view.graphics.add(result.route);
        });
        const directions = [];
        if (data.routeResults.length > 0) {
            data.routeResults[0].directions?.features.forEach(function (result, i) {
                let direction = {
                    text: result.attributes.text,
                    length: result.attributes.length,
                    time: result.attributes.time,
                    ETA: result.attributes.ETA,
                    maneuverType: result.attributes.maneuverType
                };
                directions.push(direction);
            });
        }
        unsetWaitCursor(viewId);
        return directions;
    }
    catch (error) {
        logError(error, viewId);
    }
    return [];
}
export function solveServiceArea(url, driveTimeCutoffs, serviceAreaSymbol, viewId) {
    try {
        setWaitCursor(viewId);
        let view = arcGisObjectRefs[viewId];
        const featureSet = new FeatureSet({
            features: [view.graphics[0]]
        });
        const taskParameters = new ServiceAreaParameters({
            facilities: featureSet,
            defaultBreaks: driveTimeCutoffs,
            trimOuterPolygon: true,
            outSpatialReference: view.spatialReference
        });
        serviceArea.solve(url, taskParameters)
            .then(function (result) {
            if (result.serviceAreaPolygons.length) {
                result.serviceAreaPolygons.forEach(function (graphic) {
                    graphic.symbol = serviceAreaSymbol;
                    view.graphics.add(graphic, 0);
                });
            }
            unsetWaitCursor(viewId);
        }, function (error) {
            logError(error, viewId);
        });
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function getAllGraphics(layerIndex, viewId) {
    try {
        let dotNetGraphics = [];
        let view = arcGisObjectRefs[viewId];
        view.map.layers.filter(l => l.type === "graphics")[layerIndex].graphics?._items.forEach(g => {
            let dotNetGraphic = buildDotNetGraphic(g);
            dotNetGraphics.push(dotNetGraphic);
        });
        return dotNetGraphics;
    }
    catch (error) {
        logError(error, viewId);
    }
    return [];
}
export function getCenter(viewId) {
    return buildDotNetPoint(arcGisObjectRefs[viewId].center);
}
export function drawWithGeodesicBufferOnPointer(cursorSymbol, bufferSymbol, geodesicBufferDistance, geodesicBufferUnit, viewId) {
    let cursorGraphicId = cursorSymbol.id;
    let bufferGraphicId = bufferSymbol.id;
    let view = arcGisObjectRefs[viewId];
    view.on('pointer-move', async (evt) => {
        let cursorPoint = view.toMap({
            x: evt.x,
            y: evt.y,
        });
        if (cursorPoint) {
            if (cursorPoint.spatialReference.wkid !== 3857 &&
                cursorPoint.spatialReference.wkid !== 4326) {
                cursorPoint = (await projection.project(cursorPoint, new SpatialReference({
                    wkid: 4326
                })));
            }
            if (!cursorPoint)
                return;
            const buffer = await geometryEngine.geodesicBuffer(cursorPoint, geodesicBufferDistance, geodesicBufferUnit);
            if (buffer) {
                try {
                    let cursorSymbolGraphic = arcGisObjectRefs[cursorGraphicId];
                    if (cursorSymbolGraphic !== undefined && cursorSymbolGraphic !== null) {
                        view.graphics.remove(cursorSymbolGraphic);
                        cursorSymbolGraphic.destroy();
                        delete arcGisObjectRefs[cursorGraphicId];
                    }
                    let bufferSymbolGraphic = arcGisObjectRefs[bufferGraphicId];
                    if (bufferSymbolGraphic !== undefined && bufferSymbolGraphic !== null) {
                        view.graphics.remove(bufferSymbolGraphic);
                        bufferSymbolGraphic.destroy();
                        delete arcGisObjectRefs[bufferGraphicId];
                    }
                }
                catch {
                    // ignore if they weren't created yet
                }
                if (cursorGraphicId === undefined) {
                }
                addGraphic({
                    geometry: cursorPoint,
                    symbol: cursorSymbol,
                    id: cursorGraphicId
                }, viewId);
                addGraphic({
                    geometry: buffer,
                    symbol: bufferSymbol,
                    id: bufferGraphicId
                }, viewId);
            }
        }
    });
}
export function displayQueryResults(query, symbol, popupTemplate, viewId) {
    setWaitCursor(viewId);
    queryLayer.queryFeatures(query)
        .then((results) => {
        results.features.map((feature) => {
            feature.symbol = symbol;
            feature.popupTemplate = popupTemplate;
            return feature;
        });
        let view = arcGisObjectRefs[viewId];
        view.popup.close();
        view.graphics.removeAll();
        view.graphics.addMany(results.features);
        unsetWaitCursor(viewId);
    }).catch((error) => {
        logError(error, viewId);
    });
}
export async function addWidget(widget, viewId) {
    try {
        let view = arcGisObjectRefs[viewId];
        switch (widget.type) {
            case 'locate':
                const locate = new Locate({
                    view: view,
                    useHeadingEnabled: widget.useHeadingEnabled,
                    goToOverride: function (view, options) {
                        options.target.scale = widget.zoomTo;
                        return view.goTo(options.target);
                    }
                });
                view.ui.add(locate, widget.position);
                arcGisObjectRefs[widget.id] = locate;
                break;
            case 'search':
                const search = new Search({
                    view: view
                });
                view.ui.add(search, widget.position);
                arcGisObjectRefs[widget.id] = search;
                search.on('select-result', (evt) => {
                    widget.searchWidgetObjectReference.invokeMethodAsync('OnSearchSelectResult', {
                        extent: buildDotNetExtent(evt.result.extent),
                        feature: buildDotNetFeature(evt.result.feature),
                        name: evt.result.name
                    });
                });
                break;
            case 'basemapToggle':
                const basemapToggle = new BasemapToggle({
                    view: view,
                    nextBasemap: widget.nextBasemap
                });
                view.ui.add(basemapToggle, widget.position);
                arcGisObjectRefs[widget.id] = basemapToggle;
                break;
            case 'basemapGallery':
                let source = new PortalBasemapsSource();
                if (widget.portalBasemapsSource !== undefined && widget.portalBasemapsSource !== null) {
                    const portal = new Portal();
                    if (widget.portalBasemapsSource.portal?.url !== undefined &&
                        widget.portalBasemapsSource.portal?.url !== null) {
                        portal.url = widget.portalBasemapsSource.portal.url;
                    }
                    source = new PortalBasemapsSource({
                        portal
                    });
                    if (widget.portalBasemapsSource.queryParams !== undefined &&
                        widget.portalBasemapsSource.queryParams !== null) {
                        source.query = widget.portalBasemapsSource.queryParams;
                    }
                    else if (widget.portalBasemapsSource.queryString !== undefined &&
                        widget.portalBasemapsSource.queryString !== null) {
                        source.query = widget.portalBasemapsSource.queryString;
                    }
                }
                else if (widget.title !== undefined && widget.title !== null) {
                    source.query = {
                        title: widget.title
                    };
                }
                const basemapGallery = new BasemapGallery({
                    view: view,
                    source: source
                });
                view.ui.add(basemapGallery, widget.position);
                arcGisObjectRefs[widget.id] = basemapGallery;
                break;
            case 'scaleBar':
                const scaleBar = new ScaleBar({
                    view: view
                });
                if (widget.unit !== undefined && widget.unit !== null) {
                    scaleBar.unit = widget.unit;
                }
                view.ui.add(scaleBar, widget.position);
                arcGisObjectRefs[widget.id] = scaleBar;
                break;
            case 'legend':
                const legend = new Legend({
                    view: view
                });
                view.ui.add(legend, widget.position);
                arcGisObjectRefs[widget.id] = legend;
                break;
        }
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function createGraphic(graphicObject) {
    let popupTemplate = undefined;
    if (graphicObject.popupTemplate !== undefined && graphicObject.popupTemplate !== null) {
        popupTemplate = buildPopupTemplate(graphicObject.popupTemplate);
    }
    const graphic = new Graphic({
        geometry: graphicObject.geometry,
        symbol: graphicObject.symbol,
        attributes: graphicObject.attributes,
        popupTemplate: popupTemplate
    });
    arcGisObjectRefs[graphicObject.id] = graphic;
    return graphic;
}
export function addLayer(layerObject, viewId, isBasemapLayer, isQueryLayer, callback) {
    try {
        let view = arcGisObjectRefs[viewId];
        let newLayer;
        switch (layerObject.type) {
            case 'graphics':
                newLayer = new GraphicsLayer();
                layerObject.graphics?.forEach(graphicObject => {
                    addGraphic(graphicObject, viewId, newLayer);
                });
                break;
            case 'feature':
                newLayer = new FeatureLayer({
                    url: layerObject.url,
                    opacity: layerObject.opacity,
                    definitionExpression: layerObject.definitionExpression
                });
                let featureLayer = newLayer;
                if (layerObject.opacity !== undefined && layerObject.opacity !== null) {
                    newLayer.opacity = layerObject.opacity;
                }
                if (layerObject.definitionExpression !== undefined && layerObject.definitionExpression !== null) {
                    featureLayer.definitionExpression = layerObject.definitionExpression;
                }
                if (layerObject.renderer !== undefined && layerObject.renderer !== null) {
                    featureLayer.renderer = layerObject.renderer;
                }
                if (layerObject.labelingInfo !== undefined && layerObject.labelingInfo?.length > 0) {
                    featureLayer.labelingInfo = layerObject.labelingInfo;
                }
                if (layerObject.outFields !== undefined && layerObject.outFields?.length > 0) {
                    featureLayer.outFields = layerObject.outFields;
                }
                if (layerObject.popupTemplate !== undefined && layerObject.popupTemplate !== null) {
                    featureLayer.popupTemplate = buildPopupTemplate(layerObject.popupTemplate);
                }
                break;
            case 'vectorTile':
                if (layerObject.portalItem !== undefined && layerObject.portalItem !== null) {
                    newLayer = new VectorTileLayer({
                        portalItem: layerObject.portalItem
                    });
                }
                else {
                    newLayer = new VectorTileLayer({
                        url: layerObject.url
                    });
                }
                if (layerObject.opacity !== undefined && layerObject.opacity !== null) {
                    newLayer.opacity = layerObject.opacity;
                }
                break;
            case 'tile':
                newLayer = new TileLayer({
                    portalItem: {
                        id: layerObject.portalItem.id
                    }
                });
                break;
            case 'geo-json':
                newLayer = new GeoJSONLayer({
                    url: layerObject.url,
                    copyright: layerObject.copyright
                });
                let gjLayer = newLayer;
                if (layerObject.renderer !== undefined && layerObject.renderer !== null) {
                    gjLayer.renderer = layerObject.renderer;
                }
                if (layerObject.spatialReference !== undefined && layerObject.spatialReference !== null) {
                    gjLayer.spatialReference = new SpatialReference({
                        wkid: layerObject.spatialReference.wkid
                    });
                }
                break;
            default:
                return;
        }
        if (isBasemapLayer) {
            view.map.basemap.baseLayers.push(newLayer);
        }
        else if (isQueryLayer) {
            queryLayer = newLayer;
            if (callback !== undefined) {
                callback();
            }
        }
        else {
            view.map.add(newLayer);
        }
        arcGisObjectRefs[layerObject.id] = newLayer;
    }
    catch (error) {
        logError(error, viewId);
    }
}
export function buildPopupTemplate(popupTemplateObject) {
    let content;
    if (popupTemplateObject.stringContent !== undefined && popupTemplateObject.stringContent !== null) {
        content = popupTemplateObject.stringContent;
    }
    else {
        content = popupTemplateObject.content;
    }
    return new PopupTemplate({
        title: popupTemplateObject.title,
        content: content
    });
}
async function resetCenterToSpatialReference(center, spatialReference, viewId) {
    center = await projection.project(center, spatialReference);
    arcGisObjectRefs[viewId].center = center;
}
function logError(error, viewId) {
    if (error.stack !== undefined && error.stack !== null) {
        console.log(error.stack);
        dotNetRefs[viewId].invokeMethodAsync('OnJavascriptError', error.stack);
    }
    else {
        console.log(error.message);
        dotNetRefs[viewId].invokeMethodAsync('OnJavascriptError', error.message);
    }
    unsetWaitCursor(viewId);
}
export function buildDotNetGraphic(graphic) {
    let dotNetGraphic = {};
    dotNetGraphic.uid = graphic.uid;
    switch (graphic.geometry?.type) {
        case 'point':
            dotNetGraphic.geometry = buildDotNetPoint(graphic.geometry);
            break;
        case 'polyline':
            dotNetGraphic.geometry = buildDotNetPolyline(graphic.geometry);
            break;
        case 'polygon':
            dotNetGraphic.geometry = buildDotNetPolygon(graphic.geometry);
            break;
        case 'extent':
            dotNetGraphic.geometry = buildDotNetExtent(graphic.geometry);
            break;
    }
    return dotNetGraphic;
}
function buildDotNetFeature(feature) {
    let dotNetFeature = {
        attributes: feature.attributes
    };
    dotNetFeature.uid = feature.uid;
    switch (feature.geometry?.type) {
        case 'point':
            dotNetFeature.geometry = buildDotNetPoint(feature.geometry);
            break;
        case 'polyline':
            dotNetFeature.geometry = buildDotNetPolyline(feature.geometry);
            break;
        case 'polygon':
            dotNetFeature.geometry = buildDotNetPolygon(feature.geometry);
            break;
        case 'extent':
            dotNetFeature.geometry = buildDotNetExtent(feature.geometry);
            break;
    }
    return dotNetFeature;
}
export function buildDotNetPoint(point) {
    return {
        type: 'point',
        latitude: point.latitude,
        longitude: point.longitude,
        hasM: point.hasM,
        hasZ: point.hasZ,
        extent: buildDotNetExtent(point.extent),
        x: point.x,
        y: point.y,
        spatialReference: point.spatialReference
    };
}
export function buildDotNetPolyline(polyline) {
    return {
        type: 'polyline',
        paths: polyline.paths,
        hasM: polyline.hasM,
        hasZ: polyline.hasZ,
        extent: buildDotNetExtent(polyline.extent),
        spatialReference: polyline.spatialReference
    };
}
export function buildDotNetPolygon(polygon) {
    return {
        type: 'polygon',
        rings: polygon.rings,
        hasM: polygon.hasM,
        hasZ: polygon.hasZ,
        extent: buildDotNetExtent(polygon.extent),
        spatialReference: polygon.spatialReference
    };
}
export function buildDotNetExtent(extent) {
    if (extent === undefined || extent === null)
        return null;
    return {
        type: 'extent',
        xmin: extent.xmin,
        ymin: extent.ymin,
        xmax: extent.xmax,
        ymax: extent.ymax,
        zmin: extent.zmin,
        zmax: extent.zmax,
        mmin: extent.mmin,
        mmax: extent.mmax,
        hasM: extent.hasM,
        hasZ: extent.hasZ,
        spatialReference: extent.spatialReference
    };
}
function setWaitCursor(viewId) {
    let viewContainer = document.getElementById(`map-container-${viewId}`);
    if (viewContainer !== null) {
        viewContainer.style.cursor = 'wait';
    }
}
function unsetWaitCursor(viewId) {
    let viewContainer = document.getElementById(`map-container-${viewId}`);
    if (viewContainer !== null) {
        viewContainer.style.cursor = 'unset';
    }
}
export function getActiveWidgetsForView(viewId) {
    // @ts-ignore
    let realWidgets = arcGisObjectRefs[viewId]?.ui?._components.filter(c => c?.widget !== undefined).map(c => c.widget);
    let registeredWidgets = Object.values(arcGisObjectRefs).filter(o => o?.declaredClass.includes('esri.widgets'));
    return realWidgets?.filter(wc => registeredWidgets.find(r => r === wc));
}
//# sourceMappingURL=arcGisJsInterop.js.map