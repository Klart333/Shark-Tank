using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : PooledMonoBehaviour
{
    [SerializeField]
    private int shots = 10;

    [SerializeField]
    private float rotationForce = 10;

    private Vector3 lastPosition = new Vector3();

    private float deltaSpeed;
    private float deltaDistance;
    private void Update()
    {
        FollowClick();

        GetSpeed();

        SwingToZero();

        RotateToSpeed();

        if (Input.GetMouseButtonDown(0))
        {
            shots--;
            CheckForDisable();
        }

        lastPosition = transform.position;
    }

    private void RotateToSpeed()
    {
        float currentZ = transform.eulerAngles.z;
        float targetAngle = currentZ - (deltaSpeed * rotationForce);

        Quaternion target = transform.rotation * Quaternion.AngleAxis(targetAngle - transform.eulerAngles.z, new Vector3(0, 0, 1));

        transform.rotation = Quaternion.Lerp(transform.rotation, target, 0.05f);
    }

    private void FollowClick()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = clickPos.ReplaceWith(z: -8); // Replaces the z of the vector3 to the specified -8
        transform.position = position;
    }
    private void GetSpeed()
    {
        deltaDistance = Vector3.Distance(transform.position, lastPosition);

        if (lastPosition.x > transform.position.x || lastPosition.y > transform.position.y) // If we're going negativ on the x or y we set the deltaDistance to negativ
        {
            deltaDistance *= -1;
        }
        

        deltaSpeed = deltaDistance / Time.deltaTime;
    }

    private void SwingToZero()
    {
        Quaternion target = transform.rotation * Quaternion.AngleAxis(0 - transform.eulerAngles.z, new Vector3(0, 0, 1)); // Gets the current rotation and changes the Z value using the AngleAxis method, may be a kind of roundabout way of setting a target with a z of 0, but quaternions dont really work the same way with angles n shit so i don't really feel challenged to improve it 

        transform.rotation = Quaternion.Lerp(transform.rotation, target, 0.01f); // Lerps the rotation between the target of 0 on the z and the current rotation
    }

    private void CheckForDisable()
    {
        if (shots <= 0)
        {
            StartCoroutine(DisableAfterDelay());

        }
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        
        PowerupManager.Instance.gunActive = false;
        shots = 10;

        gameObject.SetActive(false);
    }
}
