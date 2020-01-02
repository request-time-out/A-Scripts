// Decompiled with JetBrains decompiler
// Type: AIProject.ActionPointItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using UnityEngine;

namespace AIProject
{
  public class ActionPointItem : ActionPointComponentBase
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private GameObject[] _itemObjects;
    [SerializeField]
    private MapItemKeyValuePair[] _itemData;

    public GameObject[] ItemObjects
    {
      get
      {
        return this._itemObjects;
      }
    }

    public MapItemKeyValuePair[] ItemData
    {
      get
      {
        return this._itemData;
      }
    }

    protected override void OnStart()
    {
      this._itemObjects = ActionPointItemData.Table.get_Item(this._id);
      this._actionPoint.MapItemObjs = this._itemObjects;
      this._itemData = ActionPointItemData.Dic.get_Item(this._id);
      this._actionPoint.MapItemData = this._itemData;
    }
  }
}
