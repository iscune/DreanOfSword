using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class NotifierManager
{
    public static Notifier notifier = new Notifier();
    public static void registerNotification(Enum type, Action<Notification> receiver)
    {
        notifier.Add(type, receiver);
    }

    public static void SendNotification(Enum type, params Object[] datas)
    {
        notifier.Send(type, datas);
    }
}