using System.Net;
using System.Text.RegularExpressions;

namespace WebsiteImages
{
    public class WebsiteImagesFinder
    {
        public string Host { get; set; }
        public string Url { get; set; }
        public WebClient Client { get; set; }

        public WebsiteImagesFinder(string url)
        {
            Url = url;
            Client = new();

            Uri uri = new(url);
            Host = uri.Host;
        }

        public string GetContent()
        {
            return Client.DownloadString(Url);
        }

        public int ImgTagCount()
        {
            string content = Client.DownloadString(Url);
            string target = "<img";

            int count = 0;
            int index = 0;


            while ((index = content.IndexOf(target, index)) != -1)
            {
                index += target.Length;
                count++;
            }

            return count;
        }

        public HashSet<string> GetImageUrls()
        {
            string pattern = @"<img[^>]*\s+src\s*=\s*[""']([^""']+)[""'][^>]*>";
            string content = Client.DownloadString(Url);

            MatchCollection matches = Regex.Matches(content, pattern);

            HashSet<string> urls = new();

            foreach (Match match in matches.Cast<Match>())
            {
                string url = match.Groups[1].Value;

                if (url[0] == '/')
                {
                    url = $"https://{Host}{url}";
                }

                urls.Add(url);
            }

            return urls;
        }

        public async Task DownloadFileAsync(string url, string outputPath)
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();

                FileStream fileStream = File.Create(outputPath);
                await stream.CopyToAsync(fileStream);
            }
            else
            {
                Console.WriteLine($"Failed to download file. Status code: {response.StatusCode}");
            }
        }
    }
}
