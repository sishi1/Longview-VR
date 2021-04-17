using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState : MonoBehaviour
{
    public bool markedForInterest;
    public bool markedForConfiscate;
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
        if (markedForConfiscate)
        {
            notebook.markedObjectsForConfiscate.Remove(gameObject.name);
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
        markedForConfiscate = false;
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

        if (!markedForConfiscate)
        {
            notebook.markedObjectsForConfiscate.Add(gameObject.name);
        }

        markedForInterest = false;
        markedForConfiscate = true;
        markedForSpecialist = false;
    }

    public void MarkForSpecialist()
    {
        if (markedForInterest)
        {
            notebook.markedObjectsForInterest.Remove(gameObject.name);
        }
        else if (markedForConfiscate)
        {
            notebook.markedObjectsForConfiscate.Remove(gameObject.name);
        }

        if (!markedForSpecialist)
        {
            notebook.markedObjectsForSpecialist.Add(gameObject.name);
        }

        markedForInterest = false;
        markedForConfiscate = false;
        markedForSpecialist = true;
    }

    public void MarkForNothing()
    {
        if (markedForInterest)
        {
            notebook.markedObjectsForInterest.Remove(gameObject.name);
        }
        else if (markedForConfiscate)
        {
            notebook.markedObjectsForConfiscate.Remove(gameObject.name);
        }
        else if (markedForSpecialist)
        {
            notebook.markedObjectsForSpecialist.Remove(gameObject.name);
        }

        markedForInterest = false;
        markedForConfiscate = false;
        markedForSpecialist = false;
    }
}
