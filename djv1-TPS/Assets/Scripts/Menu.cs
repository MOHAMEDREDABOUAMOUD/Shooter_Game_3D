using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
  [SerializeField] private SkinsManager skinsManager;
  public void Quit()
  {
    Application.Quit();
  }

  public void Play()
  {
    PlayerPrefs.SetInt("skinIndex", skinsManager.GetSelectedSkinIndex());
    PlayerPrefs.Save();
    SceneManager.LoadScene(1, LoadSceneMode.Single);
  }
}
