﻿using System;

namespace BlazorWorker.WorkerCore
{
    /// <summary>
    /// Simple static message service that runs in the worker thread.
    /// </summary>
    public class MessageService
    {
        private static readonly DOMObject self = new DOMObject("self");

        public static event EventHandler<string> Message;

        static MessageService()
        {   
        }

        public static void OnMessage(string message)
        {
            Message?.Invoke(null, message);
#if DEBUG
            Console.WriteLine($"{nameof(MessageService)}.{nameof(OnMessage)}: {message}");
#endif
        }

        public static void PostMessage(string message)
        {
            self.Invoke("postMessage", message);
        }

        public static void Dispose()
        {
            self.Dispose();
        }
    }
}
