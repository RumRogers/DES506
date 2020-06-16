using GameCore.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MutableEntity 
{
    [Tooltip("This determines the speed of transition, with the larger the number, the faster it goes (Between 0 and 1)")]
    public float m_transitionSpeed = 0.01f;
    
    private Light m_light;

    void Start()
    {
        //Currently uses get component call, but if prefered can be changed
        m_light = GetComponent<Light>();

        if (!m_light)
            Debug.Log("<color=red> Error: </color>Missing light component for Day/Night entity");

        //Sets default state, subject to preference of designers
        SetState(new DayState(this, m_light, m_transitionSpeed)); 
    }

    public override void Is(string lexeme)
    {
        switch (lexeme) //Contains my solution for caps, happy to change to wider solution
        {
            case "day":
            case "Day":
                SetState(new DayState(this, m_light, m_transitionSpeed));
                break;

            case "night":
            case "Night":
                SetState(new NightState(this, m_light, m_transitionSpeed));
                break;

            default:
                Debug.Log("<color=yellow> INVALID RULE: </color>The submitted rule is incorrect (found in Day/Night)");
                break;
        }
       // base.Is(lexeme);
    }
}