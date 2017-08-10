using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidebarManager : MonoBehaviour {

    private GameObject[] panels;
    private Image background;
    private void Awake() {
        background = GetComponent<Image>();
        panels = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            panels[i] = transform.GetChild(i).gameObject;
        }
    }

    public void ActivatePanel(int _index) {
        background.enabled = _index == -1;
        for (int i = 0; i < panels.Length; i++) {
                panels[i].SetActive(i == _index);
        }
    }
}
