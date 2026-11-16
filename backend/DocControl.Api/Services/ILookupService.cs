using DocControl.Api.Models;

namespace DocControl.Api.Services;

public interface ILookupService
{
    List<LookupItem> GetSections();
    List<LookupItem> GetDocTypes();
}