using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Items
{
    public class FirearmsItem : BaseItem
    {
        public enum FirearmsType
        {
            AssaultRifle,
            HandGun
        }

        public FirearmsType currentFirearmsType;
        public string armsName;
    }

}

