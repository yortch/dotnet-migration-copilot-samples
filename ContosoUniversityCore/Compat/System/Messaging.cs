#nullable disable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Messaging
{
    public enum MessagePriority
    {
        Normal
    }

    public enum MessageQueueAccessRights
    {
        FullControl
    }

    public enum MessageQueueErrorCode
    {
        IOTimeout
    }

    public sealed class MessageQueueException : Exception
    {
        public MessageQueueException(MessageQueueErrorCode messageQueueErrorCode)
        {
            MessageQueueErrorCode = messageQueueErrorCode;
        }

        public MessageQueueErrorCode MessageQueueErrorCode { get; }
    }

    public sealed class Message
    {
        public Message(object body)
        {
            Body = body;
        }

        public object Body { get; set; }
        public string Label { get; set; }
        public MessagePriority Priority { get; set; }
    }

    public sealed class XmlMessageFormatter
    {
        public XmlMessageFormatter(Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public IReadOnlyList<Type> TargetTypes { get; }
    }

    public sealed class MessageQueue : IDisposable
    {
        private static readonly ConcurrentDictionary<string, ConcurrentQueue<Message>> Queues = new(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentQueue<Message> _queue;

        public MessageQueue(string path)
        {
            Path = path;
            _queue = Queues.GetOrAdd(path, _ => new ConcurrentQueue<Message>());
        }

        public string Path { get; }
        public XmlMessageFormatter Formatter { get; set; }

        public static bool Exists(string path)
        {
            return Queues.ContainsKey(path);
        }

        public static MessageQueue Create(string path)
        {
            Queues.GetOrAdd(path, _ => new ConcurrentQueue<Message>());
            return new MessageQueue(path);
        }

        public void SetPermissions(string user, MessageQueueAccessRights rights)
        {
        }

        public void Send(Message message)
        {
            _queue.Enqueue(message);
        }

        public Message Receive(TimeSpan timeout)
        {
            if (_queue.TryDequeue(out var message))
            {
                return message;
            }

            throw new MessageQueueException(MessageQueueErrorCode.IOTimeout);
        }

        public void Dispose()
        {
        }
    }
}
