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
                return EntityDictionary[identifier];
            }
            catch (NullReferenceException e)
            {
                
            }

            var converted = ConvertFromModel(model);
            var convertedIdentifier = model?.GetType().GetProperty(EntityIdentifierProperty).GetValue(converted);
            EntityDictionary[convertedIdentifier] = converted;
            ModelDictionary[identifier] = model;
            return converted;
        }

        public TTo To(TFrom entity)
        {
            object identifier = entity?.GetType().GetProperty(EntityIdentifierProperty).GetValue(entity);
            try
            {
                return ModelDictionary[identifier];
            }
            catch (NullReferenceException e)
            {
                
            }

            var converted = ConvertFromEntity(entity);
            var convertedIdentifier = entity?.GetType().GetProperty(EntityIdentifierProperty).GetValue(converted);
            ModelDictionary[convertedIdentifier] = converted;
            EntityDictionary[identifier] = entity;
            return converted;
        }

        public abstract TTo ConvertFromEntity(TFrom entity);
        public abstract TFrom ConvertFromModel(TTo model);
        public string ModelIdentifierProperty { get; protected set; }
        public string EntityIdentifierProperty { get; protected set; }

        private static readonly Dictionary<object, TFrom> EntityDictionary = new();
        private static readonly Dictionary<object, TTo> ModelDictionary = new();
    }
}