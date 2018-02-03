using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Entity {

    public GameObject[] Enemies;
    public Transform[] SpawnObjects;

    private bool BL_CanSpawn;
    private float FL_CurrentTime;

	// Use this for initialization
	override public void Start () {
        base.Start();
        FL_CurrentTime = Time.time;
        SpawnObjects = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            SpawnObjects[i] = transform.GetChild(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (BL_CanSpawn)
        {
            FL_CurrentTime = Time.time;
            int RandomEnemy = Random.Range(0, Enemies.Length - 1);
            int RandomSpawn = Random.Range(0, SpawnObjects.Length - 1);
            GameObject SpawnedObject = GameObject.Instantiate(Enemies[RandomEnemy], SpawnObjects[RandomSpawn].transform.position, SpawnObjects[RandomSpawn].transform.rotation);
            BL_CanSpawn = false;
            if (GM.DBL_CoolDown <= 0.0000001d)
                return;
            if (GM.DBL_CoolDown <= 0.1d)
                GM.DBL_CoolDown = GM.DBL_CoolDown - 0.0000000000005d;
            else if (GM.DBL_CoolDown <= 0.5d)
                GM.DBL_CoolDown = GM.DBL_CoolDown - 0.0005d;
            else if (GM.DBL_CoolDown <= 1.0d)
                GM.DBL_CoolDown = GM.DBL_CoolDown - 0.005d;
            else if (GM.DBL_CoolDown <= 2.0d)
                GM.DBL_CoolDown = GM.DBL_CoolDown - 0.05d;
            else
                GM.DBL_CoolDown = GM.DBL_CoolDown - 0.5d;
        }
        else if (Time.time > FL_CurrentTime + GM.DBL_CoolDown)
        {
            BL_CanSpawn = true;
        }
	}
}
