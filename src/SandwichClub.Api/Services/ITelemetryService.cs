namespace SandwichClub.Api.Services
{
    public interface ITelemetryService
    {
        void TrackEvent(string eventName, object customProperties)
    }
}