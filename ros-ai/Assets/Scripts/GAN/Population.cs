using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN
{
    public class Population
    {
        public List<Genome> Genomes { get; set; }
        public float Growth { get; set; }
        public int Size => this.Genomes.Count;

        public Population()
        {
            this.Genomes = null;
            this.Growth = 0f;
        }

        public Population(List<Genome> genomes) : this()
        {
            this.Genomes = genomes;
        }

        public List<Genome> Fittest(int absolute)
        {
            return this.Genomes.OrderByDescending(genome => genome.Fitness).ToList().GetRange(0, absolute);
        }

        public List<Genome> Fittest(float percentage)
        {
            return Fittest(Mathf.FloorToInt(percentage * this.Genomes.Count));
        }

        public List<Genome> Breed(float surviving, float mutation, float perpetuation)
        {
            List<Genome> parents = Fittest(surviving);
            List<Genome> children = new List<Genome>();
            Genome fittest = parents[0];

            //k-Means

            // Let the fittest survive.
            children.Add(fittest);
            children.Add(fittest.Mutate(mutation, perpetuation));

            // Breed babies with parents dna.
            List<Genome> babies;
            babies = Genome.CrossBreed(parents);

            foreach(Genome baby in babies)
                children.Add(baby.Mutate(mutation, perpetuation));

            // Fill remaining slots with random children.
            int growth = Mathf.FloorToInt(this.Genomes.Count * this.Growth);
            int remaining = this.Genomes.Count + growth - children.Count;

            for(int i = 0; i < remaining; ++i)
                children.Add(Genome.Random(fittest.Weights.Count));

            return children;
        }
    }
}
