using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OK_PlayerMovement : MonoBehaviour {

    // Variables
    private NavMeshAgent nma_Agent;
    private float fl_Speed;

	// Use this for initialization
	void Start () {

        // Set variable gameobjects and set a static speed variable
        nma_Agent = GetComponent<NavMeshAgent>();
        fl_Speed = nma_Agent.speed;
	}
	
	// Update is called once per frame
	void Update () {

        // The ship is constantly moving, transform.Translate works better than using a rigidbody due to jitters
        transform.Translate(Vector3.forward * Time.deltaTime * fl_Speed);

        // Player Input click raycast formed
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                // Navmesh agent moves to player clicked position
                nma_Agent.destination = hit.point;

            }
        }
	}
}
