using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBlock : Block
{
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);

        if (source.isPlaying)
        {
            source.Stop();
        }
        else
        {
            source.Play();
        }
    }
}
