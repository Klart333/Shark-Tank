using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Vector3 StartDeg = new Vector3(0, 0, 0);
    public Vector3 EndDeg = new Vector3(0, 0, 10);

    public float Speed = 1;

    private float t = 0;

    private bool goingUp = true;
    private bool goingDown = true;

    private void Update()
    {
        t += Time.deltaTime * Speed;
        t = Mathf.Clamp01(t);
        if (goingUp)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(StartDeg), Quaternion.Euler(EndDeg), t);
            
            if (t >= 1)
            {
                goingUp = false;
                goingDown = true;
                t = 0;
            }
        }
        else if (goingDown)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(EndDeg), Quaternion.Euler(StartDeg), t);

            if (t >= 1)
            {
                goingUp = true;
                goingDown = false;
                t = 0;
            }
        }
    }
}
