using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.iOS;

public class WeaponVisualController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform[] gunTransforms;//所有枪的Teansform

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;
    [SerializeField] private float rigIncreaseStep;
    [SerializeField] private float leftHandIKIncreaseStep;
    private bool rigShouldBeIncrease;
    private bool shouldIncreaseLeftHandIKWeight;
    private bool busyGrabbingWeapon;

    private Transform leftHand;
    private Transform currentGun;
    private Rig rig;
    private TwoBoneIKConstraint leftHandIK;
    


    private void Awake()
    {
        leftHand = GameObject.FindGameObjectWithTag("LeftHand_IK_target").transform;
        rig = GameObject.FindGameObjectWithTag("Rig").GetComponent<Rig>();
        leftHandIK = rig.transform.Find("LeftHand_IK").GetComponent<TwoBoneIKConstraint>();
    }
    private void Start()
    {
        SwitchOn(pistol);
        
        anim = GetComponentInParent<Animator>();
        SwitchAnimationLayer(1);
    }

    private void Update()
    {
        CheckWeaponSwitch();
        AttachLeftHand();

        if (Input.GetKeyDown(KeyCode.R) && !busyGrabbingWeapon)
        {
            PauseRig();
            anim.SetTrigger("Reload");
        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }
    /// <summary>
    /// 更新左手IK权重
    /// </summary>
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKIncreaseStep * Time.deltaTime;
            if (leftHandIK.weight >= 1)
                shouldIncreaseLeftHandIKWeight = false;
        }
    }

    /// <summary>
    /// 更新Rig权重
    /// </summary>
    private void UpdateRigWeight()
    {
        if (rigShouldBeIncrease)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;
            if (rig.weight >= 1)
                rigShouldBeIncrease = false;
        }
    }

   
    /// <summary>
    /// 播放换枪动画
    /// </summary>
    /// <param name="grabType"></param>
    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        rigShouldBeIncrease = false;
        shouldIncreaseLeftHandIKWeight = false;

        leftHandIK.weight = 0.15f;
        PauseRig();
        anim.SetFloat("WeaponGrabType", ((float)grabType));
        anim.SetTrigger("WeaponGrab");
        SetBusyGrabbingWeaponTo(true);
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    /// <summary>
    /// 设置正在换枪
    /// </summary>
    /// <param name="busy"></param>
    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        busyGrabbingWeapon = busy;
        anim.SetBool("BusyGrabbingWeapon", busyGrabbingWeapon);
    }

    /// <summary>
    /// 让Rig权重归1
    /// </summary>
    public void ReturnRigWeightToOne() => rigShouldBeIncrease = true;

    /// <summary>
    /// 让左手权重归1
    /// </summary>
    public void ReturnLeftHandIKWeightToOne() => shouldIncreaseLeftHandIKWeight = true;


    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
    /// <summary>
    /// 启用传进来的枪
    /// </summary>
    /// <param name="gunTransform"></param>
    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;
    }
    /// <summary>
    /// 禁用所有枪
    /// </summary>
    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 连接左手
    /// </summary>
    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetsform>().transform;

        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
    }

    /// <summary>
    /// 选择动画层
    /// </summary>
    /// <param name="layerIndex"></param>
    private void SwitchAnimationLayer(int layerIndex)
    {
        for(int i = 1; i< anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);

    }


}

public enum GrabType
{
    SideGrab,
    BackGrab
};

