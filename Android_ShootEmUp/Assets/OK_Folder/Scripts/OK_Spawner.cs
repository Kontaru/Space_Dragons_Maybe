using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK_Spawner : MonoBehaviour {

    public GameObject mGO_Enemy;
    public float fl_Timer;
    private float fl_clock;
    
    // Use this for initialization
	void Start () {
        fl_clock = fl_Timer;
	}
	
	// Update is called once per frame
	void Update () {

        fl_clock -= Time.deltaTime;

        if (fl_clock <= 0)
        {
            Instantiate(mGO_Enemy, transform.position, transform.rotation);
            fl_clock = fl_Timer;
        }

	}
}
