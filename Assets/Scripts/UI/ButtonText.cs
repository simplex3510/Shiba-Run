using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonWithText : Button
{
    private TextMeshProUGUI buttonText;

    private Color originalTextColor;

    protected override void Awake()
    {
        base.Awake();

        image = GetComponentInChildren<Image>();

        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // 런타임 초기화 시점에서 원본 색상 저장
        if (buttonText != null)
        {
            originalTextColor = buttonText.color;
        }
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (targetGraphic == null || buttonText == null)
            return;

        if (base.gameObject.activeInHierarchy)
        {
            /// 버튼의 그래픽 색상과 동일하게 텍스트 색상 설정
            buttonText.color = targetGraphic.color * originalTextColor;
        }
        
    }
}
