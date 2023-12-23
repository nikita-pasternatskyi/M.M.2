using UnityEngine;

namespace MykroFramework.Debugging
{
    public interface IDebuggable
    {
        public string Name { get;}
        public string Description { get; }

        void Open(DebugTextBlock textBlock);

        void Close();

        void DoDebug();
    }

    public class FPSDebugger : IDebuggable
    {
        private DebugTextBlock _debugTextBlock;
        private const string TITLE = "==FPS DEBUG==";
        private const string FRAMES_NAME = "FPS: ";
        private const string CPU_FRAME_TIME_NAME = "CPU Frame Time: ";
        private const string GPU_FRAME_TIME_NAME = "GPU Frame Time: ";

        private float _fpsRefreshTime = 1;
        private float _fpsRefreshTimer;
        private int _frameCount;

        private float _frameRate;

        private FrameTiming[] _timings = new FrameTiming[1];

        string IDebuggable.Name { get => "FPS";}
        string IDebuggable.Description { get => "Shows FrameRate and FrameTime"; }

        public void DoDebug()
        {
            FrameTimingManager.CaptureFrameTimings();
            FrameTimingManager.GetLatestTimings(1, _timings);

            if (_fpsRefreshTimer < _fpsRefreshTime)
            {
                _fpsRefreshTimer += Time.deltaTime;
                _frameCount++;
            }
            else
            {
                _frameRate = (float)_frameCount / _fpsRefreshTimer;
                _frameCount = 0;
                _fpsRefreshTimer = 0;
            }
            _debugTextBlock.AddText(TITLE, DebugColors.COLOR_CYAN, true, true);

            _debugTextBlock.AddText(FRAMES_NAME, DebugColors.COLOR_WHITE, false);
            _debugTextBlock.AddText(_frameRate.ToString(), DebugColors.COLOR_GREEN, true);
            _debugTextBlock.AddLine();
            
            _debugTextBlock.AddText(CPU_FRAME_TIME_NAME, DebugColors.COLOR_WHITE, false);
            _debugTextBlock.AddText(_timings[0].cpuFrameTime.ToString(), DebugColors.COLOR_GREEN, true);
            _debugTextBlock.AddLine();

            _debugTextBlock.AddText(GPU_FRAME_TIME_NAME, DebugColors.COLOR_WHITE, false);
            _debugTextBlock.AddText(_timings[0].gpuFrameTime.ToString(), DebugColors.COLOR_GREEN, true);
            _debugTextBlock.AddLine();
        }

        public void Open(DebugTextBlock textBlock)
        {
            _debugTextBlock = textBlock;
        }

        public void Close()
        {
        }
    }
}
