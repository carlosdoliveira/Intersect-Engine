using System.Media;
using Intersect.Editor.Content;
using Intersect.Editor.Core;
using Intersect.Editor.Localization;
using Microsoft.Extensions.Logging;

namespace Intersect.Editor.Forms.Editors;

public partial class FrmSoundPlayer : ResponsiveForm
{
    private SoundPlayer? _soundPlayer;

    public FrmSoundPlayer()
    {
        InitializeComponent();
        Icon = Program.Icon;
        InitLocalization();
    }

    private void InitLocalization()
    {
        Text = Strings.SoundPlayer.title;
        btnPlayStop.Text = Strings.SoundPlayer.play;
        btnClose.Text = Strings.SoundPlayer.close;
    }

    private bool _isPlaying;

    private void FrmSoundPlayer_Load(object sender, EventArgs e)
    {
        PopulateFileList();
    }

    private void PopulateFileList()
    {
        lstFiles.Items.Clear();
        var soundNames = GameContentManager.SmartSortedSoundNames;
        if (soundNames != null)
        {
            foreach (var name in soundNames)
            {
                lstFiles.Items.Add(name);
            }
        }
    }

    private void UpdatePlayStopButton()
    {
        btnPlayStop.Text = _isPlaying ? Strings.SoundPlayer.stop : Strings.SoundPlayer.play;
    }

    private void btnPlayStop_Click(object sender, EventArgs e)
    {
        if (_isPlaying)
        {
            StopSound();
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

            PlaySound(selectedFile);
            UpdatePlayStopButton();
        }
    }

    private void PlaySound(string fileName)
    {
        StopSound();

        var filePath = System.IO.Path.Combine("resources", "sounds", fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return;
        }

        try
        {
            _soundPlayer = new SoundPlayer(filePath);
            _soundPlayer.Play();
            _isPlaying = true;
        }
        catch (Exception ex)
        {
            Intersect.Core.ApplicationContext.Context.Value?.Logger.LogError(ex, "Failed to play sound: {0}", fileName);
            StopSound();
        }
    }

    private void StopSound()
    {
        if (_soundPlayer != null)
        {
            _soundPlayer.Stop();
            _soundPlayer.Dispose();
            _soundPlayer = null;
        }

        _isPlaying = false;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Only update button state; don't auto-play on selection
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        StopSound();
        base.OnFormClosed(e);
    }
}
