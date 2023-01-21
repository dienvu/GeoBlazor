﻿using dymaptic.GeoBlazor.Core.Components.Geometries;
using dymaptic.GeoBlazor.Core.Objects;
using Microsoft.JSInterop;


namespace dymaptic.GeoBlazor.Core.Components.Layers;

/// <summary>
///     The FeatureLayerView is responsible for rendering a FeatureLayer's features as graphics in the View. The methods in the FeatureLayerView provide developers with the ability to query and highlight graphics in the view. See the code snippets in the methods below for examples of how to access client-side graphics from the view.
///     <a target="_blank" href="https://developers.arcgis.com/javascript/latest/api-reference/esri-views-MapView.html#whenLayerView">ArcGIS JS API</a>
/// </summary>
public class FeatureLayerView: LayerView
{
    internal FeatureLayerView(LayerView layerView, AbortManager abortManager)
    {
        _abortManager = abortManager;
        Layer = layerView.Layer;
        JsObjectReference = layerView.JsObjectReference;
        SpatialReferenceSupported = layerView.SpatialReferenceSupported;
        Suspended = layerView.Suspended;
        Updating = layerView.Updating;
        Visible = layerView.Visible;
    }
    
    /// <summary>
    ///    Highlights the given feature(s).
    /// </summary>
    /// <param name="objectId">
    ///     The ObjectID of the graphic to highlight.
    /// </param>
    /// <returns>
    ///     A handle that allows the highlight to be removed later.
    /// </returns>
    public async Task<HighlightHandle> Highlight(int objectId)
    {
        IJSObjectReference objectRef = 
            await JsObjectReference!.InvokeAsync<IJSObjectReference>("highlight", objectId);
        return new HighlightHandle(objectRef);
    }
    
    /// <summary>
    ///    Highlights the given feature(s).
    /// </summary>
    /// <param name="objectIds">
    ///     The ObjectIDs of the graphics to highlight.
    /// </param>
    /// <returns>
    ///     A handle that allows the highlight to be removed later.
    /// </returns>
    public async Task<HighlightHandle> Highlight(IEnumerable<int> objectIds)
    {
        IJSObjectReference objectRef = 
            await JsObjectReference!.InvokeAsync<IJSObjectReference>("highlight", objectIds);
        return new HighlightHandle(objectRef);
    }
    
    /// <summary>
    ///    Highlights the given feature(s).
    /// </summary>
    /// <param name="graphic">
    ///     The <see cref="Graphic"/> to highlight.
    /// </param>
    /// <returns>
    ///     A handle that allows the highlight to be removed later.
    /// </returns>
    public async Task<HighlightHandle> Highlight(Graphic graphic)
    {
        IJSObjectReference objectRef = 
            await JsObjectReference!.InvokeAsync<IJSObjectReference>("highlight", graphic);
        return new HighlightHandle(objectRef);
    }

    /// <summary>
    ///    Highlights the given feature(s).
    /// </summary>
    /// <param name="graphics">
    ///     The graphics to highlight.
    /// </param>
    /// <returns>
    ///     A handle that allows the highlight to be removed later.
    /// </returns>
    public async Task<HighlightHandle> Highlight(IEnumerable<Graphic> graphics)
    {
        IJSObjectReference objectRef = 
            await JsObjectReference!.InvokeAsync<IJSObjectReference>("highlight", graphics);
        return new HighlightHandle(objectRef);
    }

    /// <summary>
    ///     Creates query parameter object that can be used to fetch features as they are being displayed. It sets the query parameter's outFields property to ["*"] and returnGeometry to true. The output spatial reference outSpatialReference is set to the spatial reference of the view. Parameters of the filter currently applied to the layerview are also incorporated in the returned query object. The results will include geometries of features and values for availableFields.
    /// </summary>
    public async Task<Query> CreateQuery()
    {
        return await JsObjectReference!.InvokeAsync<Query>("createQuery");
    }
    
    /// <summary>
    ///     Executes a Query against features available for drawing in the layerView and returns the Extent of features that satisfy the query. Valid only for hosted feature services on arcgis.com and for ArcGIS Server 10.3.1 and later. If query parameters are not provided, the extent and count of all features available for drawing are returned.
    ///     To query for the extent of features directly from a Feature Service rather than those visible in the view, you must use the <see cref="FeatureLayer.QueryExtent"/> method.
    /// </summary>
    /// <remarks>
    ///     Spatial queries are executed against quantized geometries in the layerView. The resolution of layerView geometries, is only as precise as the scale resolution of the view. Therefore, the results of the same query could be different when executed at different scales. This also means that geometries returned from any layerView query will likewise be at the same scale resolution of the view.
    ///     Spatial queries have the same limitations as those listed in the projection engine documentation.
    ///     Spatial queries are not currently supported if the FeatureLayerView has any of the following SpatialReferences:
    ///         GDM 2000 (4742) – Malaysia,
    ///         Gusterberg (Ferro) (8042) – Austria/Czech Republic,
    ///         ISN2016 (8086) - Iceland,
    ///         SVY21 (4757) - Singapore
    /// </remarks>
    /// <param name="query">
    ///     Specifies the attributes and spatial filter of the query. When no parameters are passed to this method, all features in the client are returned. To only return features visible in the view, set the geometry parameter in the query object to the view's extent.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the query operation.
    /// </param>
    public async Task<ExtentQueryResult> QueryExtent(Query query, CancellationToken cancellationToken = default)
    {
        IJSObjectReference abortSignal = await _abortManager.CreateAbortSignal(cancellationToken);
        ExtentQueryResult result = await JsObjectReference!.InvokeAsync<ExtentQueryResult>("queryExtent", 
            cancellationToken, query, new {signal = abortSignal});

        await _abortManager.DisposeAbortController(cancellationToken);
        return result;
    }
    
