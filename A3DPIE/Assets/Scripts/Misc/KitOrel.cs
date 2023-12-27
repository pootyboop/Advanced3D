using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//manages effects on the player's spaceship like speed trails and engine noise
public class KitOrel : MonoBehaviour
{
    Vector3 lastPosition;   //where the spaceship was last frame
    public TrailRenderer[] trailRenderers;  //the trail renderers on the spaceship's blasters? boosters? whatever they're called
    public AudioSource engineRumble;    //constant engine noise around the ship

    private float trailRendererStrength = 0.06f;    //length of the trail renderers



    //starts spaceship flight effects
    public void StartTrailRenderers()
    {
        engineRumble.volume = 1.0f;
        StartCoroutine(UpdateTrailRenderers());
    }



    //randomly changes the length of the trail renderers and the pitch of the engine noise based on speed
    IEnumerator UpdateTrailRenderers()
    {
        //will continue running until the ship stops moving
        while (true)
        {
            float dist = Vector3.Distance(lastPosition, transform.position);

            //hardcoding bad i know. these values give about the pitch range i want when the ship is flying
            engineRumble.pitch = Mathf.Clamp (.8f + .5f * dist, 0.8f, 1.5f);

            //ship stopped moving
            if (dist == 0)
            {
                //low rumble as if the engine's still on
                engineRumble.volume = 0.1f;
                engineRumble.pitch = 0.3f;
                engineRumble.maxDistance = 7f;
                yield break;    //done updating the effects, so stop this coroutine
            }

            //update the trail renderers when there's substantial movement
            if (dist > 0.1f)
            {
                foreach (TrailRenderer trail in trailRenderers)
                {
                    //trail length scales by the ship's speed and a random amount to add realism
                    trail.time = UnityEngine.Random.Range(1f, 2f) * dist * trailRendererStrength;
                }
            }

            lastPosition = transform.position;  //update lastPosition for next time
            yield return new WaitForSeconds(.05f);  //instead of updating each frame, wait a second so the effect is more pronounced/staggered
        }
    }
}
