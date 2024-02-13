using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager instance;
    public VolumeProfile profile;
    private Fog fog;
    private Exposure exposure;

    public float exposureFadeInSpeed = .5f;
    private float exposureStart = 1f;
    private float exposureTarget = 2.5f;


    void Start()
    {
        instance = this;

        if (!profile.TryGet(out fog))
        {
            throw new System.NullReferenceException(nameof(fog));
        }


        if (!profile.TryGet(out exposure))
        {
            throw new System.NullReferenceException(nameof(exposure));
        }
    }



    public void SetFog(bool newActive)
    {
        fog.enabled.value = newActive;

        if (newActive)
        {
            FadeInExposureCompensation();
        }
    }



    public void FadeInExposureCompensation()
    {
        exposure.compensation.value = exposureStart;
        StartCoroutine(ExposureFade());
    }



    private IEnumerator ExposureFade()
    {
        while (exposure.compensation.value < exposureTarget)
        {
            float newAddTime = exposureFadeInSpeed * Time.deltaTime;   //new time to scale the added alpha by, scaled by speed

            exposure.compensation.value += newAddTime;

            //wait a bit then go again
            yield return null;
        }
    }
}
