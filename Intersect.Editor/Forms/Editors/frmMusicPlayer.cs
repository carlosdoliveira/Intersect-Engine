using System.Media;
using DarkUI.Controls;
using DarkUI.Forms;
using Intersect.Editor.Content;
using Intersect.Editor.Core;
using Intersect.Editor.Localization;
using NAudio.Vorbis;
using NAudio.Wave;

namespace Intersect.Editor.Forms.Editors;

public partial class FrmMusicPlayer : ResponsiveForm
{
    // Static state so music persists after window is closed
    private static WaveOutEvent? _musicOutput;
    private static VorbisWaveReader? _musicReader;
    private static string? _currentMusicFile;

    public FrmMusicPlayer()
    {
        InitializeComponent();
        Icon = Program.Icon;
        InitLocalization();
    }

    private void InitLocalization()
    {
        Text = Strings.MusicPlayer.title;
        btnPlayStop.Text = IsPlaying ? Strings.MusicPlayer.stop : Strings.MusicPlayer.play;
        btnClose.Text = Strings.MusicPlayer.close;
    }

    private static bool IsPlaying => _musicOutput?.PlaybackState == PlaybackState.Playing;

    private void FrmMusicPlayer_Load(object sender, EventArgs e)
    {
        PopulateFileList();
        UpdatePlayStopButton();
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
        if (!string.IsNullOrEmpty(_currentMusicFile))
        {
            var index = lstFiles.Items.IndexOf(_currentMusicFile);
            if (index >= 0)
            {
                lstFiles.SelectedIndex = index;
            }
        }
    }

    private void UpdatePlayStopButton()
    {
        if (IsPlaying)
        {
            btnPlayStop.Text = Strings.MusicPlayer.stop;
        }
        else
        {
            btnPlayStop.Text = Strings.MusicPlayer.play;
        }
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

        try
        {
            _musicReader = new VorbisWaveReader(filePath);
            _musicOutput = new WaveOutEvent();
            _musicOutput.Init(_musicReader);
            _musicOutput.PlaybackStopped += OnMusicPlaybackStopped;
            _musicOutput.Play();
            _currentMusicFile = fileName;
        }
        catch
        {
            StopMusic();
        }
    }

    private static void OnMusicPlaybackStopped(object? sender, StoppedEventArgs e)
    {
        // Cleanup only when naturally finished (not manually stopped)
        if (e.Exception == null && _musicOutput?.PlaybackState == PlaybackState.Stopped)
        {
            DisposeAudioResources();
            _currentMusicFile = null;
        }
    }

    private static void StopMusic()
    {
        if (_musicOutput != null)
        {
            _musicOutput.PlaybackStopped -= OnMusicPlaybackStopped;
            _musicOutput.Stop();
        }

        DisposeAudioResources();
        _currentMusicFile = null;
    }

    private static void DisposeAudioResources()
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
