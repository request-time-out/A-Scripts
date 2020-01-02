// Decompiled with JetBrains decompiler
// Type: AIProject.TrialCommonDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace AIProject
{
  public class TrialCommonDefine : SerializedScriptableObject
  {
    [SerializeField]
    private TrialCommonDefine.FileNameGroup _fileNames;

    public TrialCommonDefine()
    {
      base.\u002Ector();
    }

    public TrialCommonDefine.FileNameGroup FileNames
    {
      get
      {
        return this._fileNames;
      }
    }

    [Serializable]
    public class FileNameGroup
    {
      [SerializeField]
      private string _mainCameraName = string.Empty;

      public string MainCameraName
      {
        get
        {
          return this._mainCameraName;
        }
      }
    }
  }
}
