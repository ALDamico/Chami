using System.Threading.Tasks;

namespace ChamiUI.Utils;

public static class TaskExtensions
{
    public async static void Await(this Task task)
    {
        await task;
    }
}