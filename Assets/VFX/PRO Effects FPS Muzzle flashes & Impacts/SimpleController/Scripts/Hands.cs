using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Effects.SimpleController
{
    /// <summary>
    /// Player Hands behaviour
    /// </summary>
    public class Hands : MonoBehaviour
    {
        /// <summary>
        /// Hand weapons list.
        /// </summary>
        [Tooltip("Hand weapons list")] public Weapon[] Weapons;
        /// <summary>
        /// Player camera.
        /// </summary>
        [Tooltip("Player camera")] public Camera Cam;
        /// <summary>
        /// Index array in circle weapon selector of each weapon.
        /// </summary>
        [Tooltip("Index array in circle weapon selector of each weapon")] public int[] Indices;
        /// <summary>
        /// Weapon selector.
        /// </summary>
        [Tooltip("Weapon selector")] public WeaponSelector weaponSelector;

        float startFov;

        private int currentWeaponIndex = -1;

        void Start()
        {
            startFov = Cam.fieldOfView;
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].gameObject.SetActive(false);
            }
            weaponSelector.OnSelectedEvent += OnWeaponSelected;
        }

        private void OnWeaponSelected(int index)
        {
            for (int i = 0; i < Indices.Length; i++)
            {
                if(index == Indices[i])
                {
                    index = i;
                    break;
                }
            }
            Deploy(index);
        }

        void Update()
        {
            
            foreach (Weapon weapon in Weapons)
            {
                if (weapon.gameObject.activeSelf)
                {
                    Cam.fieldOfView = weapon.CurrentFov;
                    return;
                }
            }
            Cam.fieldOfView = startFov;
        }

        private void Deploy(int index)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (i == index)
                    continue;

                Weapons[i].gameObject.SetActive(false);
            }
            Weapons[index].gameObject.SetActive(true);
        }
    }
}