using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Character : Entity
{

    [Header("Movement Params")]
    public float FL_FTL_MoveRate;
    public float FL_MoveRate;
    public float FL_DecelerationRate;
    public float Speed;
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 targetDir = GM.GO_Player.transform.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180);

        transform.Translate(Vector3.up * Time.deltaTime * Speed);

        if (GM.BL_KillAll)
        {
            GM.CurrentPlayer.Score += 87;
            Destroy(gameObject);
        }

        #region -- FTL ACTIVISION --
        if (GM.BL_FTL)
        {
            if (Speed > FL_FTL_MoveRate)
                Speed -= FL_DecelerationRate;
            else
                Speed = FL_FTL_MoveRate;
        }
        else
        {
            Speed = FL_MoveRate;
        }
        #endregion
    }

    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.GetComponent<Entity>().EntityType == Entity.Entities.Player)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            GM.BL_KillAll = true;
        }
        else if (coll.GetComponent<Entity>().EntityType == Entity.Entities.Bullet)
        {
            FindObjectOfType<AudioManager>().Play("Enemy Death");
            Destroy(gameObject);
        }

        GM.CurrentPlayer.Score += 100;

    }
}
