namespace NTFSChecker.Core.Interfaces;

public interface INetworkPathResolver
{
    public bool TryGetRemoteComputerName(string localPath, out string remoteComputerName);
}