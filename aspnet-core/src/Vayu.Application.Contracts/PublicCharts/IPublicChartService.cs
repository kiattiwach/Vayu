using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Vayu.PublicCharts;

public interface IPublicChartService : IApplicationService
{
    Task<ChartDoughnutDto> GetPublicChartAsync(PublicChartFilterDto input);
}