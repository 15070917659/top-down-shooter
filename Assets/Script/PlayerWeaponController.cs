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

    /// <summary>
    /// ÊµÀý»¯ÎäÆ÷
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateWeapon()
    {
        yield return null;//ÑÓ³ÙÒ»Ö¡
        Instantiate(weappon, weaponTransform);
    }

    /// <summary>
    /// Éä»÷
    /// </summary>
    private void Shoot()
    {
       GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}
