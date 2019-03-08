using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class LerpDeDerp : MonoBehaviour {

    [SerializeField] private float speed;

    private bool swappin = false;

    private Vector2 target;
    private Vector2 moveVector;

    private Player player;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb2D = GetComponent<Rigidbody2D>();
    }


    public void SetTarget(Vector2 newTarget)
    {
        player.enabled = false;
        rb2D.bodyType = RigidbodyType2D.Static;
        target = newTarget;
        swappin = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (swappin)
        {
            moveVector = target - (Vector2)transform.position;
            if(moveVector.magnitude < speed * Time.deltaTime)
            {
                transform.position = target;
                swappin = false;
                player.enabled = true;
                rb2D.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                transform.position += (Vector3)moveVector.normalized * speed * Time.deltaTime;
            }
        }
	}
}
