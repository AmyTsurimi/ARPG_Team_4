﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour {
    private Animator _animator;
    private const string AnimWeaponInHand = "WeaponInHand";
    private int _weaponEquipped = 1;
    void Update() { 
        if (Input.GetKey(KeyCode.Alpha1)) _weaponEquipped = 1;
        else if (Input.GetKey(KeyCode.Alpha2)) _weaponEquipped = 2;
        else if (Input.GetKey(KeyCode.Alpha3)) _weaponEquipped = 3;
        _animator.SetInteger(AnimWeaponInHand, _weaponEquipped);
    }
}
