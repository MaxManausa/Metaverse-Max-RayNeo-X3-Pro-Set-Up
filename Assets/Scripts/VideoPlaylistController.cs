using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoPlaylistController : MonoBehaviour
{
    // ... (Variables and Start method remain the same) ...
    public VideoPlayer videoPlayer;
    public List<VideoClip> playlist = new List<VideoClip>();
    public int currentClipIndex = 0;

    void Start()
    {
        // 1. Check for the VideoPlayer component.
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned. Please assign it in the Inspector.");
            enabled = false;
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
        videoPlayer.loopPointReached += OnVideoFinished;

        // 4. Start playback with the first clip.
        PlayClip(currentClipIndex);
    }

    /// <summary>
    /// Starts playing the video clip at the given index.
    /// </summary>
    public void PlayClip(int index)
    {
        if (index >= 0 && index < playlist.Count)
        {
            videoPlayer.clip = playlist[index];
            videoPlayer.Play();
            Debug.Log($"Now playing: **{playlist[index].name}** (Index: {index})");
        }
    }

    /// <summary>
    /// Called when the current video clip finishes playback. (Logic unchanged)
    /// </summary>
    public void OnVideoFinished(VideoPlayer vp)
    {
        // This is where auto-advance happens. It calls PlayClip automatically.
        currentClipIndex++;

        if (currentClipIndex >= playlist.Count)
        {
            currentClipIndex = 0;
        }

        PlayClip(currentClipIndex);
    }

    // --- CORRECTED METHOD ---
    /// <summary>
    /// Advances the index and immediately plays the next clip.
    /// </summary>
    public void PlayNext()
    {
        // 1. Increment the index.
        currentClipIndex++;

        // 2. Wrap around if necessary (based on your existing auto-advance logic).
        if (currentClipIndex >= playlist.Count)
        {
            currentClipIndex = 0;
        }

        // 3. Call the function that actually assigns the clip and starts playing.
        PlayClip(currentClipIndex);
    }

    // --- ENHANCED METHOD ---
    /// <summary>
    /// Decrements the index and immediately plays the previous clip.
    /// </summary>
    public void GoBack()
    {
        // 1. Decrement the index.
        currentClipIndex--;

        // 2. Wrap around to the end of the list if necessary.
        if (currentClipIndex < 0)
        {
            currentClipIndex = playlist.Count - 1;
        }

        // 3. Play the clip.
        PlayClip(currentClipIndex);
    }

    // ... (Pause, Play, and OnDestroy methods remain the same) ...
    public void Pause()
    {
        videoPlayer.Pause();
    }

    public void Play()
    {
        videoPlayer.Play();
    }

    public void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}