using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace File.HttpApi.Host.Hubs;

public class UploadingHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Task.CompletedTask;
    }

    public async Task UploadStream(string path, string webkitRelativePath,string fileName, long length, ChannelReader<byte[]> stream)
    {
        bool isException = false;
        var filePath = Path.Combine(path, webkitRelativePath.TrimEnd(fileName.ToCharArray()));
        await using var fileStream = System.IO.File.Open(Path.Combine(filePath,fileName), FileMode.Create, FileAccess.Write);
        try
        {
            while (await stream.WaitToReadAsync())
            {
                while (stream.TryRead(out var item)) 
                {
                    await fileStream.WriteAsync(item);
                }
            }
        }
        catch (Exception e)
        {
            isException = true;
            fileStream.Close();
            System.IO.File.Delete(Path.Combine(filePath,fileName));
        }
        finally
        {
            if (!isException)
            {
                fileStream.Close();

                await base.Clients.Client(base.Context.ConnectionId).SendAsync("");
            }   
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Task.CompletedTask;
    }
}