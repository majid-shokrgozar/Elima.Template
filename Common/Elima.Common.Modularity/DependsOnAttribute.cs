﻿using System.Diagnostics.CodeAnalysis;

namespace Elima.Common.Modularity;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute, IDependedTypesProvider
{
    [NotNull]
    public Type[] DependedTypes { get; }

    public DependsOnAttribute(params Type[] dependedTypes)
    {
        DependedTypes = dependedTypes ?? new Type[0];
    }

    public virtual Type[] GetDependedTypes()
    {
        return DependedTypes;
    }
}
