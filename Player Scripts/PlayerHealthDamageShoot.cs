using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDamageShoot : MonoBehaviour {

    [SerializeField]
    private Transform playerBullet;

    private float distanceBetweenNewPlatforms = 120.0f;

    private LevelGenerator levelGenerator;
    private LevelGeneratorPooling levelGeneratorPool;

	// Use this for initialization
	void Awake () {
        levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR_OBJ).GetComponent<LevelGenerator>();
        levelGeneratorPool = GameObject.Find(Tags.LEVEL_GENERATOR_OBJ).GetComponent<LevelGeneratorPooling>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate() {
        Fire();
    }

    void Fire() {
        if (Input.GetKeyDown(KeyCode.K)) {
            Vector3 bulletPos = transform.position;
            bulletPos.y += 1.5f;
            bulletPos.x += 1f;
            Transform newBullet = (Transform)Instantiate(playerBullet, bulletPos, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }
    }


    void OnTriggerEnter(Collider target){

        if (target.tag == Tags.MONSTER_BULLET_TAG || target.tag == Tags.BOUNDS_TAG) {
            // inform game controller that player died
            Destroy(gameObject);
        }

        if (target.tag == Tags.HEALTH_TAG) {
            // inform game controller that player got health
            target.gameObject.SetActive(false);
        }

        if (target.tag == Tags.MORE_PLATFORMS_TAG) {
            Vector3 temp = target.transform.position;
            temp.x += distanceBetweenNewPlatforms;
            target.transform.position = temp;
            //levelGenerator.GenerateLevel(false);
            levelGeneratorPool.PoolingPlatforms();
        }
    }

    void OnCollisionEnter(Collision target) {
        if (target.gameObject.tag == Tags.MONSTER_TAG) {
            // inform game controller that player died
            Destroy(gameObject);
        }
    }

}//class
