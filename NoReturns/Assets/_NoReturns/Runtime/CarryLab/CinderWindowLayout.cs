using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NoReturns.CarryLab {
// Native test-player launch layout; never changes regular gameplay or automated input.
public sealed class CinderWindowLayout : MonoBehaviour {
    public static RectInt Cell(RectInt work, int count, int slot) {
        if ((count != 2 && count != 4) || slot < 0 || slot >= count)
            throw new ArgumentOutOfRangeException(nameof(slot));
        int column = slot % 2, row = count == 4 ? slot / 2 : 0;
        int left = work.x + work.width * column / 2;
        int right = work.x + work.width * (column + 1) / 2;
        int top = work.y + (count == 4 ? work.height * row / 2 : 0);
        int bottom = work.y + (count == 4 ? work.height * (row + 1) / 2 : work.height);
        return new RectInt(left, top, right-left, bottom-top);
    }
    public static RectInt Viewport(RectInt cell, float scale) {
        int title=Mathf.CeilToInt(32*scale);
        int width=cell.width-8, height=Mathf.RoundToInt(width*9f/16);
        if(height>cell.height-title-8) {height=cell.height-title-8;width=Mathf.RoundToInt(height*16f/9);}
        return new RectInt(cell.x+(cell.width-width)/2,cell.y+(cell.height-height-title)/2,width,height);
    }

#if UNITY_EDITOR || CARRY_TEST_AUTOMATION
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
    [DllImport("/usr/lib/libobjc.A.dylib")] static extern IntPtr objc_getClass(string name);
    [DllImport("/usr/lib/libobjc.A.dylib")] static extern IntPtr sel_registerName(string name);
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend")] static extern IntPtr Message(IntPtr target, IntPtr selector);
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend")] static extern double Number(IntPtr target, IntPtr selector);
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend")] static extern ulong Integer(IntPtr target, IntPtr selector);
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend")] static extern IntPtr Item(IntPtr target, IntPtr selector, ulong index);
    [StructLayout(LayoutKind.Sequential)] struct Point { public double x,y; }
    [StructLayout(LayoutKind.Sequential)] struct Frame { public double x,y,width,height; }
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend")] static extern void SetTopLeft(IntPtr target, IntPtr selector, Point point);
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend")] static extern Frame ReadFrame(IntPtr target, IntPtr selector);
    [DllImport("/usr/lib/libobjc.A.dylib", EntryPoint="objc_msgSend_stret")] static extern void ReadIntelFrame(out Frame frame, IntPtr target, IntPtr selector);
    static IntPtr NativeWindow() {
        var app=Message(objc_getClass("NSApplication"),sel_registerName("sharedApplication"));
        var windows=Message(app,sel_registerName("windows"));
        for(ulong i=0;i<Integer(windows,sel_registerName("count"));i++) {
            var window=Item(windows,sel_registerName("objectAtIndex:"),i);
            if((Integer(window,sel_registerName("styleMask"))&1)!=0 && Integer(window,sel_registerName("isVisible"))!=0) return window;
        }
        throw new InvalidOperationException("Native game window not available");
    }
    static Frame NativeFrame(IntPtr window) {
        if(RuntimeInformation.ProcessArchitecture==Architecture.Arm64) return ReadFrame(window,sel_registerName("frame"));
        ReadIntelFrame(out var frame,window,sel_registerName("frame"));return frame;
    }
    static float WindowScale() {
        var screen=Message(objc_getClass("NSScreen"),sel_registerName("mainScreen"));
        return screen==IntPtr.Zero ? 1 : (float)Number(screen,sel_registerName("backingScaleFactor"));
    }
#else
    static float WindowScale() => 1;
#endif
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Begin() {
        var args = Environment.GetCommandLineArgs();
        if (Array.IndexOf(args, "--cinder-window-count") < 0 || Array.IndexOf(args, "-nographics") >= 0) return;
        new GameObject("Native test window layout").AddComponent<CinderWindowLayout>();
    }

    static string Arg(string[] args, string key) {
        int index = Array.IndexOf(args, key);
        return index >= 0 && index + 1 < args.Length ? args[index+1] : null;
    }
    [Serializable] class Evidence {
        public int count, slot, displayWidth, displayHeight, width, height;
        public string display;
        public RectInt workArea, cell;
        public Vector2Int position;
        public float windowScale;
        public Rect nativeFrame;
    }
    IEnumerator Start() {
        var args = Environment.GetCommandLineArgs();
        int count = int.Parse(Arg(args, "--cinder-window-count"));
        int slot = int.Parse(Arg(args, "--cinder-window-slot"));
        // Allow the native window/display information to settle after startup.
        for (int frame=0; frame<10; frame++) yield return null;
        var display = Screen.mainWindowDisplayInfo;
        float scale=WindowScale();
        var work=display.workArea;
        // macOS reports workArea relative to the visible origin, but the move API
        // takes window coordinates in points. Leave space above for the menu bar.
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
        int menu=Mathf.CeilToInt(38*scale);
        work.y+=menu; work.height-=menu;
#endif
        var cell = Cell(work, count, slot);
        // Fit a landscape 16:9 game viewport inside each tile.
        var viewport=Viewport(cell,scale);
        Screen.SetResolution(viewport.width,viewport.height, FullScreenMode.Windowed);
        for (int frame=0; frame<10; frame++) yield return null;
        var position=viewport.position;
        Rect nativeFrame=default;
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
        // Unity's main-window API can return (0,0) for a background client.
        // Resolve this process's visible titled window without taking input focus.
        var window=NativeWindow();
        SetTopLeft(window,sel_registerName("setFrameTopLeftPoint:"),new Point{x=position.x/scale,y=(display.height-position.y)/scale});
        var frameRect=NativeFrame(window);
        nativeFrame=new Rect((float)frameRect.x,(float)(display.height/scale-frameRect.y-frameRect.height),(float)frameRect.width,(float)frameRect.height);
#else
        var move = Screen.MoveMainWindowTo(in display, position);
        yield return move;
#endif
        for (int frame=0; frame<10; frame++) yield return null;
        var folder = Arg(args, "--cinder-window-dir");
        if (!string.IsNullOrEmpty(folder)) {
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder,"window.json"), JsonUtility.ToJson(new Evidence {
                count=count, slot=slot, display=display.name, displayWidth=display.width, displayHeight=display.height,
                workArea=work, cell=cell, width=Screen.width, height=Screen.height, windowScale=scale,
                position=Screen.mainWindowPosition, nativeFrame=nativeFrame
            }, true));
        }
        Destroy(gameObject);
    }
#endif
}
}
