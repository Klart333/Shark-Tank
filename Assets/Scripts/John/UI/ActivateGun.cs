using UnityEngine;
public class ActivateGun : MonoBehaviour, IClickable // Activates the gun when clicked
{
    private void Start()
    {
        var cam = Camera.main;
        transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.8f, Screen.height * 0.05f, 0)) - new Vector3(0, 0, -10);
    }

    [SerializeField]
    private UIGunText gunText;
    public void OnClicked()
    {
        gunText.ActivateGun();
    }
}