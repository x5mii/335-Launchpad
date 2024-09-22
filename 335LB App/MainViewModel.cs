using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Plugin.Maui.Audio;
using Microsoft.Maui.Storage;
using System.Windows.Input;

namespace _335LB_App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> SoundButtons { get; set; }
        public ICommand ButtonCommand { get; set; }
        public ICommand ImportSoundCommand { get; set; }

        private Dictionary<string, string> soundFiles;

        public MainViewModel()
        {
            SoundButtons = new ObservableCollection<string>
            {
                "Launchpad1", "Launchpad2", "Launchpad3",
                "Launchpad4", "Launchpad5", "Launchpad6",
                "Launchpad7", "Launchpad8", "Launchpad9"
            };

            soundFiles = new Dictionary<string, string>
            {
                { "Launchpad1", "kick.wav" },
                { "Launchpad2", "clap.wav" },
                { "Launchpad3", "hat.wav" },
                { "Launchpad4", "snare.wav" },
                { "Launchpad5", "snare.wav" },
                { "Launchpad6", "snare.wav" },
                { "Launchpad7", "snare.wav" },
                { "Launchpad8", "snare.wav" },
                { "Launchpad9", "snare.wav" },
            };

            // Lade gespeicherte Dateipfade aus den Einstellungen
            foreach (var key in soundFiles.Keys.ToList())
            {
                var savedPath = Preferences.Get(key, null);
                if (!string.IsNullOrEmpty(savedPath))
                {
                    soundFiles[key] = savedPath;
                }
            }

            ButtonCommand = new Command<string>(OnButtonClicked);
            ImportSoundCommand = new Command(async () => await OnImportSoundClicked());
        }

        private async void OnButtonClicked(string buttonId)
        {
            if (soundFiles.TryGetValue(buttonId, out var filePath))
            {
                IAudioPlayer audioPlayer;

                if (File.Exists(filePath))
                {
                    audioPlayer = AudioManager.Current.CreatePlayer(File.OpenRead(filePath));
                }
                else
                {
                    try
                    {
                        audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(filePath));
                    }
                    catch
                    {
                        // Fehlerbehandlung
                        return;
                    }
                }

                audioPlayer.Play();
            }
        }

        private async Task OnImportSoundClicked()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.audio" } },
                { DevicePlatform.Android, new[] { "audio/*" } },
                { DevicePlatform.WinUI, new[] { ".wav", ".mp3", ".ogg" } },
                { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
            });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Select a file"
            });

            if (result != null)
            {
                string filePath = result.FullPath;

                // Benutzer wählen lassen, welcher Button ersetzt wird
                string selectedButton = await Application.Current.MainPage.DisplayActionSheet(
                    "Select button to replace sound:",
                    "Cancel", null, SoundButtons.ToArray());

                if (!string.IsNullOrEmpty(selectedButton) && soundFiles.ContainsKey(selectedButton))
                {
                    soundFiles[selectedButton] = filePath;
                    Preferences.Set(selectedButton, filePath);

                    await Application.Current.MainPage.DisplayAlert("Success", $"Sound for {selectedButton} replaced successfully.", "OK");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
