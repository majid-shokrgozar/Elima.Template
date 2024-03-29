﻿using JetBrains.Annotations;

namespace Elima.Common.EntityFramework.EntityFrameworkCore.Modeling;

public class ModelBuilderConfigurationOptions
{
    [NotNull]
    public string TablePrefix
    {
        get => _tablePrefix;
        set
        {
            Check.NotNull(value, nameof(value), $"{nameof(TablePrefix)} can not be null! Set to empty string if you don't want a table prefix.");
            _tablePrefix = value;
        }
    }

    private string _tablePrefix = default!;

    public string? Schema { get; set; }

    public ModelBuilderConfigurationOptions(
        [NotNull] string tablePrefix = "",
        string? schema = null)
    {
        Check.NotNull(tablePrefix, nameof(tablePrefix));

        TablePrefix = tablePrefix;
        Schema = schema;
    }
}
