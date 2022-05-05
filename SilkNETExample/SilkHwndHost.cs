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
            // IGLContext???
            var view = SdlWindowing.CreateFrom((void*)Hwnd);
            _sdlWindow = view as IWindow;
            if (_sdlWindow is null) throw new Exception("Failed to create SDL window.");
            
            _sdlWindow.FramebufferResize += SDLWindow_FramebufferResize;
            _sdlWindow.Resize += SDLWindow_Resize;
            _sdlWindow.Render += SDLWindow_Render;
            _sdlWindow.Update += SDLWindow_Update;

            _renderTask = new Task(_sdlWindow.Run, TaskCreationOptions.LongRunning);
            _renderTask.Start();
        }

        private void SDLWindow_FramebufferResize(Vector2D<int> obj)
        {
            throw new NotImplementedException();
        }

        private void SDLWindow_Update(double obj)
        {
            // Update render state
        }

        private void SDLWindow_Render(double obj)
        {
            // Render frame
        }

        private void SDLWindow_Resize(Silk.NET.Maths.Vector2D<int> obj)
        {
            throw new NotImplementedException();
        }

        protected override void ResizeHostedContent()
        {
            if (_sdlWindow is not null)
            {
                var area = this.GetScaledWindowSize();
                _sdlWindow.Size = new Vector2D<int>((int)area.Width, (int)area.Height);
            }
        }

        protected override void UninitializeHostedContent()
        {
            if (_sdlWindow is not null)
            {
                _sdlWindow.Dispose();
                _sdlWindow = null;
            }

            if (_renderTask is not null)
            {
                _renderTask.Wait();
                _renderTask.Dispose();
                _renderTask = null;
            }
        }

        private IWindow? _sdlWindow;
        private Task? _renderTask;
    }
}
