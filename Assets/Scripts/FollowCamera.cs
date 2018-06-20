using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject player;

    Vector3 disWithPlayer = new Vector3(0,5,3);
    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        transform.position = player.transform.position + disWithPlayer;
        transform.LookAt(player.transform.position);
    }
}
