using System;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Pushover;

namespace Pushover.NLog
{
    [Target("Pushover")]
    public class PushoverTarget : TargetWithLayout
    {
        public PushoverTarget()
        {
            ApiBase = "https://api.pushover.net/1/messages.json";
            Title = "${level:uppercase=true}";
            Name = "Pushover";
            EmergencyMessageRetryInterval = 30;
            EmergencyMessageExpiration = 60*60; //1h
        }

        #region Pushover Parameters
        [RequiredParameter]
        public string ApiBase { get; set; }

        [RequiredParameter]
        public string AppToken { get; set; }

        [RequiredParameter]
        public string UserOrGroupKey { get; set; }

        public string Device { get; set; }

        public Layout Title { get; set; }

        public string Url { get; set; }
        public string UrlTitle { get; set; }
        public int EmergencyMessageRetryInterval { get; set; }
        public int EmergencyMessageExpiration { get; set; }
        #endregion

        protected override void Write(LogEventInfo logEvent)
        {
            Pushover.PushoverClient client = new PushoverClient(AppToken);
            PushoverMessageBase message = new PushoverMessageBase()
            {
                Device = Device,
                Message = Layout.Render(logEvent),
                Timestamp = DateTime.UtcNow,
                Title = Title.Render(logEvent),
                User = UserOrGroupKey,
                Url = Url,
                UrlTitle = UrlTitle,
                Priority = GetPriority(logEvent.Level),
                Expiration = EmergencyMessageExpiration,
                Retry = EmergencyMessageRetryInterval
            };
            client.Push(message);
        }

        /// <summary>
        /// Maps an NLog-LogLevel to a Pushover Priority
        /// </summary>
        private MessagePriority GetPriority(LogLevel level)
        {
            switch (level.Ordinal)
            {
                case 2: //Info
                    return MessagePriority.Low;
                case 3: //Warning
                    return MessagePriority.Normal;
                case 4: //Error
                    return MessagePriority.High;
                case 5: //Fatal
                    return MessagePriority.Emergency;
                default: //Debug and Trace
                    return MessagePriority.Lowest;

            }
        }
    }
}
