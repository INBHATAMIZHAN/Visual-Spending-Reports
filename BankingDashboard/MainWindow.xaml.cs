using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BankingDashboard
{
    public partial class MainWindow : Window
    {
        private readonly List<Transaction> _allTransactions;
        private readonly Brush[] palette =
        {
            Brushes.DodgerBlue,
            Brushes.Green,
            Brushes.Crimson,
            Brushes.Orange,
            Brushes.MediumPurple,
            Brushes.Teal,
            Brushes.DeepSkyBlue,
            Brushes.DarkViolet
        };

        public MainWindow()
        {
            InitializeComponent();
            _allTransactions = DashboardData.GetTransactions();
            PrepareFilters();
            LoadDashboard();
            ApplySection("Overview");
        }

        private void PrepareFilters()
        {
            var months = _allTransactions
                .Select(t => new DateTime(t.Date.Year, t.Date.Month, 1))
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            MonthFilter.Items.Clear();
            MonthFilter.Items.Add("All Months");
            foreach (var month in months)
            {
                MonthFilter.Items.Add(month.ToString("MMM yyyy"));
            }
            MonthFilter.SelectedIndex = 0;

            CategoryFilter.Items.Clear();
            CategoryFilter.Items.Add("All Categories");
            foreach (var category in _allTransactions.Select(t => t.Category).Distinct().OrderBy(c => c))
            {
                CategoryFilter.Items.Add(category);
            }
            CategoryFilter.SelectedIndex = 0;
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDashboard();
        }

        private List<Transaction> GetFilteredTransactions()
        {
            var selectedMonth = MonthFilter.SelectedItem?.ToString();
            var selectedCategory = CategoryFilter.SelectedItem?.ToString();

            var filtered = _allTransactions.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(selectedMonth) && selectedMonth != "All Months")
            {
                var monthDate = DateTime.ParseExact(selectedMonth, "MMM yyyy", CultureInfo.InvariantCulture);
                filtered = filtered.Where(t => t.Date.Year == monthDate.Year && t.Date.Month == monthDate.Month);
            }

            if (!string.IsNullOrWhiteSpace(selectedCategory) && selectedCategory != "All Categories")
            {
                filtered = filtered.Where(t => t.Category == selectedCategory);
            }

            return filtered.OrderByDescending(t => t.Date).ToList();
        }

        private void LoadDashboard()
        {
            var transactions = GetFilteredTransactions();
            var grouped = transactions
                .GroupBy(t => t.Category)
                .Select(g => new CategorySummary
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            if (grouped.Count > 0)
            {
                TotalSpendValue.Text = FormatCurrency(transactions.Sum(t => t.Amount));
                AverageSpendValue.Text = FormatCurrency(transactions.Average(t => t.Amount));
                TopCategoryValue.Text = grouped.First().Category;
                NetIncomeValue.Text = FormatCurrency(transactions.Sum(t => t.Amount) * 1.8);
            }
            else
            {
                TotalSpendValue.Text = "₹0.00";
                AverageSpendValue.Text = "₹0.00";
                TopCategoryValue.Text = "-";
                NetIncomeValue.Text = "₹0.00";
            }

            TransactionGrid.ItemsSource = transactions;
            FullTransactionGrid.ItemsSource = transactions;
            LoadReportData(grouped);
            LoadAccountData();
            RenderBarChart(grouped);
            RenderPieChart(grouped);
        }

        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySection("Overview");
        }

        private void TransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySection("Transactions");
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySection("Reports");
        }

        private void AccountsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySection("Accounts");
        }

        private void ApplySection(string sectionName)
        {
            OverviewPanel.Visibility = sectionName == "Overview" ? Visibility.Visible : Visibility.Collapsed;
            TransactionsPanel.Visibility = sectionName == "Transactions" ? Visibility.Visible : Visibility.Collapsed;
            ReportsPanel.Visibility = sectionName == "Reports" ? Visibility.Visible : Visibility.Collapsed;
            AccountsPanel.Visibility = sectionName == "Accounts" ? Visibility.Visible : Visibility.Collapsed;

            OverviewButton.Background = sectionName == "Overview" ? new SolidColorBrush(Color.FromRgb(30, 41, 59)) : Brushes.Transparent;
            TransactionsButton.Background = sectionName == "Transactions" ? new SolidColorBrush(Color.FromRgb(30, 41, 59)) : Brushes.Transparent;
            ReportsButton.Background = sectionName == "Reports" ? new SolidColorBrush(Color.FromRgb(30, 41, 59)) : Brushes.Transparent;
            AccountsButton.Background = sectionName == "Accounts" ? new SolidColorBrush(Color.FromRgb(30, 41, 59)) : Brushes.Transparent;

            OverviewButton.Foreground = sectionName == "Overview" ? Brushes.White : new SolidColorBrush(Color.FromRgb(203, 213, 225));
            TransactionsButton.Foreground = sectionName == "Transactions" ? Brushes.White : new SolidColorBrush(Color.FromRgb(203, 213, 225));
            ReportsButton.Foreground = sectionName == "Reports" ? Brushes.White : new SolidColorBrush(Color.FromRgb(203, 213, 225));
            AccountsButton.Foreground = sectionName == "Accounts" ? Brushes.White : new SolidColorBrush(Color.FromRgb(203, 213, 225));
        }

        private void LoadReportData(List<CategorySummary> categories)
        {
            var total = categories.Sum(c => c.TotalAmount);
            var items = categories
                .Select(c => new ReportItem
                {
                    Category = c.Category,
                    Spent = c.TotalAmount,
                    Share = total == 0 ? 0 : c.TotalAmount / total * 100
                })
                .OrderByDescending(c => c.Spent)
                .ToList();

            ReportListView.ItemsSource = items.Select(item => new
            {
                item.Category,
                SpentText = FormatCurrency(item.Spent),
                ShareText = item.Share.ToString("0.0") + "%"
            }).ToList();
        }

        private void LoadAccountData()
        {
            var accounts = DashboardData.GetAccounts();
            AccountGrid.ItemsSource = accounts;

            SavingsBalance.Text = FormatCurrency(accounts.First(a => a.Name == "Primary Savings").Balance);
            CheckingBalance.Text = FormatCurrency(accounts.First(a => a.Name == "Checking").Balance);
            CreditBalance.Text = FormatCurrency(accounts.First(a => a.Name == "Credit Card").Balance);
        }

        private void RenderBarChart(List<CategorySummary> categories)
        {
            BarCanvas.Children.Clear();
            if (categories.Count == 0)
                return;

            double maxVal = categories.Max(c => c.TotalAmount);
            double chartHeight = 150;
            double barWidth = 48;
            double gap = 28;
            double startingX = 18;

            var baseLine = new Line
            {
                X1 = 0,
                X2 = 420,
                Y1 = 190,
                Y2 = 190,
                Stroke = Brushes.SlateGray,
                StrokeThickness = 1,
                Opacity = 0.5
            };
            BarCanvas.Children.Add(baseLine);

            for (int i = 0; i < categories.Count; i++)
            {
                double normalizedHeight = maxVal == 0 ? 0 : (categories[i].TotalAmount / maxVal) * chartHeight;

                var rect = new Rectangle
                {
                    Width = barWidth,
                    Height = normalizedHeight,
                    Fill = palette[i % palette.Length],
                    RadiusX = 8,
                    RadiusY = 8,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(rect, startingX);
                Canvas.SetBottom(rect, 20);
                BarCanvas.Children.Add(rect);

                var valueLabel = new TextBlock
                {
                    Text = FormatCurrencyShort(categories[i].TotalAmount),
                    Foreground = Brushes.SlateGray,
                    FontSize = 10,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Canvas.SetLeft(valueLabel, startingX - 4);
                Canvas.SetBottom(valueLabel, normalizedHeight + 25);
                BarCanvas.Children.Add(valueLabel);

                var categoryLabel = new TextBlock
                {
                    Text = GetShortCategory(categories[i].Category),
                    Foreground = Brushes.SlateGray,
                    FontSize = 10,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Canvas.SetLeft(categoryLabel, startingX - 2);
                Canvas.SetTop(categoryLabel, 176);
                BarCanvas.Children.Add(categoryLabel);

                startingX += barWidth + gap;
            }
        }

        private void RenderPieChart(List<CategorySummary> categories)
        {
            PieCanvas.Children.Clear();
            PieLegend.Children.Clear();
            if (categories.Count == 0)
                return;

            double total = categories.Sum(c => c.TotalAmount);
            if (total == 0)
                return;

            double startAngle = 0;
            Point center = new Point(130, 80);
            double radius = 64;

            for (int i = 0; i < categories.Count; i++)
            {
                double sharePercentage = categories[i].TotalAmount / total;
                double sweepAngle = sharePercentage * 360;
                double endAngle = startAngle + sweepAngle;

                double startRad = Math.PI * (startAngle - 90) / 180.0;
                double endRad = Math.PI * (endAngle - 90) / 180.0;

                var edgeStart = new Point(center.X + radius * Math.Cos(startRad), center.Y + radius * Math.Sin(startRad));
                var edgeEnd = new Point(center.X + radius * Math.Cos(endRad), center.Y + radius * Math.Sin(endRad));
                bool largeArc = sweepAngle > 180;

                var figure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true,
                    IsFilled = true
                };

                figure.Segments.Add(new LineSegment(edgeStart, true));
                figure.Segments.Add(new ArcSegment(edgeEnd, new Size(radius, radius), 0, largeArc, SweepDirection.Clockwise, true));
                figure.Segments.Add(new LineSegment(center, true));

                var geometry = new PathGeometry(new[] { figure });
                var segment = new Path
                {
                    Data = geometry,
                    Fill = palette[i % palette.Length],
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };

                PieCanvas.Children.Add(segment);

                if (sharePercentage >= 0.12)
                {
                    double middleAngle = (startAngle + endAngle) / 2;
                    double midRad = Math.PI * (middleAngle - 90) / 180.0;
                    double textX = center.X + Math.Cos(midRad) * (radius * 0.62) - 18;
                    double textY = center.Y + Math.Sin(midRad) * (radius * 0.62) - 8;

                    var label = new TextBlock
                    {
                        Text = categories[i].Category,
                        Foreground = Brushes.White,
                        FontSize = 9,
                        FontWeight = FontWeights.Bold
                    };

                    Canvas.SetLeft(label, textX);
                    Canvas.SetTop(label, textY);
                    PieCanvas.Children.Add(label);
                }

                var legendItem = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 12, 0) };
                var swatch = new Border
                {
                    Width = 12,
                    Height = 12,
                    Background = palette[i % palette.Length],
                    CornerRadius = new CornerRadius(3),
                    Margin = new Thickness(0, 0, 6, 0)
                };
                var textBlock = new TextBlock
                {
                    Text = categories[i].Category + " (" + FormatCurrencyShort(categories[i].TotalAmount) + ")",
                    Foreground = Brushes.SlateGray,
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold
                };

                legendItem.Children.Add(swatch);
                legendItem.Children.Add(textBlock);
                PieLegend.Children.Add(legendItem);

                startAngle = endAngle;
            }
        }

        private static string GetShortCategory(string category)
        {
            return category.Length > 6 ? category.Substring(0, 6) : category;
        }

        private static string FormatCurrency(double amount)
        {
            return string.Format(new CultureInfo("en-IN"), "₹{0:N2}", amount);
        }

        private static string FormatCurrencyShort(double amount)
        {
            return string.Format(new CultureInfo("en-IN"), "₹{0:N0}", amount);
        }
    }

    public class CategorySummary
    {
        public string Category { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
    }

    public class ReportItem
    {
        public string Category { get; set; } = string.Empty;
        public double Spent { get; set; }
        public double Share { get; set; }
    }
}
