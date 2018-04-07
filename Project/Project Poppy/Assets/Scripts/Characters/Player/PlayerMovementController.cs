using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerMovementController : MonoBehaviour
{
    private Controller2D controller;

    private float gravity = -20f;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    private void Update()
    {
        float xInput = xInput = Input.GetAxisRaw("Horizontal"); ;

        if(controller.GetObjectOrientation() == 0 || controller.GetObjectOrientation() == 180)
        {
           
           
        }       
       

        if(controller.GetObjectOrientation() == 0)
        {
            velocity.x = xInput * 6f;
            velocity.y += -20 * Time.deltaTime;
        }
        else if(controller.GetObjectOrientation() == 180)
        {
            velocity.x = -xInput * 6f;
            velocity.y -= 20 * Time.deltaTime;
        }
        
        controller.Move(velocity * Time.deltaTime);
      
    }
}
