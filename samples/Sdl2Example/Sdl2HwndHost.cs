using System;
using System.Threading.Tasks;
using Win32Host;

namespace Sdl2Example
{
    /// <summary>
    /// Hosts a child window created using SDL2-CS.
    /// </summary>
    public class Sdl2HwndHost : Win32HwndHost
    {
        protected override void InitializeHostedContent()
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

        private Sdl2Window? _sdlWindow;
        private Task? _renderTask;
    }
}
