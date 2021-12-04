using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBridge : MonoBehaviour
{
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("DJIEIFEEFJIEFFEJE");
        gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal*100f, hit.point);
    }
}
