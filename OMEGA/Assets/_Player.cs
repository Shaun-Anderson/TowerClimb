using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{
    [Header("General")]
    public float moveSpeed = 6;
    public float gravity;
    public Vector3 velocity;
    public bool isFacingRight;
    public Vector2 directionalInput;
    public float velocityXSmoothing;
    public float accelerationTimeGrounded = .05f;
    public int wallDirX;

    [Header("Jumping")]
    public bool canJump;
    public bool doubleJump;
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    [HideInInspector]public float maxJumpVelocity;
    [HideInInspector]public float minJumpVelocity;
    public float accelerationTimeAirborne = .1f;

    [Header("WallClimb/Slide")]
    public bool wallSliding;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    [Space(5)]
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    [HideInInspector]public float timeToWallUnstick;
}


[System.Serializable]
public class References
{
    [Header("General")]
    public Controller2D controller;
    public Camera pCam;
    public SpriteRenderer sRend;
    public Material normalMat;
    public Material hitMat;

    [Space(5)]
    [Header("Sound")]
    public AudioSource audio;
    public AudioClip switchWeaponSound;


    [Header("Character")]
    public Transform head;
    public Transform body;
    public Transform rotatetransform;
    public Transform headTransform;
}


[System.Serializable]
public class Weapons
{
    [Header("Current Weapon")]
    public GameObject bullet;
    public Transform bulletSpawn;
    public int damage;
    public int range;
    public float speed;
    public float firerate;
    public float shakeAmount;


    [Header("WeaponList")]
    public List<GameObject> weaponList = new List<GameObject>();
    public int currentWeapon;
    [HideInInspector]public int weaponSizeofList;
    public Transform currentWeaponOBJ;
    public Transform weaponLoc;

    [Header("Shooting")]
    public bool canFire;
    public float angle;
    public float targetVelocityX;

    [HideInInspector]public float kickback;
    [HideInInspector]public float kickbackY;
}


[System.Serializable]
public class Items
{
    [Header("Item List")]
    public List<GameObject> itemList = new List<GameObject>();
    public int currentItem;
    [HideInInspector]public int itemSizeofList;
    public Transform itemOBJ;

    [Header("Interaction")]
    public bool canInteract;
    public bool interact;
    public Transform interactObj;
}
