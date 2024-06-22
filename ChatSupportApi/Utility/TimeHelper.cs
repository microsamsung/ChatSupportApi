namespace ChatSupportApi.Utility
{
    public static class TimeHelper
    {
        public static bool IsOfficeHours()
        {
            var currentTime = DateTime.UtcNow;
            return currentTime.Hour >= 9 && currentTime.Hour < 17;
        }
    }
}
