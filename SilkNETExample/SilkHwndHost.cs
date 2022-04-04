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
            //INativeContext native;
            //var sdl = new Sdl(native);
            //var sdlContext = new SdlContext(sdl, (Window*)Hwnd, null);
            //_view = SdlWindowing.CreateFrom((void*)Hwnd, sdlContext);

            _view = SdlWindowing.CreateFrom((void*)Hwnd);
            _view.FramebufferResize += View_FramebufferResize;
            _view.Resize += View_Resize;
            _view.Render += View_Render;
            _view.Update += View_Update;

            _renderTask = new Task(_view.Run, TaskCreationOptions.LongRunning);
        }

        private void View_FramebufferResize(Vector2D<int> obj)
        {
            throw new NotImplementedException();
        }

        private void View_Update(double obj)
        {
            throw new NotImplementedException();
        }

        private void View_Render(double obj)
        {
            throw new NotImplementedException();
        }

        private void View_Resize(Silk.NET.Maths.Vector2D<int> obj)
        {
            throw new NotImplementedException();
        }

        protected override void ResizeHostedContent()
        {
            // ???
            //Silk.NET.SDL.
        }

        protected override void UninitializeHostedContent()
        {
            if (_view is not null)
            {
                _view.Dispose();
                _view = null;
            }

            if (_renderTask is not null)
            {
                _renderTask.Wait();
                _renderTask.Dispose();
                _renderTask = null;
            }
        }

        private IView? _view;
        private Task? _renderTask;
    }
}
