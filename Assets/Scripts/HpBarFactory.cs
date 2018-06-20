using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarFactory :MonoBehaviour
{
    public Slider template;

    List<Slider> pool;

    protected virtual void Start()
    {
        pool = new List<Slider>();
    }

    Slider Create()
    {
        Slider obj = (Slider) Instantiate(template);

        GameObject hpBarPool = GameObject.Find("HpBarPool");
        if (!hpBarPool)
        {
            GameObject canvas = GameObject.Find("Canvas");
            hpBarPool = new GameObject();
            hpBarPool.GetComponent<Transform>().SetParent(canvas.GetComponent<Transform>());
        }
        obj.GetComponent<Transform>().SetParent(hpBarPool.GetComponent<Transform>());
        return obj;
    }

    public Slider Get()
    {
        foreach(Slider sl1 in pool)
        {
            if (!sl1.IsActive())
            {
                sl1.enabled = true;
                return sl1;
            }
        }

        Slider sl = Create();
        pool.Add(sl);

        return sl;
    }
}
