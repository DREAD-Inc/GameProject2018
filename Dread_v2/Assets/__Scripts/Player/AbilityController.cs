using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DreadInc
{
    public class AbilityController : MonoBehaviour
    {

        public DreadInc.Weapon weapon;
        void Start()
        {
            //weapon = transform.GetChild(transform.childCount - 2).GetComponent<Weapon>(); // only works if the weapon component is the second to last child (see modelhandler)
            weapon = GetComponentInChildren<Weapon>();
        }

        // Update is called once per frame
        void Update()
        {
            //weapon = transform.GetChild(transform.childCount - 2).GetComponent<Weapon>();

            if (!weapon)
            {
                //print("weapon component not set in abilitycontroller");
                SetWeapon();
                return;
            }
            //Shoot
            if ((Input.GetButton("Fire1") /*|| (vertiArrow > 0 || horizArrow > 0)*/))
                weapon.isShooting = true;
            else weapon.isShooting = false;

            //if (!weapon) weapon = player.weaponComponent;
        }

        public void SetWeapon()
        {
            weapon = GetComponentInChildren(typeof(LaserController)) as Weapon;
            print(weapon);
            print("SetWeapon called");
            //this.weapon = weapon;
        }
    }
}
