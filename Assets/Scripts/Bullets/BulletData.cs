using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Bullet Data")]

public class BulletData : ScriptableObject
{
    public GameObject explosion;
    public LayerMask whatIsAffected;

    [Header("Fisicas")]
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //la bala explota?
    [Header("¿Es Explosiva?")]
    public bool isExplosive = false;

    //la bala se queda en una superficie?
    [Header("¿Es Pegadiza?")]
    public bool isSticky = false;

    //la bala atrae?
    [Header("¿Es Magnetica?")]
    public bool isMagnetic = false;

    //es caotica?
    [Header("¿Es Caotica?")]
    public bool isChaotic = false;


    //Rango de efecto
    public float effectRange;
    [Header("Datos en caso de que sea explosiva")]
    //fuerza de explosion
    public float explosionForce;


    //fuerza de magnetismo / Rango de la orbita / Fuerza de orbita
    [Header("Datos en caso de que sea magnetica")]
    public float MagneticForce;
    public float OrbitRange;
    public float orbitSpeed;


    //Tiempo de vida
    [Header("Tiempo de vida")]
    public int maxCollision;
    public float maxLifetime;
    public bool explodeOnTouch = true;
}
