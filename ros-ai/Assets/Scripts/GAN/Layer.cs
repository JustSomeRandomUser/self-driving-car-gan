using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public List<float> Weights
        {
            get
            {
                List<float> weights = new List<float>();

                foreach(Neuron neuron in this.Neurons)
                    foreach(float weight in neuron.Weights)
                        weights.Add(weight);

                return weights;
            }

            set
            {
                int index = 0;

                foreach(Neuron neuron in this.Neurons)
                {
                    neuron.Weights = value.GetRange(index, neuron.Weights.Count);
                    index += neuron.Weights.Count;
                }
            }
        }

        public Layer()
        {
            this.Neurons = null;
        }

        public Layer(List<Neuron> neurons) : this()
        {
            this.Neurons = neurons;
        }

        public Layer(int neurons, int inputs) : this()
        {
            this.Neurons = new List<Neuron>();

            for(int n = 0; n < neurons; ++n)
                this.Neurons.Add(new Neuron(inputs));
        }

        public List<float> Evaluate(List<float> input)
        {
            List<float> output = new List<float>();

            foreach(Neuron neuron in this.Neurons)
                output.Add(neuron.Activate(input));

            return output;
        }
    }
}
