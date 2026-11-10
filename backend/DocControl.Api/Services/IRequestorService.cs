using DocControl.Api.Models;

namespace DocControl.Api.Services;

public interface IRequestorService
{
    List<Requestor> GetRequestors();
}