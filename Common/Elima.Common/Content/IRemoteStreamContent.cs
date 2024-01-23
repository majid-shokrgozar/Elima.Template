using System;
using System.IO;

namespace Elima.Common.Content;

public interface IRemoteStreamContent : IDisposable
{
    string? FileName { get; }

    string ContentType { get; }

    long? ContentLength { get; }

    Stream GetStream();
}
