using exam_system.Features.Attempts.StartAttempt.DTOs;

namespace exam_system.Extensions;

public static class CollectionsExtensions
{
    public static ICollection<T> ShuffleList<T>(this IEnumerable<T> source)
    {
        var listToBeShuffled = source.ToList();
        int randomValue;
        T temp;

        for (int counter = listToBeShuffled.Count - 1; counter >= 0; counter--)
        {
            randomValue = Random.Shared.Next(0, counter + 1);

            temp = listToBeShuffled[counter];
            listToBeShuffled[counter] = listToBeShuffled[randomValue];
            listToBeShuffled[randomValue] = temp;
        }

        return listToBeShuffled;
    }
}
