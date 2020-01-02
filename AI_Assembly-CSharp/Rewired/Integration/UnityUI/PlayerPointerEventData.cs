// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.PlayerPointerEventData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.UI;
using System.Text;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
  public class PlayerPointerEventData : PointerEventData
  {
    public PlayerPointerEventData(EventSystem eventSystem)
    {
      base.\u002Ector(eventSystem);
      this.playerId = -1;
      this.inputSourceIndex = -1;
      this.buttonIndex = -1;
    }

    public int playerId { get; set; }

    public int inputSourceIndex { get; set; }

    public IMouseInputSource mouseSource { get; set; }

    public ITouchInputSource touchSource { get; set; }

    public PointerEventType sourceType { get; set; }

    public int buttonIndex { get; set; }

    public virtual string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<b>Player Id</b>: " + (object) this.playerId);
      stringBuilder.AppendLine("<b>Mouse Source</b>: " + (object) this.mouseSource);
      stringBuilder.AppendLine("<b>Input Source Index</b>: " + (object) this.inputSourceIndex);
      stringBuilder.AppendLine("<b>Touch Source/b>: " + (object) this.touchSource);
      stringBuilder.AppendLine("<b>Source Type</b>: " + (object) this.sourceType);
      stringBuilder.AppendLine("<b>Button Index</b>: " + (object) this.buttonIndex);
      stringBuilder.Append(base.ToString());
      return stringBuilder.ToString();
    }
  }
}
