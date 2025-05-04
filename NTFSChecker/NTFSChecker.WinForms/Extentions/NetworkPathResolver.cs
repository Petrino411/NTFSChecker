using System;
using System.Runtime.InteropServices;

namespace NTFSChecker.WinForms.Extentions;

public static class NetworkPathResolver
{
    private const int UNIVERSAL_NAME_INFO_LEVEL = 1;
    private const int ERROR_MORE_DATA = 234;

    [DllImport("mpr.dll", CharSet = CharSet.Auto)]
    private static extern int WNetGetUniversalName(
        string lpLocalPath,
        int dwInfoLevel,
        IntPtr lpBuffer,
        ref int lpBufferSize);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct UNIVERSAL_NAME_INFO
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpUniversalName;
    }

    public static bool TryGetRemoteComputerName(string localPath, out string remoteComputerName)
    {
        int bufferSize = 66000; 
        IntPtr buffer = Marshal.AllocHGlobal(bufferSize);

        try
        {
            int result = WNetGetUniversalName(localPath, UNIVERSAL_NAME_INFO_LEVEL, buffer, ref bufferSize);
            if (result == ERROR_MORE_DATA)
            {
                Marshal.FreeHGlobal(buffer);
                buffer = Marshal.AllocHGlobal(bufferSize);
                result = WNetGetUniversalName(localPath, UNIVERSAL_NAME_INFO_LEVEL, buffer, ref bufferSize);
            }

            if (result == 0) 
            {
                UNIVERSAL_NAME_INFO uni = Marshal.PtrToStructure<UNIVERSAL_NAME_INFO>(buffer);
                Uri uri = new Uri(uni.lpUniversalName);
                remoteComputerName = uri.Host;
                return true;
            }
            else
            {
                remoteComputerName = null;
                return false;
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }
}

