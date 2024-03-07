using System.Collections.Generic;

[System.Serializable]
public class CommandConsumer
{
    private LinkedList<ICommand> m_UndoStack;
    private LinkedList<ICommand> m_RedoStack;

    private int m_MaxUndos;

    public int UndoCount
    {
        get { return m_UndoStack.Count; }
    }

    private bool IsStackFull
    {
        get
        {
            return m_MaxUndos != 0 && m_UndoStack.Count >= m_MaxUndos;
        }
    }

    public CommandConsumer(int maxUndos = 0)
    {

        m_UndoStack = new LinkedList<ICommand>();
        m_RedoStack = new LinkedList<ICommand>();
        m_MaxUndos = maxUndos;
    }

    private bool ExecuteCommand(ICommand command, bool redo)
    {
        if (command.Execute())
        {
            if (IsStackFull)
            {
                m_UndoStack.RemoveFirst();
            }
            m_UndoStack.AddLast(command);
            if(!redo) m_RedoStack.Clear();
        }
        return true;
    }

    public bool ExecuteCommand(ICommand command)
    {
        return ExecuteCommand(command, false);
    }

    public void UndoLastCommand()
    {
        ICommand undoCommand = m_UndoStack.Last?.Value;
        if (undoCommand == null) return;

        m_UndoStack.RemoveLast();
        undoCommand.Undo();
        m_RedoStack.AddLast(undoCommand);
    }

    public bool RedoLastUndo()
    {
        ICommand redoCommand = m_RedoStack.Last?.Value;
        if (redoCommand == null) return false;
        m_RedoStack.RemoveLast();
        return ExecuteCommand(redoCommand, true);
    }
}
