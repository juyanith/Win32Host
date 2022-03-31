using System;

using static SDL2.SDL;

namespace Sdl2Example
{
    /// <summary>
    /// Exception for SDL2 errors.
    /// </summary>
    public class Sdl2Exception : Exception
    {
        public Sdl2Exception(string msg, bool sdlGetError = false)
            : base(sdlGetError ? msg + SDL_GetError() : msg)
        {
        }
    }
}
