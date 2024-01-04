using System.Net.Sockets;
var printerIP = "10.6.51.27";
var printerPort = 9100;
var baseDirectory = Environment.CurrentDirectory;
var fileName = "OriginalPdf.pdf";
var fullFileName = Path.Combine(baseDirectory, fileName);
using var fileStream = File.OpenRead(fullFileName);
using (var tcpClient = new TcpClient())
{
    await tcpClient.ConnectAsync(printerIP, printerPort);
    if (tcpClient.Connected)
    {
        using var printerStream = tcpClient.GetStream();
        byte[] buffer = new byte[tcpClient.SendBufferSize];
        await fileStream.CopyToAsync(printerStream, tcpClient.SendBufferSize);
        await fileStream.FlushAsync();
        await printerStream.FlushAsync();
        await printerStream.DisposeAsync();
        await fileStream.DisposeAsync();
    }
}
