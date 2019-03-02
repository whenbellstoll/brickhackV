using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlatformSpiked : Platform {

    /// <summary>
    /// Controls the Timed Spiked Platforms' animation and trap activation
    /// </summary>

    //timer fields
    [SerializeField] [Range(1, 10)] private float waitTime;
    [SerializeField] [Range(1, 10)] private float spikeTime;
    private float timer = 0;

    //animation fields
    private bool spikeOut = false;
    private Animator anim;

    //spikes fields
    private BoxCollider2D coll;

    private void Awake()
    {
        //get ref to spike collider
        coll = transform.GetChild(0).GetComponent<BoxCollider2D>();

        //find the animator
        anim = GetComponent<Animator>();
        anim.SetBool("Out", spikeOut);

        timer = waitTime;
    }

    protected override void Update()
    {
        base.Update(); //do normal platform things
       
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            //set animator
            spikeOut = !spikeOut;
            anim.SetBool("Out", spikeOut);
            //reset timer
            timer += spikeOut ? spikeTime : waitTime;
            //collider state
            coll.enabled = spikeOut;
        }
    }
}