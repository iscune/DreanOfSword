using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GameController : MonoBehaviour {

    public GameObject enermy;
	// Use this for initialization
	void Start () {
        MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (MethodInfo m in methods)
        {
            Subscribe[] subs = (Subscribe[])m.GetCustomAttributes(typeof(Subscribe), true);
            if (subs != null && subs.Length > 0)
                NotifierManager.registerNotification(subs[0].GetSubscription(), Delegate.CreateDelegate(typeof(Action<Notification>), this, m.Name) as Action<Notification>);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [Subscribe(PublicEnum.NotifierSendType.EnemyDie)]
    void enemyDieListener(Notification note)
    {
         var obj = Instantiate(enermy) as GameObject;   
    }
}
