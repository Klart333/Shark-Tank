using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunClick : MonoBehaviour // Basically the same as ClickScript, with the exceptions of increase to hitspree and the sound effect played. I felt like this was a really clear way of doing things instead of placing checks in ClickScript. 
{
    [SerializeField]
    private SimpleAudioEvent gunshotSFX;

    private UIHitSpree hitSpree;
    private ClickScript punchScript;
    private AudioSource audioSource;
    private void Awake()
    {
        hitSpree = FindObjectOfType<UIHitSpree>(); // The guns are pooled so it should be fine
        punchScript = FindObjectOfType<ClickScript>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        punchScript.enabled = false;
    }
    void Update()
    {
        if (ShouldClick())
        {
            gunshotSFX.Play(this.audioSource);
            if (Click() == true) // If we hit something
            {
                GameManager.Instance.hitSpree += 2; // The gun increases the hitspree with double
                hitSpree.UpdateHitSpree();
            }
            else
            {

                GameManager.Instance.hitSpree = 0;
                hitSpree.UpdateHitSpree();
            }

        }
    }

    private bool ShouldClick()
    {
        return (Input.GetMouseButtonDown(0) && GameManager.Instance.Frozen == false);
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

        Collider[] hitColliders = Physics.OverlapSphere(position, 0.2f);

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

    private void OnDisable()
    {
        punchScript.enabled = true;
    }
}
