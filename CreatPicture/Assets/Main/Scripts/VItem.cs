using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GD
{

    public class VItem : MonoBehaviour
    {

        Text text;
        private void Start()
        {
            text = transform.GetComponentInChildren<Text>();
        }
        public void SetItem(string showInfo)
        {
            if (text == null)
                text = transform.GetComponentInChildren<Text>();
            text.text = showInfo;
        }
    }
}
