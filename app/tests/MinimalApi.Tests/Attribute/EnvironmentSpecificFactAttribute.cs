﻿// Copyright (c) Microsoft. All rights reserved.

using Xunit;

/// <summary>
/// A base class for environment-specific fact attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class EnvironmentSpecificFactAttribute : FactAttribute
{
    private readonly string _skipMessage;

    /// <summary>
    /// Creates a new instance of the <see cref="EnvironmentSpecificFactAttribute" /> class.
    /// </summary>
    /// <param name="skipMessage">The message to be used when skipping the test marked with this attribute.</param>
    protected EnvironmentSpecificFactAttribute(string skipMessage)
    {
        _skipMessage = skipMessage ?? throw new ArgumentNullException(nameof(skipMessage));
    }

    public sealed override string Skip => IsEnvironmentSupported() ? null : _skipMessage;

    /// <summary>
    /// A method used to evaluate whether to skip a test marked with this attribute. Skips iff this method evaluates to false.
    /// </summary>
    protected abstract bool IsEnvironmentSupported();
}

public sealed class ApiKeyFactAttribute : EnvironmentSpecificFactAttribute
{
    private readonly string[] _envVariableName;
    public ApiKeyFactAttribute(params string[] envVariableName) : base($"{envVariableName} is not found in env")
    {
        _envVariableName = envVariableName;
    }

    /// <inheritdoc />
    protected override bool IsEnvironmentSupported()
    {
        return _envVariableName.Any(Environment.GetEnvironmentVariables().Contains);
    }
}