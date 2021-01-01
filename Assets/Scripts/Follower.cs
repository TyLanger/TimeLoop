using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public PlayerController followerTarget; // need to refactor to make more generic
    PlayerController controller;

    public float timeOffset = 3;
    // how to make this more variable?
    // the offset is related to the current number of points in the list
    // a larger time offset leads to a larger backlog
    // list.length = timeoffset * fixedupdates/second or 1/Time.fixedDeltaTime
    // how do i do multiple actions per fixed update?
    // increasing speed doesn't shoot me past the desired move location so that doesn't help
    // adjust the time scale? Might fail if computer can't keep up
    // move to the average of multiple movements?
    // ex: player moving in a straight line when your time offset changes
    // do you speed up to catch up to the new time offset?
    // mayeb that's the wrong way to think of it. The follower isn't a 'real' thing
    // it's just you 3 seconds ago. When you change the time offset to 2s, it's you from 2 sec ago
    // so it should fade/teleport/lerp whatever looks best
    // I guess the tricky thing is the possible spacial offset
    // Probably the best thing to do would be to just do the intermediate changes all at once or over a short time frame.
    // Should I just use corroutines instead with a default wait time of Time.fixedDeltaTime
    // and that time can change based on different time offsets?
    // I'm not even sure when I will change the time offset
    // maybe teleporting is what I want

    public float moveSpeed = 10f;
    float baseMoveSpeed;
    // want this to be the same or larger than what you're following to keep it 1:1
    // if it's lower, you move less distance than what you're following

    public bool mirrored = false;

    bool destroyElement = true;

    ActionInTime currentNextAction;
    Coroutine nextActionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        baseMoveSpeed = moveSpeed;
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        /// Testing
        /*
        if(Input.GetButtonDown("Jump"))
        {
            timeOffset = 1;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            timeOffset = 2;
        }
        */

        if(currentNextAction == null && followerTarget.actionHistory.Count > 0)
        {
            currentNextAction = followerTarget.actionHistory[0];

            // figure out what type of action it is
            nextActionCoroutine = StartCoroutine(Shoot((timeOffset + currentNextAction.time) - Time.time));
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeOffset)
        {
            if (followerTarget.movementHistory.Count-1 > (int)(timeOffset / Time.fixedDeltaTime))
            {
                // too many elements
                //Debug.LogFormat("Too many elements. Count: {0} Calculation: {1}", followerTarget.movementHistory.Count-1, (int)(timeOffset / Time.fixedDeltaTime));
                
                // this isn't linear, but linear might look the best
                // might need to come back to this and try different stuff depending on how the change in tether is triggered
                // maybe just 2 UpdateMovements per frame is all it needs (double speed)
                int diff = (followerTarget.movementHistory.Count - 1) - (int)(timeOffset / Time.fixedDeltaTime);
                int amountThisFrame = (int)Mathf.Max(1, (diff * 0.2f));
                //amountThisFrame = 10;
                //amountThisFrame = 1;


                //Debug.LogFormat("Diff: {0} This frame: {1}", diff, amountThisFrame);
                for (int i = 0; i < amountThisFrame; i++)
                {
                    // doing a bunch of movements in the same frame allows you to teleport through blocks.
                    // That may or may not be wanted. It might only be 'intuitive' to people who program their movement like this. General gamers might have a different idea of how this works
                    // it's something like fast forwarding changing the time offset
                    UpdateMovement();
                }
            }
            else if(followerTarget.movementHistory.Count - 1 < (int)(timeOffset / Time.fixedDeltaTime))
            {
                //Debug.Log("Too few");
                //Debug.LogFormat("Too many elements. Count: {0} Calculation: {1}", followerTarget.movementHistory.Count-1, (int)(timeOffset / Time.fixedDeltaTime));

                // skip until caught up?
                // would like to just go slow, but how?
                // small problem is updateMovement destroying the list
                // this causes it to take longer to catch up than just standing still and waiting
                // possible solution
                // half move speed, do each element in the list twice
                // don't destroy the element every other time. This causes you to do each movement twice
                destroyElement = !destroyElement;
                moveSpeed = baseMoveSpeed * 0.5f;
            }
            else
            {
                moveSpeed = baseMoveSpeed;
                destroyElement = true;
            }

            UpdateMovement(destroyElement);
        }
    }

    void UpdateMovement()
    {
        UpdateMovement(true);
    }

    void UpdateMovement(bool destroyElement)
    {
        Vector3 deltaPosition = Vector3.zero;
        if (mirrored)
        {
            // should probably also reverse the rotation
            deltaPosition = new Vector3(-followerTarget.movementHistory[0].movement.x, followerTarget.movementHistory[0].movement.y, followerTarget.movementHistory[0].movement.z);
        }
        else
        {
            deltaPosition = followerTarget.movementHistory[0].movement;
        }

        if(controller != null)
        {
            controller.SetInput(deltaPosition);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + deltaPosition, moveSpeed * Time.fixedDeltaTime);
        }

        transform.rotation = followerTarget.movementHistory[0].rotation;

        if (destroyElement)
            followerTarget.movementHistory.RemoveAt(0);
    }

    IEnumerator Shoot(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        controller?.FireEvent();
        // missing some bullets
        // this coroutine runs correctly, but not all bullets are spawned
        // possibly a small discrepency between the timing
        // time is 12.345677
        // time of next attack is 12.345678
        // off by 0.000001
        Debug.Log("Bam! Bam! at " + Time.time);
        ClearCurrentAction();
        yield return null;
    }

    void ClearCurrentAction()
    {
        followerTarget.actionHistory.RemoveAt(0);
        currentNextAction = null;
    }
}
