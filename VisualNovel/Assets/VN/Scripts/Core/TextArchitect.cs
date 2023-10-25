using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using static TextArchitect;

public class TextArchitect
{
    // UI
    private TextMeshProUGUI tmproUI;
    // 3D空間テキスト
    private TextMeshPro tmproWorld;

    public TMP_Text tmproText => tmproUI != null ? tmproUI : tmproWorld;

    public string currentText => tmproText.text;

    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int preTextLength = 0;

    public string fullTargetText => preText + targetText;

    public enum BuildMethod 
    { 
        instant, // すぐ表示
        typewriter, // 一文字づつ
        fade // フェード
    };
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color textColor { get { return tmproText.color; } set { tmproText.color = value; } }

    private const float baseSpeed = 1;
    private float speedMultiplier = 1;
    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }

    public int charaPerCycle { get { return speed <= 2f ? charaMultiplier : speed <= 2.5f ? charaMultiplier * 2 : charaMultiplier * 3; } }
    private int charaMultiplier = 1;

    public bool immediatelyText = false;

    public TextArchitect(TextMeshProUGUI ui)
    {
        tmproUI = ui;
    }

    public TextArchitect(TextMeshPro world)
    {
        tmproWorld = world;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    /// <summary>
    /// Build Text
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmproText.StartCoroutine(CoBuilding());
        return buildProcess;
    }

    /// <summary>
    /// Append text to what is already in the text architect
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Append(string text)
    {
        preText = tmproText.text;
        targetText = text;

        Stop();

        buildProcess = tmproText.StartCoroutine(CoBuilding());
        return buildProcess;
    }

    public void Stop()
    {
        if (!isBuilding)
        {
            return;
        }

        tmproText.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    private void PrepareInstant()
    {
        tmproText.color = textColor;
        tmproText.text = fullTargetText;
        tmproText.ForceMeshUpdate();
        tmproText.maxVisibleCharacters = tmproText.textInfo.characterCount;
    }

    private void PrepareTypewriter()
    {
        tmproText.color = textColor;
        tmproText.maxVisibleCharacters = 0;
        tmproText.text = preText;

        if(preText != null) 
        {
            tmproText.ForceMeshUpdate();
            tmproText.maxVisibleCharacters = tmproText.textInfo.characterCount;
        }

        tmproText.text += targetText;
        tmproText.ForceMeshUpdate();
    }

    private void PrepareFade()
    {

    }

    private void OnComplete()
    {
        buildProcess = null;
        immediatelyText = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmproText.maxVisibleCharacters = tmproText.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                break;
        }

        Stop();
        OnComplete();
    }

    IEnumerator CoBuilding()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                PrepareInstant();
                break;
            case BuildMethod.typewriter:
                PrepareTypewriter();
                yield return CoBuildTypewriter();
                break;
            case BuildMethod.fade:
                PrepareFade();
                yield return CoBuildFade();
                break;
        }

        OnComplete();
    }

    IEnumerator CoBuildTypewriter()
    {
        while(tmproText.maxVisibleCharacters < tmproText.textInfo.characterCount) 
        {
            tmproText.maxVisibleCharacters += immediatelyText ? charaPerCycle * 5 : charaPerCycle;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }

    IEnumerator CoBuildFade()
    {
        yield return null;
    }
}
