// Decompiled with JetBrains decompiler
// Type: Studio.RootButtonCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class RootButtonCtrl : MonoBehaviour
  {
    [SerializeField]
    private RootButtonCtrl.CommonInfo add;
    [SerializeField]
    private RootButtonCtrl.ManipulateInfo manipulate;
    [SerializeField]
    private RootButtonCtrl.CommonInfo sound;
    [SerializeField]
    private RootButtonCtrl.CommonInfo system;
    [SerializeField]
    private Canvas canvas;
    private RootButtonCtrl.CommonInfo[] ciArray;

    public RootButtonCtrl()
    {
      base.\u002Ector();
    }

    public ObjectCtrlInfo objectCtrlInfo
    {
      set
      {
        this.manipulate.manipulatePanelCtrl.objectCtrlInfo = value;
      }
    }

    public int select { get; private set; }

    public void OnClick(int _kind)
    {
      this.select = this.select != _kind ? _kind : -1;
      for (int index = 0; index < this.ciArray.Length; ++index)
        this.ciArray[index].active = index == this.select;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
    }

    private void Start()
    {
      this.select = -1;
      this.ciArray = new RootButtonCtrl.CommonInfo[4]
      {
        this.add,
        (RootButtonCtrl.CommonInfo) this.manipulate,
        this.sound,
        this.system
      };
      for (int index = 0; index < this.ciArray.Length; ++index)
      {
        this.ciArray[index].canvas = this.canvas;
        this.ciArray[index].active = false;
      }
    }

    [Serializable]
    private class CommonInfo
    {
      public GameObject objRoot;
      public Button button;

      public Canvas canvas { get; set; }

      public virtual bool active
      {
        set
        {
          if (this.objRoot.get_activeSelf() == value)
            return;
          this.objRoot.SetActive(value);
          this.select = value;
        }
      }

      public bool select
      {
        set
        {
          ((Graphic) ((Selectable) this.button).get_image()).set_color(!value ? Color.get_white() : Color.get_green());
          SortCanvas.select = this.canvas;
        }
      }
    }

    [Serializable]
    private class ManipulateInfo : RootButtonCtrl.CommonInfo
    {
      [SerializeField]
      private ManipulatePanelCtrl m_ManipulatePanelCtrl;

      public ManipulatePanelCtrl manipulatePanelCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_ManipulatePanelCtrl, (Object) null))
            this.m_ManipulatePanelCtrl = (ManipulatePanelCtrl) this.objRoot.GetComponent<ManipulatePanelCtrl>();
          return this.m_ManipulatePanelCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.manipulatePanelCtrl.active = value;
          this.select = value;
        }
      }
    }
  }
}
