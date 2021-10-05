using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class UILoadScene : MonoBehaviour
{
    [Header("Pick One")]
    [SerializeField]
    private string sceneToLoadName = "";
    [SerializeField]
    public int sceneToLoadIndex = 0;

    [SerializeField]
    private SimpleAudioEvent simpleAudioEvent;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void OnButtonClicked()
    {
        AfterButtonClicked();
    }

    protected void AfterButtonClicked()
    {
        if (simpleAudioEvent != null)
        {
            simpleAudioEvent.Play(audioSource);
        }

        FindObjectOfType<FadePanel>().StartCoroutine(FindObjectOfType<FadePanel>().FadeOut(this));
    }

    public void ChangeScene()
    {
        if (string.IsNullOrEmpty(sceneToLoadName))
        {
            SceneManager.LoadScene(sceneToLoadIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoadName);
        }
    }
}
