using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteEvent : MonoBehaviour
{
    // From Helium's answer at https://answers.unity.com/questions/1519824/undesirable-call-of-toggles-on-value-changed-when.html

    public static void Mute(UnityEngine.Events.UnityEventBase ev)
    {
        int count = ev.GetPersistentEventCount();
        for (int i = 0; i < count; i++)
        {
            ev.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
        }
    }

    public static void Unmute(UnityEngine.Events.UnityEventBase ev)
    {
        int count = ev.GetPersistentEventCount();
        for (int i = 0; i < count; i++)
        {
            ev.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }
    }
}
