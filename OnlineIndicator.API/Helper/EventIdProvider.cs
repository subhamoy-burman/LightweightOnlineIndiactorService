namespace OnlineIndicator.API.Helper
{
    public interface IEventIdProvider
    {
        string GetNextId();
    }

    public class EventIdProvider : IEventIdProvider
    {
        private long currentId = 0;

        public string GetNextId() => Interlocked.Increment(ref currentId).ToString();
    }

}
