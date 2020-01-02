// Decompiled with JetBrains decompiler
// Type: HSceneSpriteClothCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HSceneSpriteClothCondition : HSceneSpriteCategory
{
  public List<HSceneSpriteClothBtn> objs = new List<HSceneSpriteClothBtn>();
  private int[] allState = new int[2];
  public HSceneSpriteChaChoice hSceneSpriteChaChoice;
  public Button[] AllChange;
  private HScene hScene;
  private ChaControl[] females;
  [SerializeField]
  private HSceneSprite hSceneSprite;
  private HSceneManager hSceneManager;

  public void Init()
  {
    this.hScene = (HScene) ((Component) Singleton<HSceneFlagCtrl>.Instance).GetComponent<HScene>();
    this.hSceneManager = Singleton<HSceneManager>.Instance;
    this.females = this.hScene.GetFemales();
    this.SetClothCharacter(true);
    // ISSUE: method pointer
    this.hSceneSpriteChaChoice.SetAction(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
  }

  public void SetClothCharacter(bool init = false)
  {
    if (!((Component) this).get_gameObject().get_activeSelf() && !init)
      return;
    if (Object.op_Inequality((Object) this.females[this.hSceneManager.numFemaleClothCustom].objBodyBone, (Object) null) && this.females[this.hSceneManager.numFemaleClothCustom].visibleAll)
    {
      int num = -1;
      bool flag = true;
      for (int index = 0; index < 8; ++index)
      {
        this.objs[index].SetButton((int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index]);
        bool _active = this.females[this.hSceneManager.numFemaleClothCustom].IsClothesStateKind(index);
        this.SetActive(_active, index);
        if (_active && flag)
        {
          if (num < 0)
            num = (int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index];
          else
            flag = num == (int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index];
        }
      }
      if (!flag)
        this.allState[this.hSceneManager.numFemaleClothCustom] = 1;
      else if (num >= 0)
      {
        this.allState[this.hSceneManager.numFemaleClothCustom] = num;
        this.allState[this.hSceneManager.numFemaleClothCustom] %= 3;
      }
      for (int index = 0; index < this.AllChange.Length; ++index)
      {
        if (this.allState[this.hSceneManager.numFemaleClothCustom] != index)
          ((Component) this.AllChange[index]).get_gameObject().SetActive(false);
        else
          ((Component) this.AllChange[index]).get_gameObject().SetActive(true);
      }
    }
    if (!this.hSceneSpriteChaChoice.Content.get_activeSelf())
      return;
    this.hSceneSpriteChaChoice.Content.SetActive(false);
  }

  public void OnClickCloth(int _cloth)
  {
    if (Singleton<Scene>.Instance.IsNowLoading || Singleton<Scene>.Instance.IsNowLoadingFade || (this.hSceneSprite.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()) || Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) this.females[this.hSceneManager.numFemaleClothCustom]) && !Manager.Config.HData.Cloth)
      return;
    this.females[this.hSceneManager.numFemaleClothCustom].SetClothesStateNext(_cloth);
    int num = -1;
    bool flag = true;
    for (int index = 0; index < 8; ++index)
    {
      if (((Component) this.objs[index]).get_gameObject().get_activeSelf())
      {
        this.objs[index].SetButton((int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index]);
        if (flag)
        {
          if (num < 0)
            num = (int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index];
          else
            flag = num == (int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index];
        }
      }
    }
    if (!flag)
      this.allState[this.hSceneManager.numFemaleClothCustom] = 1;
    else if (num >= 0)
    {
      this.allState[this.hSceneManager.numFemaleClothCustom] = num;
      this.allState[this.hSceneManager.numFemaleClothCustom] %= 3;
    }
    for (int index = 0; index < this.AllChange.Length; ++index)
    {
      if (this.allState[this.hSceneManager.numFemaleClothCustom] != index)
        ((Component) this.AllChange[index]).get_gameObject().SetActive(false);
      else
        ((Component) this.AllChange[index]).get_gameObject().SetActive(true);
    }
  }

  public void OnClickAllCloth()
  {
    if (Singleton<Scene>.Instance.IsNowLoading || Singleton<Scene>.Instance.IsNowLoadingFade || (this.hSceneSprite.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    ++this.allState[this.hSceneManager.numFemaleClothCustom];
    this.allState[this.hSceneManager.numFemaleClothCustom] %= 3;
    this.females[this.hSceneManager.numFemaleClothCustom].SetClothesStateAll((byte) this.allState[this.hSceneManager.numFemaleClothCustom]);
    for (int index = 0; index < this.AllChange.Length; ++index)
    {
      if (this.allState[this.hSceneManager.numFemaleClothCustom] != index)
        ((Component) this.AllChange[index]).get_gameObject().SetActive(false);
      else
        ((Component) this.AllChange[index]).get_gameObject().SetActive(true);
    }
    for (int index = 0; index < 8; ++index)
      this.objs[index].SetButton((int) this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.clothesState[index]);
  }

  public override void SetActive(bool _active, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.objs.Count; ++index)
      {
        if (((Component) this.objs[index]).get_gameObject().get_activeSelf() != _active)
          ((Component) this.objs[index]).get_gameObject().SetActive(_active);
      }
    }
    else
    {
      if (this.objs.Count <= _array || ((Component) this.objs[_array]).get_gameObject().get_activeSelf() == _active)
        return;
      ((Component) this.objs[_array]).get_gameObject().SetActive(_active);
    }
  }
}
