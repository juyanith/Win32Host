# Win32Host
Root implementation of HwndHost for hosting WIN32 elements. See usage and examples.

## Usage

Extend Win32HwndHost and implement the following methods:
* InitializeHostedContent - Called to create the hosted elements.
* UninitializeHostedContent - Called to destroy hosted elements.
* ResizeHostedContent - Called when the size of the window changes.

## Samples

* Sdl2Example - Hosts a child window created and rendered using SDL2 library from https://github.com/JeremySayers/Sayers.SDL2.Core
* SilkNETExample - Host a child window using Silk.NET.SDL (https://github.com/dotnet/Silk.NET/tree/main/src/Windowing/Silk.NET.SDL)
  WIP: It seems that you cannot create child windows.
* Win32Listbox - Hosts a WIN32 Listbox control and demonstrates interaction using Windows messages.
 