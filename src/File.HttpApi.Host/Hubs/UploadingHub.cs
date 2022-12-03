using File.HttpApi.Host.Module;
using File.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Threading.Channels;
using File.Application.Contract;

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
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }


        await using var fileStream =
            System.IO.File.Open(Path.Combine(filePath, fileName), FileMode.Create, FileAccess.Write);

        try
        {
            var token = CancellationToken.None;
            var state = false;

            _ = Task.Factory.StartNew(async () =>
            {
                const int size = 2;
                for (var i = 0; i < size; i++)
                {
                    state = false;
                    // 等待5s如果没有做上传将取消上传操作
                    await Task.Delay(5000, token);
                    if (!state)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        i = 0;
                    }
                }
            }, token);

            var len = 0;
            var size = 80;
            while (await stream.WaitToReadAsync(token))
            {
                while (stream.TryRead(out var item))
                {
                    state = true;
                    len += item.Length;
                    size--;
                    if (size == 0)
                    {
                        _ = Clients.Client(Context.ConnectionId).SendAsync("upload",
                            new UploadModule(fileName, len, false, UploadState.BeingProcessed), token);
                        size = 80;
                    }

                    await fileStream.WriteAsync(item, token);
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