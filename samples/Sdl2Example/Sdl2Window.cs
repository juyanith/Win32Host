﻿using System;

using static SDL2.SDL;

namespace Sdl2Example
{
    /// <summary>
    /// Simple renderer using SDL2.
    /// </summary>
    internal class Sdl2Window : IDisposable
    {
        public Sdl2Window(IntPtr hwnd)
        {
            if (SDL_Init(SDL_INIT_VIDEO) < 0)
            {
                throw new Sdl2Exception("SDL_Init failed. ", true);
            }

            SdlWindow = SDL_CreateWindowFrom(hwnd);
            if (SdlWindow == IntPtr.Zero)
            {
                throw new Sdl2Exception("Failed to create child window. ", true);
            }

            SDL_SysWMinfo wmInfo = new();
            if (SDL_GetWindowWMInfo(SdlWindow, ref wmInfo) == SDL_bool.SDL_TRUE)
            {
                Hwnd = wmInfo.info.win.window;
            }

            //SDL_SetWindowResizable(SdlWindow, SDL_bool.SDL_TRUE);
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