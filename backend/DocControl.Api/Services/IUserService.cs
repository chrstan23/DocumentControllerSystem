using DocControl.Api.Models;

namespace DocControl.Api.Services;

public interface IUserService
{
    List<UserListItems> GetUsers();
    UserResult Update(string employeeId, UpdateUserRequest request);
    UserResult SetStatus(string employeeId, bool active);
    UserResult Delete(string employeeId);
}