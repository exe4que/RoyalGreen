using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidebarElementBehaviour : MonoBehaviour {

    Image thisImg;
    Text thisTxt;

    void Awake() {
        thisTxt = transform.GetChild(0).GetComponent<Text>();
        thisImg = transform.GetChild(1).GetComponent<Image>();
    }

    public void SetElement(string _text, Texture2D _image) {
        thisTxt.text = _text;
        thisImg.material.mainTexture = _image;
    }
}
