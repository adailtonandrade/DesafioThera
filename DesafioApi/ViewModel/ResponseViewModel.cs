using Newtonsoft.Json;
using System.Collections.Generic;

namespace DesafioApi.ViewModel
{
    public class ResponseViewModel<T> where T : class
    {
        [JsonProperty(PropertyName = "Mensagem")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "Conteúdo")]
        public T Content { get; set; }
        [JsonProperty(PropertyName = "Erros")]
        public List<string> Errors { get; set; }
    }
}