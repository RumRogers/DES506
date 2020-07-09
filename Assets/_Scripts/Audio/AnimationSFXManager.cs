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

    public void playJumpSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerMovement/Player_Jump");
    }

    public void playLandSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerMovement/Player_Land");
    }
}
