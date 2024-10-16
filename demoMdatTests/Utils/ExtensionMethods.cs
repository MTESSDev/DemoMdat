using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DemoAPI.SV.Tests.Utils;

public static class ExtensionMethods
{
    public static async Task<object?> GetAndDeserialize<TReturn>(this HttpClient client, string requestUri)
    {
        HttpResponseMessage res = await client.GetAsync(requestUri);

        if (res.IsSuccessStatusCode)
        {
            if(typeof(TReturn) == typeof(byte[]))
            {
                if (res.Content.Headers.ContentEncoding.Contains("br"))
                {
                    using (var compressedStream = await res.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new MemoryStream())
                    {
                        using (var brotliStream = new BrotliStream(compressedStream, CompressionMode.Decompress))
                        {
                            await brotliStream.CopyToAsync(decompressedStream);
                        }

                        return new Retour<byte[]> { StatusCode = res.StatusCode, Content = decompressedStream.ToArray(), Headers = FusionnerHeaders(res.Headers, res.Content.Headers) };
                    }
                }
                else
                {
                    return new Retour<byte[]> { StatusCode = res.StatusCode, Content = await res.Content.ReadAsByteArrayAsync(), Headers = FusionnerHeaders(res.Headers, res.Content.Headers) };
                }
            }
            else
            {
                return new Retour<TReturn> { StatusCode = res.StatusCode, Content = JsonConvert.DeserializeObject<TReturn>(await res.Content.ReadAsStringAsync()) };
            }
        }
        else
        {
            string msg = await res.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<object?>(msg);
            return new Retour<object?> { StatusCode = res.StatusCode, Content = content };
        }
    }

    public static async Task<object?> PostAndDeserialize<TSend, TReturn>(this HttpClient client, TSend send, string requestUri)
    {
        var contentEnvoie = JsonContent.Create(send);

        HttpResponseMessage res = await client.PostAsync(requestUri, contentEnvoie);

        if (res.IsSuccessStatusCode && res.StatusCode != HttpStatusCode.Created && typeof(TReturn) == typeof(string))
        {
            return new Retour<string> { StatusCode = res.StatusCode, Content = await res.Content.ReadAsStringAsync() };
        }
        else if(res.IsSuccessStatusCode && res.StatusCode != HttpStatusCode.Created)
        {
            return new Retour<TReturn> { StatusCode = res.StatusCode, Content = await res.Content.ReadFromJsonAsync<TReturn>() };
        }
        else
        {
            string msg = await res.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<object?>(msg);
            return new Retour<object?> { StatusCode = res.StatusCode, Content = content, Headers = FusionnerHeaders(res.Headers, res.Content.Headers) };
        }
    }

	public static async Task<object?> PostAndDeserializeMultipartFormData<TSend, TReturn>(this HttpClient client, TSend send, string requestUri)
	{
        using (var contentEnvoie = new MultipartFormDataContent())
        {
            foreach (var property in typeof(TSend).GetProperties())
            {
                var value = property.GetValue(send);

                if (value != null)
                {
                    if (value is IFormFile formFile)
                    {
                        var fileContent = new StreamContent(formFile.OpenReadStream());
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(formFile.ContentType);
						contentEnvoie.Add(fileContent, property.Name, formFile.FileName);
                    }
                    else
                    {
						contentEnvoie.Add(new StringContent(value.ToString()), property.Name);
                    }
                }
            }

			HttpResponseMessage res = await client.PostAsync(requestUri, contentEnvoie);

			if (res.IsSuccessStatusCode && res.StatusCode != HttpStatusCode.Created && typeof(TReturn) == typeof(string))
			{
				return new Retour<string> { StatusCode = res.StatusCode, Content = await res.Content.ReadAsStringAsync() };
			}
			else if (res.IsSuccessStatusCode && res.StatusCode != HttpStatusCode.Created)
			{
				return new Retour<TReturn> { StatusCode = res.StatusCode, Content = await res.Content.ReadFromJsonAsync<TReturn>() };
			}
			else
			{
				string msg = await res.Content.ReadAsStringAsync();
                object? content;

                try
                {
					content = JsonConvert.DeserializeObject<object?>(msg);
                }
                catch {
                    content = msg;
                }

				return new Retour<object?> { StatusCode = res.StatusCode, Content = content, Headers = FusionnerHeaders(res.Headers, res.Content.Headers) };
			}
		}
	}

	private static Dictionary<string, IEnumerable<string>> FusionnerHeaders(HttpHeaders headers, HttpHeaders autreHeaders)
    {
        Dictionary<string, IEnumerable<string>> dictionnaireHeaders = headers.ToDictionary(h => h.Key, h => h.Value);
        Dictionary<string, IEnumerable<string>> dictionnaireAutreHeaders = autreHeaders.ToDictionary(h => h.Key, h => h.Value);

        dictionnaireAutreHeaders.ToList().ForEach
        (
            pair =>
            {
                dictionnaireHeaders.TryAdd(pair.Key, pair.Value);
            }
        );

        return dictionnaireHeaders;
    }
}

public class Retour<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public T? Content { get; set; }
    public Dictionary<string, IEnumerable<string>> Headers { get; set; }
}