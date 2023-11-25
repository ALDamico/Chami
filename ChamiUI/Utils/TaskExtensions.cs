using System.Threading.Tasks;

namespace ChamiUI.Utils;

public static class TaskExtensions
{
    public static async void Await(this Task task)
    {
        await task;
    }
}