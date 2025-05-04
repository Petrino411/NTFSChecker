using NTFSChecker.Core.Interfaces;

public class LinuxNetworkPathResolver : INetworkPathResolver
{
    public bool TryGetRemoteComputerName(string localPath, out string remoteComputerName)
    {
        remoteComputerName = "localhost";
        return true;
    }
}