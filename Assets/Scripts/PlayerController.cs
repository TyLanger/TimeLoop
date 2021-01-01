using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 input = Vector3.zero;
    [SerializeField] float moveSpeed = 10;

    public List<PointInTime> movementHistory;
    float historyLength = 30;
    public List<ActionInTime> actionHistory;

    [SerializeField] Gun gun;

    public bool recording = true;

    // Start is called before the first frame update
    void Start()
    {
        movementHistory = new List<PointInTime>();
        actionHistory = new List<ActionInTime>();
    }

    void FixedUpdate()
    {
        Vector3 oldPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + input, moveSpeed * Time.fixedDeltaTime);
        RecordMovement(transform.position - oldPos);
    }

    public void SetInput(Vector3 _input)
    {
        input = _input;
    }

    public void SetAimDirection(Vector3 _forward)
    {
        transform.forward = _forward;
    }

    public void FireEvent()
    {
        if (gun == null)
            return;

        // shoot
        if (gun.Shoot())
        {
            // returns true if you actually shoot a bullet
            RecordAction();
        }
    }

    public void ReloadEvent()
    {
        if (gun == null)
            return;


        if(gun.StartReload())
        {
            //RecordAction(reload);
        }
    }

    void RecordMovement(Vector3 positionDelta)
    {
        // your ghost doesn't need to record its own actions
        if (!recording)
            return;

        if (movementHistory.Count > historyLength / Time.fixedDeltaTime)
        {
            movementHistory.RemoveAt(movementHistory.Count - 1);
        }

        movementHistory.Add(new PointInTime(positionDelta, transform.rotation));
    }

    void RecordAction()
    {
        if (!recording)
            return;

        if (actionHistory.Count > historyLength/Time.fixedDeltaTime)
        {
            actionHistory.RemoveAt(actionHistory.Count - 1);
        }

        actionHistory.Add(new ActionInTime(Time.time));
    }

}
