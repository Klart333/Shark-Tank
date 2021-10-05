using UnityEngine;
using TMPro;

public class UIGunText : MonoBehaviour
{
    [SerializeField]
    private GunScript gunPrefab;

    [SerializeField]
    private ActivateGun gunObject;

    private TextMeshProUGUI activateGunText;
    private Animator animator;

    private bool gunActivatible;

    private int gunHitSpreeRequirement = 20;
    private float gunCooldown = 20f;
    private float coolDownTimer;

    private void Start()
    {
        activateGunText = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponentInChildren<Animator>();

        GameManager.Instance.OnSharkKilled += TryActivateGun; // Everytime we kill a shark we check if the gun can be activated
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;
    }

    private void TryActivateGun(float timeToKill = 0)
    {
        if (GameManager.Instance.hitSpree >= gunHitSpreeRequirement && coolDownTimer >= gunCooldown) // If the hitspree and the timer are good we make the gun activatable
        {
            GunActivatible();
        }
        else // Otherwise it's not
        {
            GunNotActivatible();
        }
    }

    private void GunActivatible()
    {
        animator.ResetTrigger("Reset"); // Incase the trigger was active, it needs to get reset, otherwise it will immediately go back 
        animator.SetTrigger("Breath");

        gunObject.gameObject.SetActive(true);
        activateGunText.text = "Press To\nActivate GUN!";
        gunActivatible = true;
    }

    private void GunNotActivatible() // Same but false
    {
        animator.ResetTrigger("Breath");
        animator.SetTrigger("Reset");

        gunObject.gameObject.SetActive(false);
        activateGunText.text = "";
        gunActivatible = false;
    }

    public void ActivateGun()
    {
        coolDownTimer = 0;

        gunPrefab.Get<GunScript>(); // Gets a gun from the pool
        PowerupManager.Instance.gunActive = true; 

        TryActivateGun(); // We want to deactivate the Gun UI when we activate the gun 
    }
}
