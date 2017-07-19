using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {
    public Sprite[] images;
    private Image img;

    private void Awake() {
        img = transform.GetChild(0).GetComponent<Image>();
    }

    public void ShowImage(int _index) {
        this.gameObject.SetActive(true);
        img.sprite = images[_index];
    }
}
