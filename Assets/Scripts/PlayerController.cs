using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10;
    Vector2 input = Vector2.zero;

    public List<PointInTime> movementHistory;
    float historyLength = 30;

    // Start is called before the first frame update
    void Start()
    {
        movementHistory = new List<PointInTime>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        Vector3 oldPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(input.x, 0, input.y), moveSpeed * Time.fixedDeltaTime);
        Record(transform.position - oldPos);
    }

    void Record(Vector3 positionDelta)
    {
        if (movementHistory.Count > historyLength / Time.fixedDeltaTime)
        {
            movementHistory.RemoveAt(movementHistory.Count - 1);
        }

        movementHistory.Add(new PointInTime(positionDelta, transform.rotation));
    }

}
