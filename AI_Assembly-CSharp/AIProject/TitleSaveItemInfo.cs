// Decompiled with JetBrains decompiler
// Type: AIProject.TitleSaveItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using SceneAssist;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  public class TitleSaveItemInfo : MonoBehaviour
  {
    public Button btnEntry;
    public GameObject objSelect;
    public PointerEnterExitAction action;
    public SelectUI selectUI;
    [Header("セーブデータあり")]
    public GameObject objSave;
    public Text txtTitle;
    public Text txtDay;
    public Text txtTime;
    [Header("セーブデータなし")]
    public GameObject objInitialize;
    public Text txtInitialize;
    [Header("初期値")]
    public string initializeComment;
    [Header("セーブファイル情報")]
    public string path;
    public int num;
    public bool isData;

    public TitleSaveItemInfo()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (Object.op_Implicit((Object) this.txtInitialize))
        this.txtInitialize.set_text(this.initializeComment);
      if (!Object.op_Implicit((Object) this.objSelect))
        return;
      this.objSelect.SetActiveIfDifferent(false);
    }
  }
}
