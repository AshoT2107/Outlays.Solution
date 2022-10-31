namespace Outlays.Bot.Helper
{
    public class RandomGenerator
    {
        public static string RandomKey
        {
            get=> Guid.NewGuid().ToString("N")[..10];
        }
        
    }
}
