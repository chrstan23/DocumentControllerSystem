using DocControl.Api.Models;

namespace DocControl.Api.Services;

public interface ILookupService
{
    List<LookupItem> GetSections(string? docType = null);
    List<LookupItem> GetDocTypes();
}