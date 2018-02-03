using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OK_EnemyBehaviour : MonoBehaviour {

    // Variables created
    private GameObject GO_Player;
    private NavMeshAgent nma_Agent;
    private float fl_DistanceFire;
    private float fl_dis;
    private float fl_time;

    public float Fire_Rate;
    public GameObject Bullet;
    public float Health;

    // Use this for initialization
    void Start () {


        //Gameobject and component variables set
        GO_Player = GameObject.FindGameObjectWithTag("Player");
        nma_Agent = gameObject.GetComponent<NavMeshAgent>();
        //Variables controlling firing
        fl_DistanceFire = nma_Agent.stoppingDistance;
    }
	
	// Update is called once per frame
	void Update ()
    {
        NavMove();
	}

    // This method controls movement
    void NavMove()
    {
        // Agent moves towards player and checks distance
        nma_Agent.destination = GO_Player.transform.position;
        fl_dis = Vector3.Distance(GO_Player.transform.position, transform.position);

        // If the agent is close enough they will fire
        if (fl_dis <= fl_DistanceFire)
        {
            ProjectileFire();
        }

    }

    // This method controls firing
    void ProjectileFire()
    {
        
        fl_time -= Time.deltaTime;
        if (fl_time <= 0)
        {
            Instantiate(Bullet, transform.position, transform.rotation);
            fl_time = Fire_Rate;
        }
    }

    // This method controls the incoming damage
    void EnHealth(float fl_damage)
    {

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
