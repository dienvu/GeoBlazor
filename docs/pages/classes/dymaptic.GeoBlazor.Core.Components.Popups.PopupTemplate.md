---
layout: default
title: PopupTemplate
parent: Classes
---
#### [dymaptic.GeoBlazor.Core](index.html 'index')
### [dymaptic.GeoBlazor.Core.Components.Popups](index.html#dymaptic.GeoBlazor.Core.Components.Popups 'dymaptic.GeoBlazor.Core.Components.Popups')

## PopupTemplate Class

A PopupTemplate formats and defines the content of a Popup for a specific Layer or Graphic. The user can also use  
the PopupTemplate to access values from feature attributes and values returned from Arcade expressions when a  
feature in the view is selected.  
<a target="_blank" href="https://developers.arcgis.com/javascript/latest/api-reference/esri-PopupTemplate.html">ArcGIS JS API</a>

```csharp
public class PopupTemplate : dymaptic.GeoBlazor.Core.Components.MapComponent,
System.IEquatable<dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate>
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [Microsoft.AspNetCore.Components.ComponentBase](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Components.ComponentBase 'Microsoft.AspNetCore.Components.ComponentBase') &#129106; [MapComponent](dymaptic.GeoBlazor.Core.Components.MapComponent.html 'dymaptic.GeoBlazor.Core.Components.MapComponent') &#129106; PopupTemplate

Implements [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[PopupTemplate](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')
### Constructors

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate()'></a>

## PopupTemplate() Constructor

Parameterless constructor for using as a razor component

```csharp
public PopupTemplate();
```

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_)'></a>

## PopupTemplate(string, string, IEnumerable<string>, IEnumerable<FieldInfo>, IEnumerable<PopupContent>, IEnumerable<ExpressionInfo>, Nullable<bool>, Nullable<bool>, IEnumerable<ActionBase>) Constructor

Constructs a new PopupTemplate in code with parameters

```csharp
public PopupTemplate(string title, string? stringContent=null, System.Collections.Generic.IEnumerable<string>? outFields=null, System.Collections.Generic.IEnumerable<dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo>? fieldInfos=null, System.Collections.Generic.IEnumerable<dymaptic.GeoBlazor.Core.Components.Popups.PopupContent>? contents=null, System.Collections.Generic.IEnumerable<dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo>? expressionInfos=null, System.Nullable<bool> overwriteActions=null, System.Nullable<bool> returnGeometry=null, System.Collections.Generic.IEnumerable<dymaptic.GeoBlazor.Core.Components.ActionBase>? actions=null);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).title'></a>

`title` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

The title of the popup

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).stringContent'></a>

`stringContent` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Use this parameter if the content is a simple string

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).outFields'></a>

`outFields` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

An array of field names used in the PopupTemplate.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).fieldInfos'></a>

`fieldInfos` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[FieldInfo](dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo.html 'dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

An array of FieldInfo that defines how fields in the dataset or values from Arcade expressions participate in a  
popup.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).contents'></a>

