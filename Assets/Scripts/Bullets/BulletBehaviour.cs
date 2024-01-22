using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Rigidbody rb;

    //Data del interactable object
    [SerializeField] BulletData bulletData;

    private float maxLifetime;

    [SerializeField]private int collisions;
    private PhysicMaterial physicMat;

    private GameObject explosionParticle;

    private bool explosionHappened = false;

    private float distanceToBullet;

    static float GravitationalConstant = 1;


    private void Start()
    {
        bulletSetup();

        if (bulletData.isChaotic)
        {
            bulletData.isSticky = false;
            bulletData.isMagnetic = false;
            bulletData.isExplosive = false;
        }
    }

    private void Update()
    {
        //explota cuando el numero de colisiones llegue al limite
        if (collisions > bulletData.maxCollision) {

            if (bulletData.isSticky)
            {
                rb.velocity = Vector3.zero;

                if(bulletData.isMagnetic && !bulletData.isChaotic)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(0).transform.localScale = new Vector3(bulletData.OrbitRange * 5f, bulletData.OrbitRange * 5f, bulletData.OrbitRange * 5f);
                }

                if(bulletData.isMagnetic && bulletData.isChaotic)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(0).transform.localScale = new Vector3(bulletData.effectRange / 0.5f, bulletData.effectRange / 0.5f, bulletData.effectRange / 0.5f);
                }
            }
            Explode(); 
        }

        maxLifetime = bulletData.maxLifetime;
        //cuenta el tiempo de vida de la bala
        maxLifetime -= Time.deltaTime;

        if(bulletData.maxLifetime <= 0) Explode();
    }

    private void bulletSetup()
    {
        //asignar valores de fisicas
        physicMat = new PhysicMaterial();
        physicMat.bounciness = bulletData.bounciness;
        physicMat.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMat.bounceCombine = PhysicMaterialCombine.Maximum;

        //asignar collider
        GetComponent<SphereCollider>().material = physicMat;

        //asignar gravedad
        rb.useGravity = bulletData.useGravity; 

    }

    private void Explode()
    {
        if(bulletData.explosion != null)
        {
            if(!explosionHappened)
            {
                explosionParticle = Instantiate(bulletData.explosion, transform.position, Quaternion.identity);
                explosionHappened = true;
            }

            //Mirar los objetos alrededor
            Collider[] objects = Physics.OverlapSphere(transform.position, bulletData.effectRange, bulletData.whatIsAffected);
            

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<Rigidbody>())
                {
                    //si la bala es explosiva...
                    if (bulletData.isExplosive)
                    {
                        objects[i].GetComponent<Rigidbody>().AddExplosionForce(bulletData.explosionForce, transform.position, bulletData.effectRange);
                    }

                    //si la bala es magnetica...
                    if (bulletData.isMagnetic)
                    {
    
                        //rango de orbita, debe ser siempre menor al rango de efecto
                        Collider[] orbit = Physics.OverlapSphere(transform.position, bulletData.OrbitRange, bulletData.whatIsAffected);

                        //calcular la masa de los dos elementos
                        float massProduct = rb.mass * objects[i].GetComponent<Rigidbody>().mass * GravitationalConstant;

                        //calcular la distancia de los objetos y su diferencia
                        Vector3 differenceBetweenPoints = rb.position - objects[i].GetComponent<Rigidbody>().position;
                        float distanceBetweenPoints = Vector3.Distance(rb.position , objects[i].GetComponent<Rigidbody>().position);

                        //generar la fuerza de magnitud en relacion a la distancia y agregar la fuerza magnetica
                        float unScaledForceMagnitude = massProduct / distanceBetweenPoints * distanceBetweenPoints;
                        float forceMagnitude = GravitationalConstant * unScaledForceMagnitude * bulletData.MagneticForce;

                        //agregar dirección de la fuerza
                        Vector3 forceDirection = differenceBetweenPoints.normalized;

                        //dirección de la fuerza * magnitud
                        Vector3 forceVector = forceDirection * forceMagnitude;

                        if (distanceBetweenPoints > bulletData.OrbitRange) {
                            //si esta sobre el rango de efecto pero menos que el rango de orbita debe atraer el objeto
                            objects[i].GetComponent<Rigidbody>().AddForce(forceVector);
                        } else if(!bulletData.isChaotic)
                        {
                            objects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                            objects[i].GetComponent<Rigidbody>().transform.RotateAround(transform.position, Vector3.up, bulletData.orbitSpeed * Time.deltaTime);
                        }
                    }

                    if (bulletData.isChaotic && bulletData.isMagnetic)
                    {
                        Invoke("ChaoticBullet", 1.5f);
                    }
                }
            }

            if (!bulletData.isSticky)
            {
                Invoke("DestroyBullet", 0.05f);
            }

            if (bulletData.isSticky)
            {
                Invoke("DestroyBullet", maxLifetime);
            }
        }
    }


    private void ChaoticBullet()
    {
        Explode();
        bulletData.isMagnetic = false;
        transform.GetChild(0).gameObject.SetActive(false);
        bulletData.isExplosive = true;
    }
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //contar numero de colisiones
        collisions++;

        if (bulletData.explodeOnTouch)
        {
            if (collision.gameObject.GetComponent<Rigidbody>())
            {
                Explode();
            }
        }

        if (bulletData.isChaotic)
        {
            bulletData.isSticky = true;
            bulletData.isMagnetic = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bulletData.effectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, bulletData.OrbitRange);
    }



}


