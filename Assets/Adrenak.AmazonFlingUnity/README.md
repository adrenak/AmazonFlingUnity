# AmazonFlingUnity

Note that this is only for Android devices (currently).

Amazon Fling Untiy plugin has a `.so` file that is supposed to be for ARM64 devices, but has actually been build for 32 bit architecture. This makes it impossible to release an app on Google Play, as it requires apks to have 64-bit support.

This wrapper works by directly invoking the Fling jar files and provides C# classes that act as proxy for typesafety and other C# benefits. The jar files in this repo are the ones released in 2020 (Amazon did release iOS and Android SDK updates in 2020 but the Unity plugin hasn't been updated since 2018). 

Readme, docs and samples coming soon. Until then, this is a good starting point to start testing:

```
using System.Collections.Generic;
using UnityEngine;
using Adrenak.AmazonFlingUnity;
using System.Linq;

// Attach this script to a gameobject in your test scene.
public class FlingTest : MonoBehaviour {
    DiscoveryController controller;
    List<RemoteMediaPlayer> players = new List<RemoteMediaPlayer>();

    void Start() {
        AmazonFlingUnityConfig.EnableDebugging = true;
        controller = new DiscoveryController();
        controller.Start(new IDiscoveryListener()
            .OnPlayerDiscovered(discovered => {
                // if the players list doesn't contain an element with the same unique ID, add the new player
                if(players.Where(p => p.GetUniqueIdentifier().Equals(discovered.GetUniqueIdentifier())).Count() == 0)
                    players.Add(discovered);
            })
            .OnPlayerLost(lost => {
                // Update the players list by excluding any elements that have the same unique ID
                players = players.Where(p => p.GetUniqueIdentifier().Equals(lost.GetUniqueIdentifier())).ToList();
            }));
    }

    // Invoke this using a UI button or something
    public void Play() {
        if(players.Count > 0)
            players[0].SetMediaSource("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", "Big Buck Bunny", true, true);
        else
            Debug.Log("No players available!");
    }
}
```