using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDWININICHANGE = 0x02;

    static async Task Main()
    {
        using HttpClient client = new();
        string[] urls = new[]
        {
            "https://i.pinimg.com/736x/5e/fc/0a/5efc0af36d46f0ad390cc00be9619950.jpg",
            "https://i.pinimg.com/736x/11/07/fb/1107fb16d7cd0e81867dd32021d074ad.jpg",
            "https://i.pinimg.com/736x/a6/3a/74/a63a740a183fcecff34392d8ad699c80.jpg",
            "https://i.pinimg.com/736x/d3/99/40/d39940787b9052757b5aab1d81767cad.jpg",
            "https://i.pinimg.com/736x/bc/30/dc/bc30dc05595eb3bb4e1e84ac10e355ed.jpg"
        };
        string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wallpaper.jpg");
        Random random = new();

        while (true)
        {
            string url = urls[random.Next(urls.Length)];
            byte[] imageBytes = await client.GetByteArrayAsync(url);
            await System.IO.File.WriteAllBytesAsync(tempPath, imageBytes);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            await Task.Delay(1000);
        }
    }
}