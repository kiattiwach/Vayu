using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Vayu.Localization;
using Vayu.Shared;
using Vayu.Tasks;
using Volo.Abp.Application.Services;
using Task = System.Threading.Tasks.Task;

namespace Vayu.PublicCharts;

public class PublicChartService : ApplicationService, IPublicChartService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;

    public PublicChartService(ITaskRepository taskRepository,
        IStringLocalizer<VayuResource> stringLocalize)
    {
        _taskRepository = taskRepository;
        _stringLocalize = stringLocalize;
    }

    public async Task<ChartDoughnutDto> GetPublicChartAsync(PublicChartFilterDto input)
    {
        var beginDate = new DateTime(input.Year, input.Month, 1);
        var endDate = beginDate.AddMonths(1).AddDays(-1);

        var taskResult =
            await _taskRepository.GetListAsync(minCompleteDateTime: beginDate, maxCompleteDateTime: endDate);
        var taskTypeResult = taskResult.GroupBy(g => g.TaskType)
            .Select(s => new { TaskType = s.Key, Count = s.Count() }).ToList();

        var colors = RandomColorGenerator.Generate(taskTypeResult.Count);
        var backgroundColor = colors.Select(s => s.GeneratePastelRgbaColors((decimal)0.3)).ToList();
        var backgroundHoverColor = colors.Select(s => s.GeneratePastelRgbaColors((decimal)0.6)).ToList();

        return new ChartDoughnutDto
        {
            Data = new ChartDoughnutDataDto()
            {
                Labels = taskTypeResult.Select(s => s.TaskType.Name).ToList(),
                Datasets = new List<ChartDoughnutDataSetDto>()
                {
                    new ()
                    {
                        Label = _stringLocalize["Chart:TaskTypeCount"],
                        Data = taskTypeResult.Select(s => s.Count).ToList(),
                        BackgroundColor = backgroundColor,
                        HoverBackgroundColor = backgroundHoverColor
                    }
                }
            },
            Options = new ChartDoughnutOptionDto()
            {
                Plugins = new ChartDoughnutPluginDto()
                {
                    Legend = new ChartDoughnutLegendDto(),
                    Title = new ChartDoughnutTitleDto()
                    {
                        Text = _stringLocalize["Chart:PublicChart:TotalTasks", taskResult.Count]
                    }
                }
            }
        };
    }
}