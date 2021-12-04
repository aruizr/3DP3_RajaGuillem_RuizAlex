using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeForceApplier : MonoBehaviour
{
    [SerializeField]
    private Rigidbody bridgeRigidBody;

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bridgeRigidBody.AddForceAtPosition(-hit.normal*100f, hit.point);
    }
}
