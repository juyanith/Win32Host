using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkNETExample
{
    /// <summary>
    /// Simple renderer using Silk.NET.Sdl.
    /// </summary>
    internal unsafe class SilkSdlWindow : IDisposable
    {
        static SilkSdlWindow()
        {
            SDL = Sdl.GetApi();
            if (SDL.Init(Sdl.InitEverything) < 0)
            {
                throw new SilkSdlException("SDL_Init failed. ", true);
            }
        }

        public SilkSdlWindow(IntPtr hwnd, bool openGL = false)
        {
            Hwnd = hwnd;

            void* hwndPtr = hwnd.ToPointer();

            if (openGL)
            {
                // SDL doesn't create OpenGL context when using SDL_CreateWindowFrom.
                // See https://wiki.libsdl.org/SDL_CreateWindowFrom
                // and https://gamedev.stackexchange.com/a/119903.
                var dummy = SDL.CreateWindow(
                    "OpenGL Dummy", 0, 0, 1, 1,
                     (uint)(WindowFlags.WindowOpengl | WindowFlags.WindowHidden));
                var addrStr = new IntPtr(dummy).ToString("X");
                SDL.SetHint(Sdl.HintVideoWindowSharePixelFormat, addrStr);
                _sdlWindow = SDL.CreateWindowFrom(hwndPtr);
                SDL.SetHint(Sdl.HintVideoWindowSharePixelFormat, string.Empty);
                SDL.DestroyWindow(dummy);
            }
            else
            {
                _sdlWindow = SDL.CreateWindowFrom(hwndPtr);
            }

            if (SdlWindow == IntPtr.Zero)
            {
                throw new SilkSdlException("Failed to create child window. ", true);
            }
        }

        public IntPtr Hwnd { get; private set; }

        public IntPtr SdlWindow { get => new(_sdlWindow); }
        private Window* _sdlWindow;

        public void Resize(int width, int height)
        {
            SDL.SetWindowSize(_sdlWindow, width, height);
        }

        public void Run()
        {
            // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
            var renderer = SDL.CreateRenderer(
                _sdlWindow,
                -1,
                (uint)(RendererFlags.RendererAccelerated | RendererFlags.RendererPresentvsync));
            // This sure seems like an awkward way to test for 0...
            if (renderer == IntPtr.Zero.ToPointer())
            {
                throw new SilkSdlException("There was an issue creating the renderer. ", true);
            }

            while (true)
            {
                // Process all events
                Event e;
                while (SDL.PollEvent(&e) == 1)
                {
                    switch (e.Type)
                    {
                        case (uint)EventType.Quit:
                            return;

                            // TODO...
                    }
                }

                // Sets the color that the screen will be cleared with.
                if (SDL.SetRenderDrawColor(renderer, 135, 206, 235, 255) < 0)
                {
                    Console.WriteLine($"There was an issue with setting the render draw color. {SDL.GetErrorS()}");
                }

                // Clears the current render surface.
                if (SDL.RenderClear(renderer) < 0)
                {
                    Console.WriteLine($"There was an issue with clearing the render surface. {SDL.GetErrorS()}");
                }

                SDL.SetRenderDrawColor(renderer, 0, 0, 0, 255);
                Rectangle<int> rect = new(50, 50, 200, 200);
                SDL.RenderFillRect(renderer, ref rect);

                // Switches out the currently presented render surface with the one we just did work on.
                SDL.RenderPresent(renderer);
            }
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (SdlWindow != IntPtr.Zero)
                    {
                        SDL.DestroyWindow(_sdlWindow);
                        _sdlWindow = (Window*)IntPtr.Zero.ToPointer();
                        SDL.Quit();
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        #endregion

        private static readonly Sdl SDL;
    }
}