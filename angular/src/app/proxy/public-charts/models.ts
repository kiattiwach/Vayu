
export interface ChartDoughnutDataDto {
  labels: string[];
  datasets: ChartDoughnutDataSetDto[];
}

export interface ChartDoughnutDataSetDto {
  label?: string;
  data: number[];
  backgroundColor: string[];
  hoverBackgroundColor: string[];
}

export interface ChartDoughnutDto {
  type?: string;
  data: ChartDoughnutDataDto;
  options: ChartDoughnutOptionDto;
}

export interface ChartDoughnutLegendDto {
  position?: string;
}

export interface ChartDoughnutOptionDto {
  plugins: ChartDoughnutPluginDto;
}

export interface ChartDoughnutPluginDto {
  legend: ChartDoughnutLegendDto;
  title: ChartDoughnutTitleDto;
}

export interface ChartDoughnutTitleDto {
  display: boolean;
  text?: string;
}

export interface PublicChartFilterDto {
  month: number;
  year: number;
}
