using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponBehaviour : MonoBehaviour
{
    //Data del interactable object
    [SerializeField] WeaponData weaponData;

    //Leer cuando se haga un disparo a traves del input
    private PlayerInput playerInput;

    //variables de uso del disparo
    bool shooting, readyToShoot, reloading;

    //de donde sale y hacia donde va la bala
    public Camera cam;
    public Transform attackPoint;

    public bool allowInvoke = true;

    //Mostrar munición restante
    public TextMeshProUGUI ammunitionDisplay;

    //Mostrar nombre del arma
    public TextMeshProUGUI weaponName;

    //permitir efecto del muzzle
    private bool IsMuzzleReady = true;

    //Recharge UI
    [SerializeField] private GameObject RechargeCanvas;



    private void Awake()
    {
        //Pistola totalmente cargada
        weaponData.bulletsLeft = weaponData.magazineSize;
        readyToShoot = true;
        playerInput = new PlayerInput();
    }

    //habilitar Input System
    private void OnEnable()
    {
        playerInput.Input.Enable();
    }

    private void OnDisable()
    {
        playerInput.Input.Disable();
    }


    private void ReadyToShoot()
    {
        //mostar nombre del arma
        if (weaponData.weaponName != null)
        {
            weaponName.SetText(weaponData.weaponName);
        }

        //mostar municion disponible
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(weaponData.bulletsLeft / weaponData.bulletsPerTap + " / " + weaponData.magazineSize / weaponData.bulletsPerTap);
        }

        //efecto de fogonazo
        if (weaponData.muzzleEffect != null)
        {
            if (IsMuzzleReady)
            {
                Instantiate(weaponData.muzzleEffect, attackPoint.position, Quaternion.identity);
                IsMuzzleReady = false;
            }

        }


        //El arma puede sor sostenida o debe hacer tap?
        bool isShootKeyHeld = playerInput.Input.Shoot.ReadValue<float>() > 0.1f;
        if (weaponData.allowButtonHold) shooting = isShootKeyHeld;
        else shooting = playerInput.Input.Shoot.triggered;

        //Disparar
        if(readyToShoot && shooting && !reloading && weaponData.bulletsLeft > 0)
        {
            weaponData.bulletsShot = 0;

            Shooting();
        }

        //Recargar con tecla
        if (playerInput.Input.Reload.triggered && !reloading && weaponData.bulletsLeft < weaponData.magazineSize) Reloading();

        //Recargar al disparar sin munición
        if (readyToShoot && shooting && !reloading && weaponData.bulletsLeft <= 0) Reloading();
    }

    private void Update()
    {
        ReadyToShoot(); 
    }

    private void Shooting()
    {
       

        readyToShoot = false;

        //posicion central de la camara
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //Ray cast colisiona con algo
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        } else
        {
            targetPoint = ray.GetPoint(10);
        }

        //calcular la dirección de lanzamiento entre el punto de spawneo de la bala y el objetivo
        Vector3 directionRay = targetPoint - attackPoint.position;

        //el arama tendra esparcimiento?
        float xSpread = Random.Range(-weaponData.spread, weaponData.spread);
        float ySpread = Random.Range(-weaponData.spread, weaponData.spread);

        //dirección con esparcimiento
        Vector3 directionSpread = directionRay + new Vector3(xSpread, ySpread, 0);

        //Instanciar la bala
        GameObject mainBullet = Instantiate(weaponData.bullet, attackPoint.position, Quaternion.identity);

        //rotar la bala en la dirección indicada
        mainBullet.transform.forward = directionSpread.normalized;

        //Añadir valores de fuerza
        mainBullet.GetComponent<Rigidbody>().AddForce(directionSpread * weaponData.shootForce, ForceMode.Impulse); 
        mainBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * weaponData.upwardForce, ForceMode.Impulse);

        weaponData.bulletsLeft--;
        weaponData.bulletsShot++;

        //Invocar funciones y condiciones una vez
        if(allowInvoke)
        {
            Invoke("ResetShot", weaponData.timeBetweenShooting);
            allowInvoke = false;
        }
      
    }

    //no permitir el disparo de un proyectil hasta tiempo determinado
    private void ResetShot()
    {
        IsMuzzleReady = true;
        readyToShoot = true;
        allowInvoke = true;
    }

    //recargar el arma
    private void Reloading()
    {
        reloading = true;
        Invoke("ReloadFinished", weaponData.reloadTime);

        //UI recarga
        RechargeCanvas.SetActive(true);
    }


    private void ReloadFinished()
    {
        weaponData.bulletsLeft = weaponData.magazineSize;
        RechargeCanvas.SetActive(false);
        reloading = false;
    }










}
