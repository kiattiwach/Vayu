using System;
using System.Collections.Generic;
using System.Drawing;
using Vayu.Shared;

namespace Vayu.PublicCharts;

public class ChartDoughnutDto
{
    public string Type { get; } = "doughnut";
    public ChartDoughnutDataDto Data { get; set; }
    public ChartDoughnutOptionDto Options { get; set; } = new ChartDoughnutOptionDto();
}

public class ChartDoughnutDataDto
{
    public List<string> Labels { get; set; }
    public List<ChartDoughnutDataSetDto> Datasets { get; set; }
}

public class ChartDoughnutDataSetDto
{
    public string Label { get; set; }
    public List<int> Data { get; set; }
    public List<string> BackgroundColor { get; set; }
    public List<string> HoverBackgroundColor { get; set; }
}

public class ChartDoughnutOptionDto
{
    /*public decimal AspectRatio { get; } = (decimal)3.5;
    public bool Responsive { get; } = false;*/
    public ChartDoughnutPluginDto Plugins { get; set; } = new ChartDoughnutPluginDto();

}

public class ChartDoughnutPluginDto
{
    public ChartDoughnutLegendDto Legend { get; set; }
    public ChartDoughnutTitleDto Title { get; set; } = new ChartDoughnutTitleDto();
}

public class ChartDoughnutLegendDto
{
    public string Position { get; set; } = "top";
}

public class ChartDoughnutTitleDto
{
    public bool Display { get; } = true;
    public string Text { get; set; }
}