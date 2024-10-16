using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public Transform Control;

    public Vector3 evaluate(float t)
    {
        Vector3 ac = Vector3.Slerp(A.position, Control.position, t);
        Vector3 cb = Vector3.Slerp(Control.position, B.position, t);
        return Vector3.Slerp(ac, cb, t);
    }

    private void OnDrawGizmos()
    {
        if (A == null || B == null || Control == null) return;

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(evaluate(i / 20f), 0.06f);
        }
    }
}