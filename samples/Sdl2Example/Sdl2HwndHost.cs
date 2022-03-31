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
            sdlWindow = new(Hwnd);
            _mapWindowTask = new Task(sdlWindow.Run, TaskCreationOptions.LongRunning);
            _mapWindowTask.Start();
        }

        protected override void ResizeHostedContent()
        {
            if (sdlWindow is not null)
            {
                var area = this.GetScaledWindowSize();
                sdlWindow.Resize((int)area.Width, (int)area.Height);
            }
        }

        protected override void UninitializeHostedContent()
        {
            if (sdlWindow is not null)
            {
                sdlWindow.Dispose();
                sdlWindow = null;
                _mapWindowTask?.Wait();
                _mapWindowTask = null;
            }
        }

        private Sdl2Window? sdlWindow;
        private Task? _mapWindowTask;
    }
}
