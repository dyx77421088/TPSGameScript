using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.Bags
{
    public class SlotItem : MonoBehaviour
    {
        private Text text;
        public int capacity;
        void Start()
        {
            text = GetComponent<Text>();
        }


        void Update()
        {

        }
    }
}
