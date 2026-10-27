using System.Globalization;
using System.Windows;

namespace BankingDashboard
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CultureInfo.CurrentCulture = new CultureInfo("en-IN");
            CultureInfo.CurrentUICulture = new CultureInfo("en-IN");
        }
    }
}
