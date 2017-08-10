using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {
    public Image sourceImage;
    public Sprite[] images;
    
    public void ShowImage(int _index) {
        this.gameObject.SetActive(true);
        sourceImage.sprite = images[_index];
    }
}
