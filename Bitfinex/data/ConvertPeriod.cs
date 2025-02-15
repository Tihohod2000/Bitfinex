namespace Bitfinex.data;

public class ConvertPeriod
{
    private static readonly Dictionary<int, string> AvailablePeriods = new()
    {
        { 60, "1m" }, { 300, "5m" }, { 900, "15m" }, { 1800, "30m" }, { 3600, "1h" },
        { 10800, "3h" }, { 21600, "6h" }, { 43200, "12h" }, { 86400, "1D" }, { 604800, "1W" },
        { 1209600, "14D" }, { 2592000, "1M" }
    };
    
    public static string ConvPeriod(int periodInSec)
    {
        int closestPeriod = AvailablePeriods.Keys.OrderBy(p => Math.Abs(p - periodInSec)).First();
        return AvailablePeriods[closestPeriod];
    }
}