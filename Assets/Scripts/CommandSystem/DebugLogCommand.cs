using UnityEngine.Events;
public class ActionCommand : ICommand
{
    UnityAction m_action;
    UnityAction m_undoAction;

    public ActionCommand(UnityAction action, UnityAction undoAction)
    {
        m_action = action;
        m_undoAction = undoAction;
    }

    public bool Execute()
    {
        m_action.Invoke();
        return true;
    }

    public void Undo()
    {
        m_undoAction.Invoke();
    }
}

