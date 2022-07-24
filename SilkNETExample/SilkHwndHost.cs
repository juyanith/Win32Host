using Silk.NET.Core.Contexts;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl;
using System;
using System.Threading.Tasks;
using Win32Host;

namespace SilkNETExample
{
    public class SilkHwndHost : Win32HwndHost
    {
        protected unsafe override void InitializeHostedContent()
        {
            _sdlWindow = new(Hwnd);
            _renderTask = new Task(_sdlWindow.Run, TaskCreationOptions.LongRunning);
            _renderTask.Start();
        }

        protected override void ResizeHostedContent()
        {
            if (_sdlWindow is not null)
            {
                var area = this.GetScaledWindowSize();
                _sdlWindow.Resize((int)area.Width, (int)area.Height);
            }
        }

        protected override void UninitializeHostedContent()
        {
            if (_sdlWindow is not null)
            {
                _sdlWindow.Dispose();
                _sdlWindow = null;

                _renderTask?.Wait();
                _renderTask = null;
            }
        }

        private SilkSdlWindow? _sdlWindow;
        private Task? _renderTask;
    }
}
