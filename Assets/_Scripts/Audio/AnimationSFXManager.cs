using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AnimationSFXManager : MonoBehaviour
{
    public void playLeftstepSFX ()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerMovement/Player_LeftStep");
    }

    public void playRightstepSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerMovement/Player_RightStep");
    }

    public void testingSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Button_Click");
    }
}
