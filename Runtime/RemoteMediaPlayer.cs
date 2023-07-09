using System;
using System.Threading.Tasks;

using UnityEngine;

using Config = Adrenak.AmazonFlingUnity.AmazonFlingUnityConfig;

namespace Adrenak.AmazonFlingUnity {
    /// <summary>
    /// The class that represents an instance of FireTV based media player.
    /// This is a 1:1 mapping to the RemoteMediaPlayer class in the Fling Android SDK.
    /// </summary>
    public class RemoteMediaPlayer {
        const string TAG = "RemoteMediaPlayer(Adrenak)";

        readonly AndroidJavaObject native;

        internal RemoteMediaPlayer(AndroidJavaObject native) {
            this.native = native;
        }

        /// <summary>
        /// Gets the name of the FireTV device.
        /// </summary>
        /// <returns></returns>
        public string GetName() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getName");
            return native.Call<string>("getName");
        }

        /// <summary>
        /// Gets a unique ID of the FireTV device.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueIdentifier() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getUniqueIdentifier");
            return native.Call<string>("getUniqueIdentifier");
        }

        /// <summary>
        /// Gets the volume of the media player on the FireTV device.
        /// Note: This doesn't work with the default/inguilt media player.
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetVolume() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getVolume");
            var x = await GetAsStringAsync("getVolume");
            return double.Parse(x);
        }

        /// <summary>
        /// Sets the volume of the media player on the FireTV device.
        /// Note: This doesn't work with the default/inguilt media player.
        /// </summary>
        /// <param name="value">The volume to set.</param>
        /// <returns></returns>
        public Task SetVolume(double value) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling setVolume");
            return InvokeAsync("setVolume", value);
        }

        /// <summary>
        /// Whether the media player on the FireTV device is muted.
        /// Note: This doesn't work with the default/inguilt media player.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsMute() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling isMute");
            var str = await GetAsStringAsync("isMute");
            return bool.Parse(str);
        }

        /// <summary>
        /// Sets the media player on the FireTV device as muted/unmuted.
        /// Note: This doesn't work with the default/inguilt media player.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SetMute(bool value) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling setMute");
            return InvokeAsync("setMute", value);
        }

        /// <summary>
        /// Gets the current position of the media playing on the media player.
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetPosition() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getPosition");
            var str = await GetAsStringAsync("getPosition");
            return long.Parse(str);
        }

        /// <summary>
        /// Gets the duration of the media playing on the FireTV device.
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetDuration() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getDuration");
            var str = await GetAsStringAsync("getDuration");
            return long.Parse(str);
        }

        /// <summary>
        /// Gets the status of the media player on the FireTV device.
        /// </summary>
        /// <returns></returns>
        public async Task<MediaPlayerStatus> GetStatus() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getStatus");
            var obj = await GetAsAndroidJavaObjectAsync("getStatus");
            return MediaPlayerStatus.From(obj);
        }

        /// <summary>
        /// Whether the media player on the FireTV device supports the
        /// given Mime type.
        /// </summary>
        /// <param name="mime">The Mime type to check for.</param>
        /// <returns></returns>
        public async Task<bool> IsMimeTypeSupported(string mime) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling isMimeTypeSupported with query " + mime);
            var str = await GetAsStringAsync("isMimeTypeSupported", mime);
            return bool.Parse(str);
        }

        /// <summary>
        /// Pauses the media playback.
        /// </summary>
        /// <returns></returns>
        public async Task Pause() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling pause");
            await InvokeAsync("pause");
        }

        /// <summary>
        /// Plays the media.
        /// </summary>
        /// <returns></returns>
        public async Task Play() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling play");
            await InvokeAsync("play");
        }

        /// <summary>
        /// Stops the media playback.
        /// </summary>
        /// <returns></returns>
        public async Task Stop() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling stop");
            await InvokeAsync("stop");
        }

        /// <summary>
        /// Seeks to a position (in milliseconds)
        /// </summary>
        /// <param name="playerSeekMode">The seek mode.</param>
        /// <param name="delta">Position to seek to.</param>
        /// <returns></returns>
        public async Task Seek(PlayerSeekMode playerSeekMode, long delta) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling seek with mode " + playerSeekMode + " and delta " + delta);
            var myClass = new AndroidJavaClass("com.amazon.whisperplay.fling.media.service.CustomMediaPlayer$PlayerSeekMode");
            AndroidJavaObject myEnum = myClass.GetStatic<AndroidJavaObject>(playerSeekMode.ToString());
            await InvokeAsync("seek", myEnum, delta);
        }

        /// <summary>
        /// Sets a media source.
        /// </summary>
        /// <param name="url">The URL to play from.</param>
        /// <param name="title">The title of the media</param>
        /// <param name="autoPlay">Whether the playback should begin automatically once the content is ready.</param>
        /// <param name="playInBackground">Whether the playback should continue when the player is in the background.</param>
        /// <returns></returns>
        public Task SetMediaSource(string url, string title, bool autoPlay, bool playInBackground) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, $"Calling setMediaSource with url {url}, title {title}, autoPlay {autoPlay} and playInBackground {playInBackground}");
            return InvokeAsync("setMediaSource", url, title, autoPlay, playInBackground);
        }

        /// <summary>
        /// Sets the player style.
        /// Note: This doesn't work with the default/inguilt media player.
        /// </summary>
        /// <param name="style">Style to set.</param>
        /// <returns></returns>
        public async Task SetPlayerStyle(string style) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling setPlayerStyle with style " + style);
            await InvokeAsync("setPlayerStyle", style);
        }

        /// <summary>
        /// Adds a listener to <see cref="RemoteMediaPlayer"/> status change events.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public Task AddStatusListener(StatusListener listener) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling addStatusListener");
            return InvokeAsync("addStatusListener", listener);
        }

        /// <summary>
        /// Removes a listener <see cref="RemoteMediaPlayer"/> status change events.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public Task RemoveStatusListener(StatusListener listener) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling removeStatusListener");
            return InvokeAsync("removeStatusListener", listener);
        }

        /// <summary>
        /// Sets the interval at which status change events will be raised.
        /// </summary>
        /// <param name="interval">The interval in milliseconds</param>
        /// <returns></returns>
        public Task SetPositionUpdateInterval(long interval) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling setPositionUpdateInternal with interval " + interval);
            return InvokeAsync("setPositionUpdateInterval", interval);
        }

        /// <summary>
        /// Sends a command to the media player.
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <returns></returns>
        public Task SendCommand(string command) {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling sendCommand with command " + command);
            return InvokeAsync("sendCommand", command);
        }

        /// <summary>
        /// Gets info of the media currently assigned to the media player.
        /// </summary>
        /// <returns></returns>
        public async Task<MediaPlayerInfo> GetMediaInfo() {
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, "Calling getMediaInfo");
            var obj = await GetAsAndroidJavaObjectAsync("getMediaInfo");
            return MediaPlayerInfo.From(obj);
        }

        // Invokes an async method on the player over JNI
        async Task InvokeAsync(string methodName, params object[] args) {
            var future = native.Call<AndroidJavaObject>(methodName, args);
            while (!future.Call<bool>("isDone")) {
                Debug.Log("Waiting...");
                await Task.Delay(100);
            }
            Debug.Log("Done!");
            return;
        }

        // Invokes an async method on the player over JNI and returns the result as AndroidJavaObject
        async Task<AndroidJavaObject> GetAsAndroidJavaObjectAsync(string methodName, params object[] args) {
            var future = native.Call<AndroidJavaObject>(methodName, args);
            while (!future.Call<bool>("isDone"))
                await Task.Delay(100);
            return future.Call<AndroidJavaObject>("get");
        }

        // Invokes an async method on the player over JNI and returns the result as string.
        async Task<string> GetAsStringAsync(string methodName, params object[] args) {
            return (await GetAsAndroidJavaObjectAsync(methodName, args)).Call<string>("toString");
        }
    }

    /// <summary>
    /// Provides event listening capabilities for <see cref="RemoteMediaPlayer"/> status updates.
    /// This is a 1:1 mapping to the StatusListener interface in the CustomMediaplayer class in the Fling Android SDK.
    /// </summary>
    public class StatusListener : AndroidJavaProxy {
        const string TAG = "StatusListener(Adrenak)";
        public StatusListener() : base("com.amazon.whisperplay.fling.media.service.CustomMediaPlayer$StatusListener") { }

        /// <summary>
        /// Invoked on <see cref="RemoteMediaPlayer"/> status update.
        /// </summary>
        public event Action<MediaPlayerStatus, long> OnStatusChange;

        void onStatusChange(AndroidJavaObject statusObj, long position) {
            var status = MediaPlayerStatus.From(statusObj);
            if (Config.EnableDebugging)
                Debug.unityLogger.Log(TAG, $"Status changed to {status} at position {position}");
            OnStatusChange?.Invoke(status, position);
        }
    }
}