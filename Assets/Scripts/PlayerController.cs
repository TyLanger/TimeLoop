using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10;
    Vector2 input = Vector2.zero;

    public List<PointInTime> history;
    float historyLength = 30;

    // Start is called before the first frame update
    void Start()
    {
        history = new List<PointInTime>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(input.x, 0, input.y), moveSpeed * Time.fixedDeltaTime);
        Record();
    }

    void Record()
    {
        //PointInTime current = new PointInTime(transform.position, transform.rotation);

        if(history.Count > historyLength / Time.fixedDeltaTime)
        {
            history.RemoveAt(history.Count - 1);
        }

        history.Add(new PointInTime(transform.position, transform.rotation));
    }

}
