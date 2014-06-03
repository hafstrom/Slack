﻿using SlackAPI;
using System;
using System.Threading;

namespace SlackInterface.Adapters
{
    public class ChannelAdapter : MessagesAdapter
    {
        protected Channel info;

        public ChannelAdapter(Channel info, ConnectedInterface connected, Slack client)
        {
            this.info = info;
            this.connected = connected;
            this.client = client;
        }

        public override string GetId()
        {
            return info.id;
        }

        public override string GetTitle()
        {
            return info.name;
        }

        public override string GetTopic()
        {
            return info.topic == null ? "" : info.topic.value;
        }

        public override MessageHistory GetMessages(DateTime? latest = null, DateTime? oldest = null, int? count = null)
        {
            MessageHistory history = null;
            EventWaitHandle wait = new EventWaitHandle(false, EventResetMode.ManualReset);
            client.GetChannelHistory((his) =>
            {
                history = his;
                wait.Set();
            }, info, latest, oldest, count);
            wait.WaitOne();

            return history;
        }

        public override string GetChannelPrefix()
        {
            return "#";
        }
    }
}
