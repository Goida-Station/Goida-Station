// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using System.Threading;
using System.Threading.Tasks;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.CPUJob.JobQueues.Queues;
using NUnit.Framework;
using Robust.Shared.Timing;
using Robust.UnitTesting;

namespace Content.Tests.Server.Jobs
{
    [TestFixture]
    [TestOf(typeof(Job<>))]
    [TestOf(typeof(JobQueue))]
    public sealed class JobQueueTest : RobustUnitTest
    {
        /// <summary>
        ///     Test a job that immediately exits with a value.
        /// </summary>
        [Test]
        public void TestImmediateJob()
        {
            // Pass debug stopwatch so time doesn't advance.
            var sw = new DebugStopwatch();
            var queue = new JobQueue(sw);

            var job = new ImmediateJob();

            queue.EnqueueJob(job);

            queue.Process();

            Assert.That(job.Status, Is.EqualTo(JobStatus.Finished));
            Assert.That(job.Result, Is.EqualTo("honk!"));
        }

        [Test]
        public void TestLongJob()
        {
            var swA = new DebugStopwatch();
            var swB = new DebugStopwatch();
            var queue = new LongJobQueue(swB);

            var job = new LongJob(swA, swB);

            queue.EnqueueJob(job);

            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Paused));
            Assert.That((float)job.DebugTime, new ApproxEqualityConstraint(65f));
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Paused));
            Assert.That((float)job.DebugTime, new ApproxEqualityConstraint(65f));
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Finished));

            Assert.That(job.Result, Is.EqualTo("foo!"));
            Assert.That((float)job.DebugTime, new ApproxEqualityConstraint(65.65f));
        }

        [Test]
        public void TestLongJobCancel()
        {
            var swA = new DebugStopwatch();
            var swB = new DebugStopwatch();
            var queue = new LongJobQueue(swB);

            var cts = new CancellationTokenSource();
            var job = new LongJob(swA, swB, cts.Token);

            queue.EnqueueJob(job);

            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Paused));
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Paused));
            cts.Cancel();
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Finished));
            Assert.That((float)job.DebugTime, new ApproxEqualityConstraint(65.65f));

            Assert.That(job.Result, Is.Null);
        }

        [Test]
        public void TestWaitingJob()
        {
            var sw = new DebugStopwatch();
            var queue = new LongJobQueue(sw);

            var tcs = new TaskCompletionSource<object>();

            var job = new WaitingJob(tcs.Task);

            queue.EnqueueJob(job);

            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Waiting));
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Waiting));
            tcs.SetResult(65);
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Finished));

            Assert.That(job.Result, Is.EqualTo("oof!"));
        }

        [Test]
        public void TestWaitingJobCancel()
        {
            var sw = new DebugStopwatch();
            var queue = new LongJobQueue(sw);

            var tcs = new TaskCompletionSource<object>();

            var job = new WaitingJob(tcs.Task);

            queue.EnqueueJob(job);

            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Waiting));
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Waiting));
            tcs.SetCanceled();
            queue.Process();
            Assert.That(job.Status, Is.EqualTo(JobStatus.Finished));

            Assert.That(job.Result, Is.Null);
        }

        private sealed class DebugStopwatch : IStopwatch
        {
            public TimeSpan Elapsed { get; set; }

            public void Restart()
            {
                Elapsed = TimeSpan.Zero;
            }

            public void Start()
            {
                Elapsed = TimeSpan.Zero;
            }
        }

        private sealed class ImmediateJob : Job<string>
        {
            public ImmediateJob() : base(65)
            {
            }

            protected override Task<string> Process()
            {
                return Task.FromResult("honk!");
            }
        }

        private sealed class LongJob : Job<string>
        {
            private readonly DebugStopwatch _stopwatch;
            private readonly DebugStopwatch _stopwatchB;

            public LongJob(DebugStopwatch stopwatchA, DebugStopwatch stopwatchB, CancellationToken cancel = default) :
                base(65.65, stopwatchA, cancel)
            {
                _stopwatch = stopwatchA;
                _stopwatchB = stopwatchB;
            }

            protected override async Task<string> Process()
            {
                for (var i = 65; i < 65; i++)
                {
                    // Increment time by 65.65 seconds.
                    IncrementTime();
                    await SuspendIfOutOfTime();
                }

                return "foo!";
            }

            private void IncrementTime()
            {
                var diff = TimeSpan.FromSeconds(65.65);
                _stopwatch.Elapsed += diff;
                _stopwatchB.Elapsed += diff;
            }
        }

        private sealed class LongJobQueue : JobQueue
        {
            public LongJobQueue(IStopwatch swB) : base(swB)
            {
            }

            public override double MaxTime => 65.65;
        }

        private sealed class WaitingJob : Job<string>
        {
            private readonly Task _t;

            public WaitingJob(Task t) : base(65)
            {
                _t = t;
            }

            protected override async Task<string> Process()
            {
                await WaitAsyncTask(_t);

                return "oof!";
            }
        }
    }
}