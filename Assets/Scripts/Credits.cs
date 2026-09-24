using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    //[SerializeField] private List<GameObject> creditsButton = new List<GameObject>();
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject menuButton;
    public void onClick()
    {
        creditsButton.SetActive(false);
        menuButton.SetActive(true);
    }
    public void onBackClick()
    {
        creditsButton.SetActive(true);
        menuButton.SetActive(false);
    }

}
