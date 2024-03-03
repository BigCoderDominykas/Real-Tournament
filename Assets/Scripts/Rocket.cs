using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20;
    public int damage = 10;
    public int bounceCount = 0;
    public GameObject explosionParticles;
    public GameObject bounceParticles;
    public GameObject bulletHole;
    public AudioClip enemyHitSound;
    public AudioClip wallHitSound;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if(health != null)
        {
            health.Damage(damage);
            GetComponent<AudioSource>().PlayOneShot(enemyHitSound);
        }

        var obj = Instantiate(bulletHole, transform.position, Quaternion.identity);
        obj.transform.forward = collision.contacts[0].normal;

        if (bounceCount > 0)
        {
            transform.forward = collision.contacts[0].normal;
            bounceCount--;
            // Wall hit effect
            GetComponent<AudioSource>().PlayOneShot(wallHitSound);
            obj = Instantiate(bounceParticles, transform.position, Quaternion.identity);
            obj.transform.forward = collision.contacts[0].normal;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(wallHitSound);
            Destroy(gameObject);
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }
    }
}
