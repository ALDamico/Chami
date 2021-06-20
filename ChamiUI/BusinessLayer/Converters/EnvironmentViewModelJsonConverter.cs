using System;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentViewModelJsonConverter : JsonConverter
    {
        public override bool CanWrite => true;

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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
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

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EnvironmentViewModel) ||
                   objectType == typeof(ChamiUI.DataLayer.Entities.Environment);
        }

        public override bool CanRead => true;
    }
}