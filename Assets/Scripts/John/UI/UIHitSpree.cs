using UnityEngine;
using TMPro;
public class UIHitSpree : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshProUGUI spreeText;

    public void UpdateHitSpree()
    {
        spreeText.text = GameManager.Instance.hitSpree + "x";
        if (GameManager.Instance.hitSpree == 0)
        {
            animator.SetTrigger("Reset");
        }
    }
}
