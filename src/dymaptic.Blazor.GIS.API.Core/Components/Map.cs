﻿using dymaptic.Blazor.GIS.API.Core.Components.Layers;
using Microsoft.AspNetCore.Components;

namespace dymaptic.Blazor.GIS.API.Core.Components;

public class Map : MapComponent
{
    public Basemap? Basemap { get; set; }

    public HashSet<Layer> Layers { get; set; } = new();

    [Parameter]
    public string? ArcGISDefaultBasemap { get; set; }

    [Parameter]
    public string? Ground { get; set; }

    public override async Task RegisterChildComponent(MapComponent child)
    {
        switch (child)
        {
            case Basemap basemap:
                if (!basemap.Equals(Basemap))
                {
                    Basemap = basemap;
                    await UpdateComponent();
                }

                break;
            case Layer layer:
                if (!Layers.Contains(layer))
                {
                    Type typeOfLayer = layer.GetType();
                    IEnumerable<Layer> allLayersOfType = Layers.Where(l => l.GetType() == typeOfLayer);
                    layer.LayerIndex = allLayersOfType.Count();
                    Layers.Add(layer);

                    if (MapRendered)
                    {
                        await layer.UpdateComponent();
                    }
                    else
                    {
                        await UpdateComponent();
                    }
                }

                break;
            default:
                await base.RegisterChildComponent(child);

                break;
        }
    }

    public override async Task UnregisterChildComponent(MapComponent child)
    {
        switch (child)
        {
            case Basemap _:
                Basemap = null;

                break;
            case Layer layer:
                await layer.RemoveComponent();
                Layers.Remove(layer);

                break;
            default:
                await base.UnregisterChildComponent(child);

                break;
        }
    }

    public override async Task UpdateComponent()
    {
        await InvokeAsync(async () =>
        {
            StateHasChanged();
            await RenderView();
        });
    }
}