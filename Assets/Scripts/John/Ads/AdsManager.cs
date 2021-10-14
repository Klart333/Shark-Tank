using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public event Action<bool> OnAdFinished = delegate { };

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("3975843", false);
    }

    public void ShowAdd(string placementID)
    {
        StartCoroutine(ShowAnAd(placementID));
    }

    private IEnumerator ShowAnAd(string placementID)
    {
        while (!Advertisement.IsReady(placementID))
        {
            yield return null;
        }

        Advertisement.Show(placementID);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            OnAdFinished(true);

            var rev = FindObjectOfType<UIRevive>();
            if (rev != null)
                rev.WatchedAd(true);
        }
        else
        {
            OnAdFinished(false);

            var rev = FindObjectOfType<UIRevive>();
            if (rev != null)
                rev.WatchedAd(false);
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }
}