`contents` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[PopupContent](dymaptic.GeoBlazor.Core.Components.Popups.PopupContent.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupContent')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

Pass advanced [PopupContent](dymaptic.GeoBlazor.Core.Components.Popups.PopupContent.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupContent') parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).expressionInfos'></a>

`expressionInfos` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[ExpressionInfo](dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo.html 'dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

An array of objects or ExpressionInfo[] that reference Arcade expressions following the specification defined by  
the Arcade Popup Profile.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).overwriteActions'></a>

`overwriteActions` [System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

Indicates whether actions should replace existing popup actions.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).returnGeometry'></a>

`returnGeometry` [System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

Indicates whether to include the feature's geometry for use by the template.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.PopupTemplate(string,string,System.Collections.Generic.IEnumerable_string_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.PopupContent_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo_,System.Nullable_bool_,System.Nullable_bool_,System.Collections.Generic.IEnumerable_dymaptic.GeoBlazor.Core.Components.ActionBase_).actions'></a>

`actions` [System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[ActionBase](dymaptic.GeoBlazor.Core.Components.ActionBase.html 'dymaptic.GeoBlazor.Core.Components.ActionBase')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

Defines actions that may be executed by clicking the icon or image symbolizing them in the popup
### Properties

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Actions'></a>

## PopupTemplate.Actions Property

Defines actions that may be executed by clicking the icon or image symbolizing them in the popup

```csharp
public System.Collections.Generic.HashSet<dymaptic.GeoBlazor.Core.Components.ActionBase>? Actions { get; set; }
```

#### Property Value
[System.Collections.Generic.HashSet&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')[ActionBase](dymaptic.GeoBlazor.Core.Components.ActionBase.html 'dymaptic.GeoBlazor.Core.Components.ActionBase')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Content'></a>

## PopupTemplate.Content Property

The template for defining and formatting a popup's content, provided as a collection of [PopupContent](dymaptic.GeoBlazor.Core.Components.Popups.PopupContent.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupContent')  
s.

```csharp
public System.Collections.Generic.HashSet<dymaptic.GeoBlazor.Core.Components.Popups.PopupContent> Content { get; set; }
```

#### Property Value
[System.Collections.Generic.HashSet&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')[PopupContent](dymaptic.GeoBlazor.Core.Components.Popups.PopupContent.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupContent')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')

### Remarks
Either [Content](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html#dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Content 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Content') or [StringContent](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html#dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.StringContent 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.StringContent') should be defined, but not both.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.DotNetPopupTemplateReference'></a>

## PopupTemplate.DotNetPopupTemplateReference Property

Object reference for callbacks from JavaScript.

```csharp
public Microsoft.JSInterop.DotNetObjectReference<dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate> DotNetPopupTemplateReference { get; }
```

#### Property Value
[Microsoft.JSInterop.DotNetObjectReference&lt;](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.JSInterop.DotNetObjectReference-1 'Microsoft.JSInterop.DotNetObjectReference`1')[PopupTemplate](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.JSInterop.DotNetObjectReference-1 'Microsoft.JSInterop.DotNetObjectReference`1')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.ExpressionInfos'></a>

## PopupTemplate.ExpressionInfos Property

An array of objects or ExpressionInfo[] that reference Arcade expressions following the specification defined by  
the Arcade Popup Profile.

```csharp
public System.Collections.Generic.HashSet<dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo>? ExpressionInfos { get; set; }
```

#### Property Value
[System.Collections.Generic.HashSet&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')[ExpressionInfo](dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo.html 'dymaptic.GeoBlazor.Core.Components.Popups.ExpressionInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.FieldInfos'></a>

## PopupTemplate.FieldInfos Property

An array of FieldInfo that defines how fields in the dataset or values from Arcade expressions participate in a  
popup. If no FieldInfo are specified, nothing will display since the popup will only display the fields that are  
defined by this array. Each FieldInfo contains properties for a single field or expression. This property can be  
set directly within the PopupTemplate or within the fields content element. If this is not set within the fields  
content element, it will default to whatever is specified directly within the PopupTemplate.fieldInfos. The image  
on the left is a result of using the first example snippet below, whereas the image on the right is a result of the  
second snippet.

```csharp
public System.Collections.Generic.HashSet<dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo>? FieldInfos { get; set; }
```

#### Property Value
[System.Collections.Generic.HashSet&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')[FieldInfo](dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo.html 'dymaptic.GeoBlazor.Core.Components.Popups.FieldInfo')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.HashSet-1 'System.Collections.Generic.HashSet`1')

### Remarks
Use this fieldInfos property to specify any formatting options for numbers displayed in chart or text elements.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.OutFields'></a>

## PopupTemplate.OutFields Property

An array of field names used in the PopupTemplate. Use this property to indicate what fields are required to fully  
render the PopupTemplate. This is important if setting content via a function since any fields needed for  
successful rendering should be specified here.  
Generally speaking, it is good practice to always set this property when instantiating a new popup template.  
To fetch the values from all fields, use ["*"].

```csharp
public System.Collections.Generic.IEnumerable<string>? OutFields { get; set; }
```

#### Property Value
[System.Collections.Generic.IEnumerable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable-1 'System.Collections.Generic.IEnumerable`1')

### Remarks
This will not fetch fields from related tables. If related features are needed, set this using FieldInfo.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.OverwriteActions'></a>

## PopupTemplate.OverwriteActions Property

Indicates whether actions should replace existing popup actions.

```csharp
public System.Nullable<bool> OverwriteActions { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.ReturnGeometry'></a>

## PopupTemplate.ReturnGeometry Property

Indicates whether to include the feature's geometry for use by the template. This property should be set to true if  
needing to access the popup's selected feature's geometry. Access the geometry via the returned graphic from the  
popup's selectedFeatureWidget. This is needed since the geometry is not automatically queried and returned in the  
popup's selected feature.  
If the feature layer does not specify its outFields and the template's outFields isn't set, the returned popup's  
geometry is only returned if returnGeometry is set to true. This also applies when working with WebMaps.

```csharp
public System.Nullable<bool> ReturnGeometry { get; set; }
```

#### Property Value
[System.Nullable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Nullable-1 'System.Nullable`1')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.StringContent'></a>

## PopupTemplate.StringContent Property

The template for defining and formatting a popup's content, provided as a simple string.

```csharp
public string? StringContent { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Either [Content](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html#dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Content 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Content') or [StringContent](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html#dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.StringContent 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.StringContent') should be defined, but not both.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Title'></a>

## PopupTemplate.Title Property

The template for defining how to format the title used in a popup.

```csharp
public string? Title { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')
### Methods

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Equals(object)'></a>

## PopupTemplate.Equals(object) Method

Determines whether the specified object is equal to the current object.

```csharp
public override bool Equals(object? obj);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.Equals(object).obj'></a>

`obj` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

The object to compare with the current object.

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool') if the specified object  is equal to the current object; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool').

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.GetHashCode()'></a>

## PopupTemplate.GetHashCode() Method

Serves as the default hash function.

```csharp
public override int GetHashCode();
```

#### Returns
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')  
A hash code for the current object.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.OnTriggerAction(string)'></a>

## PopupTemplate.OnTriggerAction(string) Method

JS-invokable method for triggering actions.

```csharp
public System.Threading.Tasks.Task OnTriggerAction(string actionId);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.OnTriggerAction(string).actionId'></a>

`actionId` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

The action ID.

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.RegisterChildComponent(dymaptic.GeoBlazor.Core.Components.MapComponent)'></a>

## PopupTemplate.RegisterChildComponent(MapComponent) Method

Called from [dymaptic.GeoBlazor.Core.Components.MapComponent.OnInitializedAsync](https://docs.microsoft.com/en-us/dotnet/api/dymaptic.GeoBlazor.Core.Components.MapComponent.OnInitializedAsync 'dymaptic.GeoBlazor.Core.Components.MapComponent.OnInitializedAsync') to "Register" the current component with it's parent.

```csharp
public override System.Threading.Tasks.Task RegisterChildComponent(dymaptic.GeoBlazor.Core.Components.MapComponent child);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.RegisterChildComponent(dymaptic.GeoBlazor.Core.Components.MapComponent).child'></a>

`child` [MapComponent](dymaptic.GeoBlazor.Core.Components.MapComponent.html 'dymaptic.GeoBlazor.Core.Components.MapComponent')

The calling, child component to register

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')

#### Exceptions

[InvalidChildElementException](dymaptic.GeoBlazor.Core.Exceptions.InvalidChildElementException.html 'dymaptic.GeoBlazor.Core.Exceptions.InvalidChildElementException')  
Throws if the current child is not a valid sub-component to the parent.

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.UnregisterChildComponent(dymaptic.GeoBlazor.Core.Components.MapComponent)'></a>

## PopupTemplate.UnregisterChildComponent(MapComponent) Method

Undoes the "Registration" of a child with its parent.

```csharp
public override System.Threading.Tasks.Task UnregisterChildComponent(dymaptic.GeoBlazor.Core.Components.MapComponent child);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.UnregisterChildComponent(dymaptic.GeoBlazor.Core.Components.MapComponent).child'></a>

`child` [MapComponent](dymaptic.GeoBlazor.Core.Components.MapComponent.html 'dymaptic.GeoBlazor.Core.Components.MapComponent')

The child to unregister

#### Returns
[System.Threading.Tasks.Task](https://docs.microsoft.com/en-us/dotnet/api/System.Threading.Tasks.Task 'System.Threading.Tasks.Task')
### Operators

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.op_Equality(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate,dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate)'></a>

## PopupTemplate.operator ==(PopupTemplate, PopupTemplate) Operator

Equality operator

```csharp
public static bool operator ==(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate? left, dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate? right);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.op_Equality(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate,dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate).left'></a>

`left` [PopupTemplate](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.op_Equality(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate,dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate).right'></a>

`right` [PopupTemplate](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate')

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.op_Inequality(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate,dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate)'></a>

## PopupTemplate.operator !=(PopupTemplate, PopupTemplate) Operator

Inequality operator

```csharp
public static bool operator !=(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate? left, dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate? right);
```
#### Parameters

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.op_Inequality(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate,dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate).left'></a>

`left` [PopupTemplate](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate')

<a name='dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.op_Inequality(dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate,dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate).right'></a>

`right` [PopupTemplate](dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate.html 'dymaptic.GeoBlazor.Core.Components.Popups.PopupTemplate')

#### Returns
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')
