public class TasksStatusData
{
    public int TasksRequired { get; }
    public int Team1TasksCompleted { get; }
    public int Team2TasksCompleted { get; }

    public TasksStatusData(int tasksRequired, int team1TasksCompleted, int team2TasksCompleted)
    {
        TasksRequired = tasksRequired;
        Team1TasksCompleted = team1TasksCompleted;
        Team2TasksCompleted = team2TasksCompleted;
    }
}