using UnityEngine;
public class ActivateGun : MonoBehaviour, IClickable // Activates the gun when clicked
{
    [SerializeField]
    private UIGunText gunText;
    public void OnClicked()
    {
        gunText.ActivateGun();
    }
}