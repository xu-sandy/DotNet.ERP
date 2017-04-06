using Newtonsoft.Json;
using Pharos.Logic.ApiData.Pos.Sale.Barcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Pharos.Logic.ApiData.Pos.Sale.Suspend
{
    //public class BarcodeJsonConverter : JsonConverter
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer,
                                       object value,
                                       JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    /// <summary>
    /// 接口反序列化
    /// </summary>
    public class BarcodeConverter : JsonCreationConverter<IBarcode>
    {
        public BarcodeConverter() { }
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IIdentification)||objectType == typeof(IBarcode));
        }
        protected override IBarcode Create(Type objectType, JObject jObject)
        {
            var barcodeType = (BarcodeType)jObject["BarcodeType"].ToObject(typeof(BarcodeType));
            IBarcode barcode;
            switch (barcodeType)
            {
                case BarcodeType.BundlingBarcode:
                    barcode = new BundlingBarcode();
                    break;
                case BarcodeType.CustomBarcode:
                    barcode = new CustomBarcode();
                    break;
                case BarcodeType.StandardBarcode:
                    barcode = new StandardBarcode();
                    break;
                case BarcodeType.WeighBarcode:
                    barcode = new WeighBarcode();
                    break;
                default:
                    barcode = new StandardBarcode();
                    break;
            }
            return barcode;
        }
    }

    public class TypeNameSerializationBinder : SerializationBinder
    {
        public string TypeFormat { get; private set; }

        public TypeNameSerializationBinder(string typeFormat)
        {
            TypeFormat = typeFormat;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            string resolvedTypeName = string.Format(TypeFormat, typeName);

            return Type.GetType(resolvedTypeName, true);
        }
    }
}
