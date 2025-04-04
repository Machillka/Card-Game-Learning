using System;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public PlayableDirector director;

    public ObjectEventSO loadMenuEvent;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnPlayableDirectorStopped;
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        loadMenuEvent.RaiseEvent(null, this);

        // 快速退出
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && director.state == PlayState.Playing)
        {
            // 快速退出
            director.Stop();
        }
    }
}
