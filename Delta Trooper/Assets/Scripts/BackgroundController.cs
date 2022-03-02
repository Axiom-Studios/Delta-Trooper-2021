using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    Material material;
    public Vector2 scrollSpeed;
    public Texture[] textures;
    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollSpeed * Time.deltaTime;
    }

    public void ChangeBG(int level)
    {
        material.mainTextureOffset = new Vector2(0, 0);
        //material.mainTexture = textures[level];

        material.SetTexture("_EmissionMap",textures[level]);
    }
}
