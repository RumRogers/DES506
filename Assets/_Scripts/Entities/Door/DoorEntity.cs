using GameCore.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntity : MutableEntity
{
    private Vector3 m_pivotPosition;
    private float counter = 0.0f;
    void Start()
    {
        GetPivotPoint();
        SetState(new ClosedState(this));
    }

    //protected override void Update()
    //{
    //    counter += Time.deltaTime;
            
    //}

    public override void Is(string lexeme)
    {
        switch (lexeme)
        {
            case "Open":
            case "open":

                SetState(new OpenState(this, this.gameObject));
                break;

            case "Closed":
            case "closed":

                break;
        }
        base.Is(lexeme);
    }

    void GetPivotPoint()
    {
        m_pivotPosition = this.transform.position;
        float offset = this.transform.localScale.x;

        m_pivotPosition.y -= offset / 2;
    }
}
