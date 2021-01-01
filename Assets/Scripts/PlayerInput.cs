using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    PlayerController player;

    Vector3 input = Vector3.zero;
    Vector3 aimPoint = Vector3.zero;
    Camera mainCam;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        player.SetInput(input);

        if (Input.GetButton("Fire1"))
        {
            player.FireEvent();
        }

        if (Input.GetKeyDown("r"))
        {
            player.ReloadEvent();
        }

        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane aimPlane = new Plane(Vector3.up, transform.position);

        if (aimPlane.Raycast(cameraRay, out float cameraDist))
        {
            aimPoint = cameraRay.GetPoint(cameraDist);
        }

        player.SetAimDirection(aimPoint - transform.position);
        //player.SetAimDirection(new Vector3(aimPoint.x - transform.position.x, 0, aimPoint.z - transform.position.z));
    }
}
