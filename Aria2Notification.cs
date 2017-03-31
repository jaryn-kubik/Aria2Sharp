using System;
using System.Collections.Generic;

namespace Aria2.NET
{
    public enum Aria2Notification
    {
        OnDownloadStart,
        OnDownloadPause,
        OnDownloadStop,
        OnDownloadComplete,
        OnDownloadError,
        OnBtDownloadComplete
    }

    public class Aria2NotificationEventArgs : EventArgs
    {
        public Aria2Notification Notification { get; }
        public string Gid { get; }
        public Aria2NotificationEventArgs(string notification, string gid)
        {
            Notification = notifications[notification];
            Gid = gid;
        }

        private static readonly Dictionary<string, Aria2Notification> notifications = new Dictionary<string, Aria2Notification>
        {
            ["aria2.onDownloadStart"] = Aria2Notification.OnDownloadStart,
            ["aria2.onDownloadPause"] = Aria2Notification.OnDownloadPause,
            ["aria2.onDownloadStop"] = Aria2Notification.OnDownloadStop,
            ["aria2.onDownloadComplete"] = Aria2Notification.OnDownloadComplete,
            ["aria2.onDownloadError"] = Aria2Notification.OnDownloadError,
            ["aria2.onBtDownloadComplete"] = Aria2Notification.OnBtDownloadComplete
        };
    }
}