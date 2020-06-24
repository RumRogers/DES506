using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSimple : MonoBehaviour
{
    [SerializeField]
    private GameObject m_player;

    private bool m_isPlayerInRange = false;
    private const float c_viewRadius = 5.0f;
    Vector3 playerToTarget;
    Vector3 test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerToTarget = m_player.transform.position - this.transform.position;
        playerToTarget.y = 0;

        float angle = Vector3.Angle(transform.forward, playerToTarget);
        Vector3 cross = Vector3.Cross(transform.forward, playerToTarget);

        if (cross.y < 0)
            angle = -angle;

        Debug.Log(angle);
        Quaternion target = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);



        test = new Vector3(target.x, target.y, target.z);

        test = Quaternion.Euler(0, angle, 0) * transform.forward;
       
        
        if (Vector3.Distance(m_player.transform.position, transform.position) <= c_viewRadius)
        {
            test = Vector3.Lerp(transform.forward, test, 0.1f);
            
           // transform.LookAt(new Vector3(m_player.transform.position.x, transform.position.y, m_player.transform.position.z), m_player.transform.up);            
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, transform.forward * 10);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, test * 10);
    }
}
