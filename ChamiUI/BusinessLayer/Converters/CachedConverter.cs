using System;
using System.Collections.Generic;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    public abstract class CachedConverter<TFrom, TTo> : IConverter<TFrom, TTo> where TFrom: IChamiEntity
    {
        public TFrom From(TTo model)
        {
            object identifier = model?.GetType().GetProperty(ModelIdentifierProperty).GetValue(model);
            try
            {
                return _entityDictionary[identifier];
            }
            catch (NullReferenceException e)
            {
                
            }

            var converted = ConvertFromModel(model);
            var convertedIdentifier = model?.GetType().GetProperty(EntityIdentifierProperty).GetValue(converted);
            _entityDictionary[convertedIdentifier] = converted;
            _modelDictionary[identifier] = model;
            return converted;
        }

        public TTo To(TFrom entity)
        {
            throw new System.NotImplementedException();
        }

        public Func<TFrom, TTo> ConvertFromEntity { get; protected set; }
        public Func<TTo, TFrom> ConvertFromModel { get; protected set; }
        public string ModelIdentifierProperty { get; protected set; }
        public string EntityIdentifierProperty { get; protected set; }

        private static Dictionary<object, TFrom> _entityDictionary = new();
        private static Dictionary<object, TTo> _modelDictionary = new();
    }
}