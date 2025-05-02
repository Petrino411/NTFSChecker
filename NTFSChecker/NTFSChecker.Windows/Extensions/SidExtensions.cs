using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace NTFSChecker.Windows.Extensions;

public static class SidExtensions
{
    public static bool TryParseSid(string sidString, out SecurityIdentifier sid)
    {
        try
        {
            sid = new SecurityIdentifier(sidString);
            return true;
        }
        catch (ArgumentException) 
        {
            sid = null;
            return false;
        }
        catch (Exception ex) 
        {
            sid = null;
            return false;
        }
    }
    
    
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool LookupAccountSid(
        string lpSystemName,
        [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
        StringBuilder lpName,
        ref uint cchName,
        StringBuilder lpReferencedDomainName,
        ref uint cchReferencedDomainName,
        out SID_NAME_USE peUse);

    private enum SID_NAME_USE
    {
        SidTypeUser = 1,
        SidTypeGroup,
        SidTypeDomain,
        SidTypeAlias,
        SidTypeWellKnownGroup,
        SidTypeDeletedAccount,
        SidTypeInvalid,
        SidTypeUnknown,
        SidTypeComputer
    }

    public static bool TryLookupAccountSid(string systemName, string sidString, out string accountName, out string domainName)
    {
        var sid = new SecurityIdentifier(sidString);
        var sidArray = new byte[sid.BinaryLength];
        sid.GetBinaryForm(sidArray, 0);

        uint cchName = 1024;
        uint cchReferencedDomainName = 1024;
        var name = new StringBuilder((int)cchName);
        var referencedDomainName = new StringBuilder((int)cchReferencedDomainName);

        var result = LookupAccountSid(systemName, sidArray, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out _);
        if (result)
        {
            accountName = name.ToString();
            domainName = "Объект на " + referencedDomainName;
            return true;
        }

        accountName = null;
        domainName = null;
        return false;
    }
    

    
}