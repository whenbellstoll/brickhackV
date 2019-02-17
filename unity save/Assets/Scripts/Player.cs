using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] int controllerNumber;
    string horizontal;
    string jump;

    public Animator animation;
    public Rigidbody2D myBody;              //players rigidBody
    public BoxCollider2D box;               // Player's box collider
    public SpriteRenderer sprite;           // The player sprite
    public RaycastHit2D ground;
    public Collision2D botCollision;                  // Stores a vertical collision
    public Collision2D test = new Collision2D();
    public Vector2 position;                //the players current location
    public Vector2 tempPosition;            // Temporary position for hit detection
    public Vector3 velocity;                //the players velocity added to position
    public Vector3 acceleration;            //the players acceleration added to velocity
    public Vector3 mousePos;                //locates where the mouse is
    public Vector3 playerToMouse;           //draws a line between the player and the mouse position
    public Vector3 scaleBy;
    public Vector3 normalScale;

    public float mass;                      //the mass of a player
    public float maxAcceleration;             //the maximum acceleration of a player
    public float maxVelocity;                 //the maximum speed of a player
    public float rollSpeed;                 // This is the speed of a player while rolling.  Higher than regular.
    public float normalSpeed;               // This is the normal speed
    public float fallSpeed;                 //rate of downards acceleration
    public float jumpSpeed;                 //jump height
    public float timer;
    public float distToGround;              //Distance from the center of the sprite to the ground
    public float offset;                    // This is so the raycast never hits its own collider
    public float angle;


    public int baseHealth;                  //the total amount of health the player has
    public int currentHealth;               //the current remaining health the player has
    

    public bool falling;                    //boolean of whether the player is currently falling
    public bool rolling;                    // Boolean of whether the player is currently rolling
    public bool facingLeft;
    public bool reloading;
    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        horizontal = "C" + controllerNumber + "Horizontal";
        jump = "C" + controllerNumber + "Jump";
    }
	// Use this for initialization
	void Start ()
    {
        position = transform.position;
        myBody = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        // box = gameObject.GetComponent<BoxCollider2D>();
        scaleBy = new Vector3(1, 1, 1);
        normalScale = transform.localScale;
        // distToGround = sprite.bounds.extents.y;
        // offset = distToGround + .01f;
	}
	
	// Update is called once per frame
	void Update ()
    {

        acceleration = Vector3.zero;


        angle = Mathf.Atan2(playerToMouse.y, playerToMouse.x);
        //Set Animation
        animation.SetFloat("xVelocity", velocity.x);
        animation.SetFloat("yVelocity", velocity.y);
        animation.SetBool("grounded", !falling);

        if (currentHealth <= 0)
        {
           
        }

        
        // moving right
        if (Input.GetAxis(horizontal) > 0.35f)
        {
            ApplyForce(new Vector2(maxVelocity, 0));
            facingLeft = false;
        }

        // moving left
        if (Input.GetAxis(horizontal) < -0.35f)
        {
            ApplyForce(new Vector2(-maxVelocity, 0));
            facingLeft = true;
        }
        else
        {
            if (horizontal == "C1Horizontal")
            {
                if (Input.GetKey(KeyCode.D))
                {
                    ApplyForce(new Vector2(maxVelocity, 0));
                    facingLeft = false;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    ApplyForce(new Vector2(-maxVelocity, 0));
                    facingLeft = true;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    ApplyForce(new Vector2(maxVelocity, 0));
                    facingLeft = false;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    ApplyForce(new Vector2(-maxVelocity, 0));
                    facingLeft = true;
                }
            }
        }



        if (rolling)
        {
            //transform.localScale = new Vector3(1, 1, 1);
           
        }
        
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
           
        }
        else
        {
           
        }


        if (facingLeft)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //jump
        if (Input.GetAxis(jump) > 0 && !falling)
        {
            ApplyForce(new Vector2(0, jumpSpeed));
            // falling = true;
        }
        else 
        {
            if (jump == "C1Jump")
            {
                if(Input.GetKeyDown(KeyCode.W) && !falling)
                {
                    ApplyForce(new Vector2(0, jumpSpeed));
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.UpArrow) && !falling)
                {
                    ApplyForce(new Vector2(0, jumpSpeed));
                }
            }

        }

            // Checks if grounded
            ground = IsGrounded();
        
        // If the player is above a certain point, start fallign
        if (ground.distance >= .1f || ground.collider == null)
        {
            falling = true;
        }
        // Otherwise, the player is grounded, so stop falling and set
        // vertical velocity to zero
        else
        {
           falling = false;
            //animation
            velocity.y = 0;
        }

        // Falling
        if(falling)
        {
            ApplyForce(new Vector2(0, fallSpeed));
        }
        
        // Roll
        if (rolling)
        {
            // Change max velocity so you move faster
            maxVelocity = rollSpeed;
        }
        else
        {
            
            maxVelocity = normalSpeed;
        }



       Movement();
        

        velocity.x = velocity.x / 2;



        // falling = true;
        //move player after calculations
        

    }

    /// <summary>
    /// Method to check if player is grounded??? Doesn't work, but it might
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D IsGrounded()
    {

        LayerMask mask = LayerMask.GetMask("Solid");
        tempPosition = transform.position;

        

        // tempPosition.y -= offset;
        // Debug.Log("ORIGIN: " + tempPosition);


        tempPosition.x -= sprite.bounds.extents.x - .525f;
        tempPosition.y -= sprite.bounds.extents.y;

        if (facingLeft)
        {
            tempPosition.x += .28f;
        }

        RaycastHit2D hit1 = Physics2D.Raycast(tempPosition,
            //transform.position - sprite.bounds.extents,
            //transform.position + new Vector3(0,-1,0),
            -Vector2.up,
            distToGround + 5f,
            mask);



        Debug.DrawLine(tempPosition, transform.position + new Vector3(0, -1, 0) * 3);

        tempPosition = transform.position;

        tempPosition.x += sprite.bounds.extents.x - .825f;
        tempPosition.y -= sprite.bounds.extents.y;


        if (facingLeft)
        {
            tempPosition.x += .28f;
        }


        RaycastHit2D hit2 = Physics2D.Raycast(tempPosition,
            //transform.position + new Vector3(0,-1,0),
            -Vector2.up,
            distToGround + 5f,
            mask);
        
        Debug.DrawLine(tempPosition, transform.position + new Vector3 (0,-1,0) * 3);
        // Debug.DrawLine(transform.position - sprite.bounds.extents, transform.position + new Vector3(0, -1, 0) * 3);

        // Sends a ray out on either side of the object.  Checks which one is closer to the ground
        // If they are both of equal(ish) distance, then you can fall off a platform
        if ((hit2.distance < hit1.distance && hit2.collider != null) || (hit1.collider == null && hit2.collider != null))
        {
            //Debug.Log("return hit2 during check");
            return hit2;
        }
        else if ((hit1.distance < hit2.distance && hit1.collider != null)|| (hit2.collider == null && hit1.collider != null))
        {
            //Debug.Log("return hit1 during check");
            return hit1;
        }
        

        // Debug.Log("return hit1 after check");
        return hit1;



    }
    /// <summary>
    /// AppkyForce Method accelerates based on force
    /// </summary>
    /// <param name="force"></param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// Movement method calculates change in position based on factors
    /// </summary>
    public void Movement()
    {
        Vector3.ClampMagnitude(acceleration, maxAcceleration);
        velocity += acceleration;
        if(velocity.x > maxVelocity)
        {
            velocity.x = maxVelocity;
        }
        else if (velocity.x < - maxVelocity)
        {
            velocity.x = -maxVelocity;
        }

        if (falling)
        {

            if (transform.position.y + velocity.y < ground.point.y + .1f)
            {
                myBody.MovePosition(new Vector2(transform.position.x, ground.point.y + .1f) 
                    // * Time.deltaTime
                    );
            }
        }
        myBody.MovePosition(transform.position + velocity 
            * Time.deltaTime
            );
        
    }

    /// <summary>
    /// This executes a roll instead of normal movement
    /// </summary>
    public void Roll()
    {
        if (!facingLeft)
        {
            velocity.x = rollSpeed;
        }

        else if (facingLeft)
        {
            velocity.x = -rollSpeed;
        }
        if (falling)
        {
            velocity.y = 0.8f * jumpSpeed;
        }

        myBody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    /// <summary>
    /// Calculates the vector between the player's position and the mouse position
    /// Normalizes it and returns it as a direction vector
    /// </summary>

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (falling)
    //    {
    //        falling = false;
    //        velocity.y = 0;
    //        botCollision = collision;
    //    }
    //}
    

}
