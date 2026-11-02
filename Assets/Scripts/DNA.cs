using System;
using UnityEngine;

// Two integer genes. Each gene encodes: 0 = forward, 1 = left, 2 = right.
[Serializable]
public sealed class DNA
{
    private readonly int[] genes;

    public DNA(int groundAction, int edgeAction)
    {
        genes = new[] { CheckAction(groundAction), CheckAction(edgeAction) };
    }

    public int GetGene(int index)
    {
        if (index < 0 || index >= genes.Length) throw new ArgumentOutOfRangeException(nameof(index));
        return genes[index];
    }

    public DNA Copy() => new DNA(genes[0], genes[1]);

    public static DNA RandomGenome() => new DNA(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3));

    public static DNA Cross(DNA parentA, DNA parentB)
    {
        if (parentA == null || parentB == null) throw new ArgumentNullException("parent");

        int gene0, gene1;

        int cutPoint = UnityEngine.Random.Range(0, 2);

        if (cutPoint == 0)
        {
            gene0 = parentB.GetGene(0);
            gene1 = parentA.GetGene(1);
        }
        else
        {
            gene0 = parentA.GetGene(0);
            gene1 = parentB.GetGene(1);
        }
        return new DNA(gene0, gene1);
    }

    public void Mutate(float probabilityPerGene)
    {
        float p = Mathf.Clamp01(probabilityPerGene);
        for (int i = 0; i < genes.Length; i++)
        {
            if (UnityEngine.Random.value >= p) continue;

            int oldValue = genes[i];
            int newValue;

            do
            {
                newValue = UnityEngine.Random.Range(0, 3);
            } while (newValue == oldValue);

            genes[i] = newValue;
        }
    }

    private static int CheckAction(int action)
    {
        if (action < 0 || action > 2) throw new ArgumentOutOfRangeException(nameof(action));
        return action;
    }

    public override string ToString() => "[" + genes[0] + ", " + genes[1] + "]";
}
