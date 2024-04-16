using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCommand : ICommand
{
    GameObject m_objToDelete;
    public DeleteCommand(GameObject objToDelete)
    {
        m_objToDelete = objToDelete;
    }

    public bool Execute()
    {
        if (m_objToDelete)
        {
            m_objToDelete.SetActive(false);
            return true;
        }
        return false;
    }

    public void Undo()
    {
        m_objToDelete.SetActive(true);
    }

    ~DeleteCommand()
    {
        if(!m_objToDelete.activeInHierarchy)
            Object.Destroy(m_objToDelete);
    }
}
