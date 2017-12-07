using System.Linq;
using Microsoft.ApplicationInsights;

namespace SandwichClub.Api.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly TelemetryClient _client = new TelemetryClient();

        public void TrackEvent(string eventName, object customProperties)
        {
            var propertyDictionary = customProperties.GetType().GetFields()
                .ToDictionary(f => f.Name, f => f.GetValue(customProperties)?.ToString());

            _client.TrackEvent(eventName, propertyDictionary);
        }
    }
}
