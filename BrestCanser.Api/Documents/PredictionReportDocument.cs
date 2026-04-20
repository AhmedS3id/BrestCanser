using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace BrestCanser.Api.Documents;

public class PredictionReportDocument : IDocument
{
	private readonly IEnumerable<PredictionHistory> _histories;
	private readonly string _fullName;

	public PredictionReportDocument(IEnumerable<PredictionHistory> histories, string fullName)
	{
		_histories = histories;
		_fullName = fullName;
	}

	public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

	public void Compose(IDocumentContainer container)
	{
		container.Page(page =>
		{
			page.Size(PageSizes.A4);
			page.Margin(2, Unit.Centimetre);
			page.DefaultTextStyle(x => x.FontSize(11));

			page.Header().Element(ComposeHeader);
			page.Content().Element(ComposeContent);
			page.Footer().AlignCenter().Text(text =>
			{
				text.Span("Page ");
				text.CurrentPageNumber();
				text.Span(" of ");
				text.TotalPages();
			});
		});
	}

	private void ComposeHeader(IContainer container)
	{
		container.Column(col =>
		{
			col.Item().AlignCenter().Text("Breast Cancer Prediction Report")
				.FontSize(20).Bold().FontColor(Colors.Pink.Darken2);

			col.Item().AlignCenter().Text($"Patient: {_fullName}")
				.FontSize(13).FontColor(Colors.Grey.Darken2);

			var culture = new CultureInfo("en-US");

			col.Item().AlignCenter().Text($"Generated: {DateTime.UtcNow.ToString("dddd, dd MMMM yyyy - hh:mm tt", culture)}")
				.FontSize(10).FontColor(Colors.Grey.Medium);

			col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Pink.Lighten2);
		});
	}

	private void ComposeContent(IContainer container)
	{
		container.Column(col =>
		{
			col.Item().PaddingVertical(12).Element(ComposeSummary);
			col.Item().PaddingTop(8).Element(ComposeTable);
		});
	}

	private void ComposeSummary(IContainer container)
	{
		var total = _histories.Count();
		var benign = _histories.Count(x => x.Status == Enum.PredictionStatus.Benign);
		var malignant = _histories.Count(x => x.Status == Enum.PredictionStatus.Malignant);
		var uncertain = _histories.Count(x => x.Status == Enum.PredictionStatus.Uncertain);
		var avgConf = _histories.Average(x => x.Confidence);

		container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12).Column(col =>
		{
			col.Item().Text("Summary").FontSize(14).Bold().FontColor(Colors.Pink.Darken2);
			col.Item().PaddingTop(6).Row(row =>
			{
				SummaryItem(row.RelativeItem(), "Total Scans", total.ToString(), Colors.Blue.Lighten3);
				SummaryItem(row.RelativeItem(), "Benign", benign.ToString(), Colors.Green.Lighten3);
				SummaryItem(row.RelativeItem(), "Malignant", malignant.ToString(), Colors.Red.Lighten3);
				SummaryItem(row.RelativeItem(), "Uncertain", uncertain.ToString(), Colors.Orange.Lighten3);
				SummaryItem(row.RelativeItem(), "Avg. Confidence", $"{avgConf:F1}%", Colors.Purple.Lighten3);
			});
		});
	}

	private static void SummaryItem(IContainer container, string label, string value, string bgColor)
	{
		container.Padding(4).Background(bgColor).Padding(8).Column(col =>
		{
			col.Item().AlignCenter().Text(value).FontSize(18).Bold();
			col.Item().AlignCenter().Text(label).FontSize(9).FontColor(Colors.Grey.Darken2);
		});
	}

	private void ComposeTable(IContainer container)
	{
		container.Column(col =>
		{
			col.Item().Text("Prediction History").FontSize(14).Bold().FontColor(Colors.Pink.Darken2);
			col.Item().PaddingTop(6).Table(table =>
			{
				table.ColumnsDefinition(cols =>
				{
					cols.ConstantColumn(30);   // #
					cols.RelativeColumn(2);    // Date
					cols.RelativeColumn(2);    // Diagnosis
					cols.RelativeColumn(1.5f); // Status
					cols.RelativeColumn(1.5f); // Confidence
				});

				// Header
				table.Header(header =>
				{
					foreach (var title in new[] { "#", "Date", "Diagnosis", "Status", "Confidence" })
					{
						header.Cell().Background(Colors.Pink.Darken2).Padding(6)
							.Text(title).FontColor(Colors.White).Bold().FontSize(10);
					}
				});

				// Rows
				var index = 1;
				foreach (var item in _histories.OrderByDescending(x => x.CreatedAt))
				{
					var bg = index % 2 == 0 ? Colors.Grey.Lighten4 : Colors.White;
					var statusColor = item.Status switch
					{
						Enum.PredictionStatus.Benign => Colors.Green.Darken1,
						Enum.PredictionStatus.Malignant => Colors.Red.Darken1,
						_ => Colors.Orange.Darken1
					};

					table.Cell().Background(bg).Padding(5).Text(index.ToString()).FontSize(10);
					table.Cell().Background(bg).Padding(5).Text(item.CreatedAt.ToString("yyyy-MM-dd HH:mm")).FontSize(10);
					table.Cell().Background(bg).Padding(5).Text(item.Diagnosis).FontSize(10);
					table.Cell().Background(bg).Padding(5).Text(item.Status.ToString()).FontColor(statusColor).Bold().FontSize(10);
					table.Cell().Background(bg).Padding(5).Text($"{item.Confidence:F1}%").FontSize(10);

					index++;
				}
			});
		});
	}
}
