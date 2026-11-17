using DocControl.Api.Models;

namespace DocControl.Api.Services;

public interface IDashboardService
{
    DashboardSummary GetSummary();
}