using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private Transform playerMeshTransform;

    public void SetPlayerMovementSpeed(float speed, bool isMovingObject, bool isPushing)
    {
        if (speed == 0)
            isMovingObject = false;

        playerAnimator.SetBool("isPushingObject", isPushing);
        playerAnimator.SetBool("isMovingObject", isMovingObject);
        playerAnimator.SetInteger("moveSpeed",(int)speed);        
        
    }

    public void SetPlayerMeshRotationBasedOnVelocity(Vector3 velocity)
    {
        float sideDirection = Mathf.Sign(velocity.x);

        Vector3 playerMeshRotation = Vector3.zero;
        
        if(sideDirection == 1)
        {
            playerMeshRotation.y = 90;
        }
        else if(sideDirection == -1)
        {
            playerMeshRotation.y = 270;
        }

        playerMeshTransform.localEulerAngles = playerMeshRotation;
    }

    public void SetPlayerMeshRotation(Transform mesh)
    {
        playerMeshTransform.rotation = mesh.rotation;
    }
   

    public void SetPlayerFalling(bool isFalling)
    {
        playerAnimator.SetBool("falling", isFalling);
    }

    public void SetPlayerCastingLightBeam(bool isCasting)
    {
        playerAnimator.SetBool("isHitByLightBeam",false);
        playerAnimator.SetBool("isCastingLightBeam",isCasting);
    }

    public void SetPlayerHitByLightBeam()
    {
        playerAnimator.SetBool("isHitByLightBeam",true);        
    }
}
