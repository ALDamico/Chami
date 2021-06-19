using System;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentViewModelJsonConverter:JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
           // JToken rootObject = JToken.Load(reader);
            JObject jObject = JObject.Load(reader);
            var viewModel = new EnvironmentViewModel();
            viewModel.Name = jObject.GetValue("name", StringComparison.InvariantCultureIgnoreCase).ToString();
            var environmentVariablesJObject =
                jObject.GetValue("environmentvariables", StringComparison.InvariantCultureIgnoreCase) as JObject;
            if (environmentVariablesJObject != null)
            {
                bool isNameValuePair = true;
                //var isNameValuePair = IsNameValuePair(environmentVariablesJObject[0])
                foreach (var environmentVariable in environmentVariablesJObject)
                {
                    if (isNameValuePair)
                    {
                        var environmentVariableViewModel = new EnvironmentVariableViewModel();
                        environmentVariableViewModel.Environment = viewModel;
                        environmentVariableViewModel.Name = environmentVariable.Key;
                        environmentVariableViewModel.Value = environmentVariable.Value.ToString();
                        viewModel.EnvironmentVariables.Add(environmentVariableViewModel);
                    }
                    else
                    {
                        
                    }
                }
            }
            
            
            return viewModel;
        }

        private bool IsNameValuePair(JObject jObject)
        {
            if (jObject.GetValue("name") != null && jObject.GetValue("value", StringComparison.InvariantCultureIgnoreCase) != null)
            {
                return true;
            }

            return false;
        }

        public EnvironmentViewModel ReadJsonAsEnvironmentViewModel(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadJson(reader, objectType, existingValue, serializer) as EnvironmentViewModel;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EnvironmentViewModel) ||
                   objectType == typeof(ChamiUI.DataLayer.Entities.Environment);
        }

        public override bool CanRead => true;
    }
}