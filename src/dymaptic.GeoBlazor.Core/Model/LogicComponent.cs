﻿using dymaptic.GeoBlazor.Core.Exceptions;
using dymaptic.GeoBlazor.Core.Objects;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;


namespace dymaptic.GeoBlazor.Core.Model;

/// <summary>
///     A base class for non-map components, such as GeometryEngine, Projection, etc.
/// </summary>
public abstract class LogicComponent : IDisposable
{
    /// <summary>
    ///     Default constructor
    /// </summary>
    /// <param name="jsRuntime">
    ///     Injected JavaScript Runtime reference
    /// </param>
    /// <param name="authenticationManager">
    ///     Injected Identity Manager reference
    /// </param>
    public LogicComponent(IJSRuntime jsRuntime, AuthenticationManager authenticationManager)
    {
        JsRuntime = jsRuntime;
        _authenticationManager = authenticationManager;
    }

    /// <summary>
    ///     Implement this handler in your calling code to catch and handle Javascript errors.
    /// </summary>
    [Obsolete("Methods now pass on JavaScript errors as exceptions")]
    public Func<JavascriptException, Task>? OnJavascriptErrorHandler { get; set; }

    /// <summary>
    ///     The name of the logic component.
    /// </summary>
    protected abstract string ComponentName { get; }

    /// <summary>
    ///     A JavaScript Module reference to the component's JS file.
    /// </summary>
    protected IJSObjectReference? Component { get; set; }

    /// <summary>
    ///     A .NET Object reference to this class for use in JavaScript.
    /// </summary>
    protected DotNetObjectReference<LogicComponent> DotNetObjectReference =>
        Microsoft.JSInterop.DotNetObjectReference.Create(this);

    /// <summary>
    ///     The project library which houses this particular logic component.
    /// </summary>
    protected virtual string Library => "Core";

    /// <summary>
    ///     Disposes of the Logic Component and cancels all external calls
    /// </summary>
    public void Dispose()
    {
        CancellationTokenSource.Dispose();
    }

    /// <summary>
    ///     A JavaScript invokable method that returns a JS Error and converts it to an Exception.
    /// </summary>
    /// <param name="error">
    ///     The original JavaScript error.
    /// </param>
    /// <exception cref="JavascriptException">
    ///     The converted error to exception.
    /// </exception>
    [JSInvokable]
    public void OnJavascriptError(JavascriptError error)
    {
        var exception = new JavascriptException(error);

        if (OnJavascriptErrorHandler is not null)
        {
            OnJavascriptErrorHandler?.Invoke(exception);
        }
        else
        {
            throw exception;
        }
    }

    /// <summary>
    ///     Convenience method to invoke a JS function from the .NET logic component class.
    /// </summary>
    /// <param name="method">
    ///     The name of the JS function to call.
    /// </param>
    /// <param name="parameters">
    ///     The collection of parameters to pass to the JS call.
    /// </param>
    protected virtual async Task InvokeVoidAsync(string method, params object?[] parameters)
    {
        await Initialize();

        await Component!.InvokeVoidAsync(method, CancellationTokenSource.Token, parameters);
    }

    /// <summary>
    ///     Convenience method to invoke a JS function from the .NET logic component class.
    /// </summary>
    /// <param name="method">
    ///     The name of the JS function to call.
    /// </param>
    /// <param name="parameters">
    ///     The collection of parameters to pass to the JS call.
    /// </param>
    protected virtual async Task<T> InvokeAsync<T>(string method, params object?[] parameters)
    {
        await Initialize();

        return await Component!.InvokeAsync<T>(method, CancellationTokenSource.Token, parameters);
    }

    /// <summary>
    ///    Initializes the JavaScript reference component, if not already initialized.
    /// </summary>
    public virtual async Task Initialize()
    {
        if (Component is null)
        {
            await _authenticationManager.Initialize();
            IJSObjectReference module = await _authenticationManager.GetArcGisJsInterop();

            Component = await module.InvokeAsync<IJSObjectReference>($"get{ComponentName}Wrapper",
                CancellationTokenSource.Token, DotNetObjectReference);
        }
    }

    /// <summary>
    ///     The reference to the JS Runtime.
    /// </summary>
    protected readonly IJSRuntime JsRuntime;
    private readonly AuthenticationManager _authenticationManager;

    /// <summary>
    ///     Creates a cancellation token to control external calls
    /// </summary>
    protected readonly CancellationTokenSource CancellationTokenSource = new();
}