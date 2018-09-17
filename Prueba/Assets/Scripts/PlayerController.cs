using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GameObject playerCamera;

    public Text numberOfPlayers;

    static Animator anim;
	public float speed = 50.0f;
	public float rotationSpeed = 75.0f;

    [SyncVar]
    public int playersConnected;

    public void Start() 
    {
        anim = GetComponent<Animator>(); 

        if(isLocalPlayer) 
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }           
    }

    void Update()
    {

        if(isServer) {
           playersConnected = NetworkServer.connections.Count;
        }

        numberOfPlayers.text = "Players: " + playersConnected; 

        if (!isLocalPlayer)
        {
            return;
        }

        float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;

		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);

		if(translation != 0)
		{
			anim.SetInteger("PlayerActions", 1);
		}
		/* 
		if(Input.GetKey(KeyCode.UpArrow))
		{
			Debug.Log("arriba");
			anim.SetInteger("PlayerActions", 1);
		}	*/
		else
		{
			anim.SetInteger("PlayerActions", 0);
		}

/* 
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer ()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}