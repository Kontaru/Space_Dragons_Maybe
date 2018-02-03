using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : Entity
{
    [Header("Movement Params")]
    private float moveSpeed = 0;
    public float FL_MinMoveSpeed;
    public float FL_MaxMoveSpeed;
    public float FL_AccelerationRate;
    public float FL_DecelerationRate;
    public float rotationRate;

    [Header("Firing Params")]
    public Transform projectilespawn;
    public float fl_cool_down = 1;
    private float fl_delay;

    private bool CanFTL = true;
    private float FTL_Cooldown = 2.0f;
    private float FTL_Time;
    public float FL_ZoomCounter = 0f;

    // Use this for initialization
    override public void Start()
    {
        base.Start();
        GM.GO_Player = gameObject;
        FindObjectOfType<AudioManager>().Play("Throttle");
    }

    // Update is called once per frame
    void Update()
    {   
        #region --- MOVEMENT ---
        transform.Translate(0, moveSpeed * Time.deltaTime, 0, gameObject.transform);

        //Left and right
        if (Input.GetKey(GM.KC_TurnLeft))
        {
            transform.Rotate(0, 0, rotationRate * Time.deltaTime);
        }
        if (Input.GetKey(GM.KC_TurnRight))
        {
            transform.Rotate(0, 0, -rotationRate * Time.deltaTime);
        }

        //Dethrottle
        if (Input.GetKey(GM.KC_Dethrottle))
        {
            if (moveSpeed > FL_MinMoveSpeed)
                moveSpeed -= FL_DecelerationRate;
            else
                moveSpeed = FL_MinMoveSpeed;
        }
        else
        {
            if (moveSpeed < FL_MaxMoveSpeed)
                moveSpeed += FL_AccelerationRate;
            else
                moveSpeed = FL_MaxMoveSpeed;
        }
        #endregion

        #region --- SHOOT ---
        if (Input.GetKey(GM.KC_Shoot) && Time.time > fl_delay)
        {
            Shoot();
        }


        #endregion

        #region --- FTL ACTIVISION ---
        Camera.main.orthographicSize = Mathf.Lerp(5.0f, 6.0f, FL_ZoomCounter);

        if (Input.GetKey(GM.KC_FTL) && CanFTL)
        {
            if (FL_ZoomCounter <= 1.0f)
                FL_ZoomCounter += 0.5f * Time.deltaTime;

            if (GM.FL_FTLbar > 0)                               //Make sure we have FTL juice
            {
                GM.BL_FTL = true;                               //Tell the GameManager we're activating FTL
                GM.FL_FTLbar -= 0.1f;                           //Deplete FTL juice
            }else
            {
                CanFTL = false;
                FTL_Time = Time.time;
            }
        }else if (Input.GetKeyUp(GM.KC_FTL) && CanFTL)
        {
            CanFTL = false;
            FTL_Time = Time.time;
        }
        else
        {
            if (FL_ZoomCounter >= 0.0f)
                FL_ZoomCounter -= 0.5f * Time.deltaTime;

            GM.BL_FTL = false;                                  //Tell the GameManager we're DEactivating FTL
            if (GM.FL_FTLbar < 20.0f) GM.FL_FTLbar += 0.1f;    //Replenish FTL juice
        }

        if (CanFTL == false)
        {
            if (Time.time > FTL_Time + FTL_Cooldown) CanFTL = true;
        }


        if (GM.BL_FTL)
            FindObjectOfType<AudioManager>().Play("FTL_Start");
        else
            FindObjectOfType<AudioManager>().Play("FTL_Stop");
        #endregion



    }

    void Shoot()
    {
        fl_delay = Time.time + fl_cool_down;
        Instantiate(GM.PF_Bullet, projectilespawn.position, projectilespawn.rotation);
        Instantiate(GM.PF_Bullet, projectilespawn.position, projectilespawn.rotation * Quaternion.Euler(0, 90, 0));
        Instantiate(GM.PF_Bullet, projectilespawn.position, projectilespawn.rotation * Quaternion.Euler(0, -90, 0));
        FindObjectOfType<AudioManager>().Play("Shoot");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.GetComponent<Entity>().EntityType == Entity.Entities.Enemy)
        {
            GM.Player_HP--;
            GM.RespawnPlayer();
            //Destroy(gameObject);
        }
    }
}
 