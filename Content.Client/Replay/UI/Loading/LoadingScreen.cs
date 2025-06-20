// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.ResourceManagement;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.Timing;

namespace Content.Client.Replay.UI.Loading;

[Virtual]
public class LoadingScreen<TResult> : State
{
    [Dependency] private readonly IResourceCache _resourceCache = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

    public event Action<TResult?, Exception?>? OnJobFinished;
    private LoadingScreenControl _screen = default!;
    public Job<TResult>? Job;

    public override void FrameUpdate(FrameEventArgs e)
    {
        base.FrameUpdate(e);
        if (Job == null)
            return;

        Job.Run();
        if (Job.Status != JobStatus.Finished)
            return;

        OnJobFinished?.Invoke(Job.Result, Job.Exception);
        Job = null;
    }

    protected override void Startup()
    {
        _screen = new(_resourceCache);
        _userInterfaceManager.StateRoot.AddChild(_screen);
    }

    protected override void Shutdown()
    {
        _screen.Dispose();
    }

    public void UpdateProgress(float value, float maxValue, string header, string subtext = "")
    {
        _screen.Bar.Value = value;
        _screen.Bar.MaxValue = maxValue;
        _screen.Header.Text = header;
        _screen.Subtext.Text = subtext;
    }
}