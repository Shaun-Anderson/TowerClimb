using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class PlayerHandler : MonoBehaviour {

    [Space(5)]
    public References References;
    [Space(5)]
    public Movement Movement;
    [Space(5)]
    public Weapons Weapons;
    [Space(5)]
    public Items Items;
    [Space(5)]

    public GameObject pistol;

    //a vector that points in the middle between left and right parts of the slingshot
    private Vector3 SlingshotMiddleVector;

    public SlingshotState slingshotState;
    public GameObject tragetoryStart;
    //this linerenderer will draw the projected trajectory of the thrown bird
    public LineRenderer TrajectoryLineRenderer;

    public bool Dashing;
    public bool onWall;
    public bool hi;
    //the position of the bird tied to the slingshot
    public Transform BirdWaitPosition;

    public GameObject dmgScreen;
    public float ThrowSpeed;
    public float distance;
    public Vector3 startTouch;
    public Vector3 curTouch;

    public Vector3 heading;
    public Vector3 direction;

    public float TimeSinceThrown;

    public Animator pAni;

    public float aimAngle;

    public Text jumpText;
    public int jumps;

    public bool dmgImmune;

    public Text healthText;
    public Image healthBar;
    public float health;
    public float maxHealth;
    public int gold;
    public Text goldText;

    public GameManager GM; 

    void Start() {
        slingshotState = SlingshotState.Idle;
        References.controller = GetComponent<Controller2D> ();
        TrajectoryLineRenderer.sortingLayerName = "2";
        Movement.maxJumpVelocity = Mathf.Abs(Movement.gravity) * Movement.timeToJumpApex;
        Movement.minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (Movement.gravity) * Movement.minJumpHeight);
        References.audio = GetComponent<AudioSource>();
        UpdateHealthBar();
    }

	void Update() {
        if (_GameManager.NoMove == false)
        {
            CalculateVelocity();
            HandleWallSliding();
            //jumpText.text = jumps.ToString();
            References.controller.Move(Movement.velocity * Time.deltaTime, Movement.directionalInput);

            if (References.controller.collisions.above || References.controller.collisions.below)
            {
                if (References.controller.collisions.slidingDownMaxSlope)
                {
                    Movement.velocity.y += References.controller.collisions.slopeNormal.y * -Movement.gravity * Time.deltaTime;
                }
                else
                {
                    Movement.velocity.y = 0;
                    Movement.velocity.x = 0;
                    Dashing = false;
                    pAni.SetInteger("SwipeType", 0);
                    jumps = 3;
                }
            }

            if (!References.controller.collisions.above && !References.controller.collisions.below && !References.controller.collisions.right && !References.controller.collisions.left)
            {
                hi = false;
            }

            switch (slingshotState)
            {
                case SlingshotState.Idle:
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (jumps > 0)
                        {
                            startTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            startTouch.z = 0;
                            slingshotState = SlingshotState.UserPulling;
                        }
                    }
                    break;

                case SlingshotState.UserPulling:

                    curTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    curTouch.z = 0;
                    heading = startTouch - curTouch;
                    direction = heading / distance;

                    Vector2 diference = startTouch - curTouch;
                    float sign = (curTouch.y < startTouch.y) ? -1.0f : 1.0f;
                    aimAngle = Vector2.Angle(Vector2.right, diference) * sign;

                    Time.timeScale = 0.1f;



                    if (Input.GetMouseButton(0))
                    {
                        float tempDist;
                        distance = Vector3.Distance(startTouch, curTouch);

                        if (distance > 1f)
                        {
                            tempDist = 1f;
                        }
                        else
                        {
                            tempDist = distance;
                        }

                        if (distance > 0.2f)
                        {
                            DisplayTrajectory(tempDist);
                            if (aimAngle <= -90 && aimAngle >= -180 && !Movement.isFacingRight && !Movement.wallSliding)
                            {
                                Flip();
                            }
                            if (aimAngle >= 90 && aimAngle < 180 && !Movement.isFacingRight && !Movement.wallSliding)
                            {
                                Flip();
                            }
                            if (aimAngle <= -0 && aimAngle > -90 && Movement.isFacingRight && !Movement.wallSliding)
                            {
                                Flip();
                            }
                            if (aimAngle < 90 && aimAngle >= 0 && Movement.isFacingRight && !Movement.wallSliding)
                            {
                                Flip();
                            }
                        }
                        else
                        {
                            SetTrajectoryLineRenderesActive(false);
                        }

                        //display projected trajectory based on the distance
                    }
                    else//user has removed the tap 
                    {
                        SetTrajectoryLineRenderesActive(false);
                        TimeSinceThrown = Time.time;
                        if (distance >= 0.2f)
                        {
                            if (distance > 1f)
                            {
                                distance = 1f;
                                slingshotState = SlingshotState.BirdFlying;
                                ThrowBird(distance, aimAngle);
                            }
                            else
                            {
                                slingshotState = SlingshotState.BirdFlying;
                                ThrowBird(distance, aimAngle);
                            }
                        }
                        if (distance < 0.2f && slingshotState == SlingshotState.UserPulling)
                        {
                            Movement.velocity = Vector3.zero;
                            slingshotState = SlingshotState.Idle;
                            Dashing = false;
                        }
                        Time.timeScale = 1f;
                        Movement.gravity = -4;

                    }
                    break;
                case SlingshotState.BirdFlying:
                    break;
                default:
                    break;
            }
        }
	}

	public void SetDirectionalInput (Vector2 input) {
        Movement.directionalInput = input;
	}
		
	void HandleWallSliding() {
        Movement.wallDirX = (References.controller.collisions.left) ? -1 : 1;
        Movement.wallSliding = false;
		if ((References.controller.collisions.left || References.controller.collisions.right) && !References.controller.collisions.below)
        {
            Movement.wallSliding = true;
            pAni.SetBool("WallHold", true);
            if(hi == false)
            {
                StartCoroutine(OnWall());
                hi = true;
            }         
            jumps = 3;
        }
        else
        {
            pAni.SetBool("WallHold", false);
        }

    }
    
	void CalculateVelocity() {

        if(!Movement.wallSliding)
        {
            Movement.velocity.y += Movement.gravity * Time.deltaTime;
        }
        

        if(Movement.velocity.x > 0 && Movement.isFacingRight)
        {
            Flip();
        }
        if(Movement.velocity.x < 0 && !Movement.isFacingRight)
        {
            Flip();
        }
	}

    private void ThrowBird(float distance, float angle)
    {
        Dashing = true;
        Movement.gravity = -4;
        Movement.velocity = new Vector2(direction.x, direction.y) * ThrowSpeed * distance;
        slingshotState = SlingshotState.Idle;
        startTouch = Vector3.zero;
        curTouch = Vector3.zero;
        jumps -= 1;
        

        if (angle >= -45 && angle < 0 || angle >= 0 && angle <= 45)
        {
            pAni.SetInteger("SwipeType", 3);
        }
        if (angle <= -145 && angle > -180 || angle > 145 && angle < 180)
        {
            pAni.SetInteger("SwipeType", 3);
        }
        if(angle > 45 && angle <= 90 || angle < -45 && angle >= -90)
        {
            pAni.SetInteger("SwipeType", 2);
        }
        if(angle < -90 && angle > -145)
        {
            pAni.SetInteger("SwipeType", 2);
        }
    }

    void SetTrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }

    private void DisplayTrajectory(float distance)
    {
        SetTrajectoryLineRenderesActive(true);
        
        Vector3 v2 = direction;
        int segmentCount = 2;
        float segmentScale = 2;
        Vector2[] segments = new Vector2[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = tragetoryStart.transform.position;

        // The initial velocity
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * (ThrowSpeed*1.6f) * distance;

        float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        float time = segmentScale / segVelocity.magnitude;
        for (int i = 1; i < segmentCount; i++)
        {
            //x axis: spaceX = initialSpaceX + velocityX * time
            //y axis: spaceY = initialSpaceY + velocityY * time + 1/2 * accelerationY * time ^ 2
            //both (vector) space = initialSpace + velocity * time + 1/2 * acceleration * time ^ 2
            float time2 = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        TrajectoryLineRenderer.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }



    protected void Flip()
    {
        Movement.isFacingRight = !Movement.isFacingRight;
        Vector2 bodyScale = References.body.localScale;
        bodyScale.x *= -1;
        References.body.localScale = bodyScale;
    }

    public void Land()
    {
        Movement.doubleJump = false;
        Movement.canJump = true;
    }

    public void Hit(int dmg, float shakeAmount)
    {
        health -= dmg;
        UpdateHealthBar();
        StartCoroutine(Hit());
        StartCoroutine(Shake(shakeAmount));
    }

    public void UpdateHealthBar()
    {
        if(health <= 0)
        {
            health = 0;
            GameManager.instance.Death(gold);
        }
        float calcHealth = health / maxHealth;
        healthText.text = health + "/" + maxHealth;
        healthBar.transform.localScale = new Vector3(calcHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
    public void ChangeCoins()
    {
        goldText.text = gold.ToString("000");
    }

    public void ShakeCam(float shakeAmount)
    {
        StartCoroutine(Shake(shakeAmount));
    }

    public IEnumerator Hit()
    {
        dmgImmune = true;
        References.sRend.material = References.hitMat;
        dmgScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        dmgScreen.SetActive(false);
        References.sRend.material = References.normalMat;
        yield return new WaitForSeconds(0.2f);
        References.sRend.material = References.hitMat;
        yield return new WaitForSeconds(0.2f);
        References.sRend.material = References.normalMat;
        yield return new WaitForSeconds(0.2f);
        References.sRend.material = References.hitMat;
        yield return new WaitForSeconds(0.2f);
        References.sRend.material = References.normalMat;
        dmgImmune = false;
    }
    public IEnumerator Kickback(float kickX, float kickY)
    {
        Weapons.kickback = kickX;
        Weapons.kickbackY = kickY;
        yield return new WaitForSeconds(0.001f);
        Weapons.kickback = 0;
        Weapons.kickbackY = 0;
        yield break;
    }
    public IEnumerator FirerateWait(float firerate)
    {
        Weapons.canFire = false;
        yield return new WaitForSeconds(firerate);
        Weapons.canFire = true;
    }
    public IEnumerator Shake(float mag)
    {

        float elapsed = 0.0f;
        float duration = 0.2f;
        //float magnitude = 0.1f;
        Vector3 originalCamPos = References.pCam.transform.position;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 2.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = References.pCam.transform.position.x + Random.Range(-1f, 1f) * mag;
            float y = References.pCam.transform.position.y + Random.Range(-1f, 1f) * mag;

            References.pCam.transform.position = new Vector3(x, y, originalCamPos.z);

            yield return null;
        }

        //Camera.main.transform.position = Vector3.Lerp(Camera.main) originalCamPos;
    }

    public IEnumerator OnWall()
    {
            onWall = true;
        Movement.velocity.y = 0;
        Movement.velocity.x = 0;
        yield return new WaitForSeconds(0.01f);
        onWall = false;
        Dashing = false;
    }

}