using Intersect.Editor.Content;
using Intersect.Editor.Core;
using Intersect.Editor.Localization;
using Microsoft.Extensions.Logging;
using NAudio.Vorbis;
using NAudio.Wave;

namespace Intersect.Editor.Forms.Editors;

public partial class FrmMusicPlayer : ResponsiveForm
{
    // Static state so music persists after window is closed
    private static readonly object _audioLock = new();
    private static WaveOutEvent? _musicOutput;
    private static VorbisWaveReader? _musicReader;
    private static string? _currentMusicFile;

    // Only one instance at a time
    private static FrmMusicPlayer? _instance;

    public static FrmMusicPlayer GetOrCreate()
    {
        if (_instance == null || _instance.IsDisposed)
        {
            _instance = new FrmMusicPlayer();
        }

        return _instance;
    }

    public FrmMusicPlayer()
    {
        InitializeComponent();
        Icon = Program.Icon;
    }

    private void InitLocalization()
    {
        Text = Strings.MusicPlayer.title;
        btnClose.Text = Strings.MusicPlayer.close;
        UpdatePlayStopButton();
    }

    private static bool IsPlaying
    {
        get
        {
            lock (_audioLock)
            {
                return _musicOutput?.PlaybackState == PlaybackState.Playing;
            }
        }
    }

    private void FrmMusicPlayer_Load(object sender, EventArgs e)
    {
        InitLocalization();
        PopulateFileList();
    }

    private void PopulateFileList()
    {
        lstFiles.Items.Clear();
        var musicNames = GameContentManager.SmartSortedMusicNames;
        if (musicNames != null)
        {
            foreach (var name in musicNames)
            {
                lstFiles.Items.Add(name);
            }
        }

        // If there is something currently playing, select it in the list
        string? current;
        lock (_audioLock)
        {
            current = _currentMusicFile;
        }

        if (!string.IsNullOrEmpty(current))
        {
            var index = lstFiles.Items.IndexOf(current);
            if (index >= 0)
            {
                lstFiles.SelectedIndex = index;
            }
        }
    }

    private void UpdatePlayStopButton()
    {
        btnPlayStop.Text = IsPlaying ? Strings.MusicPlayer.stop : Strings.MusicPlayer.play;
    }

    private void btnPlayStop_Click(object sender, EventArgs e)
    {
        if (IsPlaying)
        {
            StopMusic();
            UpdatePlayStopButton();
        }
        else
        {
            if (lstFiles.SelectedIndex < 0)
            {
                return;
            }

            var selectedFile = lstFiles.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedFile))
            {
                return;
            }

            PlayMusic(selectedFile);
            UpdatePlayStopButton();
        }
    }

    private static void PlayMusic(string fileName)
    {
        StopMusic();

        var filePath = System.IO.Path.Combine("resources", "music", fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return;
        }

        lock (_audioLock)
        {
            try
            {
                _musicReader = new VorbisWaveReader(filePath);
                _musicOutput = new WaveOutEvent();
                _musicOutput.Init(_musicReader);
                _musicOutput.PlaybackStopped += OnMusicPlaybackStopped;
                _musicOutput.Play();
                _currentMusicFile = fileName;
            }
            catch (Exception ex)
            {
                Intersect.Core.ApplicationContext.Context.Value?.Logger.LogError(ex, "Failed to play music: {0}", fileName);
                DisposeAudioResourcesLocked();
                _currentMusicFile = null;
            }
        }
    }

    private static void OnMusicPlaybackStopped(object? sender, StoppedEventArgs e)
    {
        // Invoked on NAudio background thread; use lock to protect static state
        lock (_audioLock)
        {
            // Natural completion: cleanup resources (manual stop unsubscribes before calling Stop())
            DisposeAudioResourcesLocked();
            _currentMusicFile = null;
        }
    }

    private static void StopMusic()
    {
        lock (_audioLock)
        {
            if (_musicOutput != null)
            {
                _musicOutput.PlaybackStopped -= OnMusicPlaybackStopped;
                _musicOutput.Stop();
            }

            DisposeAudioResourcesLocked();
            _currentMusicFile = null;
        }
    }

    /// <summary>Must be called while holding <see cref="_audioLock"/>.</summary>
    private static void DisposeAudioResourcesLocked()
    {
        _musicOutput?.Dispose();
        _musicOutput = null;
        _musicReader?.Dispose();
        _musicReader = null;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Only update button state; don't auto-play on selection
    }
}
