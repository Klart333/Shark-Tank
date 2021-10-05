using System.Collections;

public class UIPlayScript : UILoadScene
{
    private AdsManager adsManager;

    private void Start()
    {
        adsManager = FindObjectOfType<AdsManager>();
        adsManager.OnAdFinished += AdsManager_OnAdFinished;
    }

    public override void OnButtonClicked()
    {
        adsManager.ShowAdd("video");
    }

    private void AdsManager_OnAdFinished(bool obj)
    {
        AfterButtonClicked();
    }
}

