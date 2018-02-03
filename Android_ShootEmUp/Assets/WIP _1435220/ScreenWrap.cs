using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{

    public bool WrapX;
    public bool WrapY;

    float tHeight;
    float tWidth;

    //Rigidbody2D	mRB; 
    //if we're going to use RigidBody, then rename all "transform" to "mRB", and "Vector3" to "Vector2"

    void Awake()
    {
        //		mRB = GetComponent<Rigidbody2D> ();					//We are going to use the rigidbody to reposition our GO
        tHeight = Camera.main.orthographicSize;		//We get the size of the viewable space here, starting with Height
        tWidth = Camera.main.aspect * tHeight;			//Once we have the Height we can calculate the width using the aspect ratio
    }

    void Update()
    {
        if (WrapX)
        {
            if (transform.position.x >= tWidth)
            {                       //Check Width
                transform.position += Vector3.left * tWidth * 2;      //This is a little trick to move us 2 x Width to the left
            }
            else if (transform.position.x <= -tWidth)
            {               //Using an else here as we cant be both off the right & left
                transform.position += Vector3.right * tWidth * 2;
            }
        }

        if (WrapY)
        {
            if (transform.position.y >= tHeight)
            {                       //Same for height
                transform.position += Vector3.down * tHeight * 2;
            }
            else if (transform.position.y <= -tHeight)
            {
                transform.position += Vector3.up * tHeight * 2;
            }
        }
    }
}
