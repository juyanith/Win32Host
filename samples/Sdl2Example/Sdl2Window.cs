using System;

using static SDL2.SDL;

namespace Sdl2Example
{
    /// <summary>
    /// Simple renderer using SDL2.
    /// </summary>
    internal class Sdl2Window : IDisposable
    {
        public Sdl2Window(IntPtr hwnd, bool openGL = false)
        {
            Hwnd = hwnd;

            if (SDL_Init(SDL_INIT_VIDEO) < 0)
            {
                throw new Sdl2Exception("SDL_Init failed. ", true);
            }

            if (openGL)
            {
                // SDL doesn't create OpenGL context when using SDL_CreateWindowFrom.
                // See https://wiki.libsdl.org/SDL_CreateWindowFrom
                // and https://gamedev.stackexchange.com/a/119903.
                IntPtr dummy = SDL_CreateWindow(
                    "OpenGL Dummy", 0, 0, 1, 1, 
                    SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_HIDDEN);
                var addrStr = dummy.ToString("X");
                SDL_SetHint(SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT, addrStr);
                SdlWindow = SDL_CreateWindowFrom(hwnd);
                SDL_SetHint(SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT, string.Empty);
                SDL_DestroyWindow(dummy);
            }
            else
            {
                SdlWindow = SDL_CreateWindowFrom(hwnd);
            }

            if (SdlWindow == IntPtr.Zero)
            {
                throw new Sdl2Exception("Failed to create child window. ", true);
            }
        }

        public IntPtr Hwnd { get; private set; }

        public IntPtr SdlWindow { get; private set; }

        public void Resize(int width, int height)
        {
            SDL_SetWindowSize(SdlWindow, width, height);
        }

        public void Run()
        {
            // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
            var renderer = SDL_CreateRenderer(
                SdlWindow,
                -1,
                SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            if (renderer == IntPtr.Zero)
            {
                throw new Sdl2Exception("There was an issue creating the renderer. ", true);
            }

            while (true)
            {
                // Process all events
                while (SDL_PollEvent(out SDL_Event e) == 1)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            return;

                            // TODO...
                    }
                }

                // Sets the color that the screen will be cleared with.
                if (SDL_SetRenderDrawColor(renderer, 135, 206, 235, 255) < 0)
                {
                    Console.WriteLine($"There was an issue with setting the render draw color. {SDL_GetError()}");
                }

                // Clears the current render surface.
                if (SDL_RenderClear(renderer) < 0)
                {
                    Console.WriteLine($"There was an issue with clearing the render surface. {SDL_GetError()}");
                }

                SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
                var rect = new SDL_Rect()
                {
                    x = 50,
                    y = 50,
                    w = 200,
                    h = 200,
                };
                SDL_RenderFillRect(renderer, ref rect);

                // Switches out the currently presented render surface with the one we just did work on.
                SDL_RenderPresent(renderer);
            }
        }

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (SdlWindow != IntPtr.Zero)
                    {
                        SDL_DestroyWindow(SdlWindow);
                        SdlWindow = IntPtr.Zero;
                        SDL_Quit();
                    }
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}