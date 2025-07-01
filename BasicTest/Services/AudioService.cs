using System.IO;
using System.Windows.Media;

namespace BasicTest
{
    public class AudioService : IAudioService
    {
        private MediaPlayer? _player;

        public void PlayLoopingAudio(string audioPath)
        {
            try
            {
                if (!File.Exists(audioPath))
                    throw new FileNotFoundException("ÒôÆµÎÄ¼þÎ´ÕÒµ½", audioPath);

                StopAudio();
                _player = new MediaPlayer();
                _player.Open(new Uri(audioPath, UriKind.Absolute));
                _player.MediaEnded += (s, e) => { _player.Position = TimeSpan.Zero; _player.Play(); };
                _player.Play();
            }
            catch (Exception ex)
            {
                LoggerService.LogException(ex, "AudioService.PlayLoopingAudio");
                throw;
            }
        }

        public void StopAudio()
        {
            if (_player != null)
            {
                _player.Stop();
                _player.Close();
                _player = null;
            }
        }

        public void Dispose()
        {
            StopAudio();
        }
    }
}