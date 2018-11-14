using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN
{
    public class Species
    {
        public int Generation { get; private set; }
        public Population Population { get; private set; }
        public float SurvivingRate { get; set; }
        public float MutationRate { get; set; }
        public float PerpetuationRate { get; set; }

        public static Species Random(int weights, int population, float survivingRate = 0.25f, float mutationRate = 0.1f, float perpetuationRate = 0.4f)
        {
            List<Genome> genomes = new List<Genome>();

            for(int i = 0; i < population; ++i)
                genomes.Add(Genome.Random(weights));

            return new Species(genomes, survivingRate, mutationRate, perpetuationRate);
        }

        public Species()
        {
            this.Generation = 0;
            this.Population = null;
            this.SurvivingRate = 0f;
            this.MutationRate = 0f;
            this.PerpetuationRate = 0f;
        }

        public Species(List<Genome> genomes, float survivingRate = 0.25f, float mutationRate = 0.1f, float perpetuationRate = 0.4f) : this()
        {
            this.Population = new Population(genomes);
            this.SurvivingRate = survivingRate;
            this.MutationRate = mutationRate;
            this.PerpetuationRate = perpetuationRate;
        }

        public void Evolve()
        {
            List<Genome> genomes = this.Population.Breed(this.SurvivingRate, this.MutationRate, this.PerpetuationRate);
            this.Population = new Population(genomes);
            ++this.Generation;
        }

        public Genome GenomeAt(int index)
        {
            return this.Population.Genomes[index];
        }
    }
}
