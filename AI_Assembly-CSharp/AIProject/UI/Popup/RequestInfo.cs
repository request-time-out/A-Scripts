// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Popup.RequestInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject.UI.Popup
{
  public class RequestInfo
  {
    public RequestInfo(int _type, Tuple<int, int, int>[] _items)
    {
      this.Type = _type;
      this.Items = _items;
    }

    public string[] Title { get; private set; }

    public int Type { get; private set; }

    public string[] Message { get; private set; }

    public Tuple<int, int, int>[] Items { get; private set; }

    public void SetText(string[] _title, string[] _message)
    {
      this.Title = _title;
      this.Message = _message;
      if (this.Type == 1 && this.Message == null)
        this.Message = new string[2]
        {
          string.Empty,
          string.Empty
        };
      if (this.Type != 0 || this.Items != null)
        return;
      this.Items = new Tuple<int, int, int>[0];
    }
  }
}
