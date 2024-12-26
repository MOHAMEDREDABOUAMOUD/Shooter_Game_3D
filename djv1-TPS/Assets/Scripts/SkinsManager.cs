using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] skins;
    [SerializeField] private int selectedSkinIndex;
    [SerializeField] private bool isRandomSkin;

    private void Awake()
    {
        skins[selectedSkinIndex].SetActive(true);
        if(isRandomSkin)
            SelectRandomSkin();

        if (isRandomSkin) return;
        var index = PlayerPrefs.GetInt("skinIndex");
        SelectSkin(index);
    }

    private void SelectSkin(int index)
    {
        skins[selectedSkinIndex].SetActive(false);
        skins[index].SetActive(true);
        selectedSkinIndex = index;
    }

    public void SelectNextSkin()
    {
        skins[selectedSkinIndex].SetActive(false);
        selectedSkinIndex = selectedSkinIndex + 1 > skins.Length - 1 ? 0 : selectedSkinIndex + 1;
        skins[selectedSkinIndex].SetActive(true);
    }
    
    public void SelectPreviousSkin()
    {
        skins[selectedSkinIndex].SetActive(false);
        selectedSkinIndex = selectedSkinIndex - 1 < 0  ? skins.Length - 1 : selectedSkinIndex - 1;
        skins[selectedSkinIndex].SetActive(true);
    }

    public void SelectRandomSkin()
    {
        skins[selectedSkinIndex].SetActive(false);
        var index = Random.Range(0, skins.Length);
        skins[index].SetActive(true);
        selectedSkinIndex = index;
    }

    public GameObject GetSelectedSkin()
    {
        return skins[selectedSkinIndex];
    }

    public int GetSelectedSkinIndex()
    {
        return selectedSkinIndex;
    }
}
