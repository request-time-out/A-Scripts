// Decompiled with JetBrains decompiler
// Type: HSceneSpriteAccessoryCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using AIProject.ColorDefine;
using AIProject.SaveData;
using Manager;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HSceneSpriteAccessoryCondition : MonoBehaviour
{
  public HSceneSpriteChaChoice hSceneSpriteChaChoice;
  public HSceneSpriteToggleCategory AccessorySlots;
  public Toggle AllChange;
  public HSceneSpriteAccessoryCondition.EquipBt[] EquipBts;
  private HScene hScene;
  private ChaControl[] females;
  private ChaControl[] Males;
  private bool[] allState;
  [SerializeField]
  private HSceneSprite hSceneSprite;
  private HSceneManager hSceneManager;
  private StringBuilder sbAcsName;
  private bool[,] before;
  private bool player;

  public HSceneSpriteAccessoryCondition()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    this.hScene = (HScene) ((Component) Singleton<HSceneFlagCtrl>.Instance).GetComponent<HScene>();
    this.hSceneManager = Singleton<HSceneManager>.Instance;
    this.females = this.hScene.GetFemales();
    this.Males = this.hScene.GetMales();
    for (int index1 = 0; index1 < 4; ++index1)
    {
      ChaControl chaControl = index1 >= 2 ? this.Males[index1 - 2] : this.females[index1];
      if (!Object.op_Equality((Object) chaControl, (Object) null))
      {
        for (int index2 = 0; index2 < 4; ++index2)
          this.before[index1, index2] = !Object.op_Equality((Object) chaControl.objExtraAccessory[index2], (Object) null) && (index2 == 3 ? (index1 >= 2 ? (!Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) || !Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) chaControl) ? chaControl.objExtraAccessory[index2].get_activeSelf() : this.hSceneManager.Player.EquipedItem != null && Object.op_Inequality((Object) this.hSceneManager.Player.EquipedItem.AsGameObject, (Object) null) && this.hSceneManager.Player.EquipedItem.AsGameObject.get_activeSelf()) : (index1 != 1 || !Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) || !Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) chaControl) ? this.hSceneManager.FemaleLumpActive[index1] : this.hSceneManager.Player.EquipedItem != null && Object.op_Inequality((Object) this.hSceneManager.Player.EquipedItem.AsGameObject, (Object) null) && this.hSceneManager.Player.EquipedItem.AsGameObject.get_activeSelf())) : chaControl.objExtraAccessory[index2].get_activeSelf());
      }
    }
    this.sbAcsName = new StringBuilder();
    this.SetAccessoryCharacter(true);
    // ISSUE: method pointer
    this.hSceneSpriteChaChoice.SetAction(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
  }

  public void SetAccessoryCharacter(bool init = false)
  {
    if (!init && !((Component) this).get_gameObject().get_activeSelf())
      return;
    Color color1 = (Color) null;
    for (int index1 = 0; index1 < this.AccessorySlots.GetToggleNum(); ++index1)
    {
      this.sbAcsName.Clear();
      int index2 = index1;
      ListInfoBase listInfoBase = this.hSceneManager.numFemaleClothCustom >= 2 ? this.Males[this.hSceneManager.numFemaleClothCustom - 2].infoAccessory[index2] : this.females[this.hSceneManager.numFemaleClothCustom].infoAccessory[index2];
      UnityEngine.UI.Text componentInChildren = (UnityEngine.UI.Text) ((Component) this.AccessorySlots.lstToggle[index2]).GetComponentInChildren<UnityEngine.UI.Text>();
      if (listInfoBase != null)
      {
        this.sbAcsName.Append(listInfoBase.Name);
        ((Graphic) ((Component) this.AccessorySlots.lstToggle[index2]).GetComponent<Image>()).set_raycastTarget(true);
        if (this.hSceneManager.numFemaleClothCustom < 2)
          this.AccessorySlots.lstToggle[index2].set_isOn(this.females[this.hSceneManager.numFemaleClothCustom].fileStatus.showAccessory[index2]);
        else
          this.AccessorySlots.lstToggle[index2].set_isOn(this.Males[this.hSceneManager.numFemaleClothCustom - 2].fileStatus.showAccessory[index2]);
        if (this.AccessorySlots.lstToggle[index2].get_isOn())
        {
          Define.Set(ref color1, Colors.White, false);
          ((Graphic) componentInChildren).set_color(color1);
        }
        else
          ((Graphic) componentInChildren).set_color(Color32.op_Implicit(new Color32((byte) 141, (byte) 136, (byte) 129, byte.MaxValue)));
      }
      else
      {
        this.sbAcsName.AppendFormat("スロット{0}", (object) index2);
        ((Graphic) ((Component) this.AccessorySlots.lstToggle[index2]).GetComponent<Image>()).set_raycastTarget(false);
        ((Graphic) componentInChildren).set_color(Color32.op_Implicit(new Color32((byte) 141, (byte) 136, (byte) 129, byte.MaxValue)));
      }
      componentInChildren.set_text(this.sbAcsName.ToString());
    }
    if (this.hSceneManager.numFemaleClothCustom < 2)
      this.AccessorySlots.SetActive(Object.op_Inequality((Object) this.females[this.hSceneManager.numFemaleClothCustom].objBodyBone, (Object) null) && this.females[this.hSceneManager.numFemaleClothCustom].visibleAll, -1);
    else
      this.AccessorySlots.SetActive(Object.op_Inequality((Object) this.Males[this.hSceneManager.numFemaleClothCustom - 2].objBodyBone, (Object) null) && this.Males[this.hSceneManager.numFemaleClothCustom - 2].visibleAll, -1);
    if (this.hSceneManager.numFemaleClothCustom > 1 || Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) && Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) this.females[this.hSceneManager.numFemaleClothCustom]))
    {
      if (!Manager.Config.HData.Accessory)
        this.AllChange.set_isOn(false);
      else
        this.AllChange.set_isOn(true);
    }
    else
      this.AllChange.set_isOn(true);
    this.allState[this.hSceneManager.numFemaleClothCustom] = this.AllChange.get_isOn();
    UnityEngine.UI.Text componentInChildren1 = (UnityEngine.UI.Text) ((Component) this.AllChange).GetComponentInChildren<UnityEngine.UI.Text>();
    Color color2 = ((Graphic) componentInChildren1).get_color();
    if (this.allState[this.hSceneManager.numFemaleClothCustom])
      Define.Set(ref color2, Colors.White, false);
    else
      color2 = Color32.op_Implicit(new Color32((byte) 141, (byte) 136, (byte) 129, byte.MaxValue));
    ((Graphic) componentInChildren1).set_color(color2);
    AgentActor agentActor = (AgentActor) null;
    if (this.hSceneManager.numFemaleClothCustom < 2)
    {
      if (Object.op_Inequality((Object) this.hSceneManager.females[this.hSceneManager.numFemaleClothCustom], (Object) null))
        agentActor = (AgentActor) ((Component) this.hSceneManager.females[this.hSceneManager.numFemaleClothCustom]).GetComponent<AgentActor>();
    }
    else if (Object.op_Inequality((Object) this.hSceneManager.male, (Object) null))
      agentActor = (AgentActor) ((Component) this.hSceneManager.male).GetComponent<AgentActor>();
    AgentData agentData = !Object.op_Inequality((Object) agentActor, (Object) null) ? (AgentData) null : agentActor.AgentData;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    if (agentData != null)
    {
      this.player = false;
      flag1 = agentData.EquipedHeadItem != null && agentData.EquipedHeadItem.ID != -1;
      flag2 = agentData.EquipedBackItem != null && agentData.EquipedBackItem.ID != -1;
      flag3 = agentData.EquipedNeckItem != null && agentData.EquipedNeckItem.ID != -1;
      flag4 = agentData.EquipedLampItem != null && agentData.EquipedLampItem.ID != -1;
    }
    else
    {
      PlayerActor playerActor = (PlayerActor) null;
      if (this.hSceneManager.numFemaleClothCustom < 2)
      {
        if (Object.op_Inequality((Object) this.hSceneManager.females[this.hSceneManager.numFemaleClothCustom], (Object) null))
          playerActor = (PlayerActor) ((Component) this.hSceneManager.females[this.hSceneManager.numFemaleClothCustom]).GetComponent<PlayerActor>();
      }
      else if (Object.op_Inequality((Object) this.hSceneManager.male, (Object) null))
        playerActor = (PlayerActor) ((Component) this.hSceneManager.male).GetComponent<PlayerActor>();
      PlayerData playerData = !Object.op_Inequality((Object) playerActor, (Object) null) ? (PlayerData) null : playerActor.PlayerData;
      if (playerData != null)
      {
        this.player = true;
        flag1 = playerData.EquipedHeadItem != null && playerData.EquipedHeadItem.ID != -1;
        flag2 = playerData.EquipedBackItem != null && playerData.EquipedBackItem.ID != -1;
        flag3 = playerData.EquipedNeckItem != null && playerData.EquipedNeckItem.ID != -1;
        flag4 = playerData.EquipedLampItem != null && playerData.EquipedLampItem.ID != -1;
      }
    }
    if (this.hSceneManager.numFemaleClothCustom < 2)
    {
      this.EquipBts[0].chara = this.females[this.hSceneManager.numFemaleClothCustom];
      this.EquipBts[1].chara = this.females[this.hSceneManager.numFemaleClothCustom];
      this.EquipBts[2].chara = this.females[this.hSceneManager.numFemaleClothCustom];
      this.EquipBts[3].chara = this.females[this.hSceneManager.numFemaleClothCustom];
    }
    else
    {
      this.EquipBts[0].chara = this.Males[this.hSceneManager.numFemaleClothCustom - 2];
      this.EquipBts[1].chara = this.Males[this.hSceneManager.numFemaleClothCustom - 2];
      this.EquipBts[2].chara = this.Males[this.hSceneManager.numFemaleClothCustom - 2];
      this.EquipBts[3].chara = this.Males[this.hSceneManager.numFemaleClothCustom - 2];
    }
    this.EquipBts[0].Base.SetActive(flag1);
    this.EquipBts[1].Base.SetActive(flag2);
    this.EquipBts[2].Base.SetActive(flag3);
    this.EquipBts[3].Base.SetActive(flag4);
    this.EquipBts[0].id = 0;
    this.EquipBts[1].id = 1;
    this.EquipBts[2].id = 2;
    this.EquipBts[3].id = 3;
    if (Object.op_Inequality((Object) this.EquipBts[0].chara, (Object) null) && Object.op_Inequality((Object) this.EquipBts[0].chara.objExtraAccessory[0], (Object) null) && this.EquipBts[0].chara.objExtraAccessory[0].get_activeSelf() != this.EquipBts[0].active)
      this.EquipBts[0].ChangeState();
    if (Object.op_Inequality((Object) this.EquipBts[1].chara, (Object) null) && Object.op_Inequality((Object) this.EquipBts[1].chara.objExtraAccessory[1], (Object) null) && this.EquipBts[1].chara.objExtraAccessory[1].get_activeSelf() != this.EquipBts[1].active)
      this.EquipBts[1].ChangeState();
    if (Object.op_Inequality((Object) this.EquipBts[2].chara, (Object) null) && Object.op_Inequality((Object) this.EquipBts[2].chara.objExtraAccessory[2], (Object) null) && this.EquipBts[2].chara.objExtraAccessory[2].get_activeSelf() != this.EquipBts[2].active)
      this.EquipBts[2].ChangeState();
    if (Object.op_Inequality((Object) this.EquipBts[3].chara, (Object) null))
    {
      GameObject gameObject = (GameObject) null;
      if (!this.player)
        gameObject = this.EquipBts[3].chara.objExtraAccessory[3];
      else if (this.hSceneManager.numFemaleClothCustom < 2)
      {
        if (this.hSceneManager.females[this.hSceneManager.numFemaleClothCustom].EquipedItem != null)
          gameObject = this.hSceneManager.females[this.hSceneManager.numFemaleClothCustom].EquipedItem.AsGameObject;
      }
      else if (this.hSceneManager.male.EquipedItem != null)
        gameObject = this.hSceneManager.male.EquipedItem.AsGameObject;
      if (Object.op_Inequality((Object) gameObject, (Object) null) && gameObject.get_activeSelf() != this.EquipBts[3].active)
        this.EquipBts[3].ChangeState();
    }
    if (!this.hSceneSpriteChaChoice.Content.get_activeSelf())
      return;
    this.hSceneSpriteChaChoice.Content.SetActive(false);
  }

  public void OnClickAccessory(int _accessory)
  {
    if (Singleton<Scene>.Instance.IsNowLoading || Singleton<Scene>.Instance.IsNowLoadingFade || (this.hSceneSprite.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    for (int index1 = 0; index1 < this.AccessorySlots.GetToggleNum(); ++index1)
    {
      int index2 = index1;
      if (index2 == _accessory)
      {
        UnityEngine.UI.Text componentInChildren = (UnityEngine.UI.Text) ((Component) this.AccessorySlots.lstToggle[index2]).GetComponentInChildren<UnityEngine.UI.Text>();
        Color color = ((Graphic) componentInChildren).get_color();
        if ((this.hSceneManager.numFemaleClothCustom > 1 || Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) && Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) this.females[this.hSceneManager.numFemaleClothCustom])) && !Manager.Config.HData.Accessory)
          this.AccessorySlots.lstToggle[_accessory].set_isOn(false);
        if (this.AccessorySlots.lstToggle[_accessory].get_isOn())
          Define.Set(ref color, Colors.White, false);
        else
          color = Color32.op_Implicit(new Color32((byte) 141, (byte) 136, (byte) 129, byte.MaxValue));
        ((Graphic) componentInChildren).set_color(color);
        break;
      }
    }
    if (this.hSceneManager.numFemaleClothCustom < 2)
      this.females[this.hSceneManager.numFemaleClothCustom].SetAccessoryState(_accessory, this.AccessorySlots.lstToggle[_accessory].get_isOn());
    else
      this.Males[this.hSceneManager.numFemaleClothCustom - 2].SetAccessoryState(_accessory, this.AccessorySlots.lstToggle[_accessory].get_isOn());
  }

  public void OnClickAllAccessory()
  {
    if (Singleton<Scene>.Instance.IsNowLoading || Singleton<Scene>.Instance.IsNowLoadingFade || (this.hSceneSprite.isFade || !this.hSceneManager.HSceneUISet.get_activeSelf()))
      return;
    if ((this.hSceneManager.numFemaleClothCustom > 1 || Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) && Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) this.females[this.hSceneManager.numFemaleClothCustom])) && !Manager.Config.HData.Accessory)
      this.AllChange.set_isOn(false);
    this.allState[this.hSceneManager.numFemaleClothCustom] = this.AllChange.get_isOn();
    for (int _array = 0; _array < this.AccessorySlots.lstToggle.Count; ++_array)
    {
      if (!GlobalMethod.StartsWith(((UnityEngine.UI.Text) ((Component) this.AccessorySlots.lstToggle[_array]).GetComponentInChildren<UnityEngine.UI.Text>()).get_text(), "スロット"))
        this.AccessorySlots.SetCheck(this.AllChange.get_isOn(), _array);
    }
    UnityEngine.UI.Text componentInChildren = (UnityEngine.UI.Text) ((Component) this.AllChange).GetComponentInChildren<UnityEngine.UI.Text>();
    Color color = ((Graphic) componentInChildren).get_color();
    if (this.allState[this.hSceneManager.numFemaleClothCustom])
      Define.Set(ref color, Colors.White, false);
    else
      color = Color32.op_Implicit(new Color32((byte) 141, (byte) 136, (byte) 129, byte.MaxValue));
    ((Graphic) componentInChildren).set_color(color);
    if (this.hSceneManager.numFemaleClothCustom < 2)
      this.females[this.hSceneManager.numFemaleClothCustom].SetAccessoryStateAll(this.allState[this.hSceneManager.numFemaleClothCustom]);
    else
      this.Males[this.hSceneManager.numFemaleClothCustom - 2].SetAccessoryStateAll(this.allState[this.hSceneManager.numFemaleClothCustom]);
  }

  public void EndProc()
  {
    for (int index1 = 0; index1 < 4; ++index1)
    {
      ChaControl chaControl = index1 >= 2 ? this.Males[index1 - 2] : this.females[index1];
      if (!Object.op_Equality((Object) chaControl, (Object) null))
      {
        for (int index2 = 0; index2 < 4; ++index2)
          chaControl.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) index2, true);
      }
    }
  }

  public void ChangeState(int id)
  {
    this.EquipBts[id].ChangeState();
  }

  public void ChangeStateAllEquip(bool val)
  {
    if (Object.op_Equality((Object) this.hSceneManager.Player, (Object) null))
      return;
    for (int index = 0; index < 4; ++index)
    {
      if (index != 3)
      {
        this.hSceneManager.Player.ChaControl.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) index, val);
      }
      else
      {
        this.hSceneManager.Player.ChaControl.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) index, val);
        if (this.hSceneManager.Player.EquipedItem != null && Object.op_Inequality((Object) this.hSceneManager.Player.EquipedItem.AsGameObject, (Object) null))
          this.hSceneManager.Player.EquipedItem.AsGameObject.SetActive(val);
      }
    }
  }

  [Serializable]
  public class EquipBt
  {
    public int id = -1;
    public GameObject Base;
    public Button On;
    public Button Off;
    public ChaControl chara;
    public bool active;

    public void ChangeState()
    {
      if (Object.op_Equality((Object) this.chara, (Object) null))
        return;
      HSceneManager instance = Singleton<HSceneManager>.Instance;
      if (this.id != 3)
      {
        if (this.active)
          this.chara.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) this.id, false);
        else
          this.chara.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) this.id, true);
      }
      else if (this.active)
      {
        this.chara.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) this.id, false);
        if (Object.op_Inequality((Object) instance.Player, (Object) null) && Object.op_Equality((Object) instance.Player.ChaControl, (Object) this.chara) && (instance.Player.EquipedItem != null && Object.op_Inequality((Object) instance.Player.EquipedItem.AsGameObject, (Object) null)))
          instance.Player.EquipedItem.AsGameObject.SetActive(false);
      }
      else
      {
        this.chara.ShowExtraAccessory((ChaControlDefine.ExtraAccessoryParts) this.id, true);
        if (Object.op_Inequality((Object) instance.Player, (Object) null) && Object.op_Equality((Object) instance.Player.ChaControl, (Object) this.chara) && (instance.Player.EquipedItem != null && Object.op_Inequality((Object) instance.Player.EquipedItem.AsGameObject, (Object) null)))
          instance.Player.EquipedItem.AsGameObject.SetActive(true);
      }
      ((Component) this.On).get_gameObject().SetActive(!this.active);
      ((Component) this.Off).get_gameObject().SetActive(this.active);
      HSceneSpriteAccessoryCondition.EquipBt equipBt = this;
      equipBt.active = ((equipBt.active ? 1 : 0) ^ 1) != 0;
    }
  }
}
