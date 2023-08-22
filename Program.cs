using WebsiteImages;

namespace WebsiteImages;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please Enter URL");
            return;
        }

        string url = args[0];

        Console.WriteLine($"URL: {url}");
        
        WebsiteImagesFinder finder = new(url);

        foreach (var img in finder.GetImageUrls())
        {
            Console.WriteLine(img);
        }

    }
}