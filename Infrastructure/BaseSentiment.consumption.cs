﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace Infrastructure
{
    public partial class BaseSentiment
    {
        /// <summary>
        /// model input class for BaseSentiment.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"description")]
            public string Description { get; set; }

            [ColumnName(@"ratings")]
            public float Ratings { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for BaseSentiment.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"description")]
            public float[] Description { get; set; }

            [ColumnName(@"ratings")]
            public uint Ratings { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"PredictedLabel")]
            public float PredictedLabel { get; set; }

            [ColumnName(@"Score")]
            public float[] Score { get; set; }

        }

        #endregion

        private static string MLNetModelPath = "wwwroot/BaseSentiment.zip";

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}