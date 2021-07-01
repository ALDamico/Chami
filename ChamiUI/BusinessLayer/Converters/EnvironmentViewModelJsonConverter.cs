using System;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Custom <see cref="JsonConverter"/> for converting <see cref="Environment"/>s and <see cref="EnvironmentViewModel"/>s to JSON.
    /// </summary>
    public class EnvironmentViewModelJsonConverter : JsonConverter
    {
        public override bool CanWrite => true;

        /// <summary>
        /// Custom logic for writing an <see cref="EnvironmentViewModel"/> to JSON.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter"/>.</param>
        /// <param name="value">The <see cref="EnvironmentViewModel"/> to write to JSON.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to use.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var viewModel = value as EnvironmentViewModel;

            if (viewModel == null)
            {
                return;
            }

            JObject jObjectToWrite = new JObject {{"name", viewModel.Name}};
            var environmentVariablesObject = new JObject();
            jObjectToWrite.Add("environmentVariables", environmentVariablesObject);
            foreach (var environmentVariable in viewModel.EnvironmentVariables)
            {
                environmentVariablesObject.Add(environmentVariable.Name, environmentVariable.Value);
            }

            jObjectToWrite.WriteTo(writer);
        }

        /// <summary>
        /// Custom logic for reading a JSON object and converting it to an <see cref="EnvironmentViewModel"/>.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader"/>.</param>
        /// <param name="objectType">The <see cref="Type"/> to convert from.</param>
        /// <param name="existingValue">Unused.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to use.</param>
        /// <returns>A converter <see cref="EnvironmentViewModel"/> object.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
            {
                throw new NotSupportedException($"The converter cannot handle type {objectType}!");
            }
            
            JObject jObject = JObject.Load(reader);
            var viewModel = new EnvironmentViewModel
            {
                Name = jObject.GetValue("name", StringComparison.InvariantCultureIgnoreCase).ToString()
            };
            if (jObject.GetValue("environmentvariables", StringComparison.InvariantCultureIgnoreCase) is JObject
                environmentVariablesJObject)
            {
                foreach (var environmentVariable in environmentVariablesJObject)
                {
                    var environmentVariableViewModel = new EnvironmentVariableViewModel
                    {
                        Environment = viewModel,
                        Name = environmentVariable.Key,
                        Value = environmentVariable.Value.ToString()
                    };
                    viewModel.EnvironmentVariables.Add(environmentVariableViewModel);
                }
            }

            return viewModel;
        }

        /// <summary>
        /// Determines if the <see cref="Type"/> can be serialized or deserialized using this <see cref="JsonConverter"/>.
        /// </summary>
        /// <param name="objectType">The <see cref="Type"/> of the object to serialize.</param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EnvironmentViewModel) ||
                   objectType == typeof(Environment);
        }

        public override bool CanRead => true;
    }
}