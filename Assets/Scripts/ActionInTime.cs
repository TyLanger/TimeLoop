using UnityEngine;

public class ActionInTime
{
    public float time;

    public ActionInTime(float _time)
    {
        time = _time;
        // actionType = type;
        // type will be like shoot or reload maybe?
        Debug.Log("Set an action at " + _time);
    }
}
