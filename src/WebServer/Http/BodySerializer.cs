﻿using Devkoes.Restup.WebServer.Http.RequestFactory;
using Devkoes.Restup.WebServer.Models.Schemas;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Devkoes.Restup.WebServer.Http
{
    internal class BodySerializer
    {
        internal object FromBody(string body, MediaType bodyMediaType, Type bodyType)
        {
            if (bodyMediaType == MediaType.JSON)
            {
                return JsonConvert.DeserializeObject(body, bodyType);
            }
            else if (bodyMediaType == MediaType.XML)
            {
                return XmlDeserializeObject(body, bodyType);
            }

            throw new NotImplementedException();
        }

        internal string ToBody(object bodyObject, HttpRequest req)
        {
            if (bodyObject == null)
            {
                return null;
            }

            if (req.ResponseContentType == MediaType.JSON)
            {
                return JsonConvert.SerializeObject(bodyObject);
            }
            else if (req.ResponseContentType == MediaType.XML)
            {
                return XmlSerializeObject(bodyObject);
            }

            return null;
        }

        private static string XmlSerializeObject(object toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private static object XmlDeserializeObject(string body, Type toType)
        {
            var serializer = new XmlSerializer(toType);
            object result;

            using (TextReader reader = new StringReader(body))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
