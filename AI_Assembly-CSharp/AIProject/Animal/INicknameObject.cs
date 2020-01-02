// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.INicknameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject.Animal
{
  public interface INicknameObject
  {
    Transform NicknameRoot { get; }

    bool NicknameEnabled { get; set; }

    string Nickname { get; }

    Action ChangeNickNameEvent { get; set; }

    int InstanceID { get; }
  }
}
