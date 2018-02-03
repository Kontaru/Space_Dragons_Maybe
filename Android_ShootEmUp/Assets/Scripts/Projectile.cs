using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity {

    // ----------------------------------------------------------------------
    // Variables

    public float fl_range = 30;
    public float fl_speed = 10;
    public float fl_damage = 10;
    public GameObject enemy;

    private Rigidbody2D RB_projectile;

    // ----------------------------------------------------------------------
    // Use this for initialization
    override public void Start()
    {
        base.Start();
        Destroy(gameObject, fl_range / fl_speed);
        RB_projectile = GetComponent<Rigidbody2D>();
        RB_projectile.velocity = transform.TransformDirection(fl_speed,0,0);
    } //-----

    // ----------------------------------------------------------------------
    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<Entity>().EntityType == Entity.Entities.Enemy)
            Destroy(gameObject);
    }

}//==========