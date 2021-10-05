using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{
    [SerializeField]
    private SimpleAudioEvent punchSFX;

    private UIHitSpree hitSpreeScript;
    private AudioSource audioSource;

    private void Start()
    {
        hitSpreeScript = FindObjectOfType<UIHitSpree>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (ShouldClick())
        {
            punchSFX.Play(audioSource); // Whether he hit or miss, works with the gun because this script is disabled
            if (Click() == true) // If we hit something
            {

            }
            else
            {
                GameManager.Instance.hitSpree = 0;
                hitSpreeScript.UpdateHitSpree();
            }

        }
    }
    private bool ShouldClick()
    {
        return (Input.GetMouseButtonDown(0) && !GameManager.Instance.Frozen);
    }

    private bool Click()
    {
        Vector3 position = GetWorldPointClicked();

        IClickable target = null;
        target = TryClickAtPosition(position);

        return (target != null); // We hit something if target isn't null
    }

    private Vector3 GetWorldPointClicked()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f); // I don't understand why z has to be 10, and it has to be 10
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    private IClickable TryClickAtPosition(Vector3 position)
    {
        IClickable target = null;

        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);

        foreach (Collider hit in hitColliders)
        {
            target = hit.GetComponent<IClickable>();
            if (target != null)
            {
                target.OnClicked();
            }
        }

        return target;
    }


}
