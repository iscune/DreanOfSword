using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject player;

    Vector3 disWithPlayer;
    private void Awake()
    {
        disWithPlayer = player.transform.position - transform.position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        transform.position = player.transform.position - disWithPlayer;
        transform.LookAt(player.transform.position);
    }
}
