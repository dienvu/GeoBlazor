﻿using Microsoft.AspNetCore.Components;


namespace dymaptic.GeoBlazor.Core.Components.Layers;

/// <summary>
///     Defines label expressions, symbols, scale ranges, label priorities, and label placement options for labels on a
///     layer. See the Labeling guide for more information about labeling.
///     <a target="_blank" href="https://developers.arcgis.com/javascript/latest/api-reference/esri-layers-support-LabelClass.html">
///         ArcGIS
///         JS API
///     </a>
/// </summary>
public class Label : LayerObject, IEquatable<Label>
{
    /// <summary>
    ///     Compares two <see cref="Label" />s for equality.
    /// </summary>
    public static bool operator ==(Label? left, Label? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    ///     Compares two <see cref="Label" />s for inequality.
    /// </summary>
    public static bool operator !=(Label? left, Label? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    ///     The position of the label.
    /// </summary>
    [Parameter]
    public string? LabelPlacement { get; set; }

    /// <summary>
    ///     Defines the labels for a MapImageLayer.
    /// </summary>
    /// <remarks>
    ///     MapImageLayer not yet implemented in GeoBlazor
    /// </remarks>
    [Parameter]
    public string? LabelExpression { get; set; }

    /// <summary>
    ///     Defines the labels for a <see cref="FeatureLayer" />.
    /// </summary>
    public LabelExpressionInfo? LabelExpressionInfo { get; set; }

    /// <inheritdoc />
    public bool Equals(Label? other)
    {
        return other?.Id == Id;
    }

    /// <inheritdoc />
    public override async Task RegisterChildComponent(MapComponent child)
    {
        switch (child)
        {
            case LabelExpressionInfo labelExpressionInfo:
                // ReSharper disable once RedundantCast
                if (!((object)labelExpressionInfo).Equals(LabelExpressionInfo))
                {
                    LabelExpressionInfo = labelExpressionInfo;
                }

                break;
            default:
                await base.RegisterChildComponent(child);

                break;
        }
    }

    /// <inheritdoc />
    public override async Task UnregisterChildComponent(MapComponent child)
    {
        switch (child)
        {
            case LabelExpressionInfo _:
                LabelExpressionInfo = null;

                break;
            default:
                await base.UnregisterChildComponent(child);

                break;
        }
    }

    /// <inheritdoc />
    internal override void ValidateRequiredChildren()
    {
        base.ValidateRequiredChildren();
        LabelExpressionInfo?.ValidateRequiredChildren();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((Label)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}