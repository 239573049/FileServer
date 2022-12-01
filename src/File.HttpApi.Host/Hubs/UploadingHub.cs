using System.Collections.Concurrent;
using System.Threading.Channels;
using File.Application.Contract.Base;
using File.HttpApi.Host.Module;
using File.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace File.HttpApi.Host.Hubs;

public class UploadingHub : Hub
{
    private readonly ConcurrentDictionary<string, Guid> _concurrent = new();
    public override async Task OnConnectedAsync()
    {
        _concurrent.TryAdd(Context.ConnectionId, GetUserId());

        await Task.CompletedTask;
    }

    public async Task UploadStream(string path, string webkitRelativePath, string fileName,
        ChannelReader<byte[]> stream)
    {
        var isException = false;
        var filePath = Path.Combine(path, webkitRelativePath.TrimEnd(fileName.ToCharArray()));
        await using var fileStream =
            System.IO.File.Open(Path.Combine(filePath, fileName), FileMode.Create, FileAccess.Write);
        try
        {
            var len = 0;
            while (await stream.WaitToReadAsync())
            {
                while (stream.TryRead(out var item))
                {
                    _ = Clients.Client(Context.ConnectionId).SendAsync("upload",
                        new UploadModule(fileName, len, false, UploadState.BeingProcessed));
                    len += item.Length;
                    await fileStream.WriteAsync(item);
                }
            }
        }
        catch (Exception e)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("upload",
                new UploadModule(fileName, 0, true, UploadState.BeDefeated, e.Message));
            isException = true;
            fileStream.Close();
            System.IO.File.Delete(Path.Combine(filePath, fileName));
        }
        finally
        {
            if (!isException)
            {
                fileStream.Close();
                await Clients.Client(Context.ConnectionId).SendAsync("upload",
                    new UploadModule(fileName, 0, true, UploadState.Complete));
            }
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _concurrent.Remove(Context.ConnectionId, out _);
        await Task.CompletedTask;
    }

    private Guid GetUserId()
    {
        var value = Context.User?.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        if (value.IsNullOrEmpty())
        {
            throw new BusinessException("未登录");
        }
        return Guid.Parse(value);
    }
}