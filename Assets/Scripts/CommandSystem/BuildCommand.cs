using UnityEngine;

public class BuildCommand : ICommand
{
    private GameObject m_BuildObject;
    private Vector3 m_BuildPosition;
    private Quaternion m_rotation;

    GameObject m_InstantiatedObject;

    public BuildCommand(GameObject buildObject, Vector3 buildPosition, Quaternion rotation)
    {
        m_BuildObject = buildObject;
        m_BuildPosition = buildPosition;
        m_rotation = rotation;
    }

    public bool Execute()
    {
        m_InstantiatedObject = Object.Instantiate(m_BuildObject, m_BuildPosition, m_rotation);
        return m_InstantiatedObject != null;
    }

    public void Undo()
    {
        Object.Destroy(m_InstantiatedObject);
    }
}
