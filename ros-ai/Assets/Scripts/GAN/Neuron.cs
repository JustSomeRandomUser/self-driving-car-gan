using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN
{
    public class Neuron
    {
        public List<float> Weights { get; set; }
        public float Bias
        {
            get { return this.Weights[this.Weights.Count - 1]; }
            set { this.Weights[this.Weights.Count - 1] = value; }
        }

        public static float Tanh(float value)
        {
            return (float)Math.Tanh(value);
        }

        public static float RandomClamped()
        {
            return UnityEngine.Random.Range(-1f, 1f);
        }

        public Neuron()
        {
            this.Weights = null;
        }

        public Neuron(List<float> weights) : this()
        {
            this.Weights = weights;
        }

        public Neuron(int inputs) : this()
        {
            this.Weights = new List<float>();

            for(int i = 0; i < inputs; ++i)
                this.Weights.Add(RandomClamped());

            // Add Bias
            this.Weights.Add(RandomClamped());
        }

        public float Activate(List<float> input)
        {
            if((this.Weights.Count - 1) != input.Count)
                throw new System.ArgumentException(String.Format("{0} is different than {1}.", this.Weights.Count, input.Count), "input");

            float stimulation = 0f;

            for(int i = 0; i < (this.Weights.Count - 1); ++i)
                stimulation += input[i] * this.Weights[i];

            return Tanh(stimulation - this.Bias);
        }
    }
}
