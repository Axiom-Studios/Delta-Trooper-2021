using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IntroBackgroundAnimation : MonoBehaviour
{
    Material material;
    public Vector2 scrollSpeed;
    public Texture[] textures;
    public GameObject startButtons;
    int curFrame;
    bool isAnimating = true;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating)
        {
            curFrame = Mathf.FloorToInt(Time.time * 19);
            if (curFrame >= textures.Length)
            {
                curFrame = textures.Length - 1;
                startButtons.SetActive(true);
                isAnimating = false;
            }
            material.SetTexture("_EmissionMap", textures[curFrame]);
            Debug.Log(curFrame);
        } 
    }
}
