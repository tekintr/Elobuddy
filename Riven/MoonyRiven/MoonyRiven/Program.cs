using EloBuddy.SDK.Events;

namespace MoonyRiven
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += eventArgs =>
            {
                new Riven();
                new DependencyManager();
            };
        }
    }
}
