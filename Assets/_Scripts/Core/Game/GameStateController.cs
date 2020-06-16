using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : GameCore.System.Automaton
{
    void Awake()
    {
        SetState(new Playing_State(this));
    }

    override protected void Update()
    {
        m_state.Manage();
    }
}
