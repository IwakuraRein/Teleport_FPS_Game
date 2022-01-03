using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Items
{
    public abstract class BaseItem : MonoBehaviour
    {
        public enum ItemType
        {
            Firearms,
            Others
        }

        public ItemType currentItemType;
        public int itemId;
    }

}