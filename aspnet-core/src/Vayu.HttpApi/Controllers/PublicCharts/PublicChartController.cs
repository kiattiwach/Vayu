using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.PublicCharts;

namespace Vayu.Controllers.PublicCharts;

[Route("api/chart")]
public class PublicChartController : VayuController, IPublicChartService
{
    private readonly IPublicChartService _publicChartService;

    public PublicChartController(IPublicChartService publicChartService)
    {
        _publicChartService = publicChartService;
    }

    [HttpPost("get-public-chart/{input}")]
    public Task<ChartDoughnutDto> GetPublicChartAsync(PublicChartFilterDto input)
    {
        return _publicChartService.GetPublicChartAsync(input);
    }
}