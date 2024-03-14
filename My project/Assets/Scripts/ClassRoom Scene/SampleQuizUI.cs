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

    /*
     * Purpose: Close SampleQuiz UI
     * Input: Click on "X" button in SampleQuiz UI
     * Output: Close SampleQuiz UI
     */
    public void CloseSampleQuizUI()
    {
        sampleQuizUI.SetActive(false);
    }

    /*
     * Purpose: Open SampleQuiz UI
     * Input: Click on "Join Quiz" button in NetworkManager UI
     * Output: Open SampleQuiz UI
     */
    public void OpenSampleQuizUI()
    {
        sampleQuizUI.SetActive(true);
    }

    /*
     * Purpose: Go to next question in SampleQuiz UI
     * Input: Click on "Next Page" button in SampleQuiz UI
     * Output: Go to next question
     */
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

    /*
     * Purpose: Go to previous question in SampleQuiz UI
     * Input: Click on "Prev Page" button in SampleQuiz UI
     * Output: Go to previous question
     */
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
