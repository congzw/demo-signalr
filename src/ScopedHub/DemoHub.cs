using Microsoft.AspNetCore.SignalR;

namespace ScopedHub
{
    //基本事件
    public partial class DemoHub : Hub
    {
        //连接
        //public Task OnConnectedAsync()

        //断开
        //public Task OnDisconnectedAsync(Exception exception)

        //更新设备数据
        //public Task UpdateDeviceData(UpdateDeviceDataVo updateDeviceDataVo)

        //更新APP控制端 白板缩略图
        //public Task UpdateAppControlSnap(string podId, string snapUrl)

        //状态改变
        //public Task StatusChange(string deviceId, Dictionary<string, object> data)
    }

    //界面事件
    public partial class DemoHub : Hub
    {
        //全屏
        //public Task Fullscreen(FullscreenVo device)

        //截屏
        //public Task ShotScreen(string deviceId)

        //设备切换
        //public Task Switch(SwitchVo switchVo)

        //广播
        //public Task Broadcast(BroadcastVo broadcastVo)

        //分屏
        //public Task SplitScreen(SplitScreenVo splitScreenVo)

        //增加POD分屏
        //public Task AddSplitScreenPod(SplitScreenPod pod)
    }

    //播放器事件
    public partial class DemoHub : Hub
    {
        //开始推流
        //public Task StartPush(Client client)
        //结束推流
        //public Task EndPush(Client client)
        //播放器调整位置？
        //public Task PlayerOrientation(string orientation)
        //状态改变
        //public Task StatusChange(string deviceId, Dictionary<string, object> data)
    }

    //白板相关
    public partial class DemoHub : Hub
    {
        //开始绘画
        //public Task StrokeBegin(StrokeBeginVo paramsVo, string deviceId)

        //绘画
        //public Task Draw(List<decimal> Point, string deviceId)

        //撤销
        //public Task Undo(DrawActionVo drawActionVo)

        //清除
        //public Task Clear(DrawActionVo drawActionVo)

        //标注
        //public Task Mark(Client device)
    }

    //日志相关
    public partial class DemoHub : Hub
    {
        //public Task StartSpan(ClientSpan args)
        //public Task Log(LogArgs args)
        //public Task SetTags(SetTagArgs args)
        //public Task FinishSpan(FinishSpanArgs args)
        //private Task SafeCallback(string method, Func<Task> apiFunc, IClientSpanLocate args, bool locateForCurrent)
    }
}