    /// <summary>
    ///     Executes a Query against features available for drawing in the layerView and returns the number of features that satisfy the query. If query parameters are not provided, the count of all features available for drawing is returned.
    ///     To query for the count of features directly from a Feature Service rather than those visible in the view, you must use the <see cref="FeatureLayer.QueryFeatureCount"/> method.
    /// </summary>
    /// <remarks>
    ///     Spatial queries are executed against quantized geometries in the layerView. The resolution of layerView geometries, is only as precise as the scale resolution of the view. Therefore, the results of the same query could be different when executed at different scales. This also means that geometries returned from any layerView query will likewise be at the same scale resolution of the view.
    ///     Spatial queries have the same limitations as those listed in the projection engine documentation.
    ///     Spatial queries are not currently supported if the FeatureLayerView has any of the following SpatialReferences:
    ///         GDM 2000 (4742) – Malaysia,
    ///         Gusterberg (Ferro) (8042) – Austria/Czech Republic,
    ///         ISN2016 (8086) - Iceland,
    ///         SVY21 (4757) - Singapore
    /// </remarks>
    /// <param name="query">
    ///     Specifies the attributes and spatial filter of the query. When no parameters are passed to this method, all features in the client are returned. To only return features visible in the view, set the geometry parameter in the query object to the view's extent.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the query operation.
    /// </param>
    /// <returns></returns>
    public async Task<int> QueryFeatureCount(Query? query = null, CancellationToken cancellationToken = default)
    {
        IJSObjectReference abortSignal = await _abortManager.CreateAbortSignal(cancellationToken);
        int result = await JsObjectReference!.InvokeAsync<int>("queryFeatureCount", cancellationToken, 
            query, new {signal = abortSignal});

        await _abortManager.DisposeAbortController(cancellationToken);

        return result;
    }
    
    /// <summary>
    ///     Executes a Query against features available for drawing in the layerView and returns a FeatureSet. If query parameters are not provided, all features available for drawing are returned along with their attributes that are available on the client. For client-side attribute queries, relevant fields should exist in availableFields list for the query to be successful.
    ///     To execute a query against all the features in a feature service rather than only those in the client, you must use the <see cref="FeatureLayer.QueryFeatures"/> method.
    /// </summary>
    /// <remarks>
    ///     Attribute values used in attribute queries executed against layerViews are case sensitive.
    ///     Spatial queries are executed against quantized geometries in the layerView. The resolution of layerView geometries, is only as precise as the scale resolution of the view. Therefore, the results of the same query could be different when executed at different scales. This also means that geometries returned from any layerView query will likewise be at the same scale resolution of the view.
    ///     Spatial queries have the same limitations as those listed in the projection engine documentation.
    ///     Spatial queries are not currently supported if the FeatureLayerView has any of the following SpatialReferences:
    ///         GDM 2000 (4742) – Malaysia,
    ///         Gsterberg (Ferro) (8042) – Austria/Czech Republic,
    ///         ISN2016 (8086) - Iceland,
    ///         SVY21 (4757) - Singapore
    /// </remarks>
    /// <param name="query">
    ///     Specifies the attributes and spatial filter of the query. When this parameter is not passed to queryFeatures() method, all features available for drawing are returned along with their attributes that are available on the client. To only return features visible in the view, set the geometry parameter in the query object to the view's extent.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the query operation.
    /// </param>
    /// <returns></returns>
    public async Task<FeatureSet> QueryFeatures(Query? query = null, CancellationToken cancellationToken = default)
    {
        IJSObjectReference abortSignal =
            await _abortManager.CreateAbortSignal(cancellationToken);
        FeatureSet result = await JsObjectReference!.InvokeAsync<FeatureSet>("queryFeatures", cancellationToken, 
            query, new {signal = abortSignal});

        await _abortManager.DisposeAbortController(cancellationToken);

        return result;
    }
    
    /// <summary>
    ///     Executes a Query against features available for drawing in the layerView and returns array of the ObjectIDs of features that satisfy the input query. If query parameters are not provided, the ObjectIDs of all features available for drawing are returned.
    ///     To query for ObjectIDs of features directly from a Feature Service rather than those visible in the view, you must use the <see cref="FeatureLayer.QueryObjectIds"/> method.
    /// </summary>
    /// <param name="query">
    ///     Specifies the attributes and spatial filter of the query. When no parameters are passed to this method, all features in the client are returned. To only return features visible in the view, set the geometry parameter in the query object to the view's extent.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the query operation.
    /// </param>
    public async Task<int[]> QueryObjectIds(Query query, CancellationToken cancellationToken = default)
    {
        IJSObjectReference abortSignal = await _abortManager.CreateAbortSignal(cancellationToken);
        
        int[] queryResult = await JsObjectReference!.InvokeAsync<int[]>("queryObjectIds", cancellationToken,
            query, new {signal = abortSignal});

        await _abortManager.DisposeAbortController(cancellationToken);

        return queryResult;
    }
    
    private readonly AbortManager _abortManager;
}