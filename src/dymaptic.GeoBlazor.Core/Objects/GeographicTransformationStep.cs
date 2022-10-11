﻿using System.Text.Json.Serialization;


namespace dymaptic.GeoBlazor.Core.Objects;

/// <summary>
///
///     <a target="_blank" href="">ArcGIS JS API</a>
/// </summary>
public class GeographicTransformationStep
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsInverse { get; set; }
    
    /// <summary>
    ///     Wkid = Well-Know Id
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Wkid { get; set; }
    
    /// <summary>
    ///     Wkt = Well-Known Text
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Wkt { get; set; }
}