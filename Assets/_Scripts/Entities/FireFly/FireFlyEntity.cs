using GameCore.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Currently an entity, but might change as the current application doesn't use a direct rule
//Future consideration: Have timer be a part of the entity, that way static is not needed as 
//I worry it is keeping the value the same for all fireflies (though when proper animation is in this won't be needed)

public class FireFlyEntity : MutableEntity 
{
    [Range(0.1f, 2.0f)]
    public float m_ffSpeed = 0.1f;

    //Fix for size change rule in progress
    private Vector3 m_originalScale;

    void Start()
    {
        m_originalScale = this.gameObject.transform.localScale;

        //Current default. Plan to update it so it determines the default based on day/night entity
        SetState(new FireFlySleepState(this, this.gameObject));
    }

    public override void Is(string lexeme)
    {
        switch (lexeme)
        {
            case "Day":
            case "day":
                SetState(new FireFlySleepState(this, this.gameObject));
                break;

            case "night":
            case "Night":
                SetState(new FireFlyAwakeState(this, this.gameObject, m_ffSpeed));
                break;

            case "big":
            case "Big":
                SetState(new LargeFireFlyState(this, this.gameObject));
                break;
        }
    }
}
