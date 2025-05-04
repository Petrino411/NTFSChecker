using NTFSChecker.Core.Models;

namespace NTFSChecker.Core.Interfaces;

public interface IUserGroupHelper
{
    public void CheckDomainAvailability();
    public Task<List<ExcelDataModel>> SetDescriptionsAsync(List<ExcelDataModel> data);

}