using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon Data")]

public class WeaponData : ScriptableObject
{
    //nombre del arma
    public string weaponName;

    //bala a disparar
    public GameObject bullet;

    [Header("Valores de Fuerza")]
    //fuerza de empuje y de subida
    public float shootForce, upwardForce;

    [Header("Valores de Disparo")]
    //tiempo entre tiros, esparcimiento, tiempo de recarga, tiempo que tarda el disparo
    public float timeBetweenShots, spread, reloadTime, timeBetweenShooting;

    [Header("Munición")]
    //tamaño del cargador / Balas gastadas por click
    public int magazineSize, bulletsPerTap;

    [Header("Disparo Continuo")]
    //Puede el arma disparar si el botón/click se mantiene apretado?
    public bool allowButtonHold;

    //balas faltantes / balas disparadas
    public int bulletsLeft, bulletsShot;

    //efecto de disparo
    public GameObject muzzleEffect;
}
