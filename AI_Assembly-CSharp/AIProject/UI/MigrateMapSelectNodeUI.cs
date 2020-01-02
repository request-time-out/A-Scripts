// Decompiled with JetBrains decompiler
// Type: AIProject.UI.MigrateMapSelectNodeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class MigrateMapSelectNodeUI : MonoBehaviour
  {
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _text;

    public MigrateMapSelectNodeUI()
    {
      base.\u002Ector();
    }

    public Button Button
    {
      get
      {
        return this._button;
      }
    }

    public Text Text
    {
      get
      {
        return this._text;
      }
    }

    private void Reset()
    {
      this._button = (Button) ((Component) this).GetComponent<Button>();
      this._text = (Text) ((Component) this).GetComponentInChildren<Text>();
    }
  }
}
