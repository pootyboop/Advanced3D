using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitOrel : MonoBehaviour
{
    Vector3 lastPosition;
    public TrailRenderer[] trailRenderers;
    public AudioSource engineRumble;

    private float trailRendererStrength = 0.06f;



    public void StartTrailRenderers()
    {
        engineRumble.volume = 1.0f;
        StartCoroutine(UpdateTrailRenderers());
    }



    IEnumerator UpdateTrailRenderers()
    {
        while (true)
        {
            float dist = Vector3.Distance(lastPosition, transform.position);

            engineRumble.pitch = Mathf.Clamp (.8f + .5f * dist, 0.8f, 1.5f);

            if (dist == 0)
            {
                //low rumble as if still on
                engineRumble.volume = 0.1f;
                engineRumble.pitch = 0.3f;
                yield break;
            }

            if (dist > 0.1f)
            {
                for (int i = 0; i < trailRenderers.Length; i++)
                {
                    trailRenderers[i].time = UnityEngine.Random.Range(1f, 2f) * dist * trailRendererStrength;
                }
            }

            lastPosition = transform.position;
            yield return new WaitForSeconds(.05f);
        }
    }
}
