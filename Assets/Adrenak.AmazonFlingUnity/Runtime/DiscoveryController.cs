using System;

using UnityEngine;

using Config = Adrenak.AmazonFlingUnity.AmazonFlingUnityConfig;

namespace Adrenak.AmazonFlingUnity {
    /// <summary>
    /// Provides service to discover FireTV devices in the local network.
    /// This is a 1:1 mapping to DisconveryController class in the Fling 
    /// Android SDK class.
    /// </summary>
    public class DiscoveryController {
        const string TAG = "DiscoveryController(Adrenak)";

        readonly AndroidJavaObject native;

        public DiscoveryController() {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            native = new AndroidJavaObject(
                "com.amazon.whisperplay.fling.media.controller.DiscoveryController",
                context
            );
        }

        /// <summary>
        /// Starts discovery of FireTV devices
        /// </summary>
        /// <param name="listener">
        /// The <see cref="IDiscoveryListener"/> instance that provides disvoery related events.
        /// </param>
        public void Start(IDiscoveryListener listener) {
            native.Call("start", "amzn.thin.pl", listener);
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Started discovery");
        }

        /// <summary>
        /// Stops the discovery process.
        /// </summary>
        public void Stop() {
            native.Call("stop");
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Stopped discovery");
        }
    }

    /// <summary>
    /// Provides events listening capabilities for FireTV discovery.
    /// This is a 1:1 mapping to IDiscoveryListener interface in the DiscoveryController
    /// class in the Fling Android SDK.
    /// </summary>
    public class IDiscoveryListener : AndroidJavaProxy {
        const string TAG = "IDiscoveryListener(Adrenak)";

        Action<RemoteMediaPlayer> onPlayerDiscoveredCB;
        Action<RemoteMediaPlayer> onPlayerLostCB;
        Action onDiscoveryFailureCB;

        public IDiscoveryListener() : base("com.amazon.whisperplay.fling.media.controller.DiscoveryController$IDiscoveryListener") { }

        /// <summary>
        /// Add a subscriber to <see cref="RemoteMediaPlayer"/> discovery event.
        /// </summary>
        /// <param name="callback">The callback to handle the event.</param>
        /// <returns></returns>
        public IDiscoveryListener OnPlayerDiscovered(Action<RemoteMediaPlayer> callback) {
            onPlayerDiscoveredCB = callback;
            return this;
        }

        /// <summary>
        /// Add a subscriber to <see cref="RemoteMediaPlayer"/> discovery event.
        /// </summary>
        /// <param name="callback">The callback to handle the event.</param>
        /// <returns></returns>
        public IDiscoveryListener OnPlayerLost(Action<RemoteMediaPlayer> callback) {
            onPlayerLostCB = callback;
            return this;
        }

        /// <summary>
        /// Add a subscriber to discovery failure event.
        /// </summary>
        /// <param name="callback">The callback to handle the event.</param>
        /// <returns></returns>
        public IDiscoveryListener OnDiscoveryFailure(Action callback) {
            onDiscoveryFailureCB = callback;
            return this;
        }

        // The next three methods are unused in C#, they are invoked by the 
        // DiscoveryController Java class using reflection. Don't remove them.
        void playerDiscovered(AndroidJavaObject player) {
            var rmp = new RemoteMediaPlayer(player);
            onPlayerDiscoveredCB?.Invoke(rmp);
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Player discovered: " + rmp.GetName() + " " + rmp.GetUniqueIdentifier());
        }

        void playerLost(AndroidJavaObject player) {
            var rmp = new RemoteMediaPlayer(player);
            onPlayerLostCB?.Invoke(rmp);
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Player lost: " + rmp.GetName() + " " + rmp.GetUniqueIdentifier());
        }

        void discoveryFailure() {
            onDiscoveryFailureCB?.Invoke();
            if (Config.EnableDebugging)
                Debug.unityLogger.LogError(TAG, "Discovery failed!");
        }
    }
}
