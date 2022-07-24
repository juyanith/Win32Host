using Silk.NET.SDL;
using System;

namespace SilkNETExample
{
    /// <summary>
    /// Exception for SDL errors.
    /// </summary>
    public class SilkSdlException : Exception
    {
        public SilkSdlException(string msg, bool sdlGetError = false)
            : base(sdlGetError ? msg + Sdl.GetApi().GetErrorS() : msg)
        {
        }
    }
}
