using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AnimationSFXManager : MonoBehaviour
{
    //Player Movement

    public void playLeftstepSFX ()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Player_LeftStep");
    }

    public void playRightstepSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Player_RightStep");
    }

    public void playJumpSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Player_Jump");
    }

    public void playLandSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Player_Land");
    }

    public void playRespawnRecover()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Respawn/Respawn_Recover");
    }

    public void playRespawnGetup()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PLAYER/MOVEMENT/Respawn/Respawn_GetUp");
    }

    //Player Spellcasting
    public void spellcastSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SPELLS/Spell_Cast");
    }
}
