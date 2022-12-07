using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public class Feedbacks : MonoBehaviour
    {
        public Feedback[] FeedbacksList;
        
        public virtual void PlayFeedbacks()
        {
            foreach (Feedback feedback in FeedbacksList)
            {
                feedback.PlayFeedback();
            }
        }

        public virtual void StopFeedbacks()
        {
            foreach (Feedback feedback in FeedbacksList)
            {
                feedback.StopFeedback();
            }
        }
    }

}
