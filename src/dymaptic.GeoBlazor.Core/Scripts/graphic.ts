﻿import Graphic from "@arcgis/core/Graphic";
import {DotNetGeometry, DotNetPopupTemplate} from "./definitions";
import {buildJsGeometry, buildJsPopupTemplate, buildJsSymbol} from "./jsBuilder";
import {buildDotNetGeometry, buildDotNetPopupTemplate} from "./dotNetBuilder";

export default class GraphicWrapper {
    public graphic: Graphic;

    constructor(graphic: Graphic) {
        this.graphic = graphic;
        // set all properties from graphic
        for (let prop in graphic) {
            if (graphic.hasOwnProperty(prop)) {
                this[prop] = graphic[prop];
            }
        }
    }

    setAttribute(name: string, value: any): void {
        if (this.graphic.attributes[name] !== value) {
            this.graphic.attributes[name] = value;
        }
    }

    getAttribute(name: string): any {
        return this.graphic.attributes[name];
    }

    removeAttribute(name: string): void {
        delete this.graphic.attributes[name];
    }

    setGeometry(geometry: DotNetGeometry): void {
        let jsGeometry = buildJsGeometry(geometry);
        if (jsGeometry !== null && this.graphic.geometry !== jsGeometry) {
            this.graphic.geometry = jsGeometry;
        }
    }

    getGeometry(): DotNetGeometry | null {
        return buildDotNetGeometry(this.graphic.geometry);
    }

    setSymbol(symbol: any): void {
        if (this.graphic.symbol !== symbol) {
            this.graphic.symbol = buildJsSymbol(symbol) as any;
        }
    }

    getSymbol(): any {
        return this.graphic.symbol;
    }

    setVisibility(visible: boolean): void {
        this.graphic.visible = visible;
    }

    getVisibility(): boolean {
        return this.graphic.visible;
    }

    setPopupTemplate(popupTemplate: DotNetPopupTemplate, viewId: string): void {
        let jsPopupTemplate = buildJsPopupTemplate(popupTemplate, viewId);
        if (jsPopupTemplate !== null && this.graphic.popupTemplate !== jsPopupTemplate) {
            this.graphic.popupTemplate = jsPopupTemplate;
        }
    }

    getPopupTemplate(): DotNetPopupTemplate | null {
        return buildDotNetPopupTemplate(this.graphic.popupTemplate);
    }
}