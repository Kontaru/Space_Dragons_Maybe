using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    //GameManager
    private GameObject GO_GM;   //GameManager game object (automatically found in inspector)
    [HideInInspector]           //Hides the below value
    public GameManager GM;      //GameManager settings (can't be set to protected due to PC referencing itself to the GM)
    protected AudioSource[] SoundClips;        //Array of audio sources

    virtual public void Start()
    {
        GO_GM = GameObject.Find("GameManager");     //Find the gameobject named "GameManager"
        GM = GO_GM.GetComponent<GameManager>();

        SoundClips = GetComponents<AudioSource>();
    }

    #region --- Entity Types Setter ---

    public enum Entities
    {
        None,
        Player,
        Enemy,
        Bullet
    }

    #endregion

    public Entities EntityType;




}
