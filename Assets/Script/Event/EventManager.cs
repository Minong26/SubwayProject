using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Event
{
    [Header("Progress")]
    public string eventName;
    public float[] progressDuration;
    
    [Header("values")]
    public float moveDuration;
    public Transform startPos;

}

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    public Event[] Event_List;

    [Header("Prefabs")]
    public GameObject e_CatMonster, e_RatMonster;
    public GameObject deathObject;
    
    [Header("Scripts")]
    public CanvasManager theCanvasManager;
    public PlayerManager thePlayerManager;
    public MonsterSpawner theMonsterSpawner;

    // Start is called before the first frame update
    void Start()
    {
        thePlayerManager.PlayEvent(0);
    }

    public void _Event0(string type)
    {
        if (type == "Start")
        {
            theCanvasManager.FadeImageEvent();
            thePlayerManager.LookFront();
        }   
    }

    public void _Event1()
    {
        StartCoroutine("Event1Coroutine");
    }
    
    IEnumerator Event1Coroutine()
    {
        thePlayerManager.ControlMove(false, false);
        thePlayerManager.LerpRotation(Vector3.zero, 1f);

        yield return new WaitForSeconds(Event_List[0].progressDuration[0]);

        thePlayerManager.LookFront();
        thePlayerManager.ControlMove(false, true);

        e_CatMonster.transform.position = Event_List[0].startPos.position;
        e_CatMonster.SetActive(true);

        yield return new WaitForSeconds(Event_List[0].progressDuration[1]);

        thePlayerManager.ControlMove(true, true);

        yield return new WaitUntil(() =>  thePlayerManager.isHiding);

        Debug.Log("NICE");

        yield return new WaitForSeconds(Event_List[0].progressDuration[2]);

        e_CatMonster.SetActive(false);
        Debug.Log("Monster Is Gone");
        GameManager.instance.StartMonsterSpawn();
    }

    public void DeathEvent(bool type)
    {
        thePlayerManager.gameObject.SetActive(!type);
        if (type)
        {
            deathObject.SetActive(true);
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            deathObject.SetActive(false);
        }
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(Event_List[1].progressDuration[0]);
        
        DeathEvent(false);
    }
}
