using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the SampleQuizUI
 */

public class SampleQuizUI : MonoBehaviour
{
    public GameObject sampleQuizUI;
    public List<GameObject> questions;
    public Text questionNumber;
    public Button nextQuestionBtn, prevQuestionBtn;
    public void CloseSampleQuizUI()
    {
        sampleQuizUI.SetActive(false);
    }

    public void OpenSampleQuizUI()
    {
        sampleQuizUI.SetActive(true);
    }

    public void NextQuestion()
    {
        int curQuestionNumber = int.Parse(questionNumber.text);
        
        questions[curQuestionNumber].SetActive(true);
        questions[curQuestionNumber-1].SetActive(false);

        curQuestionNumber += 1;
        questionNumber.text = curQuestionNumber.ToString();

        if (curQuestionNumber == questions.Count)
        {
            nextQuestionBtn.gameObject.SetActive(false);
        }
        if (curQuestionNumber != 1)
        {
            prevQuestionBtn.gameObject.SetActive(true);
        }
    }

    public void PrevQuestion()
    {
        int curQuestionNumber = int.Parse(questionNumber.text);
        curQuestionNumber -= 1;

        questions[curQuestionNumber].SetActive(false);
        questions[curQuestionNumber - 1].SetActive(true);

        questionNumber.text = curQuestionNumber.ToString();

        if (curQuestionNumber == 1)
        {
            prevQuestionBtn.gameObject.SetActive(false);
        }
        if (curQuestionNumber != questions.Count)
        {
            nextQuestionBtn.gameObject.SetActive(true);
        }

    }
}
