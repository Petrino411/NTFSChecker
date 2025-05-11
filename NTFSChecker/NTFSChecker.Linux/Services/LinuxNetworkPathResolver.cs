using NTFSChecker.Core.Interfaces;

namespace NTFSChecker.Linux.Services;

public class LinuxNetworkPathResolver : INetworkPathResolver
{
    public bool TryGetRemoteComputerName(string localPath, out string remoteComputerName)
    {
        remoteComputerName = "localhost";
        return true;
    }
}