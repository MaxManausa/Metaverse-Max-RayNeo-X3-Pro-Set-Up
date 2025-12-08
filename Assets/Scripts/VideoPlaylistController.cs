using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoPlaylistController : MonoBehaviour
{
    // The VideoPlayer component that will play the clips.
    [Tooltip("The VideoPlayer component on this GameObject or a child.")]
    public VideoPlayer videoPlayer;

    // The list of video clips to play in order.
    [Tooltip("The list of video clips for the playlist.")]
    public List<VideoClip> playlist = new List<VideoClip>();

    // Index to track the current video in the playlist.
    private int currentClipIndex = 0;

    void Start()
    {
        // 1. Check for the VideoPlayer component.
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned. Please assign it in the Inspector.");
            enabled = false; // Disable the script if no player is found.
            return;
        }

        // 2. Check if the playlist has clips.
        if (playlist.Count == 0)
        {
            Debug.LogError("The playlist is empty. Please add video clips to the list in the Inspector.");
            enabled = false;
            return;
        }

        // 3. Subscribe to the 'loopPointReached' event.
        // This event fires when the video finishes (if looping is off).
        videoPlayer.loopPointReached += OnVideoFinished;

        // 4. Start playback with the first clip.
        PlayClip(currentClipIndex);
    }

    /// <summary>
    /// Starts playing the video clip at the given index.
    /// </summary>
    private void PlayClip(int index)
    {
        if (index >= 0 && index < playlist.Count)
        {
            videoPlayer.clip = playlist[index];
            videoPlayer.Play();
            Debug.Log($"Now playing: **{playlist[index].name}** (Index: {index})");
        }
    }

    /// <summary>
    /// Called when the current video clip finishes playback.
    /// </summary>
    private void OnVideoFinished(VideoPlayer vp)
    {
        // Move to the next index.
        currentClipIndex++;

        // Check if we have reached the end of the playlist.
        if (currentClipIndex >= playlist.Count)
        {
            // If the playlist is finished, reset the index to 0 to loop, or stop.
            currentClipIndex = 0; // Uncomment this to loop the entire playlist.
            // videoPlayer.Stop(); // Uncomment this to stop after the last clip.
            // Debug.Log("Playlist finished.");
            // return; // Exit if you choose to stop.
        }

        // Play the next clip in the list.
        PlayClip(currentClipIndex);
    }

    // Clean up the event subscription when the script is destroyed.
    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}