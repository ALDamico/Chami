using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using ChamiUI.BusinessLayer.Converters;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Reads environments from a JSON file.
    /// </summary>
    public class EnvironmentJsonReader : IEnvironmentReader<EnvironmentViewModel>
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentJsonReader"/> object and initializes its internal stream.
        /// </summary>
        /// <param name="inputFile"></param>
        public EnvironmentJsonReader(string inputFile)
        {
            _stream = File.Open(inputFile, FileMode.Open);
        }

        private readonly Stream _stream;

        /// <summary>
        /// Reads a JSON file containing a single environment using the <see cref="EnvironmentViewModelJsonConverter"/>.
        /// </summary>
        /// <returns>An <see cref="EnvironmentViewModel"/> constructed using the information read in the input file.</returns>
        /// <exception cref="NullReferenceException">If the private stream is null, an exception is thrown.</exception>
        /// <seealso cref="EnvironmentViewModelJsonConverter"/>
        public EnvironmentViewModel Process()
        {
            if (_stream == null)
            {
                throw new InvalidDataException("The input stream was null!");
            }

            ResetStream();
            var streamReader = new StreamReader(_stream).ReadToEnd();

            var environment =
                JsonConvert.DeserializeObject<EnvironmentViewModel>(streamReader,
                    new EnvironmentViewModelJsonConverter());
            return environment;
        }

        /// <summary>
        /// Reads a JSON file containing an array of environments using the <see cref="EnvironmentViewModelJsonConverter"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="EnvironmentViewModel"/>s.</returns>
        /// <exception cref="NullReferenceException">If the private stream is null, an exception is thrown.</exception>
        public ICollection<EnvironmentViewModel> ProcessMultiple()
        {
            if (_stream == null)
            {
                throw new InvalidDataException("The input stream was null!");
            }

            ResetStream();
            var jsonText = new StreamReader(_stream).ReadToEnd();

            var environments =
                JsonConvert.DeserializeObject<IEnumerable<EnvironmentViewModel>>(jsonText,
                    new EnvironmentViewModelJsonConverter());
            return environments as List<EnvironmentViewModel>;
        }

        private void ResetStream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
        }
    }
}