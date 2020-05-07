using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FamilyNeedBar : MonoBehaviour
{
    public Slider slider;

    public float MaxValue = 100;
    public FamilyEnum familyMember;
    private Family familyScript;
    public Sprite[] faces;
    public Image faceIcon;

    private void Start()
    {
        slider.value = MaxValue;
        GameObject go = GameObject.FindGameObjectWithTag("GameManager");
        familyScript = go.GetComponent<Family>();
        StartCoroutine(UpdateFamilyMemberStatsUI());
    }

    private IEnumerator UpdateFamilyMemberStatsUI()
    {

        yield return new WaitForSeconds(2F);
        for (; ; )
        {

            FamilyMember familyStats = familyScript.familyMembers.FirstOrDefault(x => x.Name.Contains(familyMember.ToString()));
            float startingValue = familyStats.StartingSatisfaction;
            float currentValue = familyStats.CurrentSatisfactionLevel;

            if (currentValue / startingValue * 100 >= 80)
            {
                faceIcon.sprite = faces[0];
            }
            else if (currentValue / startingValue * 100 >= 60)
            {
                faceIcon.sprite = faces[1];
            }
            else if (currentValue / startingValue * 100 >= 40)
            {
                faceIcon.sprite = faces[2];
            }
            else if (currentValue / startingValue * 100 >= 20)
            {
                faceIcon.sprite = faces[3];
            }
            else if (currentValue / startingValue * 100 >= 10)
            {
                faceIcon.sprite = faces[4];
            }
            else if (currentValue / startingValue * 100 >= 0)
            {
                faceIcon.sprite = faces[5];
            }

            float sliderValue = currentValue / startingValue;
            SetBarValue(sliderValue * 100);
            yield return new WaitForSeconds(1F);
        }
    }

    public void LowerMood(float moodTaken)
    {
        slider.value -= moodTaken;
        SetBarValue(slider.value);
    }

    public void SetBarValue(float newValue)
    {
        slider.value = newValue;
    }

    public void SetMaxBarValue(float newMaxValue)
    {
        slider.maxValue = newMaxValue;
        slider.value = newMaxValue;
    }
}
public enum FamilyEnum { Björn, Steven, Nesterly, Florence, Uzbecheal }