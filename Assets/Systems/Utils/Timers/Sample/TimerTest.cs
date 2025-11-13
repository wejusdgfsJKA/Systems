using UnityEngine;
using Timers;
public class TimerTest : MonoBehaviour
{
    Timer countdown = new CountdownTimer(3),timer=new CountdownTimer(5);
    void Start()
    {
        countdown.OnTimerStart += () => Debug.Log("Timer1 started.");
        countdown.OnTimerStop += () => Debug.Log("Timer1 stopped.");
        timer.OnTimerStart += () => Debug.Log("Timer2 started.");
        timer.OnTimerStop += () => Debug.Log("Timer2 stopped.");
    }
    public bool b;
    private void Update()
    {
        if(b)
        {
            b = false;
            countdown.Start();
            timer.Start();
        }
    }

}
