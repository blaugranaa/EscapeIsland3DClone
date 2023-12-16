using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public GameObject SuccesPanel;

  private void Awake()
  {
    SuccesPanel.SetActive(false);
  }

  private void OnEnable()
  {
    EventManager.AddListener(GameEvent.OnLevelEnd,OpenSuccessUI);
  }

  private void OnDisable()
  {
    EventManager.RemoveListener(GameEvent.OnLevelEnd,OpenSuccessUI);

  }

  void OpenSuccessUI()
  {
    SuccesPanel.SetActive(true);
  }
}
