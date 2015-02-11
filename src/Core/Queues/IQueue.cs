﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Foundatio.Queues {
    public interface IQueue<T> : IDisposable where T : class {
        string Enqueue(T data);
        void StartWorking(Action<QueueEntry<T>> handler, bool autoComplete = false);
        void StopWorking();
        QueueEntry<T> Dequeue(TimeSpan? timeout = null);
        void Complete(string id);
        void Abandon(string id);
        // TODO: Change to get all stats at the same time to allow optimization of retrieval.
        long GetQueueCount();
        long GetWorkingCount();
        long GetDeadletterCount();
        IEnumerable<T> GetDeadletterItems();
        void DeleteQueue();
        long EnqueuedCount { get; }
        long DequeuedCount { get; }
        long CompletedCount { get; }
        long AbandonedCount { get; }
        long WorkerErrorCount { get; }
        string QueueId { get; }
    }

    public interface IQueue2<T> : IDisposable where T : class {
        Task<string> EnqueueAsync(T data);
        void StartWorking(Func<QueueEntry<T>, Task> handler, bool autoComplete = false);
        void StopWorking();
        Task<QueueEntry<T>> DequeueAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default(CancellationToken));
        Task CompleteAsync(string id);
        Task AbandonAsync(string id);
        Task<QueueStats> GetQueueStatsAsync();
        Task<IEnumerable<T>> GetDeadletterItemsAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteQueueAsync();
        string QueueId { get; }
    }

    public class QueueStats {
        public long Active { get; private set; }
        public long Working { get; private set; }
        public long Deadletter { get; private set; }
        public long LocalEnqueued { get; private set; }
        public long LocalDequeued { get; private set; }
        public long LocalCompleted { get; private set; }
        public long LocalAbandoned { get; private set; }
        public long LocalWorkerErrors { get; private set; }
    }
}
