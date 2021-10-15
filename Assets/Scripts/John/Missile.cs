using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : PooledMonoBehaviour
{
    [SerializeField]
    private float speed = 2;

    private Shark target;

    private void Update()
    {
        if (target != null && target.isActiveAndEnabled)
        {
            Vector3 targetDir = (target.transform.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(transform.forward, targetDir);
            var smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.035f);

            transform.rotation = smoothedRotation;
        }
        else
        {
            var sharks = FindObjectsOfType<Shark>(false);
            print(sharks.Length);
            if (sharks.Length > 0)
            {
                target = sharks[UnityEngine.Random.Range(0, sharks.Length)];
            }
        }

        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        Shark shark = other.GetComponent<Shark>();
        if (shark != null)
        {
            shark.OnClicked();
        }
    }
}
