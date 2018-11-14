using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN.Gym
{
    public class Zoo
    {
        public GameObject Prefab { get; set; }
        public List<Agent> Agents { get; set; }
        public Species Species { get; set; }

        public Zoo()
        {
            this.Prefab = null;
            this.Agents = null;
            this.Species = null;
        }

        public Zoo(GameObject prefab, Species species, NeuralNetwork network) : this()
        {
            this.Prefab = prefab;
            this.Species = species;
            this.Agents = new List<Agent>();

            for(int i = 0; i < this.Species.Population.Size; ++i)
            {
                Agent agent = GameObject.Instantiate<GameObject>(Prefab).GetComponent<Agent>();
                agent.Network = network.Copy();
                agent.Genome = this.Species.GenomeAt(i);

                this.Agents.Add(agent);
            }
        }

        public void Next()
        {
            foreach(Agent agent in this.Agents)
                agent.Finish();

            Debug.Log("Fittest for Generation: " + this.Species.Generation + " is " + this.Species.Population.Fittest(1)[0].Fitness);

            this.Species.Evolve();

            for(int i = 0; i < this.Species.Population.Size; ++i)
                this.Agents[i].Genome = this.Species.GenomeAt(i);
        }
    }
}
