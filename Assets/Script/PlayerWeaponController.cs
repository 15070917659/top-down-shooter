using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    public Transform weaponTransform;
    public GameObject weappon;
    void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(InstantiateWeapon());
        player.controls.Player.Fire.performed += context => Shoot();
    }

    void Update()
    {
       
    }


    IEnumerator InstantiateWeapon()
    {
        //yield return new WaitForSeconds(0.1f);
        yield return null;
        Instantiate(weappon, weaponTransform);
    }


    private void Shoot()
    {
       GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}
