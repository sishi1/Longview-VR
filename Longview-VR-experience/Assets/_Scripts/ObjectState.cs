using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState : MonoBehaviour
{
    public bool markedForInterest;
    public bool markedForTaking;
    public bool markedForSpecialist;

    public bool shouldBeMarked;

    private Notebook notebook;
    //private TrainingSystem trainingSystem;

    private bool isFound;

    void Start()
    {
        notebook = FindObjectOfType<Notebook>();

        //if (shouldBeMarked)
        //{
        //    trainingSystem = GameObject.FindWithTag("Player").GetComponentInChildren<TrainingSystem>();
        //    trainingSystem.shouldBeMarkedList.Add(gameObject.name);
        //}
    }

    void Update()
    {
        //if (shouldBeMarked && (markedForTaking || markedForSpecialist))
        //{
        //    if (!isFound)
        //    {
        //        isFound = true;
        //        trainingSystem.shouldBeMarkedList.Remove(gameObject.name);
        //    }
        //}
        //else if (shouldBeMarked && !markedForTaking && !markedForSpecialist)
        //{
        //    if (isFound)
        //    {
        //        isFound = false;
        //        trainingSystem.shouldBeMarkedList.Add(gameObject.name);
        //    }
        //}
    }

    public void MarkForInterest()
    {
        if (markedForTaking)
        {
            notebook.markedObjectsForTaking.Remove(gameObject.name);
        }
        else if (markedForSpecialist)
        {
            notebook.markedObjectsForSpecialist.Remove(gameObject.name);
        }

        if (!markedForInterest)
        {
            notebook.markedObjectsForInterest.Add(gameObject.name);
        }

        markedForInterest = true;
        markedForTaking = false;
        markedForSpecialist = false;
    }

    public void MarkForConfiscate()
    {
        if (markedForInterest)
        {
            notebook.markedObjectsForInterest.Remove(gameObject.name);
        }
        else if (markedForSpecialist)
        {
            notebook.markedObjectsForSpecialist.Remove(gameObject.name);
        }

        if (!markedForTaking)
        {
            notebook.markedObjectsForTaking.Add(gameObject.name);
        }

        markedForInterest = false;
        markedForTaking = true;
        markedForSpecialist = false;
    }

    public void MarkForSpecialist()
    {
        if (markedForInterest)
        {
            notebook.markedObjectsForInterest.Remove(gameObject.name);
        }
        else if (markedForTaking)
        {
            notebook.markedObjectsForTaking.Remove(gameObject.name);
        }

        if (!markedForSpecialist)
        {
            notebook.markedObjectsForSpecialist.Add(gameObject.name);
        }

        markedForInterest = false;
        markedForTaking = false;
        markedForSpecialist = true;
    }

    public void MarkForNothing()
    {
        if (markedForInterest)
        {
            notebook.markedObjectsForInterest.Remove(gameObject.name);
        }
        else if (markedForTaking)
        {
            notebook.markedObjectsForTaking.Remove(gameObject.name);
        }
        else if (markedForSpecialist)
        {
            notebook.markedObjectsForSpecialist.Remove(gameObject.name);
        }

        markedForInterest = false;
        markedForTaking = false;
        markedForSpecialist = false;
    }
}
