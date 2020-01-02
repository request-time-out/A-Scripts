// Decompiled with JetBrains decompiler
// Type: Battlehub.Utils.CursorHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Battlehub.Utils
{
  public class CursorHelper
  {
    private readonly Dictionary<KnownCursor, Texture2D> m_knownCursorToTexture = new Dictionary<KnownCursor, Texture2D>();
    private object m_locker;

    public void Map(KnownCursor cursorType, Texture2D texture)
    {
      this.m_knownCursorToTexture[cursorType] = texture;
    }

    public void Reset()
    {
      this.m_knownCursorToTexture.Clear();
    }

    public void SetCursor(object locker, KnownCursor cursorType)
    {
      this.SetCursor(locker, cursorType, new Vector2(0.5f, 0.5f), (CursorMode) 0);
    }

    public void SetCursor(object locker, KnownCursor cursorType, Vector2 hotspot, CursorMode mode)
    {
      Texture2D texture;
      if (!this.m_knownCursorToTexture.TryGetValue(cursorType, out texture))
        texture = (Texture2D) null;
      this.SetCursor(locker, texture, hotspot, mode);
    }

    public void SetCursor(object locker, Texture2D texture)
    {
      this.SetCursor(locker, texture, new Vector2(0.5f, 0.5f), (CursorMode) 0);
    }

    public void SetCursor(object locker, Texture2D texture, Vector2 hotspot, CursorMode mode)
    {
      if (this.m_locker != null && this.m_locker != locker)
        return;
      if (Object.op_Inequality((Object) texture, (Object) null))
        ((Vector2) ref hotspot).\u002Ector((float) ((Texture) texture).get_width() * (float) hotspot.x, (float) ((Texture) texture).get_height() * (float) hotspot.y);
      this.m_locker = locker;
      Cursor.SetCursor((Texture2D) null, Vector2.get_zero(), (CursorMode) 0);
      Cursor.SetCursor(texture, hotspot, mode);
    }

    public void ResetCursor(object locker)
    {
      if (this.m_locker != locker)
        return;
      this.m_locker = (object) null;
      Cursor.SetCursor((Texture2D) null, Vector2.get_zero(), (CursorMode) 0);
    }
  }
}
