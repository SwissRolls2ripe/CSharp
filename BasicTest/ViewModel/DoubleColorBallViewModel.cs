using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BasicTest
{
    internal class DoubleColorBallViewModel : INotifyPropertyChanged, IDisposable
    {
        // 定义浮点数比较精度常量
        private const double ProgressComparisonPrecision = 0.01;

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    OnPropertyChanged(nameof(IsButtonEnabled));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private bool _isCancellable;
        public bool IsCancellable
        {
            get => _isCancellable;
            set
            {
                if (_isCancellable != value)
                {
                    _isCancellable = value;
                    OnPropertyChanged(nameof(IsCancellable));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                // 使用常量进行比较，提高代码可维护性
                if (Math.Abs(_progress - value) > ProgressComparisonPrecision)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        private DoubleColorBallModel _model;
        public DoubleColorBallModel Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged(nameof(Model));
                }
            }
        }

        public ICommand GenerateNumbersCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private readonly IAudioService _audioService;
        private CancellationTokenSource? _cts;

        private static readonly string[] RedBallRange = [.. Enumerable.Range(1, 33).Select(i => i.ToString("D2"))];
        private static readonly string[] BlueBallRange = [.. Enumerable.Range(1, 16).Select(i => i.ToString("D2"))];
        private const int BlueBallRangeCount = 16;

        public DoubleColorBallViewModel(IAudioService audioService)
        {
            _audioService = audioService;
            IsButtonEnabled = true;
            IsCancellable = false;
            Progress = 0;
            Model = new DoubleColorBallModel();
            GenerateNumbersCommand = new RelayCommand(async () => await GenerateNumbersAsync(), () => IsButtonEnabled && !IsCancellable);
            CancelCommand = new RelayCommand(CancelGeneration, () => IsCancellable);
        }

        private void CancelGeneration()
        {
            _cts?.Cancel();
        }

        private async Task GenerateNumbersAsync()
        {
            // 先释放旧的
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            try
            {
                IsButtonEnabled = false;
                IsCancellable = true;
                Progress = 0;

                string musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource", "Music", "抽奖.mp3");
                _audioService.PlayLoopingAudio(musicPath);

                var availableRedBalls = RedBallRange.ToArray();
                int stopIndex = 0;
                int totalBalls = 7;
                int interval = 250;
                int stopInterval = 3000;
                int elapsed = 0;
                int remainingRedBalls = 33; // 跟踪剩余可用红球数量
                Random random = new();

                while (stopIndex < totalBalls)
                {
                    _cts.Token.ThrowIfCancellationRequested();

                    for (int i = stopIndex; i < 6; i++)
                    {
                        var randomIndex = random.Next(remainingRedBalls);
                        var value = availableRedBalls[randomIndex];  // 使用 availableRedBalls 而不是 RedBallRange
                        SetRedBall(i, value);
                    }
                    SetBlueBall(BlueBallRange[random.Next(BlueBallRangeCount)]);

                    await Task.Delay(interval, _cts.Token);
                    elapsed += interval;

                    // 更新进度
                    Progress = Math.Min(100, (double)elapsed / (stopInterval * totalBalls) * 100);

                    if (elapsed % stopInterval == 0)
                    {
                        if (stopIndex < 6)
                        {
                            int index1 = Array.IndexOf(availableRedBalls, GetRedBall(stopIndex));
                            availableRedBalls[index1] = availableRedBalls[^1];
                            remainingRedBalls--;
                        }
                        stopIndex++;
                    }
                }
                Progress = 100;
            }
            catch (OperationCanceledException)
            {
                ResetBalls();
            }
            finally
            {
                _audioService.StopAudio();
                IsButtonEnabled = true;
                IsCancellable = false;
                Progress = 0;
                _cts?.Dispose();
                _cts = null;
            }
        }

        private string GetRedBall(int index)
        {
            return Model.Reds[index];
        }

        private void SetRedBall(int index, string value)
        {
            Model.Reds[index] = value;
        }

        private string GetBlueBall()
        {
            return Model.Blues[0];
        }

        private void SetBlueBall(string value)
        {
            Model.Blues[0] = value;
        }

        private void ResetBalls()
        {
            for (int i = 0; i < 6; i++)
                Model.Reds[i] = (i + 1).ToString("D2");
            Model.Blues[0] = "01";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _audioService.Dispose();
        }
    }
}
