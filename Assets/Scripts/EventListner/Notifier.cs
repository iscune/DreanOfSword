using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Notifier
{
    Dictionary<Enum, Action<Notification>> m_DictAction = new Dictionary<Enum, Action<Notification>>();
    Object m_Target = null;
    byte m_LifeState = 1;
    public Notifier()
    {
        m_Target = this;
        Init();
    }
    public Notifier(Object target)
    {
        if (target == null)
            throw new Exception("Target is null,please use New Notifier() instead");
        m_Target = target;
        Init();
    }

    void Init()
    {
        MethodInfo[] methods = m_Target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (MethodInfo m in methods)
        {
            Subscribe[] subs = (Subscribe[])m.GetCustomAttributes(typeof(Subscribe), true);
            if (subs != null && subs.Length > 0)
                Add(subs[0].GetSubscription(), Delegate.CreateDelegate(typeof(Action<Notification>), m_Target, m.Name) as Action<Notification>);
        }
    }

    public void Awake() { m_LifeState = 1; }
    public void Sleep() { m_LifeState = 0; }
    void _Execute(Enum type, Notification note)
    {
        if (m_LifeState == 0) return;
        if (m_DictAction.ContainsKey(type))
        {
            if (m_DictAction[type] != null)
                m_DictAction[type](note);
        }
    }

    public void Add(Enum type, Action<Notification> receiver)
    {
        if (receiver == null)
            throw new Exception("Receiver is null");
        if (m_DictAction.ContainsKey(type))
            throw new Exception(string.Format("{0} has added", type.ToString()));
        m_DictAction[type] = receiver;
        _Add(type, this);
    }

    public void Remove(Enum type)
    {
        m_DictAction.Remove(type);
        _Remove(type, this);
    }
    public void RemoveAll()
    {
        ICollection keys = m_DictAction.Keys;
        Enum[] arrTemp = new Enum[keys.Count];
        keys.CopyTo(arrTemp, 0);
        foreach (Enum type in arrTemp)
            Remove(type);
    }

    public void Send(Enum type, params Object[] datas)
    {
        Notification note = new Notification();
        note.Type = type;
        note.Params = datas;
        note.Target = m_Target;
        _Send(type, note);
    }


    public void Destory()
    {
        RemoveAll();
        m_DictAction = null;
        m_Target = null;
    }

    #region static functions
    static readonly Dictionary<Enum, List<Notifier>> s_DictNotifiers = new Dictionary<Enum, List<Notifier>>();
    static void _Add(Enum type, Notifier notifier)
    {
        if (!s_DictNotifiers.ContainsKey(type))
            s_DictNotifiers[type] = new List<Notifier>();
        if (!s_DictNotifiers[type].Contains(notifier))
            s_DictNotifiers[type].Add(notifier);
    }

    static void _Remove(Enum type, Notifier notifier)
    {
        if (s_DictNotifiers.ContainsKey(type))
        {
            if (s_DictNotifiers[type].Contains(notifier))
                s_DictNotifiers[type].Remove(notifier);
        }
    }

    static void _Send(Enum type, Notification note)
    {
        if (s_DictNotifiers.ContainsKey(type))
        {
            List<Notifier> notifiers = s_DictNotifiers[type];
            int count = notifiers.Count;
            for (int i = count - 1; i > -1; --i)
            {
                if (notifiers[i] != null)
                    notifiers[i]._Execute(type, note);
            }
        }
    }
    #endregion
}