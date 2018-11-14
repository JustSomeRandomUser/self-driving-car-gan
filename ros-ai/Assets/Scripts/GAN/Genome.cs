using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN
{
    public class Genome
    {
        public List<float> Weights { get; set; }
        public float Fitness { get; set; }

        public static Genome Random(int weights)
        {
            List<float> randoms = new List<float>();

            for(int i = 0; i < weights; ++i)
                randoms.Add(UnityEngine.Random.Range(-1f, 1f));

            return new Genome(randoms);
        }

        public static List<Genome> CrossBreed(Genome a, Genome b)
        {
            int weights = a.Weights.Count;
            int crossover = UnityEngine.Random.Range(0, weights);

            List<float> x = new List<float>();
            List<float> y = new List<float>();

            for(int i = 0; i < crossover; ++i)
            {
                x.Add(a.Weights[i]);
                y.Add(b.Weights[i]);
            }

            for(int i = crossover; i < weights; ++i)
            {
                x.Add(b.Weights[i]);
                y.Add(a.Weights[i]);
            }

            List<Genome> babies = new List<Genome>();
            babies.Add(new Genome(x));
            babies.Add(new Genome(y));

            return babies;
        }

        public static List<Genome> CrossBreed(List<Genome> parents)
        {
            List<Genome> babies = new List<Genome>();

            for(int a = 0; a < parents.Count; ++a)
            {
                for(int b = a + 1; b < parents.Count; ++b)
                {
                    List<Genome> breed = CrossBreed(parents[a], parents[b]);
                    babies.Add(breed[0]);
                    babies.Add(breed[1]);
                }
            }

            return babies;
        }

        public Genome()
        {
            this.Weights = null;
            this.Fitness = 0f;
        }

        public Genome(List<float> weights) : this()
        {
            this.Weights = weights;
        }

        public Genome Mutate(float mutation, float perpetuation)
        {
            List<float> weights = new List<float>();

            foreach(float weight in this.Weights)
            {
                if(mutation > 0f && UnityEngine.Random.Range(0, 1f) <= mutation)
                    weights.Add(weight + UnityEngine.Random.Range(-1f, 1f) * perpetuation); //1/perpetuation
                else
                    weights.Add(weight);
            }

            return new Genome(weights);
        }

        public List<Genome> Fission(int size, float mutation, float perpetuation)
        {
            List<Genome> genomes = new List<Genome>();

            for(int i = 0; i < size; ++i)
                genomes.Add(Mutate(mutation, perpetuation));

            return genomes;
        }
    }
}
