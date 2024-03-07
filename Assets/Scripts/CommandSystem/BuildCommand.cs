using UnityEngine;

public class BuildCommand : ICommand
{
    private GameObject m_BuildObject;
    private Vector3 m_BuildPosition;

    GameObject m_InstantiatedObject;

    public BuildCommand(GameObject buildObject, Vector3 buildPosition)
    {
        m_BuildObject = buildObject;
        m_BuildPosition = buildPosition;
    }

    public bool Execute()
    {
        m_InstantiatedObject = Object.Instantiate(m_BuildObject, m_BuildPosition, Quaternion.identity);
        return m_InstantiatedObject != null;
    }

    public void Undo()
    {
        Object.Destroy(m_InstantiatedObject);
    }
}
