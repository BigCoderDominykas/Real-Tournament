using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]TMP_Text ammoText;
    [SerializeField]TMP_Text healthText;
    public GameObject grabText;

    [Header("Components")]
    public Health health;
    public Weapon weapon;
    public Transform hand;

    public LayerMask weaponLayer;

    public AnimationMod animations;

    public AudioClip playerDamageSound;
    public AudioClip weaponPickupSound;
    public AudioClip weaponDropSound;

    private void Start()
    {
        health.onDamage.AddListener(UpdateUI);
        health.onDie.AddListener(Respawn);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (weapon != null && weapon.isReloading) Invoke("UpdateUI", weapon.reloadTime);

        healthText.text = health.health + "HP";
        if (weapon != null) ammoText.text = weapon.clipAmmo + "/" + weapon.ammo;
        else ammoText.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health.Damage(10);
            AudioSystem.Play(playerDamageSound);
        }
    }

    void Respawn()
    {
        health.health = health.maxHealth;
        transform.position = Vector3.zero;
        UpdateUI();
    }

    private void Update()
    {
        var cam = Camera.main.transform;
        var collided = Physics.Raycast(cam.position, cam.forward, out var hit, 2f, weaponLayer);
        grabText.SetActive(!weapon && collided);

        if (Input.GetKeyDown(KeyCode.E))
        {               
            if(!weapon && collided)
            {
                Grab(hit.collider.gameObject);
            }
            else
            {
                Drop();
            }

            animations.weapon = weapon;
            if (weapon != null) animations.SubscribeEvents();
        }

        if (weapon == null) return;

        // Manual fire
        if (!weapon.isAutoFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.Shoot();
        }

        // Auto fire
        if (weapon.isAutoFire && Input.GetKey(KeyCode.Mouse0))
        {
            weapon.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            weapon.onRightClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R) && weapon.clipAmmo < weapon.maxAmmo)
        {
            weapon.Reload();
        }
    }

    public void Grab(GameObject gun)
    {
        if (weapon != null)
        {
            print("Already holding gun!");
            return;
        }

        weapon = gun.GetComponent<Weapon>();

        weapon.onShoot.AddListener(UpdateUI);
        weapon.onReload.AddListener(UpdateUI);

        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.transform.position = hand.position;
        weapon.transform.rotation = hand.rotation;
        weapon.transform.parent = hand;

        AudioSystem.Play(weaponPickupSound);
        UpdateUI();
    }

    public void Drop()
    {
        if (weapon == null)
        {
            print("No weapon to drop!");
            return;
        }

        weapon.onShoot.RemoveListener(UpdateUI);
        weapon.onReload.RemoveListener(UpdateUI);

        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.transform.parent = null;
        weapon = null;

        AudioSystem.Play(weaponDropSound);
        UpdateUI();
    }
}