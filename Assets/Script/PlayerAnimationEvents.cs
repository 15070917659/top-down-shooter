using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        StartCoroutine(GetVisualController());
    }

    public void ReloadIsOver()
    {
        visualController.ReturnRigWeightToOne();
    }

    public void ReturnRig()
    {
        visualController.ReturnRigWeightToOne();
        visualController.ReturnLeftHandIKWeightToOne();
    }
    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }
    private IEnumerator GetVisualController()
    {
        yield return null;
        yield return null;
        visualController = GameObject.FindGameObjectWithTag("Weapon").GetComponent<WeaponVisualController>();
    }
}
