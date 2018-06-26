using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GameController : MonoBehaviour {

    public GameObject enermy;

    public GameObject birthPlaceMin;
    public GameObject birthPlaceMax;

    int eneymyBirthNum;
    int eneymyBirthCount;

    Rect randomBirthRect;
	// Use this for initialization
	void Start () {
        MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (MethodInfo m in methods)
        {
            Subscribe[] subs = (Subscribe[])m.GetCustomAttributes(typeof(Subscribe), true);
            if (subs != null && subs.Length > 0)
                NotifierManager.registerNotification(subs[0].GetSubscription(), Delegate.CreateDelegate(typeof(Action<Notification>), this, m.Name) as Action<Notification>);
        }

        eneymyBirthNum = 1;
        eneymyBirthCount = 1;
        randomBirthRect = new Rect(birthPlaceMin.transform.position.x,birthPlaceMin.transform.position.y,birthPlaceMax.transform.position.x - birthPlaceMin.transform.position.x,birthPlaceMax.transform.position.y - birthPlaceMin.transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [Subscribe(PublicEnum.NotifierSendType.EnemyDie)]
    void enemyDieListener(Notification note)
    {
        eneymyBirthCount = eneymyBirthCount - 1;
        if (eneymyBirthCount <= 0)
        {
            eneymyBirthNum = eneymyBirthNum * 2;
            eneymyBirthCount = eneymyBirthNum;
            StartCoroutine(CreateEntity());
        }
    }

    IEnumerator CreateEntity()
    {
        for(int i = 0;i< eneymyBirthNum; i++)
        {
            var obj = Instantiate(enermy) as GameObject;

            var x = UnityEngine.Random.Range(randomBirthRect.x, randomBirthRect.width + randomBirthRect.x);
            var y = UnityEngine.Random.Range(randomBirthRect.y, randomBirthRect.height + randomBirthRect.y);
            obj.transform.position = new Vector3(x, y, birthPlaceMin.transform.position.z);
            yield return new WaitForSeconds(1f);

        }
    }
}
