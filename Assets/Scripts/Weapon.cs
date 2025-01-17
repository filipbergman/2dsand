﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float fireRate = 0; // 0 if single burst weapon
    public int Damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public float effectSpawnRate = 10;
    public Transform MuzzleFlashPrefab;
    public Transform HitPrefab;

    // Handle camera shaking
    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    public string weaponShootSound = "DefaultShot";

    // Caching
    AudioManager audioManager;

    float timeToFire = 0;
    float timeToSpawnEffect = 0;
    Transform firePoint;
    
    // Start is called before the first frame update
    void Awake() {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null) {
            Debug.LogError("No firepoint was found!");
        }
    }

    private void Start() {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null) {
            Debug.LogError("No CameraShake script found on GM object.");
        }

        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("No audioManager found in scene");
        }
    }

    // Update is called once per frame
    void Update() {
        if (fireRate == 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        } else {
            if (Input.GetButton("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot() {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, (mousePosition - firePointPosition) * 100, whatToHit);
    
       
        //Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan);
        if (hit.collider != null) {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
            
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null) {
                enemy.DamageEnemy(Damage);
            }
        }
        if (Time.time >= timeToSpawnEffect) {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null) {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            } else {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1/effectSpawnRate;
        }
    }
    
    
        
    void Effect(Vector3 hitPos, Vector3 hitNormal) {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null) {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);

            // Set positions
        }

        if(hitNormal != new Vector3(9999, 9999, 9999)) {
            Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.forward, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1.0f);
        }

        Destroy(trail.gameObject, 0.04f);

        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, 1);
        Destroy(clone.gameObject, 0.02f);

        // Shake the camera
        camShake.Shake(camShakeAmount, camShakeLength);

        // Play shoot sound
        audioManager.playSound(weaponShootSound);
    }
}
