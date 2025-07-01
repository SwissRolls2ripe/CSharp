namespace BasicTest
{
    public interface IAudioService : IDisposable
    {
        void PlayLoopingAudio(string audioPath);
        void StopAudio();
    }
}