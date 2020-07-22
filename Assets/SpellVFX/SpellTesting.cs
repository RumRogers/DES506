using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTesting : MonoBehaviour
{
    [SerializeField]
    private RFX4_EffectEvent eventHandler;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            eventHandler.ActivateEffect();
    }
}
