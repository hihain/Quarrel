using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace Refit
{
    interface IRestService
    {
        T For<T>(HttpClient client);
    }

    public static class RestService
    {
        public static T For<T>(HttpClient client, RefitSettings settings)
        {
            var className = "AutoGenerated" + typeof(T).Name;
            var requestBuilder = RequestBuilder.ForType<T>(settings);
            var typeName = typeof(T).AssemblyQualifiedName.Replace(typeof(T).Name, className);
            var generatedType = Type.GetType(typeName);

            if(generatedType == null) { 
                var message = typeof(T).Name + " doesn't look like a Refit interface. Make sure it has at least one " + 
                    "method with a Refit HTTP method attribute and Refit is installed in the project.";

                throw new InvalidOperationException(message);
            }

            return (T)Activator.CreateInstance(generatedType, client, requestBuilder);
        }

        public static T For<T>(HttpClient client) => For<T>(client, null);

        public static T For<T>(string hostUrl, RefitSettings settings)
        {
            // check to see if user provided custom auth token
            HttpMessageHandler innerHandler = null;
            if (settings != null) {
                if (settings.HttpMessageHandlerFactory != null) {
                    innerHandler = settings.HttpMessageHandlerFactory();
                }

                if (settings.AuthorizationHeaderValueGetter != null) {
                    innerHandler = new AuthenticatedHttpClientHandler(settings.AuthorizationHeaderValueGetter, innerHandler);
                }
            }

            var client = new HttpClient(innerHandler ?? new HttpClientHandler()) { BaseAddress = new Uri(hostUrl) };
            return For<T>(client, settings);
        }

        public static T For<T>(string hostUrl) => For<T>(hostUrl, null);
    }
}
