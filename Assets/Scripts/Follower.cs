using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public PlayerController followerTarget; // need to refactor to make more generic

    public float timeOffset = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeOffset)
        {
            transform.position = followerTarget.history[0].position;
            transform.rotation = followerTarget.history[0].rotation;
            followerTarget.history.RemoveAt(0);
        }
    }
}
