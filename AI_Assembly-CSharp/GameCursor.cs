// Decompiled with JetBrains decompiler
// Type: GameCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine;

public class GameCursor : Singleton<GameCursor>
{
  public static Vector3 pos = Vector3.get_zero();
  public static float speed = 2000f;
  public static bool isLock = false;
  private static bool mouseLocked = false;
  private GameCursor.POINT lockPos = new GameCursor.POINT();
  private Texture2D[] atexChange = new Texture2D[1];
  private readonly string[] anameTex = new string[1]
  {
    "spanking"
  };
  private IntPtr windowPtr = GameCursor.GetForegroundWindow();
  private GameCursor.RECT winRect = new GameCursor.RECT();
  private bool GUICheckFlag;
  private const int numTex = 1;
  private const int MOUSEEVENTF_LEFTDOWN = 2;
  private const int MOUSEEVENTF_LEFTUP = 4;

  public static bool isDraw
  {
    get
    {
      return Cursor.get_visible();
    }
    set
    {
      Cursor.set_visible(value);
    }
  }

  public static bool MouseLocked
  {
    get
    {
      return GameCursor.mouseLocked;
    }
    set
    {
      GameCursor.mouseLocked = value;
      Cursor.set_visible(!value);
      Cursor.set_lockState(!Cursor.get_visible() ? (CursorLockMode) 1 : (CursorLockMode) 0);
    }
  }

  public GameCursor.CursorKind kindCursor { get; private set; }

  private void Start()
  {
    GameCursor.pos = Input.get_mousePosition();
    GameCursor.GetCursorPos(out this.lockPos);
    this.windowPtr = GameCursor.GetForegroundWindow();
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ =>
    {
      if (GameCursor.isLock)
      {
        GameCursor.SetCursorPos(this.lockPos.X, this.lockPos.Y);
      }
      else
      {
        GameCursor.pos = Input.get_mousePosition();
        GameCursor.GetCursorPos(out this.lockPos);
      }
    }));
  }

  public bool setCursor(GameCursor.CursorKind _eKind, Vector2 _vHotSpot)
  {
    Texture2D texture2D = (Texture2D) null;
    if (_eKind == GameCursor.CursorKind.Spanking)
      texture2D = this.atexChange[(int) _eKind];
    Cursor.SetCursor(texture2D, _vHotSpot, (CursorMode) 1);
    this.kindCursor = _eKind;
    return true;
  }

  [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
  private static extern void SetCursorPos(int X, int Y);

  [DllImport("USER32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetCursorPos(out GameCursor.POINT lpPoint);

  [DllImport("USER32.dll")]
  private static extern bool ScreenToClient(IntPtr hWnd, ref GameCursor.POINT lpPoint);

  [DllImport("USER32.dll")]
  private static extern bool ClientToScreen(IntPtr hWnd, ref GameCursor.POINT lpPoint);

  [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
  private static extern void mouse_event(
    int dwFlags,
    int dx,
    int dy,
    int cButtons,
    int dwExtraInfo);

  [DllImport("user32.dll")]
  private static extern int MoveWindow(
    IntPtr hwnd,
    int x,
    int y,
    int nWidth,
    int nHeight,
    int bRepaint);

  [DllImport("user32.dll")]
  private static extern int GetWindowRect(IntPtr hwnd, ref GameCursor.RECT lpRect);

  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll")]
  private static extern IntPtr FindWindow(string className, string windowName);

  [DllImport("user32.dll")]
  public static extern bool SetWindowText(IntPtr hwnd, string lpString);

  public void SetCoursorPosition(Vector3 mousePos)
  {
    GameCursor.POINT lpPoint = new GameCursor.POINT(0, 0);
    GameCursor.ClientToScreen(this.windowPtr, ref lpPoint);
    GameCursor.GetWindowRect(this.windowPtr, ref this.winRect);
    GameCursor.POINT point = new GameCursor.POINT(lpPoint.X - this.winRect.left, lpPoint.Y - this.winRect.top);
    lpPoint.X = (int) mousePos.x;
    lpPoint.Y = Screen.get_height() - (int) mousePos.y;
    GameCursor.ClientToScreen(this.windowPtr, ref lpPoint);
    GameCursor.SetCursorPos(lpPoint.X + point.X, lpPoint.Y + point.Y);
  }

  public void SetCursorLock(bool setLockFlag)
  {
    if (setLockFlag)
    {
      if (GameCursor.isLock)
        return;
      GameCursor.GetCursorPos(out this.lockPos);
      GameCursor.isLock = true;
      Cursor.set_visible(false);
    }
    else
    {
      if (!GameCursor.isLock)
        return;
      GameCursor.SetCursorPos(this.lockPos.X, this.lockPos.Y);
      GameCursor.isLock = false;
      Cursor.set_visible(true);
    }
  }

  public void UnLockCursor()
  {
    if (!GameCursor.isLock)
      return;
    GameCursor.isLock = false;
    Cursor.set_visible(true);
  }

  public void UpdateCursorLock()
  {
    if (!GameCursor.isLock)
      return;
    GameCursor.SetCursorPos(this.lockPos.X, this.lockPos.Y);
  }

  public enum CursorKind
  {
    None = -1, // 0xFFFFFFFF
    Spanking = 0,
  }

  private struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;
  }

  public struct POINT
  {
    public int X;
    public int Y;

    public POINT(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }
  }
}
