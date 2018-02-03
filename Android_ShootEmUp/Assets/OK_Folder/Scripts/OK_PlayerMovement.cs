using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OK_PlayerMovement : MonoBehaviour {

    private NavMeshAgent nma_Agent;
    private float fl_Speed;
    //private Rigidbody rb_Player;
    private GameObject mGO_Centre;

	// Use this for initialization
	void Start () {

        nma_Agent = GetComponent<NavMeshAgent>();
        fl_Speed = nma_Agent.speed;
        //rb_Player = GetComponent<Rigidbody>();
        mGO_Centre = GameObject.Find("Center");
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector3.forward * Time.deltaTime * fl_Speed);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {

                nma_Agent.destination = hit.point;
                //nma_Agent.nextPosition = mGO_Centre.transform.position;

                

            }

        }

	}
}
