using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN.Gym
{
    public class Agent : MonoBehaviour
    {
        public NeuralNetwork Network { get; set; }
        public Genome Genome
        {
            get { return this.genome; }
            set
            {
                this.genome = value;
                this.Network.Weights = this.genome.Weights;
            }
        }

        public float Fitness { get; set; }
        public bool Alive { get; set; }

        private Genome genome;

        public void Awake()
        {
            Reset();
        }

        public void Update()
        {
        }

        public void Reset()
        {
            this.Fitness = 0f;
            this.Alive = true;
        }

        public void Finish()
        {
            this.Genome.Fitness = this.Fitness;
        }

        public void Reward(float reward)
        {
            this.Fitness += reward;
        }

        public void Punish(float punish)
        {
            this.Fitness -= punish;
        }

        public List<float> Evaluate(List<float> inputs)
        {
            return this.Network.Evaluate(inputs);
        }
    }
}
