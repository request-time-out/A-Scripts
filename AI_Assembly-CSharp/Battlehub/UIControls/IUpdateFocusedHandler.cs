// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.IUpdateFocusedHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public interface IUpdateFocusedHandler : IEventSystemHandler
  {
    void OnUpdateFocused(BaseEventData eventData);
  }
}
