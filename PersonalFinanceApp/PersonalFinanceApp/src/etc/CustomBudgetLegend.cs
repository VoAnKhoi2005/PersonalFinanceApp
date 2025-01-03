﻿using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using SkiaSharp;

namespace PersonalFinanceApp.etc;

public class CustomBudgetLegend : IChartLegend<SkiaSharpDrawingContext>
{
    private static readonly int s_zIndex = 10050;
    private readonly StackPanel<RoundedRectangleGeometry, SkiaSharpDrawingContext> _stackPanel = new();

    private readonly SolidColorPaint _fontPaint = new(SKColors.CornflowerBlue)
    {
        SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold),
        ZIndex = s_zIndex + 1,
    };

    public void Draw(Chart<SkiaSharpDrawingContext> chart)
    {
        var legendPosition = chart.GetLegendPosition();

        _stackPanel.X = legendPosition.X - 20f;
        _stackPanel.Y = legendPosition.Y;

        chart.AddVisual(_stackPanel);
        if (chart.LegendPosition == LegendPosition.Hidden) chart.RemoveVisual(_stackPanel);
    }

    public LvcSize Measure(Chart<SkiaSharpDrawingContext> chart)
    {
        _stackPanel.Orientation = ContainerOrientation.Vertical;
        _stackPanel.VerticalAlignment = Align.Start;
        _stackPanel.HorizontalAlignment = Align.Start;
        _stackPanel.MaxWidth = double.MaxValue;
        _stackPanel.MaxHeight = chart.ControlSize.Height;

        foreach (var visual in _stackPanel.Children.ToArray())
        {
            _ = _stackPanel.Children.Remove(visual);
            chart.RemoveVisual(visual);
        }

        var theme = LiveCharts.DefaultSettings.GetTheme<SkiaSharpDrawingContext>();

        var totalValue = chart.Series.Where(x => x.IsVisibleAtLegend).Sum(series => series.Values.Cast<double>().Sum());

        foreach (var series in chart.Series.Where(x => x.IsVisibleAtLegend))
        {
            var seriesValue = series.Values.Cast<double>().Sum();
            var percentage = totalValue > 0 ? seriesValue / totalValue * 100 : 0;

            var seriesColor = series.Name == "Remaining" ? SKColors.Gray : theme.GetSeriesColor(series).AsSKColor();

            var panel = new StackPanel<RectangleGeometry, SkiaSharpDrawingContext>
            {
                Padding = new Padding(12, 6),
                VerticalAlignment = Align.Middle,
                HorizontalAlignment = Align.Middle,
                Children =
                {
                    new SVGVisual
                    {
                        Path = SKPath.ParseSvgPathData(SVGPoints.Circle),
                        Width = 10,
                        Height = 10,
                        ClippingMode = ClipMode.None,
                        Fill = new SolidColorPaint(seriesColor)
                        {
                            ZIndex = s_zIndex + 1
                        }
                    },

                    new LabelVisual
                    {
                        Text = $"{series.Name ?? string.Empty} ({percentage:N1}%)",
                        Paint = _fontPaint,
                        TextSize = 15,
                        ClippingMode = ClipMode.None,
                        Padding = new Padding(8, 0, 0, 0),
                        VerticalAlignment = Align.Start,
                        HorizontalAlignment = Align.Start,
                        MaxWidth = 150,
                    }
                }
            };

            panel.PointerDown += GetPointerDownHandler(series);
            _stackPanel.Children.Add(panel);
        }

        return _stackPanel.Measure(chart);
    }

    private static VisualElementHandler<SkiaSharpDrawingContext> GetPointerDownHandler(
        IChartSeries<SkiaSharpDrawingContext> series)
    {
        return (visual, args) => { series.IsVisible = !series.IsVisible; };
    }
}