using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public GameObject monsterDiedEffect;
    public Transform bullet;
    public float distanceFromPlayerToMove = 20f;
    public float movementSpeedMin = 1f;
    public float movementSpeedMax = 2f;

    private bool moveRight;
    private float movementSpeed;
    private bool isPlayerInRange;

    private string FUNCTION_TO_INVOKE = "StartShooting";

    private Transform playerTransform;

    public bool canShoot;

    // Use this for initialization
    void Start () {
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            moveRight = true;
        }
        else {
            moveRight = false;
        }

        movementSpeed = Random.Range(movementSpeedMin, movementSpeedMax);

        playerTransform = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerTransform) {
            float distanceFromP = (playerTransform.position - transform.position).magnitude;
            if (distanceFromP < distanceFromPlayerToMove)
            {
                if (moveRight)
                {
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * movementSpeed,
                        transform.position.y, transform.position.z);
                }
                else
                {
                    Vector3 temp = transform.position;
                    temp.x -= Time.deltaTime * movementSpeed;
                    transform.position = temp;
                }

                if (!isPlayerInRange)
                {
                    if (canShoot)
                    {
                        InvokeRepeating(FUNCTION_TO_INVOKE, 0.5f, 1.5f);
                    }
                    isPlayerInRange = true;
                }

            }
            else {
                CancelInvoke(FUNCTION_TO_INVOKE);
            }
        }

    }

    void StartShooting() {
        if (playerTransform) {
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1.5f;
            bulletPos.x -= 1f;
            Transform newBullet = (Transform)Instantiate(bullet, bulletPos, Quaternion.identity);
            newBullet.GetComponent<Rigidbody> ().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }
    }

    void MonsterDied() {
        Vector3 effectPos = transform.position;
        effectPos.y += 2f;
        Instantiate(monsterDiedEffect, effectPos, Quaternion.identity);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider target) {
        if (target.tag == Tags.PLAYER_BULLET_TAG) {
            MonsterDied();
        }        
    }

    void OnCollisionEnter(Collision target) {
        if (target.gameObject.tag == Tags.PLAYER_TAG) {
            MonsterDied();
        }
    }

}//class

