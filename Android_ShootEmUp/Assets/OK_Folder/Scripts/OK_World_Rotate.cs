using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK_World_Rotate : MonoBehaviour {


    public GameObject mGO_Player;
    public GameObject mGO_Scene;

    public float fl_rotate;
    //public bool bl_Direction;
    
    // Use this for initialization
	void Start () {

        mGO_Player = GameObject.FindGameObjectWithTag("Player");
        mGO_Scene = GameObject.Find("Scene");

	}
	

    private void OnTriggerStay(Collider cl_trigger)
    {

        if (cl_trigger.gameObject == mGO_Player)
        {
            mGO_Scene.transform.Rotate(Vector3.right * Time.deltaTime * -fl_rotate);
        }
    }
}
