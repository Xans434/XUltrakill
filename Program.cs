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
        string url = "https://i.pinimg.com/736x/5e/fc/0a/5efc0af36d46f0ad390cc00be9619950.jpg";
        string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "wallpaper.jpg");

        while (true)
        {
            byte[] imageBytes = await client.GetByteArrayAsync(url);
            await System.IO.File.WriteAllBytesAsync(tempPath, imageBytes);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            await Task.Delay(1000);
        }
    }
}