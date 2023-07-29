using OnlineIndicator.API.Models;

namespace OnlineIndicator.API.Helper
{
    internal static class ServerSentEventsHelper
    {
        public static async Task WriteSseEventAsync(this HttpResponse response, ServerSentEvent serverSentEvent)
        {
            await response.WriteAsync($"id: {serverSentEvent.Id}\n");
            await response.WriteAsync($"event: {serverSentEvent.Type}\n");
            await response.WriteAsync($"data: {serverSentEvent.Data}\n\n");
            await response.Body.FlushAsync();
        }
    }

}
