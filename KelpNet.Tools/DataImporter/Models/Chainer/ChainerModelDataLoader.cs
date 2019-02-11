﻿using System;

namespace KelpNet.Tools.DataImporter.Models.Chainer
{
    public class ChainerModelDataLoader<T> where T : unmanaged, IComparable<T>
    {
        public static void ModelLoad(string path, FunctionStack<T> model)
        {
            var modelData = new NpzDictionary(path);

            foreach (var function in model.Functions)
            {
                SetParams(function, modelData);
            }
        }

        static void SetParams(Function<T> func, NpzDictionary modelData)
        {
            if (func is Linear<T>)
            {
                Linear<T> linear = (Linear<T>)func;

                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/W.npy"]), linear.Weight.Data, linear.Weight.Data.Length);

                if (!linear.NoBias)
                {
                    Array.Copy(Real<T>.GetArray(modelData[func.Name + "/b.npy"]), linear.Bias.Data, linear.Bias.Data.Length);
                }
            }
            else if (func is Convolution2D<T>)
            {
                Convolution2D<T> conv2D = (Convolution2D<T>)func;

                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/W.npy"]), conv2D.Weight.Data, conv2D.Weight.Data.Length);

                if (!conv2D.NoBias)
                {
                    Array.Copy(Real<T>.GetArray(modelData[func.Name + "/b.npy"]), conv2D.Bias.Data, conv2D.Bias.Data.Length);
                }
            }
            else if (func is Deconvolution2D<T>)
            {
                Deconvolution2D<T> deconv2D = (Deconvolution2D<T>)func;

                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/W.npy"]), deconv2D.Weight.Data, deconv2D.Weight.Data.Length);

                if (!deconv2D.NoBias)
                {
                    Array.Copy(Real<T>.GetArray(modelData[func.Name + "/b.npy"]), deconv2D.Bias.Data, deconv2D.Bias.Data.Length);
                }
            }
            else if (func is EmbedID<T>)
            {
                EmbedID<T> embed = (EmbedID<T>)func;

                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/W.npy"]), embed.Weight.Data, embed.Weight.Data.Length);
            }
            else if (func is BatchNormalization<T>)
            {
                BatchNormalization<T> bn = (BatchNormalization<T>)func;

                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/beta.npy"]), bn.Beta.Data, bn.Beta.Data.Length);
                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/gamma.npy"]), bn.Gamma.Data, bn.Gamma.Data.Length);

                if (bn.IsTrain)
                {
                    if (modelData.ContainsKey(func.Name + "/avg_mean.npy")) Array.Copy(Real<T>.GetArray(modelData[func.Name + "/avg_mean.npy"]), bn.AvgMean.Data, bn.AvgMean.Data.Length);
                    if (modelData.ContainsKey(func.Name + "/avg_var.npy")) Array.Copy(Real<T>.GetArray(modelData[func.Name + "/avg_var.npy"]), bn.AvgVar.Data, bn.AvgVar.Data.Length);
                }
            }
            else if (func is MultiplyScale<T>)
            {
                MultiplyScale<T> scale = (MultiplyScale<T>)func;

                Array.Copy(Real<T>.GetArray(modelData[func.Name + "/W.npy"]), scale.Weight.Data, scale.Weight.Data.Length);

                if (scale.BiasTerm)
                {
                    Array.Copy(Real<T>.GetArray(modelData[func.Name + "/bias/b.npy"]), scale.Bias.Data, scale.Bias.Data.Length);
                }

            }
        }
    }
}