using Microsoft.AspNetCore.Hosting;

namespace BSC.Api;

public class Program
{
    [Obsolete]
    public static void Main(string[] args)
    {
        var startup = new Startup();
        startup.Initialize(args);
    }
}