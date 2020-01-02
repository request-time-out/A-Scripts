// Decompiled with JetBrains decompiler
// Type: bl_KeyOptionsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bl_KeyOptionsUI : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField]
  private bool DetectIfKeyIsUse;
  [Header("References")]
  [SerializeField]
  private GameObject KeyOptionPrefab;
  [SerializeField]
  private Transform KeyOptionPanel;
  [SerializeField]
  private GameObject WaitKeyWindowUI;
  [SerializeField]
  private Text WaitKeyText;
  private bool WaitForKey;
  private bl_KeyInfo WaitFunctionKey;
  private List<bl_KeyInfoUI> cacheKeysInfoUI;
  private bool m_InterectableKey;

  public bl_KeyOptionsUI()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.InstanceKeysUI();
    this.WaitKeyWindowUI.SetActive(false);
  }

  private void InstanceKeysUI()
  {
    List<bl_KeyInfo> blKeyInfoList = new List<bl_KeyInfo>();
    List<bl_KeyInfo> allKeys = bl_Input.Instance.AllKeys;
    for (int index = 0; index < allKeys.Count; ++index)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.KeyOptionPrefab);
      ((bl_KeyInfoUI) gameObject.GetComponent<bl_KeyInfoUI>()).Init(allKeys[index], this);
      gameObject.get_transform().SetParent(this.KeyOptionPanel, false);
      ((Object) gameObject.get_gameObject()).set_name(allKeys[index].Function);
      this.cacheKeysInfoUI.Add((bl_KeyInfoUI) gameObject.GetComponent<bl_KeyInfoUI>());
    }
  }

  private void ClearList()
  {
    foreach (Component component in this.cacheKeysInfoUI)
      Object.Destroy((Object) component.get_gameObject());
    this.cacheKeysInfoUI.Clear();
  }

  private void Update()
  {
    if (!this.WaitForKey || !this.m_InterectableKey)
      return;
    this.DetectKey();
  }

  private void DetectKey()
  {
    foreach (KeyCode keyCode in Enum.GetValues(typeof (KeyCode)))
    {
      if (Input.GetKey(keyCode))
      {
        if (this.DetectIfKeyIsUse && bl_Input.Instance.isKeyUsed(keyCode) && keyCode != this.WaitFunctionKey.Key)
        {
          this.WaitKeyText.set_text(string.Format("KEY <b>'{0}'</b> IS ALREADY USE, \n PLEASE PRESS ANOTHER KEY FOR REPLACE <b>{1}</b>", (object) keyCode.ToString().ToUpper(), (object) this.WaitFunctionKey.Description.ToUpper()));
        }
        else
        {
          this.KeyDetected(keyCode);
          this.WaitForKey = false;
        }
      }
    }
  }

  public void SetWaitKeyProcess(bl_KeyInfo info)
  {
    if (this.WaitForKey)
      return;
    this.WaitFunctionKey = info;
    this.WaitForKey = true;
    this.WaitKeyText.set_text(string.Format("PRESS A KEY FOR REPLACE <b>{0}</b>", (object) info.Description.ToUpper()));
    this.WaitKeyWindowUI.SetActive(true);
  }

  private void KeyDetected(KeyCode KeyPressed)
  {
    if (this.WaitFunctionKey == null)
    {
      Debug.LogError((object) "Empty function waiting");
    }
    else
    {
      if (!bl_Input.Instance.SetKey(this.WaitFunctionKey.Function, KeyPressed))
        return;
      this.ClearList();
      this.InstanceKeysUI();
      this.WaitFunctionKey = (bl_KeyInfo) null;
      this.WaitKeyWindowUI.SetActive(false);
    }
  }

  public void CancelWait()
  {
    this.WaitForKey = false;
    this.WaitFunctionKey = (bl_KeyInfo) null;
    this.WaitKeyWindowUI.SetActive(false);
    this.InteractableKey = true;
  }

  public bool InteractableKey
  {
    get
    {
      return this.m_InterectableKey;
    }
    set
    {
      this.m_InterectableKey = value;
    }
  }
}
