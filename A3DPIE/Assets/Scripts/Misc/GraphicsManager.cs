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

    public bool startWithFogEnabled = true;

    public float exposureFadeInTime = 3f;
    public float exposureStart = 1f;
    public float exposureTarget = 2.5f;

    public FogData defaultFogData;
    private FogData currentFogData;
    public float fogFadeTime = 3f;


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

        SetFog(startWithFogEnabled);
        currentFogData = defaultFogData;
        if (startWithFogEnabled) {
        ChangeFogAppearance(currentFogData);
        }
    }



    public void ChangeFogAppearance(FogData newFogData)
    {
        StartCoroutine(FogFade(newFogData));
    }



    private IEnumerator FogFade(FogData newFogData)
    {
        float time = 0.0f;
        while (time < fogFadeTime)
        {
            time += Time.deltaTime;
            float alpha = time / fogFadeTime;

            fog.meanFreePath.value = Mathf.Lerp(currentFogData.attenuationDistance, newFogData.attenuationDistance, alpha);
            fog.color.value = Color.Lerp(currentFogData.HDRColor, newFogData.HDRColor, alpha);

            yield return null;
        }

        fog.meanFreePath.value = newFogData.attenuationDistance;
        fog.color.value = newFogData.HDRColor;
        currentFogData = newFogData;
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
        float time = 0.0f;
        while (exposure.compensation.value < exposureTarget)
        {
            time += Time.deltaTime;
            float alpha = time / exposureFadeInTime;

            exposure.compensation.value = Mathf.Lerp(exposureStart, exposureTarget, alpha);

            //wait a bit then go again
            yield return null;
        }

        exposure.compensation.value = exposureTarget;
    }
}
