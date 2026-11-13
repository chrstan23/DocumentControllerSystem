using DocControl.Api.Models;

namespace DocControl.Api.Services;

public interface IProcedureService
{
    AddProcedureResult AddExisting(ExistingProcedureForm form);
}   