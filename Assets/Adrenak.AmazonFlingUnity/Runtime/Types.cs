using System;
using System.Text;

using UnityEngine;

namespace Adrenak.AmazonFlingUnity {
    /// <summary>
    /// Represents information related to the media player.
    /// </summary>
    [Serializable]
    public class MediaPlayerInfo {
        /// <summary>
        /// The media/content source
        /// </summary>
        public string source;

        /// <summary>
        /// The metadata associated with the media/content
        /// </summary>
        public string metadata;

        /// <summary>
        /// Extra data associated with the media player.
        /// </summary>
        public string extra;

        /// <summary>
        /// Constructs an instnace using an AndroidJavaObject
        /// </summary>
        /// <param name="obj">The AndroidJavaObject to use for construction</param>
        /// <returns></returns>
        public static MediaPlayerInfo From(AndroidJavaObject obj) {
            return new MediaPlayerInfo {
                source = obj.Call<string>("getSource"),
                metadata = obj.Call<string>("getMetadata"),
                extra = obj.Call<string>("getExtra")
            };
        }
    }

    /// <summary>
    /// Represents the media player status.
    /// </summary>
    [Serializable]
    public class MediaPlayerStatus {
        /// <summary>
        /// The state of the media player.
        /// </summary>
        public MediaState mediaState;

        /// <summary>
        /// The condition of the media playback.
        /// </summary>
        public MediaCondition mediaCondition;

        /// <summary>
        /// Whether the media player is in mute state.
        /// </summary>
        public bool mute;

        /// <summary>
        /// The volume of the media player.
        /// </summary>
        public double volume;

        /// <summary>
        /// ?
        /// </summary>
        public bool muteSet;

        /// <summary>
        /// ?
        /// </summary>
        public bool volumeSet;

        /// <summary>
        /// Constructs an instance using an AndroidJavaObject
        /// </summary>
        /// <param name="statusObj">The AndroidJavaObject to use for construction.</param>
        /// <returns></returns>
        public static MediaPlayerStatus From(AndroidJavaObject statusObj) {
            return new MediaPlayerStatus {
                mediaState = (MediaState)Enum.Parse(typeof(MediaState), statusObj.Call<AndroidJavaObject>("getState").Call<string>("toString")),
                mediaCondition = (MediaCondition)Enum.Parse(typeof(MediaCondition), statusObj.Call<AndroidJavaObject>("getCondition").Call<string>("toString")),
                mute = statusObj.Call<bool>("isMute"),
                volume = statusObj.Call<double>("getVolume"),
                muteSet = statusObj.Call<bool>("isMuteSet"),
                volumeSet = statusObj.Call<bool>("isVolumeSet")
            };
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("mediaState: ").Append(mediaState).Append(" ")
            .Append("mediaCondition: ").Append(mediaCondition).Append(" ")
            .Append("mute: ").Append(mute).Append(" ")
            .Append("volume: ").Append(volume).Append(" ")
            .Append("muteSet: ").Append(muteSet).Append(" ")
            .Append("volumeSet: ").Append(volumeSet);
            return sb.ToString();
        }
    }

    /// <summary>
    /// The seek mode/style/method
    /// </summary>
    public enum PlayerSeekMode {
        /// <summary>
        /// Seeks to an absolute position. This must be non zero and smaller than the media duration.
        /// </summary>
        Absolute,

        /// <summary>
        /// Seeks to a position relative to the current position in the player. This can be negative.
        /// </summary>
        Relative
    }

    /// <summary>
    /// The current state of the media associated with the player
    /// </summary>
    public enum MediaState {
        /// <summary>
        /// No source has been set in the player.
        /// </summary>
        NoSource,

        /// <summary>
        /// The player is currently preparing the media.
        /// </summary>
        PreparingMedia,

        /// <summary>
        /// The player is ready to play the media.
        /// </summary>
        ReadyToPlay,

        /// <summary>
        /// The player is playing the media.
        /// </summary>
        Playing,

        /// <summary>
        /// The media is in paused state in the player.
        /// </summary>
        Paused,

        /// <summary>
        /// The player is seeking over the loaded media.
        /// </summary>
        Seeking,

        /// <summary>
        /// The player has finished playing the media.
        /// </summary>
        Finished,

        /// <summary>
        /// The player is in an errored state.
        /// </summary>
        Error
    }

    /// <summary>
    /// Represents the condition of media playback.
    /// </summary>
    public enum MediaCondition {
        /// <summary>
        /// The media is playing well.
        /// </summary>
        Good,

        /// <summary>
        /// ?
        /// </summary>
        WarningContent,

        /// <summary>
        /// The player is facing bacndwidth limitations while playing the media.
        /// </summary>
        WarningBandwidth,

        /// <summary>
        /// There is error in the content. 
        /// </summary>
        ErrorContent,

        /// <summary>
        /// ?
        /// </summary>
        ErrorChannel,

        /// <summary>
        /// Unknown error has been encountered in media playback.
        /// </summary>
        ErrorUnknown
    }
}