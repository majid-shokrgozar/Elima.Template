using System.Runtime.InteropServices;

namespace Elima.Common.System.Runtime;

public interface IOSPlatformProvider
{
    OSPlatform GetCurrentOSPlatform();
}
